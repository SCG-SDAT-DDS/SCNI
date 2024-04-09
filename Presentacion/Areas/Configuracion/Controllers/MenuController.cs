using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Sistema.Extensiones;
using Datos;
using Datos.Recursos;
using Datos.Enums;
using Datos.Repositorios.Configuracion;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios;
using Presentacion.Controllers;

namespace Presentacion.Areas.Configuracion.Controllers
{
    public class MenuController : ControladorBase
    {
        private MenuViewModel _menuViewModel;
        private MenuRepositorio _menuRepositorio;

        [HttpGet]
        public ActionResult Index(string idRol, string idUsuario, string idMenuPadre, string habilitado = "on")
        {
            try
            {
                var idGestionMenu = 0;
                var tipoGestion = (Enumerados.GestionMenu)0;
                var nombreTipoGestion = string.Empty;
                var buscar = string.Empty;
                var habilitadoBuscar = habilitado.TryToBool();
                var idRolBuscar = idRol.DecodeFrom64().TryToInt();
                var idUsuarioBuscar = idUsuario.DecodeFrom64().TryToInt();
                var idMenuPadreBuscar = idMenuPadre.DecodeFrom64().TryToInt();

                try
                {
                    tipoGestion = ObtenerDatoTemporal<Enumerados.GestionMenu>(Enumerados.TempData.TipoGestion);
                    idGestionMenu = ObtenerDatoTemporal<int>(Enumerados.TempData.IdGestionarMenu);
                    nombreTipoGestion = ObtenerDatoTemporal<string>(Enumerados.TempData.NombreTipoGestion);
                }
                catch (Exception ex)
                {
                    var mensaje = ex.Message;
                    idGestionMenu = 0;
                }
                finally
                {
                    _menuViewModel = new MenuViewModel(idRolBuscar, idUsuarioBuscar, idMenuPadreBuscar, buscar,
                    habilitadoBuscar, idGestionMenu, tipoGestion, nombreTipoGestion)
                    {
                        IdPadre = idMenuPadreBuscar
                    };

                    if (idRolBuscar > 0 || idUsuarioBuscar > 0 || _menuViewModel.IdGestionarMenu >= 0)
                    {
                        GuardarDatoTemporal(Enumerados.TempData.TipoGestion, _menuViewModel.TipoGestion, true);
                        GuardarDatoTemporal(Enumerados.TempData.IdGestionarMenu, _menuViewModel.IdGestionarMenu, true);
                        GuardarDatoTemporal(Enumerados.TempData.NombreTipoGestion, _menuViewModel.NombreTipoGestion, true);
                    }
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(_menuViewModel);
        }

        [HttpPost]
        public ActionResult Index(string cbxHabilitados, string txtOpcion)
        {
            try
            {
                var idGestionMenu = 0;
                var tipoGestion = (Enumerados.GestionMenu)0;
                var nombreTipoGestion = string.Empty;
                var habilitadoBuscar = cbxHabilitados.TryToBool();
                var idRol = ObtenerParametroGetEnInt(Enumerados.Parametro.IdRol);
                var idUsuario = ObtenerParametroGetEnInt(Enumerados.Parametro.IdUsuario);
                var idMenuPadre = ObtenerParametroGetEnInt(Enumerados.Parametro.IdMenuPadre);

                try
                {
                    tipoGestion = ObtenerDatoTemporal<Enumerados.GestionMenu>(Enumerados.TempData.TipoGestion, true);
                    idGestionMenu = ObtenerDatoTemporal<int>(Enumerados.TempData.IdGestionarMenu, true);
                    nombreTipoGestion = ObtenerDatoTemporal<string>(Enumerados.TempData.NombreTipoGestion);
                }
                catch (Exception ex)
                {
                    var mensaje = ex.Message;
                    idGestionMenu = 0;
                }
                finally
                {
                    _menuViewModel = new MenuViewModel(idRol, idUsuario, idMenuPadre, txtOpcion, habilitadoBuscar,
                        idGestionMenu, tipoGestion, nombreTipoGestion);
                    ViewBag.TipoGestion = _menuViewModel.TipoGestion;
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }

            return View(_menuViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idMenu, string idMenuPadre)
        {
            var menu = new Menu();
            var idMenuGuardar = idMenu.DecodeFrom64().TryToInt();
            var idMenuPadreGuardar = idMenuPadre.DecodeFrom64().TryToInt();
            try
            {
                using (var bd = new Contexto())
                {
                    _menuRepositorio = new MenuRepositorio(bd);
                    if (idMenuGuardar != 0) menu = _menuRepositorio.BuscarPorId(idMenuGuardar);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(menu);
        }

        [HttpPost]
        public ActionResult Guardar(Menu menu)
        {
            var correcto = false;
            var idMenu = ObtenerParametroGetEnInt(Enumerados.Parametro.IdMenu);
            var idMenuPadre = ObtenerParametroGetEnInt(Enumerados.Parametro.IdMenuPadre);

            if (ModelState["IDMenu"] != null) ModelState["IDMenu"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        //temporarl cambiarlo habilitando la validaciones
                        bd.Configuration.ValidateOnSaveEnabled = false;
                        _menuRepositorio = new MenuRepositorio(bd);
                        menu.Habilitado = true;
                        menu.IDMenu = idMenu;
                        if (idMenuPadre > 0 || idMenu == 0)
                        {
                            menu.Padre = idMenuPadre;
                            menu.Camino = string.Empty;
                            correcto = _menuRepositorio.Guardar(menu);
                            GuardarMovimiento("Creación", "Menu", menu.IDMenu);
                        }
                        else
                        {
                            correcto = _menuRepositorio.Modificar(menu);
                            GuardarMovimiento("Modificación", "Menu", menu.IDMenu);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Menu), correcto);
            ModelState.Clear();
            return View(new Menu());
        }

        [HttpGet]
        public ActionResult Habilitar(string idMenu)
        {
            var idPadre = ObtenerParametroGetEnInt(Enumerados.Parametro.IdMenuPadre);
            var IdMenu = idMenu.DecodeFrom64().TryToInt();
            try
            {
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _menuRepositorio = new MenuRepositorio(bd);
                    _menuRepositorio.CambiarHabilitado(IdMenu, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Menu", IdMenu);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Menu));
            }

            return RedirectToAction("Index", new { idMenuPadre = idPadre });
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idMenu)
        {
            var idPadre = ObtenerParametroGetEnInt(Enumerados.Parametro.IdMenuPadre);
            var IdMenu = idMenu.DecodeFrom64().TryToInt();
            

            try
            {
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _menuRepositorio = new MenuRepositorio(bd);
                    _menuRepositorio.CambiarHabilitado(IdMenu, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Menu", IdMenu);
                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                EscribirError(string.Format(General.FalloDeshabilitar, General.Menu));
            }

            return RedirectToAction("Index", new { idMenuPadre = idPadre });
        }

        [HttpGet]
        public ActionResult Agregar(string idMenu)
        {
            var idMenuAgregar = idMenu.DecodeFrom64().TryToInt();
            var idGestionarMenu = ObtenerDatoTemporal<int>(Enumerados.TempData.IdGestionarMenu);
            var tipoGestion = ObtenerDatoTemporal<Enumerados.GestionMenu>(Enumerados.TempData.TipoGestion);
            var idGestion = idGestionarMenu.ToString(CultureInfo.InvariantCulture).EncodeTo64();
            var correcto = false;
            try
            {
                _menuViewModel = new MenuViewModel(tipoGestion);
                _menuViewModel.AgregarOpcionMenu(idGestionarMenu, idMenuAgregar);
                correcto = true;
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            EnviarAlerta(General.AgregadoCorrectamente, General.FalloAgregar, correcto);
            return tipoGestion == Enumerados.GestionMenu.Usuario
                ? RedirectToAction("Index", new { idUsuario = idGestion })
                : RedirectToAction("Index", new { idRol = idGestion });
        }

        [HttpGet]
        public ActionResult Quitar(string idMenu)
        {
            var idMenuAgregar = idMenu.DecodeFrom64().TryToInt();
            var idGestionarMenu = ObtenerDatoTemporal<int>(Enumerados.TempData.IdGestionarMenu);
            var tipoGestion = ObtenerDatoTemporal<Enumerados.GestionMenu>(Enumerados.TempData.TipoGestion);
            var idGestion = idGestionarMenu.ToString(CultureInfo.InvariantCulture).EncodeTo64();
            var correcto = false;
            try
            {
                _menuViewModel = new MenuViewModel(tipoGestion);
                _menuViewModel.QuitarOpcionMenu(idGestionarMenu, idMenuAgregar);
                correcto = true;
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            EnviarAlerta(General.AgregadoCorrectamente, General.FalloAgregar, correcto);
            return tipoGestion == Enumerados.GestionMenu.Usuario
                ? RedirectToAction("Index", new { idUsuario = idGestion })
                : RedirectToAction("Index", new { idRol = idGestion });
        }

        [WebMethod]
        [HttpPost]
        public ActionResult ActivarMenuPersonalizado(string idUsuario, bool activar)
        {
            bool correcto=false;
            var idUsuarioCambiar = idUsuario.DecodeFrom64().TryToInt();
            try
            {
                using (var bd = new Contexto())
                {
                    //temporarl cambiarlo habilitando la validaciones
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    var usuarioRepositorio = new UsuarioRepositorio(bd);
                    correcto = usuarioRepositorio.CambiarTieneMenuPersonalziado(idUsuarioCambiar, activar);
                }
                return Json(new { @guardado = correcto });
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
            }

            return Json(new {@guardado = correcto });
        }

        [WebMethod]
        [HttpPost]
        public ActionResult ComprobarMenuPadre(string idPadre)
        {
            return Json(idPadre.DecodeFrom64().TryToInt() == 0);
        }
    }
}
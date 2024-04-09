using System;
using System.Linq;
using System.Web.Mvc;
using Sistema.Extensiones;
using Datos;
using Datos.Recursos;
using Datos.Enums;
using Datos.Repositorios.Configuracion;
using Datos.DTO.Infraestructura.ViewModels;
using Presentacion.Controllers;

namespace Presentacion.Areas.Configuracion.Controllers
{
    public class RolController : ControladorBase
    {
        private RolRepositorio _rolRepositorio;

        [HttpGet]
        public ActionResult Index()
        {
            var rolViewModel = new RolViewModel();
            try
            {
                using (var bd = new Contexto())
                {
                    _rolRepositorio = new RolRepositorio(bd);
                    rolViewModel.Roles = _rolRepositorio.Buscar(r => r.Habilitado,
                        r => r.OrderBy(x => x.Nombre),
                        r => r.MenuInicio);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(rolViewModel);
        }

        [HttpPost]
        public ActionResult Index(Rol rol)
        {
            var rolViewModel = new RolViewModel();
            try
            {
                if (rol.Nombre == null) rol.Nombre = string.Empty;
                ModelState["rol.Nombre"].Errors.Clear();

                using (var bd = new Contexto())
                {
                    _rolRepositorio = new RolRepositorio(bd);
                    rolViewModel.Roles = _rolRepositorio.Buscar(r => r.Nombre.Contains(rol.Nombre)
                        && r.Habilitado == rol.Habilitado,
                        r => r.OrderBy(x => x.Nombre),
                        r => r.MenuInicio);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(rolViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idRol)
        {
            var rol = new Rol();
            var idRolGuardar = idRol.DecodeFrom64().TryToInt();
            try
            {
                using (var bd = new Contexto())
                {
                    _rolRepositorio = new RolRepositorio(bd);
                    rol = _rolRepositorio.BuscarUnoSolo(r => r.IDRol == idRolGuardar);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            GuardarDatoTemporal(Enumerados.TempData.IdModificar, idRolGuardar, true);
            return View(rol);
        }

        [HttpPost]
        public ActionResult Guardar(Rol rol)
        {
            rol.IDRol = ObtenerParametroGetEnInt(Enumerados.Parametro.IdRol);
            var correcto = false;
            try
            {
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _rolRepositorio = new RolRepositorio(bd);                    
                    if (rol.IDRol > 0)
                    {
                        correcto = _rolRepositorio.Modificar(rol);
                        GuardarMovimiento("Modificación", "Rol", rol.IDRol);
                    }
                    else
                    {
                        rol.FechaCreacion = DateTime.Now;
                        rol.Habilitado = true;
                        _rolRepositorio.Guardar(rol);
                        correcto = bd.SaveChanges() >= 1;
                        GuardarMovimiento("Creación", "Rol", rol.IDRol);
                    }

                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }

            EscribirMensaje(correcto ? General.GuardadoCorrecto : string.Format(General.FalloGuardado, General.Rol));

            ModelState.Clear();

            return correcto ? RedirectToAction("Guardar") : (ActionResult) View(rol);
        }

        [HttpGet]
        public ActionResult Habilitar(string idRol)
        {
            try
            {
                var idRolHabilitar = idRol.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _rolRepositorio = new RolRepositorio(bd);
                    _rolRepositorio.CambiarHabilitado(idRolHabilitar, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Rol", idRolHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idRol)
        {
            try
            {
                var idRolHabilitar = idRol.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _rolRepositorio = new RolRepositorio(bd);
                    _rolRepositorio.CambiarHabilitado(idRolHabilitar, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Rol", idRolHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloDeshabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult VerificarExiste(Rol rol)
        {
            var idModificar = ObtenerDatoTemporal<int>(Enumerados.TempData.IdModificar, true);
            using (var bd = new Contexto())
            {
                _rolRepositorio = new RolRepositorio(bd);
                var existe = _rolRepositorio.Buscar(s => s.Nombre == rol.Nombre && s.IDRol != idModificar).Any();
                return Json(!existe, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
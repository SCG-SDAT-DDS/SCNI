using System;
using System.Linq;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Recursos;
using Datos.Repositorios.Catalogos;
using Presentacion.Controllers;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class PuestoController : ControladorBase
    {
        private PuestoRepostorio _puestoRepostorio;

        [HttpGet]
        public ActionResult Index(PuestoViewModel puestoViewModel)
        {
            try
            {
                puestoViewModel.Puesto = new Puesto { Habilitado = true };
                using (var bd = new Contexto())
                {
                    _puestoRepostorio = new PuestoRepostorio(bd);
                    puestoViewModel.Puestos = _puestoRepostorio.Buscar(puestoViewModel);
                    puestoViewModel.TotalEncontrados = _puestoRepostorio.ObtenerTotalRegistros(puestoViewModel);
                    puestoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(puestoViewModel.TotalEncontrados,
                                                    puestoViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(puestoViewModel);
        }

        [HttpPost]
        public ActionResult Index(PuestoViewModel puestoViewModel, string pagina)
        {
            if (string.IsNullOrEmpty(pagina)) pagina = "1";
            try
            {
                puestoViewModel.PaginaActual = pagina.TryToInt();
                using (var bd = new Contexto())
                {
                    _puestoRepostorio = new PuestoRepostorio(bd);
                    puestoViewModel.Puestos = _puestoRepostorio.Buscar(puestoViewModel);
                    puestoViewModel.TotalEncontrados = _puestoRepostorio.ObtenerTotalRegistros(puestoViewModel);
                    puestoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(puestoViewModel.TotalEncontrados,
                                                    puestoViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(puestoViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idPuesto)
        {
            var idPuestoBuscar = idPuesto.DecodeFrom64().TryToInt();
            var puesto = new Puesto { IDPuesto = idPuestoBuscar };

            if (idPuestoBuscar > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        _puestoRepostorio = new PuestoRepostorio(bd);
                        puesto = _puestoRepostorio.BuscarUnoSolo(p => p.IDPuesto == idPuestoBuscar);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            GuardarDatoTemporal(Enumerados.TempData.IdModificar, idPuestoBuscar, true);
            return View(puesto);
        }

        [HttpPost]
        public ActionResult Guardar(Puesto puesto)
        {
            var correcto = false;
            puesto.IDPuesto = Request.QueryString["idPuesto"].DecodeFrom64().TryToInt();
            if (puesto.IDPuesto > 0)
            {
                ModelState["idPuesto"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    puesto.IDPuesto = ObtenerParametroGetEnInt(Enumerados.Parametro.IdPuesto);
                    puesto.Habilitado = true;
                    using (var bd = new Contexto())
                    {
                        _puestoRepostorio = new PuestoRepostorio(bd);
                        var _movimiento = "Creación";
                        if (puesto.IDPuesto > 0)
                        {
                            _puestoRepostorio.Modificar(puesto);
                            _movimiento = "Modificación";
                        }
                        else
                        {
                            _puestoRepostorio.Guardar(puesto);
                        }
                        correcto = bd.SaveChanges() >= 1;
                        GuardarMovimiento(_movimiento, "Puesto", puesto.IDPuesto);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }

            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Puesto), correcto);
            ModelState.Clear();
            return View(new Puesto());
        }

        [HttpGet]
        public ActionResult Habilitar(string idPuesto)
        {
            try
            {
                var idPuestoHabilitar = idPuesto.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _puestoRepostorio = new PuestoRepostorio(bd);
                    _puestoRepostorio.CambiarHabilitado(idPuestoHabilitar, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Puesto", idPuestoHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Puesto));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idPuesto)
        {
            try
            {
                var idPuestoHabilitar = idPuesto.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _puestoRepostorio = new PuestoRepostorio(bd);
                    _puestoRepostorio.CambiarHabilitado(idPuestoHabilitar, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Puesto", idPuestoHabilitar);
                }
            }
            catch (Exception ex)
            {
                EscribirError(string.Format(General.FalloDeshabilitar, General.Puesto));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(string idPuesto)
        {
            var idPuestoEliminar = idPuesto.DecodeFrom64().TryToInt();
            var correcto = false;
            try
            {
                using (var bd = new Contexto())
                {
                    _puestoRepostorio = new PuestoRepostorio(bd);
                    _puestoRepostorio.Eliminar(new Puesto { IDPuesto = idPuestoEliminar });
                    correcto = bd.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            EnviarAlerta(General.EliminadoCorrecto, string.Format(General.FalloEliminado, General.Puesto), correcto);
            return RedirectToAction("Index");
        }

        public ActionResult VerificarExiste(Puesto puesto)
        {
            var idModificar = ObtenerDatoTemporal<int>(Enumerados.TempData.IdModificar, true);
            using (var bd = new Contexto())
            {
                _puestoRepostorio = new PuestoRepostorio(bd);
                var existe = _puestoRepostorio.Buscar(s => s.Nombre == puesto.Nombre && s.IDPuesto != idModificar).Any();
                return Json(!existe, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

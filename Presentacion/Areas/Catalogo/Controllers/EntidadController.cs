using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Presentacion.Controllers;
using Resources;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class EntidadController : ControladorBase
    {
        private EntidadRepositorio _entidadRepositorio;
           
        // GET: Catalogo/Entidad
        [HttpGet]
        public ActionResult Index(EntidadViewModel entidadViewModel)
        {
            try
            {
                entidadViewModel.Entidad = new Entidad { Habilitado = true };
                using (var bd = new Contexto())
                {
                    _entidadRepositorio = new EntidadRepositorio(bd);
                    entidadViewModel.Entidades = _entidadRepositorio.Buscar(entidadViewModel);
                    entidadViewModel.TotalEncontrados = _entidadRepositorio.ObtenerTotalRegistros(entidadViewModel);
                    entidadViewModel.Paginas = Paginar.ObtenerCantidadPaginas(entidadViewModel.TotalEncontrados, entidadViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {

            }
            return View(entidadViewModel);
        }

        [HttpPost]
        public ActionResult Index(EntidadViewModel entidadViewModel, string pagina)
        {
            entidadViewModel.PaginaActual = pagina.TryToInt();
            if (entidadViewModel.PaginaActual <= 0) entidadViewModel.PaginaActual = 1;

            try
            {
                using (var bd = new Contexto())
                {
                    _entidadRepositorio = new EntidadRepositorio(bd);
                    entidadViewModel.Entidades = _entidadRepositorio.Buscar(entidadViewModel);
                    entidadViewModel.TotalEncontrados = _entidadRepositorio.ObtenerTotalRegistros(entidadViewModel);
                    entidadViewModel.Paginas = Paginar.ObtenerCantidadPaginas(entidadViewModel.TotalEncontrados, entidadViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {

            }
            LimpiarModelState();
            return View(entidadViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idEntidad)
        {
            var entidad = new Entidad { IDEntidad = idEntidad.DecodeFrom64().TryToInt() };

            if (entidad.IDEntidad > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        _entidadRepositorio = new EntidadRepositorio(bd);
                        entidad = _entidadRepositorio.BuscarPorId(entidad.IDEntidad);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            LimpiarModelState();
            ModelState.Clear();
            return View(entidad);
        }

        [HttpPost]
        public ActionResult Guardar(Entidad entidad)
        {

            var correcto = false;
            entidad.IDEntidad = Request.QueryString["idEntidad"].DecodeFrom64().TryToInt();

            if (ModelState.ContainsKey("IDEntidad"))
                ModelState["IDEntidad"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    entidad.Habilitado = true;
                    entidad.IDEntidad = ObtenerParametroGetEnInt(Enumerados.Parametro.IdEntidad);
                    using (var bd = new Contexto())
                    {
                        _entidadRepositorio = new EntidadRepositorio(bd);
                        var _movimiento = "Creación";
                        if (entidad.IDEntidad > 0)
                        {
                            _entidadRepositorio.Modificar(entidad);
                            _movimiento = "Modificación";
                        }
                        else
                        {
                            _entidadRepositorio.Guardar(entidad);
                        }
                        correcto = bd.SaveChanges() >= 1;
                        if (correcto)
                        {
                            GuardarMovimiento(_movimiento, "Entidad", entidad.IDEntidad);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //EscribirError(ex.Message);
                }
            }
            
            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, "Entidad"), correcto);
            LimpiarModelState();
            ModelState.Clear();
            return View(new Entidad());
        }

        [HttpGet]
        public ActionResult Habilitar(string idEntidad)
        {
            try
            {
                var idEntidadHabilitar = idEntidad.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _entidadRepositorio = new EntidadRepositorio(bd);
                    _entidadRepositorio.CambiarHabilitado(idEntidadHabilitar, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Entidad", idEntidadHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Entidad));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idEntidad)
        {
            try
            {
                var idEntidadHabilitar = idEntidad.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _entidadRepositorio = new EntidadRepositorio(bd);
                    _entidadRepositorio.CambiarHabilitado(idEntidadHabilitar, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Entidad", idEntidadHabilitar);
                }
            }
            catch (Exception ex)
            {
                EscribirError(string.Format(General.FalloDeshabilitar, General.Entidad));
            }

            return RedirectToAction("Index");
        }
    }
}
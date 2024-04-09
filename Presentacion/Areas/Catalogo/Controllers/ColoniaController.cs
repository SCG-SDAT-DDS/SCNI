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
    public class ColoniaController : ControladorBase
    {
        private ColoniaRepositorio _coloniaRepositorio;

        // GET: Catalogo/Colonia
        [HttpGet]
        public ActionResult Index(ColoniaViewModel coloniaViewModel)
        {
            try
            {
                coloniaViewModel.Colonia = new Colonia { Habilitado = true };
                using (var bd = new Contexto())
                {
                    _coloniaRepositorio = new ColoniaRepositorio(bd);
                    coloniaViewModel.Colonias = _coloniaRepositorio.Buscar(coloniaViewModel);
                    coloniaViewModel.TotalEncontrados = _coloniaRepositorio.ObtenerTotalRegistros(coloniaViewModel);
                    coloniaViewModel.Paginas = Paginar.ObtenerCantidadPaginas(coloniaViewModel.TotalEncontrados,
                                                    coloniaViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            return View(coloniaViewModel);
        }

        [HttpPost]
        public ActionResult Index(ColoniaViewModel coloniaViewModel, string pagina)
        {
            coloniaViewModel.PaginaActual = pagina.TryToInt();
            if (coloniaViewModel.PaginaActual <= 0) coloniaViewModel.PaginaActual = 1;

            try
            {
                using (var bd = new Contexto())
                {
                    _coloniaRepositorio = new ColoniaRepositorio(bd);
                    coloniaViewModel.Colonias = _coloniaRepositorio.Buscar(coloniaViewModel);
                    coloniaViewModel.TotalEncontrados = _coloniaRepositorio.ObtenerTotalRegistros(coloniaViewModel);
                    coloniaViewModel.Paginas = Paginar.ObtenerCantidadPaginas(coloniaViewModel.TotalEncontrados,
                                                    coloniaViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(coloniaViewModel);
        }


        [HttpGet]
        public ActionResult Guardar(string idColonia)
        {
            var colonia = new Colonia { IDColonia = idColonia.DecodeFrom64().TryToInt() };

            if (colonia.IDColonia > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        _coloniaRepositorio = new ColoniaRepositorio(bd);
                        colonia = _coloniaRepositorio.BuscarPorId(colonia.IDColonia);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            return View(colonia);
        }

        [HttpPost]
        public ActionResult Guardar(Colonia colonia)
        {
            
            var correcto = false;
            colonia.IDColonia = Request.QueryString["idColonia"].DecodeFrom64().TryToInt();
            ModelState["idColonia"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    colonia.IDColonia = ObtenerParametroGetEnInt(Enumerados.Parametro.IdColonia);
                    using (var bd = new Contexto())
                    {
                        _coloniaRepositorio = new ColoniaRepositorio(bd);

                        var _movimiento = "Creación";
                        if (colonia.IDColonia > 0)
                        {
                            _coloniaRepositorio.Modificar(colonia);
                            _movimiento = "Modificación";
                        }
                        else
                        {
                            _coloniaRepositorio.Guardar(colonia);
                        }
                        correcto = bd.SaveChanges() >= 1;
                        if (correcto)
                        {
                            GuardarMovimiento(_movimiento, "Colonia", colonia.IDColonia);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //EscribirError(ex.Message);
                }
            }

            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Colonia), correcto);
            ModelState.Clear();
            return View(new Colonia());
        }

        [HttpGet]
        public ActionResult Habilitar(string idColonia)
        {
            try
            {
                var idColoniaHabilitar = idColonia.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _coloniaRepositorio = new ColoniaRepositorio(bd);
                    _coloniaRepositorio.CambiarHabilitado(idColoniaHabilitar, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                }
            }
            catch (Exception ex)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idColonia)
        {
            try
            {
                var idColoniaHabilitar = idColonia.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _coloniaRepositorio = new ColoniaRepositorio(bd);
                    _coloniaRepositorio.CambiarHabilitado(idColoniaHabilitar, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                }
            }
            catch (Exception ex)
            {
                EscribirError(string.Format(General.FalloDeshabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ListaColonias(int idMunicipio, int idEstado, string buscar)
        {
            JsonResult lista = new JsonResult();
            using (var bd = new Contexto())
            {
                _coloniaRepositorio = new ColoniaRepositorio(bd);
                lista = Json(_coloniaRepositorio.BuscarColoniasPorMunicipio(idMunicipio, idEstado, buscar));
                return lista;
            }
            
        }

        [WebMethod]
        [HttpPost]
        public JsonResult CodigoPostal(int idColonia)
        {
            Colonia CP;
            using (var bd = new Contexto())
            {
                _coloniaRepositorio = new ColoniaRepositorio(bd);
                CP = _coloniaRepositorio.BuscarPorId(idColonia);
            }
            return Json(CP.CodigoPostal);
        }

    }
}

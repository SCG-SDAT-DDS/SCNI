using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Presentacion.Areas.Catalogo.Models;
//using Presentacion.Areas.Catalogo.Models.ViewModels;
using Presentacion.Controllers;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Sistema.Paginador;
using Resources;
using Sistema.Extensiones;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class MunicipioController : ControladorBase
    {
        private MunicipioRepositorio _municipioRepositorio;
        private Contexto db = new Contexto();

        // GET: Catalogo/Estado
        [HttpGet]
        public ActionResult Index(MunicipioViewModel municipioViewModel)
        {
            try
            {
                municipioViewModel.Municipio = new Municipio();
                using (var bd = new Contexto())
                {
                    _municipioRepositorio = new MunicipioRepositorio(bd);
                    municipioViewModel.Municipios = _municipioRepositorio.Buscar(municipioViewModel);
                    municipioViewModel.TotalEncontrados = _municipioRepositorio.ObtenerTotalRegistros(municipioViewModel);
                    municipioViewModel.Paginas = Paginar.ObtenerCantidadPaginas(municipioViewModel.TotalEncontrados, municipioViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            //return View();
            return View(municipioViewModel);
            //return View(db.Colonia.ToList());
        }

        [HttpPost]
        public ActionResult Index(MunicipioViewModel municipioViewModel, string pagina)
        {
            municipioViewModel.PaginaActual = pagina.TryToInt();
            if (municipioViewModel.PaginaActual <= 0) municipioViewModel.PaginaActual = 1;

            try
            {
                using (var bd = new Contexto())
                {
                    _municipioRepositorio = new MunicipioRepositorio(bd);
                    municipioViewModel.Municipios = _municipioRepositorio.Buscar(municipioViewModel);
                    municipioViewModel.TotalEncontrados = _municipioRepositorio.ObtenerTotalRegistros(municipioViewModel);
                    municipioViewModel.Paginas = Paginar.ObtenerCantidadPaginas(municipioViewModel.TotalEncontrados, municipioViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(municipioViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idMunicipio, string idEstado)
        {
            var municipio = new Municipio { IDMunicipio = idMunicipio.DecodeFrom64().TryToInt() };
            var estado = new Estado { IDEstado = idEstado.DecodeFrom64().TryToInt() };

            if (municipio.IDMunicipio > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        _municipioRepositorio = new MunicipioRepositorio(bd);
                        municipio = _municipioRepositorio.BuscarPorId(municipio.IDMunicipio, estado.IDEstado);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            return View(municipio);
        }

        [HttpPost]
        public ActionResult Guardar(Municipio municipio)
        {
            var correcto = false;
            municipio.IDMunicipio = Request.QueryString["idMunicipio"].DecodeFrom64().TryToInt();
            municipio.IDEstado = Request.QueryString["idEstado"].DecodeFrom64().TryToInt();
            ModelState["idMunicipio"].Errors.Clear();
            ModelState["idEstado"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    municipio.IDMunicipio = ObtenerParametroGetEnInt(Enumerados.Parametro.IdMunicipio);
                    municipio.IDEstado = ObtenerParametroGetEnInt(Enumerados.Parametro.IdEstado);
                    using (var bd = new Contexto())
                    {
                        _municipioRepositorio = new MunicipioRepositorio(bd);

                        if (municipio.IDMunicipio > 0 && municipio.IDEstado > 0)
                        {
                            _municipioRepositorio.Modificar(municipio);
                        }
                        else
                        {
                            _municipioRepositorio.Guardar(municipio);
                        }
                        correcto = bd.SaveChanges() >= 1;
                    }
                    municipio.Estado = new EstadoRepositorio(db).BuscarPorId(Request.QueryString["idEstado"].DecodeFrom64().TryToInt());
                }
                catch (Exception ex)
                {
                    //EscribirError(ex.Message);
                }
            }

            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Municipio
                ), correcto);
            return View(municipio);
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerMunicipios(int idEstado)
        {
            JsonResult lista = new  JsonResult();
            using (var bd = new Contexto())
            {
                _municipioRepositorio = new MunicipioRepositorio(bd);
                lista = Json(_municipioRepositorio.Buscar(idEstado));
                return lista;
            }
        }
    }
}

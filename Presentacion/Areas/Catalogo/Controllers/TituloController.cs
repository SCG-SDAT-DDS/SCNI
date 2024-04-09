using System;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Presentacion.Controllers;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class TituloController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index(TituloViewModel tituloViewModel)
        {
            try
            {
                tituloViewModel.Titulo = new Titulo {Habilitado = true};
                using (var bd = new Contexto())
                {
                    var repositorio = new TituloRepositorio(bd);
                    tituloViewModel.Titulos = repositorio.Buscar(tituloViewModel);
                    tituloViewModel.TotalEncontrados = repositorio.ObtenerTotalRegistros(tituloViewModel);
                    tituloViewModel.Paginas = Paginar.ObtenerCantidadPaginas(tituloViewModel.TotalEncontrados,
                                                    tituloViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(tituloViewModel);
        }

        [HttpPost]
        public ActionResult Index(TituloViewModel tituloViewModel, string pagina)
        {
            if (string.IsNullOrEmpty(pagina)) pagina = "1";
            try
            {
                tituloViewModel.PaginaActual = pagina.TryToInt();
                using (var bd = new Contexto())
                {
                    var repositorio = new TituloRepositorio(bd);
                    tituloViewModel.Titulos = repositorio.Buscar(tituloViewModel);
                    tituloViewModel.TotalEncontrados = repositorio.ObtenerTotalRegistros(tituloViewModel);
                    tituloViewModel.Paginas = Paginar.ObtenerCantidadPaginas(tituloViewModel.TotalEncontrados,
                                                    tituloViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(tituloViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idTitulo)
        {
            var idTituloBuscar = idTitulo.DecodeFrom64().TryToInt();
            var titulo = new Titulo { IDTitulo = idTituloBuscar };

            if (idTituloBuscar > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        var repositorio = new TituloRepositorio(bd);
                        titulo = repositorio.BuscarUnoSolo(t => t.IDTitulo == idTituloBuscar);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            GuardarDatoTemporal(Enumerados.TempData.IdModificar, idTituloBuscar, true);
            return View(titulo);
        }

        [HttpPost]
        public ActionResult Guardar(Titulo titulo)
        {
            var correcto = false;
            titulo.IDTitulo = Request.QueryString["idTitulo"].DecodeFrom64().TryToInt();
            if (titulo.IDTitulo > 0)
            {
                ModelState["idTitulo"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    titulo.IDTitulo = ObtenerParametroGetEnInt(Enumerados.Parametro.IdTitulo);
                    titulo.Habilitado = true;
                    using (var bd = new Contexto())
                    {
                        var repositorio = new TituloRepositorio(bd);
                        var movimiento = "Creación";
                        if (titulo.IDTitulo > 0)
                        {
                            repositorio.Modificar(titulo);
                            movimiento = "Modificación";
                        }
                        else
                        {
                            repositorio.Guardar(titulo);
                        }
                        correcto = bd.SaveChanges() >= 1;
                        GuardarMovimiento(movimiento, "Titulo", titulo.IDTitulo);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }

            EnviarAlerta(Resources.General.GuardadoCorrecto,
                string.Format(Resources.General.FalloGuardado, Resources.General.Puesto), correcto);
            ModelState.Clear();
            return View(titulo);
        }

        [HttpGet]
        public ActionResult Habilitar(string idTitulo)
        {
            try
            {
                var idTituloHabilitar = idTitulo.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    var repositorio = new TituloRepositorio(bd);
                    repositorio.CambiarHabilitado(idTituloHabilitar, true);

                    EscribirMensaje(Resources.General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Titulo", idTituloHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(Resources.General.FalloHabilitar, Resources.General.Titulo));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idTitulo)
        {
            try
            {
                var idTituloDeshabilitar = idTitulo.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    var repositorio = new TituloRepositorio(bd);
                    repositorio.CambiarHabilitado(idTituloDeshabilitar, false);

                    EscribirMensaje(Resources.General.HabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Titulo", idTituloDeshabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(Resources.General.FalloHabilitar, Resources.General.Titulo));
            }

            return RedirectToAction("Index");
        }
    }
}
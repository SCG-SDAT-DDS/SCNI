using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.Repositorios.Carta;
using Datos.DTO.Infraestructura.ViewModels;
using Presentacion.Controllers;
using Sistema.Paginador;

namespace Presentacion.Areas.Carta.Controllers
{
    public class PrecioController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var precioViewModel = new PrecioViewModel
            {
                PaginaActual = 1,
                Precio = new Precio
                {
                    Valor = -1
                }
            };

            using (var bd = new Contexto())
            {
                var precioRepositorio = new CartaPrecioRepositorio(bd);

                //precioViewModel.FechaInicio = new DateTime();
                //precioViewModel.FechaFin = DateTime.Now;

                precioViewModel.Precios = precioRepositorio.Buscar(precioViewModel);

                precioViewModel.TotalEncontrados = precioRepositorio.ObtenerTotalRegistros(precioViewModel);
                precioViewModel.Paginas =
                    Paginar.ObtenerCantidadPaginas(precioViewModel.TotalEncontrados, precioViewModel.TamanoPagina);
            }

            return View(precioViewModel);
        }

        [HttpPost]
        public ActionResult Index(PrecioViewModel precioViewModel, int? pagina = null)
        {
            precioViewModel.PaginaActual = pagina ?? 1;
            precioViewModel.Precio = new Precio {Valor = -1};

            using (var bd = new Contexto())
            {
                var precioRepositorio = new CartaPrecioRepositorio(bd);

                precioViewModel.Precios = precioRepositorio.Buscar(precioViewModel);
                precioViewModel.TotalEncontrados = precioRepositorio.ObtenerTotalRegistros(precioViewModel);
                precioViewModel.Paginas =
                    Paginar.ObtenerCantidadPaginas(precioViewModel.TotalEncontrados, precioViewModel.TamanoPagina);

                ModelState.Clear();
            }

            return View(precioViewModel);
        }

        [WebMethod]
        [HttpPost]
        public JsonResult AsignarPrecioTodos(string precio)
        {
            try
            {
                using (var bd = new Contexto())
                {
                    var repositorioPrecio = new CartaPrecioRepositorio(bd);

                    var _precio = new Precio
                    {
                        Valor = float.Parse(precio)
                    };

                    repositorioPrecio.GuardarNuevoPrecio(_precio);

                    return Json(new
                    {
                        success = true
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
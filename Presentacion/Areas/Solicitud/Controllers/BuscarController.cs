using System.Collections.Generic;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios.Solicitudes;
using Presentacion.Controllers;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Solicitud.Controllers
{
    public class BuscarController : ControladorBase
    {
        SolicitudRepositorio _solicitudRepositorio;
        
        [HttpGet]
        public ActionResult Index(SolicitudViewModel solicitudViewModel, char? p = null)
        {
            using (var bd = new Contexto())
            {
                solicitudViewModel.UrlDescargarArchivo = ObtenerUrlDescargar("Carta");
                solicitudViewModel.IdEmpleado = ObtenerSesion().IdEmpleado;

                solicitudViewModel.Solicitud = new Datos.Solicitud();

                solicitudViewModel.Solicitud.Medio = p != null ? MediosSolicitud.Web : MediosSolicitud.Presencial;

                solicitudViewModel.Solicitud.Carta = new List<Datos.Carta>();
                solicitudViewModel.Solicitud.Entidad = new Entidad();
                solicitudViewModel.Solicitud.Persona = new Persona();

                _solicitudRepositorio = new SolicitudRepositorio(bd);
                solicitudViewModel.Solicitudes = _solicitudRepositorio.Buscar(solicitudViewModel);
                solicitudViewModel.TotalEncontrados = _solicitudRepositorio.ObtenerTotalRegistros(solicitudViewModel);
                solicitudViewModel.Paginas = Paginar.ObtenerCantidadPaginas(solicitudViewModel.TotalEncontrados,
                    solicitudViewModel.TamanoPagina);
            }

            return View(solicitudViewModel);
        }
        
        [HttpPost]
        public ActionResult Index(SolicitudViewModel solicitudViewModel, string pagina)
        {
            solicitudViewModel.UrlDescargarArchivo = ObtenerUrlDescargar("Carta");
            solicitudViewModel.IdEmpleado = ObtenerSesion().IdEmpleado;

            solicitudViewModel.PaginaActual = pagina.TryToInt();

            solicitudViewModel.Solicitud.Entidad = solicitudViewModel.Solicitud.Entidad ?? new Entidad();

            if (solicitudViewModel.PaginaActual <= 0) solicitudViewModel.PaginaActual = 1;

            using (var bd = new Contexto())
            {
                _solicitudRepositorio = new SolicitudRepositorio(bd);
                solicitudViewModel.Solicitudes = _solicitudRepositorio.Buscar(solicitudViewModel);
                solicitudViewModel.TotalEncontrados = _solicitudRepositorio.ObtenerTotalRegistros(solicitudViewModel);
                solicitudViewModel.Paginas = Paginar.ObtenerCantidadPaginas(solicitudViewModel.TotalEncontrados,
                    solicitudViewModel.TamanoPagina);
            }

            if (ModelState != null)
            {
                ModelState.Clear();
            }

            return View(solicitudViewModel);
        }
    }
}
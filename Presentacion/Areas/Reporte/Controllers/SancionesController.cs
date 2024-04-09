using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using Datos.Repositorios.Catalogos;
using Negocio.Servicios;
using Presentacion.Controllers;

namespace Presentacion.Areas.Reporte.Controllers
{
    public class SancionesController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public FileResult Index(ReporteSancionViewModel criterios)
        {
            List<ReporteSancionDto> sanciones;
            int total;
            
            if (criterios.FechaFinEjecutoria != DateTime.MinValue)
                criterios.FechaFinEjecutoria = criterios.FechaFinEjecutoria.AddDays(1).AddMilliseconds(-1);
            
            if (criterios.FechaFinResolucion != DateTime.MinValue)
                criterios.FechaFinResolucion = criterios.FechaFinResolucion.AddDays(1).AddMilliseconds(-1);

            if (criterios.FechaFinInscripcion != DateTime.MinValue)
                criterios.FechaFinInscripcion = criterios.FechaFinInscripcion.AddDays(1).AddMilliseconds(-1);

            using (var db = new Contexto())
            {
                var repositorio = new SancionRepositorio(db);
                sanciones = repositorio.ObtenerReporteSanciones(criterios);
                total = repositorio.ObtenerTotalSanciones();
            }

            var servicios = new ServiciosReporte();
            var reporte = servicios.GenerarReporteSanciones(sanciones, criterios, total);

            return File(reporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporteSanciones.xlsx");
        }
    }
}
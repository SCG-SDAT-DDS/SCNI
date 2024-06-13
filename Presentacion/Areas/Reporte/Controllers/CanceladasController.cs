using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using Datos.Repositorios.Carta;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Solicitudes;
using Negocio.Servicios;
using Presentacion.Controllers;

namespace Presentacion.Areas.Reporte.Controllers
{
    public class CanceladasController : ControladorBase
    {
        // GET: Reporte/Canceladas
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public FileResult Index(ReporteCanceladaViewModel criterios)
        {
            List<ReporteCanceladaDto> solicitudes;
            int total;

            if (criterios.FechaFin != DateTime.MinValue)
                criterios.FechaFin = criterios.FechaFin.AddDays(1).AddMilliseconds(-1);

            using (var db = new Contexto())
            {
                var repositorio = new SolicitudRepositorio(db);
                solicitudes = repositorio.ObtenerReporteSolicitudesCanceladas(criterios);
            }

            var servicios = new ServiciosReporte();
            var reporte = servicios.GenerarReporteCanceladas(solicitudes, criterios);

            return File(reporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporteCanceladas.xlsx");
        }
    }
}
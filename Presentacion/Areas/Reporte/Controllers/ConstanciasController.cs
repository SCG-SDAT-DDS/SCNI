using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using Datos.Repositorios.Carta;
using Negocio.Servicios;
using Presentacion.Controllers;

namespace Presentacion.Areas.Reporte.Controllers
{
    public class ConstanciasController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public FileResult Index(ReporteConstanciaViewModel criterios)
        {
            List<ReporteConstanciaDto> constancias;
            int total;
            
            if (criterios.FechaFin != DateTime.MinValue)
                criterios.FechaFin = criterios.FechaFin.AddDays(1).AddMilliseconds(-1);

            using (var db = new Contexto())
            {
                var repositorio = new CartasRepositorio(db);
                constancias = repositorio.ObtenerReporteConstancias(criterios);
                total = repositorio.ObtenerTotalConstancias();
            }

            var servicios = new ServiciosReporte();
            var reporte = servicios.GenerarReporteConstancias(constancias, criterios, total);

            return File(reporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporteConstancias.xlsx");
        }
    }
}
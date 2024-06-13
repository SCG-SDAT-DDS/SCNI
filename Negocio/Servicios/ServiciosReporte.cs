using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sistema.Extensiones;

namespace Negocio.Servicios
{
    public class ServiciosReporte
    {
        public byte[] GenerarReporteConstancias(List<ReporteConstanciaDto> constancias, ReporteConstanciaViewModel criterios, int total)
        {
            var filtros = new List<string>();

            if (criterios.FechaInicio > DateTime.MinValue)
                filtros.Add("Fecha inicio: " + criterios.FechaInicio.ToString("dd/MM/yyyy"));

            if (criterios.FechaFin > DateTime.MinValue)
                filtros.Add("Fecha fin: " + criterios.FechaFin.ToString("dd/MM/yyyy"));

            if (criterios.TipoSolicitud > 0)
                filtros.Add("Tipo solicitud: " + ((TiposSolicitud)criterios.TipoSolicitud).Descripcion());

            if (criterios.Medio > 0)
                filtros.Add("Medio: " + ((MediosSolicitud)criterios.Medio).Descripcion());

            if (criterios.TipoSancion > 0)
                filtros.Add("Tipo sanción: " + ((TiposSancion) criterios.TipoSancion).Descripcion());

            if (criterios.TipoEntidad > 0)
                filtros.Add("Tipo entidad: " + ((TiposEntidad)criterios.TipoEntidad).Descripcion());

            if (criterios.ConSinSancion > 0)
                filtros.Add("Con sanciones: " + (criterios.ConSinSancion == 1 ? "Sí" : "No"));

            using (var st = new MemoryStream(ReportesFormatos.ReporteConstancias))
            {
                var paqueteExcel = new ExcelPackage(st);

                var hojaExcel = paqueteExcel.Workbook.Worksheets["Constancias"];

                hojaExcel.Cells["I4"].Value = DateTime.Now.ToString("dd/MM/yyyy");
                hojaExcel.Cells["B5"].Value = string.Join(", ", filtros);
                hojaExcel.Cells["E6"].Value = constancias.Count;
                hojaExcel.Cells["G6"].Value = total.ToString();

                var fila = 8;

                double totalPrecio = 0;

                foreach (var constancia in constancias)
                {
                    hojaExcel.Cells["A" + fila].Value = constancia.Folio;
                    hojaExcel.Cells["B" + fila].Value = constancia.TipoCarta.Descripcion();
                    hojaExcel.Cells["C" + fila].Value = constancia.FechaSolicitud.ToString("dd/MM/yyyy");
                    hojaExcel.Cells["D" + fila].Value = constancia.FechaFirma.ToString("dd/MM/yyyy");
                    hojaExcel.Cells["E" + fila].Value = constancia.IdPersona.ToString();
                    hojaExcel.Cells["F" + fila].Value = constancia.Nombre;
                    hojaExcel.Cells["G" + fila].Value = constancia.Paterno;
                    hojaExcel.Cells["H" + fila].Value = constancia.Materno;
                    hojaExcel.Cells["I" + fila].Value = constancia.Medio;
                    hojaExcel.Cells["J" + fila].Value = constancia.TipoSolicitud.Descripcion();
                    hojaExcel.Cells["K" + fila].Value = constancia.Precio ?? 0d;

                    if (constancia.Precio.HasValue)
                        totalPrecio += constancia.Precio.Value;

                    fila++;
                }

                hojaExcel.Cells["I6"].Value = totalPrecio;

                return paqueteExcel.GetAsByteArray();
            }
        }

        public byte[] GenerarReporteSanciones(List<ReporteSancionDto> sanciones, ReporteSancionViewModel criterios, int total)
        {
            var filtros = new List<string>();

            if (criterios.FechaInicioEjecutoria > DateTime.MinValue)
                filtros.Add("Fecha inicio ejecutoria: " + criterios.FechaInicioEjecutoria.ToString("dd/MM/yyyy"));

            if (criterios.FechaFinEjecutoria > DateTime.MinValue)
                filtros.Add("Fecha fin ejecutoria: " + criterios.FechaFinEjecutoria.ToString("dd/MM/yyyy"));

            if (criterios.FechaInicioResolucion > DateTime.MinValue)
                filtros.Add("Fecha inicio resolución: " + criterios.FechaInicioResolucion.ToString("dd/MM/yyyy"));

            if (criterios.FechaFinResolucion > DateTime.MinValue)
                filtros.Add("Fecha fin resolución: " + criterios.FechaFinResolucion.ToString("dd/MM/yyyy"));

            if (criterios.FechaInicioInscripcion > DateTime.MinValue)
                filtros.Add("Fecha inicio inscripción: " + criterios.FechaInicioInscripcion.ToString("dd/MM/yyyy"));

            if (criterios.FechaFinInscripcion > DateTime.MinValue)
                filtros.Add("Fecha fin inscripción: " + criterios.FechaFinInscripcion.ToString("dd/MM/yyyy"));

            if (criterios.TipoSancion > 0)
                filtros.Add("Tipo sanción: " + ((TiposSancion)criterios.TipoSancion).Descripcion());

            if (criterios.TipoEntidad > 0)
                filtros.Add("Tipo entidad: " + ((TiposEntidad)criterios.TipoEntidad).Descripcion());

            if (criterios.Origen > 0)
                filtros.Add("Origen: " + ((Origenes)criterios.Origen).Descripcion());

            if (criterios.ConSinObservaciones > 0)
                filtros.Add("Con observaciones: " + (criterios.ConSinObservaciones == 1 ? "Sí" : "No"));

            using (var st = new MemoryStream(ReportesFormatos.ReporteSanciones))
            {
                var paqueteExcel = new ExcelPackage(st);

                var hojaExcel = paqueteExcel.Workbook.Worksheets["Sanciones"];

                hojaExcel.Cells["I4"].Value = DateTime.Now.ToString("dd/MM/yyyy");
                hojaExcel.Cells["B5"].Value = string.Join(", ", filtros);
                hojaExcel.Cells["G6"].Value = sanciones.Count;
                hojaExcel.Cells["I6"].Value = total.ToString();

                var fila = 8;

                foreach (var sancion in sanciones)
                {
                    hojaExcel.Cells["A" + fila].Value = sancion.IdPersona.ToString();
                    hojaExcel.Cells["B" + fila].Value = sancion.Nombre;
                    hojaExcel.Cells["C" + fila].Value = sancion.Paterno;
                    hojaExcel.Cells["D" + fila].Value = sancion.Materno;
                    hojaExcel.Cells["E" + fila].Value = sancion.FechaInscripcion.ToString();
                    hojaExcel.Cells["F" + fila].Value = sancion.FechaResolucion?.ToString("dd/MM/yyyy");
                    hojaExcel.Cells["G" + fila].Value = sancion.FechaEjecución?.ToString("dd/MM/yyyy");
                    hojaExcel.Cells["H" + fila].Value = sancion.FechaInscripcion?.ToString("dd/MM/yyyy");
                    hojaExcel.Cells["I" + fila].Value = sancion.NumeroExpediente;
                    hojaExcel.Cells["J" + fila].Value = sancion.TipoSancion.Descripcion();
                    if (sancion.Origen > 0) hojaExcel.Cells["K" + fila].Value = sancion.Origen.Descripcion();
                    hojaExcel.Cells["L" + fila].Value = sancion.SancionAno.ToString();
                    hojaExcel.Cells["M" + fila].Value = sancion.TiempoAnos.ToString();
                    hojaExcel.Cells["N" + fila].Value = sancion.TiempoMes.ToString();
                    hojaExcel.Cells["O" + fila].Value = sancion.TiempoDias.ToString();
                    hojaExcel.Cells["P" + fila].Value = sancion.IdEntidad.ToString();
                    hojaExcel.Cells["Q" + fila].Value = sancion.Entidad;
                    hojaExcel.Cells["R" + fila].Value = sancion.TipoEntidad.Descripcion();
                    hojaExcel.Cells["S" + fila].Value = sancion.Monto;
                    hojaExcel.Cells["T" + fila].Value = sancion.Observaciones;

                    fila++;
                }

                return paqueteExcel.GetAsByteArray();
            }
        }

        public byte[] GenerarReporteSancionado(string nombre, List<ReporteSancionadoDto> sanciones)
        {
            using (var st = new MemoryStream(ReportesFormatos.ReporteSancionado))
            {
                var paqueteExcel = new ExcelPackage(st);

                var hojaExcel = paqueteExcel.Workbook.Worksheets["Sanciones"];

                hojaExcel.Cells["L4"].Value = DateTime.Now.ToString("dd/MM/yyyy");

                var fila = 7;

                foreach (var sancion in sanciones)
                {
                    hojaExcel.Cells["A" + fila].Value = sancion.Nombre;
                    hojaExcel.Cells["B" + fila].Value = sancion.Paterno;
                    hojaExcel.Cells["C" + fila].Value = sancion.Materno;
                    hojaExcel.Cells["D" + fila].Value = sancion.Origen == Origenes.SituaciónPatrimonial
                        ? "SP"
                        : sancion.Origen == Origenes.ResponsabilidadOficial ? "RO" : "APM";
                    hojaExcel.Cells["E" + fila].Value = sancion.NumeroExpediente;
                    hojaExcel.Cells["F" + fila].Value = sancion.TipoSancion.Descripcion();
                    hojaExcel.Cells["G" + fila].Value = sancion.SancionAno;
                    hojaExcel.Cells["H" + fila].Value = sancion.Monto;
                    hojaExcel.Cells["I" + fila].Value = sancion.TiempoAnos;
                    hojaExcel.Cells["J" + fila].Value = sancion.TiempoMes;
                    hojaExcel.Cells["K" + fila].Value = sancion.TiempoDias;
                    hojaExcel.Cells["L" + fila].Value = sancion.Entidad;
                    hojaExcel.Cells["M" + fila].Value = sancion.FechaInscripcion.HasValue
                        ? sancion.FechaInscripcion.Value.ToString("dd/MM/yyyy")
                        : string.Empty;

                    fila++;
                }

                if (sanciones.Any() == false)
                {
                    hojaExcel.Cells["A7"].Value = "NO CUENTA CON ANTECEDENTES " + nombre;
                    hojaExcel.Cells["A7:L7"].Merge = true;
                    fila++;
                }

                hojaExcel.PrinterSettings.PrintArea = hojaExcel.Cells["A1:L" + (fila - 1)];

                if (fila != 7)
                {
                    hojaExcel.Cells["A7:L" + (fila - 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hojaExcel.Cells["A7:L" + (fila - 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    hojaExcel.Cells["A7:L" + (fila - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hojaExcel.Cells["A7:L" + (fila - 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                return paqueteExcel.GetAsByteArray();
            }
        }


        public byte[] GenerarReporteCanceladas(List<ReporteCanceladaDto> solicitudes, ReporteCanceladaViewModel criterios)
        {
            var filtros = new List<string>();

            if (criterios.FechaInicio > DateTime.MinValue)
                filtros.Add("Fecha inicio: " + criterios.FechaInicio.ToString("dd/MM/yyyy"));

            if (criterios.FechaFin > DateTime.MinValue)
                filtros.Add("Fecha fin: " + criterios.FechaFin.ToString("dd/MM/yyyy"));


            using (var st = new MemoryStream(ReportesFormatos.ReporteCanceladas))
            {
                var paqueteExcel = new ExcelPackage(st);

                var hojaExcel = paqueteExcel.Workbook.Worksheets["Canceladas"];

                hojaExcel.Cells["E4"].Value = DateTime.Now.ToString("dd/MM/yyyy");
                hojaExcel.Cells["B5"].Value = string.Join(", ", filtros);
                hojaExcel.Cells["D6"].Value = solicitudes.Count;

                var fila = 8;

                foreach (var solicitud in solicitudes)
                {
                    hojaExcel.Cells["A" + fila].Value = solicitud.FolioPaseACaja;
                    hojaExcel.Cells["B" + fila].Value = solicitud.Solicitante;
                    hojaExcel.Cells["C" + fila].Value = solicitud.FolioPago;
                    hojaExcel.Cells["D" + fila].Value = solicitud.FechaSolicitud.ToString("dd/MM/yyyy");
                    hojaExcel.Cells["E" + fila].Value = solicitud.MotivoCancelacion;

                    fila++;
                }


                return paqueteExcel.GetAsByteArray();
            }
        }
    }
}

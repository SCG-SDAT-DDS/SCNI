using System;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class InicioViewModal
    {
        public static InicioViewModal CargarInformacion()
        {
            var viewModel = new InicioViewModal();

            using (var db = new Contexto())
            {
                var hoy = DateTime.Now;
                var limiteCiudadano = hoy.AddDays(-1);
                var limiteEntidad = hoy.AddDays(-3);

                viewModel.PendientesPresencial = (from s in db.Solicitud
                    where !s.Cancelada &&
                          !s.Carta.Any() && 
                          s.Medio == MediosSolicitud.Presencial &&
                          (s.Tipo == (byte)TiposSolicitud.Entidad || s.FechaSolicitud >= limiteCiudadano) &&
                          (s.Tipo == (byte)TiposSolicitud.Ciudadano || s.FechaSolicitud >= limiteEntidad)
                    select s).Count();
                viewModel.PendientesWeb = (from s in db.Solicitud
                    where !s.Cancelada &&
                          !s.Carta.Any() &&
                          s.Medio == MediosSolicitud.Web &&
                          (s.Tipo == (byte) TiposSolicitud.Entidad || s.FechaSolicitud >= limiteCiudadano) &&
                          (s.Tipo == (byte) TiposSolicitud.Ciudadano || s.FechaSolicitud >= limiteEntidad)
                    select s).Count();

                viewModel.AtrasadasPresencial = (from s in db.Solicitud
                    where !s.Cancelada &&
                          !s.Carta.Any() &&
                          s.Medio == MediosSolicitud.Presencial &&
                          (s.Tipo == (byte) TiposSolicitud.Entidad || s.FechaSolicitud < limiteCiudadano) &&
                          (s.Tipo == (byte) TiposSolicitud.Ciudadano || s.FechaSolicitud < limiteEntidad)
                    select s).Count();
                viewModel.AtrasadasWeb = (from s in db.Solicitud
                    where !s.Cancelada &&
                          !s.Carta.Any() &&
                          s.Medio == MediosSolicitud.Web &&
                          (s.Tipo == (byte) TiposSolicitud.Entidad || s.FechaSolicitud < limiteCiudadano) &&
                          (s.Tipo == (byte) TiposSolicitud.Ciudadano || s.FechaSolicitud < limiteEntidad)
                    select s).Count();

                viewModel.MesActualPresencial = (from s in db.Solicitud
                    where !s.Cancelada &&
                          s.Medio == MediosSolicitud.Presencial &&
                          SqlFunctions.DatePart("month", s.FechaSolicitud) == hoy.Month &&
                          SqlFunctions.DatePart("year", s.FechaSolicitud) == hoy.Year
                    select s).Count();
                viewModel.MesActualWeb = (from s in db.Solicitud
                    where !s.Cancelada &&
                          s.Medio == MediosSolicitud.Web &&
                          SqlFunctions.DatePart("month", s.FechaSolicitud) == hoy.Month &&
                          SqlFunctions.DatePart("year", s.FechaSolicitud) == hoy.Year
                    select s).Count();

                viewModel.FirmarPresencial = (from c in db.Cartas
                    where c.Estado == EstadosCarta.NoFirmada &&
                          c.Solicitud.Medio == MediosSolicitud.Presencial
                    select c).Count();
                viewModel.FirmarWeb = (from c in db.Cartas
                    where c.Estado == EstadosCarta.NoFirmada &&
                          c.Solicitud.Medio == MediosSolicitud.Web
                    select c).Count();
            }

            return viewModel;
        }

        public int PendientesPresencial { get; set; }
        public int PendientesWeb { get; set; }
        public int AtrasadasPresencial { get; set; }
        public int AtrasadasWeb { get; set; }
        public int MesActualPresencial { get; set; }
        public int MesActualWeb { get; set; }
        public int FirmarPresencial { get; set; }
        public int FirmarWeb { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Datos.DTO;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;

namespace Datos.Repositorios.Carta
{
    public class CartasRepositorio : Repositorio<Datos.Carta>
    {
        public CartasRepositorio(Contexto contexto) : base(contexto)
        {
        }
        
        public PersonaCartaDto ObtenerCartaPersona(int idCarta)
        {
            return (from c in Contexto.Cartas
                where c.IDCarta == idCarta
                select new PersonaCartaDto
                {
                    Nombre = c.Solicitud.Persona.Nombre + " " +
                             c.Solicitud.Persona.ApellidoP + " " +
                             c.Solicitud.Persona.ApellidoM,
                    Carta = c
                }).Single();
        }

        public override void Modificar(Datos.Carta entidad)
        {
            var cartaModifcar = new Datos.Carta {IDCarta = entidad.IDCarta};

            Contexto.Cartas.Attach(cartaModifcar);

            cartaModifcar.Cadena = entidad.Cadena;

            foreach (var sancion in entidad.Sanciones)
            {
                Contexto.Sancion.Attach(sancion);

                cartaModifcar.Sanciones.Add(sancion);
            }
        }

        public void BorrarSanciones(int idCarta)
        {
            Contexto.Database.ExecuteSqlCommand("DELETE FROM CartaSancion WHERE IDCarta = @id", new SqlParameter("@id", idCarta));
        }

        public void QuitarSanciones(int idCarta)
        {
            Contexto.Database.ExecuteSqlCommand("DELETE FROM CartaSancion WHERE IDCarta =" + idCarta);
        }

        public List<Datos.Carta> Buscar(CartaViewModel criteriosCarta)
        {
            var query = _ObtenerQuery(criteriosCarta, true);

            return query.ToList();
        }

        public int ObtenerTotalRegistros(CartaViewModel criteriosCarta)
        {
            var query = _ObtenerQuery(criteriosCarta, false);

            return query.Count();
        }

        public IQueryable<Datos.Carta> _ObtenerQuery(CartaViewModel criteriosCarta, bool paginar)
        {
            IQueryable<Datos.Carta> query = Contexto.Set<Datos.Carta>();

            if (!string.IsNullOrEmpty(criteriosCarta.Carta.NumeroExpediente))
                query = (from c in query
                         where c.NumeroExpediente.StartsWith(criteriosCarta.Carta.NumeroExpediente)
                         select c);

            if(criteriosCarta.Carta.Estado > 0)
                query = (from c in query
                         where c.Estado == criteriosCarta.Carta.Estado
                         select c);

            if (criteriosCarta.FechaInicio.HasValue)
                query = (from c in query
                    where c.Fecha >= criteriosCarta.FechaInicio.Value
                    select c);

            if (criteriosCarta.FechaFin.HasValue)
                query = (from c in query
                         where c.Fecha < criteriosCarta.FechaFin.Value
                         select c);

            if (paginar && criteriosCarta.TamanoPagina > 0 && criteriosCarta.PaginaActual > 0)
            {
                query = query.OrderByDescending(c => c.Fecha);
                query = query.Skip((criteriosCarta.PaginaActual - 1) * criteriosCarta.TamanoPagina)
                    .Take(criteriosCarta.TamanoPagina);

                query = query.Include(c => c.Solicitud.Persona);
                query = query.Include(c => c.Precio);
            }

            return query;
        }

        public string ObtenerCorreoCodigoSolicitante(int idCarta)
        {
            return (from c in Contexto.Cartas
                where c.IDCarta == idCarta
                select c.Solicitud.Persona.CorreoElectronico).Single();
        }

        public List<ReporteConstanciaDto> ObtenerReporteConstancias(ReporteConstanciaViewModel criterios)
        {
            var query = (from q in Contexto.Cartas
                where q.Estado == EstadosCarta.Firmada &&
                      q.Consultable
                select q);

            if (criterios.FechaInicio != DateTime.MinValue)
                query = (from q in query
                    where q.Fecha >= criterios.FechaInicio
                    select q);

            if (criterios.FechaFin != DateTime.MinValue)
                query = (from q in query
                    where q.Fecha <= criterios.FechaFin
                    select q);

            if (criterios.TipoSolicitud > 0)
                query = (from q in query
                    where q.Solicitud.Tipo == criterios.TipoSolicitud
                    select q);

            if (criterios.Medio > 0)
                query = (from q in query
                    where q.Solicitud.Medio == (MediosSolicitud) criterios.Medio
                    select q);

            if (criterios.TipoSancion > 0)
                query = (from q in query
                    where q.Sanciones.Any(s => s.TipoSancion == criterios.TipoSancion)
                    select q);

            if (criterios.TipoEntidad > 0)
                query = (from q in query
                    where q.Sanciones.Any(s => s.Entidad.Tipo == criterios.TipoEntidad)
                    select q);

            if (criterios.TipoEntidad == 1)
                query = (from q in query
                    where q.Sanciones.Any()
                    select q);

            if (criterios.TipoEntidad == 2)
                query = (from q in query
                    where q.Sanciones.Any()
                    select q);

            return (from q in query
                select new ReporteConstanciaDto
                {
                    IdPersona = q.Solicitud.Persona.IDPersona,
                    Nombre = q.Solicitud.Persona.Nombre,
                    Paterno = q.Solicitud.Persona.ApellidoP,
                    Materno = q.Solicitud.Persona.ApellidoM,
                    Folio = q.NumeroExpediente,
                    FechaSolicitud = q.Solicitud.FechaSolicitud,
                    FechaFirma = q.Fecha,
                    TipoCarta = q.Tipo,
                    Medio = q.Solicitud.Medio,
                    TipoSolicitud = (TiposSolicitud) q.Solicitud.Tipo,
                    Precio = q.Precio.Valor
                }).ToList();
        }

        public int ObtenerTotalConstancias()
        {
            return Contexto.Cartas.Count(c => c.Estado == EstadosCarta.Firmada);
        }

        public Datos.Carta BuscarPorId(int idCarta)
        {
            return (from c in Contexto.Cartas
                    .Include(c => c.Solicitud)
                    .Include(c => c.Solicitud.Persona)
                   where c.IDCarta == idCarta
                   select c).SingleOrDefault();
                    
        }

        public void EliminarCarta(int idCarta)
        {
            Contexto.Database.ExecuteSqlCommand("DELETE FROM PrecioCarta WHERE IDCarta = @id",
                new SqlParameter("@id", idCarta));

            Contexto.Database.ExecuteSqlCommand("DELETE FROM Carta WHERE IDCarta = @id",
                new SqlParameter("@id", idCarta));
        }

        public Datos.Carta ObtenerCartaPorIdSolicitud(int idSolicitud)
        {
            return (from c in Contexto.Cartas
                where c.IDSolicitud == idSolicitud
                select c).SingleOrDefault();
        }

        public void CambiarEstadoConsultable(int idCarta, bool esConsultable)
        {
            var carta = Contexto.Cartas.Include(c => c.Solicitud).Single(c => c.IDCarta == idCarta);

            carta.Consultable = esConsultable;
            carta.Solicitud.Cancelada = !esConsultable;
            if(!esConsultable)
                carta.Solicitud.FechaCancelada = DateTime.Now;
        }
    }
}

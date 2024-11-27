using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;
using Datos.DTO;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using Datos.Recursos;

namespace Datos.Repositorios.Solicitudes
{
    public class SolicitudRepositorio : Repositorio<Solicitud>
    {
        public SolicitudRepositorio(Contexto contexto) 
            : base(contexto) { }

        public override void Modificar(Solicitud entidad)
        {
            Contexto.Solicitud.Attach(entidad);

            Contexto.Entry(entidad).Property(p => p.Tipo).IsModified = true;
            Contexto.Entry(entidad).Property(p => p.FolioPago).IsModified = true;
            Contexto.Entry(entidad).Property(p => p.Fecha).IsModified = true;
            Contexto.Entry(entidad).Property(p => p.Identificacion).IsModified = true;
            Contexto.Entry(entidad).Property(p => p.Codigo).IsModified = true;
            Contexto.Entry(entidad).Property(p => p.FolioPaseCaja).IsModified = true;

            var persona = entidad.Persona;
            Contexto.Persona.Attach(entidad.Persona);

            Contexto.Entry(persona).Property(p => p.RFC).IsModified = true;
            Contexto.Entry(persona).Property(p => p.Nombre).IsModified = true;
            Contexto.Entry(persona).Property(p => p.ApellidoP).IsModified = true;
            Contexto.Entry(persona).Property(p => p.ApellidoM).IsModified = true;
            Contexto.Entry(persona).Property(p => p.Genero).IsModified = true;
            Contexto.Entry(persona).Property(p => p.FechaNacimiento).IsModified = true;
            Contexto.Entry(persona).Property(p => p.CorreoElectronico).IsModified = true;
            Contexto.Entry(persona).Property(p => p.CURP).IsModified = true;
            Contexto.Entry(persona).Property(p => p.IDEstado).IsModified = true;
            Contexto.Entry(persona).Property(p => p.IDMunicipio).IsModified = true;
        }

        /// <summary>
        /// Busca solicitudes por criterios de busqueda
        /// </summary>
        /// <param name="solicitudViewModel"></param>
        /// <returns>Lista de solicitudes encontradas</returns>
        public List<Solicitud> Buscar(SolicitudViewModel solicitudViewModel)
        {
            return ObtenerQuery(solicitudViewModel, true).ToList();
        }

        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="solicitudViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(SolicitudViewModel solicitudViewModel)
        {
            return ObtenerQuery(solicitudViewModel, false).Count();
        }

        public IQueryable<Solicitud> ObtenerQuery(SolicitudViewModel criterios, bool paginar, bool incluir = true)
        {
            IQueryable<Solicitud> query = Contexto.Set<Solicitud>();

            query = query.Where(q => q.Cancelada == criterios.Solicitud.Cancelada);

            if (!string.IsNullOrEmpty(criterios.Solicitud.Persona.Nombre))
            {
                query = query.Where(c =>
                    (c.Persona.Nombre + " " + c.Persona.ApellidoP + " " + c.Persona.ApellidoM).StartsWith(
                        criterios.Solicitud.Persona.Nombre) ||
                    (c.Persona.ApellidoP + " " + c.Persona.ApellidoM + " " + c.Persona.Nombre).StartsWith(
                        criterios.Solicitud.Persona.Nombre));
            }
            if (!string.IsNullOrEmpty(criterios.Solicitud.Persona.RFC))
            {
                query = query.Where(c => c.Persona.RFC.Contains(criterios.Solicitud.Persona.RFC));
            }
            if (!string.IsNullOrEmpty(criterios.Solicitud.FolioPago))
            {
                query = query.Where(c => c.FolioPago.Contains(criterios.Solicitud.FolioPago));
            }
            if (criterios.Solicitud.Tipo > 0)
            {
                query = query.Where(c => c.Tipo == criterios.Solicitud.Tipo);
            }
            if (criterios.Solicitud.Entidad != null)
            {
                if(criterios.Solicitud.Entidad.IDEntidad > 0)
                {
                    query = query.Where(c => c.Entidad.IDEntidad == criterios.Solicitud.Entidad.IDEntidad);
                }
                if (criterios.Solicitud.Entidad.Tipo > 0)
                {
                    query = query.Where(c => c.Entidad.Tipo == criterios.Solicitud.Entidad.Tipo);
                }
            }
            if (criterios.Solicitud.Medio > 0)
            {
                query = query.Where(c => c.Medio == criterios.Solicitud.Medio);
            }

            if (criterios.SinFirmar)
            {
                query = (from q in query
                    where !q.Carta.Any()
                    select q);
            }

            var hoy = DateTime.Now;
            var limiteCiudadano = hoy.AddDays(-1);
            var limiteEntidad = hoy.AddDays(-3);

            if (criterios.Pendientes == true && criterios.Atrasadas == false)
            {
                query = (from q in query
                    where (q.Tipo == (byte) TiposSolicitud.Entidad || q.FechaSolicitud >= limiteCiudadano) &&
                          (q.Tipo == (byte) TiposSolicitud.Ciudadano || q.FechaSolicitud >= limiteEntidad)
                    select q);
            }
            else if (criterios.Pendientes == false && criterios.Atrasadas == true)
            {
                query = (from q in query
                    where (q.Tipo == (byte) TiposSolicitud.Entidad || q.FechaSolicitud < limiteCiudadano) &&
                          (q.Tipo == (byte) TiposSolicitud.Ciudadano || q.FechaSolicitud < limiteEntidad)
                    select q);
            }

            if (criterios.FiltroAnio > 0) {
                query = query.Where(c => c.FechaSolicitud.Year == criterios.FiltroAnio);
            }
            if (criterios.FiltroMes > 0)
            {
                query = query.Where(c => c.FechaSolicitud.Month == criterios.FiltroMes);
            }

            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderByDescending(m => m.FechaSolicitud);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }
            
            query = query.Include(c => c.Entidad);
            query = query.Include(c => c.Persona);
            query = query.Include(c => c.Carta);

            return query;
        }
        
        public IQueryable<Solicitud> ObtenerQueryWeb(SolicitudViewModel criterios, bool paginar)
        {
            IQueryable<Solicitud> query = Contexto.Set<Solicitud>();

            if (!string.IsNullOrEmpty(criterios.Solicitud.Persona.Nombre))
            {
                query = query.Where(c =>
                    c.Persona.Nombre.Contains(criterios.Solicitud.Persona.Nombre)
                    || c.Persona.ApellidoP.Contains(criterios.Solicitud.Persona.Nombre)
                    || c.Persona.ApellidoM.Contains(criterios.Solicitud.Persona.Nombre)
                );
            }
            if (!string.IsNullOrEmpty(criterios.Solicitud.Persona.RFC))
            {
                query = query.Where(c => c.Persona.RFC.Contains(criterios.Solicitud.Persona.RFC));
            }
            if (!string.IsNullOrEmpty(criterios.Solicitud.FolioPago))
            {
                query = query.Where(c => c.FolioPago.Contains(criterios.Solicitud.FolioPago));
            }
            if (criterios.Solicitud.Tipo > 0)
            {
                query = query.Where(c => c.Tipo == criterios.Solicitud.Tipo);
            }
            if (criterios.Solicitud.Entidad != null)
            {
                if (criterios.Solicitud.Entidad.IDEntidad > 0)
                {
                    query = query.Where(c => c.Entidad.IDEntidad == criterios.Solicitud.Entidad.IDEntidad);
                }
                if (criterios.Solicitud.Entidad.Tipo > 0)
                {
                    query = query.Where(c => c.Entidad.Tipo == criterios.Solicitud.Entidad.Tipo);
                }
            }

            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderByDescending(m => m.FechaSolicitud);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            query = query.Include(c => c.Entidad);
            query = query.Include(c => c.Persona);
            query = query.Include(c => c.Carta);

            return query;
        }

        public static List<SelectListItem> BuscarEntidadesPorTipo(int Tipo)
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from m in bd.Entidad
                         where m.Tipo == Tipo
                         orderby m.Nombre
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)m.IDEntidad).Trim(),
                             Text = m.Nombre
                         }).ToList();

                if (lista.Count > 0)
                {
                    lista.Insert(0, new SelectListItem { Text = General.Seleccione, Value = string.Empty });
                }
            }
            return lista;
        }

        public List<SelectListItem> BuscarEntidadPorTipo(int Tipo)
        {
            return (from m in Contexto.Entidad
                    where m.Tipo == Tipo
                        && m.Habilitado == true
                    orderby m.Nombre
                    select new SelectListItem
                    {
                        Value = SqlFunctions.StringConvert((decimal?)m.IDEntidad).Trim(),
                        Text = m.Nombre
                    }).ToList();
        }
        
        /// <summary>
        /// Busca una solicitud por su id.
        /// </summary>
        /// <param name="idSolicitud">Id del rol a buscar.</param>
        /// <returns>Objeto solicitud encontrado.</returns>
        public Solicitud BuscarPorId(int idSolicitud)
        {
            return (from om in Contexto.Solicitud
                    .Include(m => m.Persona.Municipio.Estado)
                    .Include(m => m.Entidad)
                    .Include(m => m.Carta)
                    where om.IDSolicitud == idSolicitud
                    select om).FirstOrDefault();
        }

        public void CancelarSolicitud(int idCarta, string motivo)
        {
            var solicitud = (from c in Contexto.Cartas
                            where c.IDCarta == idCarta
                            select c.Solicitud).SingleOrDefault();

            solicitud.Cancelada = true;
            solicitud.MotivoCancelacion = motivo;
        }

        public object BuscarSolicitudPorFolio(string folio)
        {
            var resultado = (
                from m in Contexto.Solicitud
                where m.FolioPago == folio &&
                      !m.Cancelada
                select new
                {
                    m.IDSolicitud,
                    m.FolioPago,
                    PersonaNombre = m.Persona.Nombre + "  " + m.Persona.ApellidoP + "  " + m.Persona.ApellidoM
                }).FirstOrDefault();
            if (resultado == null)
            {
                var IDSolicitud = 0;
                return IDSolicitud;
            }
            return resultado;
        }

        public bool ExisteFolioSolicitud(string folioReciboOficial, string folioPaseCaja)
        {
            var resultado = (from m in Contexto.Solicitud
                where (m.FolioPago == folioReciboOficial || m.FolioPaseCaja == folioReciboOficial ||
                       m.FolioPago == folioPaseCaja || m.FolioPaseCaja == folioPaseCaja) &&
                      !m.Cancelada
                select m).Any();

            return resultado;
        }

        public object BuscarSolicitudPorFolio(string folio, int idSolicitud)
        {
            var resultado = (
                from m in Contexto.Solicitud
                where m.FolioPago == folio &&
                      !m.Cancelada &&
                      m.IDSolicitud != idSolicitud
                select new
                {
                    m.IDSolicitud,
                    m.FolioPago,
                    PersonaNombre = m.Persona.Nombre + "  " + m.Persona.ApellidoP + "  " + m.Persona.ApellidoM
                }).FirstOrDefault();
            if (resultado == null)
            {
                var IDSolicitud = 0;
                return IDSolicitud;
            }
            return resultado;
        }

        public PersonaSancionesDto ObtenerPersonaSanciones(int idSolicitud, int[] idSanciones)
        {
            return (from s in Contexto.Solicitud
                where s.IDSolicitud == idSolicitud
                select new PersonaSancionesDto
                {
                    Nombre = s.Persona.Nombre + " " +
                             s.Persona.ApellidoP + " " +
                             s.Persona.ApellidoM,
                    Sanciones = (from sa in Contexto.Sancion
                        where idSanciones.Contains(sa.IDSancion)
                        select new SancionDto
                        {
                            Sancion = sa,
                            TipoEntidad = (TiposEntidad) sa.Entidad.Tipo
                        })
                }).Single();
        }

        public PersonaSancionesDto ObtenerPersonaCartaSanciones(int idSolicitud)
        {
            return (from s in Contexto.Solicitud
                where s.IDSolicitud == idSolicitud
                select new PersonaSancionesDto
                {
                    Nombre = s.Persona.Nombre + " " +
                             s.Persona.ApellidoP + " " +
                             s.Persona.ApellidoM,
                    Sanciones = from sa in Contexto.Sancion
                                where sa.Carta.Any(c => c.IDSolicitud == idSolicitud)
                        select new SancionDto
                        {
                            Sancion = sa,
                            TipoEntidad = (TiposEntidad)sa.Entidad.Tipo
                        }
                }).Single();
        }

        public void CancelarSolicitud(Solicitud solicitud, string motivo)
        {
            solicitud.Cancelada = true;
            solicitud.MotivoCancelacion = motivo;
        }

        public bool TieneCartaGenerada(int idSolicitud)
        {
            return (from c in Contexto.Cartas
                where c.IDSolicitud == idSolicitud &&
                      c.Estado == EstadosCarta.NoFirmada
                select c).Any();
        }

        public List<ReporteCanceladaDto> ObtenerReporteSolicitudesCanceladas(ReporteCanceladaViewModel criterios)
        {
            var query = (from q in Contexto.Solicitud
                         join c in Contexto.Cartas on q.IDSolicitud equals c.IDSolicitud into cartasGroup
                         from c in cartasGroup.DefaultIfEmpty()
                         where q.Cancelada
                         select new 
                            { q, c.NumeroExpediente});

            if (criterios.FechaInicio != DateTime.MinValue)
                query = (from q in query
                         where q.q.FechaCancelada >= criterios.FechaInicio
                         select q);

            if (criterios.FechaFin != DateTime.MinValue)
                query = (from q in query
                         where q.q.FechaCancelada <= criterios.FechaFin
                         select q);


            if (!string.IsNullOrEmpty(criterios.Nombre))
                query = (from q in query
                         where q.q.Persona.Nombre.Contains(criterios.Nombre)
                         select q);


            if (!string.IsNullOrEmpty(criterios.ApellidoP))
                query = (from q in query
                         where q.q.Persona.ApellidoP.Contains(criterios.ApellidoP)
                         select q);

            
            if (!string.IsNullOrEmpty(criterios.ApellidoM))
                query = (from q in query
                         where q.q.Persona.ApellidoM.Contains(criterios.ApellidoM)
                         select q);


            if (!string.IsNullOrEmpty(criterios.FolioDePago))
                query = (from q in query
                         where q.q.FolioPago.Contains(criterios.FolioDePago)
                         select q);

            return (from q in query
                    select new ReporteCanceladaDto
                    {
                        FolioPaseACaja = q.q.FolioPaseCaja,
                        Solicitante = q.q.Persona.Nombre + " " + q.q.Persona.ApellidoP + " " + q.q.Persona.ApellidoM,
                        FolioPago = q.q.FolioPago,
                        FechaSolicitud = q.q.FechaSolicitud,
                        MotivoCancelacion = q.q.MotivoCancelacion,
                        NumeroExpediente = q.NumeroExpediente
                    }).ToList();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Datos.DTO;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using Datos.DTO.Servicios;

namespace Datos.Repositorios.Catalogos
{
    public class SancionRepositorio : Repositorio<Sancion>
    {
        public SancionRepositorio(Contexto contexto)
            : base(contexto) { }

        /// <summary>
        /// Busca sanciones por criterios de busqueda
        /// </summary>
        /// <param name="sancionViewModel"></param>
        /// <returns>Lista de sanciones encontradas</returns>
        public List<Sancion> Buscar(SancionViewModel sancionViewModel)
        {
            return ObtenerQuery(sancionViewModel, true).ToList();
        }

        public List<ReporteSancionadoDto> ObtenerReporteSancionado(SancionViewModel sancionViewMode)
        {
            var query = ObtenerQuery(sancionViewMode, false);

            return (from q in query
                select new ReporteSancionadoDto
                {
                    Nombre = q.Persona.Nombre,
                    Paterno = q.Persona.ApellidoP,
                    Materno = q.Persona.ApellidoM,
                    NumeroExpediente = q.NumeroExpediente,
                    Origen = (Origenes)(q.Tipo ?? 0),
                    TipoSancion = (TiposSancion)q.TipoSancion,
                    SancionAno = q.Año,
                    TiempoAnos = q.TiempoAños,
                    TiempoMes = q.TiempoMeses,
                    TiempoDias = q.TiempoDias,
                    Entidad = q.Entidad.Nombre,
                    Monto = q.Monto,
                    FechaInscripcion = q.FechaInscripcion
                }).ToList();
        }

        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="sancionViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(SancionViewModel sancionViewModel)
        {
            return ObtenerQuery(sancionViewModel, false).Count();
        }

        public IQueryable<Sancion> ObtenerQuery(SancionViewModel criterios, bool paginar)
        {
            IQueryable<Sancion> query = Contexto.Set<Sancion>();

            query = query.Where(p => p.Habilitado == criterios.Sancion.Habilitado);

            if (!string.IsNullOrWhiteSpace(criterios.Sancion.NumeroExpediente))
            {
                query = query.Where(p => p.NumeroExpediente.Contains(criterios.Sancion.NumeroExpediente));
            }

            // Buscar por critero de personas
            if (criterios.Sancion.Persona != null)
            {
                if (!string.IsNullOrEmpty(criterios.Sancion.Persona.Nombre) && !string.IsNullOrEmpty(criterios.Sancion.Persona.RFC))
                {
                    query = query.Where(
                        c => (c.Persona.Nombre + " " + c.Persona.ApellidoP + " " + c.Persona.ApellidoM)
                             .Contains(criterios.Sancion.Persona.Nombre) ||
                             (c.Persona.ApellidoP + " " + c.Persona.ApellidoM + " " + c.Persona.Nombre)
                             .Contains(criterios.Sancion.Persona.Nombre) ||
                             (c.Persona.ApellidoP + " " + c.Persona.Nombre)
                             .Contains(criterios.Sancion.Persona.Nombre) ||
                             c.Persona.RFC.Contains(criterios.Sancion.Persona.RFC));
                }
                else if (!string.IsNullOrEmpty(criterios.Sancion.Persona.Nombre))
                {
                    query = query.Where(
                        c => (c.Persona.Nombre + " " + c.Persona.ApellidoP + " " + c.Persona.ApellidoM)
                             .Contains(criterios.Sancion.Persona.Nombre) ||
                             (c.Persona.ApellidoP + " " + c.Persona.ApellidoM + " " + c.Persona.Nombre)
                             .Contains(criterios.Sancion.Persona.Nombre) ||
                             (c.Persona.ApellidoP + " " + c.Persona.Nombre)
                             .Contains(criterios.Sancion.Persona.Nombre));
                }
                else if (!string.IsNullOrEmpty(criterios.Sancion.Persona.RFC))
                {
                    query = query.Where(p => p.Persona.RFC.Contains(criterios.Sancion.Persona.RFC));
                }

                if (criterios.Sancion.Persona.IDPersona > 0)
                {
                    query = query.Where(p => p.Persona.IDPersona == criterios.Sancion.Persona.IDPersona);
                }
            }

            // Buscar por critero de entidades
            if (criterios.Sancion.Entidad != null)
            {
                if (!string.IsNullOrEmpty(criterios.Sancion.Entidad.Nombre))
                {
                    query = query.Where(p => p.Entidad.Nombre.Contains(criterios.Sancion.Entidad.Nombre) ||
                                             p.Entidad.Abreviacion.Contains(criterios.Sancion.Entidad.Nombre));
                }
                if (criterios.Sancion.Entidad.IDEntidad > 0) {
                    query = query.Where(p => p.Entidad.IDEntidad == criterios.Sancion.Entidad.IDEntidad);
                }
                if (criterios.Sancion.Entidad.Tipo > 0)
                {
                    query = query.Where(p => p.Entidad.Tipo == criterios.Sancion.Entidad.Tipo);
                }
                else if (criterios.EsEstatal == true)
                {
                    query = query.Where(p => p.Entidad.Tipo != (int)TiposEntidad.Federal);
                }
            }

            // Buscar por critero de sanción
            if (criterios.Sancion.Año > 0)
            {
                query = query.Where(s => s.Año == criterios.Sancion.Año);
            }
            if (!string.IsNullOrEmpty(criterios.Sancion.NumeroExpediente))
            {
                query = query.Where(s => s.NumeroExpediente.Contains(criterios.Sancion.NumeroExpediente));
            }
            if (criterios.Sancion.Tipo > 0)
            {
                query = query.Where(s => s.Tipo == criterios.Sancion.Tipo);
            }
            if (criterios.Sancion.TipoSancion > 0)
            {
                query = query.Where(s => s.TipoSancion == criterios.Sancion.TipoSancion);
            }
            // Buscar por fecha Ejecutoria y de Resolución
            if (criterios.FechaInicioEjecutaria.HasValue)
            {
                query = query.Where(s => s.FechaEjecutoria >= criterios.FechaInicioEjecutaria);
            }
            if (criterios.FechaFinEjecutaria.HasValue)
            {
                criterios.FechaFinEjecutaria = criterios.FechaFinEjecutaria.Value.AddDays(1).AddSeconds(-1);
                query = query.Where(s => s.FechaEjecutoria <= criterios.FechaFinEjecutaria);
            }
            if (criterios.FechaInicioResolucion.HasValue)
            {
                query = query.Where(s => s.FechaResolucion >= criterios.FechaInicioResolucion);
            }
            if (criterios.FechaFinResolucion.HasValue)
            {
                criterios.FechaFinEjecutaria = criterios.FechaFinResolucion.Value.AddDays(1).AddSeconds(-1);
                query = query.Where(s => s.FechaResolucion <= criterios.FechaFinResolucion);
            }

            // Criterios de paginación y ordenamiento
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Año);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            // Iclusión de relaciones
            query = query.Include(c => c.Persona);
            query = query.Include(c => c.Entidad);

            return query;
        }

        public Sancion BuscarPorId(int idSancion)
        {
            var sancion = new Sancion();
            sancion = (from e in Contexto.Sancion
                       .Include(e => e.Entidad)
                       .Include(e => e.Persona)
                    where e.IDSancion == idSancion
                    select e).SingleOrDefault();
            return sancion;
        }

        public List<SancionDto> ObtenerSanciones(int[] idSanciones)
        {
            return (from sa in Contexto.Sancion
                where idSanciones.Contains(sa.IDSancion)
                select new SancionDto
                {
                    Sancion = sa,
                    TipoEntidad = (TiposEntidad) sa.Entidad.Tipo
                }).ToList();
        }

        public List<ReporteSancionDto> ObtenerReporteSanciones(ReporteSancionViewModel criterios)
        {
            var query = from q in Contexto.Sancion
                        select q ;

            if (criterios.FechaInicioResolucion != DateTime.MinValue)
                query = (from q in query
                    where q.FechaResolucion >= criterios.FechaInicioResolucion
                    select q);

            if (criterios.FechaFinResolucion != DateTime.MinValue)
                query = (from q in query
                    where q.FechaResolucion <= criterios.FechaFinResolucion
                    select q);

            if (criterios.FechaInicioEjecutoria != DateTime.MinValue)
                query = (from q in query
                    where q.FechaEjecutoria >= criterios.FechaInicioEjecutoria
                    select q);

            if (criterios.FechaFinEjecutoria != DateTime.MinValue)
                query = (from q in query
                    where q.FechaEjecutoria <= criterios.FechaFinEjecutoria
                    select q);

            if (criterios.TipoSancion > 0)
                query = (from q in query
                    where q.TipoSancion == criterios.TipoSancion
                    select q);

            if (criterios.Origen > 0)
                query = (from q in query
                    where q.Tipo == criterios.Origen
                    select q);

            if (criterios.ConSinObservaciones == 1)
                query = (from q in query
                    where q.Observaciones.Length > 0
                    select q);

            if (criterios.ConSinObservaciones == 2)
                query = (from q in query
                    where q.Observaciones.Length == 0
                    select q);

            if (criterios.TipoEntidad > 0)
                query = (from q in query
                    where q.Entidad.Tipo == criterios.TipoEntidad
                    select q);

            return (from q in query
                    join m in Contexto.Movmiento
                    on new { IDRegistro = q.IDSancion, Catalogo = "Sanción", Nombre = "Creación" }
                    equals new { IDRegistro = m.IDRegistro ?? 0, m.Catalogo, m.Nombre } into Sm
                    from m in Sm.DefaultIfEmpty()
                    where (criterios.FechaInicioInscripcion == DateTime.MinValue || criterios.FechaInicioInscripcion <= (q.FechaInscripcion ?? m.Fecha)) &&
                          (criterios.FechaFinInscripcion == DateTime.MinValue || criterios.FechaFinInscripcion >= (q.FechaInscripcion ?? m.Fecha))
                    select new ReporteSancionDto
                    {
                    IdPersona = q.Persona.IDPersona,
                    Nombre = q.Persona.Nombre,
                    Paterno = q.Persona.ApellidoP,
                    Materno = q.Persona.ApellidoM,
                    FechaEjecución = q.FechaEjecutoria,
                    FechaResolucion = q.FechaResolucion,
                    NumeroExpediente = q.NumeroExpediente,
                    Origen = (Origenes) (q.Tipo ?? 0),
                    TipoSancion = (TiposSancion) q.TipoSancion,
                    SancionAno = q.Año,
                    TiempoAnos = q.TiempoAños,
                    TiempoMes = q.TiempoMeses,
                    TiempoDias = q.TiempoDias,
                    IdEntidad = q.IDEntidad,
                    Entidad = q.Entidad.Nombre,
                    TipoEntidad = (TiposEntidad) q.Entidad.Tipo,
                    Monto = q.Monto,
                    Observaciones = q.Observaciones,
                    FechaInscripcion = q.FechaInscripcion ?? (DateTime?)m.Fecha
                }).ToList();
        }

        public int ObtenerTotalSanciones()
        {
            return Contexto.Sancion.Count();
        }

        public List<Sancion> ObtenerSancionesCarta(int idCarta)
        {
            return (from s in Contexto.Sancion.Include(i => i.Persona).Include(i => i.Entidad)
                where s.Carta.Any(c => c.IDCarta == idCarta)
                select s).ToList();
        }

        public List<result_sancion> ObtenerSancionesEstatalesServicio(int numeroPagina, int tamanoPagina)
        {
            var sql = "EXEC sp_ObtenerSancionesEstatalesServicio " +
                      (numeroPagina - 1) * tamanoPagina + "," + tamanoPagina;

            if (Contexto.Database.Connection.State == ConnectionState.Closed)
                Contexto.Database.Connection.Open();

            var cmd = Contexto.Database.Connection.CreateCommand();
            cmd.CommandText = sql;

            var lista = new List<result_sancion>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var sancion = new result_sancion();

                    lista.Add(sancion);

                    if (string.IsNullOrEmpty(reader["FECHA_CAPTURA"].ToString()) == false)
                        sancion.fecha_captura = Convert.ToDateTime(reader["FECHA_CAPTURA"].ToString()).ToString("yyyy-MM-dd");
                    else
                        sancion.fecha_captura = string.Empty;
                    if (string.IsNullOrEmpty(reader["RFC"].ToString()))
                        sancion.rfc = reader["RFC"].ToString();
                    else
                        sancion.rfc = string.Empty;
                    if (string.IsNullOrEmpty(reader["CURP"].ToString()))
                        sancion.curp = reader["CURP"].ToString();
                    else
                        sancion.curp = string.Empty;
                    sancion.nombre = reader["NOMBRE"].ToString();
                    sancion.apellido_paterno = reader["PRIMER_APELLIDO"].ToString();
                    sancion.apellido_materno = reader["SEGUNDO_APELLIDO"].ToString();
                    sancion.genero = reader["GENERO"].ToString()[0];
                    sancion.institucion_dependencia = new result_institucion_dependencia
                    {
                        nombre = reader["INSTITUCION_DEPENDENCIA_NOMBRE"].ToString(),
                        siglas = reader["INSTITUCION_DEPENDENCIA_SIGLAS"].ToString(),
                    };
                    //SIGLAS
                    sancion.puesto = reader["PUESTO"].ToString();
                    sancion.autoridad_sancionadora = reader["AUTORIDAD_SANCIONADORA"].ToString();
                    sancion.tipo_sancion = reader["TIPO_SANCION"].ToString();
                    sancion.tipo_falta = reader["TIPO_FALTA"].ToString();
                    sancion.causa = reader["CAUSA"].ToString();
                    sancion.expediente = reader["EXPEDIENTE"].ToString();
                    sancion.resolucion = new result_resolucion
                    {
                        url = string.Empty,
                        fecha_notificacion = string.Empty
                    };

                    if (string.IsNullOrEmpty(reader["MULTA_MONTO"].ToString()) == false)
                    {
                        sancion.multa = new result_multa
                        {
                            monto = Convert.ToInt32(reader["MULTA_MONTO"].ToString()).ToString(),
                            unidad_moneda = reader["MULTA_UNIDAD_MONEDA"].ToString()
                        };
                    }
                    else
                    {
                        sancion.multa = new result_multa
                        {
                            unidad_moneda = string.Empty
                        };
                    }

                    sancion.inhabilitacion = new result_inhabilitacion((int) reader["INHABILITACION_PLAZO_ANIO"],
                        (int) reader["INHABILITACION_PLAZO_MES"],
                        (int) reader["INHABILITACION_PLAZO_DIA"])
                    {
                        observaciones = reader["INHABILITACION_OBSERVACIONES"].ToString()
                    };

                    if (string.IsNullOrEmpty(reader["INHABILITACION_FECHA_INICIAL"].ToString()) == false)
                        sancion.inhabilitacion.fecha_inicial = Convert
                            .ToDateTime(reader["INHABILITACION_FECHA_INICIAL"].ToString())
                            .ToString("yyyy-MM-dd");
                    else
                        sancion.inhabilitacion.fecha_inicial = string.Empty;

                    if (string.IsNullOrEmpty(reader["INHABILITACION_FECHA_FINAL"].ToString()) == false)
                        sancion.inhabilitacion.fecha_final = Convert
                            .ToDateTime(reader["INHABILITACION_FECHA_FINAL"].ToString())
                            .ToString("yyyy-MM-dd");
                    else
                        sancion.inhabilitacion.fecha_final = string.Empty;

                    var documento = new result_documento
                    {
                        tipo = reader["TIPO_DOCUMENTO"].ToString(),
                        titulo = reader["TITULO_DOCUMENTO"].ToString(),
                        descripcion = reader["DESCRIPCION_DOCUMENTO"].ToString(),
                        url = reader["URL_DOCUMENTO"].ToString()
                    };
                    
                    if (string.IsNullOrEmpty(reader["ID_DOCUMENTO"].ToString()) == false)
                        documento.id = Convert.ToInt32(reader["ID_DOCUMENTO"].ToString());

                    if (string.IsNullOrEmpty(reader["FECHA_DOCUMENTO"].ToString()) == false)
                        documento.fecha = Convert.ToDateTime(reader["FECHA_DOCUMENTO"].ToString())
                            .ToString("yyyy-MM-dd");
                    else
                        documento.fecha = string.Empty;

                    sancion.documentos = new List<result_documento> { documento };
                }
            }

            return lista;
        }

        public int ObtenerSancionesEstatalesServicio()
        {
            var sql = @"EXEC sp_ObtenerCantidadSancionesEstatalesServicio";

            if (Contexto.Database.Connection.State == ConnectionState.Closed)
                Contexto.Database.Connection.Open();

            var cmd = Contexto.Database.Connection.CreateCommand();
            cmd.CommandText = sql;

            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                return int.Parse(reader["CANTIDAD"].ToString());
            }
        }
    }
}

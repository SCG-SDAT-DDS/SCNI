using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Recursos;
using Datos.Repositorios;
using Sistema;

namespace Datos.Repositorios.Catalogos
{
    public class EntidadRepositorio : Repositorio<Entidad>
    {
        public EntidadRepositorio(Contexto contexto) : base(contexto)
        {
        }

        /// <summary>
        /// Busca entidades por criterios de busqueda
        /// </summary>
        /// <param name="entidadViewModel"></param>
        /// <returns>Lista de entidades encontradas</returns>
        public List<Entidad> Buscar(EntidadViewModel entidadViewModel)
        {
            return ObtenerQuery(entidadViewModel, true).ToList();
        }
        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="entidadViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(EntidadViewModel entidadViewModel)
        {
            return ObtenerQuery(entidadViewModel, false).Count();
        }

        public IQueryable<Entidad> ObtenerQuery(EntidadViewModel criterios, bool paginar)
        {
            IQueryable<Entidad> query = Contexto.Set<Entidad>();

            query = query.Where(e => e.Habilitado == criterios.Entidad.Habilitado);

            if (!string.IsNullOrEmpty(criterios.Entidad.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(criterios.Entidad.Nombre) || c.Abreviacion.Contains(criterios.Entidad.Nombre));
            }
            if (!string.IsNullOrEmpty(criterios.Entidad.Abreviacion))
            {
                query = query.Where(c => c.Abreviacion.Contains(criterios.Entidad.Abreviacion));
            }
            if (criterios.Entidad.Tipo > 0)
            {
                query = query.Where(c => c.Tipo == criterios.Entidad.Tipo);
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }


            return query;
        }

        /// <summary>
        /// Cambia el habilitado de una entidad
        /// </summary>
        /// <param name="idEntidad">ID de la entidad</param>
        /// <param name="habilitado">Opción habilitado</param>
        /// <returns>Verdadero si se guardo correctamente</returns>
        public bool CambiarHabilitado(int idEntidad, bool habilitado)
        {
            var entidad = new Entidad { IDEntidad = idEntidad };

            Contexto.Entidad.Attach(entidad);
            entidad.Habilitado = habilitado;
            Contexto.Entry(entidad).Property(r => r.Habilitado).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Busca una Entidad por su ID
        /// </summary>
        /// <param name="idEntidad">ID de Entidad a Buscar</param>
        /// <returns></returns>
        public Entidad BuscarPorId(int idEntidad)
        {
            return (from c in Contexto.Entidad
                    where c.IDEntidad == idEntidad
                    select c).FirstOrDefault();
        }

        public static implicit operator EntidadRepositorio(EmpleadoRepostorio v)
        {
            throw new NotImplementedException();
        }

        public object BuscarPorCriterio(string Criterio)
        {
            var resultado = (
                from m in Contexto.Entidad
                where (m.Nombre.Contains(Criterio) || m.Abreviacion.Contains(Criterio))
                select new
                {
                    IDEntidad = m.IDEntidad,
                    Nombre = m.Nombre,
                    Abreviacion = m.Abreviacion,
                    Tipo = m.Tipo
                });
            return resultado;
        }
        public object BuscarEntidadPorID(int idEntidad)
        {
            var resultado = (
                from m in Contexto.Entidad
                where (m.IDEntidad == idEntidad)
                select new
                {
                    IDEntidad = m.IDEntidad,
                    Nombre = m.Nombre,
                    Abreviacion = m.Abreviacion ?? "",
                    Tipo = m.Tipo
                }).FirstOrDefault();
            return resultado;
        }

        public Entidad BuscarEntidadPorId(int idEntidad)
        {
            return (from m in Contexto.Entidad
                where (m.IDEntidad == idEntidad)
                select m
                ).FirstOrDefault();
        }

        public static List<SelectListItem> ObtenerListaEntidadesFederales()
        {
            using (var bd = new Contexto())
            {
                List<SelectListItem> entidades = new List<SelectListItem>();

                var resultados = (
                    from m in bd.Entidad
                    where m.Tipo == 3
                    select m
                    );



                entidades.Add(new SelectListItem { Text = "Seleccione...", Value = "" });
                foreach (Entidad entidad in resultados)
                {
                    entidades.Add(new SelectListItem { Text = entidad.Nombre, Value = entidad.IDEntidad.ToString() });
                }

                return entidades;
            }

    }

        public Entidad ObtenerDefault()
        {
            return (from e in Contexto.Entidad
                where e.Tipo == (byte) TiposEntidad.Federal &&
                      e.Habilitado &&
                      e.Nombre == "ADMINISTRACIÓN PÚBLICA FEDERAL"
                select e).FirstOrDefault();
        }
    }
}

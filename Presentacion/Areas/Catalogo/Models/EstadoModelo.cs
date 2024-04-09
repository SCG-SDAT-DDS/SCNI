using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Presentacion.Areas.Catalogo.Models.ViewModels;
using Presentacion.Areas.Catalogo.Models;
using System.Web;
using Datos;
using Datos.Repositorios;

namespace Presentacion.Areas.Catalogo.Models
{
    public class EstadoModelo : Repositorio<Estado>
    {
        public EstadoModelo(Contexto contexto) : base(contexto)
        {
        }

        /// <summary>
        /// Busca estados por criterios de busqueda
        /// </summary>
        /// <param name="estadoViewModel"></param>
        /// <returns>Lista de estados encontrados</returns>
        public List<Estado> Buscar(EstadoViewModel estadoViewModel)
        {
            return ObtenerQuery(estadoViewModel, true).ToList();
        }
        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="estadoViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(EstadoViewModel estadoViewModel)
        {
            return ObtenerQuery(estadoViewModel, false).Count();
        }

        public IQueryable<Estado> ObtenerQuery(EstadoViewModel criterios, bool paginar)
        {
            IQueryable<Estado> query = Contexto.Set<Estado>();

            if (!string.IsNullOrEmpty(criterios.Estado.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(criterios.Estado.Nombre));
            }
            if (!string.IsNullOrEmpty(criterios.Estado.Abreviatura))
            {
                query = query.Where(c => c.Abreviatura.Contains(criterios.Estado.Abreviatura));
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            //query = query.Include(c => c.Nombre);
            //query = query.Include(c => c.Abreviatura);

            return query;
        }

        /// <summary>
        /// Busca un estado por su ID
        /// </summary>
        /// <param name="idEstado">ID del Estado a Buscar</param>
        /// <returns></returns>
        public Estado BuscarPorId(int idEstado)
        {
            return (from c in Contexto.Estado
                    where c.IDEstado == idEstado
                    select c).FirstOrDefault();
        }

    }
}
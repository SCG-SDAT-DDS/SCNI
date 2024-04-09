using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;
using Datos.Recursos;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios;
using Datos.Repositorios.Catalogos;

namespace Datos.Repositorios.Catalogos
{
    public class EstadoRepositorio : Repositorio<Estado>
    {
        public EstadoRepositorio(Contexto contexto) : base(contexto)
        {
        }

        /// <summary>
        /// Busca los estados que contienen al menos una colonia
        /// </summary>
        /// <returns>Lista de estados encontrados con colonias</returns>
        public List<Estado> BuscarEstadosConColonias()
        {
            return (from c in Contexto.Colonia
                    select c.Municipio.Estado).Distinct().OrderBy(c => c.Nombre).ToList();
        }

        /// <summary>
        /// Busca todos los estados
        /// </summary>
        /// <returns>Lista de estados encontrados</returns>
        public List<Estado> BuscarEstados()
        {
            return (from e in Contexto.Estado
                    select e).ToList();
        }

        /// <summary>
        /// Busca colonias por criterios de busqueda
        /// </summary>
        /// <param name="coloniaViewModel"></param>
        /// <returns>Lista de colonias encontradas</returns>
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
        /// Obtener un diccionario de los estados existentes
        /// </summary>
        /// <returns>Diccionario de estados encontrados</returns>
        public Dictionary<int, string> ObtenerDiccionario()
        {
            return (from e in Contexto.Estado
                    select new { e.IDEstado, e.Nombre }).ToDictionary(e => e.IDEstado, e => e.Nombre);
        }

        public static List<SelectListItem> Buscar()
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from e in bd.Estado
                         orderby e.Nombre
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)e.IDEstado).Trim(),
                             Text = e.Nombre
                         }).ToList();

                lista.Insert(0, new SelectListItem
                {
                    Text = General.Seleccione,
                    Value = string.Empty
                });
            }
            return lista;
        }

        /// <summary>
        /// Busca un estado por su ID
        /// </summary>
        /// <param name="idEstado">ID del estado a Buscar</param>
        /// <returns></returns>
        public Estado BuscarPorId(int idEstado)
        {
            return (from c in Contexto.Estado
                    where c.IDEstado == idEstado
                    select c).FirstOrDefault();
        }
    }
}

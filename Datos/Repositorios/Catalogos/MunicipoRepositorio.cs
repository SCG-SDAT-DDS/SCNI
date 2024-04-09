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
    public class MunicipioRepositorio : Repositorio<Municipio>
    {
        public MunicipioRepositorio(Contexto contexto) : base(contexto)
        {

        }

        /// <summary>
        /// Busca municipios por criterios de busqueda
        /// </summary>
        /// <param name="municipioViewModel"></param>
        /// <returns>Lista de municipio encontrados</returns>
        public List<Municipio> Buscar(MunicipioViewModel municipioViewModel)
        {
            return ObtenerQuery(municipioViewModel, true).ToList();
        }
        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="municipioViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(MunicipioViewModel municipioViewModel)
        {
            return ObtenerQuery(municipioViewModel, false).Count();
        }

        public IQueryable<Municipio> ObtenerQuery(MunicipioViewModel criterios, bool paginar)
        {
            IQueryable<Municipio> query = Contexto.Set<Municipio>();

            if (!string.IsNullOrEmpty(criterios.Municipio.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(criterios.Municipio.Nombre));
            }
            if (criterios.Municipio.Estado != null && criterios.Municipio.Estado.IDEstado > 0)
            {
                query = query.Where(c => c.Estado.IDEstado == criterios.Municipio.Estado.IDEstado);
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            query = query.Include(c => c.Estado);
            //query = query.Include(c => c.Abreviatura);

            return query;
        }

        /// <summary>
        /// Busca Municipios con colonias de un Estado
        /// </summary>
        /// <param name="idEstado">ID del estado</param>
        /// <returns>Lista de Municipios encontrados</returns>
        public List<Municipio> BuscarMunicipiosConColoniasPorEstado(int idEstado)
        {
            return (from c in Contexto.Colonia
                    where c.IDEstado == idEstado
                    select c.Municipio).Distinct().OrderBy(m => m.Nombre).ToList();

        }

        /// <summary>
        /// Busca todos los municipios de un estado
        /// </summary>
        /// <param name="idEstado">ID del estado</param>
        /// <returns>Lista de municipios encontrados</returns>
        public List<Municipio> BuscarMunicipiosPorEstado(int idEstado)
        {
            return (from m in Contexto.Municipio
                    where m.IDEstado == idEstado
                    select m).OrderBy(m => m.Nombre).ToList();
        }

        public List<SelectListItem> Buscar(int idEstado)
        {
            return (from m in Contexto.Municipio
                    where m.IDEstado == idEstado
                    orderby m.Nombre
                    select new SelectListItem
                    {
                        Value = SqlFunctions.StringConvert((decimal?)m.IDMunicipio).Trim(),
                        Text = m.Nombre
                    }).ToList();
        }

        /// <summary>
        /// Busca un municipio por su ID
        /// </summary>
        /// <param name="idMunicipio">ID del municipio a Buscar</param>
        /// <param name="idEstado">ID del estado al que pertenece el municipio</param>
        /// <returns></returns>
        public Municipio BuscarPorId(int idMunicipio, int idEstado)
        {
            return (from c in Contexto.Municipio
                    where c.IDMunicipio == idMunicipio && c.IDEstado == idEstado
                    select c).Include(m => m.Estado).FirstOrDefault();
        }

        public static List<SelectListItem> BuscarPorEstado(int idEstado)
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from m in bd.Municipio
                         where m.IDEstado == idEstado
                         orderby m.Nombre
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)m.IDMunicipio).Trim(),
                             Text = m.Nombre
                         }).ToList();

                if (lista.Count > 0)
                {
                    lista.Insert(0, new SelectListItem { Text = General.Seleccione, Value = string.Empty });
                }
            }
            return lista;
        }

    }
}

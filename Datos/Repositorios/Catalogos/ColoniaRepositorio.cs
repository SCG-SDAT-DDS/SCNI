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

namespace Datos.Repositorios.Catalogos
{
    public class ColoniaRepositorio : Repositorio<Colonia>
    {
        public ColoniaRepositorio(Contexto contexto) : base(contexto)
        {
        }

        /// <summary>
        /// Busca las colonias de un municipio
        /// </summary>
        /// <param name="idMunicipio">ID del municipio</param>
        /// <param name="idEstado">ID del estado</param>
        /// <param name="buscar">Opcion a buscar</param>
        /// <returns>Lista de las colonias encontradas</returns>
        public List<SelectListItem> BuscarColoniasPorMunicipio(int idMunicipio, int idEstado, string buscar)
        {
            return (from c in Contexto.Colonia
                    where c.IDMunicipio == idMunicipio
                        && c.IDEstado == idEstado
                        && c.Nombre.Contains(buscar)
                    orderby c.Nombre
                    select new SelectListItem
                    {
                        Value = SqlFunctions.StringConvert((decimal?)c.IDColonia).Trim(),
                        Text = c.Nombre
                    }).ToList();
        }


        /// <summary>
        /// Busca colonias por criterios de busqueda
        /// </summary>
        /// <param name="coloniaViewModel"></param>
        /// <returns>Lista de colonias encontradas</returns>
        public List<Colonia> Buscar(ColoniaViewModel coloniaViewModel)
        {
            return ObtenerQuery(coloniaViewModel, true).ToList();
        }
        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="coloniaViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(ColoniaViewModel coloniaViewModel)
        {
            return ObtenerQuery(coloniaViewModel, false).Count();
        }

        public IQueryable<Colonia> ObtenerQuery(ColoniaViewModel criterios, bool paginar)
        {
            IQueryable<Colonia> query = Contexto.Set<Colonia>();

            query = query.Where(e => e.Habilitado == criterios.Colonia.Habilitado);

            if (!string.IsNullOrEmpty(criterios.Colonia.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(criterios.Colonia.Nombre));
            }
            if (criterios.Colonia.CodigoPostal > 0)
            {
                query = query.Where(c => c.CodigoPostal == criterios.Colonia.CodigoPostal);
            }
            if (criterios.Colonia.CodigoPostal > 0)
            {
                query = query.Where(c => c.CodigoPostal == criterios.Colonia.CodigoPostal);
            }
            if (criterios.Colonia.IDEstado > 0)
            {
                query = query.Where(c => c.IDEstado == criterios.Colonia.IDEstado);
            }
            if (criterios.Colonia.IDMunicipio > 0)
            {
                query = query.Where(c => c.IDMunicipio == criterios.Colonia.IDMunicipio);
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            query = query.Include(c => c.Estado);
            query = query.Include(c => c.Estado.Municipios);

            return query;
        }

        /// <summary>
        /// Busca una colonia por su ID
        /// </summary>
        /// <param name="idColonia">ID de colonia a Buscar</param>
        /// <returns></returns>
        public Colonia BuscarPorId(int idColonia)
        {
            return (from c in Contexto.Colonia.Include(x => x.Municipio.Estado)
                    where c.IDColonia == idColonia
                    select c).FirstOrDefault();
        }

        /// <summary>
        /// Busca las colonias de un municipio
        /// </summary>
        /// <param name="municipioId">ID del municipio</param>
        /// <returns>Lista de colonias encontradas</returns>
        public static List<SelectListItem> BuscarColoniasPorMunicipioId(int municipioId)
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from m in bd.Colonia
                         where m.IDMunicipio == municipioId
                         orderby m.Nombre ascending
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)m.IDColonia).Trim(),
                             Text = m.Nombre
                         }).ToList();

                if (lista.Count > 0)
                {
                    lista.Insert(0, new SelectListItem { Text = General.Seleccione, Value = string.Empty });
                }
            }
            return lista;
        }

        /// <summary>
        /// Cambia el habilitado de una colonia
        /// </summary>
        /// <param name="idColonia">ID de la colonia</param>
        /// <param name="habilitado">Opción habilitado</param>
        /// <returns>Verdadero si se guardo correctamente</returns>
        public bool CambiarHabilitado(int idColonia, bool habilitado)
        {
            var colonia = new Colonia { IDColonia = idColonia };

            Contexto.Colonia.Attach(colonia);
            colonia.Habilitado = habilitado;
            Contexto.Entry(colonia).Property(r => r.Habilitado).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        public static List<SelectListItem> BuscarColoniasPorMunicipioId2(int municipioId, int estadoId, string coloniaId)
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from m in bd.Colonia
                         where m.IDMunicipio== municipioId && m.IDEstado == estadoId
                         orderby m.Nombre ascending
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)m.IDColonia).Trim(),
                             Text = m.Nombre
                         }).ToList();

                if (lista.Count > 0)
                {
                    lista.First(i => i.Value == coloniaId); ;

                }
            }
            return lista;
        }

    }
}

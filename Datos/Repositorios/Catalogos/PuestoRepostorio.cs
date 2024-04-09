using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Recursos;

namespace Datos.Repositorios.Catalogos
{
    public class PuestoRepostorio : Repositorio<Puesto>
    {
        public PuestoRepostorio(Contexto contexto) : base(contexto)
        {
        }

        /// <summary>
        /// Busca todos los puestos habilitados
        /// </summary>
        /// <param name="habilitados">Opción habilitado</param>
        /// <returns>Lista de los puestos encontrados</returns>
        public List<Puesto> Buscar(bool habilitados)
        {
            return (from p in Contexto.Puesto
                    where p.Habilitado == habilitados
                    orderby p.Descripcion ascending
                    select p).ToList();
        }

        /// <summary>
        /// Busca los puestos parecidos a la opcion
        /// </summary>
        /// <param name="nombrePuesto">Opción de nombre del puesto</param>
        /// <param name="habilitado">Opción habilitado</param>
        /// <returns>Lista de los puestos encontrados</returns>
        public List<Puesto> Buscar(string nombrePuesto, bool habilitado)
        {
            return (from p in Contexto.Puesto
                    where p.Nombre.Contains(nombrePuesto) &&
                          p.Habilitado == habilitado
                    orderby p.Nombre ascending
                    select p).ToList();
        }

        /// <summary>
        /// Cambia el habilitado de un puesto
        /// </summary>
        /// <param name="idPuesto">ID del puesto</param>
        /// <param name="habilitado">Opción habilitado</param>
        /// <returns>Verdadero si se guardo correctamente</returns>
        public bool CambiarHabilitado(int idPuesto, bool habilitado)
        {
            var puestoCambiarHabilitado = new Puesto { IDPuesto = idPuesto };

            Contexto.Puesto.Attach(puestoCambiarHabilitado);
            puestoCambiarHabilitado.Habilitado = habilitado;
            Contexto.Entry(puestoCambiarHabilitado).Property(r => r.Habilitado).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Comprueba si el nombre existe
        /// </summary>
        /// <param name="puesto"></param>
        /// <returns></returns>
        public int ComprobarNombreExistente(string puesto)
        {
            return (from p in Contexto.Puesto
                    where p.Nombre == puesto
                    select p).Count();
        }

        /// <summary>
        /// Busca puestos por criterios de busqueda
        /// </summary>
        /// <param name="puesto"></param>
        /// <returns></returns>
        public List<Puesto> Buscar(Puesto puesto)
        {
            IQueryable<Puesto> query = Contexto.Set<Puesto>();

            if (!string.IsNullOrEmpty(puesto.Nombre))
            {
                query = query.Where(p => p.Nombre.Contains(puesto.Nombre));
            }

            query = query.OrderBy(q => q.Nombre);
            query = query.Where(p => p.Habilitado == puesto.Habilitado);

            return query.ToList();
        }

        /// <summary>
        /// Busca colonias por criterios de busqueda
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Lista de colonias encontradas</returns>
        public List<Puesto> Buscar(PuestoViewModel viewModel)
        {
            return ObtenerQuery(viewModel, true).ToList();
        }

        /// <summary>
        /// Obtiene todas las sucursales habilitadas
        /// </summary>
        /// <returns>Select list item</returns>
        public static List<SelectListItem> Buscar()
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from e in bd.Puesto
                         where e.Habilitado
                         orderby e.Nombre
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)e.IDPuesto).Trim(),
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
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(PuestoViewModel viewModel)
        {
            return ObtenerQuery(viewModel, false).Count();
        }

        public IQueryable<Puesto> ObtenerQuery(PuestoViewModel criterios, bool paginar)
        {
            IQueryable<Puesto> query = Contexto.Set<Puesto>();

            query = query.Where(c => c.Habilitado == criterios.Puesto.Habilitado);

            if (!string.IsNullOrEmpty(criterios.Puesto.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(criterios.Puesto.Nombre));
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            return query;
        }

        //public List<Puesto> BuscarPuestosSinResponsable()
        //{
        //        return (from p in Contexto.Puestos
        //                where p.IDEntidadOrganigrama == null
        //                select p).ToList();
        //}
    }
}

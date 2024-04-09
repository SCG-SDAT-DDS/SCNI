using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Web.Mvc;
using System.Linq;
using Datos.Recursos;

namespace Datos.Repositorios.Configuracion
{
    public class RolRepositorio : Repositorio<Rol>
    {
        public RolRepositorio(Contexto contexto) 
            : base(contexto)
        {

        }


        public bool CambiarHabilitado(int idRol, bool habilitado)
        {
            var rol = new Rol { IDRol = idRol };

            Contexto.Rol.Attach(rol);
            rol.Habilitado = habilitado;
            Contexto.Entry(rol).Property(o => o.Habilitado).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        public new bool Modificar(Rol rol)
        {
            var rolModificar = new Rol { IDRol = rol.IDRol };

            Contexto.Rol.Attach(rolModificar);

            rolModificar.Nombre = rol.Nombre;
            rolModificar.Descripcion = rol.Descripcion;
            rolModificar.PaginaInicio = rol.PaginaInicio;
            rolModificar.TipoInicio = rol.TipoInicio;

            return Contexto.SaveChanges() == 1;
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
                lista = (from e in bd.Rol
                         where e.Habilitado
                         orderby e.Nombre
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)e.IDRol).Trim(),
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

        public string ObtenerNombrePorId(int idRol)
        {
            return (from r in Contexto.Rol
                    where r.IDRol == idRol
                    select r.Nombre).SingleOrDefault();
        }

        /// <summary>
        /// Busca un rol por su id.
        /// </summary>
        /// <param name="idRol">Id del rol a buscar.</param>
        /// <returns>Objeto rol encontrado.</returns>
        public Rol BuscarPorId(int idRol)
        {
            return (from om in Contexto.Rol
                    where om.IDRol == idRol
                    select om).FirstOrDefault();
        }
    }
}

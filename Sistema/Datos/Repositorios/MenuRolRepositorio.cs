using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios
{
    public class MenuRolRepositorio
    {
        /// <summary>
        /// Inserta opciones al menu de un rol.
        /// </summary>
        /// <param name="idRol">Id del rol al cual se le insertaran las opciones del menu.</param>
        /// <param name="idOpcionesMenuInsertar">Lista con los id de las opciones del menu que se le intertaran.</param>
        /// <returns>Indica si se insertaron correctamente.</returns>
        public bool Insertar(int idRol, List<int> idOpcionesMenuInsertar)
        {
            var rolAgregarOpciones = new Rol { IDRol = idRol };

            using (var bd = new Contexto())
            {

                bd.Rol.Attach(rolAgregarOpciones);

                var menusSinAcceso = idOpcionesMenuInsertar.Where(idMenu => !PoseeOpcionMenu(idMenu, idRol))
                    .Select(m => new Menu() { IDMenu = m }).ToList();

                foreach (var menu in menusSinAcceso)
                {
                    bd.Menu.Attach(menu);
                    rolAgregarOpciones.MenuInicio.Add(menu);
                }

                return bd.SaveChanges() >= 1;

            }
        }

        /// <summary>
        /// Elimina las opciones del menu de un rol.
        /// </summary>
        /// /// <param name="idRol">Id del rol al cual se le eliminaran las opciones del menu.</param>
        /// <param name="idOpcionesMenuEliminar">Lista con los id de las opciones del menu que se le eliminaran.</param>
        /// <returns>Indica si fue posible eliminarlos.</returns>
        public bool Eliminar(int idRol, List<int> idOpcionesMenuEliminar)
        {
            var rolQuitarOpciones = new Rol { IDRol = idRol };

            using (var bd = new Contexto())
            {

                var menusConAcceso = idOpcionesMenuEliminar.Where(idMenu => PoseeOpcionMenu(idMenu, idRol))
                    .Select(m => new Menu() { IDMenu = m }).ToList();

                menusConAcceso.ForEach(m => rolQuitarOpciones.MenuInicio.Add(new Menu { IDMenu = m.IDMenu }));
                bd.Rol.Attach(rolQuitarOpciones);

                foreach (var menu in menusConAcceso)
                {
                    var opcionQuitar = rolQuitarOpciones.MenuInicio.Single(o => o.IDMenu == menu.IDMenu);
                    bd.Menu.Attach(opcionQuitar);
                    rolQuitarOpciones.MenuInicio.Remove(opcionQuitar);
                }

                return bd.SaveChanges() >= 1;

            }
        }

        /// <summary>
        /// Busca el menu de un rol.
        /// </summary>
        /// <param name="idRol">Id del rol del cual se buscara su menu.</param>
        /// <param name="soloVisibles">Indica si solo se traeran las opciones menu visibles</param>
        /// <returns>Menu del rol.</returns>
        public List<Menu> BuscarMenu(int idRol, bool soloVisibles)
        {
            using (var bd = new Contexto())
            {

                var opcionesMenu =
                    (from om in bd.Menu
                     where om.Rol.Any(r => r.IDRol == idRol) &&
                           om.Habilitado &&
                           om.Visible == soloVisibles
                     orderby om.Indice
                     //orderby (
                     //           om.Opcion.Contains("Inicio") ? 1 :
                     //           om.Opcion == "Gestión" ? 2 :
                     //           om.Opcion == "Catálogo" ? 3 : 4
                     //        ), om.Opcion, om.Camino, om.Padre
                     select om).ToList();

                return opcionesMenu;
            }
        }

        /// <summary>
        /// Busca el menu de un rol.
        /// </summary>
        /// <param name="idRol">Id del rol del cual se buscara su menu.</param>
        /// <returns>Menu del rol.</returns>
        public List<Menu> BuscarMenu(int idRol)
        {
            using (var bd = new Contexto())
            {
                var opcionesMenu =
                    (from om in bd.Menu
                     where om.Rol.Any(r => r.IDRol == idRol) &&
                           om.Habilitado
                     orderby (
                                om.Opcion.Contains("Inicio") ? 1 :
                                om.Opcion == "Gestión" ? 2 :
                                om.Opcion == "Catálogo" ? 3 : 4
                             ), om.Opcion, om.Camino, om.Padre
                     select om).ToList();

                return opcionesMenu;
            }
        }

        /// <summary>
        /// Busca opciones hijos del menu por el id de la opcion padre e indica cuales posee y no posee un rol.
        /// </summary> 
        /// <param name="idPadreOpcionMenu">Id de la opcion padre en el cual se buscara.</param>
        /// <param name="idRol">Id del rol para buscar si posee o no posee la opcion del menu.</param>
        /// <returns>Lista con las opciones hijos de menu
        ///  e indica si lo posee el rol.</returns>
        public List<Menu> BuscarPoseeOpcionMenu(int idPadreOpcionMenu, int idRol)
        {
            using (var bd = new Contexto())
            {
                return (from om in bd.Menu
                        where om.Padre == idPadreOpcionMenu &&
                              om.Habilitado
                        select om).ToList();

            }
        }

        /// <summary>
        /// Busca por su nombre opciones hijos del menu por el id de la opcion padre e indica cuales posee y no posee un rol.
        /// </summary> 
        /// <param name="idPadreMenu">Id de la opcion padre en el cual se buscara.</param>
        /// <param name="nombreOpcion">Nombre de la opcion a buscar.</param>
        /// <param name="idRol">Id del rol para buscar si posee o no posee la opcion del menu.</param>
        /// <returns>Lista con las opciones hijos de menu e indica si lo posee el rol.</returns>
        public List<Menu> BuscarPoseeOpcionMenu(int idPadreMenu, string nombreOpcion, int idRol)
        {
            using (var bd = new Contexto())
            {
                return (from om in bd.Menu
                        where om.Padre == idPadreMenu &&
                              om.Opcion.Contains(nombreOpcion) &&
                              om.Habilitado
                        select om).ToList();
            }
        }

        /// <summary>
        /// Comprueba si el rol posee el menu
        /// </summary>
        /// <param name="idOpcionMenu"></param>
        /// <param name="idRol"></param>
        /// <returns>Verdadero si el rol posee el menu</returns>
        public bool PoseeOpcionMenu(int idOpcionMenu, int idRol)
        {
            var rol = new Rol { IDRol = idRol };

            using (var bd = new Contexto())
            {
                bd.Rol.Attach(rol);

                var poseeMenuRol = (from om in bd.Menu
                                    where om.Rol.Any(x => x.IDRol == idRol) &&
                                          om.IDMenu == idOpcionMenu &&
                                          om.Habilitado
                                    select om);

                return poseeMenuRol.Any();

            }
        }

        /// <summary>
        /// Indica si un rol tiene permiso a una pagina(opcion de menu).
        /// </summary>
        /// <param name="idRol">Id del rol a verificar.</param>
        /// <param name="direccionPagina">Direccion de la pagina a la que se verificara si tiene permiso.</param>
        /// <returns>Indica si el rol tiene permiso a la pagina.</returns>
        public bool TienePermisoPagina(int idRol, string direccionPagina)
        {
            using (var bd = new Contexto())
            {
                return (from r in bd.Rol
                        where r.IDRol == idRol &&
                              r.MenuInicio.Any(om => om.Destino.Contains(direccionPagina))
                        select r
                       ).Any();

            }
        }

        /// <summary>
        /// Indica si un rol tiene permiso a una pagina(opcion de menu).
        /// </summary>
        /// <param name="idRol">Id del rol a verificar.</param>
        /// <param name="controlador"></param>
        /// <param name="accion"></param>
        /// <returns>Indica si el rol tiene permiso a la pagina.</returns>
        public bool TienePermisoPagina(int idRol, string controlador, string accion)
        {
            using (var bd = new Contexto())
            {
                return (from r in bd.Rol
                        where r.IDRol == idRol &&
                              r.MenuInicio.Any(om => om.Controlador == controlador && om.Accion == accion)
                        select r
                       ).Any();
            }
        }

        /// <summary>
        /// Buscar el permiso para guardar al cliente
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public bool BuscarPermisoGuardarCliente(int idRol)
        {
            using (var bd = new Contexto())
            {
                return (from p in bd.Rol
                        where p.IDRol == idRol
                        && p.MenuInicio.Any(pr => pr.IDMenu == 48)
                        select p).Any();

            }
        }
    }
}

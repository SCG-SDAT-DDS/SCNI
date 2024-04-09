using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios
{
    public class MenuUsuarioRepositorio
    {
        /// <summary>
        /// Busca el menu de una Empleado.
        /// </summary>
        /// <param name="idUsuario">Id de la Empleado del cual se buscara su menu.</param>
        /// <param name="soloVisibles">Indica si solo se traeran las opciones menu visibles</param>
        /// <returns>Menu de la Empleado.</returns>
        public List<Menu> BuscarMenu(int idUsuario, bool soloVisibles)
        {
            using (var bd = new Contexto())
            {
                return (from om in bd.Menu
                        where om.Usuario.Any(p => p.IDUsuario == idUsuario) &&
                              om.Habilitado && om.Visible == soloVisibles
                        orderby om.Indice ascending
                        select om).ToList();
            }
        }

        /// <summary>
        /// Busca el menu de una Empleado.
        /// </summary>
        /// <param name="idUsuario">Id de la Empleado del cual se buscara su menu.</param>
        /// <returns>Menu de la Empleado.</returns>
        public List<Menu> BuscarMenu(int idUsuario)
        {
            using (var bd = new Contexto())
            {
                return (from om in bd.Menu
                        where om.Usuario.Any(p => p.IDUsuario == idUsuario) &&
                              om.Habilitado
                        select om).ToList();
            }
        }

        /// <summary>
        /// Busca opciones hijos del menu por el id de la opcion padre e indica cuales posee y no posee una Empleado.
        /// </summary> 
        /// <param name="idPadreOpcionMenu">Id de la opcion padre en el cual se buscara.</param>
        /// <param name="idEmpleado">Id de la Empleado para buscar si posee o no posee la opcion del menu.</param>
        /// <returns>ista con las opciones hijos de menu e indica si lo posee la Empleado.</returns>
        public List<Menu> BuscarOpcionMenuPadrePosee(int idPadreOpcionMenu, int idEmpleado)
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
        /// Busca por su nombre opciones hijos del menu por el id de la opcion padre e indica cuales posee y no posee una Empleado.
        /// </summary> 
        /// <param name="idPadreOpcionMenu">Id de la opcion padre en el cual se buscara.</param>
        /// <param name="nombreOpcion">Nombre de la opcion a buscar.</param>
        /// <param name="idEmpleado">Id de la Empleado para buscar si posee o no posee la opcion del menu.</param>
        /// <returns>ista con las opciones hijos de menu e indica si lo posee la Empleado.</returns>
        public List<Menu> BuscarOpcionMenuPadrePosee(int idPadreOpcionMenu, string nombreOpcion, int idEmpleado)
        {
            using (var bd = new Contexto())
            {
                return (from om in bd.Menu
                        where om.Padre == idPadreOpcionMenu &&
                              om.Opcion.Contains(nombreOpcion) &&
                              om.Habilitado
                        select om).ToList();
            }
        }

        /// <summary>
        /// Copia el menu de un rol a una Empleado y elimina el menu actual de la Empleado.
        /// </summary>
        /// <param name="idEmpleado">Id de la Empleado a la cual se copiara y eliminara su menu actual.</param>
        /// <param name="idRol">Id del rol del cual se copiara su meno.</param>
        /// <returns>Indica si fue posible copiar el menu de rol a la Empleado.</returns>
        public bool CopiarMenuDeRol(int idEmpleado, int idRol)
        {
            using (var bd = new Contexto())
            {
                return bd.CopiarMenuDeRolEnEmpleado(idEmpleado, idRol).FirstOrDefault() > 0;
            }
        }

        /// <summary>
        /// Indica si una Empleado tiene permiso a una pagina(opcion de menu).
        /// </summary>
        /// <param name="idUsuario">Id de la Empleado a verificar.</param>
        /// <param name="direccionPagina">Direccion de la pagina a la que se verificara si tiene permiso.</param>
        /// <returns>Indica si la Empleado tiene permiso a la pagina.</returns>
        public bool TienePermisoPagina(int idUsuario, string direccionPagina)
        {
            using (var bd = new Contexto())
            {
                return (from p in bd.Usuario
                        where p.IDUsuario == idUsuario &&
                              p.Menu.Any(om => om.Destino.Contains(direccionPagina))
                        select p
                       ).Any();
            }
        }

        public bool BuscarPoseeOpcionMenu(int idOpcionMenu, int idEmpleado)
        {
            using (var bd = new Contexto())
            {
                return (from om in bd.Menu
                        where om.IDMenu == idOpcionMenu &&
                            om.Usuario.Any(u => u.IDUsuario == idEmpleado) &&
                              om.Habilitado
                        select om).Any();
            }
        }

        /// <summary>
        /// Inserta opciones al menu de un rol.
        /// </summary>
        /// <param name="idUsuario">Id del usuario al cual se le insertaran las opciones del menu.</param>
        /// <param name="idOpcionesMenu">Lista con los id de las opciones del menu que se le intertaran.</param>
        /// <returns>Indica si se insertaron correctamente.</returns>
        public bool Insertar(int idUsuario, List<int> idOpcionesMenu)
        {
            var usuario = new Usuario { IDUsuario = idUsuario };

            using (var bd = new Contexto())
            {

                bd.Usuario.Attach(usuario);

                var menusSinAcceso = idOpcionesMenu.Where(idMenu => !BuscarPoseeOpcionMenu(idMenu, idUsuario))
                    .Select(m => new Menu() { IDMenu = m }).ToList();

                foreach (var menu in menusSinAcceso)
                {
                    bd.Menu.Attach(menu);
                    usuario.Menu.Add(menu);
                }

                return bd.SaveChanges() >= 1;

            }
        }

        /// <summary>
        /// Elimina las opciones del menu de un rol.
        /// </summary>
        /// /// <param name="idUsuario">Id del rol al cual se le eliminaran las opciones del menu.</param>
        /// <param name="idOpcionesMenu">Lista con los id de las opciones del menu que se le eliminaran.</param>
        /// <returns>Indica si fue posible eliminarlos.</returns>
        public bool Eliminar(int idUsuario, List<int> idOpcionesMenu)
        {
            var usuario = new Usuario { IDUsuario = idUsuario };

            using (var bd = new Contexto())
            {

                var menusConAcceso = idOpcionesMenu.Where(idMenu => BuscarPoseeOpcionMenu(idMenu, idUsuario))
                    .Select(m => new Menu() { IDMenu = m }).ToList();

                menusConAcceso.ForEach(m => usuario.Menu.Add(new Menu { IDMenu = m.IDMenu }));
                bd.Usuario.Attach(usuario);

                foreach (var menu in menusConAcceso)
                {
                    var opcionQuitar = usuario.Menu.Single(o => o.IDMenu == menu.IDMenu);
                    bd.Menu.Attach(opcionQuitar);
                    usuario.Menu.Remove(opcionQuitar);
                }

                return bd.SaveChanges() >= 1;

            }
        }

        public bool AgregarOpcionMenu(int idUsuario, List<int> idOpcionesMenu)
        {
            var usuario = new Usuario { IDUsuario = idUsuario };

            using (var bd = new Contexto())
            {
                bd.Usuario.Attach(usuario);

                foreach (var opcionAgregar in idOpcionesMenu.Select(idOpcion => new Menu { IDMenu = idOpcion }))
                {
                    bd.Menu.Attach(opcionAgregar);
                    usuario.Menu.Add(opcionAgregar);
                }
                return bd.SaveChanges() == 1;
            }
        }

        public bool QuitarOpcionMenu(int idEmpleado, List<int> idOpcionesMenu)
        {
            var usuario = new Usuario { IDUsuario = idEmpleado };

            idOpcionesMenu.ForEach(id => usuario.Menu.Add(new Menu { IDMenu = id }));

            using (var bd = new Contexto())
            {
                bd.Usuario.Attach(usuario);

                foreach (var opcionQuitar in idOpcionesMenu.Select(idOpcion => usuario.Menu.Single(o => o.IDMenu == idOpcion)))
                {
                    bd.Menu.Attach(opcionQuitar);
                    usuario.Menu.Remove(opcionQuitar);
                }

                return bd.SaveChanges() == 1;
            }
        }

        public bool TieneMenuPersonalizado(int idUsuario)
        {
            using (var bd = new Contexto())
            {
                return (from p in bd.Usuario
                        where p.IDUsuario == idUsuario
                        select p.MenuPersonalizado).FirstOrDefault();
            }
        }
    }
}

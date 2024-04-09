using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios.Configuracion
{
    public class MenuRepositorio : Repositorio<Menu>
    {
        public MenuRepositorio(Contexto contexto) 
            : base(contexto)
        {

        }

        /// <summary>
        /// Inserta una nueva opcion al menu.
        /// </summary>
        /// <param name="menuInsertar">Objeto opcion del menu a insertar.</param>
        /// <returns>Indica si se inserto correctamente.</returns>
        public new bool Guardar(Menu menuInsertar)
        {
            Contexto.Menu.Add(menuInsertar);
            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Modifica una opcion del menu.
        /// </summary>
        /// <param name="menuCambios">Objeto opcion del menu con los nuevos cambios.</param>
        /// <returns>Indica si se fue posible modificar la opcion del menu.</returns>
        public new bool Modificar(Menu menuCambios)
        {
            var menuModificar = new Menu { IDMenu = menuCambios.IDMenu };

            Contexto.Menu.Attach(menuModificar);

            menuModificar.Opcion = menuCambios.Opcion;
            menuModificar.Destino = menuCambios.Destino;
            menuModificar.Visible = menuCambios.Visible;
            menuModificar.Camino = menuCambios.Camino;
            menuModificar.IconoCss = menuCambios.IconoCss;
            menuModificar.Controlador = menuCambios.Controlador;
            menuModificar.Accion = menuCambios.Accion;
            menuModificar.Area = menuCambios.Area;

            Contexto.Entry(menuModificar).Property(o => o.Visible).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Elimina una opcion del menu por su id.
        /// </summary>
        /// <param name="idMenu">Id de la opcion del menu a eliminar.</param>
        /// <returns>Indica si fue posible eliminar la opcion del menu.</returns>
        public bool Eliminar(int idMenu)
        {
            var menuEliminar = new Menu { IDMenu = (short)idMenu };

            Contexto.Menu.Attach(menuEliminar);
            Contexto.Menu.Remove(menuEliminar);
            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Habilita/deshabilita una opcion del menu por su id.
        /// </summary>
        /// <param name="idMenu">Id de la opcion del menu a habilitar.</param>
        /// <param name="habilitado">Habilitar/deshabilitar la opcion del menu.</param>
        /// <returns>Indica si fue posible habilitar/deshabilitar la opcion del menu.</returns>
        public bool CambiarHabilitado(int idMenu, bool habilitado)
        {
            var opcionCambiarHabilitado = new Menu { IDMenu = (short)idMenu };

            Contexto.Menu.Attach(opcionCambiarHabilitado);
            opcionCambiarHabilitado.Habilitado = habilitado;
            Contexto.Entry(opcionCambiarHabilitado).Property(o => o.Habilitado).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Busca una opcion del menu por su id.
        /// </summary>
        /// <param name="idMenu">Id de la opcion del menu a buscar.</param>
        /// <returns>Objeto opcion del menu encontrado.</returns>
        public Menu BuscarPorId(int idMenu)
        {
            return (from om in Contexto.Menu
                    where om.IDMenu == idMenu
                    select om).FirstOrDefault();
        }

        /// <summary>
        /// Busca opciones del menu indicando si deben de estar habilitados/deshabilitados.
        /// </summary>
        /// <param name="habilitados">Indica si las opciones del menu deben de estar habilitados/deshabilitados.</param>
        /// <returns>Lista con las opciones del menu encontradas.</returns>
        public List<Menu> Buscar(bool habilitados)
        {
            return (from om in Contexto.Menu
                    where om.Habilitado == habilitados
                    orderby om.Opcion
                    select om).ToList();
        }

        /// <summary>
        /// Busca opciones del menu indicando si deben de estar habilitados/deshabilitados.
        /// </summary>
        /// <returns>Lista con las opciones del menu encontradas.</returns>
        public static List<SelectListItem> Buscar()
        {
            using (var dc = new Contexto())
            {
                return (from om in dc.Menu
                        where om.Habilitado &&
                            om.Padre == 0
                        orderby om.Opcion
                        select new SelectListItem
                        {
                            Text = om.Opcion,
                            Value = SqlFunctions.StringConvert((decimal)om.IDMenu).Trim()
                        }).ToList();
            }
        }

        /// <summary>
        /// Busca opciones hijas de una opcion padre del menu indicando si deben de estar habilitados/deshabilitados.
        /// </summary>
        /// <param name="idPadreMenu">Id de la opcion padre en el cual se buscara.</param>
        /// <param name="habilitados">Indica si las opciones del menu deben de estar habilitados/deshabilitados.</param>
        /// <returns>Lista con las opciones del menu encontradas.</returns>
        public List<Menu> Buscar(int idPadreMenu, bool habilitados)
        {
            return (from om in Contexto.Menu
                    where om.Padre == idPadreMenu &&
                          om.Habilitado == habilitados
                    orderby om.Opcion
                    select om).ToList();
        }

        /// <summary>
        /// Busca por el nombre de las opciones hijos por el id de la opcion padre e indicando si estan habilitados/deshabilitados
        /// </summary>
        /// <param name="idPadreMenu">Id de la opcion padre en el cual se buscara</param>
        /// <param name="nombreOpcion">Nombre de la opcion a buscar.</param>
        /// <param name="habilitados">Indica si las opciones del menu deben de estar habilitados/deshabilitados.</param>
        /// <returns>Lista con las opciones del menu encontradas.</returns>
        public List<Menu> Buscar(int idPadreMenu, string nombreOpcion, bool habilitados)
        {
            return (from om in Contexto.Menu
                    where om.Padre == idPadreMenu &&
                          om.Opcion.Contains(nombreOpcion) &&
                          om.Habilitado == habilitados
                    orderby om.Opcion
                    select om).ToList();
        }

        /// <summary>
        /// Obtiene el id del menú
        /// </summary>
        /// <param name="controlador"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public int ObtenerId(string controlador, string accion)
        {
            return
                (from m in Contexto.Menu
                 where m.Controlador == controlador && m.Accion == accion
                 select m.IDMenu
                ).FirstOrDefault();
        }

        /// <summary>
        /// Busca una opcion menu por su id y las opciones de menu superiores a el.
        /// </summary>
        /// <param name="idOpcionMenu">Id de la opcion del menu que se buscara y apartir del cual se buscara las opciones de menu superiores.</param>
        /// <returns>Opciones de menu encontrados</returns>
        public List<Menu> BuscarOpcionMenuYOpcionesSuperiores(int idOpcionMenu)
        {
            return (from om in Contexto.BuscarOpcionMenuAndOpcionesMenuSuperiores(idOpcionMenu)
                    select new Menu
                    {
                        IDMenu = om.IDMenu ?? 0,
                        Opcion = om.Opcion ?? string.Empty,
                        Controlador = om.Controlador,
                        Accion = om.Accion,
                        Area = om.Area,
                        Destino = om.Destino
                    }).ToList();
            //return null;
        }

        /// <summary>
        /// Busca una opcion menu por su id y las opciones de menu inferiores a el.
        /// </summary>
        /// <param name="idOpcionMenu">Id de la opcion del menu que se buscara y apartir del cual se buscara las opciones de menu inferiores.</param>
        /// <returns>Opciones de menu encontrados</returns>
        public List<Menu> BuscarOpcionMenuYOpcionesInferiores(int idOpcionMenu)
        {
            return (from om in Contexto.BuscarOpcionMenuAndOpcionesMenuInferiores(idOpcionMenu)
                    select new Menu
                    {
                        IDMenu = om.IDMenu ?? 0,
                        Opcion = om.Opcion ?? string.Empty
                    }).ToList();
            //return null;
        }

        /// <summary>
        /// Busca si una opcion de menu tiene opciones inferiores habilitados.
        /// </summary>
        /// <param name="idMenu">Id de la opcion de menu a buscar si tiene opciones inferiores habilitados.</param>
        /// <returns>Indica si tiene opciones de menu habilitados</returns>
        public bool TieneOpcionesMenuInferiores(int idMenu)
        {
            return (from om in Contexto.Menu
                    where om.Padre == idMenu &&
                          om.Habilitado
                    select om).Any();
        }

        /// <summary>
        /// Busca el camino de una opcion de menu por su id.
        /// </summary>
        /// <param name="idMenu">Id de la opcion de menu a buscar su camino.</param>
        /// <returns>Camino de la opcion de menu encontrado.</returns>
        public string BuscarCaminoPorId(int idMenu)
        {
            return (from om in Contexto.Menu
                    where om.IDMenu == idMenu
                    select om.Camino).FirstOrDefault();
        }

        /// <summary>
        /// Comprueba si un nombre existe
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public int ComprobarNombreExistente(string nombre)
        {

            return (from om in Contexto.Menu
                    where om.Opcion == nombre
                    select om.Camino).Count();

        }
    }
}

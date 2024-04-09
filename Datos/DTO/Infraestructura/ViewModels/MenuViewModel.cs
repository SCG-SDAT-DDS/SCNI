using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema;
using Datos.Enums;
using Datos;
using Datos.Repositorios;
using Datos.Repositorios.Configuracion;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class MenuViewModel
    {
        public int IdPadre { get; set; }
        public int RaizArbol { get; set; }
        public bool Habilitado { get; set; }
        public bool EsMenuPersonalizado { get; set; }
        public Menu Menu { get; set; }
        public List<Menu> MenuTabla { get; set; }
        public List<Menu> MenuTreeView { get; set; }
        public List<Breadcrumb> MenuBreadcrum { get; set; }

        public Enumerados.GestionMenu TipoGestion;
        public int IdGestionarMenu;
        public string NombreTipoGestion;

        private MenuRepositorio _menuRepositorio;
        private MenuRolRepositorio _menuRolRepositorio;
        private MenuUsuarioRepositorio _menuUsuarioRepositorio;

        public MenuViewModel()
        {
        }

        public MenuViewModel(Enumerados.GestionMenu gestionMenu)
        {
            TipoGestion = gestionMenu;
        }

        public MenuViewModel(int idRol, int idUsuario, int idPadreMenu, string buscar,
            bool habilitado, int idGestionMenu, Enumerados.GestionMenu tipoGestion, string nombreTipoGestion)
        {
            _menuUsuarioRepositorio = new MenuUsuarioRepositorio();

            if (idGestionMenu == 0)
            {
                EstablecerTipoGestion(idRol, idUsuario);
            }
            else
            {
                TipoGestion = tipoGestion;
                IdGestionarMenu = idGestionMenu;
                NombreTipoGestion = nombreTipoGestion;
            }

            using (var bd = new Contexto())
            {
                _menuRepositorio = new MenuRepositorio(bd);
                MenuTreeView = _menuRepositorio.Buscar(true);
                MenuTabla = ObtenerOpcionesMenu(idPadreMenu, buscar, habilitado);
                RaizArbol = 0;
            }
            MenuBreadcrum = ObtenerBreadcrumbs(idPadreMenu);
            Habilitado = habilitado;
            EsMenuPersonalizado = (IdGestionarMenu != 0)?_menuUsuarioRepositorio.TieneMenuPersonalizado(IdGestionarMenu) : _menuUsuarioRepositorio.TieneMenuPersonalizado(idUsuario);
        }

        public bool AgregarOpcionMenu(int idGestion, int idMenuAgregar)
        {
            bool isAgrego;
            List<Menu> listaMenu;

            using (var bd = new Contexto())
            {
                _menuRepositorio = new MenuRepositorio(bd);
                listaMenu = _menuRepositorio.BuscarOpcionMenuYOpcionesSuperiores(idMenuAgregar);
            }

            switch (TipoGestion)
            {
                case Enumerados.GestionMenu.Rol:
                    _menuRolRepositorio = new MenuRolRepositorio();
                    isAgrego = _menuRolRepositorio.Insertar(idGestion, listaMenu.Select(x => x.IDMenu).ToList());
                    break;

                case Enumerados.GestionMenu.Usuario:
                    _menuUsuarioRepositorio = new MenuUsuarioRepositorio();
                    //isAgrego = _menuUsuarioRepositorio.AgregarOpcionMenu(idGestion, listaMenu.Select(x => x.IDMenu).ToList());
                    isAgrego = _menuUsuarioRepositorio.Insertar(idGestion, listaMenu.Select(x => x.IDMenu).ToList());
                    break;

                default:
                    isAgrego = false;
                    break;
            }

            return isAgrego;

        }
        public bool QuitarOpcionMenu(int id, int idOpcionMenuQuitar)
        {
            bool isQuito;
            List<Menu> listaMenu;

            using (var bd = new Contexto())
            {
                _menuRepositorio = new MenuRepositorio(bd);
                listaMenu = _menuRepositorio.BuscarOpcionMenuYOpcionesInferiores(idOpcionMenuQuitar);
            }

            switch (TipoGestion)
            {
                case Enumerados.GestionMenu.Rol:
                    _menuRolRepositorio = new MenuRolRepositorio();
                    isQuito = _menuRolRepositorio.Eliminar(id, listaMenu.Select(x => x.IDMenu).ToList());
                    break;

                case Enumerados.GestionMenu.Usuario:
                    _menuUsuarioRepositorio = new MenuUsuarioRepositorio();
                    //isQuito = _menuUsuarioRepositorio.QuitarOpcionMenu(id, listaMenu.Select(x => x.IDMenu).ToList());
                    isQuito = _menuUsuarioRepositorio.Eliminar(id, listaMenu.Select(x => x.IDMenu).ToList());
                    break;

                default:
                    isQuito = false;
                    break;
            }

            return isQuito;
        }
        public List<Breadcrumb> ObtenerBreadcrumbs(int idMenu)
        {
            var listaBreadcrumb = new List<Breadcrumb>();
            using (var bd = new Contexto())
            {
                _menuRepositorio = new MenuRepositorio(bd);
                var menus = _menuRepositorio.BuscarOpcionMenuYOpcionesSuperiores(idMenu);

                listaBreadcrumb.Add(new Breadcrumb { Nombre = "Raíz", Id = 0 });

                listaBreadcrumb.AddRange(menus.Select(menu => new Breadcrumb
                {
                    Nombre = menu.Opcion,
                    Id = menu.IDMenu,
                    IdPadre = menu.Padre
                }));
            }
            return listaBreadcrumb;
        }


        private List<Menu> ObtenerOpcionesMenu(int idPadreMenu, string nombreMenu, bool habilitado)
        {
            List<Menu> opcionesMenu;
            var menuRolRepositorio = new MenuRolRepositorio();
            var esNombreVacio = string.IsNullOrWhiteSpace(nombreMenu);

            switch (TipoGestion)
            {
                case Enumerados.GestionMenu.Menu:
                    opcionesMenu = esNombreVacio
                                       ? _menuRepositorio.Buscar(idPadreMenu, habilitado)
                                       : _menuRepositorio.Buscar(idPadreMenu, nombreMenu, habilitado);
                    break;
                case Enumerados.GestionMenu.Rol:

                    opcionesMenu = esNombreVacio
                                       ? menuRolRepositorio.BuscarPoseeOpcionMenu(idPadreMenu, IdGestionarMenu)
                                       : menuRolRepositorio.BuscarPoseeOpcionMenu(idPadreMenu, nombreMenu,
                                                                          IdGestionarMenu);
                    break;
                case Enumerados.GestionMenu.Usuario:
                    int idRolUsuario;
                    using (var bd = new Contexto())
                    {
                        var usuarioModelo = new UsuarioRepositorio(bd);
                        idRolUsuario = usuarioModelo.ObtenerIdRolPorIdUsuario(IdGestionarMenu);
                    }
                    opcionesMenu = _menuUsuarioRepositorio.TieneMenuPersonalizado(IdGestionarMenu)
                                   ? esNombreVacio
                                       ? _menuUsuarioRepositorio.BuscarOpcionMenuPadrePosee(idPadreMenu, IdGestionarMenu)
                                       : _menuUsuarioRepositorio.BuscarOpcionMenuPadrePosee(idPadreMenu, nombreMenu, IdGestionarMenu)
                                   : esNombreVacio
                                        ? menuRolRepositorio.BuscarPoseeOpcionMenu(idPadreMenu, idRolUsuario)
                                        : menuRolRepositorio.BuscarPoseeOpcionMenu(idPadreMenu, nombreMenu, idRolUsuario);
                    break;
                default:
                    opcionesMenu = new List<Menu>();
                    break;
            }

            BuscarPoseeMenu(opcionesMenu);

            return opcionesMenu;
        }
        private void EstablecerTipoGestion(int idRol, int idUsuario)
        {
            using (var bd = new Contexto())
            {
                if (idRol > 0)
                {
                    TipoGestion = Enumerados.GestionMenu.Rol;
                    IdGestionarMenu = idRol;
                    var rolRepositorio = new RolRepositorio(bd);
                    NombreTipoGestion = rolRepositorio.ObtenerNombrePorId(idRol);
                }
                else
                {
                    if (idUsuario > 0)
                    {
                        TipoGestion = Enumerados.GestionMenu.Usuario;
                        IdGestionarMenu = idUsuario;
                        var usuarioRepositorio = new UsuarioRepositorio(bd);
                        NombreTipoGestion = usuarioRepositorio.ObtenerNombrePorId(idUsuario);
                    }
                    else
                    {
                        TipoGestion = Enumerados.GestionMenu.Menu;
                    }
                }
            }
        }
        private void BuscarPoseeMenu(IEnumerable<Menu> opcionesMenu)
        {
            foreach (var menu in opcionesMenu)
            {
                switch (TipoGestion)
                {
                    case Enumerados.GestionMenu.Usuario:
                        menu.PoseeOpcionMenu = _menuUsuarioRepositorio.BuscarPoseeOpcionMenu(menu.IDMenu, IdGestionarMenu);
                        break;
                    case Enumerados.GestionMenu.Rol:
                        var menuRolRepositorio = new MenuRolRepositorio();
                        menu.PoseeOpcionMenu = menuRolRepositorio.PoseeOpcionMenu(menu.IDMenu, IdGestionarMenu);
                        break;
                }
            }
        }
    }
}

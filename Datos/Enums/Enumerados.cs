using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Enums
{
    public class Enumerados
    {
        public enum VariablesSesion
        {
            Aplicacion,
            Menu
        }
        public enum Genero
        {
            Femenino,
            Masculino
        }
        public enum GestionMenu
        {
            Menu = 1,
            Rol = 2,
            Usuario = 3
        }
        public enum TempData
        {
            Breadcrumbs,
            IdGestionarMenu,
            TipoGestion,
            Error,
            Mensaje,
            NombreTipoGestion,
            MenuBreadcrumbs,
            IdModificar,
            IdEmpleado
        }

        public enum TipoUsuario
        {
            Empleado,
            Otro
        }

        public enum Parametro
        {
            IdMenu,
            IdUsuario,
            IdRol,
            Habilitado,
            IdMenuPadre,
            IdColonia,
            IdPuesto,
            IdTitulo,
            IdEmpleado,
            IdEntidadOrganigrama,
            IdPadreEntidadOrganigrama,
            IdNivelOrganigrama,
            IdPadreNivelOrganigrama,
            IdEstado,
            IdMunicipio,
            IdEntidad,
            IdSolicitud,
            IdSancion,
            idPersona
        }

        public enum Pfx
        {
            UrlPfx,
            PasswordPfx
        }
    }
}

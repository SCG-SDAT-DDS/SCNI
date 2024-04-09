using System.Collections.Generic;

namespace Datos.DTO.Infraestructura
{
    public class SistemaUsuario
    {
        public int IdUsuario { set; get; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombrePuesto { get; set; }
        public string DescripcionPuesto { get; set; }
        public string SucursalNombre { get; set; }
        public string UrlFoto { get; set; }
        public string UrlPaginaInicio { get; set; }
        public int IdEntidad { get; set; }
        public int IdCasa { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public int IdEmpleado { get; set; }
        public int Sucursal { get; set; }
        public int IdEntidadSuperior { get; set; }
        public bool TienePrivilegioMultientidad { get; set; }
        public bool TieneMenuPersonalizado { get; set; }
        public bool TieneAccesoGuardarCliente { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public List<Menu> Menu { get; set; }
        public List<Menu> MenuBreadcrumb { get; set; }
        public ICollection<Permisos> Permisos { get; set; }
        public string SerieCertificado { get; set; }
    }
}

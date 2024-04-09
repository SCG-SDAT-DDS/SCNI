using Datos;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Sistema;

namespace Presentacion.Fabrica
{
    public static class Factory
    {
        public static EmpleadoDetalles Obtener(Empleado empleado)
        {
            if (empleado == null) return null;
            //if (empleado.Usuario != null) empleado.Usuario.Empleado = null;
            return new EmpleadoDetalles
            {
                NombreCompleto = string.Format("{0} {1} {2}", empleado.Nombre, empleado.ApellidoP, empleado.ApellidoM),
                Puesto = empleado.Puesto != null ? empleado.Puesto.Nombre : Resources.General.SinPuesto,
                Sexo = ((Enumerados.Genero)empleado.Sexo).ToString(),
                UrlFoto = empleado.UrlFoto.Trim('~'),
                //Telefono = empleado.Telefono,
                Celular = empleado.Celular,
                //Colonia = empleado.Colonia.Nombre,
                Email = empleado.Email,
                //NumeroImss = empleado.NumeroIMSS,
                Rfc = empleado.Rfc,
                //Curp = empleado.Curp,
                //CodigoPostal = empleado.Colonia.CodigoPostal.ToString(CultureInfo.InvariantCulture),
                //Direccion = empleado.Direccion,
                //DireccionNumero = empleado.DireccionNumero,
                //EntreCalles1 = empleado.EntreCalles1,
                //EntreCalles2 = empleado.EntreCalles2,
                //FechaNacimiento = empleado.FechaNacimiento.ToShortDateString(),
                //Usuario = empleado.Usuario == null ? null : Obtener(empleado.Usuario)
            };
        }

        public static UsuarioDetalles Obtener(Usuario usuario)
        {
            if (usuario == null) return null;
            if (usuario.Empleado != null) usuario.Empleado.Usuario = null;
            return new UsuarioDetalles
            {
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                UrlFoto = usuario.UrlFoto != null ? usuario.UrlFoto.Trim('~') : string.Empty,
                TipoUsuario = usuario.Empleado != null
                                ? Enumerados.TipoUsuario.Empleado.ToString()
                                : Enumerados.TipoUsuario.Otro.ToString(),
                Rol = usuario.Rol.Nombre,
                Empleado = Obtener(usuario.Empleado)
            };
        }

        public static EntidadDetalles Obtener(Entidad entidad)
        {
            if (entidad == null)
            {
                return new EntidadDetalles
                {
                    Nombre = "",
                    Abreviacion = "",
                    Tipo = ""

                };
            };
            return new EntidadDetalles
            {
                Nombre = entidad.Nombre,
                Abreviacion = entidad.Abreviacion,
                Tipo = Listas.ObtenerValorDeLista(Listas.ObtenerListaTipoEntidad(), entidad.Tipo)
            };
        }

        public static ColoniaDetalles Obtener(Colonia colonia)
        {
            if (colonia == null) return null;
            return new ColoniaDetalles
            {
                Nombre = colonia.Nombre,
                Estado = new EstadoRepositorio(new Contexto()).BuscarPorId(colonia.IDEstado).Nombre,
                Municipio = new MunicipioRepositorio(new Contexto()).BuscarPorId(colonia.IDMunicipio, colonia.IDEstado).Nombre
            };
        }
        public static MenuDetalles Obtener(Menu menu)
        {
            if (menu == null) return null;
            return new MenuDetalles
            {
                Opcion = menu.Opcion,
                Destino = menu.Destino,
                Visible = menu.Visible.ToString()

            };
        }

        public static RolDetalles Obtener(Rol rol)
        {
            if (rol == null) return null;
            return new RolDetalles
            {
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion

            };
        }

        public static PersonaDetalles Obtener(Persona persona)
        {
            if (persona == null) return null;
            return new PersonaDetalles
            {
                Nombre = string.Format("{0} {1} {2}", persona.Nombre, persona.ApellidoP, persona.ApellidoM),
                Genero = Listas.ObtenerValorDeLista(Listas.ObtenerListaGenero(), int.Parse(persona.Genero)),
                RFC = persona.RFC,
                CURP = string.IsNullOrEmpty(persona.CURP) ? "" : persona.CURP,
                Email = string.IsNullOrEmpty(persona.CorreoElectronico) ? "" : persona.CorreoElectronico

            };
        }

        public static SancionDetalles Obtener(Sancion sancion)
        {
            if (sancion == null) return null;

            return new SancionDetalles
            {
                Tipo = sancion.Tipo.HasValue
                    ? Listas.ObtenerValorDeLista(Listas.ObtenerListaTipoSancionFederal(),
                        sancion.Tipo.Value)
                    : string.Empty,
                Especificar = sancion.EspecificarOtro,
                NumeroExpediente = sancion.NumeroExpediente,
                FechaEjecutoria = sancion.FechaEjecutoria.ToString(),
                FechaResolucion = sancion.FechaResolucion.ToString(),
                Año = sancion.Año.ToString(),
                TipoSancion =
                    Listas.ObtenerValorDeLista(Listas.ObtenerListaTipoSancionFederal(),
                        sancion.TipoSancion),
                Monto = sancion.Monto.ToString(),
                TiempoAños = sancion.TiempoAños.ToString() + " Años",
                TiempoMeses = sancion.TiempoMeses.ToString() + " Meses",
                TiempoDias = sancion.TiempoDias.ToString() + " Dias",
                Habilitado = sancion.Habilitado,
                Observaciones = sancion.Observaciones,
                Entidad = Obtener(sancion.Entidad),
                Persona = Obtener(sancion.Persona),
                MostrarEspecificar = (sancion.Tipo == 3 ? true : false),
                MostrarMonto = (sancion.TipoSancion == 5 ? true : false),
                MostrarTiempos = (sancion.TipoSancion == 3 || sancion.TipoSancion == 6 ? true : false)
            };
        }

        public static SolicitudDetalles Obtener(Solicitud solicitud)
        {
            if (solicitud == null) return null;
            return new SolicitudDetalles
            {
                FolioPago = solicitud.FolioPago,
                FechaPago = solicitud.Fecha.ToString(),
                Identificacion = Listas.ObtenerValorDeLista(Listas.ObtenerListaTipoIdentificacion(), solicitud.Identificacion),
                TipoSolicitud = Listas.ObtenerValorDeLista(Listas.ObtenerListaTipoSolicitud(), solicitud.Tipo),
                Entidad = Obtener(solicitud.Entidad),
                Persona = Obtener(solicitud.Persona),
                TipoEntidad = solicitud.Tipo == 2 ? true : false

            };
        }

        public static FirmaDetalles Obtener(Firma firma) {
            if (firma == null) return null;
            return new FirmaDetalles
            {
                NumeroExpediente = firma.Carta.NumeroExpediente,
                Empleado = Obtener(firma.Empleado)
            };
        }
    }

    public class EmpleadoDetalles
    {
        public string NombreCompleto { get; set; }
        public string Puesto { get; set; }
        public string UrlFoto { get; set; }
        public string Direccion { get; set; }
        public string DireccionNumero { get; set; }
        public string EntreCalles1 { get; set; }
        public string EntreCalles2 { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string NumeroImss { get; set; }
        public string Rfc { get; set; }
        public string Curp { get; set; }
        public UsuarioDetalles Usuario { get; set; }
    }

    public class UsuarioDetalles
    {
        public string NombreUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public string UrlFoto { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public EmpleadoDetalles Empleado { get; set; }
    }

    public class SucursalDetalles
    {
        public string Nombre { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public EmpleadoDetalles Encargado { get; set; }
    }

    public class EntidadDetalles
    {
        public string Nombre { get; set; }
        public string Abreviacion { get; set; }
        public string Tipo { get; set; }
    }

    public class ColoniaDetalles {
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
    }

    public class MenuDetalles
    {
        public string Opcion { get; set; }
        public string Destino { get; set; }
        public string Visible { get; set; }
    }

    public class RolDetalles
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class PersonaDetalles
    {
        public string Nombre { get; set; }
        public string RFC { get; set; }
        public string Genero { get; set; }
        public string CURP { get; set; }
        public string Email { get; set; }
        
    }
    public class SancionDetalles
    {
        public string Tipo { get; set; }
        public string Especificar { get; set; }
        public string NumeroExpediente { get; set; }
        public string FechaEjecutoria { get; set; }
        public string FechaResolucion { get; set; }
        public string Año { get; set; }
        public string TipoSancion { get; set; }
        public string Monto { get; set; }
        public string TiempoAños { get; set; }
        public string TiempoMeses { get; set; }
        public string TiempoDias { get; set; }
        public bool Habilitado { get; set; }
        public string Observaciones { get; set; }
        public PersonaDetalles Persona { get; set; }
        public EntidadDetalles Entidad { get; set; }
        public bool MostrarEspecificar { get; set; }
        public bool MostrarTiempos { get; set; }
        public bool MostrarMonto { get; set; }

    }

    public class SolicitudDetalles
    {
        public string FolioPago { get; set; }
        public string FechaPago { get; set; }
        public string TipoSolicitud { get; set; }
        public string Identificacion { get; set; }
        public PersonaDetalles Persona { get; set; }
        public EntidadDetalles Entidad { get; set; }
        public bool TipoEntidad { get; set; }

    }

    public class FirmaDetalles {
        public string NumeroExpediente { get; set; }
        public EmpleadoDetalles Empleado { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Sistema.Extensiones;
using Datos.DTO.Infraestructura.ViewModels;

namespace Datos.Repositorios
{
    public class UsuarioRepositorio : Repositorio<Usuario>
    {
        public UsuarioRepositorio(Contexto contexto) : 
            base(contexto)
        {
        }

        /// <summary>
        /// Busca usuario por correo/nombre de usuario y contraseña
        /// </summary>
        /// <param name="correo">Correo/nombre de usuario a comparar</param>
        /// <param name="contrasena">Contraseña a comparar</param>
        /// <returns>Regresa objeto tipo usuario encontrado</returns>
        public Usuario BuscarUsuarioPorCorreoYContrasena(string correo, string contrasena)
        {
            return (from u in Contexto.Usuario
                    .Include(x => x.Rol)
                    .Include(x=> x.Rol.Permisos)
                    .Include(x => x.Empleado.Puesto)
                    .Include(x => x.Rol.MenuInicio)
                    where (u.Email == correo || u.NombreUsuario == correo) &&
                          u.Contrasena == contrasena &&
                          u.Rol.Habilitado &&
                          u.Habilitado
                    select u).FirstOrDefault();
        }

        public Usuario BuscarUsuarioPorNombreUsuario(string nombreUsuario)
        {
            return (from u in Contexto.Usuario
                .Include(x => x.Rol)
                .Include(x => x.Rol.Permisos)
                .Include(x => x.Empleado.Puesto)
                .Include(x => x.Empleado.Titulo)
                .Include(x => x.Rol.MenuInicio)
                .Include(x => x.Certificado)
                where u.NombreUsuario == nombreUsuario &&
                      u.Rol.Habilitado &&
                      u.Habilitado
                select u).FirstOrDefault();
        }

        /// <summary>
        /// Activa o desactiva el menu personalizado para el usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="tieneMenuPersonalizado"></param>
        /// <returns></returns>
        public bool CambiarTieneMenuPersonalziado(int idUsuario, bool tieneMenuPersonalizado)
        {
            var usuarioCambiar = new Usuario { IDUsuario = idUsuario };

            Contexto.Usuario.Attach(usuarioCambiar);
            usuarioCambiar.MenuPersonalizado = tieneMenuPersonalizado;
            Contexto.Entry(usuarioCambiar).Property(u => u.MenuPersonalizado).IsModified = true;
            return Contexto.SaveChanges() == 1;
        }

        public int ObtenerIdRolPorIdUsuario(int idUsuario)
        {
            return
                (from u in Contexto.Usuario
                 where u.IDUsuario == idUsuario
                 select u.Rol.IDRol).SingleOrDefault();
        }

        public string ObtenerNombrePorId(int idUsuario)
        {
            return (from u in Contexto.Usuario
                    where u.IDUsuario == idUsuario
                    select u.NombreUsuario).SingleOrDefault();
        }

        public Usuario BuscarPorCorreo(string email)
        {
            return (from u in Contexto.Usuario
                    where u.Email == email
                    select u).SingleOrDefault();
        }

        public bool ActualizarContrasena(int idUsuario, string contrasena)
        {
            var aExists = Contexto.Usuario.Find(idUsuario);
            Contexto.Entry(aExists).State = EntityState.Detached;
            aExists.IDUsuario = idUsuario;
            aExists.Contrasena = contrasena; //contrasena.GetHashSha512();
            Contexto.Entry(aExists).State = EntityState.Modified;
            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Busca usuarios habilitados/deshabilitados
        /// </summary>
        /// <param name="criterios"></param>
        /// <returns>Lista con los usuarios encontrados</returns>
        public List<Usuario> Buscar(UsuarioViewModel criterios)
        {
            return ObtenerQuery(criterios, true).ToList();
        }

        /// <summary>
        /// Obtiene el total de registros de la busqueda por criterios
        /// </summary>
        /// <param name="criterios"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(UsuarioViewModel criterios)
        {
            return ObtenerQuery(criterios, false).Count();
        }

        private IQueryable<Usuario> ObtenerQuery(UsuarioViewModel criterios, bool paginar)
        {
            IQueryable<Usuario> query = Contexto.Set<Usuario>();

            query = query.Where(b => b.Habilitado == criterios.Usuario.Habilitado);

            if (!string.IsNullOrEmpty(criterios.Usuario.NombreUsuario))
            {
                query = query.Where(b => b.NombreUsuario.Contains(criterios.Usuario.NombreUsuario));
            }
            if (criterios.Usuario.Rol != null && criterios.Usuario.Rol.IDRol > 0)
            {
                query = query.Where(b => b.Rol.IDRol == criterios.Usuario.Rol.IDRol);
            }
            //if (criterios.Usuario.IDSucursal > 0)
            //{
            //    query = query.Where(b => b.IDSucursal == criterios.Usuario.IDSucursal);
            //}
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.NombreUsuario);

                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            query = query.Include(u => u.Empleado);
            return query;
        }

        /// <summary>
        /// Busca el usuario por su ID de empleado
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns>Objeto tipo usuario encontrado</returns>
        public Usuario BuscarPorIdEmpleado(int idEmpleado)
        {
            return (from u in Contexto.Usuario
                    where u.Empleado.IDEmpleado == idEmpleado
                    select u).SingleOrDefault();
        }

        /// <summary>
        /// Busca al usuario por su ID
        /// </summary>
        /// <param name="idUsuario">ID del usuario a buscar</param>
        /// <returns>Regresa objeto tipo usuario encontrado</returns>
        public Usuario BuscarPorId(int idUsuario)
        {
            return (from r in Contexto.Usuario
                        .Include(x => x.Rol)
                        .Include(x => x.Empleado)
                    where r.IDUsuario == idUsuario
                    select r).SingleOrDefault();
        }

        /// <summary>
        /// Obtiene el id el usuario por su id de empleado
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        public int ObtenerIdUsuarioPorIdEmpleado(int idEmpleado)
        {
            return (from u in Contexto.Usuario
                    where u.Empleado.IDEmpleado == idEmpleado
                    select u.IDUsuario).SingleOrDefault();
        }

        /// <summary>
        /// Guarda un usuario tipo empleado
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="empleado"></param>
        public void GuardarUsuarioEmpleado(Usuario usuario, Empleado empleado)
        {
            Contexto.Empleado.Attach(empleado);

            usuario.Habilitado = true;
            usuario.Empleado = empleado;
            usuario.Contrasena = usuario.Contrasena.GetHashSha512();

            Guardar(usuario);
        }

        /// <summary>
        /// Actualiza un usuario de tipo empleado
        /// </summary>
        /// <param name="usuario"></param>
        public bool ActualizarUsuarioEmpleado(Usuario usuario)
        {
            var usuarioModificar = new Usuario { IDUsuario = usuario.IDUsuario };
            Contexto.Usuario.Add(usuarioModificar);
            //Contexto.Usuario.Attach(usuarioModificar);
            //Contexto.Usuario.AddOrUpdate(usuarioModificar);

            usuarioModificar.IDRol = usuario.IDRol;
            usuarioModificar.NombreUsuario = usuario.NombreUsuario;

            return Contexto.SaveChanges() == 1;
        }
        /// <summary>
        /// Actualiza un usuario de tipo empleado
        /// </summary>
        /// <param name="usuario"></param>
        public bool ActualizarUsuarioEmpleadoN(Usuario usuario)
        {
            //var usuarioModificar = new Usuario { IDUsuario = usuario.IDUsuario };
            var aExists = Contexto.Usuario.Find(usuario.IDUsuario);
            Contexto.Entry(aExists).State=EntityState.Detached;
            aExists.IDRol = usuario.IDRol;
            aExists.NombreUsuario = usuario.NombreUsuario;
            Contexto.Entry(aExists).State=EntityState.Modified;

            

            return Contexto.SaveChanges() == 1;
        }
        /// <summary>
        /// Habilita/deshabilita un usuario por su id
        /// </summary>
        /// <param name="idUsuario">Id del usuario a habilitar</param>
        /// <param name="habilitado">Habilitar/deshabilitar el usuario</param>
        /// <returns>Indica si fue posible habilitar/deshabilitar el usuario</returns>
        public bool CambiarHabilitado(int idUsuario, bool habilitado)
        {
            var eventoCambiarHabilitado = new Usuario { IDUsuario = idUsuario };

            Contexto.Usuario.Attach(eventoCambiarHabilitado);
            eventoCambiarHabilitado.Habilitado = habilitado;
            Contexto.Entry(eventoCambiarHabilitado).Property(r => r.Habilitado).IsModified = true;
            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Busca el usuario de acceso público por su Nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Objeto tipo usuario encontrado</returns>
        public Usuario BuscarUsuarioPublicoPorNombre(string nombre)
        {
            return (from u in Contexto.Usuario
                    where u.NombreUsuario == nombre
                    select u).SingleOrDefault();
        }

        public byte[] ObtenerCertificado(int idUsuario)
        {
            return (from u in Contexto.UsuarioCertificado
                where u.IDUsuario == idUsuario
                select u.Valor).FirstOrDefault();
        }
    }
}

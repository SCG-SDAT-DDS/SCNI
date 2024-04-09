using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Recursos;
using Sistema.Extensiones;

namespace Datos.Repositorios.Catalogos
{
    public class EmpleadoRepostorio : Repositorio<Empleado>
    {
        public EmpleadoRepostorio(Contexto contexto) : base(contexto)
        {
        }

        public override void Guardar(Empleado entidad)
        {
            var empleadoGuardar = new Empleado
            {
                Nombre = entidad.Nombre,
                ApellidoP = entidad.ApellidoP,
                ApellidoM = entidad.ApellidoM,
                Rfc = entidad.Rfc,
                UrlFoto = entidad.UrlFoto,
                Sexo = entidad.Sexo,
                Celular = entidad.Celular,
                Email = entidad.Email,
                FechaIngreso = DateTime.Now,
                IDPuesto = entidad.IDPuesto,
                IDTitulo = entidad.IDTitulo,
                Habilitado = true,
                Usuario = new Usuario
                {
                    NombreUsuario = entidad.Usuario.NombreUsuario,
                    Contrasena = entidad.Usuario.Contrasena.GetHashSha512(),
                    Email = entidad.Usuario.Email,
                    IDRol = entidad.Usuario.Rol.IDRol,
                    UrlFoto = entidad.Usuario.UrlFoto,
                    MenuPersonalizado = entidad.Usuario.MenuPersonalizado,
                    Habilitado = true,
                    Certificado = entidad.Usuario.Certificado
                }
            };

            Contexto.Empleado.Add(empleadoGuardar);
        }

        public override void Modificar(Empleado entidad)
        {
            var empleadoModificar = new Empleado {IDEmpleado = entidad.IDEmpleado};

            Contexto.Empleado.Attach(empleadoModificar);

            empleadoModificar.Nombre = entidad.Nombre;
            empleadoModificar.ApellidoP = entidad.ApellidoP;
            empleadoModificar.ApellidoM = entidad.ApellidoM;
            empleadoModificar.Rfc = entidad.Rfc;
            empleadoModificar.Sexo = entidad.Sexo;
            empleadoModificar.Celular = entidad.Celular;
            empleadoModificar.Email = entidad.Email;
            empleadoModificar.DefaultFirma = entidad.DefaultFirma;
            Contexto.Entry(empleadoModificar).Property(e => e.DefaultFirma).IsModified = true;

            if(!string.IsNullOrEmpty(entidad.UrlFoto))
                empleadoModificar.UrlFoto = entidad.UrlFoto;

            if (!string.IsNullOrEmpty(entidad.UrlFirma))
                empleadoModificar.UrlFirma = entidad.UrlFirma;

            var puestoPertenece = new Puesto { IDPuesto = entidad.IDPuesto };
            Contexto.Puesto.Attach(puestoPertenece);
            empleadoModificar.Puesto = puestoPertenece;

            var tituloPertenece = new Titulo {IDTitulo = entidad.IDTitulo};
            Contexto.Titulos.Attach(tituloPertenece);
            empleadoModificar.Titulo = tituloPertenece;

            if (entidad.Usuario == null) return;

            var usuarioModificar = new Usuario { IDUsuario = entidad.Usuario.IDUsuario };

            Contexto.Usuario.Attach(usuarioModificar);

            usuarioModificar.NombreUsuario = entidad.Usuario.NombreUsuario;

            if(!string.IsNullOrEmpty(entidad.Usuario.Contrasena))
                usuarioModificar.Contrasena = entidad.Usuario.Contrasena.GetHashSha512();

            var rolPertenece = new Rol {IDRol = entidad.Usuario.Rol.IDRol};
            Contexto.Rol.Attach(rolPertenece);
            usuarioModificar.Rol = rolPertenece;

            if (entidad.Usuario.Certificado?.Valor != null)
            {
                var certificadoPertenece = new UsuarioCertificado {IDUsuario = entidad.Usuario.IDUsuario};

                if (entidad.Usuario.Certificado.IDUsuario > 0)
                    Contexto.UsuarioCertificado.Attach(certificadoPertenece);
                else
                    usuarioModificar.Certificado = certificadoPertenece;

                certificadoPertenece.Valor = entidad.Usuario.Certificado.Valor;
            }
        }

        /// <summary>
        /// Cambia el habilitado de un puesto
        /// </summary>
        /// <param name="idEmpleado">ID del puesto</param>
        /// <param name="habilitado">Opción habilitado</param>
        /// <returns>Verdadero si se guardo correctamente</returns>
        public bool CambiarHabilitado(int idEmpleado, bool habilitado)
        {
            var empleado = new Empleado { IDEmpleado = idEmpleado };

            Contexto.Empleado.Attach(empleado);
            empleado.Habilitado = habilitado;
            Contexto.Entry(empleado).Property(r => r.Habilitado).IsModified = true;

            return Contexto.SaveChanges() == 1;
        }

        /// <summary>
        /// Pusca un empleado por su id
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        public Empleado BuscarPorId(int idEmpleado)
        {
            var test = new Empleado();
            test  = (from e in Contexto.Empleado
                        .Include(e => e.Puesto)
                        .Include(e => e.Usuario.Rol)
                        //.Include(e => e.Usuario.Rol.Permisos)
                        //.Include(e => e.Usuario.Menu)
                        //.Include(e => e.Usuario.Movmiento)
                        .Include(e => e.Usuario.Certificado)
                     where e.IDEmpleado == idEmpleado
                    select e).SingleOrDefault();
            return test;
        }

        /// <summary>
        /// Busca colonias por criterios de busqueda
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Lista de colonias encontradas</returns>
        public List<Empleado> Buscar(EmpleadoViewModel viewModel)
        {
            return ObtenerQuery(viewModel, true).ToList();
        }

        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(EmpleadoViewModel viewModel)
        {
            return ObtenerQuery(viewModel, false).Count();
        }

        public IQueryable<Empleado> ObtenerQuery(EmpleadoViewModel criterios, bool paginar)
        {
            IQueryable<Empleado> query = Contexto.Set<Empleado>().Where(e => e.Habilitado == criterios.Empleado.Habilitado);

            if (!string.IsNullOrEmpty(criterios.Empleado.Nombre))
            {
                query = query.Where(e => e.Nombre.Contains(criterios.Empleado.Nombre));
            }
            if (!string.IsNullOrEmpty(criterios.Empleado.ApellidoP))
            {
                query = query.Where(e => e.ApellidoP.Contains(criterios.Empleado.ApellidoP));
            }
            if (!string.IsNullOrEmpty(criterios.Empleado.ApellidoM))
            {
                query = query.Where(e => e.ApellidoM.Contains(criterios.Empleado.ApellidoM));
            }
            if (!string.IsNullOrEmpty(criterios.Empleado.Puesto.Nombre))
            {
                query = query.Where(e => e.Puesto.Nombre.Contains(criterios.Empleado.Puesto.Nombre));
            }
            if (criterios.Empleado.Sexo > 0)
            {
                query = query.Where(e => e.Sexo == criterios.Empleado.Sexo);
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            query = query.Include(e => e.Puesto);
            query = query.Include(e => e.Usuario);
            query = query.Include(e => e.Usuario.Rol);
            query = query.Include(e => e.Usuario.Rol.Permisos);

            return query;
        }

        public static IEnumerable<SelectListItem> BuscarSinEntidad(int idEmpleado)
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from e in bd.Empleado
                         where e.IDEmpleado == idEmpleado
                         orderby e.Nombre
                         select e).ToList().Select(e => ObtenerSelecListItem(e, idEmpleado)).ToList();
                lista.Insert(0, new SelectListItem
                {
                    Text = General.Seleccione,
                    Value = string.Empty
                });
            }
            return lista;
        }

        private static SelectListItem ObtenerSelecListItem(Empleado empleado, int idEmpleado = 0)
        {
            return new SelectListItem
            {
                Value = empleado.IDEmpleado.ToString(CultureInfo.InvariantCulture),
                Text =
                        string.Format("{0} {1} {2}", empleado.Nombre, empleado.ApellidoP,
                                      empleado.ApellidoM),
                Selected = empleado.IDEmpleado == idEmpleado
            };
        }

        /// <summary>
        /// Obtiene el calendario con los cumpleaños de los empleados
        /// </summary>
        /// <returns></returns>
        public static string ObtenerEstructuraCalendario()
        {
            List<CalendarioEstructura> listaCalendario;
            using (var bd = new Contexto())
            {
                var empleados = (from e in bd.Empleado where e.Habilitado select e).ToList();

                listaCalendario = new List<CalendarioEstructura>();

                foreach (var empleado in empleados)
                {
                    var titulo = string.Format("Cumpleaños de {0} {1} {2}",
                                               empleado.Nombre, empleado.ApellidoP, empleado.ApellidoM);

                    var diaMesNacimiento = empleado.FechaIngreso.ToString("MM/dd");
                    var cumpleanios = string.Format("{0}/{1}", diaMesNacimiento, DateTime.Now.Year);

                    listaCalendario.Add(new CalendarioEstructura
                    {
                        title = titulo,
                        tooltip = titulo,
                        start = cumpleanios,
                        color = "#18A689",
                        textColor = "#FFFFFF",
                        allDay = true,
                    });
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(listaCalendario);
        }
    }

    class DistinctItemComparer : IEqualityComparer<Empleado>
    {
        public bool Equals(Empleado x, Empleado y)
        {
            return x.IDEmpleado == y.IDEmpleado &&
                x.Nombre == y.Nombre &&
                x.ApellidoP == y.ApellidoP &&
                x.ApellidoM == y.ApellidoM &&
                x.Sexo == y.Sexo &&
                x.Rfc == y.Rfc;
        }

        public int GetHashCode(Empleado obj)
        {
            return obj.IDEmpleado.GetHashCode() ^
                obj.ApellidoP.GetHashCode() ^
                obj.ApellidoM.GetHashCode() ^
                obj.Sexo.GetHashCode() ^
            obj.Rfc.GetHashCode();
        }
    }
}

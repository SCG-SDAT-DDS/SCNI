using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Resources;
using System.Web.Mvc;
using Datos;
using Datos.Enums;
using Datos.DTO.Infraestructura;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios;
using Negocio.Excepciones;
using Negocio.Firma;
using Negocio.Servicios;
using Sistema;
using System.IO;

namespace Presentacion.Controllers
{
    public class LoginController : ControladorBase
    {
        private UsuarioRepositorio _usuarioRepositorio;

        [HttpGet]
        public ActionResult ProbarCorreo()
        {
            Correo.EnviarConstancia("diegomedina@live.com.mx", "hola", new byte[] {});

            var viewModel = new AccesoViewModel
            {
                Digestion = OperacionesFiel.Generar64Digestion(DateTime.Now.ToString("ddMMyyyyHHmmss")),
                TipoInicio = ObtenerUsuarioTipoInicio()
            };

            return View("Index", "Layouts/_LayoutLogin", viewModel);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new AccesoViewModel
            {
                Digestion = OperacionesFiel.Generar64Digestion(DateTime.Now.ToString("ddMMyyyyHHmmss")),
                TipoInicio = ObtenerUsuarioTipoInicio()
            };

            return View("Index","Layouts/_LayoutLogin", viewModel);
        }

        [HttpPost]
        public ActionResult Index(AccesoViewModel acceso, FormCollection Form)
        {   
            if (!ModelState.IsValid) return null; 
            
            try
            {
                var postedFile = Request.Files["pfx"];
                var urlPfx = "";
                if (postedFile != null)
                {
                    var respuestaFile = this.GuardarArchivos(postedFile);
                    if (respuestaFile.Result)
                    {
                        urlPfx = respuestaFile.Valor;
                    }
                    else
                    {
                        throw new ScniException(respuestaFile.Valor);
                    }
                }

                var servicios = new ServiciosAcceso(acceso.TipoInicio);
                var certificadoUsuario = servicios.ValidarAccesoConPFX(acceso.Usuario.NombreUsuario, acceso.Usuario.Contrasena, urlPfx);

                GuardarCookieTipoInicio(acceso.TipoInicio);
                GuardarUsuarioEnSesion(certificadoUsuario.Value, certificadoUsuario.Key,urlPfx, acceso.Usuario.Contrasena);
                return RedirectToAction("Index", "Inicio");

            }
            catch (ScniException ex)
            {
                EscribirError(ex.Message);
            }

            return View("Index", "Layouts/_LayoutLogin", acceso);
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SolicitarContrasena()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SolicitarContrasena(string email)
        {
            var enviado = false;
            using (var bd = new Contexto())
            {
                _usuarioRepositorio = new UsuarioRepositorio(bd);
                var usuarioBd = _usuarioRepositorio.BuscarPorCorreo(email);
                if (usuarioBd != null)
                {
                    var codigo = Comun.GenerarGuid();
                    var solicitudRepositorio = new SolicitudContrasenaRepositorio(bd);
                    var correcto = solicitudRepositorio.Guardar(usuarioBd.IDUsuario, codigo);

                    if (correcto)
                    {
                        enviado = await Correo.RestablecerContrasena(usuarioBd.Email, General.RestablecerContrasena, codigo);
                    }
                    EnviarAlerta(General.CorreoEnviado, General.FalloEnviarCorreo, enviado);
                }
                else
                {
                    EscribirError(General.NoExisteCorreoElectronico);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult RestablecerContrasena(string c)
        {
            var usuario = new Usuario();
            using (var bd = new Contexto())
            {
                var solicitudRepositorio = new SolicitudContrasenaRepositorio(bd);
                usuario = solicitudRepositorio.ObtenerUsuario(c);
                if (usuario == null) return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [HttpPost]
        public ActionResult RestablecerContrasena(string contrasena, string c)
        {
            bool correcto;
            using (var bd = new Contexto())
            {
                var solicitudRepositorio = new SolicitudContrasenaRepositorio(bd);
                _usuarioRepositorio = new UsuarioRepositorio(bd);
                correcto = _usuarioRepositorio.ActualizarContrasena(solicitudRepositorio.ObtenerIdUsuario(c), contrasena);
                if (correcto) solicitudRepositorio.DesactivarCodigo(c);
            }
            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Contrasena), correcto);
            return correcto
                ? (ActionResult)RedirectToAction("Index")
                : View(new Usuario());
        }

        private void GuardarUsuarioEnSesion(Usuario usuarioEncontrado, string serieCertificado = null, string urlPfx ="",string passwordPfx = "")
        {
            var usuario = new SistemaUsuario
            {
                Nombre = usuarioEncontrado.Empleado.Titulo.Abreviacion + " " +
                         usuarioEncontrado.Empleado.Nombre + " " +
                         usuarioEncontrado.Empleado.ApellidoP + " " +
                         usuarioEncontrado.Empleado.ApellidoM,
                NombreUsuario = usuarioEncontrado.NombreUsuario,
                CorreoElectronico = usuarioEncontrado.Email,
                IdUsuario = usuarioEncontrado.IDUsuario,
                IdRol = usuarioEncontrado.Rol.IDRol,
                NombreRol = usuarioEncontrado.Rol.Nombre,
                NombrePuesto = usuarioEncontrado.Empleado.Puesto.Nombre,
                DescripcionPuesto = usuarioEncontrado.Empleado.Puesto.Descripcion,
                UrlFoto = usuarioEncontrado.UrlFoto,
                TieneMenuPersonalizado = usuarioEncontrado.MenuPersonalizado,
                TipoUsuario = ObtenerTipoUsuario(usuarioEncontrado),
                UrlPaginaInicio = usuarioEncontrado.Rol.Menu.Destino,
                IdEmpleado = (int) usuarioEncontrado.Empleado?.IDEmpleado,
                Menu = (usuarioEncontrado.MenuPersonalizado != true)
                    ? ObtenerMenuUsuario(usuarioEncontrado.IDRol, 0)
                    : ObtenerMenuUsuario(0, usuarioEncontrado.IDUsuario),
                Permisos = usuarioEncontrado.Rol.Permisos,
                SerieCertificado = serieCertificado
            };

            Session[Enumerados.VariablesSesion.Aplicacion.ToString()] = usuario;
            Session[Enumerados.Pfx.UrlPfx.ToString()] = urlPfx;
            Session[Enumerados.Pfx.PasswordPfx.ToString()] = passwordPfx;
        }

        private void GuardarCookieTipoInicio(TiposInicio tipoInicioSesion)
        {
            var valor = ((byte) tipoInicioSesion).ToString();

            if (HttpContext.Response.Cookies.AllKeys.Contains("tipoInicioSesion"))
            {
                var cookie = Request.Cookies.Get("tipoInicioSesion");
                cookie.Value = valor;
                Response.Cookies.Set(cookie);
            }
            else
            {
                var cookie = new HttpCookie("tipoInicioSesion", valor)
                {
                    Expires = DateTime.Today.AddMonths(1)
                };
                Response.Cookies.Add(cookie);
            }
        }

        private TiposInicio ObtenerUsuarioTipoInicio()
        {
            var tipo = TiposInicio.Contraseña;

            if (HttpContext.Request.Cookies.AllKeys.Contains("tipoInicioSesion"))
            {
                var valorTipo = int.Parse(Request.Cookies.Get("tipoInicioSesion").Value);
                tipo = (TiposInicio) valorTipo;
            }

            return tipo;
        }

        private TipoUsuario ObtenerTipoUsuario(Usuario usuario)
        {
            if (usuario.Empleado != null)
            {
                return TipoUsuario.Empleado;
            }
            return 0;
        }

        private List<Menu> ObtenerMenuUsuario(int idRol, int idUsuario)
        {
            
            if (idUsuario != 0)
            {
                var menuUsuarioModelo = new MenuUsuarioRepositorio();
                return menuUsuarioModelo.BuscarMenu(idUsuario, true);
            }
            else
            {
                var menuRolModelo = new MenuRolRepositorio();
                return menuRolModelo.BuscarMenu(idRol, true);
            }
            
        }
        private ResultGuardarPfx GuardarArchivos(HttpPostedFileBase Archivo)
        {
            //EjercicioTxt = EjercicioTxt != null && EjercicioTxt.Length > 0 ? EjercicioTxt : "NoEspecificado";
            //nombreCarpeta = nombreCarpeta != null && nombreCarpeta.Length > 0 ? nombreCarpeta : "NoEspecificado";
            var respuesta = new ResultGuardarPfx();
            respuesta.Result = false;
            respuesta.Valor = "ocurrio un error al momento de subir el archivo.";
            try
            {

                //ruta: Archivos/nombrePlantilla/ejercicio/mes/

                //Creamos directoriro
                //string directorio_sub = $@"Archivos\Plantillas\{nombreCarpeta}\{EjercicioTxt}\{+DateTime.Now.Month}";
                string Directorio = Server.MapPath("~/Temp/");

                bool exists = System.IO.Directory.Exists(Directorio);
                if (!exists)
                    System.IO.Directory.CreateDirectory(Directorio);

                if (Archivo != null)
                {
                    var random = new Random();
                    var r = random.Next(1234567, 12345678);

                    //string tempName = Path.GetFileNameWithoutExtension(Archivo.FileName);
                    //tempName = tempName.Substring(0, ((tempName.Trim().Length) > 20 ? 20 : tempName.Trim().Length));
                    string nombreArchAnexo = r.ToString() + "_" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + Path.GetExtension(Archivo.FileName);
                    //mandeee
                    string PathArchivo = $@"{Path.Combine(Server.MapPath("~/Temp/"), nombreArchAnexo)}";
                    //var PathArchivo = Directorio + @"\" + nombreArchAnexo;
                    Archivo.SaveAs(PathArchivo);
                    respuesta.Result = true;
                    respuesta.Valor = PathArchivo;
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Result = false;
                respuesta.Valor = ex.Message;
            }
            return respuesta;
        }
    }
}
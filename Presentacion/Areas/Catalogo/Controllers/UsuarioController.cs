using System;
using System.Web.Mvc;
using Datos;
using Datos.Repositorios;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Recursos;
using Datos.Repositorios.Catalogos;
using Sistema.Paginador;
using Sistema.Extensiones;
using Presentacion.Controllers;
using Presentacion.Fabrica;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class UsuarioController : ControladorBase
    {
        private UsuarioRepositorio _usuarioRepositorio;
        // GET: Catalogo/Usuario
        public ActionResult Index(UsuarioViewModel usuarioViewModel)
        {
            try
            {
                usuarioViewModel.Usuario = new Usuario {Habilitado = true};
                using (var bd = new Contexto())
                {
                    _usuarioRepositorio = new UsuarioRepositorio(bd);
                    usuarioViewModel.Usuarios = _usuarioRepositorio.Buscar(usuarioViewModel);
                    usuarioViewModel.TotalEncontrados = _usuarioRepositorio.ObtenerTotalRegistros(usuarioViewModel);
                    usuarioViewModel.Paginas = Paginar.ObtenerCantidadPaginas(usuarioViewModel.TotalEncontrados,
                                                    usuarioViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(usuarioViewModel);
        }

        [HttpPost]
        public ActionResult Index(UsuarioViewModel usuarioViewModel, string pagina)
        {
            if (string.IsNullOrEmpty(pagina)) pagina = "1";
            try
            {
                usuarioViewModel.PaginaActual = pagina.TryToInt();
                using (var bd = new Contexto())
                {
                    _usuarioRepositorio = new UsuarioRepositorio(bd);
                    usuarioViewModel.Usuarios = _usuarioRepositorio.Buscar(usuarioViewModel);
                    usuarioViewModel.TotalEncontrados = _usuarioRepositorio.ObtenerTotalRegistros(usuarioViewModel);
                    usuarioViewModel.Paginas = Paginar.ObtenerCantidadPaginas(usuarioViewModel.TotalEncontrados,
                                                    usuarioViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(usuarioViewModel);
        }
        
        [HttpGet]
        public ActionResult CrearUsuario(string idEmpleado)
        {
            var usuario = new Usuario();
            try
            {
                var idEmpleadoBuscar = idEmpleado.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    _usuarioRepositorio = new UsuarioRepositorio(bd);
                    usuario = _usuarioRepositorio.BuscarPorIdEmpleado(idEmpleadoBuscar);
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }

            return View(usuario);
        }

        [HttpPost]
        public ActionResult CrearUsuario(string idRol, string nombreUsuario, string contrasena)
        {
            var usuario = new Usuario();
            var idEmpleadoGuardar = ObtenerParametroGetEnInt(Enumerados.Parametro.IdEmpleado);
            if (ModelState.IsValid)
            {
                try
                {
                    if (idEmpleadoGuardar > 0)
                    {
                        using (var bd = new Contexto())
                        {
                            bd.Configuration.ValidateOnSaveEnabled = false;
                            var empleadoRepositorio = new EmpleadoRepostorio(bd);
                            _usuarioRepositorio = new UsuarioRepositorio(bd);

                            var empleado = empleadoRepositorio.BuscarPorId(idEmpleadoGuardar);
                            usuario.IDUsuario = _usuarioRepositorio.ObtenerIdUsuarioPorIdEmpleado(idEmpleadoGuardar);
                            usuario.IDRol = idRol.TryToInt();
                            usuario.NombreUsuario = nombreUsuario;
                            usuario.Contrasena = contrasena;

                            bool correcto;
                            if (usuario.IDUsuario > 0)
                            {
                                correcto = _usuarioRepositorio.ActualizarUsuarioEmpleadoN(usuario);
                                GuardarMovimiento("Modificación", "Usuario", usuario.IDUsuario);
                            }
                            else
                            {
                                usuario.UrlFoto = empleado.UrlFoto;
                                usuario.Email = empleado.Email;
                                _usuarioRepositorio.GuardarUsuarioEmpleado(usuario, empleado);
                                correcto = bd.SaveChanges() >= 1;
                                if (correcto)
                                {
                                    GuardarMovimiento("Creación", "Usuario", usuario.IDUsuario);
                                }
                            }
                            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Empleado), correcto);
                        }
                    }
                    else
                    {
                        EscribirError(General.ErrorIdEmpleado);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            ModelState.Clear();
            return View(new Usuario());
        }

        [HttpPost]
        public JsonResult Obtener(string idUsuario)
        {
            Usuario usuario;
            using (var bd = new Contexto())
            {
                _usuarioRepositorio = new UsuarioRepositorio(bd);
                usuario = _usuarioRepositorio.BuscarPorId(idUsuario.DecodeFrom64().TryToInt());
            }
            return Json(Factory.Obtener(usuario));
        }

        [HttpGet]
        public ActionResult Habilitar(string idUsuario)
        {
            try
            {
                var idUsuarioHabilitar = idUsuario.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _usuarioRepositorio = new UsuarioRepositorio(bd);
                    _usuarioRepositorio.CambiarHabilitado(idUsuarioHabilitar, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Usuario", idUsuarioHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idUsuario)
        {
            try
            {
                var idUsuarioHabilitar = idUsuario.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _usuarioRepositorio = new UsuarioRepositorio(bd);
                    _usuarioRepositorio.CambiarHabilitado(idUsuarioHabilitar, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Usuario", idUsuarioHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloDeshabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }
    }
}
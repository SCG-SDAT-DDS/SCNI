using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Recursos;
using Datos.Repositorios;
using Datos.Repositorios.Catalogos;
using Negocio.Excepciones;
using Negocio.Firma;
using Presentacion.Controllers;
using Presentacion.Fabrica;
using Sistema;
using Sistema.Extensiones;
using Sistema.Paginador;
using System.Collections.Generic;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class EmpleadoController : ControladorBase
    {
        private EmpleadoRepostorio _empleadoRepostorio;
        private SistemaUsuario _sistemaUsuario;

        [HttpGet]
        public ActionResult Index(EmpleadoViewModel empleadoViewModel)
        {
            try
            {
                _sistemaUsuario = (SistemaUsuario)Session[Enumerados.VariablesSesion.Aplicacion.ToString()];
                empleadoViewModel.Empleado = new Empleado {Habilitado = true, Puesto = new Puesto()};
                using (var bd = new Contexto())
                {
                    _empleadoRepostorio = new EmpleadoRepostorio(bd);
                    empleadoViewModel.Empleados = _empleadoRepostorio.Buscar(empleadoViewModel);
                    empleadoViewModel.TotalEncontrados = _empleadoRepostorio.ObtenerTotalRegistros(empleadoViewModel);
                    empleadoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(empleadoViewModel.TotalEncontrados,
                                                    empleadoViewModel.TamanoPagina);
                    empleadoViewModel.Permisos = _sistemaUsuario.Permisos;
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return View(empleadoViewModel);
        }

        [HttpPost]
        public ActionResult Index(EmpleadoViewModel empleadoViewModel, string pagina)
        {
            if (string.IsNullOrEmpty(pagina)) pagina = "1";
            try
            {
                _sistemaUsuario = (SistemaUsuario)Session[Enumerados.VariablesSesion.Aplicacion.ToString()];
                empleadoViewModel.PaginaActual = pagina.TryToInt();
                using (var bd = new Contexto())
                {
                    _empleadoRepostorio = new EmpleadoRepostorio(bd);
                    empleadoViewModel.Empleados = _empleadoRepostorio.Buscar(empleadoViewModel);
                    empleadoViewModel.TotalEncontrados = _empleadoRepostorio.ObtenerTotalRegistros(empleadoViewModel);
                    empleadoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(empleadoViewModel.TotalEncontrados,
                                                    empleadoViewModel.TamanoPagina);
                    empleadoViewModel.Permisos = _sistemaUsuario.Permisos;
                }
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(empleadoViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idEmpleado)
        {
            var idEmpleadoBuscar = idEmpleado.DecodeFrom64().TryToInt();
            var empleado = new Empleado
            {
                IDEmpleado = idEmpleadoBuscar,
                FechaIngreso = DateTime.Now
            };

            if (idEmpleadoBuscar > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        _empleadoRepostorio = new EmpleadoRepostorio(bd);
                        empleado = _empleadoRepostorio.BuscarPorId(idEmpleadoBuscar);

                        if (empleado.Usuario.Certificado == null)
                            empleado.Usuario.Certificado = new UsuarioCertificado();
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            GuardarDatoTemporal(Enumerados.TempData.IdModificar, idEmpleadoBuscar, true);
            return View(empleado);
        }

        [HttpPost]
        public ActionResult Guardar(Empleado empleado, IEnumerable<HttpPostedFileBase> archivos)
        {
            HttpPostedFileBase imagen = null;
            HttpPostedFileBase certificado = null;
            HttpPostedFileBase firmaImagen = null;
            var c = 0;
            var httpPostedFileBases = archivos as HttpPostedFileBase[] ?? archivos.ToArray();
            foreach (var archivo in httpPostedFileBases)
            {
                if (archivo != null)
                {
                    if (c == (httpPostedFileBases.Count() == 3 ? 0 : -1)) imagen = archivo;
                    if (c == (httpPostedFileBases.Count() == 3 ? 1 : 0)) certificado = archivo;
                    if (c == (httpPostedFileBases.Count() == 3 ? 2 : 1)) firmaImagen = archivo;
                }
                c++;
            }

            var imageName = Guid.NewGuid().ToString();

            empleado.IDEmpleado = Request.QueryString["idEmpleado"].DecodeFrom64().TryToInt();

            if(empleado.IDEmpleado > 0)
                ModelState["IDEmpleado"].Errors.Clear();

            if (empleado.IDEmpleado == 0)
            {
                ModelState["Usuario.IDUsuario"].Errors.Clear();
                ModelState["Usuario.Certificado.IDUsuario"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    empleado.Habilitado = true;
                    empleado.Usuario.Habilitado = true;

                    if (imagen != null)
                        empleado.UrlFoto = Archivo.ObtenerRutaFotoBaseDatos(imagen, Archivo.Ruta.FotosEmpleados, imageName);

                    if (certificado != null)
                    {
                        if (Path.GetExtension(certificado.FileName) != ".cer")
                            throw new ScniException("La extensión del certificado es invalido.");

                        using (var ms = new MemoryStream())
                        {
                            certificado.InputStream.CopyTo(ms);
                            empleado.Usuario.Certificado.Valor = ms.ToArray();
                        }

                        ServiciosWebFiel.NoOperacionAutenticarCertificado(empleado.Usuario.Certificado.Valor);
                    }
                    else
                    {
                        empleado.Usuario.Certificado = null;
                    }

                    var nombreFirma = Guid.NewGuid().ToString("N");

                    if (firmaImagen != null)
                        empleado.UrlFirma = Archivo.ObtenerRutaFotoBaseDatos(firmaImagen, Archivo.Ruta.ImagenesFirma, nombreFirma);

                    using (var bd = new Contexto())
                    {
                        _empleadoRepostorio = new EmpleadoRepostorio(bd);

                        var movimiento = "Creación";

                        if (empleado.IDEmpleado > 0)
                        {
                            _empleadoRepostorio.Modificar(empleado);
                            movimiento = "Modificación";
                        }
                        else
                        {
                            _empleadoRepostorio.Guardar(empleado);
                        }

                        bd.SaveChanges();

                        Archivo.GuardarImagen(imagen, Archivo.Ruta.FotosEmpleados, imageName);
                        Archivo.GuardarImagen(firmaImagen, Archivo.Ruta.ImagenesFirma, nombreFirma);

                        GuardarMovimiento(movimiento, "Usuario", empleado.IDEmpleado);

                        EnviarAlerta(General.GuardadoCorrecto, string.Format(General.GuardadoCorrecto, General.Empleado), true);

                        return RedirectToAction("Guardar",
                            new {idEmpleado = (empleado.IDEmpleado).ToString(CultureInfo.InvariantCulture).EncodeTo64()});
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);

                    EnviarAlerta(General.GuardadoCorrecto, string.Format(ex.Message, General.Empleado), false);
                }
            }

            return View(empleado);
        }

        [HttpGet]
        public ActionResult CrearUsuario(string idEmpleado)
        {
            return RedirectToAction("CrearUsuario", "Usuario", new { area = "Catalogo", idEmpleado });
        }

        [HttpGet]
        public ActionResult AsignarMenu(string idEmpleado)
        {
            string idUsuario;
            using (var bd = new Contexto())
            {
                var usuarioRepositorio = new UsuarioRepositorio(bd);
                idUsuario = usuarioRepositorio.ObtenerIdUsuarioPorIdEmpleado(idEmpleado.DecodeFrom64().TryToInt())
                                .ToString(CultureInfo.InvariantCulture).EncodeTo64();
            }
            return RedirectToAction("Index", "Menu", new { area = "Configuracion", idUsuario });
        }

        [HttpGet]
        public ActionResult Calendario()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerificarExiste(Empleado empleado)
        {
            var idModificar = ObtenerDatoTemporal<int>(Enumerados.TempData.IdModificar, true);
            using (var bd = new Contexto())
            {
                var existe = false;
                _empleadoRepostorio = new EmpleadoRepostorio(bd);

                if (!string.IsNullOrEmpty(empleado.Email))
                {
                    existe = _empleadoRepostorio.Buscar(e => e.Email == empleado.Email
                        && e.IDEmpleado != idModificar).Any();
                }

                return Json(!existe, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Habilitar(string idEmpleado)
        {
            try
            {
                var idEmpleadoHabilitar = idEmpleado.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    _empleadoRepostorio = new EmpleadoRepostorio(bd);
                    _empleadoRepostorio.CambiarHabilitado(idEmpleadoHabilitar, true);
                    EscribirMensaje(General.HabilitadoCorrecto);
                    GuardarMovimiento("Habilitar", "Empleado", idEmpleadoHabilitar);
                }
            }
            catch (Exception)
            {
                EscribirError(string.Format(General.FalloHabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Deshabilitar(string idEmpleado)
        {
            try
            {
                var idEmpleadoHabilitar = idEmpleado.DecodeFrom64().TryToInt();
                using (var bd = new Contexto())
                {
                    bd.Configuration.ValidateOnSaveEnabled = false;
                    _empleadoRepostorio = new EmpleadoRepostorio(bd);
                    _empleadoRepostorio.CambiarHabilitado(idEmpleadoHabilitar, false);
                    EscribirMensaje(General.DeshabilitadoCorrecto);
                    GuardarMovimiento("Deshabilitar", "Empleado", idEmpleadoHabilitar);
                }
            }
            catch (Exception ex)
            {
                EscribirError(string.Format(General.FalloDeshabilitar, General.Rol));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [WebMethod]
        public JsonResult Obtener(string idEmpleado)
        {
            Empleado empleado;
            using (var bd = new Contexto())
            {
                _empleadoRepostorio = new EmpleadoRepostorio(bd);
                empleado = _empleadoRepostorio.BuscarPorId(idEmpleado.DecodeFrom64().TryToInt());

                empleado.Nombre = ValidarNulo(empleado.Nombre);
                empleado.ApellidoP = ValidarNulo(empleado.ApellidoP);
                empleado.ApellidoM = ValidarNulo(empleado.ApellidoM);
                empleado.Rfc = ValidarNulo(empleado.Rfc);
                empleado.Email = ValidarNulo(empleado.Email);
                empleado.Celular = ValidarNulo(empleado.Celular);
                empleado.Puesto.Nombre = ValidarNulo(empleado.Puesto.Nombre);

            }
            return Json(Factory.Obtener(empleado));
        }
    }
}

using System;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios.Firma;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Configuracion;
using Datos.Repositorios.Solicitudes;
using Presentacion.Controllers;
using Presentacion.Fabrica;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Catalogo.Controllers
{

    public class MovimientoController : ControladorBase
    {
        private MovimientoRepositorio _movimientoRepositorio;

        // GET: Catalogo/Movimiento
        [HttpGet]
        public ActionResult Index(MovimientoViewModel movimientoViewModel)
        {
            try
            {
                movimientoViewModel.Movimiento = new Movmiento();
                using (var bd = new Contexto())
                {
                    _movimientoRepositorio = new MovimientoRepositorio(bd);
                    movimientoViewModel.Movimientos = _movimientoRepositorio.Buscar(movimientoViewModel);
                    movimientoViewModel.TotalEncontrados = _movimientoRepositorio.ObtenerTotalRegistros(movimientoViewModel);
                    movimientoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(movimientoViewModel.TotalEncontrados, movimientoViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            //return View();
            return View(movimientoViewModel);
            //return View(db.Colonia.ToList());
        }

        [HttpPost]
        public ActionResult Index(MovimientoViewModel movimientoViewModel, string pagina)
        {
            movimientoViewModel.PaginaActual = pagina.TryToInt();
            if (movimientoViewModel.PaginaActual <= 0) movimientoViewModel.PaginaActual = 1;

            try
            {
                using (var bd = new Contexto())
                {
                    _movimientoRepositorio = new MovimientoRepositorio(bd);
                    movimientoViewModel.Movimientos = _movimientoRepositorio.Buscar(movimientoViewModel);
                    movimientoViewModel.TotalEncontrados = _movimientoRepositorio.ObtenerTotalRegistros(movimientoViewModel);
                    movimientoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(movimientoViewModel.TotalEncontrados, movimientoViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(movimientoViewModel);
        }

        [HttpPost]
        [WebMethod]
        public JsonResult Detalle(string id, string modulo)
        {
            if (modulo == "Entidad")
            {
                Entidad entidad;
                using (var bd = new Contexto())
                {
                    EntidadRepositorio _entidadRepositorio = new EntidadRepositorio(bd);
                    entidad = _entidadRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());

                    entidad.Abreviacion = ValidarNulo(entidad.Abreviacion);
                }
                return Json(Factory.Obtener(entidad));
            }
            else if (modulo == "Usuario")
            {
                Empleado empleado;
                using (var bd = new Contexto())
                {
                    EmpleadoRepostorio _empleadoRepostorio = new EmpleadoRepostorio(bd);
                    empleado = _empleadoRepostorio.BuscarPorId(id.DecodeFrom64().TryToInt());

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
            else if (modulo == "Colonia")
            {
                Colonia colonia;
                using (var bd = new Contexto())
                {
                    ColoniaRepositorio _coloniaRepositorio = new ColoniaRepositorio(bd);
                    colonia = _coloniaRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());
                }
                return Json(Factory.Obtener(colonia));
            }
            else if (modulo == "Firma")
            {
                Firma firma;
                using (var bd = new Contexto())
                {
                    RepositorioFirmas _firmaRepositorio = new RepositorioFirmas(bd);
                    firma = _firmaRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());
                }
                return Json(Factory.Obtener(firma));
            }
            else if (modulo == "Menu")
            {
                Menu menu;
                using (var bd = new Contexto())
                {
                    MenuRepositorio _menuRepositorio = new MenuRepositorio(bd);
                    menu = _menuRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());
                }
                return Json(Factory.Obtener(menu));
            }
            else if (modulo == "Rol")
            {
                Rol rol;
                using (var bd = new Contexto())
                {
                    RolRepositorio _rolRepositorio = new RolRepositorio(bd);
                    rol = _rolRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());
                }
                return Json(Factory.Obtener(rol));
            }
            else if (modulo == "Sanción")
            {
                Sancion sancion;
                using (var bd = new Contexto())
                {
                    SancionRepositorio _sancionRepositorio = new SancionRepositorio(bd);
                    sancion = _sancionRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());

                    sancion.EspecificarOtro = ValidarNulo(sancion.EspecificarOtro);
                    sancion.NumeroExpediente = ValidarNulo(sancion.NumeroExpediente);
                    sancion.Observaciones = ValidarNulo(sancion.Observaciones);
                    sancion.Persona.RFC = ValidarNulo(sancion.Persona.RFC);
                    sancion.Persona.Nombre = ValidarNulo(sancion.Persona.Nombre);
                    sancion.Persona.ApellidoP = ValidarNulo(sancion.Persona.ApellidoP);
                    sancion.Persona.ApellidoM = ValidarNulo(sancion.Persona.ApellidoM);
                    sancion.Entidad.Nombre = ValidarNulo(sancion.Entidad.Nombre);

                }
                return Json(Factory.Obtener(sancion));
            }
            else if (modulo == "SolicitudWeb")
            {
                Datos.Solicitud solicitud;
                using (var bd = new Contexto())
                {
                    SolicitudRepositorio _solicitudRepositorio = new SolicitudRepositorio(bd);
                    solicitud = _solicitudRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());

                    solicitud.Persona.RFC = ValidarNulo(solicitud.Persona.RFC);
                    solicitud.Persona.CURP = ValidarNulo(solicitud.Persona.CURP);
                    solicitud.Persona.CorreoElectronico = ValidarNulo(solicitud.Persona.CorreoElectronico);
                }
                return Json(Factory.Obtener(solicitud));
            }
            else if (modulo == "Solicitud")
            {
                Datos.Solicitud solicitud;
                using (var bd = new Contexto())
                {
                    SolicitudRepositorio _solicitudRepositorio = new SolicitudRepositorio(bd);
                    solicitud = _solicitudRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());

                    solicitud.Persona.RFC = ValidarNulo(solicitud.Persona.RFC);
                    solicitud.Persona.CURP = ValidarNulo(solicitud.Persona.CURP);
                    solicitud.Persona.CorreoElectronico = ValidarNulo(solicitud.Persona.CorreoElectronico);
                }
                return Json(Factory.Obtener(solicitud));
            }
            //else if (modulo == "Usuario")
            //{
            //    Usuario solicitud;
            //    using (var bd = new Contexto())
            //    {
            //        UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio(bd);
            //        solicitud = _usuarioRepositorio.BuscarPorId(id.DecodeFrom64().TryToInt());
            //    }
            //    return Json(Factory.Obtener(solicitud));
            //}
            return Json(false);
            

        }
    }
}
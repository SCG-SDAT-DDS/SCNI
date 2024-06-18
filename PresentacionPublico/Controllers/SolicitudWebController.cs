using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Recursos;
using Datos.Repositorios;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Solicitudes;
using Negocio.PaseCaja;
using Newtonsoft.Json;
using Sistema;
using Sistema.Extensiones;

namespace Presentacion.Controllers
{
    public class SolicitudWebController : Controller
    {
        private SolicitudRepositorio _solicitudRepositorio;
        
        public ActionResult Index()
        {
            return View();
        }

        #region "Acciones"
        [HttpGet]
        public ActionResult Ciudadano(SolicitudViewModel solicitudViewModel)
        {
            if (solicitudViewModel.Solicitud == null) solicitudViewModel.Solicitud = new Datos.Solicitud();
            if (solicitudViewModel.Solicitud.Persona == null) solicitudViewModel.Solicitud.Persona = new Persona();
            if (solicitudViewModel.Solicitud.Entidad == null) solicitudViewModel.Solicitud.Entidad = new Entidad();

            if (solicitudViewModel.Solicitud.Fecha.ToString("dd/MM/yyyy HH:mm:ss") == "01/01/0001 00:00:00")
            {
                solicitudViewModel.Solicitud.Fecha = DateTime.Now;
            }

            return View(solicitudViewModel);
            
        }

        [HttpPost]
        public async Task<ActionResult> Ciudadano(Solicitud solicitud, HttpPostedFileBase archivoIdentificacion)
        {
            var captchaCorrecto = await _ValidarCaptcha();
            
            // Definición de variables
            var correcto = false;
            string mensajeError = null;

            var repostiro = new PaseCajaRepository();
            var respuestaPago = repostiro.ValidarPago(solicitud.FolioPago);
            var pagoCorrecto = respuestaPago.STATUS == "0";
            
            var archivoPapeleta = Convert.FromBase64String(respuestaPago.STR_B64);

            var existeFolioPago = true;

            if (pagoCorrecto)
            {
                using (var bd = new Contexto())
                {
                    var solicitudRepositorio = new SolicitudRepositorio(bd);
                    existeFolioPago = solicitudRepositorio.ExisteFolioSolicitud(respuestaPago.DATOS.RECIBO,
                        respuestaPago.DATOS.PASE_CAJA);
                }
            }

            if (captchaCorrecto && pagoCorrecto && existeFolioPago == false)
            {
                try
                {
                    var reciboOficial = respuestaPago.DATOS.RECIBO;
                    var paseCaja = respuestaPago.DATOS.PASE_CAJA;

                    solicitud.IDSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

                    var nombrePapeleta = Guid.NewGuid().ToString();
                    var nombreIdentificacion = Guid.NewGuid().ToString();

                    solicitud.URLPapeleta = Archivo.ObtenerRutaSolicitudesBaseDatos(archivoPapeleta,
                        Archivo.Ruta.SolicitudDocumentos, nombrePapeleta);
                    solicitud.URLIdentificacion = Archivo.ObtenerRutaSolicitudesBaseDatos(archivoIdentificacion,
                        Archivo.Ruta.SolicitudDocumentos, nombreIdentificacion);

                    solicitud.Medio = MediosSolicitud.Web;

                    // Crear contexto y abrir conexión
                    using (var bd = new Contexto())
                    {
                        // Configuraciones previas de la conexión
                        bd.Configuration.ValidateOnSaveEnabled = false;
                        bd.Entry(solicitud.Persona).State = (solicitud.Persona.IDPersona > 0
                            ? EntityState.Modified
                            : EntityState.Added);

                        if (solicitud.Persona.IDPersona > 0)
                        {
                            bd.Entry(solicitud.Persona).Property(p => p.Genero).IsModified = true;
                        }

                        // Configuraciones previas del modelo
                        _solicitudRepositorio = new SolicitudRepositorio(bd);
                        solicitud.FechaSolicitud = solicitud.FechaSolicitud == new DateTime()
                            ? DateTime.Now
                            : solicitud.FechaSolicitud;
                        solicitud.Tipo = 1;

                        if (solicitud.IDSolicitud > 0)
                        {
                            // Modificar Solicitud existente
                        }
                        else
                        {
                            // Generar código de verificación
                            solicitud.Codigo = solicitud.FolioPago + solicitud.Persona.Nombre[0] +
                                               solicitud.Persona.ApellidoP[0];
                            if (solicitud.Persona.ApellidoM?.Length > 0)
                                solicitud.Codigo += solicitud.Persona.ApellidoM[0];
                            else
                                solicitud.Persona.ApellidoM = string.Empty;

                            if (solicitud.Persona.CorreoElectronico == null)
                                solicitud.Persona.CorreoElectronico = string.Empty;

                            if (solicitud.Persona.CURP == null)
                                solicitud.Persona.CURP = string.Empty;

                            //asignar folios
                            solicitud.FolioPago = reciboOficial;
                            solicitud.FolioPaseCaja = paseCaja;

                            // Crear Solicitud nueva
                            _solicitudRepositorio.Guardar(solicitud);
                        }

                        // Guardar cambios realizados al contexto
                        bd.SaveChanges();

                        correcto = true;
                        
                        Archivo.GuardarImagen(archivoIdentificacion, Archivo.Ruta.SolicitudDocumentos, nombreIdentificacion);
                        Archivo.GuardarPapeleta(archivoPapeleta, Archivo.Ruta.SolicitudDocumentos, nombrePapeleta);

                        GuardarMovimiento("Creación", "SolicitudWeb", solicitud.IDSolicitud, "Ciudadano");
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            else if(captchaCorrecto == false)
            {
                mensajeError = "Capcha inválido";
            }
            else
            {
                mensajeError = "Folio ingresado es inválido o no se tiene pago registrado.";
            }

            // Crear mensajes de alerta
            EnviarAlerta("Solicitud enviada. Posteriormente recibirá en el correo que proporcionó la constancia de no inhabilitación en un lapso no mayor a 24 hrs tomando en cuenta los días hábiles.", mensajeError ?? string.Format(General.FalloGuardado, "Solicitud"), correcto);

            // Crear nuevo viewmodel para retornar la vista limpia
            var solicitudViewModel = new SolicitudViewModel {Solicitud = solicitud};

            // Retornar vista
            return correcto ? (ActionResult) RedirectToAction("Ciudadano") : View(solicitudViewModel);
        }

        [HttpGet]
        public ActionResult Entidad(SolicitudViewModel solicitudViewModel)
        {
            try
            {
                var bd = new Contexto();
                if (solicitudViewModel.Solicitud == null) solicitudViewModel.Solicitud = new Datos.Solicitud();
                if (solicitudViewModel.Solicitud.Persona == null) solicitudViewModel.Solicitud.Persona = new Persona();
                if (solicitudViewModel.Solicitud.Entidad == null) solicitudViewModel.Solicitud.Entidad = new Entidad();

                if (solicitudViewModel.Solicitud.Fecha.ToString("dd/MM/yyyy HH:mm:ss") == "01/01/0001 00:00:00")
                {
                    solicitudViewModel.Solicitud.Fecha = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }

            return View(solicitudViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Entidad(HttpPostedFileBase archivoIdentificacion, Solicitud solicitud)
        {
            var captchaCorrecto = await _ValidarCaptcha();

            // Definición de variables
            var correcto = false;
            string mensajeError = null;

            var repostiro = new PaseCajaRepository();
            var respuestaPago = repostiro.ValidarPago(solicitud.FolioPago);
            var pagoCorrecto = respuestaPago.STATUS == "0";

            var archivoPapeleta = Convert.FromBase64String(respuestaPago.STR_B64);

            var existeFolioPago = true;

            if (pagoCorrecto)
            {
                using (var bd = new Contexto())
                {
                    var solicitudRepositorio = new SolicitudRepositorio(bd);
                    existeFolioPago = solicitudRepositorio.ExisteFolioSolicitud(respuestaPago.DATOS.RECIBO,
                        respuestaPago.DATOS.PASE_CAJA);
                }
            }

            if (captchaCorrecto && pagoCorrecto && existeFolioPago == false)
            {
                try
                {
                    var reciboOficial = respuestaPago.DATOS.RECIBO;
                    var paseCaja = respuestaPago.DATOS.PASE_CAJA;

                    var nombrePapeleta = Guid.NewGuid().ToString();
                    var nombreIdentificacion = Guid.NewGuid().ToString();

                    solicitud.IDSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

                    solicitud.URLPapeleta = Archivo.ObtenerRutaSolicitudesBaseDatos(archivoPapeleta,
                        Archivo.Ruta.SolicitudDocumentos, nombrePapeleta);
                    solicitud.URLIdentificacion = Archivo.ObtenerRutaSolicitudesBaseDatos(archivoIdentificacion,
                        Archivo.Ruta.SolicitudDocumentos, nombreIdentificacion);

                    solicitud.Medio = MediosSolicitud.Web;

                    // Crear contexto y abrir conexión
                    using (var bd = new Contexto())
                    {
                        // Configuraciones previas de la conexión
                        bd.Configuration.ValidateOnSaveEnabled = false;
                        bd.Entry(solicitud.Entidad).State = solicitud.Entidad.IDEntidad > 0
                            ? EntityState.Unchanged
                            : EntityState.Added;
                        bd.Entry(solicitud.Persona).State = solicitud.Persona.IDPersona > 0
                            ? EntityState.Unchanged
                            : EntityState.Added;

                        if (solicitud.Persona.IDPersona > 0)
                        {
                            bd.Entry(solicitud.Persona).Property(p => p.Genero).IsModified = true;
                        }

                        // Configuraciones previas del modelo
                        _solicitudRepositorio = new SolicitudRepositorio(bd);
                        solicitud.FechaSolicitud = solicitud.FechaSolicitud == new DateTime()
                            ? DateTime.Now
                            : solicitud.FechaSolicitud;
                        solicitud.Tipo = 2;

                        if (solicitud.IDSolicitud > 0)
                        {
                            // Modificar Solicitud existente
                        }
                        else
                        {
                            // Generar código de verificación
                            solicitud.Codigo = solicitud.FolioPago + solicitud.Persona.Nombre[0] +
                                               solicitud.Persona.ApellidoP[0];
                            if (solicitud.Persona.ApellidoM?.Length > 0)
                                solicitud.Codigo += solicitud.Persona.ApellidoM[0];
                            else
                                solicitud.Persona.ApellidoM = string.Empty;

                            if (solicitud.Persona.CorreoElectronico == null)
                                solicitud.Persona.CorreoElectronico = string.Empty;

                            if (solicitud.Persona.CURP == null)
                                solicitud.Persona.CURP = string.Empty;

                            //asignar folios
                            solicitud.FolioPago = reciboOficial;
                            solicitud.FolioPaseCaja = paseCaja;

                            // Crear Solicitud nueva
                            _solicitudRepositorio.Guardar(solicitud);
                        }

                        // Guardar cambios realizados al contexto
                        bd.SaveChanges();

                        correcto = true;
                        
                        Archivo.GuardarImagen(archivoIdentificacion, Archivo.Ruta.SolicitudDocumentos, nombreIdentificacion);
                        Archivo.GuardarPapeleta(archivoPapeleta, Archivo.Ruta.SolicitudDocumentos, nombrePapeleta);

                        GuardarMovimiento("Creación", "SolicitudWeb", solicitud.IDSolicitud, "Entidad");
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            else if (captchaCorrecto == false)
            {
                mensajeError = "Capcha inválido";
            }
            else
            {
                mensajeError = "Folio ingresado es inválido o no se tiene pago registrado.";
            }

            // Crear mensajes de alerta
            EnviarAlerta("Solicitud enviada. Posteriormente recibirá en el correo que proporcionó la constancia de no inhabilitación en un lapso no mayor a 24 hrs tomando en cuenta los días hábiles.", mensajeError ?? string.Format(General.FalloGuardado, "Solicitud"), correcto);

            // Crear nuevo viewmodel para retornar la vista limpia
            var solicitudViewModel = new SolicitudViewModel { Solicitud = solicitud };

            // Retornar vista
            return correcto ? (ActionResult)RedirectToAction("Entidad") : View(solicitudViewModel);
        }

        [HttpGet]
        public ActionResult Buscar()
        {
            var viewModel = new BusquedaSolicitudDependencia
            {
                UrlDescargarArchivo = _ObtenerUrlDescargar("Carta")
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Buscar(BusquedaSolicitudDependencia viewModel, int? pagina = null)
        {
            var captchaCorrecto = await _ValidarCaptcha();

            bool isPaginacion;

            bool.TryParse(viewModel.Paginacion.DecodeFrom64(), out isPaginacion);

            if (captchaCorrecto || isPaginacion)
            {
                viewModel.PaginaActual = pagina ?? 1;
                viewModel.Paginacion = true.ToString();
                viewModel.BuscarSolicitudes();
                viewModel.UrlDescargarArchivo = _ObtenerUrlDescargar("Carta");
            }
            else
            {
                ModelState.Clear();
                EnviarAlerta(string.Empty, "Capcha inválido.", false);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        #endregion

        #region "Métodos auxiliares"
        private string _ObtenerUrlDescargar(string accion)
        {
            var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri)
            {
                Path = Url.Action(accion, "Descargar"),
                Query = null,
            };

            var url = urlBuilder.ToString();

            return url;
        }
        public void LimpiarModelState()
        {
            foreach (var model in ModelState.Values)
            {
                model.Errors.Clear();
            }
        }
        protected void GuardarMovimiento(string _nombre, string _catalogo, int _id, string _nombreUsuario)
        {
            // Definición de variables
            var _usuario = new Usuario();

            // Crear contexto y abrir conexión
            using (var bd = new Contexto())
            {
                // Configuraciones previas de la conexión
                bd.Configuration.ValidateOnSaveEnabled = false;

                _usuario = new UsuarioRepositorio(bd).BuscarUsuarioPublicoPorNombre(_nombreUsuario);

                // Configuraciones previas del modelo
                MovimientoRepositorio _movimientoRepositorio = new MovimientoRepositorio(bd);
                var movimiento = new Movmiento
                {
                    Nombre = _nombre,
                    Usuario = _usuario,
                    Habilitado = true,
                    Fecha = DateTime.Now,
                    Catalogo = _catalogo,
                    IDRegistro = _id
                };
                bd.Entry(movimiento.Usuario).State = EntityState.Unchanged;

                // Crear Movimiento nuevo
                _movimientoRepositorio.Guardar(movimiento);
                // Guardar cambios realizados al contexto
                bd.SaveChanges();
            }
        }
        public int ObtenerParametroGetEnInt(Enumerados.Parametro parametro)
        {
            return Request.QueryString[parametro.ToString()].DecodeFrom64().TryToInt();
        }
        public void EscribirMensaje(string mensaje)
        {
            TempData[Enumerados.TempData.Mensaje.ToString()] = mensaje;
        }
        public void EscribirError(string error)
        {
            var log = new Log();

            TempData[Enumerados.TempData.Error.ToString()] = error;
            log.AgregarError(error);
        }
        public void EnviarAlerta(string mensajeCorrecto, string mensajeError, bool correcto)
        {
            if (correcto)
            {
                EscribirMensaje(mensajeCorrecto);
            }
            else
            {
                EscribirError(mensajeError);
            }
        }
        #endregion

        #region "Métodos web"
        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerEntidades(int Tipo)
        {
            JsonResult lista = new JsonResult();
            using (var bd = new Contexto())
            {
                SolicitudRepositorio _solicitudRepositorio = new SolicitudRepositorio(bd);
                lista = Json(_solicitudRepositorio.BuscarEntidadPorTipo(Tipo));
                return lista;
            }
        }
        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerMunicipios(int idEstado)
        {
            JsonResult lista = new JsonResult();
            using (var bd = new Contexto())
            {
                MunicipioRepositorio _municipioRepositorio = new MunicipioRepositorio(bd);
                lista = Json(_municipioRepositorio.Buscar(idEstado));
                return lista;
            }
        }
        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerPersonaPorRFC(string RFC)
        {
            JsonResult personas = new JsonResult();
            List<object> items = new List<object>();
            using (var bd = new Contexto())
            {
                PersonaRepositorio _personaRepositorio = new PersonaRepositorio(bd);
                PersonaViewModel _personaViewModel = new PersonaViewModel
                {
                    Persona = new Persona
                    {
                        RFC = RFC,
                        Municipio = new Municipio()
                    },
                    TamanoPagina = 1000
                };
                _personaViewModel.Personas = _personaRepositorio.Buscar(_personaViewModel);
                foreach (Persona persona in _personaViewModel.Personas)
                {
                    var result = _personaRepositorio.BuscarPorID(persona.IDPersona);
                    items.Add(result);
                }
                personas = Json(items, JsonRequestBehavior.AllowGet);
                return personas;
            }
        }
        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerEntidadesPorCriterio(string criterio, string tipo = "0", string esEstatal = "")
        {
            var items = new List<object>();
            using (var bd = new Contexto())
            {
                var entidadRepositorio = new EntidadRepositorio(bd);
                var entidadViewModel = new EntidadViewModel
                {
                    Entidad = new Entidad {Nombre = criterio, Tipo = int.Parse(tipo), Habilitado = true},
                    TamanoPagina = 1000
                };

                entidadViewModel.Entidades = entidadRepositorio.Buscar(entidadViewModel);

                foreach (var entidad in entidadViewModel.Entidades)
                {
                    if (esEstatal == "1" && entidad.Tipo == (byte) TiposEntidad.Federal) continue;

                    var result = entidadRepositorio.BuscarEntidadPorID(entidad.IDEntidad);
                    items.Add(result);
                }

                var entidades = Json(items, JsonRequestBehavior.AllowGet);

                return entidades;
            }
        }
        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerSolicitudPorFolio(string folio) {
            JsonResult solicitud = new JsonResult();
            using (var bd = new Contexto())
            {
                SolicitudRepositorio _solicitudRepositorio = new SolicitudRepositorio(bd);
                solicitud = Json(_solicitudRepositorio.BuscarSolicitudPorFolio(folio));
                return solicitud;
            }
        }
        #endregion

        private async Task<bool> _ValidarCaptcha(string responseCapcha = null)
        {
            var sitioKey = System.Configuration.ConfigurationManager.AppSettings["CaptchaSecreto"];
            var recaptcharesponse = responseCapcha ?? Request["g-recaptcha-response"];

            using (var cliente = new HttpClient())
            {
                var response =
                    await cliente.GetAsync(
                        "https://www.google.com/recaptcha/api/siteverify?secret=" + sitioKey + "&response=" +
                        recaptcharesponse);

                var responseString = await response.Content.ReadAsStringAsync();

                dynamic resultado = JsonConvert.DeserializeObject(responseString);

                return (bool) resultado.success;
            }
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerInformacionFolioPago(string folioPago)
        {
            var repostiro = new PaseCajaRepository();
            var respuestaPago = repostiro.ValidarPago(folioPago);

            bool existeFolioPago;

            using (var bd = new Contexto())
            {
                var solicitudRepositorio = new SolicitudRepositorio(bd);
                existeFolioPago = solicitudRepositorio.ExisteFolioSolicitud(respuestaPago.DATOS.RECIBO,
                        respuestaPago.DATOS.PASE_CAJA);
            }

            if (existeFolioPago)
            {
                return Json(new
                {
                    Nombre = (string) null,
                    Existe = true
                });
            }

            return Json(new
            {
                Nombre = respuestaPago.STATUS == "0" ? respuestaPago.DATOS.NOMBRE : null,
                Existe = false
            });
        }
    }
}
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
using Newtonsoft.Json;
using Sistema;
using Sistema.Extensiones;

namespace Presentacion.Controllers
{
    public class SolicitudWebController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region "Acciones"
        [HttpGet]
        public ActionResult Ciudadano(SolicitudViewModel solicitudViewModel)
        {
            try
            {
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
        public async Task<ActionResult> Ciudadano(Solicitud solicitud, IEnumerable<HttpPostedFileBase> archivos)
        {
            var captchaCorrecto = await _ValidarCaptcha();
            
            // Definición de variables
            bool correcto = false;

            if (captchaCorrecto)
            {
                HttpPostedFileBase imagen1 = null;
                HttpPostedFileBase imagen2 = null;
                var c = 0;
                foreach (HttpPostedFileBase archivo in archivos)
                {
                    if (archivo != null)
                    {
                        if (c == 0) imagen1 = archivo;
                        if (c == 1) imagen2 = archivo;
                    }
                    c++;
                }

                try
                {
                    solicitud.IDSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

                    var filename1 = Guid.NewGuid().ToString();
                    var filename2 = Guid.NewGuid().ToString();
                    solicitud.URLPapeleta = Archivo.ObtenerRutaSolicitudesBaseDatos(imagen1,
                        Archivo.Ruta.SolicitudDocumentos, filename1);
                    solicitud.URLIdentificacion = Archivo.ObtenerRutaSolicitudesBaseDatos(imagen2,
                        Archivo.Ruta.SolicitudDocumentos, filename2);

                    solicitud.Medio = MediosSolicitud.Web;

                    // Crear contexto y abrir conexión
                    using (var bd = new Contexto())
                    {
                        // Configuraciones previas de la conexión
                        bd.Configuration.ValidateOnSaveEnabled = false;
                        bd.Entry(solicitud.Persona).State = (solicitud.Persona.IDPersona > 0
                            ? EntityState.Unchanged
                            : EntityState.Added);

                        if (solicitud.Persona.IDPersona > 0)
                        {
                            bd.Entry(solicitud.Persona).Property(p => p.Genero).IsModified = true;
                        }

                        // Configuraciones previas del modelo
                        var solicitudRepositorio = new SolicitudRepositorio(bd);
                        solicitud.FechaSolicitud = (solicitud.FechaSolicitud == new DateTime()
                            ? DateTime.Now
                            : solicitud.FechaSolicitud);
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

                            // Crear Solicitud nueva
                            solicitudRepositorio.Guardar(solicitud);
                        }

                        // Guardar cambios realizados al contexto
                        correcto = bd.SaveChanges() >= 1;
                        if (correcto)
                        {
                            Archivo.GuardarImagen(imagen1, Archivo.Ruta.SolicitudDocumentos, filename1);
                            Archivo.GuardarImagen(imagen2, Archivo.Ruta.SolicitudDocumentos, filename2);
                            GuardarMovimiento("Creación", "SolicitudWeb", solicitud.IDSolicitud, "Ciudadano");
                        }
                    }

                }
                catch (Exception ex)
                {
                    //EscribirError(ex.Message);
                }
            }

            // Crear mensajes de alerta
            EnviarAlerta("Solicitud enviada correctamente",
                captchaCorrecto ? string.Format(General.FalloGuardado, "Solicitud") : "Capcha inválido", correcto);

            // Crear nuevo viewmodel para retornar la vista limpia
            SolicitudViewModel solicitudViewModel = new SolicitudViewModel
            {
                Solicitud = new Datos.Solicitud
                {
                    Persona = new Persona
                    {
                        Municipio = new Municipio()
                    }
                }
            };

            // Limpiar estado del modelo
            LimpiarModelState();
            ModelState.Clear();

            // Retornar vista
            return View(solicitudViewModel);
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
        public async Task<ActionResult> Entidad(IEnumerable<HttpPostedFileBase> archivos, Solicitud solicitud)
        {
            var captchaCorrecto = await _ValidarCaptcha();

            // Definición de variables
            bool correcto = false;

            if (captchaCorrecto)
            {

                try
                {
                    HttpPostedFileBase imagen1 = null;
                    HttpPostedFileBase imagen2 = null;
                    var c = 0;
                    foreach (HttpPostedFileBase archivo in archivos)
                    {

                        if (archivo != null)
                        {
                            if (c == 0) imagen1 = archivo;
                            if (c == 1) imagen2 = archivo;
                        }
                        c++;
                    }

                    var filename1 = Guid.NewGuid().ToString();
                    var filename2 = Guid.NewGuid().ToString();

                    solicitud.IDSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);
                    solicitud.URLPapeleta = Archivo.ObtenerRutaSolicitudesBaseDatos(imagen1,
                        Archivo.Ruta.SolicitudDocumentos, filename1);
                    solicitud.URLIdentificacion = Archivo.ObtenerRutaSolicitudesBaseDatos(imagen2,
                        Archivo.Ruta.SolicitudDocumentos, filename2);

                    solicitud.Medio = MediosSolicitud.Web;

                    // Crear contexto y abrir conexión
                    using (var bd = new Contexto())
                    {
                        // Configuraciones previas de la conexión
                        bd.Configuration.ValidateOnSaveEnabled = false;
                        bd.Entry(solicitud.Entidad).State = (solicitud.Entidad.IDEntidad > 0
                            ? EntityState.Unchanged
                            : EntityState.Added);
                        bd.Entry(solicitud.Persona).State = (solicitud.Persona.IDPersona > 0
                            ? EntityState.Unchanged
                            : EntityState.Added);

                        if (solicitud.Persona.IDPersona > 0)
                        {
                            bd.Entry(solicitud.Persona).Property(p => p.Genero).IsModified = true;
                        }

                        // Configuraciones previas del modelo
                        var solicitudRepositorio = new SolicitudRepositorio(bd);
                        solicitud.FechaSolicitud = (solicitud.FechaSolicitud == new DateTime()
                            ? DateTime.Now
                            : solicitud.FechaSolicitud);
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

                            // Crear Solicitud nueva
                            solicitudRepositorio.Guardar(solicitud);
                        }

                        // Guardar cambios realizados al contexto
                        correcto = bd.SaveChanges() >= 1;
                        if (correcto)
                        {
                            Archivo.GuardarImagen(imagen1, Archivo.Ruta.SolicitudDocumentos, filename1);
                            Archivo.GuardarImagen(imagen2, Archivo.Ruta.SolicitudDocumentos, filename2);
                            GuardarMovimiento("Creación", "SolicitudWeb", solicitud.IDSolicitud, "Entidad");
                        }
                    }

                }
                catch (Exception ex)
                {
                    //EscribirError(ex.Message);
                }
            }

            // Crear mensajes de alerta
            EnviarAlerta("Solicitud enviada correctamente",
                captchaCorrecto ? string.Format(General.FalloGuardado, "Solicitud") : "Captcha inválido", correcto);

            // Crear nuevo viewmodel para retornar la vista limpia
            SolicitudViewModel solicitudViewModel = new SolicitudViewModel {
                Solicitud = new Datos.Solicitud {
                    Entidad = new Entidad(),
                    Persona = new Persona {
                        Municipio = new Municipio()
                    }
                }
            };

            // Limpiar estado del modelo
            LimpiarModelState();
            ModelState.Clear();

            // Retornar vista
            return View(solicitudViewModel);
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
        public JsonResult ObtenerSolicitudPorFolio(string folio, int idSolicitud = 0) {
            var solicitud = new JsonResult();
            using (var bd = new Contexto())
            {
                var solicitudRepositorio = new SolicitudRepositorio(bd);
                solicitud = Json(solicitudRepositorio.BuscarSolicitudPorFolio(folio, idSolicitud));
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
    }
}
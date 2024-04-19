using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Solicitudes;
using Negocio.PaseCaja;
using Presentacion.Controllers;
using Resources;
using Sistema;
using Sistema.Extensiones;

namespace Presentacion.Areas.Solicitud.Controllers
{
    public class EditPersonaController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var solicitudViewModel = new SolicitudViewModel
            {
                Solicitud = new Datos.Solicitud
                {
                    Persona = new Persona(),
                    Entidad = new Entidad(),
                    NumeroDeOficio = string.Empty,
                    Fecha = DateTime.Now
                }
            };

            var idSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

            if (idSolicitud > 0)
            {
                Datos.Solicitud solicitud;

                using (var conexion = new Contexto())
                {
                    var repositorio = new SolicitudRepositorio(conexion);
                    solicitud = repositorio.BuscarPorId(idSolicitud);
                }

                if (solicitud == null || solicitud.Carta.Any(c => c.Estado == EstadosCarta.Firmada))
                    return RedirectToAction("Index", "Buscar");

                if (solicitud.Entidad == null) solicitud.Entidad = new Entidad();

                solicitudViewModel.Solicitud = solicitud;
            }

            return View(solicitudViewModel);
        }

        [HttpPost]
        public ActionResult Index(Datos.Solicitud solicitud)
        {
            var correcto = false;
            solicitud.IDSolicitud = Request.QueryString["idSolicitud"].DecodeFrom64().TryToInt();

            var repostiro = new PaseCajaRepository();
            var respuestaPago = repostiro.ValidarPago(solicitud.FolioPago);
            var pagoCorrecto = respuestaPago.STATUS == "0";

            var archivoPapeleta = Convert.FromBase64String(respuestaPago.STR_B64);
            var nombrePapeleta = Guid.NewGuid().ToString();

            var accionDefault = (ActionResult)RedirectToAction("DatosPersona", "Validar");

            if (ModelState.IsValid && pagoCorrecto)
            {
                var reciboOficial = respuestaPago.DATOS.RECIBO;
                var paseCaja = respuestaPago.DATOS.PASE_CAJA;

                solicitud.IDSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

                accionDefault = RedirectToAction("DatosPersona", "Validar",
                                   new
                                   {
                                       Area = "Solicitud",
                                       IdSolicitud = solicitud.IDSolicitud.ToString().EncodeTo64()
                                   });

                solicitud.URLPapeleta = Archivo.ObtenerRutaSolicitudesBaseDatos(archivoPapeleta,
                    Archivo.Ruta.SolicitudDocumentos, nombrePapeleta);

                if (string.IsNullOrEmpty(solicitud.Persona.CorreoElectronico)) solicitud.Persona.CorreoElectronico = string.Empty;
                if (solicitud.Persona.IDEstado == 0) solicitud.Persona.IDEstado = 26;
                if (solicitud.Persona.IDMunicipio == 0) solicitud.Persona.IDMunicipio = 30;

                // Generar código de verificación
                solicitud.Codigo = solicitud.FolioPago + solicitud.Persona.Nombre[0] + solicitud.Persona.ApellidoP[0];
                if (solicitud.Persona.ApellidoM?.Length > 0)
                    solicitud.Codigo += solicitud.Persona.ApellidoM[0];
                else
                    solicitud.Persona.ApellidoM = string.Empty;

                //asignar folios
                solicitud.FolioPago = reciboOficial;
                solicitud.FolioPaseCaja = paseCaja;

                try
                {
                    using (var bd = new Contexto())
                    {
                        if (solicitud.Entidad != null)
                        {
                            solicitud.Entidad = new EntidadRepositorio(bd).BuscarPorId(solicitud.Entidad.IDEntidad);
                        }

                        var solicitudRepositorio = new SolicitudRepositorio(bd);

                        if (solicitud.IDSolicitud > 0)
                        {
                            solicitudRepositorio.Modificar(solicitud);

                            bd.SaveChanges();
                            var tieneCarta = solicitudRepositorio.TieneCartaGenerada(solicitud.IDSolicitud);
                            if (tieneCarta)
                            {
                                new Negocio.Servicios.ServiciosCarta().GenerarCarta(solicitud.IDSolicitud, null);

                                accionDefault = RedirectToAction("DatosPersona", "Validar",
                                    new
                                    {
                                        Area = "Solicitud",
                                        IdSolicitud = solicitud.IDSolicitud.ToString().EncodeTo64()
                                    });
                            }
                        }
                        else
                        {
                            solicitud.Medio = MediosSolicitud.Presencial;

                            if (solicitud.FechaSolicitud == new DateTime()) solicitud.FechaSolicitud = DateTime.Now;

                            bd.Entry(solicitud.Persona).State = solicitud.Persona.IDPersona > 0
                                ? EntityState.Unchanged
                                : EntityState.Added;

                            if (solicitud.Persona.IDPersona > 0)
                            {
                                bd.Entry(solicitud.Persona).Property(p => p.Genero).IsModified = true;
                            }

                            solicitudRepositorio.Guardar(solicitud);

                            bd.SaveChanges();
                        }

                        Archivo.GuardarPublicoPapeleta(archivoPapeleta, Archivo.Ruta.SolicitudDocumentos, nombrePapeleta);

                        correcto = true;

                        GuardarMovimiento("Creación", "Solicitud", solicitud.IDSolicitud);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.ToString());
                }
            }

            if (solicitud.Medio == MediosSolicitud.Web)
            {
                ((RedirectToRouteResult)accionDefault).RouteValues.Add("p", "w");
            }

            var mensajeError = pagoCorrecto == false
                ? "Folio ingresado es inválido o no se tiene pago registrado."
                : string.Format(General.FalloGuardado, "Solicitud");

            EnviarAlerta(General.GuardadoCorrecto, mensajeError, correcto);

            var solicitudViewModel = new SolicitudViewModel { Solicitud = solicitud };

            if (solicitudViewModel.Solicitud.Entidad == null) solicitudViewModel.Solicitud.Entidad = new Entidad();

            if (correcto == false) accionDefault = View(solicitudViewModel);

            return accionDefault;
        }



        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerEntidades(int Tipo)
        {
            var lista = new JsonResult();
            using (var bd = new Contexto())
            {
                var solicitudRepositorio = new SolicitudRepositorio(bd);
                lista = Json(solicitudRepositorio.BuscarEntidadPorTipo(Tipo));
                return lista;
            }
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerPersonaPorRFC(string rfc)
        {
            using (var bd = new Contexto())
            {
                var personaRepositorio = new PersonaRepositorio(bd);
                var personas = personaRepositorio.BuscarPorRfc(rfc);

                return Json(personas, JsonRequestBehavior.AllowGet);
            }
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerSolicitudPorFolio(string folio)
        {
            JsonResult solicitud = new JsonResult();
            using (var bd = new Contexto())
            {
                SolicitudRepositorio _solicitudRepositorio = new SolicitudRepositorio(bd);
                solicitud = Json(_solicitudRepositorio.BuscarSolicitudPorFolio(folio));
                return solicitud;
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
                    Nombre = (string)null,
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
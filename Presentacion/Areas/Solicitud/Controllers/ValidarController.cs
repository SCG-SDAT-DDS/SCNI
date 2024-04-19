using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Carta;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Solicitudes;
using Presentacion.Controllers;
using Resources;
using Sistema.Extensiones;
using Sistema.Paginador;
using Sistema;
using System.Web.Services;


namespace Presentacion.Areas.Solicitud.Controllers
{
    public class ValidarController : ControladorBase
    {
        private SolicitudRepositorio _solicitudRepositorio;
        private SancionRepositorio _sancionRepositorio;

        [HttpGet]
        public ActionResult Index(string estado)
        {
            var solicitudViewModel = new SolicitudViewModel {
                Solicitud = new Datos.Solicitud()
            };

            if (estado == "p" || estado == "a")
            {
                solicitudViewModel.Pendientes = estado == "p";
                solicitudViewModel.Atrasadas = estado == "a";
            }
            else
            {
                solicitudViewModel.Pendientes = true;
                solicitudViewModel.Atrasadas = true;
            }

            try
            {
                using (var bd = new Contexto())
                {
                    solicitudViewModel.Solicitud = new Datos.Solicitud
                    {
                        Carta = new List<Datos.Carta>(),
                        Entidad = new Entidad(),
                        Persona = new Persona()
                    };

                    _solicitudRepositorio = new SolicitudRepositorio(bd);

                    solicitudViewModel.Solicitud.Medio = Request.QueryString["p"] == "w"
                        ? MediosSolicitud.Web
                        : MediosSolicitud.Presencial;
                    solicitudViewModel.SinFirmar = true;

                    solicitudViewModel.Solicitudes = _solicitudRepositorio.Buscar(solicitudViewModel);
                    solicitudViewModel.TotalEncontrados = _solicitudRepositorio.ObtenerTotalRegistros(solicitudViewModel);
                    solicitudViewModel.Paginas = Paginar.ObtenerCantidadPaginas(solicitudViewModel.TotalEncontrados,
                                                    solicitudViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            return View(solicitudViewModel);
        }

        [HttpPost]
        public ActionResult Index(SolicitudViewModel solicitudViewModel, string pagina, string pendientes, string atrasadas)
        {
            solicitudViewModel.PaginaActual = pagina.TryToInt();
            solicitudViewModel.Pendientes = pendientes == "true";
            solicitudViewModel.Atrasadas = atrasadas == "true";

            solicitudViewModel.Solicitud = new Datos.Solicitud
            {
                Carta = new List<Datos.Carta>(),
                Entidad = new Entidad(),
                Persona = new Persona()
            };

            using (var bd = new Contexto())
            {
                _solicitudRepositorio = new SolicitudRepositorio(bd);

                solicitudViewModel.Solicitud.Medio = Request.QueryString["p"] == "w"
                    ? MediosSolicitud.Web
                    : MediosSolicitud.Presencial;
                solicitudViewModel.SinFirmar = true;

                solicitudViewModel.Solicitudes = _solicitudRepositorio.Buscar(solicitudViewModel);
                solicitudViewModel.TotalEncontrados = _solicitudRepositorio.ObtenerTotalRegistros(solicitudViewModel);
                solicitudViewModel.Paginas = Paginar.ObtenerCantidadPaginas(solicitudViewModel.TotalEncontrados,
                                                solicitudViewModel.TamanoPagina);
            }

            ModelState.Clear();

            return View(solicitudViewModel);
        }

        [HttpGet]
        public ActionResult DatosPersona()
        {
            var validacionViewModel = new ValidacionViewModel
            {
                Solicitud = new Datos.Solicitud
                {
                    IDSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud)
                }
            };
            
            if (validacionViewModel.Solicitud.IDSolicitud <= 0)
            {
                EscribirError("El identificador de la solicitud es incorrecto.");
                return RedirectToAction("Index", "Inicio", new {Area = string.Empty});
            }

            using (var bd = new Contexto())
            {
                _solicitudRepositorio = new SolicitudRepositorio(bd);
                _sancionRepositorio = new SancionRepositorio(bd);

                validacionViewModel.Solicitud = _solicitudRepositorio.BuscarPorId(validacionViewModel.Solicitud.IDSolicitud);

                if (validacionViewModel.Solicitud == null ||
                    validacionViewModel.Solicitud.Carta.Any(c => c.Estado == EstadosCarta.Firmada))
                {
                    return RedirectToAction("Index");
                }

                var nombre = new List<string>
                {
                    validacionViewModel.Solicitud.Persona.ApellidoP,
                    validacionViewModel.Solicitud.Persona.ApellidoM,
                    validacionViewModel.Solicitud.Persona.Nombre
                };

                nombre = nombre.Where(n => !string.IsNullOrEmpty(n)).ToList();

                var persona = new Persona
                {
                    Nombre = string.Join(" ", nombre),
                    RFC = validacionViewModel.Solicitud.Persona.RFC
                };

                var sancionViewModel = new SancionViewModel
                {
                    TamanoPagina = 0,
                    PaginaActual = 0,
                    Sancion = new Sancion
                    {
                        Persona = persona,
                        Habilitado = true
                    }
                };

                validacionViewModel.Persona = persona;

                validacionViewModel.Sanciones = _sancionRepositorio.Buscar(sancionViewModel);
                validacionViewModel.SancionesSeleccion = new List<Sancion>();

                var carta = validacionViewModel.Solicitud.Carta.SingleOrDefault();
                if (carta != null)
                {
                    var repositorioCarta = new SancionRepositorio(bd);
                    validacionViewModel.SancionesSeleccion = repositorioCarta.ObtenerSancionesCarta(carta.IDCarta);
                }
            }

            validacionViewModel.UrlDescargarArchivo = ObtenerUrlDescargar("Precarta");

            return View(validacionViewModel);
        }

        [HttpPost]
        public ActionResult DatosPersona(ValidacionViewModel validacionViewModel)
        {
            var idSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

            if (idSolicitud <= 0)
            {
                EscribirError("El identificador de la solicitud es incorrecto.");
                return RedirectToAction("Index", "Inicio", new { Area = string.Empty });
            }

            using (var bd = new Contexto())
            {

                _solicitudRepositorio = new SolicitudRepositorio(bd);
                _sancionRepositorio = new SancionRepositorio(bd);

                validacionViewModel.Solicitud = _solicitudRepositorio.BuscarPorId(idSolicitud);

                if (validacionViewModel.Solicitud == null ||
                    validacionViewModel.Solicitud.Carta.Any(c => c.Estado == EstadosCarta.Firmada))
                {
                    return RedirectToAction("Index");
                }

                var nombre = new List<string>
                {
                    validacionViewModel.Persona.ApellidoP,
                    validacionViewModel.Persona.ApellidoM,
                    validacionViewModel.Persona.Nombre
                };

                nombre = nombre.Where(n => !string.IsNullOrEmpty(n)).ToList();

                var persona = new Persona
                {
                    Nombre = string.Join(" ", nombre),
                    RFC = validacionViewModel.Persona.RFC
                };

                var sancionViewModel = new SancionViewModel
                {
                    PaginaActual = 0,
                    TamanoPagina = 0,
                    Sancion = new Sancion
                    {
                        Persona = persona,
                        Habilitado = true
                    }
                };

                validacionViewModel.Sanciones = _sancionRepositorio.Buscar(sancionViewModel);
                if (validacionViewModel.SancionesSeleccion == null)
                {
                    validacionViewModel.SancionesSeleccion = new List<Sancion>();
                }
                else
                {
                    for (var i = 0; i < validacionViewModel.SancionesSeleccion.Count; i++)
                    {
                        validacionViewModel.SancionesSeleccion[i] = _sancionRepositorio.BuscarPorId(validacionViewModel.SancionesSeleccion[i].IDSancion);
                    }
                }
                if (!string.IsNullOrEmpty(validacionViewModel.IDSanciones))
                {
                    var sanciones = validacionViewModel.IDSanciones.Split(',');
                    foreach (var id in sanciones)
                    {
                        if (!validacionViewModel.SancionesSeleccion.Exists(s => s.IDSancion == (Math.Abs(int.Parse(id)))))
                        {
                            if (int.Parse(id) > 0)
                            {
                                validacionViewModel.SancionesSeleccion.Add(_sancionRepositorio.BuscarPorId(Math.Abs(int.Parse(id))));
                            }
                        }
                        else
                        {
                            if (int.Parse(id) < 0)
                            {
                                var item = validacionViewModel.SancionesSeleccion.Find(x => x.IDSancion == Math.Abs(int.Parse(id)));
                                validacionViewModel.SancionesSeleccion.Remove(item);
                            }
                        }
                    }
                }
                for (var i = validacionViewModel.Sanciones.Count - 1; i >= 0; i--)
                {
                    for (var j = 0; j < validacionViewModel.SancionesSeleccion.Count; j++)
                    {
                        if (validacionViewModel.Sanciones[i].IDSancion == validacionViewModel.SancionesSeleccion[j].IDSancion)
                        {
                            validacionViewModel.Sanciones.RemoveAt(i);
                            j = validacionViewModel.SancionesSeleccion.Count;
                        }
                    }
                }
            }

            validacionViewModel.UrlDescargarArchivo = ObtenerUrlDescargar("Precarta");

            ModelState.Clear();

            return View(validacionViewModel);
        }

        [HttpPost]
        public ActionResult GenerarCarta(ValidacionViewModel validacionViewModel)
        {
            var idSolicitud = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSolicitud);

            bool esNueva;
            using (var bd = new Contexto())
            {
                _solicitudRepositorio = new SolicitudRepositorio(bd);
                _sancionRepositorio = new SancionRepositorio(bd);

                validacionViewModel.Solicitud = _solicitudRepositorio.BuscarPorId(idSolicitud);

                validacionViewModel.Solicitud.Cancelada = false;
                
                if (validacionViewModel.SancionesSeleccion == null)
                {
                    validacionViewModel.SancionesSeleccion = new List<Sancion>();
                }
                else
                {
                    for (var i = 0; i < validacionViewModel.SancionesSeleccion.Count; i++)
                    {
                        validacionViewModel.SancionesSeleccion[i] = _sancionRepositorio
                            .BuscarPorId(validacionViewModel.SancionesSeleccion[i].IDSancion);
                    }
                }

                var items = new int[validacionViewModel.SancionesSeleccion.Count];

                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = validacionViewModel.SancionesSeleccion[i].IDSancion;
                }

                esNueva = new Negocio.Servicios.ServiciosCarta().GenerarCarta(idSolicitud, items);

                validacionViewModel.CartaGenerada = true;
            }

            EnviarAlerta("Carta generada correctamente", string.Format(General.FalloGuardado, "Validación"), true);
            
            LimpiarModelState();
            ModelState.Clear();

            validacionViewModel.UrlDescargarArchivo = ObtenerUrlDescargar("Precarta");

            return esNueva ? RedirectToAction("Index") : RedirectToAction("Index", "Buscar", new {Area = "Carta"});
        }

        [WebMethod]
        [HttpPost]
        public JsonResult AsignarSancionFederal(string tipoSancion, string anio, string tiempoAnio, string tiempoMes, string tiempoDia, string monto,
            string idPersona, string fechaEjecutoria, string periodoInicial, string periodoFinal)
        {
            try
            {
                using (var bd = new Contexto())
                {
                    var entidadRepositorio = new EntidadRepositorio(bd);
                    var sancionRepositorio = new SancionRepositorio(bd);
                    
                    var sancion = new Sancion
                    {
                        NumeroExpediente = string.Empty,
                        FechaEjecutoria = DateTime.Parse(fechaEjecutoria),
                        Año = int.Parse(anio),
                        TipoSancion = int.Parse(tipoSancion),
                        TiempoAños = int.Parse(tiempoAnio),
                        TiempoMeses = int.Parse(tiempoMes),
                        TiempoDias = int.Parse(tiempoDia),
                        Monto = decimal.Parse(monto == "" ? "0" : monto),
                        Persona = new Persona {IDPersona = idPersona.TryToInt()},
                        Entidad = entidadRepositorio.ObtenerDefault(),
                        Habilitado = true
                    };

                    if (string.IsNullOrEmpty(periodoInicial) == false && string.IsNullOrEmpty(periodoFinal) == false)
                    {
                        sancion.PeriodoInicial = DateTime.Parse(periodoInicial);
                        sancion.PeriodoFinal = DateTime.Parse(periodoFinal);
                    }

                    bd.Entry(sancion.Persona).State = System.Data.Entity.EntityState.Unchanged;

                    sancionRepositorio.Guardar(sancion);

                    if (bd.SaveChanges() > 0)
                    {
                        return Json(new
                        {
                            sancion.IDSancion,
                            NumeroExpediente = string.Empty,
                            Tipo = Listas.ObtenerValorDeLista(Listas.ObtenerListaSancionesFederal(),
                                sancion.TipoSancion),
                            Anio = sancion.Año,
                            NombrePersona = sancion.Persona.ApellidoP + " " + sancion.Persona.ApellidoM + " " +
                                            sancion.Persona.Nombre,
                            NombreEntidad = sancion.Entidad.Nombre
                        });
                    }

                    return Json(new { IDSancion = 0 });
                }
            }
            catch (Exception) {
                return Json(new { IDSancion = 0 });
            }
        }

        [WebMethod]
        [HttpPost]
        public ActionResult CancelarSolicitud(int idSolicitud, string motivoCancelacion)
        {
            using (var db = new Contexto())
            {
                var repositorio = new SolicitudRepositorio(db);

                var solicitud = repositorio.BuscarPorId(idSolicitud);
                
                repositorio.CancelarSolicitud(solicitud, motivoCancelacion);
                
                db.SaveChanges();

                if (string.IsNullOrEmpty(solicitud.Persona.CorreoElectronico) == false)
                {
                    Correo.AvisarCancelacionSolitud(solicitud.Persona.CorreoElectronico, solicitud.FolioPago, motivoCancelacion);

                    EscribirMensaje("Solicitud cancelada y correo electrónico enviado correctamente.");
                }
                else
                {
                    EscribirMensaje("Solicitud cancelada correctamente.");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
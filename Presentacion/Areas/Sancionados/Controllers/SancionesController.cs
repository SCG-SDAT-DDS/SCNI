using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.DTO.Reportes;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Solicitudes;
using Negocio.Servicios;
using Presentacion.Controllers;
using Resources;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Sancionados.Controllers
{
    public class SancionesController : ControladorBase
    {
        private SancionRepositorio _sancionRepositorio;

        // GET: Sancionados/Sanciones
        [HttpGet]
        public ActionResult Index(SancionViewModel sancionViewModel)
        {
            using (var bd = new Contexto())
            {
                sancionViewModel.Sancion = new Sancion {
                    Entidad = new Entidad(),
                    Persona = new Persona {
                        Municipio = new Municipio()
                    },
                    Habilitado = true
                };

                sancionViewModel.EsEstatal = true;

                _sancionRepositorio = new SancionRepositorio(bd);
                sancionViewModel.Sanciones = _sancionRepositorio.Buscar(sancionViewModel);
                sancionViewModel.TotalEncontrados = _sancionRepositorio.ObtenerTotalRegistros(sancionViewModel);
                sancionViewModel.Paginas = Paginar.ObtenerCantidadPaginas(sancionViewModel.TotalEncontrados,
                                                sancionViewModel.TamanoPagina);
            } 

            return View(sancionViewModel);
        }

        // POST: Sancionados/Sanciones
        [HttpPost]
        public ActionResult Index(SancionViewModel sancionViewModel, string pagina)
        {
            sancionViewModel.PaginaActual = pagina.TryToInt();

            sancionViewModel.Sancion.Persona = sancionViewModel.Sancion.Persona ?? new Persona();
            sancionViewModel.Sancion.Entidad = sancionViewModel.Sancion.Entidad ?? new Entidad();

            if (sancionViewModel.PaginaActual <= 0) sancionViewModel.PaginaActual = 1;

            sancionViewModel.EsEstatal = true;

            if (string.IsNullOrEmpty(sancionViewModel.Imprimir))
            {
                using (var bd = new Contexto())
                {
                    _sancionRepositorio = new SancionRepositorio(bd);
                    sancionViewModel.Sanciones = _sancionRepositorio.Buscar(sancionViewModel);
                    sancionViewModel.TotalEncontrados = _sancionRepositorio.ObtenerTotalRegistros(sancionViewModel);
                    sancionViewModel.Paginas = Paginar.ObtenerCantidadPaginas(sancionViewModel.TotalEncontrados,
                                                    sancionViewModel.TamanoPagina);
                }

                LimpiarModelState();

                return View(sancionViewModel);
            }
            else
            {
                List<ReporteSancionadoDto> sanciones;

                using (var bd = new Contexto())
                {
                    _sancionRepositorio = new SancionRepositorio(bd);
                    sanciones = _sancionRepositorio.ObtenerReporteSancionado(sancionViewModel);
                }

                var servicios = new ServiciosReporte();
                var reporte = servicios.GenerarReporteSancionado(sancionViewModel.Sancion.Persona.Nombre, sanciones);

                var nombre = "_";
                if (sanciones.Any())
                {
                    var primerSancion = sanciones.First();
                    nombre += primerSancion.Nombre + "_" +
                              primerSancion.Paterno.FirstOrDefault() +
                              primerSancion.Materno.FirstOrDefault();
                }

                return File(reporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sanciones" + nombre + ".xlsx");
            }
        }

        // GET: Sancionados/Sanciones/Guardar
        [HttpGet]
        public ActionResult Guardar(string idSancion)
        {
            var sancionViewModel = new SancionViewModel();

            using (var bd = new Contexto())
            {
                _sancionRepositorio = new SancionRepositorio(bd);
                var sancion = _sancionRepositorio.BuscarPorId(idSancion.DecodeFrom64().TryToInt());

                if (sancion != null)
                {
                    sancionViewModel.Sancion = sancion;
                    sancion.EspecificarOtro = sancion.EspecificarOtro ?? "";
                    sancion.Observaciones = sancion.Observaciones ?? "";
                    sancion.Persona.Genero = sancion.Persona.Genero.Trim();
                }
                else
                {
                    sancionViewModel = new SancionViewModel
                    {
                        Sancion = new Sancion
                        {
                            EspecificarOtro = "",
                            Observaciones = "",
                            Habilitado = true,
                            FechaInscripcion = DateTime.Now,
                            FechaEjecutoria = DateTime.Now,
                            FechaResolucion = DateTime.Now,
                            Entidad = new Entidad(),
                            Persona = new Persona
                            {
                                Municipio = new Municipio
                                {
                                    Estado = new Estado()
                                }
                            }
                        }
                    };
                }
            }

            return View(sancionViewModel);
        }

        // POST: Sancionados/Sanciones/Guardar
        [HttpPost]
        public ActionResult Guardar(Sancion sancion)
        {
            // Definición de variables
            bool correcto = false;

            sancion.IDSancion = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSancion);

            // Crear contexto y abrir conexión
            using (var bd = new Contexto())
            {
                // Configuraciones previas de la conexión
                bd.Configuration.ValidateOnSaveEnabled = false;

                bd.Entry(sancion.Entidad).State = (sancion.Entidad.IDEntidad > 0 ? EntityState.Unchanged : EntityState.Added);
                bd.Entry(sancion.Persona).State = (sancion.Persona.IDPersona > 0 ? EntityState.Modified : EntityState.Added);

                // Configuraciones previas del modelo
                _sancionRepositorio = new SancionRepositorio(bd);
                sancion.FechaEjecutoria = (sancion.FechaEjecutoria == new DateTime() ? DateTime.Now : sancion.FechaEjecutoria);
                sancion.FechaResolucion = (sancion.FechaResolucion == new DateTime() ? DateTime.Now : sancion.FechaResolucion);
                sancion.Persona.CorreoElectronico = sancion.Persona.CorreoElectronico ?? string.Empty;
                sancion.Persona.Municipio = new Municipio { IDMunicipio = 30, IDEstado = 26 };
                if (sancion.Persona.ApellidoM == null) sancion.Persona.ApellidoM = string.Empty;

                bd.Entry(sancion.Persona.Municipio).State = EntityState.Unchanged;

                var _movimiento = "Creación";
                if (sancion.IDSancion > 0)
                {
                    // Modificar Solicitud existente
                    sancion.IDEntidad = sancion.Entidad.IDEntidad;
                    //bd.Persona.Attach(sancion.Persona);
                    _sancionRepositorio.Modificar(sancion);
                    _movimiento = "Modificación";
                }
                else
                {
                    // Crear Solicitud nueva
                    _sancionRepositorio.Guardar(sancion);
                }

                // Guardar cambios realizados al contexto
                correcto = bd.SaveChanges() >= 1;
                if (correcto)
                {
                    GuardarMovimiento(_movimiento, "Sanción", sancion.IDSancion);
                }
            }

            // Crear mensajes de alerta
            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, "Sanción"), correcto);

            return RedirectToAction("Guardar");
        }

        // GET: Sancionados/Sanciones
        [HttpGet]
        public ActionResult BuscarFederales(SancionViewModel sancionViewModel)
        {
            using (var bd = new Contexto())
            {
                sancionViewModel.Sancion = new Sancion
                {
                    Entidad = new Entidad {
                        Tipo = 3
                    },
                    Persona = new Persona
                    {
                        Municipio = new Municipio()
                    },
                    Habilitado = true
                };

                _sancionRepositorio = new SancionRepositorio(bd);
                sancionViewModel.Sanciones = _sancionRepositorio.Buscar(sancionViewModel);
                sancionViewModel.TotalEncontrados = _sancionRepositorio.ObtenerTotalRegistros(sancionViewModel);
                sancionViewModel.Paginas = Paginar.ObtenerCantidadPaginas(sancionViewModel.TotalEncontrados,
                                                sancionViewModel.TamanoPagina);
            }

            return View(sancionViewModel);
        }

        // POST: Sancionados/Sanciones
        [HttpPost]
        public ActionResult BuscarFederales(SancionViewModel sancionViewModel, string pagina)
        {
            sancionViewModel.PaginaActual = pagina.TryToInt();

            sancionViewModel.Sancion.Persona = sancionViewModel.Sancion.Persona ?? new Persona();
            if (sancionViewModel.Sancion.Entidad != null)
            {
                sancionViewModel.Sancion.Entidad = sancionViewModel.Sancion.Entidad;
                sancionViewModel.Sancion.Entidad.Tipo = 3;
            }
            else
            {
                sancionViewModel.Sancion.Entidad = new Entidad { Tipo = 3 };
            }

            if (sancionViewModel.PaginaActual <= 0) sancionViewModel.PaginaActual = 1;
            
            LimpiarModelState();

            if (string.IsNullOrEmpty(sancionViewModel.Imprimir))
            {
                using (var bd = new Contexto())
                {

                    _sancionRepositorio = new SancionRepositorio(bd);
                    sancionViewModel.Sanciones = _sancionRepositorio.Buscar(sancionViewModel);
                    sancionViewModel.TotalEncontrados = _sancionRepositorio.ObtenerTotalRegistros(sancionViewModel);
                    sancionViewModel.Paginas = Paginar.ObtenerCantidadPaginas(sancionViewModel.TotalEncontrados,
                                                    sancionViewModel.TamanoPagina);
                }

                return View(sancionViewModel);
            }
            else
            {
                List<ReporteSancionadoDto> sanciones;

                using (var bd = new Contexto())
                {
                    _sancionRepositorio = new SancionRepositorio(bd);
                    sanciones = _sancionRepositorio.ObtenerReporteSancionado(sancionViewModel);
                }

                var servicios = new ServiciosReporte();
                var reporte = servicios.GenerarReporteSancionado(sancionViewModel.Sancion.Persona.Nombre, sanciones);

                var nombre = "_";
                if (sanciones.Any())
                {
                    var primerSancion = sanciones.First();
                    nombre += primerSancion.Nombre + "_" +
                              primerSancion.Paterno.FirstOrDefault() +
                              primerSancion.Materno.FirstOrDefault();
                }

                return File(reporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sanciones" + nombre + ".xlsx");
            }
        }

        // GET: Sancionados/Sanciones/Guardar
        [HttpGet]
        public ActionResult GuardarFederal(string idSancion)
        {
            var sancionViewModel = new SancionViewModel();

            using (var bd = new Contexto())
            {
                _sancionRepositorio = new SancionRepositorio(bd);
                var sancion = _sancionRepositorio.BuscarPorId(idSancion.DecodeFrom64().TryToInt());

                if (sancion != null)
                {
                    sancionViewModel.Sancion = sancion;
                    sancion.EspecificarOtro = sancion.EspecificarOtro ?? "";
                    sancion.Observaciones = sancion.Observaciones ?? "";
                    sancion.Persona.Genero = sancion.Persona.Genero.Trim();
                }
                else
                {
                    var entidadRepositorio = new EntidadRepositorio(bd);
                    var entidadDefault = entidadRepositorio.ObtenerDefault();

                    sancionViewModel = new SancionViewModel
                    {
                        Sancion = new Sancion
                        {
                            EspecificarOtro = "",
                            Observaciones = "",
                            Habilitado = true,
                            FechaInscripcion = DateTime.Now,
                            FechaEjecutoria = DateTime.Now,
                            FechaResolucion = DateTime.Now,
                            Entidad = entidadDefault,
                            Persona = new Persona
                            {
                                Municipio = new Municipio
                                {
                                    Estado = new Estado()
                                }
                            }
                        }
                    };
                }
            }

            return View(sancionViewModel);
        }

        // POST: Sancionados/Sanciones/Guardar
        [HttpPost]
        public ActionResult GuardarFederal(Sancion sancion)
        {
            // Definición de variables
            bool correcto;
            
            sancion.IDSancion = ObtenerParametroGetEnInt(Enumerados.Parametro.IdSancion);

            // Crear contexto y abrir conexión
            using (var bd = new Contexto())
            {
                // Configuraciones previas de la conexión
                bd.Configuration.ValidateOnSaveEnabled = false;
                bd.Entry(sancion.Entidad).State = (sancion.Entidad.IDEntidad > 0 ? EntityState.Unchanged : EntityState.Added);
                bd.Entry(sancion.Persona).State = (sancion.Persona.IDPersona > 0 ? EntityState.Modified : EntityState.Added);

                // Configuraciones previas del modelo
                _sancionRepositorio = new SancionRepositorio(bd);
                sancion.FechaEjecutoria = (sancion.FechaEjecutoria == new DateTime() ? DateTime.Now : sancion.FechaEjecutoria);
                sancion.FechaResolucion = (sancion.FechaResolucion == new DateTime() ? DateTime.Now : sancion.FechaResolucion);
                sancion.Persona.CorreoElectronico = string.Empty;
                sancion.Persona.Municipio = new Municipio { IDMunicipio = 30, IDEstado = 26 };
                if (sancion.Persona.ApellidoM == null) sancion.Persona.ApellidoM = string.Empty;

                bd.Entry(sancion.Persona.Municipio).State = EntityState.Unchanged;

                var _movimiento = "Creación";
                if (sancion.IDSancion > 0)
                {
                    // Modificar Solicitud existente
                    sancion.IDEntidad = sancion.Entidad.IDEntidad;
                    //bd.Persona.Attach(sancion.Persona);
                    _sancionRepositorio.Modificar(sancion);
                    _movimiento = "Modificación";
                }
                else
                {
                    // Crear Solicitud nueva
                    _sancionRepositorio.Guardar(sancion);
                }

                // Guardar cambios realizados al contexto
                correcto = bd.SaveChanges() >= 1;
                if (correcto)
                {
                    GuardarMovimiento(_movimiento, "Sanción", sancion.IDSancion);
                }
            }

            // Crear mensajes de alerta
            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, "Sanción"), correcto);

            return RedirectToAction("GuardarFederal");
        }

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
    }
}
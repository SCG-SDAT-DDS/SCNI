using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Solicitudes;
using Presentacion.Controllers;
using Resources;
using Sistema.Extensiones;
using Sistema.Paginador;

namespace Presentacion.Areas.Solicitud.Controllers
{
    public class PersonaController : ControladorBase
    {
        PersonaRepositorio _personaRepositorio;

        // POST: Solicitud/Persona
        [WebMethod]
        [HttpPost]
        public JsonResult ObtenerPersonaPorID(object IDPersona)
        {
            JsonResult persona = new JsonResult();
            using (var bd = new Contexto())
            {
                _personaRepositorio = new PersonaRepositorio(bd);
                persona = Json(_personaRepositorio.ObtenerPorId(IDPersona));
                return persona;
            }
        }
        // POST: Solicitud/Persona
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
    }
}
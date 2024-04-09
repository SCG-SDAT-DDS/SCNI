using System;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Web.Http;
using Datos;
using Datos.DTO.Servicios;
using Datos.Repositorios.Catalogos;

namespace Presentacion.Controllers
{
    [RoutePrefix("S3/Resp_API_Servidores_Publicos_Sancionados.json")]
    public class ServiciosController : ApiController
    {
        private void _ValidarAcceso()
        {
            var usuario = System.Configuration.ConfigurationManager.AppSettings["UsuarioWs"];
            var contrasena = System.Configuration.ConfigurationManager.AppSettings["ContrasenaWs"];

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
                return;

            string usuarioPeticion = null;
            string contrasenaPeticion = null;

            if (Request.Headers.Contains("usuario"))
            {
                usuarioPeticion = Request.Headers.GetValues("usuario").First();
            }

            if (Request.Headers.Contains("contrasena"))
            {
                contrasenaPeticion = Request.Headers.GetValues("contrasena").First();
            }

            if (usuarioPeticion == null || contrasenaPeticion == null)
            {
                throw new AuthenticationException("Usuario y contraseña son necesarios.");
            }

            if (usuario.Equals(usuarioPeticion, StringComparison.InvariantCulture) == false || 
                contrasena.Equals(contrasenaPeticion, StringComparison.InvariantCulture) == false)
            {
                throw new AuthenticationException("Usuario y/o contraseña incorrecta.");
            }
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult ObtenerSancionesEstatales()
        {
            return ObtenerSancionesEstatales(1, 10);
        }

        [Route("{page}/{pageSize}")]
        [HttpGet]
        public IHttpActionResult ObtenerSancionesEstatales(int page = 1, int pageSize = 10)
        {
            try
            {
                _ValidarAcceso();

                if (page < 1)
                    throw new ArgumentOutOfRangeException(nameof(page),
                        @"Número de página fuera de rango: " + page + @".");

                if (pageSize < 1)
                    throw new ArgumentOutOfRangeException(nameof(pageSize),
                        @"Tamaño de página fuera de rango: " + pageSize + @".");

                var lst = new LstSanciones
                {
                    pagination = new pagination
                    {
                        page = page,
                        pageSize = pageSize
                    }
                };

                using (var contexto = new Contexto())
                {
                    var repositorio = new SancionRepositorio(contexto);

                    lst.results = repositorio.ObtenerSancionesEstatalesServicio(page, pageSize);

                    lst.pagination.total = repositorio.ObtenerSancionesEstatalesServicio();
                }

                return Ok(lst);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}

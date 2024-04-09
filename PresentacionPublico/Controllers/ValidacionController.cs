using System;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios;
using Datos.Repositorios.Catalogos;
using Newtonsoft.Json;
using Sistema;
using Sistema.Extensiones;

namespace Presentacion.Controllers
{
    public class ValidacionController : Controller
    {
        [HttpGet]
        public ActionResult Index(string codigo)
        {
            var viewModel = new BusquedaConstanciaViewModel(codigo) {UrlDescargarCarta = _ObtenerUrlDescargar("Carta")};

            return View("Index", "Layouts/_LayoutValidacion", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(BusquedaConstanciaViewModel viewModel)
        {
            var captchaCorrecto = await _ValidarCaptcha();

            var codigo = string.Empty;

            if (captchaCorrecto)
            {
                if (!string.IsNullOrEmpty(viewModel.Folio))
                    codigo = viewModel.Folio;

                if (!string.IsNullOrEmpty(viewModel.Nombre))
                    codigo += viewModel.Nombre[0];

                if (!string.IsNullOrEmpty(viewModel.Paterno))
                    codigo += viewModel.Paterno[0];

                if (!string.IsNullOrEmpty(viewModel.Materno))
                    codigo += viewModel.Materno[0];
            }
            else
            {
                EnviarAlerta(string.Empty, "Capcha inválido.", false);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new {codigo});
        }

        [HttpGet]
        public ActionResult Constancia(string codigo)
        {
            var viewModel = ValidacionConstanciaViewModel.PorCodigo(codigo);
            viewModel.UrlDescargarCarta = _ObtenerUrlDescargar("Carta");

            return View("Constancia", "Layouts/_LayoutValidacion", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Constancia(ValidacionConstanciaViewModel viewModel)
        {
            var captchaCorrecto = await _ValidarCaptcha();

            if (captchaCorrecto)
            {
                viewModel = ValidacionConstanciaViewModel.PorOficio(viewModel.Oficio);
                viewModel.UrlDescargarCarta = _ObtenerUrlDescargar("Carta");
            }
            else
            {
                EnviarAlerta(string.Empty, "Capcha inválido.", false);
                return RedirectToAction("Constancia");
            }

            return View("Constancia", "Layouts/_LayoutValidacion", viewModel);
        }

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

        private async Task<bool> _ValidarCaptcha()
        {
            var sitioKey = System.Configuration.ConfigurationManager.AppSettings["CaptchaSecreto"];
            var recaptcharesponse = Request["g-recaptcha-response"];

            using (var cliente = new HttpClient())
            {
                var response =
                    await cliente.GetAsync(
                        "https://www.google.com/recaptcha/api/siteverify?secret=" + sitioKey + "&response=" +
                        recaptcharesponse);

                var responseString = await response.Content.ReadAsStringAsync();

                dynamic resultado = JsonConvert.DeserializeObject(responseString);

                return (bool)resultado.success;
            }
        }

        #region "Métodos auxiliares"
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
    }
}
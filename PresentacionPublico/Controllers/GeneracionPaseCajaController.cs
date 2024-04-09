using Datos.DTO.Infraestructura.ViewModels;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Negocio.PaseCaja;

namespace Presentacion.Controllers
{
    public class GeneracionPaseCajaController : Controller
    {
        readonly GeneracionPaseCajaRepositorio _gpcRep = new GeneracionPaseCajaRepositorio();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(PaseCajaRequest model)
        {
            if(await _gpcRep.ValidarCaptcha(
                ConfigurationManager.AppSettings["CaptchaSecreto"], 
                Request["g-recaptcha-response"]))
            {
                try
                {
                    var pase = new PaseCajaRepository();
                    var id = pase.Generar(model);
                    ViewBag.Folio = pase.Folio;
                    ViewBag.UrlSitio = _ObtenerUrlDescargar("PaseCaja");
                    return View("Descargar", id);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Capcha", ex.Message);
                }
            }
            else ModelState.AddModelError("Capcha", "Capcha inválida");
            return View(model);
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
    }
}
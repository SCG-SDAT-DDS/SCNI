using System.Web.Mvc;
using Datos.DTO.Infraestructura.ViewModels;

namespace Presentacion.Controllers
{
    public class InicioController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = InicioViewModal.CargarInformacion();

            return View(viewModel);
        }
    }
}
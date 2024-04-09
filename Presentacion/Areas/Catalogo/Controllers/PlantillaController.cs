using System.Web.Mvc;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Recursos;
using Datos.Repositorios.Catalogos;
using Presentacion.Controllers;
using Sistema;
using Sistema.Extensiones;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class PlantillaController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new PlantillaViewModel();

            using (var bd = new Contexto())
            {
                var repositorio = new PlantillaRepostorio(bd);
                viewModel.Plantillas = repositorio.Obtener();
                viewModel.TotalEncontrados = viewModel.Plantillas.Count;
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idPlantilla)
        {
            var idPlantillaBuscar = idPlantilla.DecodeFrom64().TryToInt();
            Plantilla plantilla = null;

            if (idPlantillaBuscar > 0)
            {
                using (var bd = new Contexto())
                {
                    var repositorio = new PlantillaRepostorio(bd);
                    plantilla = repositorio.BuscarUnoSolo(p => p.IDPlantilla == idPlantillaBuscar);
                }
            }
            GuardarDatoTemporal(Enumerados.TempData.IdModificar, idPlantillaBuscar, true);
            return View(plantilla);
        }

        [HttpPost]
        public ActionResult Guardar(Plantilla plantilla)
        {
            plantilla.IDPlantilla = ObtenerDatoTemporal<int?>(Enumerados.TempData.IdModificar) ?? 0;
            
            var archivo = Request.Files["archivo"];

            if (plantilla.IDPlantilla == 0 && (archivo == null || archivo.ContentLength == 0))
            {
                EnviarAlerta(string.Empty, "Es necesario la plantilla", false);
                return View(plantilla);
            }

            if (archivo != null && archivo.ContentLength > 0)
            {
                plantilla.NombrePlantilla = Archivo.GuardarPlantilla(archivo);

                if (plantilla.NombrePlantilla == null)
                {
                    EnviarAlerta(string.Empty, "Plantilla incorrecta", false);
                    return View(plantilla);
                }
            }

            if (ModelState.IsValid)
            {
                using (var bd = new Contexto())
                {
                    var repositorio = new PlantillaRepostorio(bd);

                    if (plantilla.Actual)
                        repositorio.DesactivarActual();

                    if (plantilla.IDPlantilla > 0)
                        repositorio.Modificar(plantilla);
                    else
                        repositorio.Guardar(plantilla);

                    bd.SaveChanges();

                    var movimiento = plantilla.IDPlantilla > 0 ? "Modificar" : "Guardar";

                    GuardarMovimiento(movimiento, "Plantilla", plantilla.IDPlantilla);
                }
            }

            EnviarAlerta(General.GuardadoCorrecto, string.Empty, true);
            ModelState.Clear();
            return View(new Plantilla());
        }
    }
}
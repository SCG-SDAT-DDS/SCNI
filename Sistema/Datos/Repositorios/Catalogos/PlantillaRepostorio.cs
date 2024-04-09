using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Datos.Repositorios.Catalogos
{
    public class PlantillaRepostorio : Repositorio<Plantilla>
    {
        public PlantillaRepostorio(Contexto contexto) : base(contexto)
        {
        }

        public List<Plantilla> Obtener()
        {
            return (from p in Contexto.Plantillas
                select p).ToList();
        }

        public override void Modificar(Plantilla entidad)
        {
            var plantillaModificar = new Plantilla {IDPlantilla = entidad.IDPlantilla};

            Contexto.Plantillas.Attach(plantillaModificar);

            plantillaModificar.Descripcion = entidad.Descripcion;
            plantillaModificar.NombrePlantilla = entidad.NombrePlantilla;
            plantillaModificar.Actual = entidad.Actual;

            Contexto.Entry(plantillaModificar).Property(p => p.Actual).IsModified = true;
        }

        public Plantilla ObtenerActiva()
        {
            return (from p in Contexto.Plantillas
                where p.Actual
                select p).SingleOrDefault();
        }

        public void DesactivarActual()
        {
            Contexto.Database.ExecuteSqlCommand("UPDATE Plantilla SET Actual = 0");
        }
    }
}

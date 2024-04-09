using System;
using System.Linq;

namespace Datos.Repositorios.Carta
{
    public class FoliosRepositorio : Repositorio<Folio>
    {
        public FoliosRepositorio(Contexto contexto) : base(contexto)
        {
        }

        public Folio NuevoFolio()
        {
            var anio = DateTime.Now.Year; 

            return (from f in Contexto.Folios
                    where f.Anio == anio
                    select f).SingleOrDefault();
        }

        public void ActualizarFolio(int? idFolio)
        {
            if (idFolio.HasValue)
            {
                Contexto.Database.ExecuteSqlCommand("UPDATE Folio SET Numero = Numero + 1 WHERE Anio = " + idFolio);
            }
            else
            {
                Contexto.Folios.Add(new Folio
                {
                    Anio = DateTime.Now.Year,
                    Numero  = 1
                });
            }
        }
    }
}

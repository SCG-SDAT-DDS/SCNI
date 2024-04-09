using System;

namespace Datos.Repositorios.Firma
{
    public class RepositorioLotes : Repositorio<Lote>
    {
        public RepositorioLotes(Contexto contexto) : base(contexto)
        {
        }

        public void CambiarEstado(int idLote, DateTime fecha, LoteEstados estado, string error)
        {
            var lote = new Lote {IDLote = idLote};

            Contexto.Lotes.Attach(lote);

            lote.FechaFin = fecha;
            lote.Estado = estado;
            lote.Error = error;
        }
    }
}

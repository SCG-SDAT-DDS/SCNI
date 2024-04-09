using System;
using System.Linq;
using Datos.DTO;

namespace Datos.Repositorios.Firma
{
    public class RepositorioFirmas : Repositorio<Datos.Firma>
    {
        public RepositorioFirmas(Contexto contexto) : base(contexto)
        {
        }

        public void FirmarCarta(int idCarta, string nombreFirmante, string puestoFirmante, string fecha)
        {
            var cadena = Contexto.Cartas.Where(c => c.IDCarta == idCarta).Select(c => c.Cadena).Single();

            cadena = Datos.Carta.ReemplazarFechaCadena(cadena, fecha);

            var carta = new Datos.Carta {IDCarta = idCarta};

            Contexto.Cartas.Attach(carta);

            carta.Estado = EstadosCarta.Firmada;
            carta.Cadena = cadena + "|" + nombreFirmante + "|" + puestoFirmante;
            carta.Fecha = DateTime.Now;
        }

        public EmpleadoFirmaDto ObtenerNombreFirma(int idCarta)
        {
            return (from f in Contexto.Firmas
                where f.IDCarta == idCarta &&
                      f.Estado == FirmaEstados.Terminado
                select new EmpleadoFirmaDto
                {
                    Nombre = f.Empleado.Titulo.Abreviacion + " " +
                             f.Empleado.Nombre + " " +
                             f.Empleado.ApellidoP + " " +
                             f.Empleado.ApellidoM,
                    Puesto = f.Empleado.Puesto.Descripcion,
                    PuestoCorto = f.Empleado.Puesto.Nombre,
                    Firma = f.Valor,
                    UrlFirma = f.Empleado.UrlFirma
                }).FirstOrDefault();
        }

        public EmpleadoFirmaDto ObtenerNombreFirmanteDefault()
        {
            return (from e in Contexto.Empleado
                    where e.DefaultFirma
                    select new EmpleadoFirmaDto
                    {
                        Nombre = e.Titulo.Abreviacion + " " +
                                 e.Nombre + " " +
                                 e.ApellidoP + " " +
                                 e.ApellidoM,
                        Puesto = e.Puesto.Descripcion,
                        PuestoCorto = e.Puesto.Nombre,
                        Firma = "SIN FIRMA ELECTRÓNICA AVANZADA. VISTA PRELIMINAR.",
                        UrlFirma = e.UrlFirma
                    }).Single();
        }

        public Datos.Firma BuscarPorId(int idFirma)
        {
            var firma = new Datos.Firma();
            firma = (from e in Contexto.Firmas
                     .Include("Carta")
                     .Include("Empleado")
                     where e.IDFirma == idFirma
                     select e).SingleOrDefault();
            return firma;
        }

        public bool ExisteFirma(int idCarta)
        {
            return (from f in Contexto.Firmas
                where f.IDCarta == idCarta &&
                      f.Estado == FirmaEstados.Terminado
                select f).Any();
        }
    }
}

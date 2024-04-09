namespace Negocio.Firma
{
    public class ConstructorFirmaCarta
    {
        public int IdCarta { get; private set; }
        public string Folio { get; private set; }
        public string Digestion { get; private set; }
        public string Pkcs7 { get; private set; }
        public string NombreFirmante { get; private set; }
        public string PuestoFirmante { get; private set; }
        public string Fecha { get; private set; }

        private ConstructorFirmaCarta()
        {
        }

        public FirmaCarta Build()
        {
            return new FirmaCarta(this);
        }

        public static ConstructorFirmaCarta FirmaCarta()
        {
            return new ConstructorFirmaCarta();
        }

        public ConstructorFirmaCarta AgregarIdCarta(int idCarta)
        {
            IdCarta = idCarta;
            return this;
        }

        public ConstructorFirmaCarta AgregarFolio(string folio)
        {
            Folio = folio;
            return this;
        }

        public ConstructorFirmaCarta AgregarDigestion(string digestion)
        {
            Digestion = digestion;
            return this;
        }

        public ConstructorFirmaCarta AgregarPkcs7(string pkcs7)
        {
            Pkcs7 = pkcs7;
            return this;
        }

        public ConstructorFirmaCarta AgregarNombreFirmante(string nombreFirmante)
        {
            NombreFirmante = nombreFirmante;
            return this;
        }

        public ConstructorFirmaCarta AgregarPuestoFirmante(string puestoFirmante)
        {
            PuestoFirmante = puestoFirmante;
            return this;
        }

        public ConstructorFirmaCarta AgregarFecha(string fecha)
        {
            Fecha = fecha;
            return this;
        }
    }
}

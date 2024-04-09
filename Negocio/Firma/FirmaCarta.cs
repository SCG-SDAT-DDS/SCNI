namespace Negocio.Firma
{
    public  class FirmaCarta
    {
        public FirmaCarta(ConstructorFirmaCarta constructor)
        {
            IdCarta = constructor.IdCarta;
            Folio = constructor.Folio;
            Digestion = constructor.Digestion; //OperacionesFiel.ConvertirDigestionBytes(constructor.Digestion);
            Pkcs7 = OperacionesFiel. ConvertirPkcs7Bytes(constructor.Pkcs7);
            Firma = constructor.Pkcs7;
            NombreFirmante = constructor.NombreFirmante;
            PuestoFirmante = constructor.PuestoFirmante;
            Fecha = constructor.Fecha;
        }

        public int IdCarta { get; private set; }
        public string Folio { get; private set; }
        public string Digestion { get; private set; }
        public byte[] Pkcs7 { get; private set; }
        public string NombreFirmante { get; private set; }
        public string PuestoFirmante { get; private set; }
        public string Fecha { get; private set; }
        public string Firma { get; private set; }
    }
}
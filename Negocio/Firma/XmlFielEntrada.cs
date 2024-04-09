
using System;

namespace Negocio.Firma
{
    internal class XmlFielEntrada
    {
        public string Folio { get; set; }
        public string Digestion { get; set; }
        public DateTime Fecha { get; set; }
        public string[] Pkcs7 { get; set; }
    }
}

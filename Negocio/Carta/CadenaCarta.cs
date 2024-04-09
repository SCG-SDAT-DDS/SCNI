using System;
using System.Collections.Generic;
using System.Globalization;
using Datos;

namespace Negocio.Carta
{
    public class CadenaCarta
    {
        private string _folio;
        private DateTime _fecha;
        private string _nombre;
        private string _textoEstatal;
        private string _textoFederal;
        private TiposCarta _tipoCarta;

        public CadenaCarta AgregarTipo(TiposCarta tipoCarta)
        {
            _tipoCarta = tipoCarta;
            return this;
        }

        public CadenaCarta AgregarFolio(string folio)
        {
            _folio = folio;
            return this;
        }

        public CadenaCarta AgregarFecha(DateTime fecha)
        {
            _fecha = fecha;
            return this;
        }

        public CadenaCarta AgregarNombre(string nombre)
        {
            _nombre = nombre.ToUpper();
            return this;
        }

        public CadenaCarta AgregarTextoEstatal(string textoEstatal)
        {
            _textoEstatal = textoEstatal;
            return this;
        }

        public CadenaCarta AgregarTextoFederal(string textoFederal)
        {
            _textoFederal = textoFederal;
            return this;
        }

        public string GenerarCadena()
        {
            var partes = new List<string>
            {
                _folio,
                _fecha.Day + " " + _fecha.ToString(@"\de MMMM \de yyyy",  new CultureInfo("es-ES")),
                _tipoCarta == TiposCarta.Inhabilitacion ? "INHABILITACIÓN" : "NO INHABILITACIÓN",
                _nombre,
                _textoEstatal
            };

            if (_textoFederal != null)
            {
                partes.Add(_textoFederal);   
            }

            return string.Join("|", partes);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Datos {
    [MetadataType(typeof(CartaMedaData))]
    public partial class Carta
    {public static string ReemplazarFechaCadena(string cadena, string fecha)
        {
            var partesCadena = cadena.Split('|');
            
            partesCadena[1] = fecha;

            cadena = string.Join("|", partesCadena);

            return cadena;
        }
    }

    public class CartaMedaData
    {
        [Display(Name = @"Oficio")]
        public string NumeroExpediente { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class CartaViewModel : PaginacionViewModel
    {
        public Carta Carta { get; set; }
        public List<Carta> Cartas { get; set; } 
        public List<int> IdCartasFirmar { get; set; } 
        public List<string> Pkcs7s { get; set; }
        public List<string> Digestiones { get; set; }
        public List<string> Folios { get; set; }
        public List<string> Fechas { get; set; }
        public string Enviar { get; set; }
        public string Rechazar { get; set; }
        [Display(Name = @"Fecha Inicio")]
        public DateTime? FechaInicio { get; set; }
        [Display(Name = @"Fecha Fin")]
        public DateTime? FechaFin { get; set; }
        public string UrlDescargarArchivo { get; set; }
        public string SerieCertificado { get; set; }
        public string NombreFirmante { get; set; }
        public string PuestoFirmante { get; set; }
        public int IdEmpleado { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class PaseCajaRequest
    {
        [Required(ErrorMessage = "Requerido")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string Email     { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Nombre    { get; set; }
    }
}
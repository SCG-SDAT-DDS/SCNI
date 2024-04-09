using System;
using System.Collections.Generic;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class PrecioViewModel : PaginacionViewModel
    {
        public Precio Precio { get; set; }
        public List<Precio> Precios { get; set; }
        public string PrecioGlobal { get; set; }
    }
}
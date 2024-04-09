using System.Collections.Generic;
using Datos.Repositorios.Catalogos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class EmpleadoViewModel : PaginacionViewModel
    {
        public Enumerados.Parametro EmpleadoID;
        public EmpleadoViewModel()
        {
            
        }

        
        public EmpleadoViewModel(Enumerados.Parametro IdEmpleado)
        {
            EmpleadoID = IdEmpleado;
        }
        public Empleado Empleado { get; set; }
        public List<Empleado> Empleados { get; set; }
        public ICollection<Permisos> Permisos { get; set; }
    }
}

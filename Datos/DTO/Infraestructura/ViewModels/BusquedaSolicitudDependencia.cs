using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using Sistema.Paginador;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class BusquedaSolicitudDependencia : PaginacionViewModel
    {
        public BusquedaSolicitudDependencia()
        {
            Solicitudes = new List<Solicitud>();

            using (var db = new Contexto())
            {
                Entidades = (from e in db.Entidad
                    where e.Habilitado
                    select new
                    {
                        e.IDEntidad,
                        e.Nombre
                    }).ToDictionary(e => e.IDEntidad, e => e.Nombre);
            }
        }

        public void BuscarSolicitudes()
        {
            if (IDEntidad == 0) return;

            using (var db = new Contexto())
            {
                var query = (from s in db.Solicitud
                    select s);

                if (IDEntidad > 0)
                {
                    query = (from s in db.Solicitud
                        where s.Entidad.IDEntidad == IDEntidad
                        select s);
                }

                if (!string.IsNullOrWhiteSpace(Oficio))
                {
                    query = (from s in db.Solicitud
                             where s.NumeroDeOficio == Oficio
                             select s);
                }

                TotalEncontrados = query.Count();

                query = query.OrderByDescending(m => m.FechaSolicitud);
                query = query.Skip((PaginaActual - 1) * 2).Take(2);

                Paginas = Paginar.ObtenerCantidadPaginas(TotalEncontrados, 2);
                Solicitudes = (from s in query
                    .Include(i => i.Persona)
                    .Include(i => i.Carta)
                    select s).ToList();
            }
        }

        public string Oficio { get; set; }
        [Display(Name = "Fecha Inicio")]
        public DateTime? FechaInicio { get; set; }
        [Display(Name = "Fecha Fin")]
        public DateTime? FechaFin { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Dependencia")]
        public int IDEntidad { get; set; }
        public List<Solicitud> Solicitudes { get; set; }
        public Dictionary<int, string> Entidades { get; set; } 
        public string UrlDescargarArchivo { get; set; }
        public string Paginacion { get; set; }
    }
}

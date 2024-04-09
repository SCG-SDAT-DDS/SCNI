using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using Datos.DTO.Infraestructura.ViewModels;
using System.Web.Mvc;
using Datos.Recursos;

namespace Datos.Repositorios.Catalogos
{
    public class TituloRepositorio : Repositorio<Titulo>
    {
        public TituloRepositorio(Contexto contexto) : base(contexto)
        {
        }

        public void CambiarHabilitado(int idTitulo, bool habilitado)
        {
            var tituloCambiarHabilitado = new Titulo { IDTitulo = idTitulo };

            Contexto.Titulos.Attach(tituloCambiarHabilitado);
            tituloCambiarHabilitado.Habilitado = habilitado;
            Contexto.Entry(tituloCambiarHabilitado).Property(r => r.Habilitado).IsModified = true;

            Contexto.SaveChanges();
        }

        public List<Titulo> Buscar(TituloViewModel viewModel)
        {
            return _ObtenerQuery(viewModel, true).ToList();
        }

        public int ObtenerTotalRegistros(TituloViewModel viewModel)
        {
            return _ObtenerQuery(viewModel, false).Count();
        }

        public IQueryable<Titulo> _ObtenerQuery(TituloViewModel criterios, bool paginar)
        {
            IQueryable<Titulo> query = Contexto.Set<Titulo>();

            query = query.Where(c => c.Habilitado == criterios.Titulo.Habilitado);

            if (!string.IsNullOrEmpty(criterios.Titulo.Abreviacion))
            {
                query = query.Where(t => t.Abreviacion.Contains(criterios.Titulo.Abreviacion));
            }

            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(t => t.Abreviacion);
                query = query.Skip((criterios.PaginaActual - 1)*criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            return query;
        }

        public static List<SelectListItem> Buscar()
        {
            List<SelectListItem> lista;
            using (var bd = new Contexto())
            {
                lista = (from t in bd.Titulos
                         where t.Habilitado
                         orderby t.Abreviacion
                         select new SelectListItem
                         {
                             Value = SqlFunctions.StringConvert((decimal?)t.IDTitulo).Trim(),
                             Text = t.Abreviacion
                         }).ToList();

                lista.Insert(0, new SelectListItem
                {
                    Text = General.Seleccione,
                    Value = string.Empty
                });
            }
            return lista;
        }
    }
}

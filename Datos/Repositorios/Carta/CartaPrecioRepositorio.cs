using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Datos.DTO.Infraestructura.ViewModels;

namespace Datos.Repositorios.Carta
{
    public class CartaPrecioRepositorio : Repositorio<Precio>
    {
        public CartaPrecioRepositorio(Contexto contexto) : base(contexto)
        {

        }

        public List<Precio> Buscar(PrecioViewModel criteriosPrecio)
        {
            var query = _ObtenerQuery(criteriosPrecio, true);

            return query.ToList();
        }

        public int ObtenerTotalRegistros(PrecioViewModel criteriosPrecio)
        {
            var query = _ObtenerQuery(criteriosPrecio, false);

            return query.Count();
        }

        public IQueryable<Precio> _ObtenerQuery(PrecioViewModel criteriosPrecio, bool paginar)
        {
            IQueryable<Precio> query = Contexto.Set<Precio>();


            if (criteriosPrecio.Precio?.Valor > 0)
            {
                query = from c in query
                         where c.Valor == criteriosPrecio.Precio.Valor
                         select c;
            }

            if (paginar && criteriosPrecio.TamanoPagina > 0 && criteriosPrecio.PaginaActual > 0)
            {
                query = query.OrderByDescending(c => c.FechaCreacion);
                query = query.Skip((criteriosPrecio.PaginaActual - 1) * criteriosPrecio.TamanoPagina)
                    .Take(criteriosPrecio.TamanoPagina);

                query = query.Include(c => c.Carta);
            }

            return query;
        }

        public bool GuardarNuevoPrecio(Precio precio)
        {
            CambhiarEsPrecioActual(false);

            precio.FechaCreacion = DateTime.Now;
            precio.EsPrecioActual = true;

            Contexto.Entry(precio).State = EntityState.Added;

            return Contexto.SaveChanges() == 1;
        }

        public void CambhiarEsPrecioActual(bool esActual, int idPrecio = 0)
        {
            // Cambiar EsActual solo a un registro
            if (idPrecio != 0)
            {
                var precio = Contexto.Precio.Find(idPrecio);
                precio.EsPrecioActual = esActual;
                Contexto.Entry(precio).State = EntityState.Modified;
            }
            // Cambiar EsActual a todos los registros en que EsActual == True
            else
            {
                var precios = (from p in Contexto.Precio where p.EsPrecioActual select p).ToList();

                foreach (var precio in precios)
                {
                    precio.EsPrecioActual = esActual;
                    Contexto.Entry(precio).State = EntityState.Modified;
                }
            }

            // Guardar cambios
            Contexto.SaveChanges();
        }

        public void GuardarCartaPrecioActual(Datos.Carta carta)
        {
            var precioActual = (from p in Contexto.Precio
                                where p.EsPrecioActual
                                select p).Single();

            precioActual.Carta.Add(carta);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;
using Datos.Recursos;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios;
using Datos.Repositorios.Catalogos;

namespace Datos.Repositorios.Catalogos
{
    public class MovimientoRepositorio : Repositorio<Movmiento>
    {
        public MovimientoRepositorio(Contexto contexto) : base(contexto)
        {

        }

        /// <summary>
        /// Busca movimientos por criterios de busqueda
        /// </summary>
        /// <param name="movimientoViewModel"></param>
        /// <returns>Lista de movimientos encontrados</returns>
        public List<Movmiento> Buscar(MovimientoViewModel movimientoViewModel)
        {
            return ObtenerQuery(movimientoViewModel, true).ToList();
        }
        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="movimientoViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(MovimientoViewModel movimientoViewModel)
        {
            return ObtenerQuery(movimientoViewModel, false).Count();
        }

        public IQueryable<Movmiento> ObtenerQuery(MovimientoViewModel criterios, bool paginar)
        {
            IQueryable<Movmiento> query = Contexto.Set<Movmiento>();

            if (!string.IsNullOrEmpty(criterios.Movimiento.Nombre))
            {
                query = query.Where(c => c.Nombre == criterios.Movimiento.Nombre);
            }
            if (!string.IsNullOrEmpty(criterios.Movimiento.Usuario?.NombreUsuario))
            {
                query = query.Where(c => c.Usuario.NombreUsuario == criterios.Movimiento.Usuario.NombreUsuario);
            }
            if (!string.IsNullOrEmpty(criterios.Movimiento.Catalogo))
            {
                query = query.Where(c => c.Catalogo.Contains(criterios.Movimiento.Catalogo));
            }
            if (criterios.Movimiento.Fecha != new DateTime())
            {
                query = query.Where(c => c.Fecha == criterios.Movimiento.Fecha);
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderByDescending(q => q.Fecha);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            query = query.Include(c => c.Usuario);
            //query = query.Include(c => c.Abreviatura);

            return query;
        }

        public static void GuardarMovimiento(string _nombre, string _catalogo, int _id, string _nombreUsuario)
        {
            // Definición de variables
            var _usuario = new Usuario();

            // Crear contexto y abrir conexión
            using (var bd = new Contexto())
            {
                // Configuraciones previas de la conexión
                bd.Configuration.ValidateOnSaveEnabled = false;

                _usuario = new UsuarioRepositorio(bd).BuscarUsuarioPublicoPorNombre(_nombreUsuario);

                // Configuraciones previas del modelo
                MovimientoRepositorio _movimientoRepositorio = new MovimientoRepositorio(bd);
                var movimiento = new Movmiento
                {
                    Nombre = _nombre,
                    Usuario = _usuario,
                    Habilitado = true,
                    Fecha = DateTime.Now,
                    Catalogo = _catalogo,
                    IDRegistro = _id
                };
                bd.Entry(movimiento.Usuario).State = EntityState.Unchanged;

                // Crear Movimiento nuevo
                _movimientoRepositorio.Guardar(movimiento);
                // Guardar cambios realizados al contexto
                bd.SaveChanges();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
namespace Datos.Repositorios
{
    public abstract class Repositorio<TEntity> where TEntity : class
    {
        protected readonly Contexto Contexto;

        protected Repositorio(Contexto contexto)
        {
            Contexto = contexto;
        }
        
        /// <summary>
        /// Metodo generico para guardar una entidad
        /// </summary>
        /// <param name="entidad"></param>
        public virtual void Guardar(TEntity entidad)
        {
            Contexto.Set<TEntity>().Add(entidad);
        }
        
        /// <summary>
        /// Metodo generico para modificar una entidad
        /// </summary>
        /// <param name="entidad">La entidad a modificar</param>
        public virtual void Modificar(TEntity entidad)
        {
            Contexto.Set<TEntity>().Attach(entidad);
            Contexto.Entry(entidad).State = EntityState.Modified;
        }
        
        /// <summary>
        /// Metodo generico para recuperar una entidad a partir de su identidad
        /// </summary>
        /// <param name="idEntidad">La identidad de la entidad</param>
        /// <returns>La entidad</returns>
        public virtual TEntity ObtenerPorId(object idEntidad)
        {
            return Contexto.Set<TEntity>().Find(idEntidad);
        }
        
        /// <summary>
        /// Metodo generico para eliminar una entidad pasandole la entidad
        /// </summary>
        /// <param name="entidad">Entidad a eliminar</param>
        public void Eliminar(TEntity entidad)
        {
            Contexto.Set<TEntity>().Attach(entidad);
            Contexto.Set<TEntity>().Remove(entidad);
        }
        
        /// <summary>
        /// Metodo generico para eliminar una entidad del contexto
        /// </summary>
        /// <param name="idEntidad">La identidad de la entidad</param>
        public void Eliminar(object idEntidad)
        {
            var entityToDelete = Contexto.Set<TEntity>().Find(idEntidad);
            Eliminar(entityToDelete);
        }
        
        /// <summary>
        /// Metodo generico para recuperar una coleccion de entidades
        /// </summary>
        /// <param name="filtro">Expresion para filtrar las entidades</param>
        /// <param name="ordenarPor">Orden en el que se quiere recuperar las entidades</param>
        /// <param name="incluirPropiedades">Propiedades de Navegacion a incluir</param>
        /// <returns>Un listado de objetos de la entidadgenerica</returns>
        public List<TEntity> Buscar(Expression<Func<TEntity, bool>> filtro = null,
                                           string[] incluirPropiedades = null,
                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordenarPor = null)
        {
            IQueryable<TEntity> query = Contexto.Set<TEntity>();

            if (filtro != null)
                query = query.Where(filtro);

            if (incluirPropiedades != null)
            {
                foreach (var includeProperty in incluirPropiedades)
                {
                    query = query.Include(includeProperty);
                }
            }

            return ordenarPor != null ? ordenarPor(query).ToList() : query.ToList();
        }
        
        /// <summary>
        /// Busqueda de entidades.
        /// </summary>
        /// <param name="filtro">Expresion para filtrar las entidades.</param>
        /// <param name="ordenarPor">Expresion para ordenar las entidades.</param>
        /// <param name="incluirPropiedades">Expresione para incluir propiedades de navegacion.</param>
        /// <returns>Lista de entidades.</returns>
        public List<TEntity> Buscar(Expression<Func<TEntity, bool>> filtro,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordenarPor,
            params Expression<Func<TEntity, object>>[] incluirPropiedades)
        {
            IQueryable<TEntity> query = Contexto.Set<TEntity>();

            if (filtro != null)
                query = query.Where(filtro);

            if (incluirPropiedades != null)
            {
                foreach (var incluir in incluirPropiedades)
                {
                    query = query.Include(incluir);
                }
            }

            return ordenarPor != null ? ordenarPor(query).ToList() : query.ToList();
        }
        
        /// <summary>
        /// Metodo generico para recuperar una entidad
        /// </summary>
        /// <param name="filtro">Expresion para filtrar las entidades</param>
        /// <param name="incluirPropiedades">Propiedades de Navegacion a incluir</param>
        /// <returns>Entidad encontrada</returns>
        public TEntity BuscarUnoSolo(Expression<Func<TEntity, bool>> filtro, string[] incluirPropiedades = null)
        {
            IQueryable<TEntity> query = Contexto.Set<TEntity>();

            if (filtro != null)
                query = query.Where(filtro);

            if (incluirPropiedades != null)
            {
                foreach (var includeProperty in incluirPropiedades)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.SingleOrDefault();
        }
        
        /// <summary>
        /// Metodo generico para recuperar una entidad
        /// </summary>
        /// <param name="filtro">Expresion para filtrar las entidades</param>
        /// <param name="ordenarPor"></param>
        /// <param name="incluirPropiedades">Propiedades de Navegacion a incluir</param>
        /// <returns>Entidad encontrada</returns>
        public TEntity BuscarUnoSolo(Expression<Func<TEntity, bool>> filtro,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordenarPor,
            params Expression<Func<TEntity, object>>[] incluirPropiedades)
        {
            IQueryable<TEntity> query = Contexto.Set<TEntity>();

            if (filtro != null)
                query = query.Where(filtro);

            if (incluirPropiedades != null)
            {
                foreach (var incluir in incluirPropiedades)
                {
                    query = query.Include(incluir);
                }
            }

            return ordenarPor != null ? ordenarPor(query).SingleOrDefault() : query.SingleOrDefault();
        }
    }
}

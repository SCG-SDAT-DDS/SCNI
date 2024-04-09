using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;
using Datos.DTO;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Recursos;

namespace Datos.Repositorios.Solicitudes
{
    public class PersonaRepositorio : Repositorio<Persona>
    {
        public PersonaRepositorio(Contexto contexto) 
            : base(contexto) { }

        /// <summary>
        /// Busca personas por criterios de busqueda
        /// </summary>
        /// <param name="personaViewModel"></param>
        /// <returns>Lista de personas encontradas</returns>
        public List<Persona> Buscar(PersonaViewModel personaViewModel)
        {
            return ObtenerQuery(personaViewModel, true).ToList();
        }

        /// <summary>
        /// Obtiene el total de registros sin paginar
        /// </summary>
        /// <param name="entidadViewModel"></param>
        /// <returns></returns>
        public int ObtenerTotalRegistros(PersonaViewModel personaViewModel)
        {
            return ObtenerQuery(personaViewModel, false).Count();
        }

        public IQueryable<Persona> ObtenerQuery(PersonaViewModel criterios, bool paginar)
        {
            IQueryable<Persona> query = Contexto.Set<Persona>();

            if (!string.IsNullOrEmpty(criterios.Persona.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(criterios.Persona.Nombre));
            }
            if (!string.IsNullOrEmpty(criterios.Persona.RFC))
            {
                query = query.Where(c => c.RFC.Contains(criterios.Persona.RFC));
            }
            if (paginar && criterios.TamanoPagina > 0 && criterios.PaginaActual > 0)
            {
                query = query.OrderBy(q => q.Nombre);
                query = query.Skip((criterios.PaginaActual - 1) * criterios.TamanoPagina).Take(criterios.TamanoPagina);
            }

            return query;
        }

        public dynamic BuscarPorRfc(string rfc)
        {
            var resultado = (from m in Contexto.Persona
                where m.RFC.StartsWith(rfc)
                select new
                {
                    m.IDPersona,
                    m.Nombre,
                    APaterno = m.ApellidoP,
                    AMaterno = m.ApellidoM,
                    FechaNacimiento = (string.IsNullOrEmpty(m.FechaNacimiento.ToString()) ? "" : m.FechaNacimiento.ToString()),
                    CURP = (string.IsNullOrEmpty(m.CURP) ? "" : m.CURP),
                    RFC = (string.IsNullOrEmpty(m.RFC) ? "" : m.RFC),
                    m.Genero,
                    Correo = (string.IsNullOrEmpty(m.CorreoElectronico) ? "" : m.CorreoElectronico),
                    m.IDEstado,
                    m.IDMunicipio
                }).ToList();

            return resultado;
        }

        public object BuscarPorID(int ID)
        {

            var resultado = (
                from m in Contexto.Persona
                where m.IDPersona == ID
                select new
                {
                    IDPersona = m.IDPersona,
                    Nombre = m.Nombre,
                    APaterno = m.ApellidoP,
                    AMaterno = m.ApellidoM,
                    FechaNacimiento = (string.IsNullOrEmpty(m.FechaNacimiento.ToString()) ? "" : m.FechaNacimiento.ToString()),
                    CURP = (string.IsNullOrEmpty(m.CURP) ? "" : m.CURP),
                    RFC = (string.IsNullOrEmpty(m.RFC) ? "" : m.RFC),
                    m.Genero,
                    Correo = (string.IsNullOrEmpty(m.CorreoElectronico) ? "" : m.CorreoElectronico),
                    IDEstado = m.IDEstado,
                    IDMunicipio = m.IDMunicipio
                }).FirstOrDefault();
            if (resultado == null)
            {
                var IDPersona = 0;
                return IDPersona;
            }
            return resultado;
        }

        public Persona BuscarPersonaPorID(int ID) {
            return (from m in Contexto.Persona
            where m.IDPersona == ID
            select m).FirstOrDefault();
        }

        public GenerosPersona ObtenerGenero(int idPersona)
        {
            var genero = (from p in Contexto.Persona
                where p.IDPersona == idPersona
                select p.Genero).Single();

            return (GenerosPersona) int.Parse(genero);
        }
    }
}

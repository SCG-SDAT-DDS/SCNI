using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios
{
    public class SolicitudContrasenaRepositorio : Repositorio<SolicitudContrasena>
    {
        public SolicitudContrasenaRepositorio(Contexto contexto) 
            : base(contexto)
        {
        }

        public bool Guardar(int idUsuario, string codigo)
        {
            var solicitudContrasena = new SolicitudContrasena
            {
                IDUsuario = idUsuario,
                Codigo = codigo,
                Usado = false,
                FechaRegistro = DateTime.Now,
                TipoUsuario = 1
            };

            Guardar(solicitudContrasena);
            return Contexto.SaveChanges() == 1;
        }

        public Usuario ObtenerUsuario(string codigo)
        {
            return (from sc in Contexto.SolicitudContrasena
                    where sc.Codigo == codigo && !sc.Usado
                    select sc.Usuario).SingleOrDefault();
        }

        public int ObtenerIdUsuario(string codigo)
        {
            return (from sc in Contexto.SolicitudContrasena
                    where sc.Codigo == codigo && !sc.Usado
                    select sc.Usuario.IDUsuario).SingleOrDefault();
        }

        public void DesactivarCodigo(string codigo)
        {
            var solicitudContrasena =
                (from sc in Contexto.SolicitudContrasena
                 where sc.Codigo == codigo
                 select sc).SingleOrDefault();

            if (solicitudContrasena != null)
            {
                solicitudContrasena.Usado = true;
                Contexto.Entry(solicitudContrasena).Property(m => m.Usado).IsModified = true;
            }
            Contexto.SaveChanges();
        }
    }
}

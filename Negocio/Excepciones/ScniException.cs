
using System;

namespace Negocio.Excepciones
{
    public class ScniException : ApplicationException
    {
        public ScniException(string mensaje) : base(mensaje)
        {
        }
    }
}

using System;

namespace Negocio.Firma
{
    public class FielException : ApplicationException
    {
        public FielException()
        {

        }

        public FielException(string mensaje) : base(mensaje)
        {
        }
    }
}

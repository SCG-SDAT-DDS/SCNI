using System;

namespace Sistema.Excepciones
{
    public class DatosException : ApplicationException
    {
        public DatosException(string mensaje) 
            : base(mensaje)
        {

        }
    }
}

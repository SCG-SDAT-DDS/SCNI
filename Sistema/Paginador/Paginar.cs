using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistema.Paginador
{
    public class Paginar
    {
        public static int ObtenerCantidadPaginas(int totalRegistros, int tamanoPagina)
        {
            return (int)Math.Ceiling((double)((totalRegistros + tamanoPagina - 1) / tamanoPagina));
        }

        public static List<T> ObtenerRegistrosPagina<T>(int numeroPagina, int tamanoPagina, List<T> lista)
        {
            return lista.Skip((numeroPagina - 1) * tamanoPagina).Take(tamanoPagina).ToList();
        }
    }
}

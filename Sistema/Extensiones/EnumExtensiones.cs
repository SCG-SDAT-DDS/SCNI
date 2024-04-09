using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sistema.Extensiones
{
    public static class EnumExtenciones
    {
        /// <summary>
        /// Obtiene la descripcion de un enumerado
        /// </summary>
        /// <param name="enumValor">Enumerado</param>
        /// <returns>Descripcion del enumerado</returns>
        public static string Descripcion(this Enum enumValor)
        {
            if (enumValor.GetType().GetField(enumValor.ToString()) == null)
            {
                return string.Empty;
            }

            var atributos = enumValor.GetType().GetField(enumValor.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (atributos.Length > 0)
            {
                return ((DescriptionAttribute)atributos.Single()).Description;
            }

            var resultado = enumValor.ToString();

            //"FooBar" -> "Foo Bar"
            resultado = Regex.Replace(resultado, @"([a-z])([A-Z])", "$1 $2");

            //"Foo123" -> "Foo 123"
            resultado = Regex.Replace(resultado, @"([A-Za-z])([0-9])", "$1 $2");

            //"123Foo" -> "123 Foo"
            resultado = Regex.Replace(resultado, @"([0-9])([A-Za-z])", "$1 $2");

            //"FOOBar" -> "FOO Bar"
            resultado = Regex.Replace(resultado, @"(?<!^)(?<! )([A-Z][a-z])", " $1");

            return resultado;
        }

        /// <summary>
        /// Determina si el enumerado es una combinacion exacta del parametro.
        /// </summary>
        /// <typeparam name="T">Tipo de enumerado</typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Es<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T AgregarBandera<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("No se pudo agregar el valor al enumerado {0}.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Quita la bandera de un enumerado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T QuitarBandera<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("No se pudo quitar el valor del enumerado {0}.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Determina si el enumerado tiene la bandera que se envia como parametro.
        /// </summary>
        /// <typeparam name="T">Tipo Enumerado</typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Tiene<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }
    }
}

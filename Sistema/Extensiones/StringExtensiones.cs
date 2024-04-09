using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Sistema.Seguridad;

namespace Sistema.Extensiones
{
    public static class StringExtensiones
    {
        /// <summary>
        /// Convierte la representacion de un numero en string a un valor short.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en short del numero</returns>
        public static short ToShort(this string str)
        {
            return short.Parse(str);
        }

        /// <summary>
        /// Convierte la representacion de un numero en string a un valor byte.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en byte del numero</returns>
        public static byte ToByte(this string str)
        {
            return byte.Parse(str);
        }

        /// <summary>
        /// Convierte la representacion de un numero en string a un valor int.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en int del numero</returns>
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        /// <summary>
        /// Intenta convertir la representacion de un numero en string a un valor byte.
        /// Si no es posible regresara 0.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en byte del numero</returns>
        public static byte TryToByte(this string str)
        {
            byte byt;
            byte.TryParse(str, out byt);

            return byt;
        }

        /// <summary>
        /// Intenta convertir la representacion de un numero en string a un valor short.
        /// Si no es posible regresara 0.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en short del numero</returns>
        public static short TryToShort(this string str)
        {
            short sho;
            short.TryParse(str, out sho);

            return sho;
        }

        /// <summary>
        /// Intenta convertir la representacion de un numero en string a un valor int.
        /// Si no es posible regresara 0.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en int del numero</returns>
        public static int TryToInt(this string str)
        {
            int i;
            int.TryParse(str, out i);

            return i;
        }

        /// <summary>
        /// Intenta convertir la representacion de un numero en string a un valor int.
        /// Si no es posible regresara 0.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en int del numero</returns>
        public static bool TryToBool(this string str)
        {
            bool i;
            bool.TryParse(str, out i);

            if (str == "on") i = true;

            return i;
        }

        /// <summary>
        /// Intenta convertir la representacion de una fecha en string a un valor Date Time.
        /// Si no es posible regresara '01-01-0001'.
        /// </summary>
        /// <param name="str">Representacion de la fecha en string</param>
        /// <returns>Valor en DateTime</returns>
        public static DateTime TryToDateTime(this string str)
        {
            DateTime dt;
            DateTime.TryParse(str, out dt);

            return dt;
        }

        /// <summary>
        /// Intenta convertir la representacion de un numero en string a un valor decimal.
        /// Si no es posible regresará 0.
        /// Cambiará los puntos (.) por comas (,) para regresar el decimal.
        /// </summary>
        /// <param name="str">Representacion del numero en string</param>
        /// <returns>Valor en decimal del numero</returns>
        public static decimal TryToDecimal(this string str)
        {
            decimal dml;
            decimal.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out dml);

            return dml;
        }

        /// <summary>
        /// Intenta convertir una lista de representaciones de numeros en string a sus valores en short.
        /// Si no es posible no lo regresara en la lista.
        /// </summary>
        /// <param name="strLista">Representacion del numero en string</param>
        /// <returns>Lista con los valores en short de los numeros</returns>
        public static List<short> TryToShort(this List<string> strLista)
        {
            return strLista.Select(TryToShort).Where(sho => sho > 0).ToList();
        }

        /// <summary>
        /// Intenta convertir una lista de representaciones de numeros en string a sus valores en int.
        /// Si no es posible no lo regresara en la lista.
        /// </summary>
        /// <param name="strLista">Representacion del numero en string</param>
        /// <returns>Lista con los valores en int de los numeros</returns>
        public static List<int> TryToInt(this List<string> strLista)
        {
            return strLista.Select(TryToInt).Where(i => i > 0).ToList();
        }

        /// <summary>
        /// Obtiene el valor hasg MD5 de una cadena.
        /// </summary>
        /// <param name="cadena">Cadena a calcular su valor hash MD5.</param>
        /// <returns>Valor del hash como una cadena hexadecimal de 32 caracteres.</returns>
        public static string GetMd5Hash(this string cadena)
        {
            var md5Hash = MD5.Create();

            var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(cadena));

            return HashBytesToString(hash);
        }

        /// <summary>
        /// Obtiene el hash de una cadena usando el algoritmo SHA512.
        /// </summary>
        /// <param name="cadena">Cadena a obtener su hash.</param>
        /// <returns>Hash de la cadena usando el algoritmo SHA512</returns>
        public static string GetHashSha512(this string cadena)
        {
            var alg = SHA512.Create();

            var hash = alg.ComputeHash(Encoding.UTF8.GetBytes(cadena));

            var final = HashBytesToString(hash);

            return final;
        }

        private static string HashBytesToString(byte[] hash)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString(@"x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Codifica la cadena en Base64.
        /// </summary>
        /// <param name="str">Cadena con los caracteres a codigicar.</param>
        /// <returns>Cadena codificada en Base64.</returns>
        public static string EncodeTo64(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            try
            {
                return CifradoAes.Cifrar_Metodo2(str, "clave");
                //var arrayStr = Encoding.ASCII.GetBytes(str);

                //return Convert.ToBase64String(arrayStr);
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Decodifica una cadena en Base64.
        /// </summary>
        /// <param name="str64">Cadena con los caracteres a decodificar.</param>
        /// <returns>Cadena decodificada.</returns>
        public static string DecodeFrom64(this string str64)
        {
            if (string.IsNullOrEmpty(str64)) return string.Empty;

            try
            {
                return CifradoAes.Descifrar_Metodo2(str64, "clave");
                //var arrayStr = Convert.FromBase64String(str64);

                //return Encoding.ASCII.GetString(arrayStr);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Anada un espacio antes de una mayuscula a la cadena de caracteres.
        /// </summary>
        /// <param name="txt">Cadena a la que se le realizara.</param>
        /// <returns>Cadena con un espacio antes de cada mayuscula.</returns>
        public static string AddSpaceBeforeUpper(this string txt)
        {
            if (string.IsNullOrWhiteSpace(txt)) return string.Empty;

            var newText = new StringBuilder(txt.Length * 2);

            newText.Append(txt[0]);

            for (var i = 1; i < txt.Length; i++)
            {
                if (char.IsUpper(txt[i]) && txt[i - 1] != ' ')
                {
                    newText.Append(' ');
                }

                newText.Append(txt[i]);
            }

            return newText.ToString();
        }

        public static string ToTitleCase(this string txt)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            txt = textInfo.ToTitleCase(txt.ToLower());

            var partes = txt.Split(' ');

            for (var i = 0; i < partes.Length; i++)
            {
                if (partes[i].Length < 3)
                {
                    partes[i] = partes[i].ToLower();
                }
            }

            txt = string.Join(" ", partes);
            return txt;
        }

        /// <summary>
        /// Convierte la primera letra de la cadena en mayuscula
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string UpperFirstLetter(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;
            return txt.First().ToString().ToUpper() + txt.Substring(1);
        }

        /// <summary>
        /// Quitar todos acento o tilde en una cadena.
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns> cadena sin acentos ni eñes</returns>
        public static string SinAcentos(this string cadena)
        {
            var a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            var e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            var i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            var o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            var u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            var n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
            cadena = a.Replace(cadena, "a");
            cadena = e.Replace(cadena, "e");
            cadena = i.Replace(cadena, "i");
            cadena = o.Replace(cadena, "o");
            cadena = u.Replace(cadena, "u");
            cadena = n.Replace(cadena, "n");
            return cadena;
        }
    }
}

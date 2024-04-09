using System;
using System.Security.Cryptography;
using System.Text;

namespace Sistema.Seguridad
{
    public static class Encriptacion
    {
        /// <summary>
        /// Obtiene el valor hasg MD5 de una cadena.
        /// </summary>
        /// <param name="cadena">Cadena a calcular su valor hash MD5.</param>
        /// <returns>Valor del hash como una cadena hexadecimal de 32 caracteres.</returns>
        public static string GetMd5Hash(string cadena)
        {
            var md5Hash = MD5.Create();

            var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(cadena));

            return HashBytesToString(hash);
        }

        /// <summary>
        /// Verifica el hash MD5 contra el hash de una cadena.
        /// </summary>
        /// <param name="entrada">Cadena a verificar.</param>
        /// <param name="hash">Hash MD5 contra el que se comparara.</param>
        /// <returns>Indica si el hash MD5 coincide contra el hash de la cadena.</returns>
        public static bool VerificarMd5Hash(string entrada, string hash)
        {
            var hashEntrada = GetMd5Hash(entrada);

            var compararador = StringComparer.OrdinalIgnoreCase;

            return compararador.Compare(hashEntrada, hash) == 0;
        }

        /// <summary>
        /// Obtiene el hash de una cadena usando el algoritmo SHA512.
        /// </summary>
        /// <param name="cadena">Cadena a obtener su hash.</param>
        /// <returns>Hash de la cadena usando el algoritmo SHA512</returns>
        public static string GetHashSha512(string cadena)
        {
            var alg = SHA512.Create();

            var hash = alg.ComputeHash(Encoding.UTF8.GetBytes(cadena));

            return HashBytesToString(hash);
        }

        public static bool CompararSha512Hash(string cadena, string hash)
        {
            var hashEntrada = GetHashSha512(cadena);

            var compararador = StringComparer.OrdinalIgnoreCase;

            return compararador.Compare(hashEntrada, hash) == 0;
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
    }
}

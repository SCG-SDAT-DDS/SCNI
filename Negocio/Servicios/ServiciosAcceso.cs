using System.Collections.Generic;
using System.Linq;
using Datos;
using Datos.Recursos;
using Datos.Repositorios;
using Negocio.Excepciones;
using Negocio.Firma;
using Sistema.Seguridad;

namespace Negocio.Servicios
{
    public class ServiciosAcceso
    {
        private readonly TiposInicio _tipo;

        public ServiciosAcceso(TiposInicio tipoInicio)
        {
            _tipo = tipoInicio;
        }

        public KeyValuePair<string, Usuario> ValidarAcceso(string nombreUsuario, string contrasena, string pkcs7)
        {
            if(string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ScniException("El nombre de usuario es requerido.");

            if (_tipo == TiposInicio.Contraseña && string.IsNullOrWhiteSpace(contrasena))
                throw new ScniException("La contraseña es requerida.");

            if (_tipo == TiposInicio.Fiel && string.IsNullOrWhiteSpace(pkcs7))
                throw new ScniException("La firma para ingresar es requerida.");

            Usuario usuario;
            string serieCertificado = null;

            using (var contexto = new Contexto())
            {
                var repositorio = new UsuarioRepositorio(contexto);

                usuario = repositorio.BuscarUsuarioPorNombreUsuario(nombreUsuario);
            }

            if(usuario == null)
                throw new ScniException("Usuario incorrecto.");

            if (_tipo != usuario.Rol.TipoInicio)
                throw new ScniException("El tipo de inicio de sesión no coincide");

            if (_tipo == TiposInicio.Contraseña)
            {
                var contraseñaHash = Encriptacion.GetHashSha512(contrasena);

                if (contraseñaHash != usuario.Contrasena)
                    throw new ScniException(General.ErrorUsuarioIncorrecto);
            }
            else
            {
                if (usuario.Certificado == null)
                    throw new ScniException("Sin certificado digital asignado.");

                try
                {
                    var bytesPkcs7 = OperacionesFiel.ConvertirPkcs7Bytes(pkcs7);

                    ServiciosWebFiel.NoOperacionVerificarPkcs7(bytesPkcs7);

                    var decodificadoPkcs7 = ServiciosWebFiel.NoOperacionDecodificarPkcs7(bytesPkcs7);
                    
                    var certificado = OperacionesFiel.ConvertirCertificadoBytes(decodificadoPkcs7[1]);

                    serieCertificado = decodificadoPkcs7[2];

                    if (!certificado.SequenceEqual(usuario.Certificado.Valor))
                        throw new FielException("La firma no corresponde al usuario.");

                    ServiciosWebFiel.NoOperacionAutenticarCertificado(certificado);
                }
                catch (FielException ex)
                {
                    throw new ScniException(ex.Message);
                }
            }

            return new KeyValuePair<string, Usuario>(serieCertificado, usuario);
        }

        public KeyValuePair<string, Usuario> ValidarAccesoConPFX(string nombreUsuario, string contrasena, string pfx)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ScniException("El nombre de usuario es requerido.");

            if (_tipo == TiposInicio.Contraseña && string.IsNullOrWhiteSpace(contrasena))
                throw new ScniException("La contraseña es requerida.");

            if (_tipo == TiposInicio.Fiel && string.IsNullOrWhiteSpace(pfx))
                throw new ScniException("el archivo pfx e para ingresar es requerida.");

            Usuario usuario;
            string serieCertificado = null;

            using (var contexto = new Contexto())
            {
                var repositorio = new UsuarioRepositorio(contexto);

                usuario = repositorio.BuscarUsuarioPorNombreUsuario(nombreUsuario);
            }

            if (usuario == null)
                throw new ScniException("Usuario incorrecto.");

            //TODO: REMOVE THIS AFTER FINISH
            if (_tipo != usuario.Rol.TipoInicio)
                //throw new ScniException("El tipo de inicio de sesión no coincide");

            if (_tipo == TiposInicio.Contraseña)
            {
                var contraseñaHash = Encriptacion.GetHashSha512(contrasena);

                if (contraseñaHash != usuario.Contrasena)
                    throw new ScniException(General.ErrorUsuarioIncorrecto);
            }
            else
            {
            //    if (usuario.Certificado == null)
            //        throw new ScniException("Sin certificado digital asignado.");

                try
                {
                    //var bytesPkcs7 = OperacionesFiel.ConvertirPkcs7Bytes(pkcs7);

                    //ServiciosWebFiel.NoOperacionVerificarPkcs7(bytesPkcs7);

                    //var decodificadoPkcs7 = ServiciosWebFiel.NoOperacionDecodificarPkcs7(bytesPkcs7);

                    //var certificado = OperacionesFiel.ConvertirCertificadoBytes(decodificadoPkcs7[1]);

                    //serieCertificado = decodificadoPkcs7[2];

                    //if (!certificado.SequenceEqual(usuario.Certificado.Valor))
                    //    throw new FielException("La firma no corresponde al usuario.");

                    //ServiciosWebFiel.NoOperacionAutenticarCertificado(certificado);
                }
                catch (FielException ex)
                {
                    throw new ScniException(ex.Message);
                }
            }

            return new KeyValuePair<string, Usuario>(serieCertificado, usuario);
        }
    }
}

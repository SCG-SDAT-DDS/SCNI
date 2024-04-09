using System;
using System.Configuration;
using System.Globalization;
using Datos;
using Datos.Repositorios.Firma;
using iTextSharp.text.pdf.codec;
using Newtonsoft.Json.Linq;

namespace Negocio.Firma
{
    public class ServiciosWebFiel
    {
        private readonly byte[] _certificado;
        private readonly string _serieCertificado;
        private readonly string _usuario;
        private readonly string _digestion;
        private readonly byte[] _pkcs7;

        internal ServiciosWebFiel(string usuario, string serieCertificado, byte[] certificado, string digestion, byte[] pkcs7)
        {
            _usuario = usuario;
            _serieCertificado = serieCertificado;
            _certificado = certificado;

            _digestion = digestion;
            _pkcs7 = pkcs7;
        }

        private cadena _RegistrarCadena(DateTime fecha, long idOperacion)
        {
            var cadena = new cadena
            {
                digestion = _digestion,
                pkcs7 = _pkcs7,
                fecha = fecha.Date,
                hora = fecha.TimeOfDay,
                idoperacion = idOperacion
            };
            RepositorioBodevaFiel.GuardarCadena(cadena);

            return cadena;
        }

        #region Sesion
        private static string _token;
        private static long _idToken;
        private static readonly object PadLockToken = new object();

        private static string Token
        {
            get
            {
                if (_token == null)
                {
                    lock (PadLockToken)
                    {
                        if (_token == null)
                        {
                            _IniciarSesionFiel();
                        }
                    }
                }

                return _token;
            }
            set
            {
                lock (PadLockToken)
                {
                    _token = value;
                }
            }
        }

        private static long IdToken
        {
            get
            {
                lock (PadLockToken)
                {
                    return _idToken;
                }
            }
            set
            {
                lock (PadLockToken)
                {
                    _idToken = value;
                }
            }
        }

        private static void _IniciarSesionFiel()
        {
            var certificado = _ObtenerCertificadoAplicacion();
            var ip = _IpWebServices();
            var serial = _SerialWebServices();
            var usuario = _UsuarioWebServices();
            var contrasena = _ContrasenaWebServices();

            var fielSesion = new WsFielSesion.FirmaIWS();
            //var fielSesion1 = new WebReferenceFirma.WebService();
            //var fielSesion2 = new TFielLib.TPkcs1();


            //nonName NA
            var respuestaSesion = fielSesion.IniciarSesion(certificado, ip, serial, usuario, contrasena);

            var jsonToken = JObject.Parse(respuestaSesion[1]);

            var loginCall = new WSLoginCall
            {
                fecha = DateTime.Now,
                serieCertificado = _SerieCertificadoAplicacion(),
                mensaje = respuestaSesion[1],
                status = int.Parse(respuestaSesion[0]),
                usuario = usuario
            };
            RepositorioBodevaFiel.GuardarLoginCall(loginCall);

            string valorToken;

            if (respuestaSesion[0].Equals(@"0"))
            {
                valorToken = (string)jsonToken[@"sesion"];
                
                var tokenDesde = (string)jsonToken["desde"];
                var tokenHasta = (string)jsonToken["vence"];

                var token = new token
                {
                    idwscall = loginCall.id,
                    appid = _IdentificadorAplicacion(),
                    valorToken = valorToken,
                    status = loginCall.status,
                    validoDesde = DateTime.ParseExact(tokenDesde, "dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture),
                    validoHasta = DateTime.ParseExact(tokenHasta, "dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture),
                };

                RepositorioBodevaFiel.GuardarToken(token);

                IdToken = token.id;
            }
            else
            {
                _token = null;
                throw new FielException(@"No fue posible iniciar sesión en el WebServices de Firma. Codigo: " + respuestaSesion[0]);
            }

            _token = valorToken;
        }

        private static byte[] _ObtenerCertificadoAplicacion()
        {
            var ubicacionCertificado = ConfigurationManager.AppSettings[@"DireccionCertificadoAplicacion"];

            var rutaCertificado = AppDomain.CurrentDomain.BaseDirectory + ubicacionCertificado;

            var certificado = System.IO.File.ReadAllBytes(rutaCertificado);

            return certificado;
        }

        private static string _IpWebServices()
        {
            return ConfigurationManager.AppSettings[@"FielIp"];
        }

        private static string _SerieCertificadoAplicacion()
        {
            return ConfigurationManager.AppSettings[@"SerieCertificadoAplicacion"];
        }

        private static string _UsuarioWebServices()
        {
            return ConfigurationManager.AppSettings[@"FielUsuario"];
        }

        private static string _ContrasenaWebServices()
        {
            return ConfigurationManager.AppSettings[@"FielContrasena"];
        }

        private static string _SerialWebServices()
        {
            return ConfigurationManager.AppSettings[@"FielSerial"];
        }

        private static string _IdentificadorAplicacion()
        {
            return ConfigurationManager.AppSettings[@"IdentificadorAplicacion"];
        }
        #endregion

        #region Ocx
        internal long RegistrarFirma(DateTime fecha)
        {
            var operacion = new operacion
            {
                idtoken = IdToken,
                fecha = DateTime.Now,
                hora = DateTime.Now.TimeOfDay,
                resultadoOperacion = "Firmado correctamente",
                status = 0,
                certificadoUsuario = _certificado,
                serieCertificado = _serieCertificado,
                tipo = "Firmar OCX TSigner",
                usuario = _usuario
            };
            RepositorioBodevaFiel.GuardarOperacion(operacion);

            var cadena = _RegistrarCadena(fecha, operacion.id);

            return cadena.id;
        }
        #endregion

        #region Autenticacion
        internal void AutenticarCertificado()
        {
            var fielServicios = new WsFielServicios.FirmaCWS();

            string[] respuesta = null;

            try
            {
                for (var intentos = 1; intentos <= 2; intentos++)
                {
                    respuesta = fielServicios.AutenticarCertificado(_certificado, Token);

                    var operacion = new operacion
                    {
                        idtoken = IdToken,
                        fecha = DateTime.Now,
                        hora = DateTime.Now.TimeOfDay,
                        resultadoOperacion = respuesta[1],
                        status = int.Parse(respuesta[0]),
                        certificadoUsuario = _certificado,
                        serieCertificado = _serieCertificado,
                        tipo = "Autenticar Certificado",
                        usuario = _usuario
                    };
                    RepositorioBodevaFiel.GuardarOperacion(operacion);

                    _RegistrarCadena(operacion.fecha, operacion.id);

                    if (!respuesta[0].Equals(@"3000")) break;

                    Token = null;
                }
            }
            catch (Exception ex)
            {
                throw new FielException(ex.Message);
            }

            if(respuesta == null) throw new FielException("Error al tratar de autenticar el certificado.");

            switch (respuesta[0])
            {
                case @"0":
                    //certificado correcto
                    break;
                case @"1":
                    throw new FielException("Certificado revocado.");
                case @"3":
                    throw new FielException("Error con el servicio OSCP.");
                default:
                    throw new FielException("No es posible realizar la acción. Intente más tarde. Codigo: " +
                                            respuesta[0]);
            }
        }

        public static void NoOperacionAutenticarCertificado(byte[] certificado)
        {
            var fielServicios = new WsFielServicios.FirmaCWS();

            string[] respuesta = null;

            try
            {
                for (var intentos = 1; intentos <= 2; intentos++)
                {
                    respuesta = fielServicios.AutenticarCertificado(certificado, Token);
                    
                    if (!respuesta[0].Equals(@"3000")) break;

                    Token = null;
                }
            }
            catch (Exception ex)
            {
                throw new FielException(ex.Message);
            }

            if (respuesta == null) throw new FielException("Error al tratar de autenticar el certificado.");

            switch (respuesta[0])
            {
                case @"0":
                    //certificado correcto
                    break;
                case @"1":
                    throw new FielException("Certificado revocado.");
                case @"3":
                    throw new FielException("Error con el servicio OSCP.");
                default:
                    throw new FielException("No es posible realizar la acción. Intente más tarde. Codigo: " +
                                            respuesta[0]);
            }
        }
        #endregion

        #region Verificacion
        internal void VerificarPkcs7()
        {
            var fielServicios = new WsFielServicios.FirmaCWS();

            var respuesta = new[] {"1"};

            try
            {
                for (var intentos = 1; intentos <= 2; intentos++)
                {
                    respuesta = fielServicios.VerificarPkCS7(_pkcs7, Token);

                    var operacion = new operacion
                    {
                        idtoken = IdToken,
                        fecha = DateTime.Now,
                        hora = DateTime.Now.TimeOfDay,
                        resultadoOperacion = respuesta[1],
                        status = int.Parse(respuesta[0]),
                        certificadoUsuario = _certificado,
                        serieCertificado = _serieCertificado,
                        tipo = "Verificar Pkcs7",
                        usuario = _usuario
                    };
                    RepositorioBodevaFiel.GuardarOperacion(operacion);

                    _RegistrarCadena(operacion.fecha, operacion.id);

                    if (!respuesta[0].Equals(@"3000")) break;

                    Token = null;
                }
            }
            catch (Exception ex)
            {
                throw new FielException(ex.Message);
            }

            if (respuesta[0] != "0")
                throw new FielException("Error al verificar el PKCS7. Codigo: " + respuesta[0]);
        }

        public static void NoOperacionVerificarPkcs7(byte[] pkcs7)
        {
            var fielServicios = new WsFielServicios.FirmaCWS();

            var respuesta = new[] { "1" };

            try
            {
                for (var intentos = 1; intentos <= 2; intentos++)
                {
                    respuesta = fielServicios.VerificarPkCS7(pkcs7, Token);

                    if (!respuesta[0].Equals(@"3000")) break;

                    Token = null;
                }
            }
            catch (Exception ex)
            {
                throw new FielException(ex.ToString());
            }

            if (respuesta[0] != "0")
                throw new FielException("Error al verificar el PKCS7. Codigo: " + respuesta[0]);
        }
        #endregion

        #region Decodificacion
        internal string[] DecodificarPkcs7(long idCadena)
        {
            var fiel = new WsFielServicios.FirmaCWS();

            string[][] pkcs7Decodificado;

            try
            {
                pkcs7Decodificado = fiel.DecodificarPkcs7(_pkcs7, Token);

                var operacion = new operacion
                {
                    idtoken = IdToken,
                    fecha = DateTime.Now,
                    hora = DateTime.Now.TimeOfDay,
                    resultadoOperacion = "PKCS7 decodificado correctamente.",
                    status = 0,
                    certificadoUsuario = _certificado,
                    serieCertificado = _serieCertificado,
                    tipo = "Decodificar Pkcs7",
                    usuario = _usuario
                };
                RepositorioBodevaFiel.GuardarOperacion(operacion);

                _RegistrarCadena(operacion.fecha, operacion.id);

                var decodificacion = new decodificacion
                {
                    idCadena = idCadena,
                    firma = OperacionesFiel.ConvertirPkcs7Bytes(pkcs7Decodificado[0][0]),
                    certificado = OperacionesFiel.ConvertirCertificadoBytes(pkcs7Decodificado[0][1]),
                    serie = pkcs7Decodificado[0][2],
                    campos = pkcs7Decodificado[0][3],
                    idOperacion = operacion.id
                };
                RepositorioBodevaFiel.GuardarDecodificacion(decodificacion);
            }
            catch (Exception ex)
            {
                throw new FielException(ex.Message);
            }

            return pkcs7Decodificado[0];
        }

        internal string[] DecodificarPkcs7(byte[] _pkcs7)
        {
            var fiel = new WsFielServicios.FirmaCWS();

            string[][] pkcs7Decodificado;

            try
            {
                pkcs7Decodificado = fiel.DecodificarPkcs7(_pkcs7, Token);

                var operacion = new operacion
                {
                    idtoken = IdToken,
                    fecha = DateTime.Now,
                    hora = DateTime.Now.TimeOfDay,
                    resultadoOperacion = "PKCS7 decodificado correctamente.",
                    status = 0,
                    certificadoUsuario = _certificado,
                    serieCertificado = _serieCertificado,
                    tipo = "Decodificar Pkcs7",
                    usuario = _usuario
                };
                RepositorioBodevaFiel.GuardarOperacion(operacion);

                _RegistrarCadena(operacion.fecha, operacion.id);

                var decodificacion = new decodificacion
                {
                    idCadena = 1,
                    firma = OperacionesFiel.ConvertirPkcs7Bytes(pkcs7Decodificado[0][0]),
                    certificado = OperacionesFiel.ConvertirCertificadoBytes(pkcs7Decodificado[0][1]),
                    serie = pkcs7Decodificado[0][2],
                    campos = pkcs7Decodificado[0][3],
                    idOperacion = operacion.id
                };
                RepositorioBodevaFiel.GuardarDecodificacion(decodificacion);
            }
            catch (Exception ex)
            {
                throw new FielException(ex.Message);
            }

            return pkcs7Decodificado[0];
        }

        public static string[] NoOperacionDecodificarPkcs7(byte[] pkcs7)
        {
            var fiel = new WsFielServicios.FirmaCWS();

            string[][] pkcs7Decodificado;

            try
            {
                pkcs7Decodificado = fiel.DecodificarPkcs7(pkcs7, Token);
            }
            catch (Exception ex)
            {
                throw new FielException(ex.Message);
            }

            return pkcs7Decodificado[0];
        }
        #endregion

        #region TimeStamping
        internal long GenerarEstampillaTiempo(byte[] digestion, long idCadena)
        {
            var fielServicios = new WsFielServicios.FirmaCWS();

            var respuesta = new string[1];
            timestamping timestamping = null;
            long idOperacion = 0;

            for (var intentos = 1; intentos <= 2; intentos++)
            {
                respuesta = fielServicios.GenerarEstampillaTiempo(digestion, Token);

                var operacion = new operacion
                {
                    idtoken = IdToken,
                    fecha = DateTime.Now,
                    hora = DateTime.Now.TimeOfDay,
                    resultadoOperacion = respuesta[1],
                    status = int.Parse(respuesta[0]),
                    certificadoUsuario = _certificado,
                    serieCertificado = _serieCertificado,
                    tipo = "Solicitar Estampilla de Tiempo",
                    usuario = _usuario
                };
                RepositorioBodevaFiel.GuardarOperacion(operacion);

                _RegistrarCadena(operacion.fecha, operacion.id);

                timestamping = new timestamping
                {
                    idcadena = idCadena,
                    idoperacion = operacion.id,
                    timestamp = OperacionesFiel.ConvertirTimeStampingBytes(respuesta[1])
                };
                RepositorioBodevaFiel.GuardarTimeStamping(timestamping);

                idOperacion = operacion.id;

                if (!respuesta[0].Equals(@"3000")) break;

                Token = null;
            }

            if (!respuesta[0].Equals(@"0"))
                throw new FielException(@"Error al solicitar la estampilla de tiempo. Codigo " + respuesta[0]);

            if (timestamping?.timestamp == null || timestamping.timestamp.Length <= 10)
                throw new FielException(@"La estampilla de tiempo generada no fue generada correctamente.");

            return idOperacion;
        }
        #endregion

        #region Xml
        internal void GuardarXml(string rutaXml, long idCadena)
        {
            var operacion = new operacion
            {
                idtoken = IdToken,
                fecha = DateTime.Now,
                hora = DateTime.Now.TimeOfDay,
                resultadoOperacion = "XML",
                status = 0,
                certificadoUsuario = _certificado,
                serieCertificado = _serieCertificado,
                tipo = "Generación del XML",
                usuario = _usuario
            };
            RepositorioBodevaFiel.GuardarOperacion(operacion);

            _RegistrarCadena(operacion.fecha, operacion.id);

            var xml = new xml
            {
                idcadena = idCadena,
                idoperacion = operacion.id,
                rutaxml = rutaXml,
                foliogenerado = string.Empty
            };
            RepositorioBodevaFiel.GuardarXml(xml);
        }
        #endregion
    }
}
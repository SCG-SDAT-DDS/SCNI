using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Datos.DTO.Infraestructura;
using Datos.Enums;
using Datos.Repositorios;
using Datos.Repositorios.Carta;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Firma;
using Datos.Repositorios.Solicitudes;
using Negocio.Servicios;
using Sistema;

namespace Negocio.Firma
{
    public class OperacionesFiel
    {
        private readonly byte[] _certificadoFirmante;
        private readonly int _idEmpleado;
        private readonly int _idUsuario;
        private readonly string _usuario;
        private readonly string _serieCertificado;

        internal OperacionesFiel(SistemaUsuario usuario, byte[] certificadoFirmante, string serieCertificado)
        {
            _idEmpleado = usuario.IdEmpleado;
            _idUsuario = usuario.IdUsuario;
            _usuario = usuario.NombreUsuario;
            _certificadoFirmante = certificadoFirmante;
            _serieCertificado = serieCertificado;
        }

        internal void RegistrarFirmas(List<FirmaCarta> firmas)
        {
            var lote = new Lote
            {
                FechaInicio = DateTime.Now,
                IDUsuario = _idUsuario,
                Total = firmas.Count,
                Estado = LoteEstados.Empezado
            };

            using (var conexion = new Contexto())
            {
                var repositorioLotes = new RepositorioLotes(conexion);
                repositorioLotes.Guardar(lote);
                conexion.SaveChanges();
            }

            var idCartasFirmadas = new List<int>();

            var isCorrectoFirmas = true;
            var isCorrectoLote = true;

            try
            {
                
                foreach (var fiel in firmas)
                {
                    var serviciosFiel = new ServiciosWebFiel(_usuario, _serieCertificado, _certificadoFirmante, fiel.Digestion, fiel.Pkcs7);

                    var firma = new Datos.Firma
                    {
                        IDCarta = fiel.IdCarta,
                        IDLote = lote.IDLote,
                        IDEmpleado = _idEmpleado,
                        Valor = fiel.Firma
                    };

                    try
                    {

                        //serviciosFiel.AutenticarCertificado();

                        //firma.BovedaFiel_IDCadena = serviciosFiel.RegistrarFirma(lote.FechaInicio);

                        //serviciosFiel.VerificarPkcs7();

                        //firma.BovedaFiel_IDTimeStamping = serviciosFiel.GenerarEstampillaTiempo(fiel.Digestion,
                        //    firma.BovedaFiel_IDCadena.Value);

                        //var decodificadoPkcs7 = serviciosFiel.DecodificarPkcs7(fiel.Pkcs7);
                        //firma.Valor = decodificadoPkcs7[0];

                        ////var certificadoPkcs7 = ConvertirCertificadoBytes(decodificadoPkcs7[1]);

                        ////if (!certificadoPkcs7.SequenceEqual(_certificadoFirmante))
                        ////    throw new FielException("El PKCS7 no corresponde al firmante.");

                        //var entradaXml = new XmlFielEntrada
                        //{
                        //    Folio = fiel.Folio,
                        //    Digestion = Convert.ToBase64String(fiel.Digestion),
                        //    Fecha = lote.FechaInicio,
                        //    Pkcs7 = decodificadoPkcs7
                        //};

                        //var xmlFiel = new XmlFiel(entradaXml);
                        //var tmpXml = xmlFiel.GenerarXml();

                        //var ubicacionRelativaXml = @"\Archivos\Xml\" + DateTime.Now.Year + @"\" + DateTime.Now.Month
                        //                           + @"\" + DateTime.Now.Day + @"\";
                        //var ubicacionAbsolutaXml = AppDomain.CurrentDomain.BaseDirectory + ubicacionRelativaXml;

                        //if (!Directory.Exists(ubicacionAbsolutaXml))
                        //{
                        //    Directory.CreateDirectory(ubicacionAbsolutaXml);
                        //}

                        //firma.UbicacionXml = ubicacionRelativaXml + fiel.Folio.Replace(@"/", "_") + firma.BovedaFiel_IDCadena + ".xml";

                        //var ubicacionXml = AppDomain.CurrentDomain.BaseDirectory + firma.UbicacionXml;
                        //if (File.Exists(ubicacionXml)) File.Delete(ubicacionXml);
                        //File.Move(tmpXml, ubicacionXml);

                        //serviciosFiel.GuardarXml(firma.UbicacionXml, firma.BovedaFiel_IDCadena.Value);

                        firma.Estado = FirmaEstados.Terminado;

                        idCartasFirmadas.Add(fiel.IdCarta);
                    }
                    catch (FielException ex)
                    {
                        isCorrectoFirmas = false;

                        firma.Error = ex.ToString();
                        firma.Estado = FirmaEstados.Error;
                    }

                    using (var conexion = new Contexto())
                    {
                        var repositorioFirmas = new RepositorioFirmas(conexion);

                        var existeFirma = firma.Estado == FirmaEstados.Terminado &&
                                          repositorioFirmas.ExisteFirma(firma.IDCarta);

                        if (existeFirma)
                        {
                            isCorrectoFirmas = false;

                            firma.Error = "Constancia ya firmada";
                            firma.Estado = FirmaEstados.Error;

                            idCartasFirmadas.RemoveAt(idCartasFirmadas.Count - 1);
                        }

                        repositorioFirmas.Guardar(firma);

                        if (firma.Estado == FirmaEstados.Terminado)
                            repositorioFirmas.FirmarCarta(firma.IDCarta, fiel.NombreFirmante, fiel.PuestoFirmante, fiel.Fecha);

                        conexion.SaveChanges();

                        var usuario = new UsuarioRepositorio(conexion).BuscarPorIdEmpleado(_idEmpleado);

                        var movimientoRepositorio = new MovimientoRepositorio(conexion);
                        var movimiento = new Movmiento
                        {
                            Nombre = "Firma",
                            Usuario = usuario,
                            Habilitado = true,
                            Fecha = DateTime.Now,
                            Catalogo = "Firma",
                            IDRegistro = firma.IDFirma
                        };
                        movimientoRepositorio.Guardar(movimiento);

                        conexion.SaveChanges();
                    }
                }

                lote.Estado = LoteEstados.Terminado;
            }
            catch (Exception ex)
            {
                isCorrectoLote = false;

                lote.Error = ex.ToString();
                lote.Estado = LoteEstados.Error;
            }

            using (var conexion = new Contexto())
            {
                var repositorioLotes = new RepositorioLotes(conexion);
                repositorioLotes.CambiarEstado(lote.IDLote, DateTime.Now, lote.Estado, lote.Error);
                conexion.SaveChanges();
            }
            
            new Task(() => _EnviarCorreos(idCartasFirmadas)).Start();

            if (!isCorrectoLote)
            {
                throw new FielException("Hubo un error al firmar el lote. Es posible que algunas constancias no hayan sido firmada.");
            }

            if (!isCorrectoFirmas)
            {
                throw new FielException("Hubo un error al firmar algunas contancias.");
            }
        }

        internal void RechazarFirmas(List<FirmaCarta> firmas)
        {
            using (var conexion = new Contexto())
            {
                var repositorio = new CartasRepositorio(conexion);
                var repSolicitud = new SolicitudRepositorio(conexion);

                var usuario = new Usuario {IDUsuario = _idUsuario};

                conexion.Usuario.Attach(usuario);

                foreach (var firma in firmas)
                {
                    repSolicitud.CancelarSolicitud(firma.IdCarta, "Constancia cancelada");

                    repositorio.EliminarCarta(firma.IdCarta);

                    var movimientoRepositorio = new MovimientoRepositorio(conexion);
                    var movimiento = new Movmiento
                    {
                        Nombre = "Rechazo",
                        Usuario = usuario,
                        Habilitado = true,
                        Fecha = DateTime.Now,
                        Catalogo = "Firma",
                        IDRegistro = firma.IdCarta
                    };
                    movimientoRepositorio.Guardar(movimiento);

                    conexion.SaveChanges();
                }
            }
        }

        private void _EnviarCorreos(List<int> idCartasFirmadas)
        {
            using (var conexion = new Contexto())
            {
                var repositorioCartas = new CartasRepositorio(conexion);
                foreach (var idCarta in idCartasFirmadas)
                {
                    var correo = repositorioCartas.ObtenerCorreoCodigoSolicitante(idCarta);
                    
                    if (!string.IsNullOrEmpty(correo))
                    {
                        try
                        {
                            var serviciosCarta = new ServiciosCarta();
                            var archivoCarta = serviciosCarta.GenerarArchivoCarta(idCarta);

                            Correo.EnviarConstancia(correo, archivoCarta.Key, archivoCarta.Value);
                        }
                        catch (Exception ex)
                        {
                            new Log().AgregarError(ex.ToString());
                        }
                    }
                }
            }
        }

        #region Digestion
        public static byte[] GenerarDigestion(string cadena)
        {
            return _Digerir(cadena, new SHA1Managed());
        }

        public static string Generar64Digestion(string cadena)
        {
            var digestion = _Digerir(cadena, new SHA1Managed());

            return Convert.ToBase64String(digestion);
        }

        private static byte[] _Digerir(string cadena, HashAlgorithm sha1)
        {
            var bytesString = Encoding.ASCII.GetBytes(cadena);
            
            var digestion = sha1.ComputeHash(bytesString);

            return digestion;
        }
        #endregion

        #region Conversion
        public static byte[] ConvertirPkcs7Bytes(string pkcs7)
        {
            try
            {
                if (pkcs7.Contains(Environment.NewLine))
                    return Convert.FromBase64String(pkcs7);
                else
                    return Convert.FromBase64String(pkcs7.Replace(@"\n", @"\r\n"));
            }
            catch
            {
                throw new FielException("Error al convertir el PKCS7 a arreglo de bytes.");
            }
        }

        public static string ConvertirPkcs7Bytes(byte[] pkcs7)
        {
            try
            {
                return Convert.ToBase64String(pkcs7);
            }
            catch
            {
                throw new FielException("Error al convertir el PKCS7 a BASE64.");
            }
        }

        public static byte[] ConvertirCertificadoBytes(string certificado)
        {
            try
            {
                return Convert.FromBase64String(certificado);
            }
            catch
            {
                throw new FielException("Error al convertir el certificado a arreglo de bytes.");
            }
        }

        public static byte[] ConvertirDigestionBytes(string digestion)
        {
            try
            {
                return Convert.FromBase64String(digestion);
            }
            catch
            {
                throw new FielException("Error al convertir la digestión a arreglo de bytes.");
            }
        }

        public static byte[] ConvertirTimeStampingBytes(string timestamping)
        {
            try
            {
                return Convert.FromBase64String(timestamping);
            }
            catch
            {
                throw new FielException("Error al convertir el timestamping a arreglo de bytes.");
            }
        }
        #endregion
    }
}

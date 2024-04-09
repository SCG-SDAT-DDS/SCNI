using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sistema
{
    public class Correo
    {
        public static async Task<bool> RestablecerContrasena(string destinatario, string asunto, string codigo)
        {
            var msg = new MailMessage();
            var enlace = DireccionWebSitio() + "login/RestablecerContrasena?c=" + codigo;

            msg.To.Add(destinatario);
            msg.From = new MailAddress(CorreoElectronico(), NombreAplicacion(), Encoding.UTF8);
            msg.Subject = asunto;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = "<h2>Restablecer contraseña</h2><br/><a href='" + enlace + "'>" + enlace + "</a>";
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;

            //Si vas a enviar un correo con contenido html entonces cambia el valor a true
            //Aquí es donde se hace lo especial
            var clienteSmtp = PropiedadesServidorCorreo();
            try
            {
                await clienteSmtp.SendMailAsync(msg);
                return true;
            }
            catch (SmtpFailedRecipientsException)
            {
                return false;
            }
            catch (SmtpException)
            {
                return false;
            }
        }

        public static void EnviarConstancia(string correo, string oficio, byte[] archivoConstancia)
        {
            using (var msg = new MailMessage())
            {
                var urlImagen = "http://constanciasni.sonora.gob.mx/Content/img/logo-sonora-correo.png";

                msg.To.Add(correo.Trim());
                msg.From = new MailAddress(CorreoElectronico(), NombreAplicacion(), Encoding.UTF8);
                msg.Subject = "Constancia de No Inhabilitación " + oficio;
                msg.SubjectEncoding = Encoding.UTF8;
                msg.Body = "<br/> Estimado Usuario:" +
                           "<br/><br/>" +
                           "Se envía por medio del  presente correo electrónico Constancia de No Inhabilitación " +
                           "solicitada, quedamos a sus órdenes para cualquier duda y/o aclaración." +
                           "<br/><br/><br/>" +
                            "¡Su opinión es importante para nosotros! Por favor, ayúdenos a mejorar nuestro servicio, " +
                           "completando esta breve encuesta. " +
                           "Agradecemos de la manera más atenta su apoyo y tiempo brindado." +
                           "<br/><br/><br/>" +
                           "<a href='https://forms.office.com/r/72dxRgyPB7'>De click en este link para iniciar encuesta</a>" +
                           "<br/><br/><br/>" +
                           "QUEDA PROHIBIDA LA MODIFICACIÓN, ALTERACIÓN, MANIPULACIÓN, TACHADURAS, ENMENDADURAS " +
                           "DE ESTE DOCUMENTO, EL REGISTRO DE DATOS FALSOS PARA SU OBTENCIÓN Y EL USO INDEBIDO DE LA MISMA." +
                           "<br/><br/>" +
                           "Coordinación Ejecutiva De Sustanciación Y Resolución De Responsabilidades" +
                           "<br/>" +
                           "Secretaría de la Controlaría General del Estado de Sonora" +
                           "<br/>" +
                           "Tel. 662 213 62 07 Ext. 1307" +
                           "<br/><br/><br/>" +
                           "<img src='" + urlImagen + "' height='58' width='300'/>";
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                using (var clienteSmtp = PropiedadesServidorCorreo())
                {
                    try
                    {
                        using (var st = new MemoryStream(archivoConstancia))
                        {
                            msg.Attachments.Add(new Attachment(st, oficio.Replace("/", "_") + ".docx",
                                System.Net.Mime.MediaTypeNames.Application.Octet));

                            clienteSmtp.Send(msg);
                        }
                    }
                    catch(Exception ex)
                    {
                        var log = new Log();
                        log.AgregarError(ex.Message);
                    }
                }
            }
        }

        public static void AvisarCancelacionSolitud(string correo, string folio, string motivo)
        {
            using (var msg = new MailMessage())
            {
                var urlImagen = "http://constanciasni.sonora.gob.mx/Content/img/logo-sonora-correo.png";

                msg.To.Add(correo.Trim());
                msg.From = new MailAddress(CorreoElectronico(), NombreAplicacion(), Encoding.UTF8);
                msg.Subject = "Cancelación de la Solicitud para Constancia de No Inhabilitación";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.Body = "<br/> Estimado Usuario:" +
                           "<br/><br/>" +
                           "Se envía por medio del  presente correo electrónico el aviso de que su solicitud " +
                           "con el folio <strong>" + folio + "</strong> ha sido cancelada por el siguiente motivo:" +
                           " <br/><br/>" +
                           "<strong>" + motivo + "</strong>" +
                           "<br/><br/>" +
                           "Coordinación Ejecutiva De Sustanciación Y Resolución De Responsabilidades" +
                           "<br/>" +
                           "Secretaría de la Controlaría General del Estado de Sonora" +
                           "<br/>" +
                           "Tel. 662 213 62 07 Ext. 1307" +
                           "<br/><br/><br/>" +
                           "<img src='" + urlImagen + "' height='58' width='300'/>";
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                using (var clienteSmtp = PropiedadesServidorCorreo())
                {
                    try
                    {
                        clienteSmtp.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        var log = new Log();
                        log.AgregarError(ex.Message);
                    }
                }
            }
        }

        public static void EnviarPaseCaja(string correo, string folio, byte[] archivoPaseCaja)
        {
            using (var msg = new MailMessage())
            {
                var urlImagen = "http://constanciasni.sonora.gob.mx/Content/img/logo-sonora-correo.png";

                msg.To.Add(correo.Trim());
                msg.From = new MailAddress(CorreoElectronico(), NombreAplicacion(), Encoding.UTF8);
                msg.Subject = "Pase Caja para Constancia de No Inhabilitación";
                msg.SubjectEncoding = Encoding.UTF8;

                msg.Body = "<br/> Estimado Usuario:" +
                           "<br/><br/>" +
                           "Se envía por medio del  presente correo electrónico, Pase a Caja " +
                           "para el trámite de Constancia de No Inhabilitación, quedamos a sus órdenes " +
                           "para cualquier duda y/o aclaración." +
                           "<br/><br/><br/>" +
                           "QUEDA PROHIBIDA LA MODIFICACIÓN, ALTERACIÓN, MANIPULACIÓN, TACHADURAS, " + 
                           "ENMENDADURAS DE ESTE DOCUMENTO, EL REGISTRO DE DATOS FALSOS PARA SU OBTENCIÓN " +
                           "Y EL USO INDEBIDO DE LA MISMA." +
                           "<br/><br/>" +
                           "Coordinación Ejecutiva De Sustanciación Y Resolución De Responsabilidades" +
                           "<br/>" +
                           "Secretaría de la Controlaría General del Estado de Sonora" +
                           "<br/>" +
                           "Tel. 662 213 62 07 Ext. 1307" +
                           "<br/><br/><br/>" +
                           "<img src='" + urlImagen + "' height='58' width='300'/>";

                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                using (var clienteSmtp = PropiedadesServidorCorreo())
                {
                    try
                    {
                        using (var st = new MemoryStream(archivoPaseCaja))
                        {
                            msg.Attachments.Add(new Attachment(st, "PaseCaja_" + folio + ".pdf",
                                System.Net.Mime.MediaTypeNames.Application.Octet));

                            clienteSmtp.Send(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        var log = new Log();
                        log.AgregarError(ex.Message);
                    }
                }
            }
        }

        #region Website
        public static string DireccionWebSitio()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DireccionWebSitio"];
        }

        /// <summary>
        /// Obtiene el nombre de la aplicacion
        /// </summary>
        /// <returns>Nombre de la aplicacion</returns>
        public static string NombreAplicacion()
        {
            return System.Configuration.ConfigurationManager.AppSettings["NombreAplicacion"];
        }
        #endregion

        #region CorreoElectronico

        /// <summary>
        /// Contiene la direccion de correo electronico del sistema
        /// </summary>
        /// <returns>Direccion de correo electronico del sistema</returns>
        public static string CorreoElectronico()
        {
            return System.Configuration.ConfigurationManager.AppSettings["CorreoElectronico"];
        }

        /// <summary>
        /// Contiene el nombre de usuario de la cuenta de correo electronico del sistema
        /// </summary>
        /// <returns>Nombre de usuario de la cuenta de correo electronico del sistema</returns>
        public static string UsuarioCorreoElectronico()
        {
            return System.Configuration.ConfigurationManager.AppSettings["UsuarioCorreo"];
        }

        /// <summary>
        /// Contiene el password de la cuenta de correo electronico del sistema
        /// </summary>
        /// <returns>Password de la cuenta de correo electronico del sistema</returns>
        public static string PasswordCorreoElectronico()
        {
            return System.Configuration.ConfigurationManager.AppSettings["PasswordCorreo"];
        }

        /// <summary>
        /// Contiene la direccion del servidor de SMTP
        /// </summary>
        /// <returns>Direccion del servidor de SMTP</returns>
        public static string ServidorSmtp()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ServidorSmtp"];
        }

        /// <summary>
        /// Contiene la direccion del puerto para servidor de SMTP
        /// </summary>
        /// <returns>Direccion del puerto para el servidor de SMTP</returns>
        public static int PuertoSmtp()
        {
            return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PuertoSmtp"]);
        }

        /// <summary>
        /// Contiene si se usuara una conexion SSL para el envio del correo
        /// </summary>
        /// <returns>Indica si se usuara una conexion SSL para el envio del correo</returns>
        public static bool UsarSslSmtp()
        {
            return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UsarSslSmtp"]);
        }

        /// <summary>
        /// Contiene el asunto que tendra el correo electronico
        /// </summary>
        /// <returns>Asunto que tendra el correo electronico</returns>
        public static string AsuntoCorreoElectronico()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AsuntoCorreoElectronico"];
        }

        #endregion

        #region Servidor SMTP
        public static SmtpClient PropiedadesServidorCorreo()
        {
            var smtp = new SmtpClient
            {
                Port = PuertoSmtp(),
                Host = ServidorSmtp(),
                EnableSsl = UsarSslSmtp()
            };

            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new System.Net.NetworkCredential(UsuarioCorreoElectronico(), PasswordCorreoElectronico());

            return smtp;
        }
        #endregion

    }
}
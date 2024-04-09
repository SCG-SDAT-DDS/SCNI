using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Datos;
using Datos.DTO;
using Datos.DTO.Infraestructura;
using Datos.Extensiones;
using Datos.Repositorios.Carta;
using Datos.Repositorios.Catalogos;
using Datos.Repositorios.Firma;
using Datos.Repositorios.Solicitudes;
using Negocio.Carta;
using Negocio.Firma;
using Novacode;
using QRCoder;
using Sistema;
using Sistema.Extensiones;

namespace Negocio.Servicios
{
    public class ServiciosCarta
    {
        public bool GenerarCarta(int idSolicitud, int[] idSanciones)
        {
            using (var contexto = new Contexto())
            {
                var repositorioSolicitudes = new SolicitudRepositorio(contexto);
                var personaSanciones = idSanciones == null
                    ? repositorioSolicitudes.ObtenerPersonaCartaSanciones(idSolicitud)
                    : repositorioSolicitudes.ObtenerPersonaSanciones(idSolicitud, idSanciones);

                var repositorioCartas = new CartasRepositorio(contexto);
                var cartaActual = repositorioCartas.ObtenerCartaPorIdSolicitud(idSolicitud);
                
                string numeroExpediente;
                if (cartaActual == null)
                {
                    var repositorioFolio = new FoliosRepositorio(contexto);
                    var folio = repositorioFolio.NuevoFolio();
                    var sigalasFolio = ConfigurationManager.AppSettings["SiglasFolio"];

                    if (folio != null)
                    {
                        numeroExpediente = sigalasFolio + "/" + folio.Numero + "/" + DateTime.Now.Year;
                        repositorioFolio.ActualizarFolio(folio.Anio);
                    }
                    else
                    {
                        numeroExpediente = sigalasFolio + "/1/" + DateTime.Now.Year;
                        repositorioFolio.ActualizarFolio(null);
                    }
                }
                else
                {
                    numeroExpediente = cartaActual.NumeroExpediente;
                    contexto.Entry(cartaActual).State = EntityState.Detached;
                }

                var constructor = new ConstructorCarta(idSolicitud, personaSanciones, numeroExpediente);
                var carta = constructor.Generar();

                var repositorioPlantillas = new PlantillaRepostorio(contexto);

                if (cartaActual == null)
                {
                    carta.Plantilla = repositorioPlantillas.ObtenerActiva();
                    repositorioCartas.Guardar(carta);

                    var precioRepositorio = new CartaPrecioRepositorio(contexto);
                    precioRepositorio.GuardarCartaPrecioActual(carta);
                }
                else
                {
                    carta.IDCarta = cartaActual.IDCarta;

                    repositorioCartas.BorrarSanciones(carta.IDCarta);
                    repositorioCartas.Modificar(carta);
                }

                contexto.SaveChanges();

                return cartaActual == null;
            }
        }

        public KeyValuePair<string, byte[]> GenerarCartaPreliminar(int idSolicitud, int[] idSanciones)
        {
            using (var contexto = new Contexto())
            {
                var repositorioSolicitudes = new SolicitudRepositorio(contexto);
                var personaSanciones = repositorioSolicitudes.ObtenerPersonaSanciones(idSolicitud, idSanciones);

                var repositorioFolio = new FoliosRepositorio(contexto);
                var folio = repositorioFolio.NuevoFolio();
                var sigalasFolio = ConfigurationManager.AppSettings["SiglasFolio"];
                string numeroExpediente;
                if (folio != null)
                {
                    numeroExpediente = sigalasFolio + "/" + folio.Numero + "/" + DateTime.Now.Year;
                }
                else
                {
                    numeroExpediente = sigalasFolio + "/1/" + DateTime.Now.Year;
                }
                
                var constructor = new ConstructorCarta(idSolicitud, personaSanciones, numeroExpediente);
                var carta = constructor.Generar();

                var repositorioSolicitud = new SolicitudRepositorio(contexto);
                carta.Solicitud = repositorioSolicitud.BuscarPorId(idSolicitud);

                var repositorioPlantilla = new PlantillaRepostorio(contexto);
                carta.Plantilla = repositorioPlantilla.ObtenerActiva();

                var repositorioFirmas = new RepositorioFirmas(contexto);
                var empleadoFirma = repositorioFirmas.ObtenerNombreFirmanteDefault();

                return _GenerarArchivoCarta(carta, empleadoFirma);
            }
        }

        public Datos.Carta ModificarCarta(int idCarta, int[] idSanciones)
        {
            using (var contexto = new Contexto())
            {
                var repositorioSanciones = new SancionRepositorio(contexto);
                var sancionesNuevasDto = repositorioSanciones.ObtenerSanciones(idSanciones);

                var repositorioCartas = new CartasRepositorio(contexto);
                repositorioCartas.QuitarSanciones(idCarta);
                var personaCarta = repositorioCartas.ObtenerCartaPersona(idCarta);

                var constructor = new ConstructorCarta(personaCarta, sancionesNuevasDto);
                var carta = constructor.Generar();

                foreach (var sancionDto in sancionesNuevasDto)
                {
                    carta.Sanciones.Add(sancionDto.Sancion);
                }

                contexto.SaveChanges();

                return carta;
            }
        }

        public KeyValuePair<string, byte[]> GenerarArchivoCarta(int idCarta)
        {
            Datos.Carta carta;
            EmpleadoFirmaDto empleadoFirma;

            using (var contexto = new Contexto())
            {
                var repositorioCarta = new CartasRepositorio(contexto);
                carta = repositorioCarta.BuscarUnoSolo(c => c.IDCarta == idCarta,
                    new[] {"Solicitud.Persona", "Plantilla"});

                var repositorioFirmas = new RepositorioFirmas(contexto);

                if (carta.Estado == EstadosCarta.Firmada)
                {
                    empleadoFirma = repositorioFirmas.ObtenerNombreFirma(idCarta);
                }
                else
                {
                    empleadoFirma = repositorioFirmas.ObtenerNombreFirmanteDefault();
                    carta.Cadena = Datos.Carta.ReemplazarFechaCadena(carta.Cadena, DateTime.Now.ObtenerFechaCompleta());
                }
            }

            return _GenerarArchivoCarta(carta, empleadoFirma);
        }

        private static KeyValuePair<string, byte[]> _GenerarArchivoCarta(Datos.Carta carta, EmpleadoFirmaDto empleadoFirma)
        {
            var partesCadena = carta.Cadena.Split('|').ToList();

            var plantillaCarta = Archivo.ObtenerPlantilla(carta.Plantilla.NombrePlantilla);

            var esSegundaVersion = partesCadena[4].StartsWith("NO") || partesCadena[4].StartsWith("SÍ");

            if (esSegundaVersion == false)
            {
                if (partesCadena[4].StartsWith("no"))
                {
                    const string adminstracionEstatal = "dentro de la Administración Pública Municipal y Estatal";

                    partesCadena.Insert(4, string.Empty);
                    partesCadena[5] = partesCadena[5].Replace(adminstracionEstatal, "");
                    partesCadena.Insert(6, adminstracionEstatal);
                }
                else
                {
                    var textoEstatal = partesCadena[4];

                    partesCadena[4] = textoEstatal.Substring(11);

                    var partesTextoEstatal = textoEstatal.Split(',');
                    var lugarAdministracion = partesTextoEstatal[partesTextoEstatal.Count() - 1];

                    partesCadena[4] = partesCadena[4].Substring(0, partesCadena[4].Length - lugarAdministracion.Length);

                    partesCadena.Insert(4, "cuenta con ");
                    partesCadena.Insert(6, lugarAdministracion);
                }

                partesCadena.Insert(partesCadena[7].StartsWith("no") ? 8 : 7, string.Empty);
            }
            else
            {
                partesCadena[4] = partesCadena[4].StartsWith("NO") ? "NO" : "SÍ";
            }

            var nombre = empleadoFirma.Nombre.ToTitleCase();
            var puesto = empleadoFirma.Puesto.ToTitleCase();
            var puestoCorto = empleadoFirma.PuestoCorto.ToUpper();

            using (var streamPlantilla = new MemoryStream(plantillaCarta))
            {
                var documentoCarta = DocX.Load(streamPlantilla);

                var indiceRepetirNombre = esSegundaVersion ? 6 : 9;

                //5 cuando es vista preliminar
                //7 cuando esta firmada y no tiene texto de sancion
                if (esSegundaVersion && (partesCadena.Count == 5 || partesCadena.Count == 7))
                {
                    partesCadena.Insert(5, string.Empty);
                }

                for (var i = 0; i < partesCadena.Count; i++)
                {
                    var etiqueta = esSegundaVersion
                        ? ConstructorCarta.EtiquetasCarta2[i]
                        : ConstructorCarta.EtiquetasCarta[i];

                    var texto = partesCadena[i];

                    if (etiqueta == ConstructorCarta.EtiquetasCarta2[5] && texto != string.Empty)
                        texto = " " + texto;
                    
                    documentoCarta.ReplaceText(etiqueta, texto, false, RegexOptions.IgnoreCase);
                    
                    //agrega el nombre del firmante debajo de la firma
                    if (i == indiceRepetirNombre)
                        documentoCarta.ReplaceText("<nombre_firma>", texto.ToUpper(), false, RegexOptions.IgnoreCase);
                }

                var textoFechaCompleto = string.Format("{0} de {1} del año {2}",
                    carta.Fecha.Day.ToDayWords(),
                    carta.Fecha.ToString("MMMM", new CultureInfo("es-ES")),
                    carta.Fecha.Year.ToYearWords());

                documentoCarta.ReplaceText("<fecha_hoy>", textoFechaCompleto, false, RegexOptions.IgnoreCase);

                string articuloGenero;
                var esMasculino = carta.Solicitud.Persona.Genero.Contains(((int) GenerosPersona.Masculino).ToString());

                if (esSegundaVersion)
                {
                    articuloGenero = esMasculino ? "inhabilitado al" : "inhabilitada a la";
                }
                else
                {
                    articuloGenero = esMasculino ? "el" : "la";
                }
                
                documentoCarta.ReplaceText("<articulo>", articuloGenero, false, RegexOptions.IgnoreCase);
                
                documentoCarta.ReplaceText("<nombre_firmante>", nombre, false, RegexOptions.IgnoreCase);
                documentoCarta.ReplaceText("<nombre_firma>", nombre.ToUpper(), false, RegexOptions.IgnoreCase);
                
                documentoCarta.ReplaceText("<puesto_firmante>", puesto, false, RegexOptions.IgnoreCase);
                documentoCarta.ReplaceText("<puesto_firma>", puestoCorto, false, RegexOptions.IgnoreCase);

                documentoCarta.ReplaceText("<firma>", empleadoFirma.Firma ?? "SIN FIRMA ELECTRÓNICA AVANZADA", false, RegexOptions.IgnoreCase);

                if (carta.Estado == EstadosCarta.NoFirmada)
                {
                    carta.Cadena += "|" + nombre + "|" + puesto;
                }
                documentoCarta.ReplaceText("<cadena>", carta.Cadena, false, RegexOptions.IgnoreCase);

                var urlValidacion = "http://constanciasni.sonora.gob.mx/Validacion/Constancia?codigo=" + carta.Solicitud.Codigo;

                using (var generadorQr = new QRCodeGenerator())
                using (var datosQr = generadorQr.CreateQrCode(urlValidacion, QRCodeGenerator.ECCLevel.Q))
                using (var codigoQr = new QRCode(datosQr))
                using (var imagenQr = codigoQr.GetGraphic(2))
                using (var ms = new MemoryStream())
                {
                    imagenQr.Save(ms, ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);

                    var imagenDocQr = documentoCarta.AddImage(ms);

                    var fotoQr = imagenDocQr.CreatePicture();

                    var qrParrafo = documentoCarta.InsertParagraph(string.Empty, false);
                    qrParrafo.Alignment = Alignment.left;

                    qrParrafo.InsertPicture(fotoQr);
                }

                var textoValidacion = "Para validar la constancia utilice el código QR o consulte la siguiente liga: " +
                                      urlValidacion;

                documentoCarta.ReplaceText("<texto_validacion>", textoValidacion, false, RegexOptions.IgnoreCase);

                if (!String.IsNullOrEmpty(empleadoFirma.UrlFirma))
                {
                    var ubicacionFirmas = ConfigurationManager.AppSettings["UbicacionImagenesFirmas"];

                    if (string.IsNullOrEmpty(ubicacionFirmas)) ubicacionFirmas = AppDomain.CurrentDomain.BaseDirectory;

                    var ubicacionImagenFirma = ubicacionFirmas + empleadoFirma.UrlFirma;

                    using (var imagenFirma = System.Drawing.Image.FromFile(ubicacionImagenFirma))
                    {
                        var reemplazar = documentoCarta.Images[1];

                        using (var b = new Bitmap(reemplazar.GetStream(FileMode.Open, FileAccess.ReadWrite)))
                        using (var g = Graphics.FromImage(b))
                        {
                            g.DrawImage(imagenFirma, 0, 0);

                            b.Save(reemplazar.GetStream(FileMode.Create, FileAccess.Write), ImageFormat.Png);
                        }
                    }
                }

                documentoCarta.AddProtection(EditRestrictions.readOnly, "constancia*#@=???123");

                using (var streamCarta = new MemoryStream())
                {
                    documentoCarta.SaveAs(streamCarta);
                    return new KeyValuePair<string, byte[]>(carta.NumeroExpediente, streamCarta.ToArray());
                }
            }
        }

        public void FirmarCartas(SistemaUsuario usuario, byte[] certificado, string serieCertificado, List<FirmaCarta> firmas)
        {
            //if (certificado == null || string.IsNullOrEmpty(serieCertificado))
            //    throw new FielException("No fue posible firmar ya que el usuario no cuenta con FIEL.");

            var fiel = new OperacionesFiel(usuario, certificado, serieCertificado);
            fiel.RegistrarFirmas(firmas);
        }

        public void RechazarCartas(SistemaUsuario usuario, byte[] certificado, string serieCertificado, List<FirmaCarta> firmas)
        {
            if (certificado == null || string.IsNullOrEmpty(serieCertificado))
                throw new FielException("No fue posible rechazar ya que el usuario no cuenta con FIEL.");

            var fiel = new OperacionesFiel(usuario, certificado, serieCertificado);
            fiel.RechazarFirmas(firmas);
        }
    }
}
   
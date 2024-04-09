using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Negocio.Firma
{
    internal class XmlFiel
    {
        #region Variables Privadas
        private XmlTextWriter _archivoXml;
        private readonly XmlFielEntrada _entradaXml;
        private string _rutaArchivoXml;
        #endregion

        #region Constructores
        public XmlFiel(XmlFielEntrada xmlFielEntrada)
        {
            _entradaXml = xmlFielEntrada;
        }
        #endregion

        #region Generacion XML
        public string GenerarXml()
        {
            try
            {
                InicializarXml();
                AgregarNodo(@"Informacion");
                AgregarAtributo(@"xmlns", @"http://www.firma.sonora.gob.mx/firmaschema");
                {
                    AgregarDatosPropietario();
                    AgregarDatosDelArchivo();
                }
                CerrarNodo();
                CerrarXml();

                return _rutaArchivoXml;
            }
            catch (Exception ex)
            {
                if (_archivoXml != null)
                    CerrarXml();

                if (!string.IsNullOrEmpty(_rutaArchivoXml) && File.Exists(_rutaArchivoXml))
                    File.Delete(_rutaArchivoXml);

                throw new FielException("XML. " + ex.Message);
            }
        }
        
        private void AgregarDatosPropietario()
        {
            AgregarNodo(@"DatosPropietario");
            {
                AgregarNodo(@"Dependencia");
                {
                    AgregarValor(RemoverDiacriticos(ConfigurationManager.AppSettings[@"Dependencia"]));
                }
                CerrarNodo();

                AgregarNodo(@"UnidadAdministrativa");
                {
                    AgregarValor(RemoverDiacriticos(ConfigurationManager.AppSettings[@"UnidadAdministrativa"]));
                }
                CerrarNodo();

                AgregarElemento(@"Folio", _entradaXml.Folio);
                AgregarElemento(@"Aplicacion", @"Sistema de Constancias de No Inhabilitacion");
            }
            CerrarNodo();
        }
        
        private void AgregarDatosDelArchivo()
        {
            AgregarNodo(@"Archivos");
            {
                AgregarNodo(@"Archivo");
                AgregarAtributo(@"Nombre", string.Empty);
                AgregarAtributo(@"MimeType", string.Empty);
                {
                    AgregarNodo(@"Hash");
                    {
                        AgregarAtributo(@"type", @"SHA1");
                        AgregarAtributo(@"value", _entradaXml.Digestion);
                    }
                    CerrarNodo();
                    AgregarFirmantes();
                }
                CerrarNodo();
            }
            CerrarNodo();
        }
        
        private void AgregarFirmantes()
        {
            AgregarNodo(@"Firmantes");
            {
                AgregarDatosDelFirmante(_entradaXml.Pkcs7, _entradaXml.Fecha.ToString(@"yyyy-MM-ddTH:mm:ss"));
            }
            CerrarNodo();
        }
        
        private void AgregarDatosDelFirmante(IReadOnlyList<string> pkcs7Decodificado, string fechaFirma)
        {
            AgregarNodo(@"Firmante");
            {
                AgregarElemento(@"Nombre", RemoverDiacriticos(pkcs7Decodificado[3]));
                AgregarElemento(@"NumeroSerie", pkcs7Decodificado[2]);
                AgregarElemento(@"CertificadoValue", pkcs7Decodificado[1]);
                AgregarFirma(fechaFirma, pkcs7Decodificado[0]);
            }
            CerrarNodo();
        }
        
        private void AgregarFirma(string fechaFirma, string valorFirma)
        {
            AgregarNodo(@"Sign");
            {
                AgregarElemento(@"Fecha", fechaFirma);
                AgregarElemento(@"Value", valorFirma);
            }
            CerrarNodo();
        }
        #endregion

        #region XmlTextWriter
        /// <summary>
        /// Inicializa el xml para su manipulacion.
        /// </summary>
        private void InicializarXml()
        {
            _rutaArchivoXml = Path.GetTempPath() + Guid.NewGuid().ToString("N") + @".xml";

            _archivoXml = new XmlTextWriter(_rutaArchivoXml, new UTF8Encoding(false))
            {
                Formatting = Formatting.Indented
            };

            _archivoXml.WriteStartDocument();
        }

        /// <summary>
        /// Agrega un nodo al xml.
        /// </summary>
        /// <param name="nombreNodo">Nombre del nodo.</param>
        private void AgregarNodo(string nombreNodo)
        {
            _archivoXml.WriteStartElement(nombreNodo);
        }

        /// <summary>
        /// Agrega un valor al ultimo nodo agregado.
        /// </summary>
        /// <param name="valor">Valor a agregar.</param>
        private void AgregarValor(string valor)
        {
            _archivoXml.WriteString(valor);
        }

        /// <summary>
        /// Agrega un elemento al nodo actual.
        /// </summary>
        /// <param name="nombreElemento">Nombre del elemento.</param>
        /// <param name="valorElemento">Valor del elemento.</param>
        private void AgregarElemento(string nombreElemento, string valorElemento)
        {
            _archivoXml.WriteElementString(nombreElemento, valorElemento);
        }

        /// <summary>
        /// Agrega un atributo al ultimo nodo agregado.
        /// </summary>
        /// <param name="nombreAtributo">Nombre del atributo.</param>
        /// <param name="valorAtributo">Valor del atributo.</param>
        private void AgregarAtributo(string nombreAtributo, string valorAtributo)
        {
            _archivoXml.WriteAttributeString(nombreAtributo, valorAtributo);
        }

        /// <summary>
        /// Cierra el ultimo nodo agregado.
        /// </summary>
        private void CerrarNodo()
        {
            _archivoXml.WriteEndElement();
        }

        /// <summary>
        /// Cierra el xml.
        /// </summary>
        private void CerrarXml()
        {
            _archivoXml.WriteEndDocument();
            _archivoXml.Close();
        }
        #endregion

        public static string RemoverDiacriticos(string str)
        {
            var strFormD = str.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            
            foreach (var t in strFormD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
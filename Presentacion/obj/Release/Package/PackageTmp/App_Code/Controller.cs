using System;
using Datos;
using Org.BouncyCastle.Crypto.Tls;
//PROD
using Presentacion.WebServiceProd;
//QA
//using Presentacion.WebService;

namespace Presentacion
{

    /// <summary>
    /// Descripción breve de Controller
    /// </summary>
    public class Controller
    {
        public WebServiceSoapClient Cliente1
        {
            set;
            get;
        }
        public WebServiceSoapClient Cliente
        {
            set;
            get;
        }
        public AuthSoap Autenticacion
        {
            set;
            get;
        }
        public string Operador
        {
            set;
            get;
        }
        public string Certificado
        {
            set;
            get;
        }
        public string CadenaOriginal
        {
            set;
            get;
        }
        public string Digestion
        {
            set;
            get;
        }
        public string Fecha
        {
            set;
            get;

        }
        public string Nom
        {
            set;
            get;
        }
        public string Tsa
        {
            set;
            get;
        }
        public string Firma
        {
            set;
            get;
        }
        public string Vector
        {
            set;
            get;
        }
        public string CadenaBase64
        {
            set;
            get;
        }
        public int Codificacion
        {
            set;
            get;
        }
        private string referencia;

        public string Referencia
        {
            get { return referencia; }
            set { referencia = value; }
        }

        public static string Rc2Decrypt(string encryptedDataB64)
        {
           
            try
            {
                byte[] key = new byte[] { 9, 10, 12, 20, 34, 45, 55 };
                byte[] dataEncrypt = System.Convert.FromBase64String(encryptedDataB64);
                byte[] iv = { 0x30, 0x08, 0x06, 0x05, 0x01, 0x07, 0x05, 0x00 };
                System.Security.Cryptography.RC2CryptoServiceProvider rc2 = new System.Security.Cryptography.RC2CryptoServiceProvider();
                System.Security.Cryptography.ICryptoTransform rc2Decrypt = rc2.CreateDecryptor(key, iv);
                byte[] binData = rc2Decrypt.TransformFinalBlock(dataEncrypt, 00, dataEncrypt.Length);
                return System.Text.Encoding.UTF8.GetString(binData);
            }
            catch { return null; }
        }
        public Controller()
        {
            /*
            string entidad = (Controller.Rc2Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings["entidad"]));
            string usuario = (Controller.Rc2Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings["usuario"]));
            string password = (Controller.Rc2Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings["clave"]));
            this.Tsa = (Controller.Rc2Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings["tsaid"]));
            this.Nom = (Controller.Rc2Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings["nom"]));
            */

            string entidad = (System.Web.Configuration.WebConfigurationManager.AppSettings["entidad"]);
            string usuario = ((System.Web.Configuration.WebConfigurationManager.AppSettings["usuario"]));
            string password = ((System.Web.Configuration.WebConfigurationManager.AppSettings["clave"]));
            this.Tsa = ((System.Web.Configuration.WebConfigurationManager.AppSettings["tsaid"]));
            this.Nom = ((System.Web.Configuration.WebConfigurationManager.AppSettings["nom"]));


            Cliente = new WebServiceSoapClient();
            Autenticacion = new AuthSoap();
            Autenticacion.Entity = entidad;
            Autenticacion.User = usuario;
            Autenticacion.Password = password;
        }
        public string getDerResult()
        {
            string result = "";
            try
            {
                var respuesta = Cliente.WSReportDigest(this.Autenticacion, this.Digestion, this.Fecha, this.Referencia);
                int estado = 0;
                string descripcion = "";
                if (respuesta.State == 0)
                {
                    estado = respuesta.State;
                    descripcion = respuesta.Description;
                    result = "{\"state\":\"" + estado + "\",\"description\":\"" + descripcion + "\",\"transfer\":\"" + respuesta.Id + "\",\"date\":\"" + respuesta.StrNow + "\",\"vector\":\"" + respuesta.StrVector + "\"}";
                }
                else
                {
                    result = "{\"state\":\"" + estado + "\",\"description\":\"" + descripcion + "\"}";
                }
            }
            catch (Exception e)
            {
                result = "{\"state\":\"" + -98 + "\",\"description\":\"" + e.Message + "\"}";
            }
            return result;
        }
        public string getCertificateDetails(string certificate, bool ocsp)
        {
            string result = "";
            try
            {
                var properties = Cliente.WSDecodeCertificate(this.Autenticacion, ocsp == true ? "0" : "1", this.Referencia, this.Certificado, this.Tsa);
                result = "{\"state\":\"" + properties.State + "\",\"description\":\"" + properties.Description + "\",\"hexSerie\":\"" + properties.HexSerie + "\",\"notBefore\":\"" + properties.StrBegin + "\",\"notAfter\":\"" + properties.StrEnd + "\",\"subjectName\":\"" + properties.SubjectCn + "\",\"subjectEmail\":\"" + properties.SubjectEmail + "\",\"subjectOrganization\":\"" + properties.SubjectOrganization + "\",\"subjectDepartament\":\"" + properties.SubjectDepartament + "\",\"subjectState\":\"" + properties.SubjectState + "\",\"subjectCountry\":\"" + properties.SubjectCountry + "\",\"subjectRFC\":\"" + properties.SubjectRFC + "\",\"subjectCURP\":\"" + properties.SubjectCurp + "\",\"issuerName\":\"" + properties.IssuerCn + "\",\"issuerEmail\":\"" + properties.IssuerEmail + "\",\"issuerOrganization\":\"" + properties.IssuerOrganization + "\",\"issuerDepartament\":\"" + properties.IssuerDepartament + "\",\"issuerState\":\"" + properties.IssuerState + "\",\"issuerCountry\":\"" + properties.IssuerCountry + "\",\"issuerRFC\":\"" + properties.IssuerRFC + "\",\"issuerCURP\":\"" + properties.IssuerCurp + "\",\"publicKey\":\"" + properties.PublicKey + "\",\"fingerPrint\":\"" + properties.FingerPrint + "\",\"transfer\":\"" + properties.Id + "\",\"date\":\"" + properties.StrDate + "\",\"evidence\":\"" + properties.Acuse + "\"}";
            }
            catch (Exception e)
            {
                result = "{\"state\":\"" + -99 + "\",\"description\":\"" + e.Message + "\"}";
            }
            return result;
        }
        public string validaCadena()
        {
            string result = "";
            try
            {
                var resultadoExtendida = Cliente.WSReportPkcs1(this.Autenticacion, this.CadenaOriginal, this.Codificacion, this.Firma, this.Certificado, this.Referencia, this.Tsa, this.Nom);
                result = "{\"state\":\"" + resultadoExtendida.State + "\",\"description\":\"" + resultadoExtendida.Description + "\",\"transfer\":\"" + resultadoExtendida.Id + "\",\"date\":\"" + resultadoExtendida.Now + "\",\"evidence\":\"" + resultadoExtendida.Evidence + "\",\"commonName\":\"" + resultadoExtendida.Cn + "\",\"hexSerie\":\"" + resultadoExtendida.HexSerie + "\"}";
            }
            catch (Exception e)
            {
                result = "{\"state\":\"" + -99 + "\",\"description\":\"" + e.Message + "\"}";
            }
            return result;
        }
        public string firmaExtendida(long id)
        {
            string result = "";
            try
            {
                var resultadoExtendida = Cliente.WSReportPkcs7(this.Autenticacion, id, this.Firma, this.Certificado, this.Referencia, this.Tsa, this.Nom);
                result = "{\"state\":\"" + resultadoExtendida.State + "\",\"description\":\"" + resultadoExtendida.Description + "\",\"transfer\":\"" + resultadoExtendida.Id + "\",\"date\":\"" + resultadoExtendida.Now + "\",\"evidence\":\"" + resultadoExtendida.Evidence + "\",\"commonName\":\"" + resultadoExtendida.Cn + "\",\"hexSerie\":\"" + resultadoExtendida.HexSerie + "\"}";
            }
            catch (Exception e)
            {
                result = "{\"state\":\"" + -99 + "\",\"description\":\"" + e.Message + "\"}";
            }

            return result;
        }
        public string encodeError(string detalles)
        {
            return "{\"state\":\"-95\",\"description\":\"" + detalles + "\"}";
        }
    }
}
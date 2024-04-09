<%@ WebHandler Language="C#" Class="Controlador" %>

using System;
using System.Web;

public class Controlador : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.ContentEncoding = System.Text.Encoding.UTF8;
        if (request.HttpMethod == "POST")
        {
            string method = request.Params["metodo"];
            Controller controller = new Controller();
            controller.Referencia = request.Params["referencia"];
            controller.Tsa=request.Params["tsaName"];
            if (string.IsNullOrEmpty(controller.Tsa)||controller.Tsa=="undefined") {
                controller.Tsa = "NA";
            }

            string tsaAlgorithm = request.Params["tsaAlgorithm"];
            controller.Nom=request.Params["nomName"];
            if (string.IsNullOrEmpty(controller.Nom) || controller.Nom == "undefined")
            {
                controller.Nom = "NA";
            }
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            switch (method)
            {
                case "der":
                    if (string.IsNullOrEmpty(request.Params["digest"]) || string.IsNullOrEmpty(request.Params["fecha"]))
                    {
                        response.Write(controller.encodeError("Parámetros de petición incompletos"));
                    }
                    else
                    {
                        controller.Digestion = request.Params["digest"].Replace(" ", "+");
                        controller.Fecha = request.Params["fecha"];
                        if (string.IsNullOrEmpty(controller.Referencia))
                        {
                            controller.Referencia = "Generación token de firma";
                        }
                        response.Write(controller.getDerResult());

                    }
                    break;
                case "decodecert":
                    string cert = request.Params["cert"];
                    string ocsp = request.Params["ocsp"];
                    if (ocsp == null)
                    {
                        ocsp = "false";
                    }
                    if (!string.IsNullOrEmpty(cert))
                    {
                        cert = cert.Replace(" ", "+");
                        bool realizarOcsp = true;
                        if (ocsp == "false")
                        {
                            realizarOcsp = false;
                        }
                        if (string.IsNullOrEmpty(controller.Referencia))
                        {
                            controller.Referencia = "Decodificación Certificado";
                        }

                        controller.Certificado = cert;

                        response.Write(controller.getCertificateDetails(cert, realizarOcsp));
                    }
                    else
                    {
                        response.Write(controller.encodeError("Parámetros de petición incompletos"));
                    }

                    break;
                case "pkcs1":
                    string cadenaOriginal = request.Params["original"];
                    if (!string.IsNullOrEmpty(cadenaOriginal))
                    {
                        cadenaOriginal = cadenaOriginal.Replace(" ", "+");
                    }
                    string firma = request.Params["firma"];
                    string certificado = request.Params["cert"];
                    string evidence = request.Params["evidence"];
                    if (string.IsNullOrEmpty(cadenaOriginal) || string.IsNullOrEmpty(firma) || string.IsNullOrEmpty(certificado))
                    {
                        response.Write(controller.encodeError("Parámetros de petición incompletos"));
                    }
                    else
                    {


                        controller.Firma = firma.Replace(" ", "+");
                        controller.Certificado = certificado.Replace(" ", "+");
                        controller.Codificacion = 3;
                        controller.CadenaOriginal = cadenaOriginal;
                        if (string.IsNullOrEmpty(controller.Referencia))
                        {
                            controller.Referencia = "Firma PKCS1";
                        }
                        response.Write(controller.validaCadena());

                    }
                    break;
                case "vector":
                    string vector = request.Params["vector"];
                    string firmaVector = request.Params["firma"];
                    string certificadoBase64 = request.Params["cert"];
                    if (string.IsNullOrEmpty(vector) || string.IsNullOrEmpty(firmaVector) || string.IsNullOrEmpty(certificadoBase64) || string.IsNullOrEmpty("id"))
                    {
                        response.Write(controller.encodeError("Parámetros de petición incompletos"));
                    }
                    else
                    {
                        controller.Vector = vector.Replace(" ", "+");
                        controller.Firma = firmaVector.Replace(" ", "+");
                        controller.Certificado = certificadoBase64.Replace(" ", "+");
                        long id = -1;
                        bool isOk = long.TryParse(request.Params["id"].ToString(), out id);
                        if (isOk)
                        {
                            if (string.IsNullOrEmpty(controller.Referencia))
                            {
                                controller.Referencia = "Firma PKCS7";
                            }
                            response.Write(controller.firmaExtendida(id));
                        }
                        else
                        {
                            response.Write(controller.encodeError("Formato de transferencia no válido"));
                        }
                    }
                    break;
            }

        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
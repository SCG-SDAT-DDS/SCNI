using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Services;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios;
using Datos.Repositorios.Carta;
using Negocio.Firma;
using Negocio.Servicios;
using Presentacion.Controllers;
using Sistema.Extensiones;
using Sistema.Paginador;
using Sistema;
using static Datos.Enums.Enumerados;
using System;
using Datos.Enums;

namespace Presentacion.Areas.Carta.Controllers
{
    public class BuscarController : ControladorBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new CartaViewModel
            {
                Carta = new Datos.Carta
                {
                    Estado = EstadosCarta.NoFirmada
                },
                UrlDescargarArchivo = ObtenerUrlDescargar("Carta"),
                SerieCertificado = ObtenerSesion().SerieCertificado,
                NombreFirmante = ObtenerSesion().Nombre.ToTitleCase(),
                PuestoFirmante = ObtenerSesion().DescripcionPuesto.ToTitleCase(),
                IdEmpleado = ObtenerSesion().IdEmpleado
            };

            using (var bd = new Contexto())
            {
                var repositorio = new CartasRepositorio(bd);
                viewModel.Cartas = repositorio.Buscar(viewModel);
                viewModel.TotalEncontrados = repositorio.ObtenerTotalRegistros(viewModel);
                viewModel.Paginas = Paginar.ObtenerCantidadPaginas(viewModel.TotalEncontrados,
                    viewModel.TamanoPagina);
            }

            viewModel.SerieCertificado = ObtenerSesion().SerieCertificado;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(CartaViewModel viewModel, int? pagina = null)
        {
                viewModel.PaginaActual = pagina ?? 1;

            if (viewModel.Enviar == "Firmar" || viewModel.Rechazar == "Rechazar")
            {
                var sesion = ObtenerSesion();

                byte[] certificado;

                using (var bd = new Contexto())
                {
                    certificado = new UsuarioRepositorio(bd).ObtenerCertificado(sesion.IdUsuario);
                }


                var isCorrecto = true;
                var error = string.Empty;
                string folioDecodificando = null;

                var cantidadFirmas = viewModel.Pkcs7s.Count;

                for (var i = 0; i < cantidadFirmas; i++)
                {
                    if (string.IsNullOrEmpty(viewModel.Pkcs7s[i]))
                    {
                        viewModel.Folios.RemoveAt(i);
                        viewModel.Pkcs7s.RemoveAt(i);
                        viewModel.Fechas.RemoveAt(i);
                        viewModel.Digestiones.RemoveAt(i);

                        i--;
                        cantidadFirmas--;
                    }
                }

                var textoMensaje = string.Empty;

                try
                {
                    var firmas = new List<FirmaCarta>();
                    for (var i = 0; i < viewModel.IdCartasFirmar.Count; i++)
                    {
                        folioDecodificando = viewModel.Folios[i];

                        var carta = ConstructorFirmaCarta.FirmaCarta()
                            .AgregarIdCarta(viewModel.IdCartasFirmar[i])
                            .AgregarFolio(viewModel.Folios[i])
                            .AgregarPkcs7(viewModel.Pkcs7s[i])
                            .AgregarFecha(viewModel.Fechas[i])
                            .AgregarDigestion(viewModel.Digestiones[i])
                            .AgregarNombreFirmante(ObtenerSesion().Nombre.ToTitleCase())
                            .AgregarPuestoFirmante(ObtenerSesion().DescripcionPuesto.ToTitleCase())
                            .Build();

                        if (carta.Pkcs7.Length == 0)
                            throw new FielException("El archivo PKCS7 en Bytes[] no cumple con el tamaño mínimo requerido");

                        firmas.Add(carta);

                        folioDecodificando = null;
                    }

                    var serviciosCarta = new ServiciosCarta();

                    //Session[Enumerados.Pfx.UrlPfx.ToString()] = urlPfx;
                    //Session[Enumerados.Pfx.PasswordPfx.ToString()] = passwordPfx;
                    //byte[] pfx = System.IO.File.ReadAllBytes("c:\\tmp\\Usuario1.pfx");


                    //TFielLib.TPkcs1 pkcs1 = new TFielLib.TPkcs1();
                    //byte[] sign = pkcs1.Signature(pfx, "12121212", fiel.Pkcs7, TFielLib.TPkcs1.TDigest.Sha256);
                    //string strSign = System.Convert.ToBase64String(sign);
                    //System.Console.WriteLine("Firma: " + strSign);


                    if (viewModel.Enviar == "Firmar")
                    {
                        textoMensaje = "firmadas";
                        serviciosCarta.FirmarCartas(sesion, certificado, sesion.SerieCertificado, firmas);
                    }
                    else
                    {
                        textoMensaje = "rechazadas";
                        serviciosCarta.RechazarCartas(sesion, certificado, sesion.SerieCertificado, firmas);
                    }
                    
                }
                catch (FielException ex)
                {
                    isCorrecto = false;
                    error = string.IsNullOrEmpty(folioDecodificando) ? ex.Message :  folioDecodificando + "." + ex.Message;
                }

                EnviarAlerta("Cartas " + textoMensaje + " correctamente", error, isCorrecto);
            }

            if (viewModel.FechaFin.HasValue)
                viewModel.FechaFin = viewModel.FechaFin.Value.AddDays(1);

            using (var bd = new Contexto())
            {
                var repositorio = new CartasRepositorio(bd);
                viewModel.Cartas = repositorio.Buscar(viewModel);
                viewModel.TotalEncontrados = repositorio.ObtenerTotalRegistros(viewModel);
                viewModel.Paginas = Paginar.ObtenerCantidadPaginas(viewModel.TotalEncontrados,
                    viewModel.TamanoPagina);
            }

            ModelState.Clear();

            viewModel.UrlDescargarArchivo = ObtenerUrlDescargar("Carta");
            viewModel.IdEmpleado = ObtenerSession().IdEmpleado;
            viewModel.NombreFirmante = ObtenerSesion().Nombre.ToTitleCase();
            viewModel.PuestoFirmante = ObtenerSesion().DescripcionPuesto.ToTitleCase();
            viewModel.SerieCertificado = ObtenerSesion().SerieCertificado;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EnviarConstanciaPorCorreo(string idCarta, string correo)
        {
            using (var bd = new Contexto())
            {
                if (idCarta != null)
                {
                    var repositorio = new CartasRepositorio(bd);
                    var carta = repositorio.BuscarPorId(int.Parse(idCarta));

                    var serviciosCarta = new ServiciosCarta();
                    var archivoCarta = serviciosCarta.GenerarArchivoCarta(carta.IDCarta);

                    Correo.EnviarConstancia(correo, archivoCarta.Key, archivoCarta.Value);

                    EscribirMensaje("Correo enviado correctamente");

                    return RedirectToAction("Index", "Buscar");
                }
                else
                {
                    EscribirError("Error al enviar corrre");
                    return RedirectToAction("Index", "Buscar");
                }
            }
        }

        [WebMethod]
        [HttpPost]
        public JsonResult CambiarEstadoConsultable(int idCarta, bool esConsultable)
        {
            using (var bd = new Contexto())
            {
                var repositorio = new CartasRepositorio(bd);

                repositorio.CambiarEstadoConsultable(idCarta, esConsultable);

                bd.SaveChanges();

                return Json(new {correcto = true});
            }
        }
    }
}
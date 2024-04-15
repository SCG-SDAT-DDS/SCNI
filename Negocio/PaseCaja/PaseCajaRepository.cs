using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Datos.DTO.Infraestructura.ViewModels;
//using Negocio.mx.gob.sonora.claro.gespipci;
using Negocio.ConsultaPaseCajaQA;
using Negocio.Repositorios.PaseCaja.Format;
using Negocio.Servicios;
using Negocio.WsPagoRegistroCivil_Productivo;
//using Negocio.WsPaseCaja_Productivo;
using Negocio.WsPaseCaja_Calidad;
using Negocio.WsValidarPago_Calidad;
//using Negocio.WsValidarPago_Productivo;
//using Negocio.WsValidarPago_Calidad;

namespace Negocio.PaseCaja
{
    public class PaseCajaRepository : DT_EntradaPaseCaja
    {
        public string Folio { get; set; }
        private Datos.PaseCaja Data { get; set; }

        private static string WsUsuario()
        {
            //return "SAPPIENS";
            return "SAPPISCG";
        }

        private static string WsClave()
        {
            //return "sonora";
            //PRODD
            //return "<awYS<%CY9][]sfHB-ds$ZV$Dvrmn#-~Q&LcbZEG";
            //QA
            return "Nf(bBYv!/2Uc";
        }

        public PaseCajaRepository()
        {
            Inicializar();
        }

        public void Inicializar()
        {
            Data = new Datos.PaseCaja();
            Cabecera =
                new DT_Cabecera
                {
                    COD_BP = " ",
                    TP_CTA_CONTRATO = " ",
                    RFC = "",
                    OBSERVACIONES = " ",
                    COD_CTA_CONTRADO = " ",
                    COD_OBJ_CONTRATO = " ",
                    CLAVE_PERIODO = " ",
                    CLASE_OBJETO = " ",
                    NOMBRE_OPCIONAL_PC = " ",
                    DIRECCION_OPCIONAL_PC = " ",
                    CORREO_ELECTRONICO = " ",
                    FECHA_VENCIMIENTO = new DateTime(2000, 1, 1)
                };

            Declaracion =
                new DT_DatosDeclaracion[]
                {
                    new DT_DatosDeclaracion
                    { 
                        DES_CAMPO = " ",
                        IND_APLIC = " ",
                        NOM_CAMPO = " ",
                        VAL_CAMPO = " "
                    }
                };

            ISAN =
                new DT_TablaVehiculosISAN[]
                {
                    new DT_TablaVehiculosISAN
                    { 
                        MODELO = " ",
                        TIPO = " ",
                        UNIDADES = "0",
                        VALOR_ENAJENACION = decimal.Zero
                    }
                };

            ISRTP =
                new DT_ReferenciasISRTP[]
                {
                    new DT_ReferenciasISRTP
                    { 
                        BAS_GRAVABLE = decimal.Zero,
                        COD_CTA_CONTRATO = " ",
                        NOM_ORGANIZACION = " ",
                        NRO_EMPLEADOS = 0,
                        NRO_MES = 0
                    }
                };

            Vehicular =
                new DT_PartidasAbiertas[]
                {
                    new DT_PartidasAbiertas
                    { 
                        COD_CONCEPTP = " ",
                        COD_OP_PARCI = " ",
                        COD_OP_PRINC = " ",
                        DENOMINACION = " ",
                        DOC_CONTABLE = " ",
                        DOC_POS_1 = " ",
                        DOC_POS_2 = " ",
                        IMPORTE = decimal.Zero,
                        IND_BORRADO = " ",
                        PERIODO = " "
                    }
                };

            Generales =
                new DT_Generales
                {
                    Conceptos =
                        new DT_ZdettConceptos[]
                        {
                            new DT_ZdettConceptos
                            { 
                                CANTIDAD = 0,
                                COD_CONCEPTP = " ",
                                COD_OP_PARCI = " ",
                                COD_OP_PRINC = " ",
                                IMPORTE = decimal.Zero
                            }

                        },
                    Domicilio =
                        new DT_Domicilio()
                        { 
                            CALLE = " ",
                            COD_POSTAL = " ",
                            COLONIA = " ",
                            ENTRE_CALLES = " ",
                            ESTADO = " ",
                            LOCALIDAD = " ",
                            MUNICIPIO = " ",
                            NRO_EXT = " ",
                            NRO_INT = " ",
                            ZONA = " "
                        }
                };

            ApoyoSocial = new DT_ApoyoSocial[]
                {
                    new DT_ApoyoSocial
                    { 
                         DESCRIPCION = " ",
                         ID = " "
                    }
                };

            Rubros = new DT_Rubros[]
                {
                    new DT_Rubros
                    { 
                         FBNUM = " ",
                         ID_ESTIMULO = " ",
                         MONTO = " ",
                         PAGADO = " ",
                         PERIODO = " ",
                         PORCENTAJE = " ",
                         REFERENCIA = " ",
                         TIPO_DECL = " "
                    }
                };

            VIgenciaLicencia = new DT_VigenciaLicencia
            { 
                 VIG_LICEN = " "
            };
        }

        public int Generar(PaseCajaRequest data)
        {
            try
            {
                Data.NombrePersona = data.Nombre;
                Data.CorreoElectronico = data.Email;
                var dataPC = ObtenerConceptos();
                Cabecera.COD_BP = dataPC.COD_BP ?? " ";
                Cabecera.TP_CTA_CONTRATO = "33";
                Cabecera.COD_CTA_CONTRADO = dataPC.COD_CTA_CONTRATO;
                Cabecera.CLASE_OBJETO = "CI";
                Cabecera.COD_OBJ_CONTRATO = dataPC.COD_OBJ_CONTRATO;
                Cabecera.CORREO_ELECTRONICO = data.Email ?? " ";
                Cabecera.NOMBRE_OPCIONAL_PC = data.Nombre;

                var conceptos = new List<DT_ZdettConceptos>();
                var importePrincipal = dataPC.Conceptos.First(m => m.COD_OP_PRINC == "4319").IMPORTE;
                foreach (var item in dataPC.Conceptos)
                {
                    if (item.COD_OP_PRINC == "N450") continue;

                    var importe = item.COD_OP_PRINC.StartsWith("18")
                        ? RoundSap(importePrincipal * (item.IMPORTE / 100))
                        : item.IMPORTE;

                    conceptos.Add(new DT_ZdettConceptos
                    { 
                        CANTIDAD = 1,
                        COD_CONCEPTP = item.COD_CONCEPTP,
                        COD_OP_PRINC = item.COD_OP_PRINC,
                        COD_OP_PARCI = item.COD_OP_PARCI,
                        IMPORTE = importe
                    });
                }
                Generales.Conceptos = conceptos.ToArray();

                var paseCaja = Obtener();

                var dataSetPaseCaja = Formato(paseCaja);

                var pdfPaseCaja = Generar(dataSetPaseCaja);

                return pdfPaseCaja;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public decimal RoundSap(decimal number)
        {
            return Math.Ceiling(number) - number >= 0.50M ? Math.Floor(number) : Math.Ceiling(number);
        }

        public DT_PagoRegistroCivil ObtenerConceptos()
        {
            try
            {
                var parametros = new DT_EntradaCtaContrato
                {
                    TP_CTA_CONTRATO = "33",
                    COBRO = "CI"
                };
                using (var servicio = new SI_PagoRegistroCivil_outService
                {
                    Credentials = new System.Net.NetworkCredential(WsUsuario(), WsClave())
                })
                {
                    return servicio.SI_PagoRegistroCivil_out(parametros);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private DT_RespuestaPaseCaja Obtener()
        {
            try
            {
                var parametros = new DT_EntradaPaseCaja()
                {
                    Declaracion = this.Declaracion,
                    ISAN = this.ISAN,
                    ISRTP = this.ISRTP,
                    Cabecera = this.Cabecera,
                    Generales = this.Generales,
                    Vehicular = this.Vehicular,
                    ApoyoSocial = this.ApoyoSocial,
                    Rubros = this.Rubros,
                    VIgenciaLicencia = this.VIgenciaLicencia
                };

                using (var servicio = new SI_PaseCaja_outService
                {
                    Credentials = new System.Net.NetworkCredential(WsUsuario(), WsClave())
                })
                {
                    var respuesta = servicio.SI_PaseCaja_out(parametros);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public DataSet Formato(DT_RespuestaPaseCaja response)
        {
            Data.Folio = response.PASE_CAJA.FOLIO;
            Data.FechaRegistro = DateTime.ParseExact(response.PASE_CAJA.FECHA, "yyyyMMdd", null);
            Data.FechaVencimiento = DateTime.ParseExact(response.PASE_CAJA.FEC_VENCIMIENTO, "yyyyMMdd", null);

            var ds = new DataSet("pasecaja");
            var paseCaja = new DataTable("PASE_CAJA");
            paseCaja.Columns.Add("FOLIO");
            paseCaja.Columns.Add("FECHA");
            paseCaja.Columns.Add("FEC_VENCIMIENTO");
            paseCaja.Columns.Add("RFC");
            paseCaja.Columns.Add("NOMBRE");
            paseCaja.Columns.Add("DIRECCION");
            paseCaja.Columns.Add("TOTAL");
            paseCaja.Columns.Add("LETRA");
            paseCaja.Columns.Add("LINEA");
            paseCaja.Columns.Add("NOTA");
            paseCaja.Columns.Add("LEYENDA");
            paseCaja.Columns.Add("PADRON");
            paseCaja.Rows.Add(
                response.PASE_CAJA.FOLIO,
                Data.FechaRegistro.ToString("dd/MM/yyyy"),
                Data.FechaVencimiento.ToString("dd/MM/yyyy"),
                response.PASE_CAJA.RFC,
                response.PASE_CAJA.NOMBRE,
                response.PASE_CAJA.DIRECCION,
                string.Format(new System.Globalization.CultureInfo("es-MX"), "{0:C}", response.PASE_CAJA.TOTAL),
                response.PASE_CAJA.IMPORTE_LETRA,
                response.PASE_CAJA.LINEA_CAPOXXO,
                response.PASE_CAJA.NOTA,
                response.PASE_CAJA.LEYENDA,
                response.PASE_CAJA.DATOS_PADRON);
            ds.Tables.Add(paseCaja);

            var items = new DataTable("ITEMS_PASE_CAJA");
            items.Columns.Add("NRO_POSICION");
            items.Columns.Add("ANIO");
            items.Columns.Add("PERIODO");
            items.Columns.Add("CONCEPTO");
            items.Columns.Add("DESCRIPCION");
            items.Columns.Add("BASE");
            items.Columns.Add("CANTIDAD");
            items.Columns.Add("IMPORTE");
            foreach (var item in response.ITEMS_PASE_CAJA)
            {
                items.Rows.Add(item.NRO_POSICION, item.ANIO,
                    item.PERIODO, item.CONCEPTO, item.DESCRIPCION,
                    item.BASE, item.CANTIDAD.TrimStart('0'),
                    string.Format(new System.Globalization.CultureInfo("es-MX"), "{0:C}", item.IMPORTE));
            }
            ds.Tables.Add(items);

            var bancos = new DataTable("CONVENIOS_BANCOS");
            bancos.Columns.Add("ID_BANCO");
            bancos.Columns.Add("CONVENIO");
            foreach (var item in response.CONVENIOS_BANCOS)
            {
                bancos.Rows.Add(item.ID_BANCO, item.CONVENIO);
            }
            ds.Tables.Add(bancos);
            return ds;
        }

        public int Generar(DataSet info)
        {
            var output = DocumentoFactory.Generar(DocumentoEnum.PaseCaja, info, true);
            byte[] datos = output.ToArray();
            MemoryStream mso = new MemoryStream(datos);
            byte[] byteInfo = mso.ToArray();
            mso.Write(byteInfo, 0, byteInfo.Length);
            mso.Position = 0;
            var service = new ServiciosPaseCaja();
            Folio = Data.Folio;
            return service.Guardar(Data, mso.ToArray());
        }

        public WsValidarPago_Calidad.DT_Consulta_resp ValidarPago(string folio)
        {
            var paseCaja = string.Empty;
            var recibo = string.Empty;

            if (folio.Length == 15)
            {
                paseCaja = folio;
            }
            else
            {
                recibo = folio;
            }

            
            using (var ws = new WsValidarPago_Calidad.SI_ConsultaRo_outService
            {
                Credentials = new System.Net.NetworkCredential(WsUsuario(), WsClave())
            })
            {
                var resultado = ws.SI_ConsultaRo_out(new WsValidarPago_Calidad.DT_Consulta_req { PASEC = paseCaja, RECIBO = recibo });

                return resultado;
            }


            //using (var ws = new SI_ConsultaRo_outClient())
            //{
            //    var resultado = ws.SI_ConsultaRo_out(new WS_CONSULTARO_QA.DT_Consulta_req { PASEC = paseCaja, RECIBO = recibo });
            //    return resultado;
            //}
        }
    }
}
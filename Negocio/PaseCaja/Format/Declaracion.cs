using System;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Negocio.Repositorios.PaseCaja.Format
{
    public sealed class Declaracion : Documento, IDocumento
    {
        readonly Font _xLarge = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD);
        readonly Font _large = new Font(Font.FontFamily.HELVETICA, 9f);
        readonly Font _largeBold = new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD);
        readonly Font _medium = new Font(Font.FontFamily.HELVETICA, 8f);
        readonly Font _mediumBold = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
        readonly Font _small = new Font(Font.FontFamily.HELVETICA, 7f);
        Image _gif;
        PdfPTable _taux;

        public bool DefinirDoc()
        {
            Doc = new Document(PageSize.A4, MargenIzq, MargenDer, MargenArriba, MargenAbajo);
            Resultado = new MemoryStream();
            Writer = PdfWriter.GetInstance(Doc, Resultado);
            Doc.Open();
            Doc.NewPage();
            return true;
        }

        private Image ObtenerImagen(String imagen, float ancho, float alto)
        {
            var bytes = Convert.FromBase64String(imagen);
            _gif = Image.GetInstance(bytes);
            _gif.Alignment = Element.ALIGN_CENTER;
            _gif.Border = Rectangle.NO_BORDER;
            _gif.BorderColor = BaseColor.WHITE;
            _gif.ScaleToFit(ancho, alto);
            return _gif;
        }

        public bool Encabezado()
        {
            var tEnc = new PdfPTable(4) { WidthPercentage = 100, SplitLate = false };
            tEnc.DefaultCell.Border = Rectangle.NO_BORDER;
            tEnc.DefaultCell.BorderColor = BaseColor.WHITE;
            tEnc.HorizontalAlignment = Element.ALIGN_CENTER;

            #region Logo
            _gif = ObtenerImagen(LogoSonora, 80f, 85f);
            var tCellAux = new PdfPCell { PaddingLeft = 30f, HorizontalAlignment = Element.ALIGN_CENTER, Border = Rectangle.NO_BORDER };
            tCellAux.AddElement(_gif);
            tEnc.AddCell(tCellAux);
            #endregion

            #region Dependencia
            var pEnc = new Paragraph { Alignment = Element.ALIGN_CENTER };
            pEnc.SetLeading(0.0f, 1.5f);
            pEnc.Add(new Chunk("GOBIERNO DEL ESTADO DE\n SONORA\n", _xLarge));
            pEnc.Add(new Chunk("SECRETARÍA DE HACIENDA\n", _medium));
            pEnc.Add(new Chunk("SUBSECRETARÍA DE INGRESOS", _medium));
            #endregion

            #region Se agrega Logo y Dependencia al documento
            tCellAux = new PdfPCell { HorizontalAlignment = Element.ALIGN_CENTER, Border = Rectangle.NO_BORDER, Colspan = 2 };
            tCellAux.AddElement(pEnc);
            tEnc.AddCell(tCellAux);

            tEnc.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

            Doc.Add(tEnc);
            Doc.Add(new Paragraph("\n", _largeBold));
            #endregion

            return true;
        }

        public bool Detalle()
        {
            #region Folio y Fechas

            var paseCaja = InfoDoc.Tables["PASE_CAJA"];
            var paseCajaInfo = paseCaja.Rows[0];

            var tDet = new PdfPTable(1);
            tDet.DefaultCell.Padding = 0f;
            tDet.WidthPercentage = 100;
            tDet.SplitLate = false;

            _taux = new PdfPTable(3);
            _taux.DefaultCell.Padding = 0f;
            _taux.DefaultCell.Border = Rectangle.NO_BORDER;
            _taux.WidthPercentage = 100;
            _taux.SetWidths(new[] { 0.25f, 0.35f, 0.4f });

            var celda = new PdfPCell { Padding = 2f, PaddingLeft= 6f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("FOLIO:", _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f,  Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["FOLIO"].ToString(), _large));
            _taux.AddCell(celda);

            var folioCodigoBarras = new Barcode128
            {
                CodeType = Barcode.CODE128,
                ChecksumText = true,
                GenerateChecksum = true,
                Code = paseCajaInfo["Folio"].ToString()
            };
            var imgFolioCodigoBarras = folioCodigoBarras.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
            var itextImgFolioCodigoBarras = Image.GetInstance(imgFolioCodigoBarras, System.Drawing.Imaging.ImageFormat.Jpeg);
            itextImgFolioCodigoBarras.ScaleAbsolute(100f, 45f);
            itextImgFolioCodigoBarras.Alignment = Image.ALIGN_CENTER;
            celda = new PdfPCell(itextImgFolioCodigoBarras)
            {
                Padding = 2f,
                PaddingLeft = 6f,
                PaddingTop = 5f,
                Border = Rectangle.NO_BORDER,
                Rowspan = 3
            };
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("FECHA:", _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["FECHA"].ToString(), _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, PaddingBottom=5f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("FECHA DE VENCIMIENTO:", _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["FEC_VENCIMIENTO"].ToString(), _large));
            _taux.AddCell(celda);

            tDet.AddCell(_taux);
            #endregion

            #region InfoPersonal

            _taux = new PdfPTable(2);
            _taux.DefaultCell.Border = Rectangle.NO_BORDER;
            _taux.DefaultCell.Padding = 0f;
            _taux.WidthPercentage = 100;
            _taux.SetWidths(new[] { 0.25f, 0.75f });
            
            celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("NOMBRE:", _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["NOMBRE"].ToString(), _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("DIRECCIÓN:", _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["DIRECCION"].ToString(), _large));
            _taux.AddCell(celda);

            celda = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER };
            celda.AddElement(_taux);
            tDet.AddCell(celda);

            #endregion

            //#region Padron

            //_taux = new PdfPTable(2);
            //_taux.DefaultCell.Border = Rectangle.NO_BORDER;
            //_taux.DefaultCell.Padding = 0f;
            //_taux.WidthPercentage = 100;
            //_taux.SetWidths(new[] { 0.25f, 0.75f });

            //celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            //celda.AddElement(new Paragraph("DATOS: ", _large));
            //_taux.AddCell(celda);

            //celda = new PdfPCell { Padding = 2f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            //celda.AddElement(new Paragraph(paseCajaInfo["PADRON"].ToString(), _large));
            //_taux.AddCell(celda);

            //celda = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER };
            //celda.AddElement(_taux);
            //tDet.AddCell(celda);

            //#endregion

            #region ConceptoTitulo

            _taux = new PdfPTable(7);
            _taux.DefaultCell.Border = Rectangle.NO_BORDER;
            _taux.DefaultCell.Padding = 0f;
            _taux.WidthPercentage = 100;
            _taux.SetWidths(new[] { 0.05f, 0.05f, 0.12f, 0.50f, 0.08f, 0.08f, 0.12f });

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("AÑO", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("PER", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("CONCEPTO", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("DESCRIPCIÓN", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("BASE", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("CANT", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("IMPORTE", _large) { Alignment = Element.ALIGN_RIGHT });
            _taux.AddCell(celda);

            celda = new PdfPCell { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER };
            celda.AddElement(_taux);
            tDet.AddCell(celda);

            #endregion

            #region Concepto Detalle

            _taux = new PdfPTable(7);
            _taux.DefaultCell.Border = Rectangle.NO_BORDER;
            _taux.DefaultCell.Padding = 0f;
            _taux.WidthPercentage = 100;
            _taux.SetWidths(new[] { 0.05f, 0.05f, 0.12f, 0.50f, 0.08f, 0.08f, 0.12f });

            var itemsPaseCaja = InfoDoc.Tables["ITEMS_PASE_CAJA"];

            foreach (DataRow item in itemsPaseCaja.Rows)
            {
                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["ANIO"].ToString(), _large) { Alignment = Element.ALIGN_CENTER });
                _taux.AddCell(celda);

                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["PERIODO"].ToString(), _large) { Alignment = Element.ALIGN_CENTER });
                _taux.AddCell(celda);

                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["CONCEPTO"].ToString(), _large) { Alignment = Element.ALIGN_CENTER });
                _taux.AddCell(celda);

                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["DESCRIPCION"].ToString(), _large) { Alignment = Element.ALIGN_LEFT });
                _taux.AddCell(celda);

                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["BASE"].ToString(), _large) { Alignment = Element.ALIGN_CENTER });
                _taux.AddCell(celda);
                
                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["CANTIDAD"].ToString(), _large) { Alignment = Element.ALIGN_CENTER });
                _taux.AddCell(celda);

                celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
                celda.AddElement(new Paragraph(item["IMPORTE"].ToString(), _large) { Alignment = Element.ALIGN_RIGHT });
                _taux.AddCell(celda);
            }

            celda = new PdfPCell { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, MinimumHeight = 130f };
            celda.AddElement(_taux);
            tDet.AddCell(celda);

            #endregion

            #region Total

            _taux = new PdfPTable(7);
            _taux.DefaultCell.Border = Rectangle.NO_BORDER;
            _taux.DefaultCell.Padding = 0f;
            _taux.DefaultCell.PaddingLeft = 40f;
            _taux.DefaultCell.PaddingRight = 40f;
            _taux.WidthPercentage = 100;
            _taux.SetWidths(new[] { 0.05f, 0.05f, 0.12f, 0.50f, 0.08f, 0.08f, 0.12f });

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            _taux.AddCell(celda);
            _taux.AddCell(celda);
            _taux.AddCell(celda);
            _taux.AddCell(celda);
            celda = new PdfPCell { Colspan= 2, Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("TOTAL:", _large) { Alignment = Element.ALIGN_RIGHT });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["TOTAL"].ToString(), _large) { Alignment = Element.ALIGN_RIGHT });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, Border = Rectangle.NO_BORDER, Colspan = 7 };
            celda.AddElement(new Paragraph(paseCajaInfo["LETRA"] + "\n\n", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER };
            celda.AddElement(_taux);
            tDet.AddCell(celda);

            #endregion

            #region Linea Captura

            _taux = new PdfPTable(1);
            _taux.DefaultCell.Border = Rectangle.NO_BORDER;
            _taux.DefaultCell.Padding = 0f;
            _taux.WidthPercentage = 100;
            _taux.SetWidths(new[] { 1f });

            var codigoBarras = new Barcode128
            {
                CodeType = Barcode.CODE128,
                ChecksumText = true,
                GenerateChecksum = true,
                Code = paseCajaInfo["LINEA"].ToString()
            };

            celda = new PdfPCell
            {
                Padding = 2f,
                PaddingLeft = 6f,
                PaddingBottom = 5f,
                Border = Rectangle.NO_BORDER
            };
            var imgCodigoBarras = codigoBarras.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
            var itextImgCodigoBarras = Image.GetInstance(imgCodigoBarras, System.Drawing.Imaging.ImageFormat.Jpeg);
            itextImgCodigoBarras.ScaleToFit(200f, 50f);
            itextImgCodigoBarras.Alignment = Image.ALIGN_CENTER;
            celda.AddElement(itextImgCodigoBarras);
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph(paseCajaInfo["LINEA"].ToString(), _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Padding = 2f, PaddingLeft = 6f, PaddingBottom = 5f, Border = Rectangle.NO_BORDER };
            celda.AddElement(new Paragraph("Comercios Autorizados: Oxxo, Abarrey, Super del Norte, Benavides, Telecomm.", _large) { Alignment = Element.ALIGN_CENTER });
            _taux.AddCell(celda);

            celda = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER };
            celda.AddElement(_taux);
            tDet.AddCell(celda);

            #endregion 

            //#region Bancos

            //var conveniosBancos = InfoDoc.Tables["CONVENIOS_BANCOS"];

            //_taux = new PdfPTable(2);
            //_taux.DefaultCell.Padding = 0f;
            //_taux.DefaultCell.Border = Rectangle.NO_BORDER;
            //_taux.WidthPercentage = 100;
            //_taux.SetWidths(new[] { 0.2f, 0.8f });

            //foreach (DataRow item in conveniosBancos.Rows)
            //{
            //    if (!string.IsNullOrEmpty(item["ID_BANCO"].ToString()))
            //    {
            //        celda = new PdfPCell { Border = Rectangle.RIGHT_BORDER };
            //        celda.AddElement(ObtenerImagen(BancoImagen(item["ID_BANCO"].ToString()), 70f, 70f));
            //        _taux.AddCell(celda);
            //    }
            //    else { _taux.AddCell(new PdfPCell()); }

            //    celda = new PdfPCell { PaddingLeft = 10f, Border = Rectangle.NO_BORDER };
            //    celda.AddElement(new Paragraph(item["CONVENIO"].ToString(), _large));
            //    _taux.AddCell(celda);
            //}

            //celda = new PdfPCell { Padding = 0f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER};
            //celda.AddElement(_taux);
            //tDet.AddCell(celda);

            //#endregion

            //#region Nota

            //celda = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER, Padding = 5f, PaddingLeft = 25f, PaddingRight = 25f };
            //celda.AddElement(new Paragraph(paseCajaInfo["NOTA"].ToString(), _large) { Alignment = Element.ALIGN_JUSTIFIED });
            //tDet.AddCell(celda);

            //#endregion

            #region Leyenda

            celda = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER, Padding = 5f, PaddingBottom = 10f, PaddingLeft = 25f, PaddingRight = 25f };
            celda.AddElement(new Paragraph(paseCajaInfo["LEYENDA"].ToString(), _large) { Alignment = Element.ALIGN_JUSTIFIED });
            tDet.AddCell(celda);

            #endregion

            Doc.Add(tDet);
            return true;
        }

        public bool Pie()
        {
            return true;
        }

        public MemoryStream Generar(DataSet info)
        {
            InfoDoc = info;
            DefinirDoc();
            Encabezado();
            Detalle();
            Pie();
            Doc.Close();
            return Resultado;
        }

        public override MemoryStream GeneraEx(Exception ex)
        {
            try
            {
                var document = new Document(PageSize.A4, MargenIzq, MargenDer, MargenArriba, MargenAbajo);
                var output = new MemoryStream();
                PdfWriter.GetInstance(document, output);
                document.Open();

                document.Add(MostrarEx
                                 ? new Paragraph(ex.Message, new Font(Font.FontFamily.HELVETICA, 13f))
                                 : new Paragraph(
                                       "Estimado usuario, por el momento no se puede mostrar el documento soilcitado.",
                                       new Font(Font.FontFamily.HELVETICA, 13f)));

                document.Close();
                return output;
            }
            catch (Exception)
            { return null; }
        }

        public string BancoImagen(string bancoId)
        {
            switch (bancoId)
            {
                case "B01":
                    return LogoBancomer;
                case "B02":
                    return LogoBanamex;
                case "B03":
                    return LogoSantander;
                case "B04":
                    return LogoBanorte;
                case "B05":
                    return LogoHsbc;
                default:
                    return LogoOxxo;
            }
        }
    }
}
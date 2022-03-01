using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace Hub3a {
    //Generira PDF417 2D BAR-KOD prema HUB3a standardu.

    public class Hub3a {
        int ERROR_CORRECTION_LEVEL = 2;
        #region props
        /// <summary>
        /// Max length 8
        /// </summary>
        public string Zaglavlje { get; } = "HRVHUB30";
        /// <summary>
        /// Max length 3
        /// </summary>
        public string ValutaPlacanja { get; } = "HRK";
        /// <summary>
        /// Max length 15
        /// </summary>
        public string Iznos { get; } = "000000000000000";
        /// <summary>
        /// Max length 30
        /// </summary>
        public string PlatiteljPrvaLinija { get; } = "PLATITELJ";
        /// <summary>
        /// Max length 27
        /// </summary>
        public string PlatiteljDrugaLinija { get; } = "PLATITELJ ADRESA";
        /// <summary>
        /// Max length 27
        /// </summary>
        public string PlatiteljTrecaLinija { get; } = "PLATITELJ MJESTO";
        /// <summary>
        /// Max length 25
        /// </summary>
        public string PrimateljPrvaLinija { get; } = "PRIMATELJ";
        /// <summary>
        /// Max length 25
        /// </summary>
        public string PrimateljDrugaLinija { get; } = "PRIMATELJ ADRESA";
        /// <summary>
        /// Max length 27
        /// </summary>
        public string PrimateljTrecaLinija { get; } = "PRIMATELJ MJESTO";
        /// <summary>
        /// Max length 21
        /// </summary>
        public string PrimateljIBAN { get; } = "HR0000000000000000000";
        /// <summary>
        /// Max length 4
        /// </summary>
        public string Model { get; } = "HR99";
        /// <summary>
        /// Max length 22
        /// </summary>
        public string PozivNaBroj { get; } = "000000";
        /// <summary>
        /// Max length 4
        /// </summary>
        public string SifraNamjene { get; } = "COST";
        /// <summary>
        /// Max length 35
        /// </summary>
        public string Opis { get; } = "Troškovi";
        #endregion
        public Hub3a () {
            PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = new FontResolver ();
        }
        public Hub3a (string barCodeText) : this () {

            string[] t = barCodeText.Split ('\n', StringSplitOptions.RemoveEmptyEntries);
            Zaglavlje = $"{t[0]}";
            ValutaPlacanja = $"{t[1]}";
            Iznos = $"{t[2]}";
            PlatiteljPrvaLinija = $"{t[3]}";
            PlatiteljDrugaLinija = $"{t[4]}";
            PlatiteljTrecaLinija = $"{t[5]}";
            PrimateljPrvaLinija = $"{t[6]}";
            PrimateljDrugaLinija = $"{t[7]}";
            PrimateljTrecaLinija = $"{t[8]}";
            PrimateljIBAN = $"{t[9]}";
            Model = $"{t[10]}";
            PozivNaBroj = $"{t[11]}";
            SifraNamjene = $"{t[12]}";
            Opis = $"{t[13]}";
        }
        public string GetBarCodeText () {

            return Zaglavlje + "\n" +
                ValutaPlacanja + "\n" +
                Iznos + "\n" +
                PlatiteljPrvaLinija + "\n" +
                PlatiteljDrugaLinija + "\n" +
                PlatiteljTrecaLinija + "\n" +
                PrimateljPrvaLinija + "\n" +
                PrimateljDrugaLinija + "\n" +
                PrimateljTrecaLinija + "\n" +
                PrimateljIBAN + "\n" +
                Model + "\n" +
                PozivNaBroj + "\n" +
                SifraNamjene + "\n" +
                Opis + "\n";
        }
        public void DajPDFUplatnicu (string pdfFilePath) {
            if (System.IO.File.Exists (pdfFilePath))
                System.IO.File.Delete (pdfFilePath);

            using (FileStream outputFileStream = new FileStream (pdfFilePath, FileMode.Create)) {
                DajPDFUplatnicu ().CopyTo (outputFileStream);
            }

        }
        public Stream DajPDFUplatnicu () {
            Stream ret = new MemoryStream ();

            var embeddedProvider = new EmbeddedFileProvider (Assembly.GetExecutingAssembly ());
            var reader = embeddedProvider.GetFileInfo ("Pdf.hub-3a.pdf").CreateReadStream ();

            //File dimentions - Width = 17 inches, Height - 11 inches (Tabloid Format)
            PdfDocument pdfDocument = PdfReader.Open (reader, PdfDocumentOpenMode.Modify);

            PdfPage page = pdfDocument.Pages[0];

            var gfx = XGraphics.FromPdfPage (page);
            XPdfFontOptions options = new XPdfFontOptions (PdfFontEncoding.Unicode);
            // no options to embed fonts in PDFSharpCore, seems to work automaticallz

            var font = new XFont ("OpenSans", 10, XFontStyle.Bold, options);

            var pomX = -1;
            var pomY = -1.1;
            gfx.DrawString (PlatiteljPrvaLinija, font, XBrushes.Black, new XRect (40 + pomX, 45 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PlatiteljDrugaLinija, font, XBrushes.Black, new XRect (40 + pomX, 60 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PlatiteljTrecaLinija, font, XBrushes.Black, new XRect (40 + pomX, 75 + pomY, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString (PrimateljPrvaLinija, font, XBrushes.Black, new XRect (40 + pomX, 120 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PrimateljDrugaLinija, font, XBrushes.Black, new XRect (40 + pomX, 135 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PrimateljTrecaLinija, font, XBrushes.Black, new XRect (40 + pomX, 150 + pomY, page.Width, page.Height), XStringFormats.TopLeft);

            int cnt = 0;
            foreach (char c in ValutaPlacanja.ToCharArray ()) {
                gfx.DrawString (ValutaPlacanja.Substring (cnt, 1), font, XBrushes.Black, new XRect (239 + pomX + (cnt * 9.56), 37 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in Iznos.ToCharArray ()) {
                gfx.DrawString (Iznos.Substring (cnt, 1), font, XBrushes.Black, new XRect (296 + pomX + (cnt * 9.56), 37 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in PrimateljIBAN.ToCharArray ()) {
                gfx.DrawString (PrimateljIBAN.Substring (cnt, 1), font, XBrushes.Black, new XRect (239 + pomX + (cnt * 9.56), 94 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in Model.ToCharArray ()) {
                gfx.DrawString (Model.Substring (cnt, 1), font, XBrushes.Black, new XRect (172 + pomX + (cnt * 9.56), 117 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in PozivNaBroj.ToCharArray ()) {
                gfx.DrawString (PozivNaBroj.Substring (cnt, 1), font, XBrushes.Black, new XRect (230 + pomX + (cnt * 9.56), 117 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in SifraNamjene.ToCharArray ()) {
                gfx.DrawString (SifraNamjene.Substring (cnt, 1), font, XBrushes.Black, new XRect (172 + pomX + (cnt * 9.56), 143 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            gfx.DrawString (Opis, font, XBrushes.Black, new XRect (258 + pomX, 132 + pomY, page.Width, page.Height), XStringFormats.TopLeft);

            var fontSmall = new XFont ("OpenSans", 8, XFontStyle.Bold, options);

            gfx.DrawString ($"{ValutaPlacanja} {int.Parse(Iznos) / 100f}", fontSmall, XBrushes.Black, new XRect (458 + pomX, 41 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{PlatiteljPrvaLinija}", fontSmall, XBrushes.Black, new XRect (458 + pomX, 58 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{PrimateljIBAN}", fontSmall, XBrushes.Black, new XRect (458 + pomX, 98 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{Model} {PozivNaBroj}", fontSmall, XBrushes.Black, new XRect (458 + pomX, 120.5 + pomY, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{Opis}", fontSmall, XBrushes.Black, new XRect (458 + pomX, 139 + pomY, page.Width, page.Height), XStringFormats.TopLeft);

            //  bar code
            Stream bc = DajBarKodPNG ();
            bc.Position = 0; //Fix bug PdfSharpCore 
            XImage image = XImage.FromStream (() => bc);
            gfx.DrawImage (image, 38f, 180f, 163, 163 / 3);
            // --------
            pdfDocument.Save (ret);
            return ret;
        }
        public Stream DajBarKodPNG () {
            Stream ret = new MemoryStream ();
            //Širina barkoda iznosi 58 mm. Visina barkoda ovisi o koliĉini i
            //vrsti podataka, ali ne smije biti veća od 26 mm(sa podruĉjima tišine).
            int dpi = 150;
            double inchesW = 5.8 * 2.54;
            int imgW = Convert.ToInt32 (inchesW * dpi);
            int imgH = imgW / 3;

            var pixelData = DajBarKodPixelData (imgW, imgH);

            var bgColor = SixLabors.ImageSharp.PixelFormats.Rgba32.ParseHex ("#FFFFFF");

            var image = Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32> (pixelData.Pixels, imgW, imgH);

            image.SaveAsPng (ret, new PngEncoder () {
                BitDepth = PngBitDepth.Bit1,
                    ColorType = PngColorType.Grayscale
            });

            return ret;
        }
        public void DajBarKodPNG (string barCodePNGFilePath) {
            if (System.IO.File.Exists (barCodePNGFilePath))
                System.IO.File.Delete (barCodePNGFilePath);

            using (FileStream outputFileStream = new FileStream (barCodePNGFilePath, FileMode.Create)) {
                DajBarKodPNG ().CopyTo (outputFileStream);
            }
        }
        public ZXing.Rendering.PixelData DajBarKodPixelData (int imgW, int imgH) {

            var barcodeWriterPixelData = new ZXing.BarcodeWriterPixelData {
                Format = BarcodeFormat.PDF_417,
                Options = new EncodingOptions {
                Height = imgH,
                Width = imgW,

                },
                Renderer = new PixelDataRenderer {
                Foreground = new PixelDataRenderer.Color (unchecked ((int) 0xFF000000)),
                Background = new PixelDataRenderer.Color (unchecked ((int) 0xFFFFFFFF)),

                }
            };
            barcodeWriterPixelData.Options.Hints[EncodeHintType.ERROR_CORRECTION] = ERROR_CORRECTION_LEVEL;
            return barcodeWriterPixelData.Write (GetBarCodeText ());

        }
        public string DajBarKodSVG () {
            //Širina barkoda iznosi 58 mm. Visina barkoda ovisi o koliĉini i
            //vrsti podataka, ali ne smije biti veća od 26 mm(sa podruĉjima tišine).
            int dpi = 150;
            double inchesW = 5.8 * 2.54;
            int imgW = Convert.ToInt32 (inchesW * dpi);
            int imgH = imgW / 3;

            var barcodeWriter = new ZXing.BarcodeWriterSvg {
                Format = BarcodeFormat.PDF_417,
                Options = new EncodingOptions {
                Height = imgH,
                Width = imgW
                }
            };
            barcodeWriter.Options.Hints[EncodeHintType.ERROR_CORRECTION] = ERROR_CORRECTION_LEVEL;
            ZXing.Rendering.SvgRenderer.SvgImage ret = barcodeWriter.Write (GetBarCodeText ());

            return ret.Content;

        }
    }
}
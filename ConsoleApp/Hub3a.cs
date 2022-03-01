using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SharpPdf417;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace ConsoleApp {
    //Generiram PDF417 2D BAR-KOD prema HUB3a standardu.

    public class Hub3a {
        string barCodeText = "";
        #region props
        /// <summary>
        /// Max length 8
        /// </summary>
        public string Zaglavlje { get; }
        /// <summary>
        /// Max length 3
        /// </summary>
        public string ValutaPlacanja { get; }
        /// <summary>
        /// Max length 15
        /// </summary>
        public string Iznos { get; }
        /// <summary>
        /// Max length 30
        /// </summary>
        public string PlatiteljPrvaLinija { get; }
        /// <summary>
        /// Max length 27
        /// </summary>
        public string PlatiteljDrugaLinija { get; }
        /// <summary>
        /// Max length 27
        /// </summary>
        public string PlatiteljTrecaLinija { get; }
        /// <summary>
        /// Max length 25
        /// </summary>
        public string PrimateljPrvaLinija { get; }
        /// <summary>
        /// Max length 25
        /// </summary>
        public string PrimateljDrugaLinija { get; }
        /// <summary>
        /// Max length 27
        /// </summary>
        public string PrimateljTrecaLinija { get; }
        /// <summary>
        /// Max length 21
        /// </summary>
        public string PrimateljIBAN { get; }
        /// <summary>
        /// Max length 4
        /// </summary>
        public string Model { get; }
        /// <summary>
        /// Max length 22
        /// </summary>
        public string PozivNaBroj { get; }
        /// <summary>
        /// Max length 4
        /// </summary>
        public string SifraNamjene { get; }
        /// <summary>
        /// Max length 35
        /// </summary>
        public string Opis { get; }
        #endregion

        public Hub3a (string text) {
            PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = new FontResolver ();
            this.barCodeText = text;

            string[] t = text.Split ('\n', StringSplitOptions.RemoveEmptyEntries);
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

            //// convert them into unicode bytes.
            //byte[] unicodeBytes = Encoding.Convert(Encoding.Default, Encoding.Unicode, Encoding.Default.GetBytes(PlatiteljPrvaLinija));

            //// builds the converted string.
            //PlatiteljPrvaLinija = Encoding.Unicode.GetString(unicodeBytes);
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

            //File dimentions - Width = 17 inches, Height - 11 inches (Tabloid Format)
            PdfDocument pdfDocument = PdfReader.Open (@"Pdf/hub-3a.pdf", PdfDocumentOpenMode.Modify);

            PdfPage page = pdfDocument.Pages[0];

            var gfx = XGraphics.FromPdfPage (page);

            XPdfFontOptions options = new XPdfFontOptions (PdfFontEncoding.Unicode);

            var font = new XFont ("OpenSans", 10, XFontStyle.Bold, options);

            gfx.DrawString (PlatiteljPrvaLinija, font, XBrushes.Black, new XRect (40, 45, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PlatiteljDrugaLinija, font, XBrushes.Black, new XRect (40, 60, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PlatiteljTrecaLinija, font, XBrushes.Black, new XRect (40, 75, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString (PrimateljPrvaLinija, font, XBrushes.Black, new XRect (40, 120, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PrimateljDrugaLinija, font, XBrushes.Black, new XRect (40, 135, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString (PrimateljTrecaLinija, font, XBrushes.Black, new XRect (40, 150, page.Width, page.Height), XStringFormats.TopLeft);

            int cnt = 0;
            foreach (char c in ValutaPlacanja.ToCharArray ()) {
                gfx.DrawString (ValutaPlacanja.Substring (cnt, 1), font, XBrushes.Black, new XRect (239 + (cnt * 9.56), 37, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in Iznos.ToCharArray ()) {
                gfx.DrawString (Iznos.Substring (cnt, 1), font, XBrushes.Black, new XRect (296 + (cnt * 9.56), 37, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in PrimateljIBAN.ToCharArray ()) {
                gfx.DrawString (PrimateljIBAN.Substring (cnt, 1), font, XBrushes.Black, new XRect (239 + (cnt * 9.56), 94, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in Model.ToCharArray ()) {
                gfx.DrawString (Model.Substring (cnt, 1), font, XBrushes.Black, new XRect (172 + (cnt * 9.56), 117, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in PozivNaBroj.ToCharArray ()) {
                gfx.DrawString (PozivNaBroj.Substring (cnt, 1), font, XBrushes.Black, new XRect (230 + (cnt * 9.56), 117, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            cnt = 0;
            foreach (char c in SifraNamjene.ToCharArray ()) {
                gfx.DrawString (SifraNamjene.Substring (cnt, 1), font, XBrushes.Black, new XRect (172 + (cnt * 9.56), 143, page.Width, page.Height), XStringFormats.TopLeft);
                cnt++;

            }

            gfx.DrawString (Opis, font, XBrushes.Black, new XRect (258, 132, page.Width, page.Height), XStringFormats.TopLeft);

            var fontSmall = new XFont ("OpenSans", 8, XFontStyle.Bold, options);

            gfx.DrawString ($"{ValutaPlacanja} {int.Parse(Iznos) / 100f}", fontSmall, XBrushes.Black, new XRect (458, 41, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{PlatiteljPrvaLinija}", fontSmall, XBrushes.Black, new XRect (458, 58, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{PrimateljIBAN}", fontSmall, XBrushes.Black, new XRect (458, 98, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{Model} {PozivNaBroj}", fontSmall, XBrushes.Black, new XRect (458, 120.5, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString ($"{Opis}", fontSmall, XBrushes.Black, new XRect (458, 139, page.Width, page.Height), XStringFormats.TopLeft);

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
            int aspectRatio = 3;
            int paddingLeftRight = 0;
            int paddingTopBottom = 0;
            Pdf417Generator generator = new Pdf417Generator (this.barCodeText, ErrorCorrectionLevel.LevelFour, aspectRatio, paddingLeftRight, paddingTopBottom);
            Barcode barcode = generator.Encode ();

            //Širina barkoda iznosi 58 mm. Visina barkoda ovisi o koliĉini i
            //vrsti podataka, ali ne smije biti veća od 26 mm(sa podruĉjima tišine).
            int dpi = 300;
            double inchesW = 5.8 * 2.54;
            //           double inchesH = 1.2 * 2.54;
            var imgW = inchesW * dpi;
            var imgH = imgW * ((double) barcode.Rows / (double) barcode.Columns);

            var bgColor = SixLabors.ImageSharp.PixelFormats.Rgba32.ParseHex ("#FFFFFF");

            using (var image = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32> (
                Convert.ToInt32 (imgW),
                Convert.ToInt32 (imgH),
                bgColor)) {
                image.Metadata.HorizontalResolution = dpi;
                image.Metadata.VerticalResolution = dpi;

                image.Mutate (imageContext => {

                    // BarCode
                    int bw = Convert.ToInt32 (imgW) / barcode.Columns;
                    int bh = Convert.ToInt32 (imgH) / barcode.Rows;

                    int y = 0;
                    for (int r = 0; r < barcode.Rows; ++r) {
                        int x = 0;
                        for (int c = 0; c < barcode.Columns; ++c) {
                            if (barcode.RawData[r][c] == 1) {

                                // image.Get
                                //// e.Graphics.FillRectangle(Brushes.Black, x, y, bw, bh);

                                var points = new PointF[4];
                                points[0] = new PointF (
                                    x: x,
                                    y: y);
                                points[1] = new PointF (
                                    x: (float) (x + bw),
                                    y: (float) (y));
                                points[2] = new PointF (
                                    x: (float) (x + bw),
                                    y: (float) (y + bh));
                                points[3] = new PointF (
                                    x: (float) (x),
                                    y: (float) (y + bh));

                                // create a pen unique to this line
                                var lineColor = SixLabors.ImageSharp.Color.Black;
                                float lineWidth = bw;
                                var linePen = new Pen (lineColor, lineWidth);
                                linePen.JointStyle = JointStyle.Square;
                                linePen.EndCapStyle = EndCapStyle.Square;

                                // draw the line
                                imageContext.FillPolygon (lineColor, points);
                            }
                            x += bw;
                        }
                        y += bh;
                    }

                    // -------

                });

                image.SaveAsPng (ret, new PngEncoder () {
                    BitDepth = PngBitDepth.Bit1,
                        ColorType = PngColorType.Grayscale
                });
            }

            return ret;
        }
        public void DajBarKodPNG (string barCodePNGFilePath) {
            if (System.IO.File.Exists (barCodePNGFilePath))
                System.IO.File.Delete (barCodePNGFilePath);

            using (FileStream outputFileStream = new FileStream (barCodePNGFilePath, FileMode.Create)) {
                DajBarKodPNG ().CopyTo (outputFileStream);
            }
        }
    }
}
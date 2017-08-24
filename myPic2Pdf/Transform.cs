using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Transform
{
    /// <summary>
    /// The base class with some helper functions.
    /// </summary>
    public class Base
    {
        protected XColor backColor;
        protected XColor backColor2;
        protected XColor shadowColor;
        protected double borderWidth;
        protected XPen borderPen;
        protected static PdfDocument s_document { get; set; }

        protected Base(PdfDocument document)
        {
            s_document = document;
            this.backColor = XColors.Ivory;
            this.backColor2 = XColors.WhiteSmoke;

            this.backColor = XColor.FromArgb(212, 224, 240);
            this.backColor2 = XColor.FromArgb(253, 254, 254);

            this.shadowColor = XColors.Gainsboro;
            this.borderWidth = 4.5;
            this.borderPen = new XPen(XColor.FromArgb(94, 118, 151), this.borderWidth);//new XPen(XColors.SteelBlue, this.borderWidth);
        }

        /// <summary>
        /// Draws the page title and footer.
        /// </summary>
        public void DrawTitle(PdfPage page, XGraphics gfx, string title)
        {
            XRect rect = new XRect(new XPoint(), gfx.PageSize);
            rect.Inflate(-10, -15);
            XFont font = new XFont("Verdana", 14, XFontStyle.Bold);
            gfx.DrawString(title, font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

            rect.Offset(0, 5);
            font = new XFont("Verdana", 8, XFontStyle.Italic);
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Far;
            gfx.DrawString("Created with " + PdfSharp.ProductVersionInfo.Producer, font, XBrushes.DarkOrchid, rect, format);

            font = new XFont("Verdana", 8);
            format.Alignment = XStringAlignment.Center;
            gfx.DrawString(s_document.PageCount.ToString(), font, XBrushes.DarkOrchid, rect, format);

            s_document.Outlines.Add(title, page, true);
        }

        /// <summary>
        /// Draws a sample box.
        /// </summary>
        public void BeginBox(XGraphics gfx, int number, string title)
        {
            const int dEllipse = 15;
            XRect rect = new XRect(0, 20, 300, 200);
            if (number % 2 == 0)
                rect.X = 300 - 5;
            rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
            rect.Inflate(-10, -10);
            XRect rect2 = rect;
            rect2.Offset(this.borderWidth, this.borderWidth);
            gfx.DrawRoundedRectangle(new XSolidBrush(this.shadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
            XLinearGradientBrush brush = new XLinearGradientBrush(rect, this.backColor, this.backColor2, XLinearGradientMode.Vertical);
            gfx.DrawRoundedRectangle(this.borderPen, brush, rect, new XSize(dEllipse, dEllipse));
            rect.Inflate(-5, -5);

            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

            rect.Inflate(-10, -5);
            rect.Y += 20;
            rect.Height -= 20;
            //gfx.DrawRectangle(XPens.Red, rect);

            this.state = gfx.Save();
            gfx.TranslateTransform(rect.X, rect.Y);
        }

        public void EndBox(XGraphics gfx)
        {
            gfx.Restore(this.state);
        }

        /// <summary>
        /// Gets a five-pointed star with the specified size and center.
        /// </summary>
        protected static XPoint[] GetPentagram(double size, XPoint center)
        {
            XPoint[] points = Pentagram.Clone() as XPoint[];
            for (int idx = 0; idx < 5; idx++)
            {
                points[idx].X = points[idx].X * size + center.X;
                points[idx].Y = points[idx].Y * size + center.Y;
            }
            return points;
        }

        /// <summary>
        /// Gets a normalized five-pointed star.
        /// </summary>
        static XPoint[] Pentagram
        {
            get
            {
                if (pentagram == null)
                {
                    int[] order = new int[] { 0, 3, 1, 4, 2 };
                    pentagram = new XPoint[5];
                    for (int idx = 0; idx < 5; idx++)
                    {
                        double rad = order[idx] * 2 * Math.PI / 5 - Math.PI / 10;
                        pentagram[idx].X = Math.Cos(rad);
                        pentagram[idx].Y = Math.Sin(rad);
                    }
                }
                return pentagram;
            }
        }
        static XPoint[] pentagram;

        XGraphicsState state;
    }
    public class PDFSharpImages : Base
    {

        public PDFSharpImages(PdfDocument documnet)
            : base(documnet)
        {
        }

        public void DrawImage(XGraphics gfx, string jpegSamplePath, bool isAutoZoom = false)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            double x = (gfx.PageSize.Width - image.PixelWidth * 72 / image.HorizontalResolution) / 2;
            double y = (gfx.PageSize.Height - image.PixelHeight * 72 / image.VerticalResolution) / 2;
            if (isAutoZoom)
            {
                double s1 = gfx.PageSize.Width / image.Size.Width;
                double s2 = gfx.PageSize.Height / image.Size.Height;
                double s = s1 > s2 ? s2 : s1;
                double width = image.Size.Width * s;
                double height = image.Size.Height * s;
                if (s1 > s2)
                {
                    y = 0.0;
                    x = (gfx.PageSize.Width - image.PixelWidth * s * 72 / image.HorizontalResolution) / 2;
                }
                else
                {
                    x = 0.0;
                    y = (gfx.PageSize.Height - image.PixelHeight * s * 72 / image.VerticalResolution) / 2;
                }
                gfx.DrawImage(image, x, y, width, height);
            }
            else
                gfx.DrawImage(image, x, y);
            image.Dispose();
        }
        /// 作为遍历文件函数的子函数
        /// </summary>
        /// <param name="parentDir">目录名称</param>
        /// <param name="result">该目录下的所有文件</param>
        static public List<string> GetAllFiles(string parentDir)
        {
            List<string> _fileList = new List<string>();
            DirectoryInfo TheFolder = new DirectoryInfo(parentDir);
            foreach (FileInfo fi in TheFolder.GetFiles())//遍历文件夹下所有文件
            {
                if (fi.Extension.Equals(".txt"))
                    continue;
                _fileList.Add(fi.FullName);
            }
            return _fileList;
        }
    }
}

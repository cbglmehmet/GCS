using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SkiaSharp;

namespace System.Drawing
{
    public class Penn : ICloneable, IDisposable
    {
        private SKPaint _nativePen;

        internal SKPaint nativePen
        {
            get => _nativePen;
            set
            {
                _nativePen = value;
                Color = Color.FromArgb(_nativePen.Color.Red, _nativePen.Color.Green, _nativePen.Color.Blue);
                Brush = new SolidBrushh(Color);
            }
        }

        internal Penn()
        {
        }

        public Penn(Color color) : this(color.ToSKColor())
        {
        }

        public Penn(SKColor color, float width = 1)
        {
            Width = width;
            Color = Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
            Brush = new SolidBrushh(Color);

            try
            {
                nativePen = new SKPaint()
                {
                    Color = color,
                    StrokeWidth = Width,
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    BlendMode = SKBlendMode.SrcOver,
                    FilterQuality = SKFilterQuality.High
                };
            }
            catch (Exception)
            {
                //Console.WriteLine(e);
            }
        }

        public Penn(Color brush, float width) : this(brush.ToSKColor(), width)
        {
        }

        public Penn(Brushh brush, float width) : this(brush._color, width)
        {
        }

        public Penn(Brushh brush) : this(brush, 1)
        {
        }

        public LineJoin LineJoin { get; set; }
        public float Width { get; set; } = 2;
        public LineCap StartCap { get; set; }
        public DashStyle DashStyle { get; set; }
        public Color Color { get; set; } = Color.Black;
        public Brushh Brush { get; set; }
        public float[] DashPattern { get; set; } = new float[] { 1, 0 };
        public PenAlignment Alignment { get; set; }

        public int MiterLimit { get; set; }

        public LineCap EndCap { get; set; }

        public object Clone()
        {
            return new Penn(nativePen.Color, Width);
        }

        public void Dispose()
        {
            nativePen?.Dispose();
        }
    }
}
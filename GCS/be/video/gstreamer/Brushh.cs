using System;
using System.Drawing;
using SkiaSharp;

namespace System.Drawing
{
    public abstract class Brushh : IDisposable
    {
        internal SKPaint nativeBrush;
        public Color _color = Color.Black;

        public void Dispose()
        {
            nativeBrush?.Dispose();
        }

        public Brushh Clone()
        {
            return new SolidBrushh() { nativeBrush = nativeBrush?.Clone() };
        }
    }
}
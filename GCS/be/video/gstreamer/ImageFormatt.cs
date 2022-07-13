using System;
using SkiaSharp;

namespace System.Drawing.Imaging
{
    public sealed class ImageFormatt
    {
        internal SKEncodedImageFormat format;

        public ImageFormatt(SKEncodedImageFormat format)
        {
            this.format = format;
        }

        public static ImageFormatt Bmp { get; set; } = new ImageFormatt(SKEncodedImageFormat.Bmp);

        public static ImageFormatt Png { get; set; } = new ImageFormatt(SKEncodedImageFormat.Png);

        public static ImageFormatt MemoryBmp { get; set; } = new ImageFormatt(SKEncodedImageFormat.Wbmp);


        public static ImageFormatt Tiff { get; set; } = new ImageFormatt(SKEncodedImageFormat.Jpeg);


        public static ImageFormatt Gif { get; set; } = new ImageFormatt(SKEncodedImageFormat.Gif);

        public static ImageFormatt Jpeg { get; set; } = new ImageFormatt(SKEncodedImageFormat.Jpeg);


        public static ImageFormatt Icon { get; set; } = new ImageFormatt(SKEncodedImageFormat.Ico);
    }
}
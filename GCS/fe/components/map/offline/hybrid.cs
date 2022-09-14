namespace GMap.NET.MapProviders
{
    using System;
    using GMap.NET.Projections;
    using GMap.NET.Internals;
    using System.Drawing;
    using System.IO;
    using System.Drawing.Imaging;
    public class hybrid : BingMapProviderBase
    {
        public static readonly hybrid Instance;
        public static byte[] emptyTile;
        static readonly string UrlFormat = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"map\hybrid\{0}_{1}_{2}.jpg";
        static readonly string empty = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"map\_empty.jpg";
        private string url;
        private static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        private hybrid()
        {
            emptyTile = ImageToByteArray(new Bitmap(empty));
        }

        static hybrid()
        {
            emptyTile = ImageToByteArray(new Bitmap(empty));
            Instance = new hybrid();
        }

        private readonly Guid id = new Guid("DC69BD46-6D65-44BE-AE29-F1C8D66D3F50");

        public override Guid Id
        {
            get { return id; }
        }

        private readonly string name = "MyMap";


        public override string Name
        {
            get { return name; }
        }

        private GMapProvider[] overlays;

        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { BingMapProvider.Instance, this };
                }
                return overlays;
            }
        }
        
        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            url = MakeTileImageUrl(pos, zoom);
            try
            {
                return GetTileImageFromFile(url);
            }
            catch
            {
                return GetTileImageFromArray(emptyTile);

            }
        }
        private string MakeTileImageUrl(GPoint pos, int zoom)
        {
            return string.Format(UrlFormat, zoom, pos.Y, pos.X);
        }
    }
}

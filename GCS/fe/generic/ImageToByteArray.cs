using System.IO;

namespace Convert
{
    public sealed class ImageToByteArray
    {
        private ImageToByteArray() { }
        private static ImageToByteArray _instance = null;
        public static ImageToByteArray Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ImageToByteArray();
                }
                return _instance;
            }
        }

        public static byte[] Bitmap(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public static byte[] File(string fileIn)
        {
            System.Drawing.Image imageIn = new System.Drawing.Bitmap(fileIn);

            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;

namespace GCS.fe.components.camera
{
    /// <summary>
    /// Interaction logic for cntrlCamera.xaml
    /// </summary>
    public partial class cntrlCamera : UserControl
    {
        private string _pipeline="";
        private string _sembology = "";
        private bool _takeSnapshot = false;
        private Bitmap frame;
        private RectangleF _sembologyLocationSize = new RectangleF(70, 90, 200, 100);
        private Graphics graph;
        private static System.Drawing.Imaging.BitmapData bitmapData;
        private static System.Windows.Media.Imaging.BitmapSource bitmapSource;
        public string Pipeline
        {
            get
            {
                return _pipeline;
            }
            set
            {
                _pipeline = value;
            }
        }
        public string Sembology
        {
            get
            {
                return _sembology;
            }
            set
            {
                _sembology = value;
            }
        }
        public RectangleF SembologyLocationSize
        {
            get
            {
                return _sembologyLocationSize;
            }
            set
            {
                _sembologyLocationSize = value;
            }
        }
        public cntrlCamera()
        {
            InitializeComponent();

            //check folders
            if(!Directory.Exists("snapshot"))
            {
                Directory.CreateDirectory("snapshot");
            }

            if (!Directory.Exists("video"))
            {
                Directory.CreateDirectory("video");
            }
        }

        public void startVideo()
        {
            be.video.GST.StopAll();
            be.video.GST.LookForGstreamer();

            if (!be.video.GST.gstlaunchexists)
            {


                return;

            }

            be.video.GST.StartA("videotestsrc ! video/x-raw, width=1280, height=720,framerate=25/1 ! clockoverlay ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink");


            Thread camera = new Thread(new ThreadStart(camera_DoWork));
            camera.Start();
        }
        public void stopVideo()
        {

        }
        public void recordVideo()
        {

        }
        public void takeSnapshot()
        {
            _takeSnapshot = true;
        }

        private void camera_DoWork()
        {
            be.video.GST.onNewImage += (sender, image) =>
            {
                frame = new System.Drawing.Bitmap(image.Width, image.Height, 4 * image.Width,
                       System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                       image.LockBits(System.Drawing.Rectangle.Empty, null, SkiaSharp.SKColorType.Bgra8888)
                           .Scan0);



                graph = Graphics.FromImage(frame);

                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graph.DrawString(_sembology, new Font("Tahoma", 20), System.Drawing.Brushes.Black, _sembologyLocationSize);

                graph.Flush();


                if(_takeSnapshot)
                {
                    _takeSnapshot = false;
                    frame.Save("snapshot/snap_" + System.DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_ff") + ".bmp") ;
                }


                camera.Dispatcher.BeginInvoke((Action)(() =>
                {
                    camera.Source = Convert(frame);
                }));


            };
        }
 
        public static BitmapSource Convert(System.Drawing.Bitmap bitmap)
        {
            bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Pbgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
    }


}

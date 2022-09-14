using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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

namespace GCS.fe.components.map
{
    /// <summary>
    /// Interaction logic for cntrlMap.xaml
    /// </summary>
    public partial class cntrlMap : UserControl
    {
        private static string mapCacheLocation = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"map\";
        public cntrlMap()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if(!Directory.Exists(mapCacheLocation))
                {
                    Directory.CreateDirectory(mapCacheLocation);
                }

                if (!Directory.Exists(mapCacheLocation + "hybrid"))
                {
                    Directory.CreateDirectory(mapCacheLocation + "hybrid");
                }

                if (Directory.Exists(mapCacheLocation + "TileDBv5"))
                {
                    Directory.Delete(mapCacheLocation + "TileDBv5", true);
                }

                if (!File.Exists(mapCacheLocation + "_empty.jpg"))
                {
                    File.WriteAllBytes(mapCacheLocation + "_empty.jpg", Convert.ImageToByteArray.Bitmap(GCS.Properties.Resources._empty));
                }
            }

            


            InitializeComponent();
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            map.MapProvider = GMap.NET.MapProviders.hybrid.Instance;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {

                map.CacheLocation = mapCacheLocation;

            }
        }

        private void map_Loaded(object sender, RoutedEventArgs e)
        {
            map.Position = new GMap.NET.PointLatLng(40.91738, 29.3038825);
            map.MinZoom = 2;
            map.MaxZoom = 17;
            map.ShowCenter = false;
            map.Zoom = 16;
            map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            map.CanDragMap = true;
            map.DragButton = MouseButton.Left;
        }
    }
}

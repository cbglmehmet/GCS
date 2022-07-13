using System;
using System.Collections.Generic;
using System.IO;
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

namespace GCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //check gstreamer dll and load
            if (!File.Exists("libgstreamer-1.0-0.dll"))
            {
                File.WriteAllBytes("libgstreamer-1.0-0.dll", GCS.Properties.Resources.libgstreamer_1_0_0);
            }


            //test pipeline
            camView.Pipeline = "videotestsrc ! video/x-raw, width=1280, height=720,framerate=25/1 ! clockoverlay ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink";
            camView.startVideo();

            //test counter
            Thread threadTimer = new Thread(new ThreadStart(threadTimer_DoWork));
            threadTimer.Start();
        }

        private int counter = 0;
        private void threadTimer_DoWork()
        {
            do
            {
                counter++;
                camView.Sembology = "MehmetCBGL\n" + counter.ToString();
                Thread.Sleep(1000);
            } while (true);
        }
        private void takeSnapshot_Click(object sender, RoutedEventArgs e)
        {
            camView.takeSnapshot();
        }
    }
}

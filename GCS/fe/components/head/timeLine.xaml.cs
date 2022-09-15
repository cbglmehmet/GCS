using System;
using System.Collections.Generic;
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

namespace GCS.fe.components.head
{
    /// <summary>
    /// Interaction logic for timeLine.xaml
    /// </summary>
    public partial class timeLine : UserControl
    {
        private Thickness _margin = new Thickness(0,0,0,0);
        private double scaled = 0;
        private double _percent = 0;
        private double _lineWidth = 0;
        private string _remainingTime = "00:00";

        public double LineWidth
        {
            get
            {
                return _lineWidth;
            }
            set
            {
                _lineWidth = value;
                lineGrid.Width = _lineWidth;

            }
        }

        public string RemainingTime
        {
            get
            {
                return _remainingTime;
            }
            set
            {
                _remainingTime = value;
                time.Dispatcher.Invoke(() =>
                {
                    time.Content = _remainingTime;
                });
            }
        }
        public double Percent
        {
            get
            {
                return _percent;
            }
            set
            {
                _percent = value;
                setPosition(_percent);
            }
        }

        private void setPosition(double value)
        {
            line.Dispatcher.Invoke(() =>
            {
                scaled = Scale.valueScale.reScale(value, 0, 100, 0, _lineWidth) - (text.Width / 2);
                _margin.Left = scaled;
                text.Margin = _margin;
                line.Width = scaled + (text.Width / 2);
            });
        }

        public timeLine()
        {
            InitializeComponent();
        }
    }
}

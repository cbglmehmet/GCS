using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scale
{
    public sealed class valueScale
    {
        private valueScale() { }
        private static valueScale _instance = null;
        public static valueScale Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new valueScale();
                }
                return _instance;
            }
        }

        public static double reScale(double value, double min, double max, double minScale, double maxScale)
        {
                return (minScale + (double)(value - min) / (max - min) * (maxScale - minScale));
            
        }
    }
}

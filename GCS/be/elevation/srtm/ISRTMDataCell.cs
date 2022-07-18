using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS.be.elevation.srtm
{
    public interface ISRTMDataCell
    {
        int Latitude { get; }

        int Longitude { get; }

        int? GetElevation(double latitude, double longitude);

        double? GetElevationBilinear(double latitude, double longitude);
    }
}

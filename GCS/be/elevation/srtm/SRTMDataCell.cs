using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GCS.be.elevation.srtm
{
    public class SRTMDataCell : ISRTMDataCell
    {
        #region Lifecycle

        public SRTMDataCell(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException("File not found.", filepath);

            var filename = Path.GetFileName(filepath);
            filename = filename.Substring(0, filename.IndexOf('.')).ToLower(); // Path.GetFileNameWithoutExtension(filepath).ToLower();
            var fileCoordinate = filename.Split(new[] { 'e', 'w' });
            if (fileCoordinate.Length != 2)
                throw new ArgumentException("Invalid filename.", filepath);

            fileCoordinate[0] = fileCoordinate[0].TrimStart(new[] { 'n', 's' });

            Latitude = int.Parse(fileCoordinate[0]);
            if (filename.Contains("s"))
                Latitude *= -1;

            Longitude = int.Parse(fileCoordinate[1]);
            if (filename.Contains("w"))
                Longitude *= -1;

            try
            {
                HgtData = File.ReadAllBytes(filepath);
            }
            catch
            {

            }

            if (HgtData == null)
            {
                return;
            }


            switch (HgtData.Length)
            {
                case 1201 * 1201 * 2: // SRTM-3
                    PointsPerCell = 1201;
                    break;
                case 3601 * 3601 * 2: // SRTM-1
                    PointsPerCell = 3601;
                    break;
                default:
                    throw new ArgumentException("Invalid file size.", filepath);
            }
        }

        #endregion

        #region Properties

        private byte[] HgtData { get; set; }

        private int PointsPerCell { get; set; }

        public int Latitude { get; private set; }

        public int Longitude { get; private set; }

        #endregion

        #region Public Methods

        public int? GetElevation(double latitude, double longitude)
        {
            int localLat = (int)((latitude - Latitude) * PointsPerCell);
            int localLon = (int)(((longitude - Longitude)) * PointsPerCell);
            return ReadByteData(localLat, localLon);
        }

        public double? GetElevationBilinear(double latitude, double longitude)
        {
            double localLat = (latitude - Latitude) * PointsPerCell;
            double localLon = (longitude - Longitude) * PointsPerCell;

            int localLatMin = (int)Math.Floor(localLat);
            int localLonMin = (int)Math.Floor(localLon);
            int localLatMax = (int)Math.Ceiling(localLat);
            int localLonMax = (int)Math.Ceiling(localLon);

            int? elevation00 = ReadByteData(localLatMin, localLonMin);
            int? elevation10 = ReadByteData(localLatMax, localLonMin);
            int? elevation01 = ReadByteData(localLatMin, localLonMax);
            int? elevation11 = ReadByteData(localLatMax, localLonMax);

            if (!elevation00.HasValue || !elevation10.HasValue || !elevation01.HasValue || !elevation11.HasValue)
            {
                //Can't do bilinear if missing one of the points. Default to regular.
                return (double)GetElevation(latitude, longitude);
            }

            double deltaLat = localLatMax - localLat;
            double deltaLon = localLonMax - localLon;

            return Blerp((double)elevation00, (double)elevation10, (double)elevation01, (double)elevation11,
                deltaLat, deltaLon);
        }

        #endregion

        #region Private Methods

        private int? ReadByteData(int localLat, int localLon)
        {
            if (HgtData == null)
            {
                return null;
            }
            int bytesPos = ((PointsPerCell - localLat - 1) * PointsPerCell * 2) + localLon * 2;

            if (bytesPos < 0 || bytesPos > PointsPerCell * PointsPerCell * 2)
                throw new ArgumentOutOfRangeException("Coordinates out of range.", "coordinates");

            if (bytesPos >= HgtData.Length)
                return null;

            if ((HgtData[bytesPos] == 0x80) && (HgtData[bytesPos + 1] == 0x00))
                return null;

            // Motorola "big-endian" order with the most significant byte first
            return (HgtData[bytesPos]) << 8 | HgtData[bytesPos + 1];
        }

        private double Lerp(double start, double end, double delta)
        {
            return start + (end - start) * delta;
        }

        private double Blerp(double val00, double val10, double val01, double val11, double deltaX, double deltaY)
        {
            return Lerp(Lerp(val11, val01, deltaX), Lerp(val10, val00, deltaX), deltaY);
        }

        #endregion
    }
}

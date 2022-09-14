using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS.be.elevation.srtm
{
    public class SRTMData : ISRTMData
    {

        public SRTMData(string dataDirectory)
        {
            DataDirectory = dataDirectory;
            DataCells = new List<ISRTMDataCell>();
        }
        public SRTMData(string dataDirectory, Func<(string path, string name), bool> getMissingCell)
            : this(dataDirectory)
        {

        }

        public delegate bool GetMissingCellDelegate(string path, string name);

        public GetMissingCellDelegate GetMissingCell { get; set; }

        public string DataDirectory { get; private set; }

        private List<ISRTMDataCell> DataCells { get; set; }

        #region Public methods

        public void Unload()
        {
            DataCells.Clear();
        }

        public int? GetElevation(double latitude, double longitude)
        {
            ISRTMDataCell dataCell = GetDataCell(latitude, longitude);
            if (dataCell == null)
            {
                return null;
            }
            else
            {
                return dataCell.GetElevation(latitude, longitude);
            }


        }

        public double? GetElevationBilinear(double latitude, double longitude)
        {
            ISRTMDataCell dataCell = GetDataCell(latitude, longitude);
            return dataCell.GetElevationBilinear(latitude, longitude);
        }

        #endregion

        #region Private methods

        private ISRTMDataCell GetDataCell(double latitude, double longitude)
        {
            int cellLatitude = (int)Math.Floor(Math.Abs(latitude));
            if (latitude < 0)
            {
                cellLatitude *= -1;
                if (cellLatitude != latitude)
                { // if exactly equal, keep the current tile.
                    cellLatitude -= 1; // because negative so in bottom tile
                }
            }

            int cellLongitude = (int)Math.Floor(Math.Abs(longitude));
            if (longitude < 0)
            {
                cellLongitude *= -1;
                if (cellLongitude != longitude)
                { // if exactly equal, keep the current tile.
                    cellLongitude -= 1; // because negative so in left tile
                }
            }

            //var dataCell = DataCells.Where(dc => dc.Latitude == cellLatitude && dc.Longitude == cellLongitude).FirstOrDefault();
            //if (dataCell != null)
            //{
            //    return dataCell;
            //}

            string filename = string.Format("{0}{1:D2}{2}{3:D3}",
                cellLatitude < 0 ? "S" : "N",
                Math.Abs(cellLatitude),
                cellLongitude < 0 ? "W" : "E",
                Math.Abs(cellLongitude));

            var filePath = Path.Combine(DataDirectory, filename + ".hgt");
            var zipFilePath = Path.Combine(DataDirectory, filename + ".SRTMGL1.hgt.zip");

            if (!File.Exists(filePath) && !File.Exists(zipFilePath) &&
                this.GetMissingCell != null)
            {
                this.GetMissingCell(DataDirectory, filename);
            }
            SRTMDataCell dataCell;
            if (File.Exists(filePath))
            {
                dataCell = new SRTMDataCell(filePath);
            }
            else if (File.Exists(zipFilePath))
            {
                dataCell = new SRTMDataCell(zipFilePath);
            }
            else
            {
                //yok
                dataCell = null;
            }
            DataCells.Add(dataCell);

            return dataCell;
        }

        #endregion
    }
}

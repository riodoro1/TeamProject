using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Scope
{
    class SignalOpener
    {
        private string filePath;

        public SignalOpener(string path)
        {
            filePath = path;
        }
        
        public Signal readSignal()
        {
            StreamReader fileReader = new StreamReader(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            List<Point> readPoints = new List<Point>();
            string line;
            while ((line = fileReader.ReadLine()) != null)
            {
                string[] components = line.Split(',');

                double time;
                double value;

                if ( components.Length != 2 || !Double.TryParse(components[0], out time) || !Double.TryParse(components[1], out value))
                {
                    //failed conversion
                    throw new FileLoadException("File is not a valid CSV file", fileName);
                }

                readPoints.Add(new Point(time, value));
            }

            string name = fileName;
            fileReader.Close();
            return new TimeDomainSignal(readPoints.ToArray(), Colors.Blue, name);
        }

    }
}

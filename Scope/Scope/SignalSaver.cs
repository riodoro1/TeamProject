using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scope
{
    class SignalSaver
    {
        private Signal signal;

        public SignalSaver(Signal signal)
        {
            this.signal = signal;
        }

        public void SaveAt(string path)
        {
            StreamWriter fileWriter = new StreamWriter(path);

            foreach (Point point in signal.Points)
            {
                double time = point.X;
                double value = point.Y;

                string line = time + "," + value;
                fileWriter.WriteLine(line);
            }

            fileWriter.Close();
        }
    }
}

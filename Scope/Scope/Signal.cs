using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scope
{
    public class SignalPoint
    {
        public double time;
        public double value;

        public SignalPoint(int time, int value)
        {
            this.time = time;
            this.value = value;
        }
    }
    public class Signal
    {
        #region Properties
        public SignalPoint[] Points;
        #endregion

        #region Computed properties
        public Double StartTime
        {
            get
            {
                return Points[0].time;
            }
        }

        public Double EndTime
        {
            get
            {
                return Points[Points.Length - 1].time;
            }
        }

        public Double DeltaT
        {
            get
            {
                return (StartTime - EndTime) / Points.Length;
            }
        }
        #endregion

        public Signal()
        {
            Points = new SignalPoint[512];

            for (int x = 0; x < 512; x++)
            {
                Points[x] = new SignalPoint(x, (int)(Math.Sin(((double)x) / 10.0) * 10.0));
            }
        }

    }
}

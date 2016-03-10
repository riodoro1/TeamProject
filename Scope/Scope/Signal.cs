using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scope
{
    public class SignalPoint
    {
        public double time;
        public double value;

        public SignalPoint(double time, double value)
        {
            this.time = time;
            this.value = value;
        }
    }
    public class Signal
    {
        #region Properties
        public SignalPoint[] Points;
        public Color Color;
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

        public Signal(SignalPoint[] points, Color color)
        {
            Color = color;
            Points = points;
        }

        public int StartIndexInsideInterval(double start, double end)
        {
            //TODO: more optimal solution assuming the Points array is sorted. (binary search maybe?)
            for (int i = 0; i < Points.Length; i++)
            {
                if (Points[i].time > start && Points[i].time < end)
                    return i;
            }
            return -1;
        }

    }
}

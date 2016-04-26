using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Scope
{
    public abstract class Signal
    {
        #region Properties
        public Point[] Points;
        public Color Color { get; set; }
        public string Name { get; set; }
        public Boolean Visible { get; set; } = true;
        public virtual string HorizontalUnit { get; }
        public virtual string VerticalUnit { get; protected set; }
        public Controls.SignalDisplay Display { get; set; } = null;
        #endregion

        #region Computed properties
        public double FirstXValue
        {
            get
            {
                return Points[0].X;
            }
        }

        public double LastXValue
        {
            get
            {
                return Points[Points.Length - 1].X;
            }
        }

        public double Duration
        {
            get
            {
                return LastXValue - FirstXValue;
            }
        }

        public double PeakToPeak
        {
            get
            {
                double minimum = double.PositiveInfinity;
                double maximum = double.NegativeInfinity;
                foreach (Point point in Points)
                {
                    if ( point.Y > maximum )
                        maximum = point.Y;
                    if (point.Y < minimum)
                        minimum = point.Y;
                }
                return maximum - minimum;
            }
        }

        public double XStep
        {
            get
            {
                return (FirstXValue - LastXValue) / Points.Length;
            }
        }
        #endregion

        public Signal(Point[] points, Color color, string name)
        {
            Color = color;
            Points = points;
            Name = name;
        }

        public int StartIndexInsideInterval(double start, double end)
        {
            //TODO: more optimal solution assuming the Points array is sorted. (binary search maybe?)
            for (int i = 0; i < Points.Length; i++)
            {
                if (Points[i].X >= start && Points[i].X <= end)
                    return i;
            }
            return -1;
        }

        public int FirstIndexBefore(double x)
        {
            if (Points[0].X > x)
                return -1;

            for (int i = 0; i < Points.Length - 1; i++)
            {
                if (Points[i + 1].X > x)
                    return i;
            }

            return -1;
        }

        public double? InterpolatedValueForX(double X)
        {
            int firstPointIndex = FirstIndexBefore(X);
            if (firstPointIndex == -1)
                return null;

            Point firstPoint = Points[firstPointIndex];
            if (firstPoint.X == X)
                return firstPoint.Y;

            int secondPointIndex = firstPointIndex + 1;
            if (secondPointIndex == Points.Length)
                return null;

            Point secondPoint = Points[secondPointIndex];

            double alpha = (secondPoint.X - X) / (secondPoint.X - firstPoint.X);
            double beta = (X - firstPoint.X) / (secondPoint.X - firstPoint.X);

            return alpha * firstPoint.Y + beta * secondPoint.Y;
        }
    }
}

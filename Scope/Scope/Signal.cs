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
        public String Name { get; set; }
        public Boolean Visible { get; set; } = true;
        public virtual String HorizontalUnit { get; }
        public virtual String VerticalUnit { get; protected set; }
        #endregion

        #region Computed properties
        public Double FirstXValue
        {
            get
            {
                return Points[0].X;
            }
        }

        public Double LastXValue
        {
            get
            {
                return Points[Points.Length - 1].X;
            }
        }

        public Double XStep
        {
            get
            {
                return (FirstXValue - LastXValue) / Points.Length;
            }
        }
        #endregion

        public Signal(Point[] points, Color color, String name)
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
                if (Points[i].X > start && Points[i].X < end)
                    return i;
            }
            return -1;
        }

    }
}

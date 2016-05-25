using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Scope
{
    public class TimeDomainSignal : Signal
    {
        public override string HorizontalUnit
        {
            get
            {
                return "s";
            }
        }

        public TimeDomainSignal(Point[] points, Color color, string name, string verticalUnit) : base(points, color, name)
        {
            VerticalUnit = verticalUnit;
        }

        public TimeDomainSignal(Point[] points, Color color, string name) : this(points, color, name, "V")
        { }

        public TimeDomainSignal(Point[] points, Color color) : this(points, color, "Unnamed signal", "V")
        { }

        public TimeDomainSignal(Point[] points) : this(points, Colors.Blue)
        { }
    }
}

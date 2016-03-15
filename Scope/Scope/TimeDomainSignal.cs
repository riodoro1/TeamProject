using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Scope
{
    class TimeDomainSignal : Signal
    {
        public override string HorizontalUnit
        {
            get
            {
                return "s";
            }
        }

        public TimeDomainSignal(Point[] points, Color color, String name, String verticalUnit) : base(points, color, name)
        {
            this.VerticalUnit = verticalUnit;
        }

        public TimeDomainSignal(Point[] points, Color color, String name) : this(points, color, name, "V")
        { }

        public TimeDomainSignal(Point[] points, Color color) : this(points, color, "Unnamed signal", "V")
        { }
    }
}

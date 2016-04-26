using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scope
{
    public abstract class SignalOperator
    {
        public virtual string Name { get; }
        public virtual int NumberOfOperands { get; }

        public abstract Signal Result(params Signal[] operands);
    }

    public class SignalSummator : SignalOperator
    {
        public override string Name { get {return "Sum"; } }
        public override int NumberOfOperands { get { return 2; } }

        public override Signal Result(params Signal[] operands)
        {
            if ( operands.Length != NumberOfOperands )
            {
                throw new InvalidOperationException();
            }

            Signal firstSignal = operands[0];
            Signal secondSignal = operands[1];

            Point[] resultingPoints = new Point[firstSignal.Points.Length];

            for ( int i = 0; i < firstSignal.Points.Length; i++ )
            {
                double x = firstSignal.Points[i].X;
                double y = firstSignal.Points[i].Y;

                double? add = secondSignal.InterpolatedValueForX(x);

                resultingPoints[i].X = x;
                resultingPoints[i].Y = y + ((add.HasValue) ? add.Value : 0.0);
            }

            return new TimeDomainSignal(resultingPoints);
        }
    }

}

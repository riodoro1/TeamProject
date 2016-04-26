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

        protected bool SortSignals(ref Signal signal1, ref Signal signal2)
        {
            if ( signal1.FirstXValue > signal2.FirstXValue )
            {
                Signal temp = signal1;
                signal1 = signal2;
                signal2 = temp;
                return true;
            }
            return false;
        }

        protected int NumberOfOverlappingPoints(Signal signal1, Signal signal2)
        {
            SortSignals(ref signal1, ref signal2);

            int counter = 0;
            while (counter < signal2.Points.Length && signal2.Points[counter].X <= signal1.LastXValue ) {
                counter++;
            }

            return counter;
        }
    }

    #region Two signal operators

    public abstract class TwoSignalOperator : SignalOperator
    {
        public override int NumberOfOperands { get { return 2; } }

        protected abstract double combine(double value1, double value2);

        public override Signal Result(params Signal[] operands)
        {
            if (operands.Length != NumberOfOperands)
            {
                throw new InvalidOperationException();
            }

            Signal firstSignal = operands[0];
            Signal secondSignal = operands[1];

            bool orderChanged = SortSignals(ref firstSignal, ref secondSignal); //first signal is earlier

            int overlappingPoints = NumberOfOverlappingPoints(firstSignal, secondSignal);

            if (overlappingPoints == 0)
                return null;

            Point[] resultingPoints = new Point[firstSignal.Points.Length + secondSignal.Points.Length - overlappingPoints];

            int i = 0;

            for (; i < firstSignal.Points.Length; i++)
            {
                double x = firstSignal.Points[i].X;
                double y = firstSignal.Points[i].Y;

                double? operand = secondSignal.InterpolatedValueForX(x);

                resultingPoints[i].X = x;
                if ( operand.HasValue )
                {
                    if (!orderChanged)
                        resultingPoints[i].Y = combine(y, operand.Value);
                    else
                        resultingPoints[i].Y = combine(operand.Value, y);
                }
                else
                {
                    resultingPoints[i].Y = y;
                }
            }

            int resultIndex = i;
            i = overlappingPoints;

            for (; i < secondSignal.Points.Length;)
            {
                double x = secondSignal.Points[i].X;
                double y = secondSignal.Points[i].Y;

                resultingPoints[resultIndex].X = x;
                resultingPoints[resultIndex].Y = y;

                i++;
                resultIndex++;
            }

            return new TimeDomainSignal(resultingPoints);
        }
    }

    public class SignalSummator : TwoSignalOperator
    {
        public override string Name { get { return "Sum"; } }

        protected override double combine(double value1, double value2)
        {
            return value1 + value2;
        }
    }

    public class SignalSubtractor : TwoSignalOperator
    {
        public override string Name { get { return "Subtract"; } }

        protected override double combine(double value1, double value2)
        {
            return value1 - value2;
        }
    }

    public class SignalMultiplier : TwoSignalOperator
    {
        public override string Name { get { return "Multiply"; } }

        protected override double combine(double value1, double value2)
        {
            return value1 * value2;
        }
    }

    public class SignalDivider : TwoSignalOperator
    {
        public override string Name { get { return "Divide"; } }

        protected override double combine(double value1, double value2)
        {
            return value1 / value2;
        }
    }
    #endregion

}

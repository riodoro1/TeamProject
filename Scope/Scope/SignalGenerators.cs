using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scope
{
    public abstract class SignalGenerator
    {
        public static int numberOfSamples = 10000;

        protected double frequency;
        protected double amplitude;
        protected double dcOffset;
        protected double dutyCycle;

        public SignalGenerator(double frequency, double amplitude, double dcOffset, double dutyCycle)
        {
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.dcOffset = dcOffset;
            this.dutyCycle = dutyCycle;
        }

        protected abstract double f(double time);

        public TimeDomainSignal GenerateSignal(Double startTime, Double duration)
        {
            Double deltaT = duration / numberOfSamples / 1000.0;

            Point[] signalPoints = new Point[numberOfSamples];

            for (int i = 0; i < numberOfSamples; i++)
            {
                Double time = i * deltaT;
                signalPoints[i] = new Point(time + startTime / 1000.0, f(time));
            }

            return new TimeDomainSignal(signalPoints);
        }
    }

    public class SineWaveGenerator : SignalGenerator
    {
        public SineWaveGenerator(double frequency, double amplitude, double dcOffset, double dutyCycle = 0.5) : base(frequency, amplitude, dcOffset, dutyCycle)
        { }

        protected override double f(double time)
        {
            return amplitude / 2.0 * Math.Sin(time * frequency * Math.PI * 2.0) + dcOffset;
        }
    }

    public class SquareWaveGenerator : SignalGenerator
    {
        public SquareWaveGenerator(double frequency, double amplitude, double dcOffset, double dutyCycle) : base(frequency, amplitude, dcOffset, dutyCycle)
        {
            if (dutyCycle < 0.0)
                dutyCycle = 0.0;
            else if (dutyCycle > 1.0)
                dutyCycle = 1.0;
        }

        protected override double f(double time)
        {
            double duration = 1.0 / frequency;
            double durationLow = duration * (1.0 - dutyCycle);
            return (time % duration <= durationLow) ? 0.0 + dcOffset : amplitude + dcOffset;
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scope.Controls
{
    public class SignalDisplay : Canvas
    {
        #region Properties
        public List<Signal> Signals { get; private set; }

        private int majorVerticalDivisions = 8;
        private int majorHorizontalDivisions
        {
            get
            {
                int calculated = (int)Math.Round(ActualWidth / ActualHeight * majorVerticalDivisions);
                if (calculated % 2 != 0)
                    calculated += 1;    //enforce that the number is even
                return Math.Min(Math.Max(calculated, 4), 50);
            }
        } //depending on width to height ratio

        public Double timePerDivision { get; set; } = 0.1d; //horizontal
        public Double unitsPerDivision { get; set; } = 1.0d; //vertical

        public Double StartTime { get; set; } = 0.0d;
        public Double EndTime
        {
            get
            {
                return StartTime + majorHorizontalDivisions * timePerDivision;
            }
        }
        public Color GraticuleColor { get; set; } = Colors.Gray;

        public Double MinimumTime
        {
            get
            {
                Double minimum = 0;
                foreach (Signal signal in Signals)
                {
                    if (signal.FirstXValue < minimum)
                        minimum = signal.FirstXValue;
                }
                return minimum;
            }
        }
        public Double MaximumTime
        {
            get
            {
                Double maximum = 0;
                foreach (Signal signal in Signals)
                {
                    if (signal.LastXValue > maximum)
                        maximum = signal.LastXValue;
                }
                return maximum;
            }
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty ScrollBarProperty = DependencyProperty.Register("ScrollBar", typeof(SignalScrollBar), typeof(SignalDisplay), new UIPropertyMetadata(null));
        public SignalScrollBar ScrollBar
        {
            set
            {
                SetValue(ScrollBarProperty, value);
            }
            get
            {
                return (SignalScrollBar)GetValue(ScrollBarProperty);
            }
        }
        #endregion

        #region Supporting methods
        private void UpdateScrollBar()
        {
            if (ScrollBar == null)
                return;
            if (OnScrollBarValueChangedHandler == null)
            {
                OnScrollBarValueChangedHandler = new RoutedPropertyChangedEventHandler<double>(OnScrollBarValueChanged);
                ScrollBar.ValueChanged += OnScrollBarValueChangedHandler;
            }

            Double viewPortSize = EndTime - StartTime;
            Double scrollBarMinimum = MinimumTime;
            Double scrollBarMaximum = Math.Max(MaximumTime, EndTime) - viewPortSize;
            Double scrollBarValue = StartTime;

            ScrollBar.Minimum = scrollBarMinimum;
            ScrollBar.Maximum = scrollBarMaximum;
            ScrollBar.ViewportSize = viewPortSize;
            ScrollBar.Value = scrollBarValue;
        }

        private RoutedPropertyChangedEventHandler<double> OnScrollBarValueChangedHandler = null;
        private void OnScrollBarValueChanged(Object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StartTime = (sender as SignalScrollBar).Value;
            this.InvalidateVisual();
        }
        #endregion

        static SignalDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalDisplay), new FrameworkPropertyMetadata(typeof(SignalDisplay)));
        }

        public SignalDisplay()
        {
            Signals = new List<Signal>();
        }

        public void AddSignal(Signal signal)
        {
            Signals.Add(signal);
            UpdateScrollBar();
            this.InvalidateVisual();
        }

        public Boolean RemoveSignal(Signal signal)
        {
            Boolean removed = Signals.Remove(signal);
            if (removed)
            {
                UpdateScrollBar();
                this.InvalidateVisual();
            }
            return removed;
        }

        public void ClearSignals()
        {
            Signals.Clear();
            UpdateScrollBar();
            this.InvalidateVisual();
        }

        private void DrawGraticule(DrawingContext dc)
        {
            double centerY = ActualHeight / 2.0;
            double centerX = ActualWidth / 2.0;
            double deltaY = ActualHeight / majorVerticalDivisions / 5.0;   //5 minor divisions
            double deltaX = ActualWidth / majorHorizontalDivisions / 5.0;   //5 minor divisions

            Pen graticulePen = new Pen(new SolidColorBrush(GraticuleColor), 0.4);

            double x = 0.0;
            while (x <= ActualWidth)
            {
                dc.DrawLine(graticulePen, new Point(x, 0.0), new Point(x, ActualHeight));
                x += deltaX;
                
                for (int i = 0; i < 4 && x<=ActualWidth; i++)
                {
                    dc.DrawLine(graticulePen, new Point(x, centerY - deltaY / 2.0), new Point(x, centerY + deltaY / 2.0));
                    x += deltaX;
                }
            }

            double y = 0.0;
            while (y <= ActualHeight)
            {
                dc.DrawLine(graticulePen, new Point(0.0, y), new Point(ActualWidth, y));
                y += deltaY;
                for (int i = 0; i < 4 && y <= ActualHeight; i++)
                {
                    dc.DrawLine(graticulePen, new Point(centerX - deltaX / 2.0, y), new Point(centerX + deltaX / 2.0, y));
                    y += deltaY;
                }
            }
        }

        private Point TransformPoint(Point point)
        {
            double x = ((point.X - StartTime) * ActualWidth) / (EndTime - StartTime);
            double y = (ActualHeight / 2) - (point.Y * ActualHeight / majorVerticalDivisions);
            return new Point(x, y);
        }

        private void DrawSignals(DrawingContext dc)
        {
            if (Signals.Count == 0)
                return;
            
            foreach (Signal signal in Signals)
            {
                if (!signal.Visible)
                    continue;

                int startIndex = signal.StartIndexInsideInterval(StartTime, EndTime);
                if (startIndex == -1)
                    continue;   //signal does not occupy the screen

                PathGeometry path = new PathGeometry();
                PathFigure figure = new PathFigure();
                Brush signalBrush = new SolidColorBrush(signal.Color);

                figure.StartPoint = TransformPoint(signal.Points[startIndex]);

                for (int i = startIndex + 1; i < signal.Points.Length && signal.Points[i].X < EndTime; i++)
                {
                    figure.Segments.Add(new LineSegment(TransformPoint(signal.Points[i]), true));
                }

                path.Figures.Add(figure);
                dc.DrawGeometry(null, new Pen(signalBrush, 1), path);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            DrawSignals(dc);
            DrawGraticule(dc);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateScrollBar();
        }
    }
}
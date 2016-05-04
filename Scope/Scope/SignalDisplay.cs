
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Signal> Signals { get; private set; }
        public SignalScrollBar ScrollBar
        {
            get
            {
                return (SignalScrollBar)GetValue(ScrollBarProperty);
            }
            set
            {
                SetValue(ScrollBarProperty, value);
            }
        }
        public ListBox SignalsListBox
        {
            get
            {
                return (ListBox)GetValue(SignalsListBoxProperty);
            }
            set
            {
                SetValue(SignalsListBoxProperty, value);
            }
        }
        public SignalDisplayInfoBar InfoBar
        {
            get
            {
                return (SignalDisplayInfoBar)GetValue(InfoBarProperty);
            }
            set
            {
                SetValue(InfoBarProperty, value);
            }
        }
        public TimePerDivisionSelector TimePerDivisionSelector
        {
            get
            {
                return (TimePerDivisionSelector)GetValue(TimePerDivSelectorProperty);
            }
            set
            {
                SetValue(TimePerDivSelectorProperty, value);
            }
        }

        public int majorVerticalDivisions = 8;
        public int majorHorizontalDivisions
        {
            get
            {
                int calculated = (int)Math.Round(ActualWidth / ActualHeight * majorVerticalDivisions);
                if (calculated % 2 != 0)
                    calculated += 1;    //enforce that the number is even
                return Math.Min(Math.Max(calculated, 4), 50);
            }
        } //depending on width to height ratio

        public double timePerDivision { get; set; } = 1.0d; //horizontal
        public double unitsPerDivision { get; set; } = 1.0d; //vertical

        public double TimeSpan {
            get
            {
                return timePerDivision * majorHorizontalDivisions;
            }
            set
            {
                timePerDivision = value / majorHorizontalDivisions;
                RefreshDisplay();
            }
        }

        public double StartTime { get; set; } = 0.0d;
        public double EndTime
        {
            get
            {
                return StartTime + majorHorizontalDivisions * timePerDivision;
            }
        }
        public Color GraticuleColor { get; set; } = Colors.Gray;

        public double MinimumTime
        {
            get
            {
                double minimum = 0;
                foreach (Signal signal in Signals)
                {
                    if (signal.FirstXValue < minimum)
                        minimum = signal.FirstXValue;
                }
                return minimum;
            }
        }
        public double MaximumTime
        {
            get
            {
                double maximum = 0;
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
        public static readonly DependencyProperty SignalsListBoxProperty = DependencyProperty.Register("SignalsListBox", typeof(ListBox), typeof(SignalDisplay), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ScrollBarProperty = DependencyProperty.Register("ScrollBar", typeof(SignalScrollBar), typeof(SignalDisplay), new PropertyMetadata(OnScrollBarPropertyChanged));
        public static readonly DependencyProperty InfoBarProperty = DependencyProperty.Register("InfoBar", typeof(SignalDisplayInfoBar), typeof(SignalDisplay), new UIPropertyMetadata(null));
        public static readonly DependencyProperty TimePerDivSelectorProperty = DependencyProperty.Register("TimePerDivisionSelector", typeof(TimePerDivisionSelector), typeof(SignalDisplay), new PropertyMetadata(OnTimePerDivSelectorPropertyChanged));
        #endregion

        #region External connection methods
        private void UpdateScrollBar()
        {
            double viewPortSize = EndTime - StartTime;
            double scrollBarMinimum = MinimumTime;
            double scrollBarMaximum = Math.Max(MaximumTime, EndTime) - viewPortSize;
            double scrollBarValue = StartTime;

            ScrollBar.Minimum = scrollBarMinimum;
            ScrollBar.Maximum = scrollBarMaximum;
            ScrollBar.ViewportSize = viewPortSize;
            ScrollBar.Value = scrollBarValue;
        }
        private static void OnScrollBarPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SignalDisplay disp = (obj as SignalDisplay);
            disp.ScrollBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(disp.OnScrollBarValueChanged);
        }
        private void OnScrollBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StartTime = (sender as SignalScrollBar).Value;
            RefreshDisplay();
        }

        private static void OnTimePerDivSelectorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SignalDisplay disp = (obj as SignalDisplay);
            disp.TimePerDivisionSelector.ValueChanged += new RoutedPropertyChangedEventHandler<double>(disp.OnTimePerDivisionValueChanged);
        }
        private void OnTimePerDivisionValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timePerDivision = e.NewValue;
            RefreshDisplay();
        }
  
        
        private void UpdateInfoBar()
        {
            InfoBar.TimePerDivision.Content = (new Quantity(timePerDivision, "s/div")).ToString();
        }

        public void RefreshDisplay()
        {
            InvalidateVisual();
            UpdateScrollBar();
            UpdateInfoBar();
        }
        #endregion

        #region Constructors
        static SignalDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalDisplay), new FrameworkPropertyMetadata(typeof(SignalDisplay)));
        }

        public SignalDisplay()
        {
            Signals = new ObservableCollection<Signal>();
        }
        #endregion

        #region Data manipulation methods
        public void AddSignal(Signal signal)
        {
            Signals.Add(signal);
            signal.Display = this;
            RefreshDisplay();
        }

        public Boolean RemoveSignal(Signal signal)
        {
            Boolean removed = Signals.Remove(signal);
            if (removed)
            {
                RefreshDisplay();
            }
            return removed;
        }

        public void ClearSignals()
        {
            Signals.Clear();
            RefreshDisplay();
        }
        #endregion

        #region Drawing methods
        private void DrawGraticule(DrawingContext dc)
        {
            double centerY = ActualHeight / 2.0;
            double centerX = ActualWidth / 2.0;
            double deltaY = ActualHeight / majorVerticalDivisions / 5.0;   //5 minor divisions
            double deltaX = ActualWidth / majorHorizontalDivisions / 5.0;   //5 minor divisions

            Pen graticulePen = new Pen(new SolidColorBrush(GraticuleColor), 0.5);

            double x = 0.0;
            while (x <= ActualWidth + 1) //+1 because the grid was not always drawn.
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
            while (y <= ActualHeight + 1) //+1 because the grid was not always drawn.
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

        private Point TransformPoint(Point point, double verticalScale)
        {
            double x = point.X;
            double y = point.Y;

            if (double.IsNaN(y))
                y = 1;

            x = ((x - StartTime) * ActualWidth) / (EndTime - StartTime);
            y = (ActualHeight / 2) - (y * ActualHeight / majorVerticalDivisions) * verticalScale;

            if (y > ActualHeight)
                y = ActualHeight;
            else if (y < 0)
                y = 0;

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

                if (startIndex == -1) //signal does not occupy the screen
                    continue;   

                PathGeometry path = new PathGeometry();
                PathFigure figure = new PathFigure();
                Brush signalBrush = new SolidColorBrush(signal.Color);

                Point startingPoint = TransformPoint(signal.Points[startIndex], signal.VerticalScale);

                if (startIndex > 0 && startingPoint.X > 1) //first point is too far away from left edge
                {
                    startingPoint = TransformPoint(new Point(StartTime, signal.InterpolatedValueForX(StartTime).Value), signal.VerticalScale);
                } 

                figure.StartPoint = startingPoint;

                double lastDrawnX = -1.0;
                double lastDrawnY = 0.0;
                for (int i = startIndex + 1; i < signal.Points.Length; i++)
                {
                    if (signal.Points[i].X > EndTime)
                    {
                        Point lastPoint = TransformPoint(signal.Points[i - 1], signal.VerticalScale);
                        if (lastPoint.X < ActualWidth - 1)  //last point is too far away from right edge
                        {
                            lastPoint = TransformPoint(new Point(EndTime, signal.InterpolatedValueForX(EndTime).Value), signal.VerticalScale);
                            figure.Segments.Add(new LineSegment(lastPoint, true));
                        }
                        break;
                    }

                    Point point = TransformPoint(signal.Points[i], signal.VerticalScale);
                    if ( point.X - lastDrawnX < 0.5 && Math.Abs(point.Y - lastDrawnY) < 0.5 )
                    {
                        continue;   //do not draw a point if it is very close to the predecessor
                    }

                    lastDrawnY = point.Y;
                    lastDrawnX = point.X;
                    figure.Segments.Add(new LineSegment(point, true));
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
        #endregion
    }
}
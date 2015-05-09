using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace ChrumGraph
{
    /// <summary>
    /// Represents graph viewing window which determines what
    /// part of graph is being shown.
    /// </summary>
    public class ViewWindow
    {
        private Point startPoint;
        private Visual visual;

        //for zooming
        private const double
            zoomingTime = 0.15, //zooming time in seconds
            zoomFactor = 0.5;
        private bool zooming = false;
        private double desiredScaleFactor, v0 = 0.0;
        private double[] c = {0, 0, 0, 0};
        private DateTime zoomChangedTime;
        private Point constantPointCore, constantPointVisual;

        /// <summary>
        /// Initializes a new instance of the ViewWindow class.
        /// </summary>
        /// <param name="visual"></param>
        public ViewWindow(Visual visual)
        {
            this.visual = visual;
            Static = false;
            MarginLength = 10.0;
            startPoint = new Point(0.0, 0.0);
            ScaleFactor = 1.0;
        }

        /// <summary>
        /// Scale factor of Visual graph relative to Core.
        /// </summary>
        public double ScaleFactor { get; set; }

        /// <summary>
        /// Adjusts the viewing field.
        /// </summary>
        public void Adjust()
        {
            lock (this)
            {
                if (visual.Core.IsEmpty()) return;
                updateZoom();
                if (Static) return;

                double xMin, xMax, yMin, yMax, coreWidth, coreHeight;
                xMin = yMin = double.PositiveInfinity;
                xMax = yMax = double.NegativeInfinity;

                foreach (Vertex v in visual.Core.Vertices)
                {
                    xMin = Math.Min(xMin, v.X);
                    xMax = Math.Max(xMax, v.X);
                    yMin = Math.Min(yMin, v.Y);
                    yMax = Math.Max(yMax, v.Y);
                }

                coreWidth = xMax - xMin;
                coreHeight = yMax - yMin;
                ScaleFactor = Math.Min(
                    (Canvas.ActualWidth - 2.0 * MarginLength) / coreWidth,
                    (Canvas.ActualHeight - 2.0 * MarginLength) / coreHeight);

                Point coreMiddle, visualMiddle;
                coreMiddle = new Point(
                    (xMin + xMax) / 2.0,
                    (yMin + yMax) / 2.0);
                visualMiddle = new Point(
                    Canvas.ActualWidth / 2.0,
                    Canvas.ActualHeight / 2.0);
                setConstraint(coreMiddle, visualMiddle);
            }
        }

        private void setConstraint(Point corePosition, Point visualPosition)
        {
            startPoint = new Point(
                corePosition.X - visualPosition.X / ScaleFactor,
                corePosition.Y - visualPosition.Y / ScaleFactor);
        }

        /// <summary>
        /// Initiates zooming of a graph.
        /// </summary>
        /// <param name="delta">Magnitude of zooming. Positive value - zoom in, negative - zoom out</param>
        /// <param name="constantPoint">Represents Core position of a point that should remain constant on a canvas</param>
        public void SetZoom(double delta, Point constantPoint)
        {
            lock (this)
            {
                Static = true;
                zooming = true;
                v0 = getVelocity();
                zoomChangedTime = DateTime.Now;

                desiredScaleFactor = ScaleFactor * Math.Exp(delta * zoomFactor);
                double scaleFactorDelta = desiredScaleFactor - ScaleFactor;

                c[0] = (v0 * zoomingTime - 2 * scaleFactorDelta) / Math.Pow(zoomingTime, 3);
                c[1] = (3 * scaleFactorDelta - 2 * v0 * zoomingTime) / Math.Pow(zoomingTime, 2);
                c[2] = v0;
                c[3] = ScaleFactor;

                constantPointCore = constantPoint;
                constantPointVisual = CoreToVisualPosition(constantPoint);
            }
        }

        private void updateZoom()
        {
            if (zooming)
            {
                double t = getTimeDelta();
                ScaleFactor = getScaleFactor(t);
                if (t >= zoomingTime) zooming = false;
                setConstraint(constantPointCore, constantPointVisual);
            }
        }
        
        private double getTimeDelta()
        {
            TimeSpan timeDelta = DateTime.Now - zoomChangedTime;
            return (double)(timeDelta.Seconds + timeDelta.Milliseconds / 1000.0);
        }

        private double getScaleFactor(double t)
        {
            double scaleFactor = 0.0;
            for (int i = 0; i <= 3; i++)
            {
                scaleFactor = scaleFactor * t + c[i];
            }
            if (t >= zoomingTime) return desiredScaleFactor;
            return scaleFactor;
        }

        private double getVelocity()
        {
            return getVelocity(getTimeDelta());
        }

        private double getVelocity(double t)
        {
            double v = 0.0;
            for (int i = 0; i <= 2; i++)
            {
                v = v * t + (3.0 - i) * c[i];
            }
            if (t >= zoomingTime) return 0.0;
            return v;
        }

        public double Width { get; set; }
        public double Height { get; set; }

        /// <summary>
        /// Converts Core position to Visual position.
        /// </summary>
        /// <param name="corePosition">Core position to be converted</param>
        /// <returns>Visual position</returns>
        public Point CoreToVisualPosition(Point corePosition)
        {
            return new Point(
                (corePosition.X - startPoint.X) * ScaleFactor,
                (corePosition.Y - startPoint.Y) * ScaleFactor);
        }

        /// <summary>
        /// Converts Visual position to Core position.
        /// </summary>
        /// <param name="visualPosition">Visual position to be converted</param>
        /// <returns>Core position</returns>
        public Point VisualToCorePosition(Point visualPosition)
        {
            return new Point(
                startPoint.X + visualPosition.X / ScaleFactor,
                startPoint.Y + visualPosition.Y / ScaleFactor);
        }

        /// <summary>
        /// Shifts viewing windows by a specified vector.
        /// </summary>
        /// <param name="shift">Core shift</param>
        public void Shift(Vector shift)
        {
            startPoint = startPoint - shift;
        }

        /// <summary>
        /// Canvas on which the graph is drawn.
        /// </summary>
        public Canvas Canvas { get; set; }

        /// <summary>
        /// Specifies whether the viewing window should be adjusted or not.
        /// </summary>
        public bool Static { get; set; }// = false; <-- works only in C# 6.0

        /// <summary>
        /// Core length of margin.
        /// </summary>
        public double MarginLength { get; set; }
    }
}

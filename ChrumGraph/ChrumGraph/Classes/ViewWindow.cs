using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            if (Static || visual.Core.Vertices.Count == 0) return;

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

            Point coreMiddle, visualMiddle, visualStartPoint;
            coreMiddle = new Point(
                (xMin + xMax) / 2.0,
                (yMin + yMax) / 2.0);
            visualMiddle = CoreToVisualPosition(coreMiddle);
            visualStartPoint = new Point(
                visualMiddle.X - Canvas.ActualWidth / 2.0,
                visualMiddle.Y - Canvas.ActualHeight / 2.0);
            startPoint = VisualToCorePosition(visualStartPoint);
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

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
        private double scaleFactor, width, height;
        private Visual visual;

        /// <summary>
        /// Initializes a new instance of the ViewWindow class.
        /// </summary>
        /// <param name="visual"></param>
        public ViewWindow(Visual visual)
        {
            this.visual = visual;
            Static = false;
            MarginLength = 5.0;
        }

        /// <summary>
        /// Adjusts the viewing field.
        /// </summary>
        public void Adjust()
        {
            if (Static) Static = false;

            double xMin, xMax, yMin, yMax;
            xMin = yMin = double.PositiveInfinity;
            xMax = yMax = double.NegativeInfinity;

            foreach (Vertex v in visual.Core.Vertices)
            {
                xMin = Math.Min(xMin, v.X);
                xMax = Math.Max(xMax, v.X);
                yMin = Math.Min(yMin, v.Y);
                yMax = Math.Max(yMax, v.Y);
            }

            width = xMax - xMin + 2 * MarginLength;
            height = yMax - yMin + 2 * MarginLength;
            xMin -= MarginLength;
            yMin -= MarginLength;
            scaleFactor = Math.Min(Canvas.ActualWidth / width, Canvas.ActualHeight / height);

            double canvasRatio, coreRatio;
            canvasRatio = Canvas.ActualWidth / Canvas.ActualHeight;
            coreRatio = width / height;
            if (canvasRatio < coreRatio)
            {
                yMin += (height - Canvas.ActualHeight / scaleFactor) / 2.0;
            }
            else if (canvasRatio > coreRatio)
            {
                xMin += (width - Canvas.ActualWidth / scaleFactor) / 2.0;
            }

            startPoint = new Point(xMin, yMin);
        }

        /// <summary>
        /// Converts Core position to Visual position.
        /// </summary>
        /// <param name="corePosition">Core position to be converted</param>
        /// <returns>Visual position</returns>
        public Point CoreToVisualPosition(Point corePosition)
        {
            return new Point(
                (corePosition.X - startPoint.X) * scaleFactor,
                (corePosition.Y - startPoint.Y) * scaleFactor);
        }

        /// <summary>
        /// Converts Visual position to Core position.
        /// </summary>
        /// <param name="visualPosition">Visual position to be converted</param>
        /// <returns>Core position</returns>
        public Point VisualToCorePosition(Point visualPosition)
        {
            return new Point(
                startPoint.X + visualPosition.X / scaleFactor,
                startPoint.Y + visualPosition.Y / scaleFactor);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace ChrumGraph
{
    public partial class Vertex
    {
        public Ellipse Ellipse { get; set; }

        public TextBlock VisualLabel { get; set; }
    }

    public partial class Edge
    {
        public Line Line { get; set; }
    }

    /// <summary>
    /// Visual representation of a graph.
    /// </summary>
    /*public */class Visual : IVisual
    {
        private double scaleFactor = 0.01;
        private int vertexSize = 25;
        private double verticeToEdgeRatio = 5;
        private Canvas canvas;

        private Color vertexColor = Colors.Red;
        private Color edgeColor = Colors.Black;
        private SolidColorBrush vertexBrush;
        private SolidColorBrush edgeBrush;

        private Dictionary<TextBlock, Vertex> TextBlockToVertexDict = new Dictionary<TextBlock, Vertex>();
        private Vertex clickedVertex;
        private Point previousMousePosition;

        public Visual(Canvas _canvas)
        {
            canvas = _canvas;
            vertexBrush = new SolidColorBrush(vertexColor);
            edgeBrush = new SolidColorBrush(edgeColor);

            canvas.MouseUp += (sender, e) => { clickedVertex = null; };
            canvas.MouseMove += MouseMove;
        }

        public IVisualCore Parent { get; set; }

        public int VertexSize
        {
            get { return vertexSize; }
            set { vertexSize = value; }
        }

        public Ellipse getVisualVertex()
        {
            Ellipse e = new Ellipse();
            e.Height = e.Width = vertexSize;
            e.Fill = vertexBrush;
            return e;
        }

        private double translateCoordHor(double x)
        {
            return x + canvas.Width / 2;
        }

        private double translateCoordVert(double y)
        {
            return y + canvas.Height / 2;
        }

        public void CreateVisualVertex(Vertex v)
        {
            Ellipse e = new Ellipse();
            e.Height = e.Width = vertexSize;
            e.Fill = vertexBrush;
            canvas.Children.Add(e);
            Canvas.SetZIndex(e, 2);
            v.Ellipse = e;

            TextBlock t = new TextBlock();
            t.FontWeight = FontWeights.Bold;
            t.HorizontalAlignment = HorizontalAlignment.Center;
            t.VerticalAlignment = VerticalAlignment.Center;
            t.Width = t.Height = vertexSize;
            t.FontSize = vertexSize * 0.75;

            t.Text = v.Label;
            t.Margin = new Thickness(vertexSize / 4, 0, 0, vertexSize / 10);
            canvas.Children.Add(t);
            Canvas.SetZIndex(t, 3);
            v.VisualLabel = t;

            TextBlockToVertexDict.Add(t, v);

            t.MouseLeftButtonDown += MouseDown;
            t.MouseLeftButtonUp += MouseUp;
            t.MouseMove += MouseMove;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            clickedVertex = TextBlockToVertexDict[textBlock];
            clickedVertex.Clicked = true;
            previousMousePosition = e.GetPosition(canvas);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (clickedVertex != null)
            {
                clickedVertex.Clicked = false;
                clickedVertex = null;
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (clickedVertex == null) return;

            Point mousePosition = e.GetPosition(canvas);
            Point coreShift = (Point)(VisualToCorePosition(mousePosition) - VisualToCorePosition(previousMousePosition));
            clickedVertex.X += coreShift.X;
            clickedVertex.Y += coreShift.Y;
            previousMousePosition = mousePosition;
        }

        public void RemoveVisualVertex(Vertex v)
        {
            canvas.Children.Remove(v.Ellipse);
            canvas.Children.Remove(v.VisualLabel);
            v.Ellipse = null;
            v.VisualLabel = null;
        }
        public void CreateVisualEdge(Edge e)
        {
            Line l = new Line();
            l.Stroke = edgeBrush;
            l.StrokeThickness = vertexSize / verticeToEdgeRatio;
            canvas.Children.Add(l);
            Canvas.SetZIndex(l, 1);
            e.Line = l;
        }
        public void RemoveVisualEdge(Edge e)
        {
            canvas.Children.Remove(e.Line);
            e.Line = null;
        }

        public bool Visible { get; set; }

        private Point CoreToVisualPosition(Point corePosition)
        {
            return new Point(
                (scaleFactor * corePosition.X + 1.0) * canvas.ActualWidth / 2.0 - vertexSize / 2.0,
                (scaleFactor * corePosition.Y + 1.0) * canvas.ActualHeight / 2.0 - vertexSize / 2.0);
        }

        private Point VisualToCorePosition(Point visualPosition)
        {
            return new Point(
                (2.0 / canvas.ActualWidth * (visualPosition.X + vertexSize / 2.0) - 1) / scaleFactor,
                (2.0 / canvas.ActualHeight * (visualPosition.Y + vertexSize / 2.0) - 1) / scaleFactor);
        }

        public void Refresh()
        {
            if (!Visible) Visible = true;

            double xMin = Double.PositiveInfinity, xMax = Double.NegativeInfinity,
                yMin = Double.PositiveInfinity, yMax = Double.NegativeInfinity;

            foreach (Vertex v in Parent.Vertices)
            {
                xMin = Math.Min(xMin, v.X);
                xMax = Math.Max(xMax, v.X);
                yMin = Math.Min(yMin, v.Y);
                yMax = Math.Max(yMax, v.Y);
            }

            double delta = Math.Max(xMax - xMin, yMax - yMin);
            scaleFactor = 1.0 / delta;
            
            foreach(Vertex v in Parent.Vertices)
            {
                Ellipse e = v.Ellipse;
                try
                {
                    Point corePosition = new Point(v.X, v.Y);
                    Point visualPosition = CoreToVisualPosition(corePosition);
                    Canvas.SetLeft(e, visualPosition.X);
                    Canvas.SetTop(e, visualPosition.Y);
                }
                catch (ArgumentException) { }
            }

            foreach(Edge e in Parent.Edges)
            {
                Ellipse e1 = e.V1.Ellipse;
                Ellipse e2 = e.V2.Ellipse;
                Line l = e.Line;
                try
                {
                    l.X1 = Canvas.GetLeft(e1) + vertexSize / 2;
                    l.Y1 = Canvas.GetTop(e1) + vertexSize / 2;
                    l.X2 = Canvas.GetLeft(e2) + vertexSize / 2;
                    l.Y2 = Canvas.GetTop(e2) + vertexSize / 2;
                }
                catch (ArgumentException) { }
            }

            foreach(Vertex v in Parent.Vertices)
            {
                Ellipse e = v.Ellipse;
                e.HorizontalAlignment = HorizontalAlignment.Center;
                e.VerticalAlignment = VerticalAlignment.Center;

                TextBlock t = v.VisualLabel;

                Canvas.SetLeft(t, (scaleFactor * v.X + 1) * canvas.ActualWidth / 2 - vertexSize / 2);
                Canvas.SetTop(t, (scaleFactor * v.Y + 1) * canvas.ActualHeight / 2 - vertexSize / 2);
            }
        }

        public void Clear()
        {
            foreach (Vertex v in Parent.Vertices)
            {
                RemoveVisualVertex(v);
            }
            foreach (Edge e in Parent.Edges)
            {
                RemoveVisualEdge(e);
            }
        }
    }
}

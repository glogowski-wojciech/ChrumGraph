using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ChrumGraph
{
    /// <summary>
    /// Visual representation of a graph.
    /// </summary>
    class Visual : IVisual
    {
        private bool visible;
        private double scaleFactor = 0.01;
        private int verticeSize = 25;
        private double verticeToEdgeRatio = 5;
        private Canvas canvas;
        private Core parent;

        private Color vertexColor = Colors.Red;
        private Color edgeColor = Colors.Black;
        private SolidColorBrush vertexBrush;
        private SolidColorBrush edgeBrush;


        private List<Ellipse> ellipses = new List<Ellipse>();
        private List<Line> lines = new List<Line>();
        private Dictionary<string, Ellipse> ellipseLabels = new Dictionary<string, Ellipse>();

        public Visual(Canvas _canvas)
        {
            canvas = _canvas;
            vertexBrush = new SolidColorBrush(vertexColor);
            edgeBrush = new SolidColorBrush(edgeColor);
        }

        public Core Parent
        {
            set { parent = value; }
        }

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
            //vertices.Add(v);
        }
        public void RemoveVisualVertex(Vertex v)
        {
            //vertices.Remove(v);
        }
        public void CreateVisualEdge(Edge e)
        {
            //edges.Add(e);
        }
        public void RemoveVisualEdge(Edge e)
        {
            //edges.Remove(e);
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        public void Refresh()
        {
            if (!visible)
                visible = true;

            canvas.Children.Clear();
            ellipses.Clear();
            lines.Clear();
            ellipseLabels.Clear();

            double xMin = Double.PositiveInfinity, xMax = Double.NegativeInfinity,
                yMin = Double.PositiveInfinity, yMax = Double.NegativeInfinity;

            foreach (Vertex v in parent.Vertices)
            {
                xMin = Math.Min(xMin, v.X);
                xMax = Math.Max(xMax, v.X);
                yMin = Math.Min(yMin, v.Y);
                yMax = Math.Max(yMax, v.Y);
            }

            double delta = Math.Max(xMax - xMin, yMax - yMin);
            scaleFactor = 1.0 / delta;
            
            foreach(Vertex v in parent.Vertices)
            {
                Ellipse e = getVisualVertex();
                try
                {
                    Canvas.SetLeft(e, (scaleFactor * v.X + 1) * canvas.ActualWidth / 2 - verticeSize / 2);
                    Canvas.SetTop(e, (scaleFactor * v.Y + 1) * canvas.ActualHeight / 2 - verticeSize / 2);
                }
                catch (ArgumentException) { }
                ellipses.Add(e);
                ellipseLabels.Add(v.Label, e);
            }

            foreach(Edge e in parent.Edges)
            {
                Ellipse e1 = ellipseLabels[e.V1.Label];
                Ellipse e2 = ellipseLabels[e.V2.Label];
                Line l = new Line();
                l.Stroke = edgeBrush;
                l.StrokeThickness = verticeSize / verticeToEdgeRatio;
                try
                {
                    l.X1 = Canvas.GetLeft(e1) + verticeSize / 2;
                    l.Y1 = Canvas.GetTop(e1) + verticeSize / 2;
                    l.X2 = Canvas.GetLeft(e2) + verticeSize / 2;
                    l.Y2 = Canvas.GetTop(e2) + verticeSize / 2;
                }
                catch (ArgumentException) { }
                canvas.Children.Add(l);
            }

            foreach(Vertex v in parent.Vertices)
            {
                Ellipse e = ellipseLabels[v.Label];
                e.HorizontalAlignment = HorizontalAlignment.Center;
                e.VerticalAlignment = VerticalAlignment.Center;
                Grid g = new Grid();

                TextBlock t = new TextBlock();
                t.FontWeight = FontWeights.Bold;
                t.HorizontalAlignment = HorizontalAlignment.Center;
                t.VerticalAlignment = VerticalAlignment.Center;
                t.Width = t.Height = vertexSize;
                t.FontSize = vertexSize * 0.75;

                t.Text = v.Label;
                t.Margin = new Thickness(vertexSize / 4, 0, 0, vertexSize / 10);

                Canvas.SetLeft(t, (scaleFactor * v.X + 1) * canvas.ActualWidth / 2 - vertexSize / 2);
                Canvas.SetTop(t, (scaleFactor * v.Y + 1) * canvas.ActualHeight / 2 - vertexSize / 2);
                canvas.Children.Add(e);
                canvas.Children.Add(t);
            }
        }
    }
}

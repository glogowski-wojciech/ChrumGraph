using System;
using System.Collections.Generic;
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
    public class Visual : IVisual
    {
        private double verticeToEdgeRatio = 5;
        private Canvas canvas;

        private Color vertexColor = Colors.Red;
        private Color edgeColor = Colors.Black;
        private SolidColorBrush vertexBrush;
        private SolidColorBrush edgeBrush;

        private Dictionary<UIElement, Vertex> VertexDict = new Dictionary<UIElement, Vertex>();
        private Vertex clickedVertex;
        private Point previousMousePosition;

        public Visual(MainWindow mainWindow)
        {
            VertexSize = 25.0;

            canvas = mainWindow.MainCanvas;
            
            vertexBrush = new SolidColorBrush(vertexColor);
            edgeBrush = new SolidColorBrush(edgeColor);
            ViewWindow = new ViewWindow(this);
            ViewWindow.Canvas = canvas;

            canvas.MouseUp += MouseLeft;
            canvas.MouseMove += MouseMove;
            canvas.MouseLeave += MouseLeft;
        }

        public IVisualCore Core { get; set; }

        public ViewWindow ViewWindow { get; set; }

        public double VertexSize { get; set; }

        public Ellipse getVisualVertex()
        {
            Ellipse e = new Ellipse();
            e.Height = e.Width = VertexSize;
            e.Fill = vertexBrush;
            return e;
        }

        private double translateCoordHor(double x)
        {
            return x + canvas.Width / 2.0;
        }

        private double translateCoordVert(double y)
        {
            return y + canvas.Height / 2.0;
        }

        private void AddEventHandlers(UIElement element, Vertex v)
        {
            VertexDict.Add(element, v);

            element.MouseLeftButtonDown += MouseDown;
            element.MouseLeftButtonUp += MouseLeft;
            element.MouseMove += MouseMove;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            UIElement element = sender as UIElement;
            clickedVertex = VertexDict[element];
            if (clickedVertex != null)
            {
                clickedVertex.Clicked = true;
                previousMousePosition = e.GetPosition(canvas);

                ViewWindow.Static = true;
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (clickedVertex == null) return;

            Point mousePosition = e.GetPosition(canvas);
            Point coreShift = (Point)(ViewWindow.VisualToCorePosition(mousePosition) -
                ViewWindow.VisualToCorePosition(previousMousePosition));
            clickedVertex.X += coreShift.X;
            clickedVertex.Y += coreShift.Y;
            previousMousePosition = mousePosition;
        }

        private void MouseLeft(object sender, MouseEventArgs e)
        {
            if (clickedVertex != null)
            {
                clickedVertex.Clicked = false;
                clickedVertex = null;
            }
        }

        public void CreateVisualVertex(Vertex vertex)
        {
            Ellipse e = new Ellipse
            {
                Height = VertexSize,
                Width = VertexSize,
                Fill = vertexBrush,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            canvas.Children.Add(e);
            Canvas.SetZIndex(e, 2);

            AddEventHandlers(e, vertex);
            vertex.Ellipse = e;

            TextBlock t = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = VertexSize,
                Height = VertexSize,
                FontSize = VertexSize * 0.75,
                Text = vertex.Label,
                Margin = new Thickness(VertexSize / 4, 0, 0, VertexSize / 10.0),
            };
            canvas.Children.Add(t);
            Canvas.SetZIndex(t, 3);

            AddEventHandlers(t, vertex);
            vertex.VisualLabel = t;
        }

        public void RemoveVisualVertex(Vertex vertex)
        {
            VertexDict.Remove(vertex.Ellipse);
            VertexDict.Remove(vertex.VisualLabel);

            canvas.Children.Remove(vertex.Ellipse);
            canvas.Children.Remove(vertex.VisualLabel);
            vertex.Ellipse = null;
            vertex.VisualLabel = null;
        }
        public void CreateVisualEdge(Edge edge)
        {
            Line l = new Line
            {
                Stroke = edgeBrush,
                StrokeThickness = VertexSize / verticeToEdgeRatio,
            };
            canvas.Children.Add(l);
            Canvas.SetZIndex(l, 1);
            edge.Line = l;
        }
        public void RemoveVisualEdge(Edge edge)
        {
            canvas.Children.Remove(edge.Line);
            edge.Line = null;
        }

        public bool Visible { get; set; }

        public void Refresh()
        {
            if (!Visible) Visible = true;

            if (!ViewWindow.Static) ViewWindow.Adjust();
            
            foreach(Vertex v in Core.Vertices)
            {
                Point visualPosition = ViewWindow.CoreToVisualPosition(v.Position);
                Canvas.SetLeft(v.Ellipse, visualPosition.X - VertexSize / 2.0);
                Canvas.SetTop(v.Ellipse, visualPosition.Y - VertexSize / 2.0);

                Canvas.SetLeft(v.VisualLabel, visualPosition.X - VertexSize / 2.0);
                Canvas.SetTop(v.VisualLabel, visualPosition.Y - VertexSize / 2.0);
            }

            foreach(Edge e in Core.Edges)
            {
                e.Line.X1 = Canvas.GetLeft(e.V1.Ellipse) + VertexSize / 2.0;
                e.Line.Y1 = Canvas.GetTop(e.V1.Ellipse) + VertexSize / 2.0;
                e.Line.X2 = Canvas.GetLeft(e.V2.Ellipse) + VertexSize / 2.0;
                e.Line.Y2 = Canvas.GetTop(e.V2.Ellipse) + VertexSize / 2.0;
            }
        }

        /// <summary>
        /// Removes all visual vertices and edges.
        /// </summary>
        public void Clear()
        {
            foreach (Vertex v in Core.Vertices)
            {
                RemoveVisualVertex(v);
            }
            foreach (Edge e in Core.Edges)
            {
                RemoveVisualEdge(e);
            }
            ViewWindow.Static = false;
        }
    }
}

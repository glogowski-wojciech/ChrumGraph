using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

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

    enum MouseState { Normal, MovingVertex, MovingGraph, }
    enum GraphMode { DraggingMode, InsertingMode, }

    /// <summary>
    /// Visual representation of a graph.
    /// </summary>
    public partial class Visual : IVisual
    {
        private Canvas canvas;

        private Color vertexColor = Colors.Red;
        private Color edgeColor = Colors.Black;
        private SolidColorBrush vertexBrush;
        private SolidColorBrush edgeBrush;

        private Dictionary<UIElement, Vertex> VertexDict = new Dictionary<UIElement, Vertex>();
        private Vertex clickedVertex;
        private Point previousMousePosition;

        private MouseState mouseState = MouseState.Normal;

        /// <summary>
        /// Constructor for the Visual class.
        /// </summary>
        /// <param name="mainWindow">Application's main window</param>
        public Visual(MainWindow mainWindow)
        {
            VertexSize = 25.0;

            canvas = mainWindow.MainCanvas;
            
            vertexBrush = new SolidColorBrush(vertexColor);
            edgeBrush = new SolidColorBrush(edgeColor);
            ViewWindow = new ViewWindow(this);
            ViewWindow.Canvas = canvas;
            ViewWindow.MarginLength = 4.0 * VertexSize;

            canvas.MouseDown += CanvasMouseDown;
            canvas.MouseUp += MouseUp;
            canvas.MouseMove += MouseMove;
            canvas.MouseLeave += MouseUp;
            canvas.MouseWheel += MouseZoom;
        }

        /// <summary>
        /// Instance of Core class.
        /// </summary>
        public IVisualCore Core { get; set; }

        /// <summary>
        /// Instance of ViewWindow class.
        /// </summary>
        public ViewWindow ViewWindow { get; set; }

        /// <summary>
        /// Defines the vertices' size on the main canvas.
        /// </summary>
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
            element.MouseLeftButtonUp += MouseUp;
            element.MouseMove += MouseMove;
        }

        private void CanvasMouseDown(object sender, MouseEventArgs e)
        {
            if (mouseState != MouseState.Normal) return;

            ViewWindow.Static = true;
            previousMousePosition = e.GetPosition(canvas);
            mouseState = MouseState.MovingGraph;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            UIElement element = sender as UIElement;
            clickedVertex = VertexDict[element];
            clickedVertex.Clicked = true;
            previousMousePosition = e.GetPosition(canvas);
            mouseState = MouseState.MovingVertex;
            ViewWindow.Static = true;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseState == MouseState.Normal) return;

            Point mousePosition = e.GetPosition(canvas);
            Vector coreShift = ViewWindow.VisualToCorePosition(mousePosition) -
                ViewWindow.VisualToCorePosition(previousMousePosition);
            previousMousePosition = mousePosition;

            if (mouseState == MouseState.MovingVertex)
            {
                clickedVertex.Shift(coreShift);
            }
            else if (mouseState == MouseState.MovingGraph)
            {
                ViewWindow.Shift(coreShift);
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseState == MouseState.MovingVertex)
            {
                clickedVertex.Clicked = false;
                clickedVertex = null;
            }
            mouseState = MouseState.Normal;
        }

        private void MouseZoom(object sender, MouseWheelEventArgs e)
        {
            Point position = ViewWindow.VisualToCorePosition(e.GetPosition(canvas));
            ViewWindow.SetZoom(e.Delta / 120.0, position);
        }

        /// <summary>
        /// Creates a circle that represents a vertex on a canvas and binds them together.
        /// </summary>
        /// <param name="vertex">Vertex to be visually represented</param>
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
                FontSize = VertexSize * 0.75,
                Height = VertexSize,
                TextWrapping = TextWrapping.Wrap,
                Text = vertex.Label,
                Margin = new Thickness(VertexSize / 4, 0, 0, VertexSize / 10.0),

            };
            canvas.Children.Add(t);
            Canvas.SetZIndex(t, 3);

            AddEventHandlers(t, vertex);
            vertex.VisualLabel = t;
        }

        /// <summary>
        /// Deletes a vertex from the main canvas.
        /// </summary>
        /// <param name="vertex">Vertex to be deleted</param>
        public void RemoveVisualVertex(Vertex vertex)
        {
            VertexDict.Remove(vertex.Ellipse);
            VertexDict.Remove(vertex.VisualLabel);

            canvas.Children.Remove(vertex.Ellipse);
            canvas.Children.Remove(vertex.VisualLabel);
            vertex.Ellipse = null;
            vertex.VisualLabel = null;
        }

        /// <summary>
        /// Creates a line that represents an edge on a canvas and binds them together.
        /// </summary>
        /// <param name="edge">Edge to be visually represented</param>
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

        /// <summary>
        /// Deletes an edge from the main canvas.
        /// </summary>
        /// <param name="edge">Edge to be deleted</param>
        public void RemoveVisualEdge(Edge edge)
        {
            canvas.Children.Remove(edge.Line);
            edge.Line = null;
        }

        /// <summary>
        /// Standard getter and setter for the "Visual" booolean.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Draws a vertex on the main canvas based on its current position.
        /// </summary>
        /// <param name="v">Vertex to be redrawn</param>
        public void RedrawVertex(Vertex v)
        {
            Point visualPosition = ViewWindow.CoreToVisualPosition(v.Position);
            Canvas.SetLeft(v.Ellipse, visualPosition.X - VertexSize / 2.0);
            Canvas.SetTop(v.Ellipse, visualPosition.Y - VertexSize / 2.0);

            Canvas.SetLeft(v.VisualLabel, visualPosition.X - v.VisualLabel.ActualWidth / 2.0);
            Canvas.SetTop(v.VisualLabel, visualPosition.Y - VertexSize / 2.0);
        }

        /// <summary>
        /// Draws an edge on the main canvas based on current positions of the vertices
        /// it binds.
        /// </summary>
        /// <param name="e">Edge to be redrawn</param>
        public void RedrawEdge(Edge e)
        {
            e.Line.X1 = Canvas.GetLeft(e.V1.Ellipse) + VertexSize / 2.0;
            e.Line.Y1 = Canvas.GetTop(e.V1.Ellipse) + VertexSize / 2.0;
            e.Line.X2 = Canvas.GetLeft(e.V2.Ellipse) + VertexSize / 2.0;
            e.Line.Y2 = Canvas.GetTop(e.V2.Ellipse) + VertexSize / 2.0;
        }

        /// <summary>
        /// Draws all vertices and edges on the canvas.
        /// </summary>
        public void Refresh()
        {
            if (!Visible) Visible = true;

            ViewWindow.Adjust();
            
            foreach(Vertex v in Core.Vertices)
                RedrawVertex(v);

            foreach (Edge e in Core.Edges)
                RedrawEdge(e);
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

using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using Forms = System.Windows.Forms;
using System.Linq;

namespace ChrumGraph
{
    public partial class Vertex
    {
        /// <summary>
        /// Gets or sets Ellipse object of a vertex.
        /// </summary>
        public Ellipse Ellipse { get; set; }

        /// <summary>
        /// Gets or sets Visual label of a vertex.
        /// </summary>
        public TextBlock VisualLabel { get; set; }

        /// <summary>
        /// Changes vertex ellipse color
        /// </summary>
        /// <param name="c">New color</param>
        public void changeColor(Color c)
        {
            if (Ellipse != null)
                Ellipse.Fill = new SolidColorBrush(c);
        }
    }

    public partial class Edge
    {
        /// <summary>
        /// Gets or sets Line object of an edge.
        /// </summary>
        public Line Line { get; set; }

        /// <summary>
        /// Specifies whether edge is currently selected.
        /// </summary>
        public bool Selected { get; set; }
        
        /// <summary>
        /// Changes color of an edge.
        /// </summary>
        /// <param name="c">New color</param>
        public void changeColor(Color c)
        {
            if (Line != null)
                Line.Stroke = new SolidColorBrush(c);
        }
    }

    /// <summary>
    /// Specifies mouse state.
    /// </summary>
    public enum MouseState
    {
        /// <summary>Standard mouse state</summary>
        Normal,
        /// <summary>Mouse is currently moving vertex</summary>
        MovingVertex,
        /// <summary>Mouse is currently moving graph</summary>
        MovingGraph,
    }
    /// <summary>
    /// Specifies graph mode.
    /// </summary>
    public enum GraphMode
    {
        /// <summary>Mode oriented on dragging</summary>
        DraggingMode,
        /// <summary>Mode oriented on adding new vertices and edges</summary>
        InsertingMode,
    }

    /// <summary>
    /// Visual representation of a graph.
    /// </summary>
    public partial class Visual : IVisual
    {
        /// <summary>
        /// Constructor for the Visual class.
        /// </summary>
        /// <param name="mainWindow">Application's main window</param>
        public Visual(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            canvas = mainWindow.MainCanvas;

            VertexSize = 25.0;
            GraphMode = GraphMode.DraggingMode;
            
            ViewWindow = new ViewWindow(this);
            ViewWindow.Canvas = canvas;
            ViewWindow.MarginLength = 4.0 * VertexSize;

            addedEdge = new Line
            {
                Stroke = edgeBrush,
                StrokeThickness = VertexSize / verticeToEdgeRatio,
                Visibility = Visibility.Hidden,
            };

            canvas.Children.Add(addedEdge);
            Canvas.SetZIndex(addedEdge, 1);

            selectionBox = new Rectangle 
            {
                Visibility = Visibility.Collapsed,
                Stroke = new SolidColorBrush(selectionBoxColor),
                StrokeThickness = 1,
            };

            canvas.Children.Add(selectionBox);

            canvas.MouseDown += CanvasMouseDown;
            canvas.MouseUp += CanvasMouseUp;
            canvas.MouseUp += MouseUp;
            canvas.MouseMove += MouseMove;
            canvas.MouseLeave += MouseUp;
            canvas.MouseWheel += MouseZoom;
        }

        #region Fields
        private MainWindow mainWindow;
        private Canvas canvas;

        private SolidColorBrush vertexBrush = new SolidColorBrush(vertexColor);
        private SolidColorBrush edgeBrush = new SolidColorBrush(edgeColor);

        private Dictionary<UIElement, Vertex> VertexDict = new Dictionary<UIElement, Vertex>();
        private Dictionary<Line, Edge> EdgeDict = new Dictionary<Line, Edge>();
        private Vertex clickedVertex;
        private Edge clickedEdge;
        private Point previousMousePosition;

        private int SelectedAndPinnedVertices = 0;

        private bool mouseShiftDown = false;
        private Point mouseShiftDownPosition;
        private Rectangle selectionBox;

        private Line addedEdge;

        private MouseState mouseState = MouseState.Normal;

        private HashSet<Vertex> SelectedVertices = new HashSet<Vertex>();
        private HashSet<Edge> SelectedEdges = new HashSet<Edge>();
        #endregion
        #region Properties
        /// <summary>
        /// Instance of Core class.
        /// </summary>
        public IVisualCore Core { get; set; }

        /// <summary>
        /// Instance of ViewWindow class.
        /// </summary>
        public ViewWindow ViewWindow { get; set; }

        /// <summary>
        /// Gets or sets graph mode.
        /// </summary>
        public GraphMode GraphMode { get; set; }

        /// <summary>
        /// Defines the vertices' size on the main canvas.
        /// </summary>
        public double VertexSize { get; set; }

        /// <summary>
        /// Standard getter and setter for the "Visual" booolean.
        /// </summary>
        public bool Visible { get; set; }
        #endregion

        #region Event Handlers
        private void AddEventHandlers(UIElement element, Vertex v)
        {
            VertexDict.Add(element, v);
            element.MouseLeftButtonDown += MouseDown;
            element.MouseLeftButtonUp += VertexMouseUp;
            element.MouseLeftButtonUp += MouseUp;
            element.MouseMove += MouseMove;
        }

        private void AddEventHandlers(Line line, Edge e)
        {
            EdgeDict.Add(line, e);
            line.MouseLeftButtonDown += MouseDown;
            line.MouseLeftButtonUp += VertexMouseUp;
            line.MouseLeftButtonUp += MouseUp;
            line.MouseMove += MouseMove;
        }

        private void CanvasMouseDown(object sender, MouseEventArgs e)
        {
            if (!(e.OriginalSource is Canvas)) return;
            if (mouseState != MouseState.Normal) return;

            Point mousePosition = e.GetPosition(canvas);

            mouseShiftDownPosition = mousePosition;
            if (Forms.Control.ModifierKeys == Forms.Keys.Shift || Forms.Control.ModifierKeys == (Forms.Keys.Shift | Forms.Keys.Control))
            {
                mouseShiftDown = true;
                canvas.CaptureMouse();

                Canvas.SetLeft(selectionBox, mouseShiftDownPosition.X);
                Canvas.SetTop(selectionBox, mouseShiftDownPosition.Y);
                selectionBox.Width = 0;
                selectionBox.Height = 0;

                selectionBox.Visibility = Visibility.Visible;
            }

            if (GraphMode == GraphMode.DraggingMode)
            {   
                if(!(Forms.Control.ModifierKeys == Forms.Keys.Control || Forms.Control.ModifierKeys == (Forms.Keys.Shift | Forms.Keys.Control)))
                {
                    CleanSelectedVertices();
                    CleanSelectedEdges();
                }
                previousMousePosition = mousePosition;
                mouseState = MouseState.MovingGraph;
                ViewWindow.Static = true;
            }
            else if (GraphMode == GraphMode.InsertingMode)
            {
                if (Forms.Control.ModifierKeys == Forms.Keys.Shift || Forms.Control.ModifierKeys == (Forms.Keys.Shift | Forms.Keys.Control))
                    return;
                Point corePos = ViewWindow.VisualToCorePosition(mousePosition);
                clickedVertex = Core.CreateVertex(corePos.X, corePos.Y);
                SelectionProcessing();
            }
        }

        private void CanvasMouseUp(object sender, MouseEventArgs e)
        {
            if (mouseShiftDown)
            {
                mouseShiftDown = false;
                canvas.ReleaseMouseCapture();
                if (Forms.Control.ModifierKeys == Forms.Keys.Shift || Forms.Control.ModifierKeys == (Forms.Keys.Shift | Forms.Keys.Control))
                {
                    if (Forms.Control.ModifierKeys == Forms.Keys.Shift)
                    {
                        CleanSelectedVertices();
                        CleanSelectedEdges();
                    }

                    foreach(Vertex v in Core.Vertices)
                    {
                        if (IsPointInSelectionBox(ViewWindow.CoreToVisualPosition(v.Position)))
                            Select(v);
                    }
                       
                }
                selectionBox.Visibility = Visibility.Collapsed;
            }

            if (GraphMode == GraphMode.InsertingMode)
            {
                addedEdge.Visibility = Visibility.Hidden;
            }
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            UIElement element = sender as UIElement;
            Line line = sender as Line;
            if (line == null)
            {
                clickedVertex = VertexDict[element];
                clickedEdge = null;
            }
            if (line != null)
            {
                clickedEdge = EdgeDict[line];
                clickedVertex = null;
            }

            SelectionProcessing();
            previousMousePosition = e.GetPosition(canvas);

            if (GraphMode == GraphMode.DraggingMode)
            {
                mouseState = MouseState.MovingVertex;
                ViewWindow.Static = true;
            }
            else //adding new edge
            {
                if (line == null)
                {
                    addedEdge.X1 = addedEdge.X2 = Canvas.GetLeft(clickedVertex.Ellipse) + VertexSize / 2.0;
                    addedEdge.Y1 = addedEdge.Y2 = Canvas.GetTop(clickedVertex.Ellipse) + VertexSize / 2.0;
                    addedEdge.Visibility = Visibility.Visible;
                }
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(canvas);

            if (mouseShiftDown && Forms.Control.ModifierKeys == Forms.Keys.Shift || Forms.Control.ModifierKeys == (Forms.Keys.Shift | Forms.Keys.Control))
            {
                if (mouseShiftDownPosition.X < mousePosition.X)
                {
                    Canvas.SetLeft(selectionBox, mouseShiftDownPosition.X);
                    selectionBox.Width = mousePosition.X - mouseShiftDownPosition.X;
                }
                else
                {
                    Canvas.SetLeft(selectionBox, mousePosition.X);
                    selectionBox.Width = mouseShiftDownPosition.X - mousePosition.X;
                }

                if (mouseShiftDownPosition.Y < mousePosition.Y)
                {
                    Canvas.SetTop(selectionBox, mouseShiftDownPosition.Y);
                    selectionBox.Height = mousePosition.Y - mouseShiftDownPosition.Y;
                }
                else
                {
                    Canvas.SetTop(selectionBox, mousePosition.Y);
                    selectionBox.Height = mouseShiftDownPosition.Y - mousePosition.Y;
                }
                Console.WriteLine(selectionBox.Width);
                previousMousePosition = mousePosition;
                return;
            }
            else if (mouseShiftDown)
            {
                mouseShiftDown = false;
                canvas.ReleaseMouseCapture();
                selectionBox.Visibility = Visibility.Collapsed;
            }

            if (GraphMode == GraphMode.DraggingMode)
            {
                if (mouseState == MouseState.Normal) return;

                Vector coreShift = ViewWindow.VisualToCorePosition(mousePosition) -
                    ViewWindow.VisualToCorePosition(previousMousePosition);
                previousMousePosition = mousePosition;

                if (mouseState == MouseState.MovingVertex && clickedVertex != null)
                {
                    clickedVertex.Shift(coreShift);
                }
                else if (mouseState == MouseState.MovingGraph)
                {
                    ViewWindow.Shift(coreShift);
                }
            }
            else
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    addedEdge.X2 = mousePosition.X - 1;
                    addedEdge.Y2 = mousePosition.Y - 1;
                    if (mousePosition.X < addedEdge.X1)
                        addedEdge.X2 += 2;

                    if (mousePosition.Y < addedEdge.Y1)
                        addedEdge.Y2 += 2;
                }
            }
        }

        private void VertexMouseUp(object sender, MouseEventArgs e)
        {
            if (GraphMode == GraphMode.InsertingMode)
            {
                UIElement element = sender as UIElement;
                Line line = sender as Line;
                addedEdge.Visibility = Visibility.Hidden;
                if (line == null)
                {
                    Vertex currentVertex = VertexDict[element];
                    if (currentVertex != clickedVertex)
                        clickedEdge = Core.CreateEdge(currentVertex, clickedVertex);
                }
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (GraphMode == GraphMode.DraggingMode)
            {
                if (mouseState == MouseState.MovingVertex && clickedVertex != null)
                {
                    clickedVertex = null;
                }
                mouseState = MouseState.Normal;
            }
        }

        private void MouseZoom(object sender, MouseWheelEventArgs e)
        {
            Point position = ViewWindow.VisualToCorePosition(e.GetPosition(canvas));
            ViewWindow.SetZoom(e.Delta / 120.0, position);
        }
        #endregion

        private bool IsPointInSelectionBox(Point p)
        {
            return (p.X >= Canvas.GetLeft(selectionBox) && p.Y >= Canvas.GetTop(selectionBox) &&
                    p.X - Canvas.GetLeft(selectionBox) <= selectionBox.ActualWidth &&
                    p.Y - Canvas.GetTop(selectionBox) <= selectionBox.ActualHeight);
        }

        private void Select(Vertex v)
        {
            if (v == null) return;
            if (v.Pinned)
                SelectedAndPinnedVertices++;
            v.Selected = true;
            v.changeColor(selectVertexColor);
            SelectedVertices.Add(v);
        }

        private void Unselect(Vertex v)
        {
            if (v.Pinned)
                SelectedAndPinnedVertices--;
            v.Selected = false;
            v.changeColor(vertexColor); // TODO: What if there was pinned before select? Back to pinnedColor?
            SelectedVertices.Remove(v);
        }

        /// <summary>
        /// Changes label of selected vertex if there is only one selected.
        /// </summary>
        /// <param name="s">New label</param>
        public void ChangeSelectedLabel(string s)
        {
            if (SelectedVertices.Count == 1)
            {
                Vertex v = SelectedVertices.First();
                v.Label = s;
                v.VisualLabel.Text = s;
            }
        }

        /// <summary>
        /// Unselects all vertices.
        /// </summary>
        public void CleanSelectedVertices()
        {
            while (SelectedVertices.Count > 0)
                Unselect(SelectedVertices.First());
            SelectedVertices.Clear();
        }

        /// <summary>
        /// Deletes selected vertices.
        /// </summary>
        public void DeleteSelected()
        {
            List<Vertex> l = SelectedVertices.ToList();
            foreach (Vertex v in l)
            {
                Unselect(v);
                Core.RemoveVertex(v);
            }
        }

        private void Select(Edge e)
        {
            if (e == null) return;
            e.Selected = true;
            e.changeColor(selectEdgeColor);
            SelectedEdges.Add(e);
        }
        private void Unselect(Edge e)
        {
            e.Selected = false;
            e.changeColor(edgeColor);
            SelectedEdges.Remove(e);
        }

        /// <summary>
        /// Unselects all edges.
        /// </summary>
        public void CleanSelectedEdges()
        {
            while (SelectedEdges.Count > 0)
                Unselect(SelectedEdges.First());
            SelectedEdges.Clear();
        }

        private void SelectionProcessing()
        {
            if (Forms.Control.ModifierKeys == Forms.Keys.Control)
            {
                if (clickedVertex != null)
                {
                    if (clickedVertex.Selected)
                        Unselect(clickedVertex);
                    else
                        Select(clickedVertex);
                }
                if (clickedEdge != null)
                {
                    if (clickedEdge.Selected)
                        Unselect(clickedEdge);
                    else
                        Select(clickedEdge);
                }
            }
            else
            {
                CleanSelectedVertices();
                CleanSelectedEdges();
                Select(clickedVertex);
                Select(clickedEdge);
            }

            if (SelectedVertices.Count == 1)
                mainWindow.EnableVertexControls(SelectedVertices.First().Label);
            else
                mainWindow.DisableVertexControls();

            if (SelectedVertices.Count == 0)
            {
                mainWindow.UncheckPinnedCheckBox();
                mainWindow.DisablePinnedCheckBox();
            }

            if (SelectedAndPinnedVertices == SelectedVertices.Count && SelectedVertices.Count > 0)
                mainWindow.CheckPinnedCheckBox();
            else
                mainWindow.UncheckPinnedCheckBox();
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

            RedrawVertex(vertex);
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
            AddEventHandlers(edge.Line, edge);
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
        /// Draws a vertex on the main canvas based on its current position.
        /// </summary>
        /// <param name="v">Vertex to be redrawn</param>
        public void RedrawVertex(Vertex v)
        {
            Point visualPosition = ViewWindow.CoreToVisualPosition(v.Position);
            Canvas.SetLeft(v.Ellipse, visualPosition.X - VertexSize / 2.0);
            Canvas.SetTop(v.Ellipse, visualPosition.Y - VertexSize / 2.0);

            Canvas.SetLeft(v.VisualLabel, visualPosition.X - v.VisualLabel.ActualWidth / 2.0 - VertexSize / 4.0);
            Canvas.SetTop(v.VisualLabel, visualPosition.Y - VertexSize / 1.7);

            if (v == clickedVertex)
            {
                addedEdge.X1 = Canvas.GetLeft(clickedVertex.Ellipse) + VertexSize / 2.0;
                addedEdge.Y1 = Canvas.GetTop(clickedVertex.Ellipse) + VertexSize / 2.0;
            }
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

            foreach (Vertex v in Core.Vertices)
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

        /// <summary>
        /// Pinns all selected vertices
        /// </summary>
        public void PinnSelected()
        {
            List<Vertex> l = SelectedVertices.ToList();
            foreach (Vertex v in l)
                Core.Pin(v);

            SelectedAndPinnedVertices = SelectedVertices.Count;
        }

        /// <summary>
        /// Unpinns all selected vertices
        /// </summary>
        public void UnpinnSelected()
        {
            List<Vertex> l = SelectedVertices.ToList();
            foreach (Vertex v in l)
                Core.Unpin(v);

            SelectedAndPinnedVertices = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChrumGraph
{
    class Visual : IVisual
    {
        private bool visible;
        private Canvas canvas;
        Core parent;

        private double translateCoordHor(double x)
        {
            return x + canvas.Width / 2;
        }

        private double translateCoordVert(double y)
        {
            return y + canvas.Height / 2;
        }

        public Visual(Canvas _canvas, Core _parent)
        {
            canvas = _canvas;
            parent = _parent;
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
            foreach(Vertex v in parent.Vertices)
            {
                Ellipse e = new Ellipse();
                e.Height = e.Width = 25;
                e.Fill = new SolidColorBrush(Colors.Red);
                Canvas.SetLeft(e, (v.X + 1) * canvas.ActualWidth / 2.5);
                Canvas.SetTop(e, (v.Y + 1) * canvas.ActualHeight / 2.5);
                canvas.Children.Add(e);
            }
        }
    }
}

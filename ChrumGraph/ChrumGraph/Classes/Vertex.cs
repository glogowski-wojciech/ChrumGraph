using System.Collections.Generic;
using System.Windows;

namespace ChrumGraph
{
    /// <summary>
    /// Graph vertex.
    /// </summary>
    public partial class Vertex
    {
        private List<Edge> edges = new List<Edge>();

        /// <summary>
        /// Initializes a new instance of the Vertex class;
        /// </summary>
        /// <param name="x">X coordinate of the vertex.</param>
        /// <param name="y">Y coordinate of the vertex.</param>
        /// <param name="label">Label of the vertex.</param>
        public Vertex(double x, double y, string label="")
        {
            Position = new Point(x, y);
            Label = label;
            Pinned = false;
            Selected = false;
        }

        /// <summary>
        /// Gets or sets position of the vertex.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Position of a vertex (X coordinate).
        /// </summary>
        public double X
        {
            get { return Position.X; }
            set { Position = new Point(value, Y); }
        }

        /// <summary>
        /// Position of a vertex (Y coordinate).
        /// </summary>
        public double Y
        {
            get { return Position.Y; }
            set { Position = new Point(X, value); }
        }

        /// <summary>
        /// Moves vertex by a specified vector.
        /// </summary>
        /// <param name="shift">Core shift</param>
        public void Shift(Vector shift)
        {
            Position = Position + shift;
        }

        /// <summary>
        /// Gets list of edges whose one of endpoints is specified vertex.
        /// </summary>
        public List<Edge> Edges
        {
            get { return edges; }
        }

        /// <summary>
        /// Specifies whether a vertex is pinned.
        /// </summary>
        public bool Pinned { get; set; }

        /// <summary>
        /// Specifies whether vertex is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Specifies whether position of a vertex is forced,
        /// meaning it cannot be changed by Physics.
        /// </summary>
        public bool PositionForced
        {
            get { return Pinned || Selected; }
        }

        /// <summary>
        /// Gets or sets the label of the vertex.
        /// </summary>
        public string Label { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ChrumGraph
{
    /// <summary>
    /// Interface of Visual for Core.
    /// </summary>
    public interface IVisual
    {
        /// <summary>
        /// Creates a shape that represents a vertex in the canvas
        /// </summary>
        /// <returns></returns>
        Ellipse getVisualVertex();

        /// <summary>
        /// Creates visual representation of a given Core vertex.
        /// </summary>
        /// <param name="vertex">Core vertex.</param>
        void CreateVisualVertex(Vertex vertex);

        /// <summary>
        /// Removes visual representation of a given vertex.
        /// </summary>
        /// <param name="vertex">Vertex whose visual representation should be removed.</param>
        void RemoveVisualVertex(Vertex vertex);

        /// <summary>
        /// Creates visual representation of a given Core edge.
        /// </summary>
        /// <param name="edge">Core edge.</param>
        void CreateVisualEdge(Edge edge);

        /// <summary>
        /// Removes visual representation of a given edge.
        /// </summary>
        /// <param name="edge">Edge whose visual representation should be removed.</param>
        void RemoveVisualEdge(Edge edge);

        /// <summary>
        /// Specifies whether graph should be visible for user.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Redraws graph. If graph was not visible, the Visible property is set to true.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Removes all vertices and edges from Visual graph.
        /// </summary>
        void Clear();
    }
}

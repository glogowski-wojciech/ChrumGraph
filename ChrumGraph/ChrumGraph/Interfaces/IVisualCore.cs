using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
    /// <summary>
    /// Interface of Core for Visual.
    /// </summary>
    public interface IVisualCore
    {
        /// <summary>
        /// Gets list of all of graph's vertices.
        /// </summary>
        List<Vertex> Vertices { get; }

        /// <summary>
        /// Gets list of all of graph's edges.
        /// </summary>
        List<Edge> Edges { get; }

        /// <summary>
        /// Creates new vertex.
        /// </summary>
        /// <param name="x">X coordinate of a new vertex.</param>
        /// <param name="y">Y coordinate of a new vertex.</param>
        /// <param name="label">Label of a new vertex.</param>
        void CreateVertex(double x, double y, string label="");

        /// <summary>
        /// Removes vertex.
        /// </summary>
        /// <param name="vertex">Vertex to be removed.</param>
        void RemoveVertex(Vertex vertex);

        /// <summary>
        /// Creates new edge.
        /// </summary>
        /// <param name="v1">First vertex to be connected by a new edge.</param>
        /// <param name="v2">Second vertex to be connected by a new edge.</param>
        void CreateEdge(Vertex v1, Vertex v2);

        /// <summary>
        /// Removes edge.
        /// </summary>
        /// <param name="edge">Edge to be removed.</param>
        void RemoveEdge(Edge edge);

        /// <summary>
        /// Pins vertex.
        /// </summary>
        /// <param name="vertex">Vertex to be pinned.</param>
        void Pin(Vertex vertex);

        /// <summary>
        /// Unpins vertex.
        /// </summary>
        /// <param name="vertex">Vertex to be unpinned.</param>
        void Unpin(Vertex vertex);

        /// <summary>
        /// To be called when user clicks on a vertex.
        /// </summary>
        /// <param name="vertex">Vertex that was clicked.</param>
        void VertexClicked(Vertex vertex);

        /// <summary>
        /// To be called when user unclicks a vertex.
        /// </summary>
        /// <param name="vertex">Vertex that was unclicked.</param>
        void VertexUnclicked(Vertex vertex);

        /// <summary>
        /// Sets position of a given vertex.
        /// </summary>
        /// <param name="vertex">Vertex whose position should be set.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        void SetPosition(Vertex vertex, double x, double y);

        /// <summary>
        /// Loads graph from file.
        /// </summary>
        /// <param name="filename">Name of a file.</param>
        void LoadFromFile(string filename);

        /// <summary>
        /// Saves graph structure to file.
        /// </summary>
        /// <param name="filename">Name of a file.</param>
        void SaveGraph(string filename);

        /// <summary>
        /// Saves graph structure and vertices' positions to file.
        /// </summary>
        /// <param name="filename">Name of a file.</param>
        void SaveVisualGraph(string filename);
    }
}

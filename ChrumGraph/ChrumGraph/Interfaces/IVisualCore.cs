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
        void CreateVertex(double x, double y, string label="");
        void RemoveVertex(Vertex v);
        void CreateEdge(Vertex v1, Vertex v2);
        void RemoveEdge(Edge e);
        void Pin(Vertex v);
        void Unpin(Vertex v);
        void VertexClicked(Vertex v);
        void VertexUnclicked(Vertex v);
        void SetPosition(Vertex v, double x, double y);
        void LoadFromFile(string filename);
        void SaveGraph(string filename);
        void SaveVisualGraph(string filename);
    }
}

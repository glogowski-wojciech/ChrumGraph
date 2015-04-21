using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
    class Visual : IVisual
    {
        public Visual()
        {
  
        }
        public void CreateVisualVertex(Vertex v)
        { }
        public void RemoveVisualVertex(Vertex v)
        { }
        public void CreateVisualEdge(Edge e)
        { }
        public void RemoveVisualEdge(Edge e)
        { }
        public bool Visible
        {
            set
            {

            }
            get
            {
                return true;
            }
        }
        public void Refresh()
        { }
    }
}

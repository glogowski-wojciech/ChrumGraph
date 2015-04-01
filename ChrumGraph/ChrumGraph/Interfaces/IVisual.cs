using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
	/// <summary>
	/// Interface of Visual for Core.
	/// </summary>
	public interface IVisual
	{
		void CreateVisualVertex(Vertex v);
		void RemoveVisualVertex(Vertex v);
		void CreateVisualEdge(Edge e);
		void RemoveVisualEdge(Edge e);
		bool Visible { get; set; }
		void Refresh();
	}
}

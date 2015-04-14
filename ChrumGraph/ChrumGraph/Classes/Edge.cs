using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
	/// <summary>
	/// Graph edge.
	/// </summary>
	public partial class Edge
	{
		/// <summary>
		/// Initializes new instance of the Edge class.
		/// </summary>
		/// <param name="v1">First vertex connected by the edge.</param>
		/// <param name="v2">Second vertex connected by the edge.</param>
		public Edge(Vertex v1, Vertex v2)
		{
			V1 = v1;
			V2 = v2;
		}

		/// <summary>
		/// First vertex that is connected by an edge.
		/// </summary>
		public Vertex V1 { get; set; }

		/// <summary>
		/// Second vertex that is connected by an edge.
		/// </summary>
		public Vertex V2 { get; set; }

		/// <summary>
		/// Returns other than given end of edge.
		/// </summary>
		/// <param name="one">The given vertex.</param>
		/// <returns>The other vertex if the first one was valid, null otherwise.</returns>
		public Vertex Other(Vertex one)
		{
			if (one == V1) return V2;
			else if (one == V2) return V1;
			return null;
		}
	}
}

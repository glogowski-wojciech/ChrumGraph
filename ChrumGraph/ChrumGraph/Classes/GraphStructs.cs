using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
	/// <summary>
	/// Graph vertex.
	/// </summary>
	public partial class Vertex
	{
		private List<Edge> edges = new List<Edge>();

		/// <summary>
		/// Position of a vertex (X coordinate).
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// Position of a vertex (Y coordinate).
		/// </summary>
		public double Y { get; set; }

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
		/// Specifies whether user forced position of a vertex.
		/// </summary>
		public bool PositionForced { get; set; }

		/// <summary>
		/// Position forced by user (X coordinate).
		/// </summary>
		/// <remarks>
		/// When position is forced by user, it won't by changed by Physics.
		/// </remarks>
		public double ForcedX { get; set; }

		/// <summary>
		/// Position forced by user (Y coordinate).
		/// </summary>
		/// <remarks>
		/// When position is forced by user, it won't by changed by Physics.
		/// </remarks>
		public double ForcedY { get; set; }
	}

	/// <summary>
	/// Graph edge.
	/// </summary>
	public partial class Edge
	{
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

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
		public double X
		{
			get;
			set;
		}
		/// <summary>
		/// Position of a vertex (Y coordinate).
		/// </summary>
		public double Y
		{
			get;
			set;
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
		public bool Pinned;

		/// <summary>
		/// Specifies whether user forced position of a vertex.
		/// </summary>
		public bool PositionForced;

		/// <summary>
		/// Position forced by user (meaning it won't be changed by Physics).
		/// </summary>
		public double ForcedX, ForcedY;
	}

	/// <summary>
	/// Graph edge.
	/// </summary>
	public partial class Edge
	{
		/// <summary>
		/// First vertex that is connected by an edge.
		/// </summary>
		public Vertex V1;

		/// <summary>
		/// Second vertex that is connected by an edge.
		/// </summary>
		public Vertex V2;

        /// <summary>
        /// Return other than given end of edge.
        /// </summary>
        /// <param name="one">The given vertex.</param>
        /// <returns></returns>
        public Vertex Other(Vertex one);
	}
}

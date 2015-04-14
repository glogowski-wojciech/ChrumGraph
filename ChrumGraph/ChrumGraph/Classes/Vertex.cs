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
		private bool pinned, clicked;

		/// <summary>
		/// Initializes a new instance of the Vertex class;
		/// </summary>
		/// <param name="x">X coordinate of the vertex.</param>
		/// <param name="y">Y coordinate of the vertex.</param>
		public Vertex(double x, double y)
		{
			X = x;
			Y = y;
			Pinned = false;
			Clicked = false;
		}

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
		/// Specifies whether a vertex is being clicked by user.
		/// </summary>
		public bool Clicked { get; set; }

		/// <summary>
		/// Specifies whether position of a vertex is forced,
		/// meaning it cannot be changed by Physics.
		/// </summary>
		public bool PositionForced
		{
			get { return pinned || clicked; }
		}
	}
}

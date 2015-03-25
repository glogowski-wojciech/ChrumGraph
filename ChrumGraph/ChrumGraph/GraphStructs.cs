using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
	public partial class Vertex
	{
		public double X, Y; //położenie w Core
		public bool Pinned;

		//położenie wymuszone przez użytkownika
		public bool PositionForced;
		public double ForcedX, ForcedY;
	}

	public partial class Edge
	{
		public Vertex V1, V2;
	}
}

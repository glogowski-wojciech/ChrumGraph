using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace ChrumGraph
{
	/// <summary>
	/// Graph logic.
	/// </summary>
	public class Core: IPhysicsCore//, IVisualCore
	{
		private const double defaultFPS = 30.0;

		private DispatcherTimer refreshTimer = new DispatcherTimer();
		private double fps;

		private IVisual visual;
		private IPhysics physics;

		private List<Vertex> vertices = new List<Vertex>();
		private List<Edge> edges = new List<Edge>();

		/// <summary>
		/// Initializes a new instance of the Core class.
		/// </summary>
		public Core(IVisual visual)
		{
            this.visual = visual;
			physics = new Physics(this);

			FPS = defaultFPS;
			refreshTimer.Tick += (sender, e) =>
				{
					if (visual.Visible) visual.Refresh();
				};
		}

		/// <summary>
		/// Desired graph refresh frame rate (effective frame rate can be lower, though).
		/// </summary>
		public double FPS
		{
			get { return fps; }
			set
			{
				if (value <= 0) return;
				int ms = (int)(1000.0 / value);
				if (ms > 0)
				{
					refreshTimer.Interval = new TimeSpan(0, 0, 0, 0, ms);
					fps = value;
				}
			}
		}

		/// <summary>
		/// Pins vertex, so its position won't be changed by Physics.
		/// </summary>
		/// <param name="vertex">Vertex to be pinned.</param>
		public void Pin(Vertex vertex)
		{
			lock(this)
			{
				vertex.Pinned = true;
			}
		}

		/// <summary>
		/// Unpins vertex.
		/// </summary>
		/// <param name="vertex">Vertex to be unpinned.</param>
		public void Unpin(Vertex vertex)
		{
			lock(this)
			{
				vertex.Pinned = false;
			}
		}

		/// <summary>
		/// Sets position of a given vertex.
		/// </summary>
		/// <param name="vertex">Vertex whose position should be set.</param>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		public void SetPosition(Vertex vertex, double x, double y)
		{
			lock(this)
			{
				vertex.X = x;
				vertex.Y = y;
			}
		}

		/// <summary>
		/// Gets list of graph's vertices.
		/// </summary>
		public List<Vertex> Vertices
		{
			get { return vertices; }
		}

        /// <summary>
        /// Gets list of graph's edges.
        /// </summary>
        public List<Edge> Edges
        {
            get { return edges; }
        }

        /// <summary>
        /// Schould be called after the simulation has finished.
        /// </summary>
        public void SimulationFinished()
        {
            //TODO
        }
	}
}

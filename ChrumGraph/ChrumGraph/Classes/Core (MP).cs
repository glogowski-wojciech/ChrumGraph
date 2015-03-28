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
	public partial class Core
	{
		private const double defaultFPS = 30.0;

		private DispatcherTimer refreshTimer = new DispatcherTimer();
		private double fps;

		//only temporary here, eventually it will be in Ewa's part and assigned in constructor
		private IVisual Visual;

		//to be called in the constructor
		private void Init()
		{
			FPS = defaultFPS;
			refreshTimer.Tick += (sender, e) =>
				{
					Visual.Refresh();
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
				double secondsDouble = 1.0 / value;
				int seconds, milliseconds;
				seconds = (int)secondsDouble;
				milliseconds = (int)((secondsDouble - seconds) * 1000);
				if (seconds != 0 || milliseconds != 0)
				{
					refreshTimer.Interval = new TimeSpan(0, 0, 0, seconds, milliseconds);
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
	}
}

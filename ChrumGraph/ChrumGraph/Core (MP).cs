using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ChrumGraph
{
	public partial class Core
	{
		private const double defaultFPS = 30.0;

		private DispatcherTimer refreshTimer = new DispatcherTimer();
		private double fps;

		//liczba klatek na sekundę przy odświeżaniu grafu
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
	}
}

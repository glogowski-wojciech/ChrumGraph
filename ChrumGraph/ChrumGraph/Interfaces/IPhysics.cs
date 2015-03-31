using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
	/// <summary>
	/// Interface of Physics for Core.
	/// </summary>
    interface IPhysics
    {
        // ... parameters
        void StartSimulation(double fps);
        void StartSimulation();
        void StopSimulation();
    }
}

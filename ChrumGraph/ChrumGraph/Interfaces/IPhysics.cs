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
        double vertexForce;
        double edgeForce;
        void StartSimulation(double fps);
        void StartSimulation(); //TODO
        void StopSimulation();
    }
}

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
    public interface IPhysics
    {
        double VertexForceParam { get; set; }
        double EdgeForceParam { get; set; }
        double FrictionParam { get; set; }
        void StartSimulation(double fps);
        void StartSimulation(int ms);
        void StopSimulation();
    }
}

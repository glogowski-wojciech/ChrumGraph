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
        double VertexForce { get; set; }
        double EdgeForce { get; set; }

        void StartSimulation(double fps);
        void StartSimulation(int ms); //TODO
        void StopSimulation();
    }
}

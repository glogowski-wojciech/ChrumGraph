using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
    /// <summary>
    /// Interface of Core for Physics.
    /// </summary>
    public interface IPhysicsCore
    {
        List<Vertex> Vertices { get; }
        List<Edge> Edges { get; }

        void SimulationFinished();
    };
}

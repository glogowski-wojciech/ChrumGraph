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
        /// <summary>
        /// Gets list of all of graph's vertices.
        /// </summary>
        List<Vertex> Vertices { get; }

        /// <summary>
        /// Gets list of all of graph's edges.
        /// </summary>
        List<Edge> Edges { get; }

        /// <summary>
        /// Method that should be called right after simulation has finished.
        /// </summary>
        void SimulationFinished();
    };
}

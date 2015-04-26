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
        /// <summary>
        /// Coefficient of intervertical interaction.
        /// </summary>
        double VertexForceParam { get; set; }

        /// <summary>
        /// Coefficient of vertices connected by edge interaction.
        /// </summary>
        double EdgeForceParam { get; set; }

        /// <summary>
        /// Coefficient of friction between vertices and background.
        /// </summary>
        double FrictionParam { get; set; }

        /// <summary>
        /// Starts IPhysics simulation. Can be stopped with StopSimulation
        /// method.
        /// </summary>
        /// <param name="fps">Frequency of IPhysics iterations per second.
        /// </param>
        void StartSimulation(double fps);

        /// <summary>
        /// Starts IPhysics simulation. Stops after ms miliseconds.
        /// </summary>
        /// <param name="ms">Time of IPhysics simulation in miliseconds.
        /// </param>
        void StartSimulation(int ms);

        /// <summary>
        /// Stops simulation started with StartSimulation. If IPhysics
        /// simulation is not working, does nothing.
        /// </summary>
        void StopSimulation();
    }
}

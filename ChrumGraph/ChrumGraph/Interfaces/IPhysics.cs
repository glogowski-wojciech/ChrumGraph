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
        /// <exception cref="SimulationAlreadyRunningException">Thrown if
        /// simulation is already in run while StartSimulation invoked.
        /// </exception>
        void StartSimulation(double fps);

        /// <summary>
        /// Starts IPhysics simulation. Stops after ms miliseconds. Then
        /// invokes SimulationFinished method of IPhysicsCore.
        /// </summary>
        /// <param name="ms">Time of IPhysics simulation in miliseconds.
        /// </param>
        /// <exception cref="SimulationAlreadyRunningException">Thrown if
        /// simulation is already in run while StartSimulation invoked.
        /// </exception>
        void StartSimulation(int ms);

        /// <summary>
        /// Stops simulation started with StartSimulation. If IPhysics
        /// simulation is not working, does nothing.
        /// </summary>
        /// <exception cref="SimulationAlreadyStoppedException">Thrown if
        /// simulation is not running while StopSimulation invoked.</exception>
        void StopSimulation();
    }

    /// <summary>
    /// Thrown if IPhysics simulation is being run again with StartSimulation
    /// without StopSimulation after previous run.
    /// </summary>
    public class SimulationAlreadyRunningException : InvalidOperationException
    {
    }

    /// <summary>
    /// Thrown if IPhysics simulation is being stopped again with StopSimulation
    /// without being run after previous stop.
    /// </summary>
    public class SimulationAlreadyStoppedException : InvalidOperationException
    {
    }

}

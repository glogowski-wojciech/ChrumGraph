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
    /// Performs physical simulation on a graph.
    /// </summary>
    public class Physics : IPhysics
    {
        /// <summary>
        /// Reference to global Core.
        /// </summary>
        private IPhysicsCore physicsCore;

        /// <summary>
        /// Reference to physicsCore's Vertices.
        /// </summary>
        private List<Vertex> vertices;

        /// <summary>
        /// Reference to physicsCore's Edges.
        /// </summary>
        private List<Edge> edges;

        /// <summary>
        /// Movement vectors for updating vertices' coordinates.
        /// </summary>
        private Vector[] netForces;

        /// <summary>
        /// Vertices' coordinates are easier to manipulate as Vector objects.
        /// </summary>
        private Vector[] coordinatesArray;

        private DispatcherTimer dispatcherTimer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Physics"/> class.
        /// </summary>
        /// <param name="physicsCore">The physics core.</param>
        public Physics(IPhysicsCore physicsCore)
        {
            vertexForceParamGuard = new object();
            edgeForceParamGuard = new object();
            frictionParamGuard = new object();
            simulateGuard = new object();
            this.physicsCore = physicsCore;
            vertices = physicsCore.Vertices;
            edges = physicsCore.Edges;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += (sender, e) => { IterateSimulation(); };
            VertexForceParam = 1.0;
            EdgeForceParam = 1.0;
            Simulate = false;
        }
        
        /// <summary>
        /// Starts the simulation. If simulation is already in run throws , makes no effect.
        /// </summary>
        /// <param name="fps">The FPS.</param>
        public void StartSimulation(double fps)
        {
            if (!dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(100.0 / fps));
                dispatcherTimer.Start();
            }
        }
        
        /// <summary>
        /// Starts the simulation.
        /// </summary>
        public void StartSimulation(int ms) //TODO
        {
            nonStopSimulation();
        }
        
        /// <summary>
        /// Stops the simulation.
        /// </summary>
        public void StopSimulation()
        {
            dispatcherTimer.Stop();
        }

        /// <summary>
        /// Stops the simulation after time.
        /// </summary>
        /// <param name="ms">The ms.</param>
        private void StopSimulationAfterTime(int ms)
        {

        }
        
        /// <summary>
        /// Iterations that can be stopped only by switching Simulate to false.
        /// </summary>
        private void nonStopSimulation()
        {
            Simulate = true;
            while (Simulate)
            {
                IterateSimulation();
            }
        }

        /* Simulation step and auxiliary methods */

        /// <summary>
        /// Executes one step of simulation.
        /// </summary>
        private void IterateSimulation()
        {
            lock (physicsCore)
            {
                int n = vertices.Count;
                coordinatesArray = new Vector[n];
                netForces = new Vector[n];
                Parallel.For(0, n, (int i) =>
                {
                    Vertex v = vertices[i];
                    coordinatesArray[i] = new Vector(v.X, v.Y);
                    netForces[i] = new Vector(0.0, 0.0);
                });
                Parallel.For(0, n, (int i) =>
                {
                    updateForces(i);
                });
                Parallel.For(0, n, (int i) =>
                {
                    vertices[i].X += netForces[i].X;
                    vertices[i].Y += netForces[i].Y;
                });
            }

        }

        /// <summary>
        /// Updates the movement vector of given vertex.
        /// </summary>
        /// <param name="k">Index of vertex which movement vector is to be
        /// updated.</param>
        private void updateForces(int k)
        {
            int n = vertices.Count;
            Vector currentCoordinates = coordinatesArray[k];
            Vertex currentVertex = vertices[k];
            for (int i = 0; i < n; ++i)
            {
                netForces[k] += VertexForce(ref currentCoordinates, ref coordinatesArray[i]);
            }
            foreach (Edge e in currentVertex.Edges)
            {
                Vertex other = e.Other(currentVertex);
                Vector otherCoordinates = new Vector(other.X, other.Y);
                netForces[k] += EdgeForce(ref currentCoordinates, ref otherCoordinates);
            }
            netForces[k] += Friction(netForces[k]);
        }
       
        /* physical forces */

        /// <summary>
        /// Counts the movement vector of 'current' vertex that comes from
        /// vertical influence of 'other'.
        /// </summary>
        /// <param name="current">The current vertex.</param>
        /// <param name="other">The other vertex.</param>
        /// <returns></returns>
        private Vector VertexForce(ref Vector current, ref Vector other)
        {
            Vector d = other - current;
            double l = d.Length;
            d.Normalize();
            return d * VertexForceFunction(l);
        }

        /// <summary>
        /// Vertex influence of distance function.
        /// </summary>
        /// <param name="x">The distance.</param>
        /// <returns></returns>
        private double VertexForceFunction(double x)
        {
            return vertexForceParam * (x >= 0.0 ? (x <= 1.0 ? -1.0 : -1.0 / x)
                                                : (x >= -1.0 ? 1.0 : -1.0 / x));
        }

        /// <summary>
        /// Counts the movement vector of 'current' vertex that comes from edge
        /// influence of 'other'.
        /// </summary>
        /// <param name="current">The current vertex.</param>
        /// <param name="other">The other vertex.</param>
        /// <returns></returns>
        private Vector EdgeForce(ref Vector current, ref Vector other)
        {
            Vector v = other - current;
            return new Vector(); //TODO
        }

        /// <summary>
        /// Edge influence of distance function.
        /// </summary>
        /// <param name="x">The given distance.</param>
        /// <returns></returns>
        private double EdgeForceFunction(double x)
        {
            return -edgeForceParam * x;
        }
        
        /// <summary>
        /// Returns the movement vector of force after adding friction to
        /// current vector.
        /// </summary>
        /// <param name="force">The given vector.</param>
        /// <returns></returns>
        private Vector Friction(Vector force)
        {
            double l = force.Length;
            force.Normalize(); // we use the fact that Vector is passed by copy as it is structure
            force *= FrictionFunction(l);
            return force;
        }

        /// <summary>
        /// Frictions influence of force function.
        /// </summary>
        /// <param name="x">The given force.</param>
        /// <returns></returns>
        private double FrictionFunction(double x)
        {
            return frictionParam / 2.0 * (x + (Math.Abs(x - frictionParam) - Math.Abs(x + frictionParam)));
        }

        /* guards of fields for multithreading */

        /// <summary>
        /// The guard of vertexForceParam.
        /// </summary>
        private object vertexForceParamGuard;

        /// <summary>
        /// The guard of edgeForceParam.
        /// </summary>
        private object edgeForceParamGuard;

        /// <summary>
        /// The guard of frictionParam.
        /// </summary>
        private object frictionParamGuard;

        /// <summary>
        /// The guard of simulate.
        /// </summary>
        private object simulateGuard;
       
        /* fields accesed by properties */

        /// <summary>
        /// Linear coefficient of vertical force.
        /// </summary>
        private double vertexForceParam;

        /// <summary>
        /// Linear coefficient of edge force.
        /// </summary>
        private double edgeForceParam;

        /// <summary>
        /// Linear coefficient of friction.
        /// </summary>
        private double frictionParam;

        /// <summary>
        /// Condition for executing simulation in nonStopSimulation.
        /// </summary>
        private bool simulate;

        /* properties */

        /// <summary>
        /// Gets or sets the vertex force parameter.
        /// </summary>
        /// <value>
        /// The vertex force parameter.
        /// </value>
        public double VertexForceParam
        {
            set
            {
                lock(vertexForceParamGuard)
                {
                    vertexForceParam = value;
                }
            }

            get
            {
                double returnValue;
                lock(vertexForceParamGuard)
                {
                    returnValue = vertexForceParam;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Gets or sets the edge force parameter.
        /// </summary>
        /// <value>
        /// The edge force parameter.
        /// </value>
        public double EdgeForceParam
        {
            set
            {
                lock(edgeForceParamGuard)
                {
                    edgeForceParam = value;
                }
            }

            get
            {
                double returnValue;
                lock(edgeForceParamGuard)
                {
                    returnValue = edgeForceParam;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Gets or sets the friction parameter.
        /// </summary>
        /// <value>
        /// The friction parameter.
        /// </value>
        public double FrictionParam
        {
            set
            {
                lock(frictionParamGuard)
                {
                    frictionParam = value;
                }
            }

            get
            {
                double returnValue;
                lock(frictionParamGuard)
                {
                    returnValue = frictionParam;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Gets or sets the simualte.
        /// </summary>
        /// <value>
        /// The simulate.
        /// </value>
        private bool Simulate
        {
            set
            {
                lock(simulateGuard)
                {
                    simulate = value;
                }
            }

            get
            {
                bool returnValue;
                lock(simulateGuard)
                {
                    returnValue = Simulate;
                }
                return returnValue;
            }
        }

    };

}

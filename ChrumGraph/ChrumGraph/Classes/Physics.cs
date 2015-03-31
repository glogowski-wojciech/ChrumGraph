using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

/* Interface of Core for Physics */

namespace ChrumGraph
{

    class Physics : IPhysics
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Physics"/> class.
        /// </summary>
        /// <param name="physicsCore">The physics core.</param>
        public Physics(IPhysicsCore physicsCore)
        {
            this.physicsCore = physicsCore;
            vertices = physicsCore.Vertices;
            edges = physicsCore.Edges;
            dispatcherTimer = new DispatcherTimer();
            VertexForce = 1.0;
            EdgeForce = 1.0;
            vertices = physicsCore.Vertices;
            edges = physicsCore.Edges;
            Simulate = false;
            vertexForceParamGuard = new object();
            edgeForceParamGuard = new object();
            simulateGuard = new object();
        }
        
        /// <summary>
        /// Starts the simulation.
        /// </summary>
        /// <param name="fps">The FPS.</param>
        public void StartSimulation(double fps) //TODO
        {
            dispatcherTimer.Tick += (sender, e) => { IterateSimulation(); };
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(100.0 / fps)); // TODO
            dispatcherTimer.Start();
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
            Simulate = false;
        }

        private void nonStopSimulation()
        {
            Simulate = true;
            while (Simulate)
            {
                IterateSimulation();
            }
        }

        /* Simulation step and auxiliary methods */

        private void IterateSimulation()
        {
            lock (physicsCore)
            {
                int n = vertices.Count;
                verticesArray = vertices.ToArray();
                coordinatesArray = new Vector[n];
                netForces = new Vector[n];
                for (int i = 0; i < n; ++i)
                {
                    Vertex v = verticesArray[i];
                    coordinatesArray[i] = new Vector(v.X, v.Y);
                    netForces[i] = new Vector(0.0, 0.0);
                }
                Task[] tasks = new Task[n];
                for (int i = 0; i < n; ++i)
                {
                    tasks[i] = Task.Factory.StartNew((() => { updateForces(i); }));
                }
                try
                {
                    Task.WaitAll(tasks);
                }
                catch (AggregateException)
                {
                    Console.WriteLine("AggregateException that does something"); //TODO
                }
            }

        }

        private void updateForces(int k)
        {
            int n = vertices.Count;
            Vector currentCoordinates = coordinatesArray[k];
            Vertex currentVertex = verticesArray[k];
            for (int i = 0; i < n; ++i)
            {
                netForces[k] += vertexForce(currentCoordinates, coordinatesArray[i]);
            }
            foreach (Edge e in currentVertex.Edges)
            {
                Vertex other = e.Other(currentVertex);
                netForces[k] += edgeForce(currentCoordinates, new Vector(other.X, other.Y));
            }
        }

        private IPhysicsCore physicsCore;
        private List<Vertex> vertices;
        private List<Edge> edges;
        private Vertex[] verticesArray;
        private Vector[] netForces;
        private Vector[] coordinatesArray;

        private DispatcherTimer dispatcherTimer;
       
        /* physical forces */

        private Vector vertexForce(Vector current, Vector other)
        {
            return new Vector(); //TODO
        }

        private Vector edgeForce(Vector current, Vector other)
        {
            return new Vector(); //TODO
        }

        /* guards of fields for multithreading */

        private object vertexForceParamGuard;
        private object edgeForceParamGuard;
        private object simulateGuard;
       
        /* fields accesed by properties */

        private double vertexForceParam;
        private double edgeForceParam;
        private bool simulate;

        /* properties */

        /// <summary>
        /// Gets or sets the vertex force.
        /// </summary>
        /// <value>
        /// The vertex force.
        /// </value>
        public double VertexForce
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
        /// Gets or sets the edge force.
        /// </summary>
        /// <value>
        /// The edge force.
        /// </value>
        public double EdgeForce
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

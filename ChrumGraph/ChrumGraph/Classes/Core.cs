﻿using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;

namespace ChrumGraph
{
    /// <summary>
    /// Graph logic.
    /// </summary>
    public partial class Core: IPhysicsCore, IVisualCore
    {
        private const double defaultFPS = 60.0;

        private DispatcherTimer refreshTimer = new DispatcherTimer();
        private double fps;

        private IVisual visual;
        private IPhysics physics;

        private List<Vertex> vertices = new List<Vertex>();
        //private Dictionary<string, Vertex> verticesDict = new Dictionary<string, Vertex>();
        private List<Edge> edges = new List<Edge>();

        /// <summary>
        /// Initializes a new instance of the Core class.
        /// </summary>
        public Core(IVisual visual)
        {
            this.visual = visual;
            physics = new Physics(this);

            FPS = defaultFPS;
            refreshTimer.Tick += (sender, e) =>
                {
                    if (visual.Visible)
                    {
                        lock (this) { visual.Refresh(); }
                    }
                };
            refreshTimer.Start();
            physics.StartSimulation(FPS);
        }
        
        /// <summary>
        /// Initializes a new instance of the Core class without Canvas.
        /// </summary>
        /// <remarks>
        /// This constructor should only be used for testing.
        /// </remarks>
        public Core() : this(null) { }

        /// <summary>
        /// Gets Physics object created by Core instance.
        /// </summary>
        public IPhysics Physics
        {
            get { return physics; }
        }

        /// <summary>
        /// Desired graph refresh frame rate (effective frame rate can be lower, though).
        /// </summary>
        public double FPS
        {
            get { return fps; }
            set
            {
                if (value <= 0) return;
                int ms = (int)(1000.0 / value);
                if (ms > 0)
                {
                    refreshTimer.Interval = TimeSpan.FromMilliseconds(ms);
                    fps = value;
                }
            }
        }

        /// <summary>
        /// Indicates whether the graph is empty.
        /// </summary>
        /// <returns>true if there are no vertices in graph; otherwise false</returns>
        public bool IsEmpty()
        {
            return Vertices.Count == 0;
        }

        /// <summary>
        /// Pins vertex, so its position won't be changed by Physics.
        /// </summary>
        /// <param name="vertex">Vertex to be pinned.</param>
        public void Pin(Vertex vertex)
        {
            lock (this) { vertex.Pinned = true; }
        }

        /// <summary>
        /// Unpins vertex.
        /// </summary>
        /// <param name="vertex">Vertex to be unpinned.</param>
        public void Unpin(Vertex vertex)
        {
            lock (this) { vertex.Pinned = false; }
        }

        /// <summary>
        /// Sets position of a given vertex.
        /// </summary>
        /// <param name="vertex">Vertex whose position should be set.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void SetPosition(Vertex vertex, double x, double y)
        {
            lock(this)
            {
                vertex.X = x;
                vertex.Y = y;
            }
        }

        /// <summary>
        /// Gets list of graph's vertices.
        /// </summary>
        public List<Vertex> Vertices
        {
            get { return vertices; }
        }

        /// <summary>
        /// Gets list of graph's edges.
        /// </summary>
        public List<Edge> Edges
        {
            get { return edges; }
        }

        /// <summary>
        /// Schould be called after the simulation has finished.
        /// </summary>
        public void SimulationFinished()
        {
            visual.Visible = true;
        }

        /// <summary>
        /// Creates Core vertex and related Visual vertex.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="label">label of vertex</param>
        public Vertex CreateVertex(double x, double y, string label = "")
        {
            Vertex v = new Vertex(x, y, label);
            Vertices.Add(v);
            visual.CreateVisualVertex(v);
            return v;
        }

        /// <summary>
        /// Removes every edge connected with vertex intended to be removed.
        /// </summary>
        /// <param name="v">Vertex intended to be removed.</param>
        private void RemoveEdgesInVertex(Vertex v)
        {
            while (v.Edges.Count > 0)
                RemoveEdge(v.Edges[0]);
        }

        /// <summary>
        /// Removes Core vertex and related Visual vertex.
        /// </summary>
        /// <param name="v">Vertex to be removed.</param>
        public void RemoveVertex(Vertex v)
        {
            RemoveEdgesInVertex(v);
            Vertices.Remove(v);
            visual.RemoveVisualVertex(v);
        }

        /// <summary>
        /// Creates Core edge and related Visual edge.
        /// </summary>
        /// <param name="v1">Endpoint of creating edge.</param>
        /// <param name="v2">Endpoint of creating edge.</param>
        public Edge CreateEdge(Vertex v1, Vertex v2)
        {
            Edge e = new Edge(v1, v2);
            Edges.Add(e);
            v1.Edges.Add(e);
            v2.Edges.Add(e);
            visual.CreateVisualEdge(e);
            return e;
        }

        /// <summary>
        /// Removes Core edge and related Visual edge.
        /// </summary>
        /// <param name="e">Edge being removed.</param>
        public void RemoveEdge(Edge e)
        {
            e.V1.Edges.Remove(e);
            e.V2.Edges.Remove(e);
            Edges.Remove(e);
            visual.RemoveVisualEdge(e);
        }

        /// <summary>
        /// Getter for the visual in the core
        /// </summary>
        public IVisual Visual
        {
            get { return visual; }
        }
    }
}

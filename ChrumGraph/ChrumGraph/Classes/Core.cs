using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.IO;

namespace ChrumGraph
{
    /// <summary>
    /// Graph logic.
    /// </summary>
    public class Core: IPhysicsCore, IVisualCore
    {
        private const double defaultFPS = 30.0;

        private DispatcherTimer refreshTimer = new DispatcherTimer();
        private double fps;

        private IVisual visual;
        private IPhysics physics;

        private List<Vertex> vertices = new List<Vertex>();
        private Dictionary<string, Vertex> verticesDict = new Dictionary<string, Vertex>();
        private List<Edge> edges = new List<Edge>();

        /// <summary>
        /// Initializes a new instance of the Core class.
        /// </summary>
        public Core(System.Windows.Controls.Canvas canvas)
        {
            visual = new Visual(canvas, this);

            physics = new Physics(this);

            FPS = defaultFPS;
            refreshTimer.Tick += (sender, e) =>
                {
                    if (visual.Visible)
                    {
                        lock (this) { visual.Refresh(); }
                    }
                };
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
                    refreshTimer.Interval = new TimeSpan(0, 0, 0, 0, ms);
                    fps = value;
                }
            }
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
        /// Sets vertex state to clicked, so its position temporarily won't be
        /// changed by Physics.
        /// </summary>
        /// <param name="vertex">Vertex whose state should be changed.</param>
        public void VertexClicked(Vertex vertex)
        {
            lock (this) { vertex.Clicked = true; }
        }

        /// <summary>
        /// Sets vertex state to not clicked.
        /// </summary>
        /// <param name="vertex">Vertex whose position should by changed.</param>
        public void VertexUnclicked(Vertex vertex)
        {
            lock (this) { vertex.Clicked = false; }
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
        /// Gets dictionary of graph's vertices.
        /// </summary>
        public Dictionary<string, Vertex> VerticesDict
        {
            get { return verticesDict; }
        }

        /// <summary>
        /// Schould be called after the simulation has finished.
        /// </summary>
        public void SimulationFinished()
        {
            visual.Visible = true;
        }

        /// <summary>
        /// Creates Core vertex and releted Visual vertex.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="label">label of vertex</param>
        public void CreateVertex(double x, double y, string label = "")
        {
            Vertex v = new Vertex(x, y, label);
            Vertices.Add(v);
            VerticesDict.Add(label, v);
            visual.CreateVisualVertex(v);
        }

        /// <summary>
        /// Removes every edge connected with vertex intended to be removed.
        /// </summary>
        /// <param name="v">Vertex intended to be removed.</param>
        private void RemoveEdgesInVertex(Vertex v)
        {
            foreach (Edge e in v.Edges)
            {
                RemoveEdge(e);
            }
        }

        /// <summary>
        /// Removes Core vertex and related Visual vertex.
        /// </summary>
        /// <param name="v">Vertex to be removed.</param>
        public void RemoveVertex(Vertex v)
        {
            RemoveEdgesInVertex(v);
            Vertices.Remove(v);
            VerticesDict.Remove(v.Label);
            visual.RemoveVisualVertex(v);
        }

        /// <summary>
        /// Creates Core edge and related Visual edge.
        /// </summary>
        /// <param name="v1">Endpoint of creating edge.</param>
        /// <param name="v2">Endpoint of creating edge.</param>
        public void CreateEdge(Vertex v1, Vertex v2)
        {
            Edge e = new Edge(v1, v2);
            Edges.Add(e);
            v1.Edges.Add(e);
            v2.Edges.Add(e);
            visual.CreateVisualEdge(e);
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
        /// Loads graph from file ASD represenation, that ignores vertex position.
        /// </summary>
        /// <param name="filename">File being read.</param>
        public void LoadFromFile(string filename)
        {
            try
            {
                //TODO: Check representation correctness
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (sr.Peek() >= 0)
                    {
                        String nWord = sr.ReadLine();
                        String mWord = sr.ReadLine();
                        int n = Int32.Parse(nWord);
                        int m = Int32.Parse(mWord);
                        double angle = 2 * Math.PI / n;
                        for (int i = 1; i <= n; ++i)
                        {
                            double alpha = angle * (i - 1);
                            double x = Math.Sin(alpha);
                            double y = Math.Cos(alpha);
                            CreateVertex(x, y, i.ToString());
                        }
                        for (int i = 0; i < m; ++i)
                        {
                            String line = sr.ReadLine();
                            string[] edgeEndpoints = line.Split();
                            Vertex v1 = verticesDict[edgeEndpoints[0]];
                            Vertex v2 = verticesDict[edgeEndpoints[1]];
                            CreateEdge(v1, v2);
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            visual.Refresh();
        }

        /// <summary>
        /// Saves graph ignoring vertex positions.
        /// </summary>
        /// <param name="filename">File for saved graph.</param>
        public void SaveGraph(string filename)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                file.WriteLine(Vertices.Count);
                file.WriteLine(Edges.Count);
                foreach (Edge e in Edges)
                {
                    String edgeLine = e.V1.Label + " " + e.V2.Label;
                    file.WriteLine(edgeLine);
                }
            }
        }

        /// <summary>
        /// Saves graph keeping vertex position.
        /// </summary>
        /// <param name="filename">File for saved graph</param>
        public void SaveVisualGraph(string filename)
        {
            //TODO
        }
    }
}

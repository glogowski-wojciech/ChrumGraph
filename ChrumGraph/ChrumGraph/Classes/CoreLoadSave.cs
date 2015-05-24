using System;
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
                    visual.Clear();
                    vertices.Clear();
                    verticesDict.Clear();
                    edges.Clear();

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
            physics.StartSimulation(FPS);
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

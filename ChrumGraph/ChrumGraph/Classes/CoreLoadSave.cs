using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace ChrumGraph
{
    /// <summary>
    /// Graph logic.
    /// </summary>
    public partial class Core: IPhysicsCore, IVisualCore
    {
       // TODO: replace with false return of LoadFromFile function
        private void alert_incorrect_represenatation()
        {
            System.Windows.MessageBox.Show("Incorrect graph represenation");
            throw new FileNotFoundException(); //TODO: replace with better exception type.
        }

        private void create_graph(int n, List<Tuple<int, int>> file_edges,
            List<Tuple<int, string>> labels, List<Tuple<int, double, double>> positions)
        {
            double angle = 2 * Math.PI / n;
            for (int i = 1; i <= n; ++i)
            {
                double alpha = angle * (i - 1);
                double x = Math.Sin(alpha);
                double y = Math.Cos(alpha);
                CreateVertex(x, y, i.ToString());
            }
            foreach (Tuple<int, int> endpoints in file_edges)
            {
                Vertex v1 = Vertices[endpoints.Item1];
                Vertex v2 = Vertices[endpoints.Item2];
                CreateEdge(v1, v2);
            }
            foreach (Tuple<int, string> l in labels)
            {
                Vertices[l.Item1].Label = l.Item2;
            }
            foreach (Tuple<int, double, double> p in positions)
            {
                Vertices[p.Item1].X = p.Item2;
                Vertices[p.Item1].Y = p.Item3;
            }
        }

        private void clear_graph()
        {
            visual.Clear();
            vertices.Clear();
            verticesDict.Clear();
            edges.Clear();
        }


        /// <summary>
        /// Loads graph from file ASD represenation, that ignores vertex position.
        /// </summary>
        /// <param name="filename">File being read.</param>
        public void LoadFromFile(string filename)
        {
            int n = 0;
            List<Tuple<int, int>> fileEdges = new List<Tuple<int, int>>();
            List<Tuple<int, string>> labels = new List<Tuple<int, string>>();
            List<Tuple<int, double, double>> positions = new List<Tuple<int, double, double>>();

            string nPattern = @"^([1-9]\d*)$";
            string edgePattern = @"^(\d+)\s+(\d+)$";
            string labelPattern = @"^(\d+)\s(\S+)$";
            string positionPattern = @"^(\d+)\s(\d+.\d+)\s(\d+.\d+)$";
            string emptyLinePattern = @"(\n|\r|\r\n)";
            
            Regex nRegex = new Regex(nPattern, RegexOptions.None);
            Regex edgeRegex = new Regex(edgePattern, RegexOptions.None);
            Regex labelRegex = new Regex(labelPattern, RegexOptions.None);
            Regex positionRegex = new Regex(positionPattern, RegexOptions.None);
            Regex emptyLineRegex = new Regex(emptyLinePattern, RegexOptions.None);

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    const int edgeRead = 0;
                    const int labelRead = 1;
                    const int positionRead = 2;
                    
                    String line = sr.ReadLine();
                    String line2 = sr.ReadLine(); // TOREMOVE (and remove m parameter from file)
                    if (line != null && line2 != null) // TOCHANGE
                    {
                        Match match = nRegex.Match(line);
                        Match match2 = nRegex.Match(line2); // TOREMOVE
                        if (match.Success && match2.Success) //TOCHANGE
                            n = Int32.Parse(match.Groups[0].Value);
                        else
                            alert_incorrect_represenatation();
                    }
                    else
                        alert_incorrect_represenatation();


                    int state = edgeRead;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Match edgeMatch = edgeRegex.Match(line);
                        Match labelMatch = labelRegex.Match(line);
                        Match positionMatch = positionRegex.Match(line);
                        Match emptyLineMatch = emptyLineRegex.Match(line);
                        if (emptyLineMatch.Success)
                        {
                            ++state;
                            continue;
                        }
                        if (state == edgeRead && edgeMatch.Success)
                        {
                            int v1 = Int32.Parse(edgeMatch.Groups[1].Value);
                            int v2 = Int32.Parse(edgeMatch.Groups[2].Value);
                            if (v1 > n || v2 > n)
                                alert_incorrect_represenatation();
                            else
                                fileEdges.Add(new Tuple<int, int>(--v1, --v2));
                            continue;
                        }
                        if (state == labelRead && labelMatch.Success)
                        {
                            int v = Int32.Parse(labelMatch.Groups[1].Value);
                            if (v > n)
                                alert_incorrect_represenatation();
                            string label = labelMatch.Groups[2].Value;
                            labels.Add(new Tuple<int, string>(--v, label));
                            continue;
                        }
                        if (state == positionRead && positionMatch.Success)
                        {
                            int v = Int32.Parse(positionMatch.Groups[1].Value);
                            if (v > n)
                                alert_incorrect_represenatation();
                            double d1 = Convert.ToDouble(positionMatch.Groups[2].Value);
                            double d2 = Convert.ToDouble(positionMatch.Groups[3].Value);
                            positions.Add(new Tuple<int, double, double>(--v, d1, d2));
                            continue;
                        }
                        //alert_incorrect_represenatation(); // optional
                    }
                }
                clear_graph();
                create_graph(n, fileEdges, labels, positions);
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

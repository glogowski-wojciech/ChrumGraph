using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChrumGraph;

namespace ChrumGraphTest
{
    [TestClass]
    public class AccessTests
    {
        [TestMethod]
        public void AccessTest1()
        {
            IPhysicsCore c = new Core();
            IPhysics p = new Physics(c);
            Vertex v1 = new Vertex(1.0, 1.0);
            Vertex v2 = new Vertex(1.0, 1.0);
            Edge e1 = new Edge(v1, v2);
        }
    }

    [TestClass]
    public class PhysicsTests
    {
        [TestMethod]
        public void ParametersModifiersTest()
        {
            IPhysicsCore c = new Core();
            IPhysics p = new Physics(c);
            double v = p.VertexForceParam;
            double e = p.EdgeForceParam;
            double f = p.FrictionParam;
        }
    }

    [TestClass]
    public class LoadSaveTest
    {
        private void WriteOnConsole(Core c)
        {
            Console.WriteLine("Vertices:");
            foreach (Vertex v in c.Vertices)
            {
                Console.WriteLine("label = " + v.Label);
                string tab = "   ";
                Console.WriteLine(tab + "x = " + Math.Round(v.X, 2));
                Console.WriteLine(tab + "y = " + Math.Round(v.Y, 2));
            }
            Console.WriteLine("Edges:");
            foreach (Edge e in c.Edges)
            {
                Console.Write(e.V1.Label);
                Console.Write(", ");
                Console.Write(e.V2.Label);
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void Load()
        {
            Core c = new Core();
            //Below you should use your own path to your own load.txt file
            //string file1 = @"LoadSaveFiles\load.txt";
            //c.LoadFromFile(file1);

            WriteOnConsole(c);
        }
    }
}

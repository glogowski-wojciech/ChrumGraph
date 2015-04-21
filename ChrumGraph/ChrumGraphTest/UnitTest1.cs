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
}

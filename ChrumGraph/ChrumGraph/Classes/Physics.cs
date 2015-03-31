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
        public Physics(IPhysicsCore physicsCore)
        {
            this.physicsCore = physicsCore;
            vertices = physicsCore.Vertices;
            edges = physicsCore.Edges;
            dispatcherTimer = new DispatcherTimer();
            Simulate = false;
        }

        public void StartSimulation(double fps) //TODO
        {
            dispatcherTimer.Tick += (sender, e) => { IterateSimulation(); };
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(100.0 / fps)); // TODO
            dispatcherTimer.Start();
        }

        public void StartSimulation()
        {
            Simulate = true;
            while (Simulate)
            {
                IterateSimulation();
            }
        }

        public void StopSimulation()
        {
            Simulate = false;
        }

        private void IterateSimulation()
        {

        }

        private IPhysicsCore physicsCore;
        private List<Vertex> vertices;
        private List<Edge> edges;
        private DispatcherTimer dispatcherTimer;
        private bool simulate;
        private object simulateMutex = new object();
        private bool Simulate
        {
            set
            {
                lock(simulateMutex)
                {
                    simulate = value;
                }
            }

            get
            {
                bool returnValue;
                lock(simulateMutex)
                {
                    returnValue = Simulate;
                }
                return returnValue;
            }
        }
    };

}

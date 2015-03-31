using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChrumGraph
{
    interface IPhysics
    {
        // ... parameters
        void StartSimulation(double fps); // Mutex(Core)
        void StartSimulation();
        void StopSimulation();
    }
}

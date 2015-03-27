/*

// WG: Vertex, Edge, Core are classes, not interfaces but might be helpful
// for defining interfaces. To be deleted?`

class Vertex : PhysicsVertex, VisualVertex
{
    double X;
    double Y;
    List<Shape> Shapes;
    List<Edge> Edges;
    bool Pinned;
    bool PositionForced;
    double ForcedX, ForcedY;
};

class Edge : PhysicsEdge, VisualEdge
{
    Vertex V1;
    Vertex V2;
    Shape Shape;
};

class Core : PhysicsCore, VisualCore
{
    private Core();
    static Core Instance;
    void CreateVertex(double x, double y);
    void RemoveVertex(Vertex v);
    void CreateEdge(Vertex v1, Vertex v2);
    void RemoveEdge(Edge e);
    void SimulationFinished();
    void Pin(Vertex v); // Mutex(Core)
    void Unpin(Vertex v); // Mutex(Core)
    void SetPosition(Vertex v, double x, double y); // Mutex(Core)
    void LoadGraphFromFile(string filename);
    void SaveGraphToFile(string filename);
    void SaveVisualGraphToFile(string filename);
    List<Vertex> Vertices
    {
        get;
        set;
    }
    List<Edge> Edges
    {
        get;
        set;
    }
    DispatcherTimer timer;
};

*/

/* Interface of Core for Visual */

interface VisualCore
{
    void CreateVertex(double x, double y);
    void RemoveVertex(Vertex v);
    void CreateEdge(Vertex v1, Vertex v2);
    void RemoveEdge(Edge e);
    void Pin(Vertex v); // Mutex(Core)
    void Unpin(Vertex v); // Mutex(Core)
    void SetPosition(Vertex v, double x, double y); // Mutex(Core)
    void LoadFromFile(string filename);
    void SaveGraph(string filename);
    void SaveVisualGraph(string filename);
};

/* Interface of Core for Physics */

interface PhysicsCore
{
    List<PhysicsVertex> Vertices
    {
        get;
        set;
    }
    List<PhysicsEdge> Edges
    {
        get;
        set;
    }
    static PhysicsCore Instance; // for Mutex
    void SimulationFinished();
};


/* Interface of Visual for Core */

interface Visual
{
    private Visual();
    static Visual Instance;
    void CreateVisualVertex(Vertex v);
    void RemoveVisualVertex(Vertex v);
    void CreateVisualEdge(Edge e);
    void RemoveVisualEdge(Edge e);
    bool Visible;
    void Refresh();
};

/* Interface of Physics for Core */

interface Physics
{
    // ... parameters
    Physics(PhysicsCore physicsCore);
    void StartSimulation(double fps); // Mutex(Core)
    void StartSimulation();
    void StopSimulation();
};

/* Vertex, Edge interfaces for Physics */

interface PhysicsVertex
{
    double X
    {
        get;
        set;
    }
    double Y
    {
        get;
        set;
    }
    System.Collections.Generic.List<Edge> Edges
    {
        get;
        set;
    }
};

interface PhysicsEdge
{
    PhysicsVertex V1
    {
        get;
        set;
    }
    PhysicsVertex V2
    {
        get;
        set;
    }
};


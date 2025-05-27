using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Blazor.Core;

[Serializable]
public class SimulationFrame
{
    public BodyPosition Body1 { get; set; }
    public BodyPosition Body2 { get; set; }
    public BodyPosition Body3 { get; set; }
    public BodyPosition CenterOfMass { get; set; }
    public double Time { get; set; }
}

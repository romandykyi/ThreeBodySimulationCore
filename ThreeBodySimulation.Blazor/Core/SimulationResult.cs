namespace ThreeBodySimulation.Blazor.Core;

[Serializable]
public class SimulationResult(IList<SimulationFrame> frames, double interval)
{
    public IList<SimulationFrame> SimulationFrames { get; set; } = frames;
    public double Interval { get; set; } = interval;
}

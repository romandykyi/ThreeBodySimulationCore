using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Blazor.Core;

public class SimulationParams
{
    public SolverType Solver { get; set; }
    public double G { get; set; } = 1;
    public double StepSize { get; set; } = 0.0005;
    public double SimulationTime { get; set; } = 10;

    public Body Body1 { get; set; } = new(
        new(-0.60288589811652, 0.059162128863347, 0),
        new(0.122913546623784, 0.747443868604908, 0),
        1);
    public Body Body2 { get; set; } = new(
        new(0.252709795391, 0.105825487222437, 0),
        new(-0.019325586404545, 1.369241993562101, 0),
        1);
    public Body Body3 { get; set; } = new(
        new(-0.355389016941814, 0.1038323764315145, 0),
        new(-0.103587960218793, -2.11668586216882, 0),
        1);
}

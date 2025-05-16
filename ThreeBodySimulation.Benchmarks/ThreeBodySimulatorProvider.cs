using ThreeBodySimulation.Data;
using ThreeBodySimulation.Simulation;
using ThreeBodySimulation.Simulation.Solvers;

namespace ThreeBodySimulation.Benchmarks;

public static class ThreeBodySimulatorProvider
{
    /// <summary>
    /// Provides a test simulator with pre-defined values for the given 
    /// <paramref name="solver"/>.
    /// </summary>
    /// <param name="solver">Solver to use.</param>
    /// <returns>A test simulator with pre-defined values.</returns>
    public static BodiesSimulator GetSimulator(IBodiesSolver solver)
    {
        Body body1 = new(new(4.5, 0, -3), new(2, 0, 0), 10);
        Body body2 = new(new(-3, 0, 3), new(0, 0, 0), 20);
        Body body3 = new(new(-6, -5, -5), new(0, 0, 0), 30);

        return new(body1, body2, body3, solver, 1);
    }
}

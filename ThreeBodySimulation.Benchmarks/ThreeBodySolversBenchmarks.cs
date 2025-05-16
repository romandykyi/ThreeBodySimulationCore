using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ThreeBodySimulation.Simulation.Solvers;

namespace ThreeBodySimulation.Benchmarks;

public class ThreeBodySolversBenchmarks
{
    private readonly Consumer _consumer = new Consumer();

    [Params(1e-3, 1e-4, 1e-5)]
    public double Step { get; set; }

    [Params(1)]
    public double Time { get; set; }

    private void Solve(IBodiesSolver solver)
    {
        var simulator = ThreeBodySimulatorProvider.GetSimulator(solver);
        var states = simulator.Simulate(startTime: 0.0, endTime: Time);

        foreach (var state in states)
            _consumer.Consume(state);
    }

    [Benchmark]
    public void Yoshida4()
    {
        Yoshida4BodiesSolver solver = new() { Step = Step };
        Solve(solver);
    }

    [Benchmark]
    public void RK4()
    {
        RK4BodiesSolver solver = new() { Step = Step };
        Solve(solver);
    }
}

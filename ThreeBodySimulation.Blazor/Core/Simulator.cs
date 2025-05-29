using ThreeBodySimulation.Blazor.Core.Extensions;
using ThreeBodySimulation.Simulation;
using ThreeBodySimulation.Simulation.Solvers;
using ThreeBodySimulation.Simulation.Utils;

namespace ThreeBodySimulation.Blazor.Core;

public class Simulator(SimulationParams simulationParams)
{
    private const int updateRate = 1000;
    public readonly SimulationParams SimulationParams = simulationParams;

    public async Task<SimulationResult?> RunAsync(CancellationToken cancellationToken, double visualizationStep = 0.01)
    {
        IBodiesSolver solver = SimulationParams.Solver switch
        {
            SolverType.RK4 => new RK4Solver() { Step = SimulationParams.StepSize },
            SolverType.Yoshida4 => new Yoshida4Solver() { Step = SimulationParams.StepSize },
            _ => throw new InvalidOperationException("Unsupported solver.")
        };

        var body1 = SimulationParams.Body1.Copy();
        var body2 = SimulationParams.Body2.Copy();
        var body3 = SimulationParams.Body3.Copy();

        BodiesSimulator simulator = new(body1, body2, body3, solver, SimulationParams.G);
        var states = simulator.Simulate(endTime: SimulationParams.SimulationTime);

        List<SimulationFrame> frames = [];

        double prevTime = double.NegativeInfinity;
        int index = 0;
        foreach (var state in states)
        {
            if (cancellationToken.IsCancellationRequested) return null;

            double timeStep = state.SimulationTime - prevTime;
            if (visualizationStep > 0.0 && timeStep < visualizationStep)
                continue;

            prevTime = state.SimulationTime;
            var com = SimulationUtils.CalculateCenterOfMass(
                    simulator.Body1,
                    simulator.Body2,
                    simulator.Body3
                    );
            SimulationFrame frame = new()
            {
                Body1 = state.Body1Position,
                Body2 = state.Body2Position,
                Body3 = state.Body3Position,
                CenterOfMass = com,
                Time = state.SimulationTime
            };
            frames.Add(frame);

            if (index++ % updateRate == 0)
            {
                await Task.Delay(1);
            }
        }

        return new SimulationResult(frames, visualizationStep);
    }
}

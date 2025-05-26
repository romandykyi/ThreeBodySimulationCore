using ThreeBodySimulation.Data;
using ThreeBodySimulation.Simulation;
using ThreeBodySimulation.Simulation.Solvers;
using ThreeBodySimulation.Simulation.Utils;

namespace ThreeBodySimulation.Blazor.Core;

public class Simulator(SimulationParams simulationParams)
{
    private const double updateRate = 0.125;
    public readonly SimulationParams SimulationParams = simulationParams;

    private static Body CopyBody(Body body)
    {
        return new Body(body.Position, body.Velocity, body.Mass);
    }

    public async Task<SimulationResult?> RunAsync(IProgress<double> progress,
        CancellationToken cancellationToken, double visualizationStep = 0.01)
    {
        IBodiesSolver solver = SimulationParams.Solver switch
        {
            SolverType.RK4 => new RK4Solver() { Step = SimulationParams.StepSize },
            SolverType.Yoshida4 => new Yoshida4Solver() { Step = SimulationParams.StepSize },
            _ => throw new InvalidOperationException("Unsupported solver.")
        };

        var body1 = CopyBody(SimulationParams.Body1);
        var body2 = CopyBody(SimulationParams.Body2);
        var body3 = CopyBody(SimulationParams.Body3);

        BodiesSimulator simulator = new(body1, body2, body3, solver, SimulationParams.G);
        var states = simulator.Simulate(endTime: SimulationParams.SimulationTime);

        List<SimulationFrame> frames = [];

        double prevTime = double.NegativeInfinity;
        double prevUpdateTime = double.NegativeInfinity;
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
                CenterOfMass = com
            };
            frames.Add(frame);

            if (state.SimulationTime - prevUpdateTime >= updateRate)
            {
                prevUpdateTime = state.SimulationTime;
                progress.Report(state.SimulationTime);
                await Task.Delay(1);
            }
        }

        return new SimulationResult(frames, visualizationStep);
    }
}

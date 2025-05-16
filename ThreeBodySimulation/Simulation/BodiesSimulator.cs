using System.Collections.Generic;
using ThreeBodySimulation.Data;
using ThreeBodySimulation.Simulation.Solvers;

namespace ThreeBodySimulation.Simulation
{
    /// <summary>
    /// A component that simulates the 3 bodies system.
    /// </summary>
    public class BodiesSimulator
    {
        /// <summary>
        /// Gets/sets the gravitational constant.
        /// </summary>
        public double G { get; set; }

        /// <summary>
        /// Gets/sets the first body.
        /// </summary>
        public Body Body1 { get; set; }
        /// <summary>
        /// Gets/sets the second body.
        /// </summary>
        public Body Body2 { get; set; }
        /// <summary>
        /// Gets/sets the third body.
        /// </summary>
        public Body Body3 { get; set; }
        /// <summary>
        /// Gets/sets the equation solver that is used.
        /// </summary>
        public IBodiesSolver Solver { get; set; }

        public BodiesSimulator(
            Body body1, Body body2, Body body3, IBodiesSolver solver, double g = 1.0
            )
        {
            Body1 = body1;
            Body2 = body2;
            Body3 = body3;
            Solver = solver;
            G = g;
        }

        private SimulationState GetSimulationState(double time)
        {
            return new SimulationState(
                   Body1.Position, Body1.Velocity,
                   Body2.Position, Body2.Velocity,
                   Body3.Position, Body3.Velocity,
                   time
                   );
        }

        /// <summary>
        /// Simulates the three bodies problem.
        /// </summary>
        /// <param name="startTime">The starting time.</param>
        /// <param name="endTime">
        /// The time after which the simulation will be stopped. 
        /// Leave as infinity to simulate forever.
        /// </param>
        /// <returns>
        /// An enumerable containing the simulation states.
        /// </returns>
        public IEnumerable<SimulationState> Simulate(
            double startTime = 0.0,
            double endTime = double.PositiveInfinity
            )
        {
            if (startTime > endTime) yield break;

            yield return GetSimulationState(startTime);

            // Kahan summation
            double time = startTime;
            double c = 0.0;
            while (time < endTime)
            {
                double step = Solver.SolveStep(time, Body1, Body2, Body3, G);

                double y = step - c;
                double t = time + y;
                c = t - time - y;
                time = t;

                yield return GetSimulationState(time);
            }
        }
    }
}

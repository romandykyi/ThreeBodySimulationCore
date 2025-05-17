using System;
using ThreeBodySimulation.Data;
using ThreeBodySimulation.Numeric;

namespace ThreeBodySimulation.Simulation.Solvers
{
    /// <summary>
    /// A solver that uses the Runge-Kutta (4th order) method.
    /// </summary>
    [Obsolete("Consider using RK4Solver for better performance.")]
    public class RK4BodiesSolver : IFixedStepBodiesSolver
    {
        /// <summary>
        /// Gets/sets the step size.
        /// </summary>
        public double Step { get; set; } = 0.03125;

        public double SolveStep(double time, Body body1, Body body2, Body body3, double g)
        {
            var func = ThreeBodyMath.GetDiffFunction(body1.Mass, body2.Mass, body3.Mass, g);

            var input = ThreeBodyMath.GetInputVector(body1, body2, body3);
            var result = RK4.SolveStepVector(time, input, Step, func);
            ThreeBodyMath.ApplySolution(result, body1, body2, body3);

            return Step; // Fixed step length
        }
    }
}

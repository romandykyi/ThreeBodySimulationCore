using System;
using ThreeBodySimulation.Data;
using ThreeBodySimulation.Numeric;

namespace ThreeBodySimulation.Simulation.Solvers
{
    /// <summary>
    /// A solver that uses the Yoshida (4th order) method.
    /// </summary>
    public class Yoshida4Solver : IFixedStepBodiesSolver
    {
        /// <summary>
        /// Gets/sets the step size.
        /// </summary>
        public double Step { get; set; } = 0.03125;

        public double SolveStep(double time, Body body1, Body body2, Body body3, double g)
        {
            var func = ThreeBodyMath.GetSpanDiffFunction(body1.Mass, body2.Mass, body3.Mass, g);

            Span<double> vector = stackalloc double[18];
            ThreeBodyMath.InputToSpan(body1, body2, body3, vector);
            Yoshida4.SolveStepSpan(time, vector, Step, func, vector);
            ThreeBodyMath.ApplySolution(vector, body1, body2, body3);

            return Step; // Fixed step length
        }
    }
}

using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Simulation.Solvers
{
    /// <summary>
    /// An interface that represents the 3 bodies problem solver.
    /// </summary>
    public interface IBodiesSolver
    {
        /// <summary>
        /// Solves the 3 bodies problem step.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        /// <param name="body1">The first body.</param>
        /// <param name="body2">The second body.</param>
        /// <param name="body3">The third body.</param>
        /// <param name="g">The gravitational constant.</param>
        /// <returns>A size of the step.</returns>
        public double SolveStep(double time, Body body1, Body body2, Body body3, double g);
    }
}

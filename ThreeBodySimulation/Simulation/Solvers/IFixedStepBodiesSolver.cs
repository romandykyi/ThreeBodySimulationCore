namespace ThreeBodySimulation.Simulation.Solvers
{
    public interface IFixedStepBodiesSolver : IBodiesSolver
    {
        /// <summary>
        /// Gets/sets the step size.
        /// </summary>
        public double Step { get; set; }
    }
}

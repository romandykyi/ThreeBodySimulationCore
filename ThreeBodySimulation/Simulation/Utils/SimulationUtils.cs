using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Simulation.Utils
{
    public static class SimulationUtils
    {
        /// <summary>
        /// Calculates the center of mass of the three bodies.
        /// </summary>
        /// <param name="body1">The first body.</param>
        /// <param name="body2">The second body.</param>
        /// <param name="body3">The third body.</param>
        /// <returns>The center of mass of the three bodies.</returns>
        public static BodyPosition CalculateCenterOfMass(Body body1, Body body2, Body body3)
        {
            double massSum = body1.Mass + body2.Mass + body3.Mass;
            double SingleAxis(double r1, double r2, double r3)
            {
                return (r1 * body1.Mass + r2 * body2.Mass + r3 * body3.Mass) / massSum;
            }

            return new BodyPosition(
                SingleAxis(body1.Position.X, body2.Position.X, body3.Position.X),
                SingleAxis(body1.Position.Y, body2.Position.Y, body3.Position.Y),
                SingleAxis(body1.Position.Z, body2.Position.Z, body3.Position.Z)
                );
        }
    }
}

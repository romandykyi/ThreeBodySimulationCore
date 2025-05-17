using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Simulation.Utils
{
    /// <summary>
    /// A class that allows to calculate kinetic/potential energy for the
    /// given three bodies. 
    /// </summary>
    public class EnergyCalculator
    {
        public Body Body1 { get; }
        public Body Body2 { get; }
        public Body Body3 { get; }
        public double G { get; }

        public EnergyCalculator(BodiesSimulator simulator)
        {
            Body1 = simulator.Body1;
            Body2 = simulator.Body2;
            Body3 = simulator.Body3;
            G = simulator.G;
        }

        public EnergyCalculator(Body body1, Body body2, Body body3, double g = 1.0)
        {
            Body1 = body1;
            Body2 = body2;
            Body3 = body3;
            G = g;
        }

        /// <summary>
        /// Calculates a kinetic energy of 3 bodies.
        /// </summary>
        /// <returns>A kinetic energy of 3 bodies.</returns>
        public double CalculateKineticEnergy()
        {
            return
                0.5 * Body1.Mass * Body1.Velocity.SqrMagnitude() +
                0.5 * Body2.Mass * Body2.Velocity.SqrMagnitude() +
                0.5 * Body3.Mass * Body3.Velocity.SqrMagnitude();
        }

        /// <summary>
        /// Calculates a potential energy of 3 bodies.
        /// </summary>
        /// <returns>A potential energy of 3 bodies.</returns>
        public double CalculatePotentialEnergy()
        {
            double r12 = BodyPosition.Distance(Body1.Position, Body2.Position);
            double r13 = BodyPosition.Distance(Body1.Position, Body3.Position);
            double r23 = BodyPosition.Distance(Body2.Position, Body3.Position);
            return -G * (
                Body1.Mass * Body2.Mass / r12 +
                Body1.Mass * Body3.Mass / r13 +
                Body2.Mass * Body3.Mass / r23
                );
        }

        /// <summary>
        /// Calculates a total energy of 3 bodies.
        /// </summary>
        /// <returns>A total energy of 3 bodies.</returns>
        public double CalculateTotalEnergy()
        {
            return CalculateKineticEnergy() + CalculatePotentialEnergy();
        }
    }
}

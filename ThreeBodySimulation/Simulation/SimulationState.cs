using System;
using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Simulation
{
    /// <summary>
    /// A class that represents the simulation state.
    /// </summary>
    [Serializable]
    public class SimulationState
    {
        /// <summary>
        /// Gets the first body position.
        /// </summary>
        public BodyPosition Body1Position { get; }
        /// <summary>
        /// Gets the first body's velocity.
        /// </summary>
        public BodyPosition Body1Velocity { get; }
        /// <summary>
        /// Gets the second body's position.
        /// </summary>
        public BodyPosition Body2Position { get; }
        /// <summary>
        /// Gets the second body's velocity.
        /// </summary>
        public BodyPosition Body2Velocity { get; }
        /// <summary>
        /// Gets the third body's position.
        /// </summary>
        public BodyPosition Body3Position { get; }
        /// <summary>
        /// Gets the third body's velocity.
        /// </summary>
        public BodyPosition Body3Velocity { get; }

        /// <summary>
        /// Gets the current simulation time.
        /// </summary>
        public double SimulationTime { get; }

        public SimulationState(
            BodyPosition body1Position,
            BodyPosition body1Velocity,
            BodyPosition body2Position,
            BodyPosition body2Velocity,
            BodyPosition body3Position,
            BodyPosition body3Velocity,
            double simulationTime)
        {
            Body1Position = body1Position;
            Body1Velocity = body1Velocity;
            Body2Position = body2Position;
            Body2Velocity = body2Velocity;
            Body3Position = body3Position;
            Body3Velocity = body3Velocity;
            SimulationTime = simulationTime;
        }
    }
}

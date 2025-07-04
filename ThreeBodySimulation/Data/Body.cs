namespace ThreeBodySimulation.Data
{
    /// <summary>
    /// A class that represents the celestial body.
    /// </summary>
    [System.Serializable]
    public class Body
    {
        /// <summary>
        /// Gets/sets the body's position.
        /// </summary>
        public BodyPosition Position { get; set; }
        /// <summary>
        /// Gets/sets the body's velocity.
        /// </summary>
        public BodyPosition Velocity { get; set; }
        /// <summary>
        /// Gets/sets the body's mass.
        /// </summary>
        public double Mass { get; set; }

        public Body() { }

        public Body(BodyPosition position, BodyPosition velocity, double mass)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
        }
    }
}

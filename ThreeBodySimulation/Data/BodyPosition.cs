using System;

namespace ThreeBodySimulation.Data
{
    /// <summary>
    /// A structure that represents the body position.
    /// </summary>
    [Serializable]
    public struct BodyPosition
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public BodyPosition(double x, double y, double z) : this()
        {
            X = x; Y = y; Z = z;
        }
    }
}

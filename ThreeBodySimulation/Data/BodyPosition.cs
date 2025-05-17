using System;
using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double Magnitude() => Math.Sqrt(SqrMagnitude());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double SqrMagnitude() => X * X + Y * Y + Z * Z;

        public static double Distance(BodyPosition a, BodyPosition b)
        {
            return Math.Sqrt(
                (a.X - b.X) * (a.X - b.X) +
                (a.Y - b.Y) * (a.Y - b.Y) +
                (a.Z - b.Z) * (a.Z - b.Z)
                );
        }
    }
}

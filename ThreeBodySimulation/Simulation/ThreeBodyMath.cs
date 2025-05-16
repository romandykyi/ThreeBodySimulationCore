using System;
using System.Runtime.CompilerServices;
using ThreeBodySimulation.Data;
using ThreeBodySimulation.Numeric;
using static System.Math;

namespace ThreeBodySimulation.Simulation
{
    /// <summary>
    /// A class that provides the three body problem equations.
    /// </summary>
    /// <remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Three-body_problem" />.
    /// </remarks>
    public static class ThreeBodyMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Sqr(double x) => x * x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Cube(double x) => x * x * x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double GetDistance(
            double x1, double y1, double z1,
            double x2, double y2, double z2)
        {
            return Sqrt(Sqr(x1 - x2) + Sqr(y1 - y2) + Sqr(z1 - z2));
        }

        private static double AxisMotionEquation(
            double r1, double r2, double r3,
            double d12, double d13,
            double m2, double m3, double g)
        {
            return -g * m2 * (r1 - r2) / Cube(d12) - g * m3 * (r1 - r3) / Cube(d13);
        }

        /// <summary>
        /// Gets the differential equation function with the given parameters.
        /// </summary>
        /// <param name="m1">The mass of the first body.</param>
        /// <param name="m2">The mass of the second body.</param>
        /// <param name="m3">The mass of the third body.</param>
        /// <param name="g">The gravitational constant.</param>
        /// <remarks>
        /// The vector has this format: { x1, y1, z1, x2, ..., z3, vx1, vy1, vz1, ..., vz3 }.
        /// </remarks>
        /// <returns>
        /// The differential equation function that uses the given parameters.
        /// </returns>
        public static VectorDiffFunc GetDiffFunction(double m1, double m2, double m3, double g)
        {
            return (t, y) =>
            {
                double x1 = y[0], y1 = y[1], z1 = y[2];
                double x2 = y[3], y2 = y[4], z2 = y[5];
                double x3 = y[6], y3 = y[7], z3 = y[8];

                double vx1 = y[9], vy1 = y[10], vz1 = y[11];
                double vx2 = y[12], vy2 = y[13], vz2 = y[14];
                double vx3 = y[15], vy3 = y[16], vz3 = y[17];

                double d12 = GetDistance(x1, y1, z1, x2, y2, z2);
                double d13 = GetDistance(x1, y1, z1, x3, y3, z3);
                double d23 = GetDistance(x2, y2, z2, x3, y3, z3);

                return new double[18]
                {
                    vx1, vy1, vz1, // First body
                    vx2, vy2, vz2, // Second body
                    vx3, vy3, vz3, // Third body
                    
                    // First body
                    AxisMotionEquation(x1, x2, x3, d12, d13, m2, m3, g),
                    AxisMotionEquation(y1, y2, y3, d12, d13, m2, m3, g),
                    AxisMotionEquation(z1, z2, z3, d12, d13, m2, m3, g),

                    // Second body
                    AxisMotionEquation(x2, x3, x1, d23, d12, m3, m1, g),
                    AxisMotionEquation(y2, y3, y1, d23, d12, m3, m1, g),
                    AxisMotionEquation(z2, z3, z1, d23, d12, m3, m1, g),

                    // Third body
                    AxisMotionEquation(x3, x1, x2, d13, d23, m1, m2, g),
                    AxisMotionEquation(y3, y1, y2, d13, d23, m1, m2, g),
                    AxisMotionEquation(z3, z1, z2, d13, d23, m1, m2, g),
                };
            };
        }

        /// <summary>
        /// Extracts values from the bodies and places them into the vector 
        /// that can be used to solve the ode system.
        /// </summary>
        /// <param name="body1">The first body.</param>
        /// <param name="body2">The second body.</param>
        /// <param name="body3">The third body.</param>
        /// <returns>The vector that can be used to solve the ode system.</returns>
        public static double[] GetInputVector(Body body1, Body body2, Body body3)
        {
            return new double[18]
            {
                body1.Position.X, body1.Position.Y, body1.Position.Z,
                body2.Position.X, body2.Position.Y, body2.Position.Z,
                body3.Position.X, body3.Position.Y, body3.Position.Z,
                body1.Velocity.X, body1.Velocity.Y, body1.Velocity.Z,
                body2.Velocity.X, body2.Velocity.Y, body2.Velocity.Z,
                body3.Velocity.X, body3.Velocity.Y, body3.Velocity.Z,
            };
        }

        /// <summary>
        /// Extracts values from the <paramref name="vector"/> and applies them
        /// to each body.
        /// </summary>
        /// <param name="vector">The solution vector.</param>
        /// <param name="body1">The first body.</param>
        /// <param name="body2">The second body.</param>
        /// <param name="body3">The third body.</param>
        public static void ApplySolution(double[] vector, Body body1, Body body2, Body body3)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));
            if (vector.Length != 18)
            {
                throw new ArgumentException($"The \"{nameof(vector)}\" size is invalid." +
                    $" Expected 18 but was {vector.Length}.", nameof(vector));
            }

            body1.Position = new(vector[0], vector[1], vector[2]);
            body2.Position = new(vector[3], vector[4], vector[5]);
            body3.Position = new(vector[6], vector[7], vector[8]);

            body1.Velocity = new(vector[9], vector[10], vector[11]);
            body2.Velocity = new(vector[12], vector[13], vector[14]);
            body3.Velocity = new(vector[15], vector[16], vector[17]);
        }
    }
}

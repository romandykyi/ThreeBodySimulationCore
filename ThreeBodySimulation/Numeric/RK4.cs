using System;
using static ThreeBodySimulation.Numeric.VectorHelper;

namespace ThreeBodySimulation.Numeric
{
    /// <summary>
    /// Runge-Kutta method solver.
    /// </summary>
    public static class RK4
    {
        /// <summary>
        /// Performs a single RK4 step to approximate the next value of y.
        /// </summary>
        /// <param name="t">Current time value.</param>
        /// <param name="y">Current value of the solution.</param>
        /// <param name="step">Step size (time increment).</param>
        /// <param name="f">Function representing the differential equation.</param>
        /// <returns>An approximate solution at t + step.</returns>
        /// <exception cref="ArgumentException">
        /// The <paramref name="step"/> is not a positive real number.
        /// </exception>
        /// <exception cref="ArgumentNullException" />
        public static double SolveStep(double t, double y, double step, DiffFunc f)
        {
            if (step <= 0.0 || !double.IsFinite(step))
                throw new ArgumentException($"{nameof(step)} must be a positive real number.");
            if (f == null)
                throw new ArgumentNullException(nameof(f));

            double k1 = f(t, y);
            double k2 = f(t + step / 2, y + step * k1 / 2);
            double k3 = f(t + step / 2, y + step * k2 / 2);
            double k4 = f(t + step, y + step * k3);

            return y + step / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
        }

        public static double[] SolveStepVector(double t, double[] y, double step, VectorDiffFunc f)
        {
            if (step <= 0.0 || !double.IsFinite(step))
                throw new ArgumentException($"{nameof(step)} must be a positive real number.");
            if (f == null)
                throw new ArgumentNullException(nameof(f));

            int n = y.Length;

            double[] k1 = f(t, y);
            double[] k2 = f(t + step / 2, AddVectors(y, ScaleVector(k1, step / 2)));
            double[] k3 = f(t + step / 2, AddVectors(y, ScaleVector(k2, step / 2)));
            double[] k4 = f(t + step, AddVectors(y, ScaleVector(k3, step)));

            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = y[i] + step / 6 * (k1[i] + 2 * k2[i] + 2 * k3[i] + k4[i]);
            }

            return result;
        }
    }
}
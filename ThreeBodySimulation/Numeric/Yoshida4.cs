using System;

namespace ThreeBodySimulation.Numeric
{
    /// <summary>
    /// 4th-order Yoshida integrator.
    /// </summary>
    public static class Yoshida4
    {
        /// <summary>
        /// Performs a single Yoshida step for vector-valued ODEs.
        /// </summary>
        /// <param name="t">Current time.</param>
        /// <param name="y">State vector.</param>
        /// <param name="step">Step size.</param>
        /// <param name="f">Function returning derivative.</param>
        /// <returns>New state vector after one step.</returns>
        public static double[] SolveStepVector(double t, double[] y, double step, VectorDiffFunc f)
        {
            if (step <= 0.0 || !double.IsFinite(step))
                throw new ArgumentException($"{nameof(step)} must be a positive real number.");
            if (f == null)
                throw new ArgumentNullException(nameof(f));

            int n = y.Length / 2; // half = positions, half = velocities
            double[] result = (double[])y.Clone();

            // Yoshida coefficients
            double s = Math.Pow(2.0, 1.0 / 3.0);
            double w0 = -s / (2.0 - s);
            double w1 = 1.0 / (2.0 - s);

            double[] c = { w1 / 2, (w0 + w1) / 2, (w0 + w1) / 2, w1 / 2 };
            double[] d = { w1, w0, w1 };

            for (int i = 0; i < 3; i++)
            {
                // Position update (half step)
                for (int j = 0; j < n; j++)
                    result[j] += c[i] * step * result[j + n];

                // Compute accelerations
                double[] dydt = f(t, result);

                // Velocity update (full step)
                for (int j = 0; j < n; j++)
                    result[j + n] += d[i] * step * dydt[j + n];

                t += d[i] * step;

                // Position update (half step)
                for (int j = 0; j < n; j++)
                    result[j] += c[i + 1] * step * result[j + n];
            }

            return result;
        }
    }
}

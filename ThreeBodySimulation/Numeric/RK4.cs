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

        /// <summary>
        /// Performs a single RK4 step for vector-valued ODEs.
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

        /// <summary>
        /// Performs a single RK4 step for vector-valued ODEs stored on spans.
        /// </summary>
        /// <param name="t">Current time.</param>
        /// <param name="y">State vector.</param>
        /// <param name="step">Step size.</param>
        /// <param name="f">Function returning derivative.</param>
        /// <param name="res">
        /// A span that will contain the result of the operation (must be pre allocated).
        /// </param> 
        public static void SolveStepSpan(double t, Span<double> y, double step,
            SpanDiffFunc f, Span<double> res)
        {
            if (step <= 0.0 || !double.IsFinite(step))
                throw new ArgumentException($"{nameof(step)} must be a positive real number.");
            if (f == null)
                throw new ArgumentNullException(nameof(f));

            int n = y.Length;

            Span<double> k1 = stackalloc double[n];
            f(t, y, k1);

            Span<double> k2 = stackalloc double[n];
            ScaleSpan(k1, step / 2, k2);
            AddSpans(y, k2, k2);
            f(t + step / 2, k2, k2);

            Span<double> k3 = stackalloc double[n];
            ScaleSpan(k2, step / 2, k3);
            AddSpans(y, k3, k3);
            f(t + step / 2, k3, k3);

            Span<double> k4 = stackalloc double[n];
            ScaleSpan(k3, step, k4);
            AddSpans(y, k4, k4);
            f(t + step, k4, k4);

            for (int i = 0; i < n; i++)
            {
                res[i] = y[i] + step / 6 * (k1[i] + 2 * k2[i] + 2 * k3[i] + k4[i]);
            }
        }
    }
}
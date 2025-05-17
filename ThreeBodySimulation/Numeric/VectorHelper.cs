using System;
using System.Linq;

namespace ThreeBodySimulation.Numeric
{
    public static class VectorHelper
    {
        public static double[] AddVectors(double[] a, double[] b)
        {
            return a.Zip(b, (x, y) => x + y).ToArray();
        }

        public static double[] ScaleVector(double[] v, double scalar)
        {
            return v.Select(x => x * scalar).ToArray();
        }

        public static void AddSpans(Span<double> a, Span<double> b, Span<double> res)
        {
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] + b[i];
            }
        }

        public static void ScaleSpan(Span<double> v, double scalar, Span<double> res)
        {
            for (int i = 0; i < v.Length; i++)
            {
                res[i] = scalar * v[i];
            }
        }
    }
}
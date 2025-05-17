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
    }
}
namespace ThreeBodySimulation.Numeric
{
    /// <summary>
    /// A delegate that represents a differential equation function.
    /// </summary>
    /// <param name="t">The current time.</param>
    /// <param name="y">The current value of the solution.</param>
    /// <returns></returns>
    public delegate double DiffFunc(double t, double y);

    /// <summary>
    /// A delegate that represents a differential equation function for vectors.
    /// </summary>
    /// <param name="t">The current time.</param>
    /// <param name="y">The current value of the solution.</param>
    /// <returns></returns>
    public delegate double[] VectorDiffFunc(double t, double[] y);
}

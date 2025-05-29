using ThreeBodySimulation.Data;

namespace ThreeBodySimulation.Blazor.Core.Extensions;

public static class BodyExtensions
{
    public static Body Copy(this Body body)
    {
        return new Body(body.Position, body.Velocity, body.Mass);
    }
}

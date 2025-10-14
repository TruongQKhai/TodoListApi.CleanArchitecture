namespace WebAPI.Infrustructure;

/// <summary>
/// Follow Template Method pattern
/// </summary>
public abstract class EndpointGroupBase
{
    public virtual string? GroupName { get; }
    public abstract void Map(RouteGroupBuilder groupBuilder);
}

using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Infrustructure;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapPost(
        this IEndpointRouteBuilder builder, 
        Delegate handler,
        [StringSyntax("Rotue")] string pattern = "")
    {
        var endpoint = builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name);

        return endpoint;
    }

    public static RouteHandlerBuilder MapGet(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        var endpoint = builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name);

        return endpoint;
    }

    public static RouteHandlerBuilder MapPut(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        var endpoint = builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name);

        return endpoint;
    }

    public static RouteHandlerBuilder MapDelete(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        var endpoint = builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);

        return endpoint;
    }
}

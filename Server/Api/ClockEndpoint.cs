using Fastclock.Contracts;
using Fastclock.Server.Clocks;

namespace Fastclock.Server.Api;

internal static class ClockEndpoint
{
    public static void MapClockEndpoint(this IEndpointRouteBuilder app) =>
        app.MapGet("/api/clocks/{name}", (string name, ClockServer server) =>
        {
            var clock = server.Instance(name);
            return clock is not null ? Results.Ok(clock.Status) : Results.NotFound();
        })
        .WithName("GetClock")
        .Produces<ClockStatus>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
}

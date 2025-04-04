using NSubstitute.Core;

namespace NSubstitute.Routing.Handlers;

internal sealed class ReturnConfiguredResultHandler(ICallResults callResults) : ICallHandler
{
    public RouteAction Handle(ICall call)
    {
        if (callResults.TryGetResult(call, out var configuredResult))
        {
            return RouteAction.Return(configuredResult);
        }

        return RouteAction.Continue();
    }
}
namespace NSubstitute.Exceptions;

public sealed class CouldNotConfigureCallBaseException(string message) : SubstituteException(message)
{
    private const string CannotConfigureSingleCallMessage =
        "Cannot configure the base method call as base method implementation is missing. " +
        "You can call base method only if you create a class substitute and the method is not abstract.";

    private const string CannotConfigureAllCallsMessage =
        "Base method calls can be configured for a class substitute only, " +
        "as otherwise base implementation does not exist.";

    internal static CouldNotConfigureCallBaseException ForSingleCall() =>
        new CouldNotConfigureCallBaseException(CannotConfigureSingleCallMessage);

    internal static CouldNotConfigureCallBaseException ForAllCalls() =>
        new CouldNotConfigureCallBaseException(CannotConfigureAllCallsMessage);
}
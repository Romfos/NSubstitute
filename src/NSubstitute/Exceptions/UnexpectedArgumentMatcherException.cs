namespace NSubstitute.Exceptions;

public sealed class UnexpectedArgumentMatcherException(string message) : SubstituteException(message)
{
    public static readonly string WhatProbablyWentWrong =
        "Argument matchers (Arg.Is, Arg.Any) should only be used in place of member arguments. " +
        "Do not use in a Returns() statement or anywhere else outside of a member call." + Environment.NewLine +
        "Correct use:" + Environment.NewLine +
        "  sub.MyMethod(Arg.Any<string>()).Returns(\"hi\")" + Environment.NewLine +
        "Incorrect use:" + Environment.NewLine +
        "  sub.MyMethod(\"hi\").Returns(Arg.Any<string>())";
    public UnexpectedArgumentMatcherException() : this(WhatProbablyWentWrong) { }
}

﻿namespace NSubstitute.Core.Arguments;

internal sealed class ArgumentSpecificationCompatibilityTester(IDefaultChecker defaultChecker) : IArgumentSpecificationCompatibilityTester
{
    public bool IsSpecificationCompatible(IArgumentSpecification specification, object? argumentValue, Type argumentType)
    {
        var typeArgSpecIsFor = specification.ForType;
        return AreTypesCompatible(argumentType, typeArgSpecIsFor)
               && IsProvidedArgumentTheOneWeWouldGetUsingAnArgSpecForThisType(argumentValue, typeArgSpecIsFor);
    }

    private bool IsProvidedArgumentTheOneWeWouldGetUsingAnArgSpecForThisType(object? argument, Type typeArgSpecIsFor)
    {
        return defaultChecker.IsDefault(argument, typeArgSpecIsFor);
    }

    private bool AreTypesCompatible(Type argumentType, Type typeArgSpecIsFor)
    {
        return argumentType.IsAssignableFrom(typeArgSpecIsFor) ||
            (argumentType.IsByRef && !typeArgSpecIsFor.IsByRef && argumentType.IsAssignableFrom(typeArgSpecIsFor.MakeByRefType()));
    }
}
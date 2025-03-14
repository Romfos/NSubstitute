using NSubstitute.Exceptions;
using System.Reflection;

namespace NSubstitute.Core;

internal sealed class SubstituteFactory(
    ISubstituteStateFactory substituteStateFactory,
    ICallRouterFactory callRouterFactory,
    IProxyFactory proxyFactory) : ISubstituteFactory
{

    /// <summary>
    /// Create a substitute for the given types.
    /// </summary>
    /// <param name="typesToProxy"></param>
    /// <param name="constructorArguments"></param>
    /// <returns></returns>
    public object Create(Type[] typesToProxy, object?[] constructorArguments)
    {
        return Create(typesToProxy, constructorArguments, callBaseByDefault: false, isPartial: false);
    }

    /// <summary>
    /// Create an instance of the given types, with calls configured to call the base implementation
    /// where possible. Parts of the instance can be substituted using
    /// <see cref="SubstituteExtensions.Returns{T}(T,T,T[])">Returns()</see>.
    /// </summary>
    /// <param name="typesToProxy"></param>
    /// <param name="constructorArguments"></param>
    /// <returns></returns>
    public object CreatePartial(Type[] typesToProxy, object?[] constructorArguments)
    {
        var primaryProxyType = GetPrimaryProxyType(typesToProxy);
        if (!CanCallBaseImplementation(primaryProxyType))
        {
            throw new CanNotPartiallySubForInterfaceOrDelegateException(primaryProxyType);
        }

        return Create(typesToProxy, constructorArguments, callBaseByDefault: true, isPartial: true);
    }

    private object Create(Type[] typesToProxy, object?[] constructorArguments, bool callBaseByDefault, bool isPartial)
    {
        var substituteState = substituteStateFactory.Create(this);
        substituteState.CallBaseConfiguration.CallBaseByDefault = callBaseByDefault;

        var primaryProxyType = GetPrimaryProxyType(typesToProxy);
        var canConfigureBaseCalls = callBaseByDefault || CanCallBaseImplementation(primaryProxyType);

        var callRouter = callRouterFactory.Create(substituteState, canConfigureBaseCalls);
        var additionalTypes = typesToProxy.Where(x => x != primaryProxyType).ToArray();
        var proxy = proxyFactory.GenerateProxy(callRouter, primaryProxyType, additionalTypes, isPartial, constructorArguments);
        return proxy;
    }

    private static Type GetPrimaryProxyType(Type[] typesToProxy)
    {
        return typesToProxy.FirstOrDefault(t => t.IsDelegate())
            ?? typesToProxy.FirstOrDefault(t => t.GetTypeInfo().IsClass)
            ?? typesToProxy.First();
    }

    private static bool CanCallBaseImplementation(Type primaryProxyType)
    {
        var isDelegate = primaryProxyType.IsDelegate();
        var isClass = primaryProxyType.GetTypeInfo().IsClass;

        return isClass && !isDelegate;
    }
}
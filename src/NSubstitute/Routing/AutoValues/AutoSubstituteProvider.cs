﻿using NSubstitute.Core;
using System.Reflection;

namespace NSubstitute.Routing.AutoValues;

internal sealed class AutoSubstituteProvider(ISubstituteFactory substituteFactory) : IAutoValueProvider
{
    public bool CanProvideValueFor(Type type)
    {
        return type.GetTypeInfo().IsInterface
            || type.IsDelegate()
            || IsPureVirtualClassWithParameterlessConstructor(type);
    }

    public object GetValue(Type type)
    {
        return substituteFactory.Create([type], []);
    }

    private bool IsPureVirtualClassWithParameterlessConstructor(Type type)
    {
        if (type == typeof(object)) return false;
        if (!type.GetTypeInfo().IsClass) return false;
        if (!IsPureVirtualType(type)) return false;
        if (!HasParameterlessConstructor(type)) return false;
        return true;
    }

    private bool HasParameterlessConstructor(Type type)
    {
        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        var parameterlessConstructors = constructors.Where(x => IsCallableFromProxy(x) && x.GetParameters().Length == 0);
        if (!parameterlessConstructors.Any()) return false;
        return true;
    }

    private bool IsPureVirtualType(Type type)
    {
        if (type.GetTypeInfo().IsSealed) return false;
        var methods = type.GetMethods().Where(NotMethodFromObject).Where(NotStaticMethod);
        return methods.All(IsOverridable);
    }

    private bool IsCallableFromProxy(MethodBase constructor)
    {
        return constructor.IsPublic || constructor.IsFamily || constructor.IsFamilyOrAssembly;
    }

    private bool IsOverridable(MethodInfo methodInfo)
    {
        return methodInfo.IsVirtual && !methodInfo.IsFinal;
    }

    private bool NotMethodFromObject(MethodInfo methodInfo)
    {
        return methodInfo.DeclaringType != typeof(object);
    }

    private bool NotStaticMethod(MethodInfo methodInfo)
    {
        return !methodInfo.IsStatic;
    }
}
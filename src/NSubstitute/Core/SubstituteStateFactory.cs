﻿using NSubstitute.Routing.AutoValues;

namespace NSubstitute.Core;

internal sealed class SubstituteStateFactory(ICallSpecificationFactory callSpecificationFactory,
    ICallInfoFactory callInfoFactory,
    IAutoValueProvidersFactory autoValueProvidersFactory) : ISubstituteStateFactory
{
    public ISubstituteState Create(ISubstituteFactory substituteFactory)
    {
        var autoValueProviders = autoValueProvidersFactory.CreateProviders(substituteFactory);
        return new SubstituteState(callSpecificationFactory, callInfoFactory, autoValueProviders);
    }
}
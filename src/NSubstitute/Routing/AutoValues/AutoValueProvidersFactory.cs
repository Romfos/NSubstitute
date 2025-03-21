﻿using NSubstitute.Core;
using NSubstitute.Exceptions;

namespace NSubstitute.Routing.AutoValues;

internal sealed class AutoValueProvidersFactory : IAutoValueProvidersFactory
{
    public IReadOnlyCollection<IAutoValueProvider> CreateProviders(ISubstituteFactory substituteFactory)
    {
        IAutoValueProvider[]? result = null;
        var lazyResult = new Lazy<IReadOnlyCollection<IAutoValueProvider>>(
            () => result ?? throw new SubstituteInternalException("Value was not constructed yet."),
            LazyThreadSafetyMode.PublicationOnly);

        result =
        [
            new AutoObservableProvider(lazyResult),
            new AutoQueryableProvider(),
            new AutoSubstituteProvider(substituteFactory),
            new AutoStringProvider(),
            new AutoArrayProvider(),
            new AutoTaskProvider(lazyResult)
        ];

        return result;
    }
}
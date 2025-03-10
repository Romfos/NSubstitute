﻿using System.Reflection;
using BenchmarkDotNet.Running;

namespace NSubstitute.Benchmarks;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).GetTypeInfo().Assembly)
            .Run(args);
    }
}
﻿using System;

namespace DotnetRuntimeBootstrapper.Executable.Utils.Extensions
{
    internal static class FunctionalExtensions
    {
        public static TOut Pipe<TIn, TOut>(this TIn input, Func<TIn, TOut> transform) =>
            transform(input);
    }
}
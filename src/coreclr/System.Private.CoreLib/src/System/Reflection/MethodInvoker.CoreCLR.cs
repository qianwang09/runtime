﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.Reflection
{
    internal partial class MethodInvoker
    {
        private readonly Signature _signature;
        internal InvocationFlags _invocationFlags;

        public MethodInvoker(MethodBase method, Signature signature)
        {
            _method = method;
            _signature = signature;

#if USE_NATIVE_INVOKE
            // Always use the native invoke; useful for testing.
            _strategyDetermined = true;
#elif USE_EMIT_INVOKE
            // Always use emit invoke (if IsDynamicCodeCompiled == true); useful for testing.
            _invoked = true;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe object? InterpretedInvoke(object? obj, IntPtr* arguments)
        {
            return RuntimeMethodHandle.InvokeMethod(obj, (void**)arguments, _signature, isConstructor: false);
        }
    }
}

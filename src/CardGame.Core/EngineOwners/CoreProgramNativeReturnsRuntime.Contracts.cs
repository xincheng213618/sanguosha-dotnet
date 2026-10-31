#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramNativeReturnsKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType);
}

internal interface ICoreProgramNativeReturnsNativeReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramNativeReturns? _nativeReturns { get; }
}

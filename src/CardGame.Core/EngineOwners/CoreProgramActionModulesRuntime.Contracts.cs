#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramActionModulesProgramActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramActionRouter? _programActions { get; }
}

internal interface ICoreProgramActionModulesKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType);
}

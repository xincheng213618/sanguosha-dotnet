#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDistanceRulesDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramDistanceRules? _distanceRules { get; }
}

internal interface ICoreProgramDistanceRulesKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType);
}

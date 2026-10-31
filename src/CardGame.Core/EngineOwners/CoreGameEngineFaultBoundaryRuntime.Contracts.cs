#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreFaultBoundaryCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CommandSession _commandSession { get; }
}

internal interface ICoreFaultBoundaryKernelCoreGameEngineCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ICommandSessionRuntime CommandRuntime { get; }
}

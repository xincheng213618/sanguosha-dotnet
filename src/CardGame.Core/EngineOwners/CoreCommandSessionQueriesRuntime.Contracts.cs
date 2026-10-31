#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCommandSessionQueriesCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CommandSession _commandSession { get; }
}

internal interface ICoreCommandSessionQueriesKernelCoreGameEngineCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _revision { get; }

    global::CardGame.Core.ICommandSessionRuntime CommandRuntime { get; }
}

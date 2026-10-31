#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDiscardRuntime
{
    private readonly ICoreDiscardManualDiscardOperations _manualDiscardOperations;
    internal CoreGameEngineDiscardRuntime(ICoreDiscardManualDiscardOperations manualDiscardOperations)
    {
        this._manualDiscardOperations = manualDiscardOperations;
    }
}

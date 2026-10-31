#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFulueQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPeiYuanShaoOlEntries _characterPeiYuanShaoOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreGameEngineFulueQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterPeiYuanShaoOlEntries characterPeiYuanShaoOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._characterPeiYuanShaoOlOperations = characterPeiYuanShaoOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries _characterCaiYongLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries _coreSlashUseOrdinalsOperations;
    internal CoreGameEngineFeibaiEffectiveUseKindsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries characterCaiYongLeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries coreSlashUseOrdinalsOperations)
    {
        this._characterCaiYongLeOperations = characterCaiYongLeOperations;
        this._coreSlashUseOrdinalsOperations = coreSlashUseOrdinalsOperations;
    }
}

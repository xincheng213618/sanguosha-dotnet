#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsRuntime
{
    private readonly ICoreFeibaiEffectiveUseKindsCharacterCaiYongLeOperations _characterCaiYongLeOperations;
    private readonly ICoreFeibaiEffectiveUseKindsCoreSlashUseOrdinalsOperations _coreSlashUseOrdinalsOperations;
    internal CoreGameEngineFeibaiEffectiveUseKindsRuntime(ICoreFeibaiEffectiveUseKindsCharacterCaiYongLeOperations characterCaiYongLeOperations, ICoreFeibaiEffectiveUseKindsCoreSlashUseOrdinalsOperations coreSlashUseOrdinalsOperations)
    {
        this._characterCaiYongLeOperations = characterCaiYongLeOperations;
        this._coreSlashUseOrdinalsOperations = coreSlashUseOrdinalsOperations;
    }
}

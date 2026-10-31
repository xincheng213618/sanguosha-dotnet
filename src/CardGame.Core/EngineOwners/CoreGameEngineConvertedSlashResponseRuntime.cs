#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineConvertedSlashResponseRuntime
{
    private readonly ICoreConvertedSlashResponseCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreConvertedSlashResponseCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineConvertedSlashResponseRuntime(ICoreConvertedSlashResponseCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreConvertedSlashResponseCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicFactionCountsRuntime
{
    private readonly ICorePublicFactionCountsCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly ICorePublicFactionCountsCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePublicFactionCountsRuntime(ICorePublicFactionCountsCoreModeAllegianceOperations coreModeAllegianceOperations, ICorePublicFactionCountsCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreModeAllegianceOperations = coreModeAllegianceOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

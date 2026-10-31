#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramIronChainTargetsRuntime
{
    private readonly ICoreProgramIronChainTargetsCharacterPangTongMouMobileOperations _characterPangTongMouMobileOperations;
    private readonly ICoreProgramIronChainTargetsCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramIronChainTargetsRuntime(ICoreProgramIronChainTargetsCharacterPangTongMouMobileOperations characterPangTongMouMobileOperations, ICoreProgramIronChainTargetsCoreRuleQueriesOperations coreRuleQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterPangTongMouMobileOperations = characterPangTongMouMobileOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

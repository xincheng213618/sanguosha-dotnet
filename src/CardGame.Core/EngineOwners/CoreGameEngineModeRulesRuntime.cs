#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeRulesRuntime
{
    private readonly ICoreModeRulesCharacterDongZhaoOlOperations _characterDongZhaoOlOperations;
    private readonly ICoreModeRulesModeRulesOperations _modeRulesOperations;
    private readonly ICoreModeRulesModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreModeRulesCoreMatchOutcomesOperations _coreMatchOutcomesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineModeRulesRuntime(ICoreModeRulesCharacterDongZhaoOlOperations characterDongZhaoOlOperations, ICoreModeRulesModeRulesOperations modeRulesOperations, ICoreModeRulesModeRuntimeOperations modeRuntimeOperations, ICoreModeRulesCoreMatchOutcomesOperations coreMatchOutcomesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDongZhaoOlOperations = characterDongZhaoOlOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreMatchOutcomesOperations = coreMatchOutcomesOperations;
        this._coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess = coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

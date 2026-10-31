#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeRulesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongZhaoOlEntries _characterDongZhaoOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchOutcomesEntries _coreMatchOutcomesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineModeRulesRuntime(global::CardGame.Core.RuntimeEntries.CharacterDongZhaoOlEntries characterDongZhaoOlOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreMatchOutcomesEntries coreMatchOutcomesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDongZhaoOlOperations = characterDongZhaoOlOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreMatchOutcomesOperations = coreMatchOutcomesOperations;
        this._coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess = coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

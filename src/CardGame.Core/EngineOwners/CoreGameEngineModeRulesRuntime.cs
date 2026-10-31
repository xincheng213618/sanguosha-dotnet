#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeRulesRuntime
{
    private readonly RuntimeEntries.CharacterDongZhaoOlEntries _characterDongZhaoOlOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly RuntimeEntries.CoreMatchOutcomesEntries _coreMatchOutcomesOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineModeRulesRuntime(RuntimeEntries.CharacterDongZhaoOlEntries characterDongZhaoOlOperations, IKernelMatchModeRulesAccess modeRulesOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations, RuntimeEntries.CoreMatchOutcomesEntries coreMatchOutcomesOperations, EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDongZhaoOlOperations = characterDongZhaoOlOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreMatchOutcomesOperations = coreMatchOutcomesOperations;
        this._coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess = coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

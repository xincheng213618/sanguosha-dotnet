#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ProgramTriggerFactQueries(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHealthQueriesEntries> getCoreGameEngineHealthQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillPromptsEntries> getCoreGameEngineSkillPromptsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, global::CardGame.Core.NativeOwners.KernelGetAttackRangeCall getAttackRange, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDiscardSuitHistoryEntries> getCoreGameEngineTurnDiscardSuitHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries> getCoreActualDiscardPhaseLedgerRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::CardGame.Core.EngineStates.CoreCurrentTurnStatisticsState currentTurnStatisticsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries> getCoreGameEngineProgramPhaseColorRestrictionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredBenefitsProgramsEntries> getCoreGameEngineDeferredBenefitsProgramsRuntime, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnTrickUsesEntries> getCoreGameEngineActualTurnTrickUsesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramActualPlaySlashLossEntries> getCoreProgramActualPlaySlashLossRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPanShuOlEntries> getCharacterPanShuOlRuntime) : IProgramTriggerFactQueries
{
    public IReadOnlyList<CharacterState> Players => matchParticipantsState.ReferenceToPlayers;

    public int? UnambiguousHp(CharacterState player) => getCoreGameEngineHealthQueriesRuntime().UnambiguousHp(player);
    public (int Deaths, int Kills) ActualTurnDeathCounts(int ownerSeat) => getCoreGameEngineForcedPhaseAlcoholRuntime().ActualTurnDeathCounts(ownerSeat);
    public SkillBindingShard GetSkillBindingShard(CharacterState owner) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(owner);
    public bool GetProgramBooleanState(int seat, string skillId, string instanceId, string stateId) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(seat, skillId, instanceId, stateId);
    public int CountCardsUsedByCurrentPlayerThisTurn(int seat) => getCoreGameEngineSkillPromptsRuntime().CountCardsUsedByCurrentPlayerThisTurn(seat);
    public int CountCardsUsedOrRespondedByPlayerThisTurn(int seat) => getCoreGameEngineSkillPromptsRuntime().CountCardsUsedOrRespondedByPlayerThisTurn(seat);
    public bool UsesClassicBattleRules => getCoreNativeDamageHandlesRuntime().UsesClassicBattleRules;

    public string? GetEffectiveFactionId(CharacterState player) => getCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(player);
    public int GetAttackRange(int seat) => getAttackRange(seat);
    public bool UsesStrategicTriggerValue(SkillProgramTriggerValueKind value) => getCoreGameEngineStrategicProgramsRuntime().UsesStrategicTriggerValue(value);
    public bool TracksTurnDiscardSuits => getCoreGameEngineTurnDiscardSuitHistoryRuntime().TracksTurnDiscardSuits;
    public int CurrentTurnDiscardSuitMask => getCoreGameEngineTurnDiscardSuitHistoryRuntime().CurrentTurnDiscardSuitMask;
    public int TurnOwnerDiscardPhaseHandDiscardCount => getCoreActualDiscardPhaseLedgerRuntime().TurnOwnerDiscardPhaseHandDiscardCount;
    public bool FullDiscardPhaseSuitsAllDistinct => getCoreProgramParticipantPhaseOperationsRuntime().FullDiscardPhaseSuitsAllDistinct;

    public IReadOnlyDictionary<string, int>? CapturePublicPileProgramCounts(CharacterState owner) => getCoreGameEnginePublicPileSourcesRuntime().CapturePublicPileProgramCounts(owner);
    public int PlayPhaseKillCount => currentTurnStatisticsState.ReferenceToPlayPhaseKillCountByCurrentPlayer;
    public int PlayPhaseDamageDealt => currentTurnStatisticsState.ReferenceToPlayPhaseDamageDealtByCurrentPlayer;

    public IReadOnlyList<string>? UnfulfilledPlayPhaseColorRestrictions(int seat) => getCoreGameEngineProgramPhaseColorRestrictionsRuntime().UnfulfilledPlayPhaseColorRestrictions(seat);
    public bool IsRuleWounded(CharacterState player) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(player);
    public int CountPlayPhaseDamageTakenByAny() => getCoreGameEngineDeferredBenefitsProgramsRuntime().CountPlayPhaseDamageTakenByAny();
    public bool UsesTreatedWounded => getCoreGameEngineTreatedWoundedRuntime().UsesTreatedWounded;
    public int RoundTurnSeatCount => actualRoundProgressState.ReferenceToRoundTurnSeats.Count;
    public bool TracksActualTurnTrickUses => getCoreGameEngineActualTurnTrickUsesRuntime().TracksActualTurnTrickUses;

    public int ActualTurnTrickUseCount(int seat) => getCoreGameEngineActualTurnTrickUsesRuntime().ActualTurnTrickUseCount(seat);
    public bool IsFengjiRoundChoicePending(int seat) => getCharacterChenDengOlRuntime().IsFengjiRoundChoicePending(seat);
    public bool IsFengjiSwapAvailable(int seat) => getCharacterChenDengOlRuntime().IsFengjiSwapAvailable(seat);
    public bool TracksCurrentTurnUseKinds => getCoreGameEngineCurrentTurnUseKindsRuntime().TracksCurrentTurnUseKinds;

    public int CurrentTurnUsedCategoryCount(int seat) => getCoreGameEngineCurrentTurnUseKindsRuntime().CurrentTurnUseKinds(seat).Categories;
    public bool TracksActualPlaySlashLoss => getCoreProgramActualPlaySlashLossRuntime().TracksActualPlaySlashLoss;

    public int CurrentActualPlaySlashLossCount(int seat) => getCoreProgramActualPlaySlashLossRuntime().CurrentActualPlaySlashLossCount(seat);
    public bool CurrentTurnOwnerUsedSameSuitCards() => getCoreGameEngineCurrentTurnUseKindsRuntime().CurrentTurnOwnerUsedSameSuitCards();
    public int CountNonConvertedActualUsesThisTurn(int seat) => getCharacterPanShuOlRuntime().CountNonConvertedActualUsesThisTurn(seat);
}

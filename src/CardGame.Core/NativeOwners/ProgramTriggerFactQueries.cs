#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ProgramTriggerFactQueries(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineHealthQueriesEntries> getCoreGameEngineHealthQueriesRuntime, Func<RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CoreGameEngineSkillPromptsEntries> getCoreGameEngineSkillPromptsRuntime, Func<RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, Func<RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, KernelGetAttackRangeCall getAttackRange, Func<RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, Func<RuntimeEntries.CoreGameEngineTurnDiscardSuitHistoryEntries> getCoreGameEngineTurnDiscardSuitHistoryRuntime, Func<RuntimeEntries.CoreActualDiscardPhaseLedgerEntries> getCoreActualDiscardPhaseLedgerRuntime, Func<RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime, Func<RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, EngineStates.CoreCurrentTurnStatisticsState currentTurnStatisticsState, Func<RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries> getCoreGameEngineProgramPhaseColorRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, Func<RuntimeEntries.CoreGameEngineDeferredBenefitsProgramsEntries> getCoreGameEngineDeferredBenefitsProgramsRuntime, EngineStates.CoreActualRoundProgressState actualRoundProgressState, Func<RuntimeEntries.CoreGameEngineActualTurnTrickUsesEntries> getCoreGameEngineActualTurnTrickUsesRuntime, Func<RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime, Func<RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, Func<RuntimeEntries.CoreProgramActualPlaySlashLossEntries> getCoreProgramActualPlaySlashLossRuntime, Func<RuntimeEntries.CharacterPanShuOlEntries> getCharacterPanShuOlRuntime) : IProgramTriggerFactQueries
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

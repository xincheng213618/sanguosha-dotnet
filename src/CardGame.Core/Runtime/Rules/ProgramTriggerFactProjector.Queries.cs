namespace CardGame.Core;

// Scoped, lazy reads of legacy rule facts; no snapshot builder or engine-wide host is exposed.
internal interface IProgramTriggerFactQueries
{
    IReadOnlyList<CharacterState> Players { get; }
    int? UnambiguousHp(CharacterState player);
    (int Deaths, int Kills) ActualTurnDeathCounts(int ownerSeat);
    SkillBindingShard GetSkillBindingShard(CharacterState owner);
    bool GetProgramBooleanState(int seat, string skillId, string instanceId, string stateId);
    int CountCardsUsedByCurrentPlayerThisTurn(int seat);
    int CountCardsUsedOrRespondedByPlayerThisTurn(int seat);
    bool UsesClassicBattleRules { get; }
    string? GetEffectiveFactionId(CharacterState player);
    int GetAttackRange(int seat);
    bool UsesStrategicTriggerValue(SkillProgramTriggerValueKind value);
    bool TracksTurnDiscardSuits { get; }
    int CurrentTurnDiscardSuitMask { get; }
    int TurnOwnerDiscardPhaseHandDiscardCount { get; }
    bool FullDiscardPhaseSuitsAllDistinct { get; }
    IReadOnlyDictionary<string, int>? CapturePublicPileProgramCounts(CharacterState owner);
    int PlayPhaseKillCount { get; }
    int PlayPhaseDamageDealt { get; }
    IReadOnlyList<string>? UnfulfilledPlayPhaseColorRestrictions(int seat);
    bool IsRuleWounded(CharacterState player);
    int CountPlayPhaseDamageTakenByAny();
    bool UsesTreatedWounded { get; }
    int RoundTurnSeatCount { get; }
    bool TracksActualTurnTrickUses { get; }
    int ActualTurnTrickUseCount(int seat);
    bool IsFengjiRoundChoicePending(int seat);
    bool IsFengjiSwapAvailable(int seat);
    bool TracksCurrentTurnUseKinds { get; }
    int CurrentTurnUsedCategoryCount(int seat);
    bool TracksActualPlaySlashLoss { get; }
    int CurrentActualPlaySlashLossCount(int seat);
    bool CurrentTurnOwnerUsedSameSuitCards();
    int CountNonConvertedActualUsesThisTurn(int seat);
}

internal interface IProgramTriggerActionFactQueries
{
    bool TracksActualTurnUseOrdinal { get; }
    int? CapturedActualTurnActorUseOrdinal(CardActionContext action);
    bool TracksPlayCardHistory { get; }
    bool MatchesPreviousPlayCard(CardActionContext action);
    bool PreviousPlayCardIsBasic(CardActionContext action);
}

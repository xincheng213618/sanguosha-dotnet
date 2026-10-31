namespace CardGame.Core;

internal sealed record YangFuAttackCompletion(long ResolutionId, long? ProgramFrameId);

internal sealed record YangFuPlayer(int Seat, string Name, bool IsAlive, bool IsHuman, bool IsChained);

internal interface IYangFuFacts : IProgramSelectionQuery
{
    YangFuPlayer Player(int seat);
    IReadOnlyList<YangFuPlayer> Players { get; }
    bool ValidSeat(int seat);
    int HandCount(int seat);
    int TurnNumber { get; }
    int CurrentSeat { get; }
    TurnPhase Phase { get; }
    Winner Winner { get; }
    long MovementSequence { get; }
    IReadOnlyList<CardMovementRecord> Movements { get; }
    IEnumerable<T> History<T>() where T : IGameEvent;
    CardLocation Location(int cardId);
    CardUseFrame? CardUse(long useId);
    ContentSkillDefinition Skill(string skillId);
    SkillProgramEffect? PausedEffect(ProgramSkillFrame frame);
    bool HasTriggerOperation(SkillProgramEffectOp operation);
    bool ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right);
    bool Allies(int owner, int target);
    bool HasUseDebit(long actionId);
    bool IsEquipment(CardKind kind);
}

// These queries expose existing target policies and material claims. The
// character runtime retains the ordering and composition of their decisions.
internal interface IYangFuTargetFacts : IProgramSelectionQuery
{
    bool TryGetDesignatedExtraTargetUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window);
    bool DesignatedExtraTargetCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramCardTriggerWindowFrame window);
    int? CapturedWholeActualTurnActorUseOrdinal(CardActionContext action);
    bool IsKenshangRemovedMaterial(long useId, int cardId);
    bool IsWuJingHejiRemovedMaterial(long useId, int cardId);
    bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId);
    bool IsZhangYanSujiRemovedMaterial(long useId, int cardId);
    bool IsCurrentUsePhysicalCardClaim(long useId, int cardId);
    bool IsExchangedCardClaim(long actionId, int cardId);
    bool CanUseVirtualSlash(int owner, int target);
    bool DirectedTargetProhibited(int owner, int target, CardKind kind);
    bool TargetProhibited(int target, CardKind kind, Suit? suit, bool? isRed);
    bool BeneficiarySuitShield(int owner, int target, Suit? suit);
    bool SelfTargetForbidden(int owner, CardKind kind, IReadOnlyList<int> targets);
    bool CanBeDistanceFreeDesignatedTarget(CardUseFrame use, int target);
    bool CanUsePhysicalSlash(int owner, int target, Card card, CardConversionSource? source, CardKind kind, long useId, int? rank, IReadOnlyList<int> physicalIds);
    int CombatDistance(int owner, int target);
    bool CardDistanceExemption(int owner, int target, CardKind kind, long useId);
    bool CardPolicy(int owner, SkillProgramCardPolicyKind policy, CardKind kind);
    bool HasIssuedFuRongDistance(long useId, int actor);
    bool HasIssuedProvenanceUseDistance(long useId, int actor);
    bool HasIssuedGrantedPhaseEntityDistance(long useId, int actor);
    bool HasPhaseSuitAllowance(int owner, Suit? suit);
    bool FirstRedAvailable(int owner, Suit? suit);
    bool BorrowedSwordTarget(int owner, int target);
}

internal interface IYangFuAdjacencyFacts : IProgramSelectionQuery
{
    bool IgnoresSlashLimit(int owner, CardConversionSource source);
    bool HasIssuedJudgedRankSlashQuota(long useId, int actor);
    bool HasIssuedWuLuKangDixianUse(long useId, int actor);
    bool HasIssuedRoundGainedBasicBonus(long useId, int actor);
    void AssertCurrentSlashFirePolicy(CardUseFrame use);
    bool HasPijingTargetTail(CardUseFrame use);
    bool HasWeifuTargetTail(CardUseFrame use);
    bool HasDesignatedExtraTargetTail(CardUseFrame use);
    bool HasRecipientCategorySlashTargetTail(CardUseFrame use);
    bool HasZhangYanLangdaoTargetTail(CardUseFrame use);
    bool MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate);
    DyingFrame? ActiveDying { get; }
    bool MovementObserverEdge(long childId);
    bool DyingExitEdge(long childId);
    bool RandomEquipmentRide(ResolutionFrame child, ResolutionFrame parent);
    bool RecoveryReplacementRide(ResolutionFrame child, ResolutionFrame parent);
    bool SuspendedOrPaidRescueSuffix(DyingFrame dying);
}

internal interface IYangFuOperations : IProgramRuntimeOperations
{
    bool BeginCharacterStateChildren(long frameId);
    bool BeginHpChildren(long frameId);
    bool BeginMovementChildren(long frameId);
    bool BeginSkillChildren(long frameId);
    SkillProgramStepOutcome UseVirtualSlash(ProgramSkillFrame frame, int target);
    SkillProgramStepOutcome UseDeckEquipment(ProgramSkillFrame frame, int target, string bind);
    void Finish(ProgramSkillFrame frame, bool completed);
    int DrawCards(int recipient, int amount, CardMoveReason reason);
    void PublishState();
    void UpdateRoleUse(CardUseFrame use, CardActionContext action);
    void SetNativeOrigin(long useId, YangFuNativeOrigin origin);
    void RecordSlashDebit(long useId, int owner);
    bool AdvanceObserverSubtree();
}

namespace CardGame.Core;

// The phase-end owner reads this ledger without constructing the operation
// runtime, which itself uses the phase-end owner's exact-candidate proof.
internal interface IQuHuangPhaseEndQueries : IProgramSelectionQuery
{
    bool HasCapability { get; }
    IReadOnlyList<ActualPhaseEndItem> CollectNishouPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase);
}

internal interface IQuHuangFacts : IProgramSelectionQuery
{
    int TurnNumber { get; }
    int RoundNumber { get; }
    int TurnOwnerSeat { get; }
    Winner Winner { get; }
    int MovementSequence { get; }
    long ResolutionSequence { get; }
    long CardActionSequence { get; }
    Card Card(int cardId);
    SkillProgramTrigger Trigger(ProgramSkillFrame frame);
    bool HasRuntimeSkill(int seat, string skillId);
    SkillProgramTriggerFacts MovementFacts(int ownerSeat, CardMovementSourceCount count, CardMovementTiming? timing);
    bool IsActiveSourceEquipment(int ownerSeat, string skillId, string instance, Card card);
    bool IsEquipment(CardKind kind);
    EquipmentSlot EquipmentSlot(CardKind kind);
}

// Native action/observer owners expose their existing facts, never Qu Huang's
// eligibility, payment, first-child, ledger or return algorithms.
internal interface IQuHuangNativeQueries : IProgramSelectionQuery
{
    bool HasJudgmentCard(int seat, CardKind kind);
    bool HasCardPolicy(int seat, SkillProgramCardPolicyKind policy);
    bool IsCardUseForbidden(int seat, CardKind kind, CardActionType action);
    bool IsDirectedTargetProhibited(int actor, int target, CardKind kind);
    bool IsSelfTargetForbidden(int seat, CardKind kind, IReadOnlyList<int> targets);
    bool IsCardTargetProhibited(int seat, CardKind kind, Suit suit, bool? red);
    bool HasBeneficiarySuitShield(int actor, int target, Suit suit);
    bool IsPhysicalUseForbidden(int actor, IReadOnlyList<int> cards);
    bool IsSuitUseForbidden(int actor, Suit suit);
    CardUseFrame? CardUse(long id);
    bool UseActorMatches(CardUseFrame use, int actor, int provider);
    int? NullificationTarget(NullificationWindowFrame window);
    bool IsClaimedUseCard(long useId, int cardId);
    bool IsAlternativeCost(CardActionContext action, int cardId);
    bool PaymentBatchMatchesReceipt(CardUseFrame use, CardMovementBatchContext batch);
    bool RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool MovementReturnMatches(ProgramSkillFrame parent, CardsMovedTriggerWindowFrame child);
    bool IsMovementObserverEdge(long childId);
    bool IsSuspendedOrPaidRescueSuffix(long childId, DyingFrame dying);
    DyingFrame? ActiveDying { get; }
}

internal interface IQuHuangNativeOperations : IProgramRuntimeOperations
{
    void PublishState();
    long BeginCardUse(Card card, int actor, IReadOnlyList<int> targets, CardKind kind,
        CardConversionSource source, QuHuangNishouUseOrigin origin);
    void BeginTrickWindow(long useId, Card card, int actor, IReadOnlyList<int> targets,
        LegalActionKind kind, CardKind playedKind);
    void SkipUnavailableTarget(NullificationWindowFrame window, int target, CardEffectSkipReason reason);
}

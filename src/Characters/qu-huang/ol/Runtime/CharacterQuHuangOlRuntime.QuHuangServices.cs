#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQuHuangOlRuntime
{
    internal bool HasQuHuangCapability => _quHuangPhaseEndLedgerOperations.Get().HasCapability;

    internal bool TrySkipUnavailableQuHuangLightning(NullificationWindowFrame window) => _quHuangOperations.Get().TrySkipUnavailableQuHuangLightning(window);
    int IQuHuangFacts.TurnNumber => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int IQuHuangFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int IQuHuangFacts.TurnOwnerSeat => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    Winner IQuHuangFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IQuHuangFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    long IQuHuangFacts.ResolutionSequence => _coreResolutionIdentityBoundaryOperations._resolutionSequence;

    long IQuHuangFacts.CardActionSequence => _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence;

    Card IQuHuangFacts.Card(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
    SkillProgramTrigger IQuHuangFacts.Trigger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame);
    bool IQuHuangFacts.HasRuntimeSkill(int seat, string skill) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skill);
    SkillProgramTriggerFacts IQuHuangFacts.MovementFacts(int owner, CardMovementSourceCount count, CardMovementTiming? timing) => _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(_coreMatchParticipantsPlayersAccess._players[owner], 1, count, SkillProgramTriggerWindow.CardsMoved, timing);
    bool IQuHuangFacts.IsActiveSourceEquipment(int owner, string skill, string instance, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skill, instance, card);
    bool IQuHuangFacts.IsEquipment(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind);
    EquipmentSlot IQuHuangFacts.EquipmentSlot(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.Get(kind).Slot;
    bool IQuHuangNativeQueries.HasJudgmentCard(int seat, CardKind kind) => _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], kind);
    bool IQuHuangNativeQueries.HasCardPolicy(int seat, SkillProgramCardPolicyKind policy) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], policy);
    bool IQuHuangNativeQueries.IsCardUseForbidden(int seat, CardKind kind, CardActionType action) => _coreCardUseModulesOperations.IsCardUseForbidden(seat, kind, action);
    bool IQuHuangNativeQueries.IsDirectedTargetProhibited(int actor, int target, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, kind);
    bool IQuHuangNativeQueries.IsSelfTargetForbidden(int seat, CardKind kind, IReadOnlyList<int> targets) => _coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[seat], kind, targets);
    bool IQuHuangNativeQueries.IsCardTargetProhibited(int seat, CardKind kind, Suit suit, bool? red) => _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], kind, suit, red);
    bool IQuHuangNativeQueries.HasBeneficiarySuitShield(int actor, int target, Suit suit) => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor, target, suit);
    bool IQuHuangNativeQueries.IsPhysicalUseForbidden(int actor, IReadOnlyList<int> cards) => _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor, cards);
    bool IQuHuangNativeQueries.IsSuitUseForbidden(int actor, Suit suit) => _coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actor, suit);
    CardUseFrame? IQuHuangNativeQueries.CardUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id);
    bool IQuHuangNativeQueries.UseActorMatches(CardUseFrame use, int actor, int provider) => _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, actor, provider);
    int? IQuHuangNativeQueries.NullificationTarget(NullificationWindowFrame window) => CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(window);
    bool IQuHuangNativeQueries.IsClaimedUseCard(long use, int card) => _coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use, card);
    bool IQuHuangNativeQueries.IsAlternativeCost(CardActionContext action, int card) => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, card);
    bool IQuHuangNativeQueries.PaymentBatchMatchesReceipt(CardUseFrame use, CardMovementBatchContext batch) => CoreGameEngineCardUsePaymentsRuntime.CardUsePaymentBatchMatchesReceipt(use, batch);
    bool IQuHuangNativeQueries.RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool IQuHuangNativeQueries.MovementReturnMatches(ProgramSkillFrame parent, CardsMovedTriggerWindowFrame child) => CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(parent, child);
    bool IQuHuangNativeQueries.IsMovementObserverEdge(long childId) => _coreObserverSubtreesOperations.IsMovementObserverEdge(_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == childId));
    bool IQuHuangNativeQueries.IsSuspendedOrPaidRescueSuffix(long childId, DyingFrame dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == childId), dying);
    DyingFrame? IQuHuangNativeQueries.ActiveDying => _coreDyingCursorRuntimeOperations.Value.ActiveDying;

    void IQuHuangNativeOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    long IQuHuangNativeOperations.BeginCardUse(Card card, int actor, IReadOnlyList<int> targets, CardKind kind, CardConversionSource source, QuHuangNishouUseOrigin origin) => _coreNativeCardUseOperations.BeginCardUse(card, actor, targets, kind, conversionSource: source, quHuangNishou: origin);
    void IQuHuangNativeOperations.BeginTrickWindow(long id, Card card, int actor, IReadOnlyList<int> targets, LegalActionKind kind, CardKind playedKind) => _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor, targets, kind, playedCardKind: playedKind);
    void IQuHuangNativeOperations.SkipUnavailableTarget(NullificationWindowFrame window, int target, CardEffectSkipReason reason) => _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(window, _coreMatchParticipantsPlayersAccess._players[target], reason);
}

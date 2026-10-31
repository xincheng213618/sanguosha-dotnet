#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQuHuangOlRuntime
{
    internal bool HasQuHuangCapability => _quHuangPhaseEndLedgerOperations.Get().HasCapability;

    internal bool TrySkipUnavailableQuHuangLightning(NullificationWindowFrame window) => _quHuangOperations.Get().TrySkipUnavailableQuHuangLightning(window);
    int global::CardGame.Core.IQuHuangFacts.TurnNumber => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IQuHuangFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int global::CardGame.Core.IQuHuangFacts.TurnOwnerSeat => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    Winner global::CardGame.Core.IQuHuangFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IQuHuangFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    long global::CardGame.Core.IQuHuangFacts.ResolutionSequence => _coreResolutionIdentityBoundaryOperations._resolutionSequence;

    long global::CardGame.Core.IQuHuangFacts.CardActionSequence => _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence;

    Card global::CardGame.Core.IQuHuangFacts.Card(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
    SkillProgramTrigger global::CardGame.Core.IQuHuangFacts.Trigger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame);
    bool global::CardGame.Core.IQuHuangFacts.HasRuntimeSkill(int seat, string skill) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skill);
    SkillProgramTriggerFacts global::CardGame.Core.IQuHuangFacts.MovementFacts(int owner, CardMovementSourceCount count, CardMovementTiming? timing) => _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(_coreMatchParticipantsPlayersAccess._players[owner], 1, count, SkillProgramTriggerWindow.CardsMoved, timing);
    bool global::CardGame.Core.IQuHuangFacts.IsActiveSourceEquipment(int owner, string skill, string instance, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skill, instance, card);
    bool global::CardGame.Core.IQuHuangFacts.IsEquipment(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind);
    EquipmentSlot global::CardGame.Core.IQuHuangFacts.EquipmentSlot(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.Get(kind).Slot;
    bool global::CardGame.Core.IQuHuangNativeQueries.HasJudgmentCard(int seat, CardKind kind) => _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], kind);
    bool global::CardGame.Core.IQuHuangNativeQueries.HasCardPolicy(int seat, SkillProgramCardPolicyKind policy) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], policy);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsCardUseForbidden(int seat, CardKind kind, CardActionType action) => _coreCardUseModulesOperations.IsCardUseForbidden(seat, kind, action);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsDirectedTargetProhibited(int actor, int target, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, kind);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsSelfTargetForbidden(int seat, CardKind kind, IReadOnlyList<int> targets) => _coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[seat], kind, targets);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsCardTargetProhibited(int seat, CardKind kind, Suit suit, bool? red) => _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], kind, suit, red);
    bool global::CardGame.Core.IQuHuangNativeQueries.HasBeneficiarySuitShield(int actor, int target, Suit suit) => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor, target, suit);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsPhysicalUseForbidden(int actor, IReadOnlyList<int> cards) => _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor, cards);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsSuitUseForbidden(int actor, Suit suit) => _coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actor, suit);
    CardUseFrame? global::CardGame.Core.IQuHuangNativeQueries.CardUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id);
    bool global::CardGame.Core.IQuHuangNativeQueries.UseActorMatches(CardUseFrame use, int actor, int provider) => _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, actor, provider);
    int? global::CardGame.Core.IQuHuangNativeQueries.NullificationTarget(NullificationWindowFrame window) => global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(window);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsClaimedUseCard(long use, int card) => _coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use, card);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsAlternativeCost(CardActionContext action, int card) => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, card);
    bool global::CardGame.Core.IQuHuangNativeQueries.PaymentBatchMatchesReceipt(CardUseFrame use, CardMovementBatchContext batch) => global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime.CardUsePaymentBatchMatchesReceipt(use, batch);
    bool global::CardGame.Core.IQuHuangNativeQueries.RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool global::CardGame.Core.IQuHuangNativeQueries.MovementReturnMatches(ProgramSkillFrame parent, CardsMovedTriggerWindowFrame child) => global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(parent, child);
    bool global::CardGame.Core.IQuHuangNativeQueries.IsMovementObserverEdge(long childId) => _coreObserverSubtreesOperations.IsMovementObserverEdge(_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == childId));
    bool global::CardGame.Core.IQuHuangNativeQueries.IsSuspendedOrPaidRescueSuffix(long childId, DyingFrame dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == childId), dying);
    DyingFrame? global::CardGame.Core.IQuHuangNativeQueries.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    void global::CardGame.Core.IQuHuangNativeOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    long global::CardGame.Core.IQuHuangNativeOperations.BeginCardUse(Card card, int actor, IReadOnlyList<int> targets, CardKind kind, CardConversionSource source, QuHuangNishouUseOrigin origin) => _coreNativeCardUseOperations.BeginCardUse(card, actor, targets, kind, conversionSource: source, quHuangNishou: origin);
    void global::CardGame.Core.IQuHuangNativeOperations.BeginTrickWindow(long id, Card card, int actor, IReadOnlyList<int> targets, LegalActionKind kind, CardKind playedKind) => _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor, targets, kind, playedCardKind: playedKind);
    void global::CardGame.Core.IQuHuangNativeOperations.SkipUnavailableTarget(NullificationWindowFrame window, int target, CardEffectSkipReason reason) => _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(window, _coreMatchParticipantsPlayersAccess._players[target], reason);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineForeignSelectedCardSlashReturnsRuntime
{
    internal bool ValidSelectedForeignCardSlash(ProgramSkillFrame frame)
    {
        if (frame.ForeignSelectedCardSlash is not { } r || !_coreForeignSelectedCardSlashOperations.IsSelectedForeignSlashActivation(frame) || frame.InstructionIndex != 1 || r.InstructionIndex != frame.InstructionIndex || r.Source != global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime.SelectedForeignSlashSource(frame) || r.GameplayHash != frame.GameplayHash || r.ActualTurnNumber < 1 || r.PhaseInstanceId < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActorSeat) || r.ActorSeat == frame.OwnerSeat || !frame.SelectedTargetSeats.SequenceEqual([r.ActorSeat]) || frame.SelectedCardIds.Count != 0 || !Enum.IsDefined(r.Stage))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ForeignSelectedCardSlashStartedEvent>().Count(e => e == new ForeignSelectedCardSlashStartedEvent(frame.Id, r.Source, r.GameplayHash, r.ActualTurnNumber, r.PhaseInstanceId, r.ActorSeat)) != 1 || history.OfType<ForeignSelectedCardSlashFinishedEvent>().Any(e => e.ProgramFrameId == frame.Id))
            return false;
        if (r.Stage == ForeignSelectedCardSlashStage.ChoosingCard)
            return r.CardId is null && r.PrintedKind is null && r.From is null && !r.GeneralWeapon && r.PaymentBefore == 0 && r.PaymentAfter == 0 && r.PaymentBatchId is null && r.SlashReturn is null && !r.DamagedIssuer && r.RequestedDraw == 0 && r.ActualDraw == 0 && r.DrawBefore == 0 && r.DrawAfter == 0 && frame.PendingMovementContinuation is null && r.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && r.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == frame.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play;
        if (r.Stage == ForeignSelectedCardSlashStage.IssuingUse || r.CardId is not { } id || id <= 0 || r.PrintedKind is not { } kind || r.From is not { } from || from.OwnerSeat != r.ActorSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || _coreCardAttackStateOperations.GetAttackCard(id).Kind != kind || _coreCardAttackStateOperations.GetAttackCard(id).IsGeneralWeapon != r.GeneralWeapon || r.GeneralWeapon && from.Zone != CardZoneKind.Equipment || r.PaymentBefore < 0 || r.PaymentAfter <= r.PaymentBefore || r.PaymentBatchId is not { } batch || batch <= 0 || r.SlashReturn is not { } returned || returned != new ForeignSelectedCardSlashReturn(frame.Id, r.InstructionIndex, r.Source, r.GameplayHash, r.ActualTurnNumber, r.PhaseInstanceId, r.ActorSeat, frame.OwnerSeat, id, kind, from, r.GeneralWeapon, returned.CardUseFrameId, returned.ActionId) || returned.CardUseFrameId <= frame.Id || returned.ActionId <= 0 || history.OfType<ForeignSelectedCardSlashIssuedEvent>().Count(e => e.Return == returned) != 1 || history.OfType<ForeignSelectedCardSlashPaidEvent>().Count(e => e == new ForeignSelectedCardSlashPaidEvent(frame.Id, returned.CardUseFrameId, id, kind, from, r.GeneralWeapon ? CardLocation.OutsideGame : CardLocation.Processing, r.PaymentBefore, r.PaymentAfter, batch)) != 1 || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter && m.CardId == id && m.CardKind == kind && m.From == from && m.To == (r.GeneralWeapon ? CardLocation.OutsideGame : CardLocation.Processing) && m.Reason == CardMoveReasons.Use) != 1)
            return false;
        var paidMoves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter).ToArray();
        if (paidMoves.Any(m => m.CardId != id && !(kind == CardKind.WoodenOx && from == CardLocation.Equipment(r.ActorSeat) && m.From == CardLocation.WoodenOxGrain(r.ActorSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard)))
            return false;
        var damage = history.OfType<ForeignSelectedCardSlashDamageRecordedEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray();
        if (r.DamagedIssuer != (damage.Length > 0) || damage.Select(e => e.DamageFrameId).Distinct().Count() != damage.Length || damage.Any(e => e.CardUseFrameId != returned.CardUseFrameId || e.ActionId != returned.ActionId || e.TargetSeat != frame.OwnerSeat || e.Amount <= 0 || history.OfType<DamageRequestedEvent>().Count(d => d.ResolutionId == e.DamageFrameId && d.SourceSeat == e.SourceSeat && d.TargetSeat == e.TargetSeat && d.Amount == e.Amount && d.SourceLess == e.SourceLess && d.SourceCard is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) != 1))
            return false;
        if (r.Stage == ForeignSelectedCardSlashStage.UseIssued)
            return frame.PendingMovementContinuation is null && r.RequestedDraw == 0 && r.ActualDraw == 0 && r.DrawBefore == 0 && r.DrawAfter == 0;
        if (frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != frame.OwnerSeat || history.OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == returned.CardUseFrameId) != 1 || history.OfType<ForeignSelectedCardSlashResolvedEvent>().Count(e => e == new ForeignSelectedCardSlashResolvedEvent(frame.Id, returned.CardUseFrameId, returned.ActionId, r.DamagedIssuer)) != 1)
            return false;
        if (r.Stage == ForeignSelectedCardSlashStage.SettlementChildren)
            return r.RequestedDraw == 0 && r.ActualDraw == 0 && r.DrawBefore == 0 && r.DrawAfter == 0;
        return r.Stage == ForeignSelectedCardSlashStage.DrawChildren && r.RequestedDraw == (r.DamagedIssuer ? 2 : 1) && r.ActualDraw >= 0 && r.ActualDraw <= r.RequestedDraw && r.DrawBefore >= r.PaymentAfter && r.DrawAfter >= r.DrawBefore && history.OfType<ForeignSelectedCardSlashDrawIssuedEvent>().Count(e => e == new ForeignSelectedCardSlashDrawIssuedEvent(frame.Id, returned.CardUseFrameId, r.RequestedDraw, r.ActualDraw, r.DrawBefore, r.DrawAfter)) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "skill-program.foreign-selected-card-slash.outcome-draw") == r.ActualDraw;
    }

    internal bool IsSelectedForeignCardSlashUse(CardUseFrame use)
    {
        if (use.ForeignSelectedCardSlashReturn is not { } r || use.Id != r.CardUseFrameId || use.CardId != r.CardId || use.PhysicalCardIds is not [var id] || id != r.CardId || use.Action is not { Type: CardActionType.Use } action || action.ActionId != r.ActionId || action.ProviderSeat != r.ActorSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.PhysicalCards is not [var cost] || cost.CardId != r.CardId || cost.CardKind != r.PrintedKind || cost.From != r.From || !action.ConversionChain.Contains(r.Source) || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, r.ActorSeat, r.ActorSeat) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardKind != CardKind.Slash && !_coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == r.ProgramFrameId)is not { } frame || frame.ForeignSelectedCardSlash?.SlashReturn != r || !ValidSelectedForeignCardSlash(frame))
            return false;
        if (use.ProgramUseAccepted)
            return _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action);
        // Native Slash publishes its finalized acceptance only after the
        // committed-use and target-redirection children have returned. Until
        // then, the typed issuance, declaration and paid original attack own
        // this exact provisional action; do not invent an early acceptance.
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return !history.OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == r.ActionId) && history.OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == r.CardId && e.CardKind == CardKind.Slash && e.SourceSeat == r.ActorSeat) == 1 && history.OfType<TargetsConfirmedEvent>().Count(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual([r.OriginalTargetSeat])) == 1 && use.SourceSeat == r.ActorSeat && use.CardKind == CardKind.Slash && action.ActorSeat == r.ActorSeat && action.EffectiveKind == CardKind.Slash && action.TargetSeats.SequenceEqual([r.OriginalTargetSeat]) && action.DesignatedTargetSeats is { } designated && designated.SequenceEqual([r.OriginalTargetSeat]) && action.ConversionChain.SequenceEqual([r.Source]) && use.TargetSeats.SequenceEqual([r.OriginalTargetSeat]) && use.CardAttack is { } attack && attack.ProgramSkillCardUseFrameId == frame.Id && attack.SourceSeat == r.ActorSeat && attack.CardUserSeat == r.ActorSeat && attack.TargetSeat == r.OriginalTargetSeat && attack.CardId == r.CardId && attack.EffectiveCardKind == CardKind.Slash && attack.ConversionSource == r.Source && attack.PhysicalCardIds.SequenceEqual([r.CardId]) && _cardZonesOperations._cardZones.GetLocation(r.CardId) == (r.GeneralWeapon ? CardLocation.OutsideGame : CardLocation.Processing);
    }

    internal bool HasIssuedSelectedForeignCardSlashDistance(long? useId, int actor) => useId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && use.ForeignSelectedCardSlashReturn?.ActorSeat == actor && IsSelectedForeignCardSlashUse(use);
    internal bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { } use && use.ForeignSelectedCardSlashReturn is { GeneralWeapon: true } r && r.CardId == cardId && IsSelectedForeignCardSlashUse(use) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 600)]
    internal void ObserveSelectedForeignCardSlashAppliedDamage(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not global::CardGame.Core.NativeOwners.CardAttackHandle attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ForeignSelectedCardSlashReturn: { } returned } use || applied.TargetSeat != returned.OriginalTargetSeat || !IsSelectedForeignCardSlashUse(use) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != use.Id || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat || attack.IsSourceLess != applied.SourceLess || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Count(e => e.ResolutionId == damage.Id && e.SourceSeat == applied.SourceSeat && e.TargetSeat == applied.TargetSeat && e.Amount == applied.Amount && e.SourceLess == applied.SourceLess) != 1)
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ForeignSelectedCardSlashDamageRecordedEvent>().Any(e => e.DamageFrameId == damage.Id))
            throw new InvalidOperationException("One selected foreign-card Slash damage cannot be recorded twice.");
        var frame = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == returned.ProgramFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { ForeignSelectedCardSlash = frame.ForeignSelectedCardSlash!with { DamagedIssuer = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ForeignSelectedCardSlashDamageRecordedEvent(frame.Id, use.Id, returned.ActionId, damage.Id, applied.SourceSeat, applied.TargetSeat, applied.Amount, applied.SourceLess));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 480)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 520)]
    internal bool CanContinueSelectedForeignCardSlash(ProgramSkillFrame frame) => frame.ForeignSelectedCardSlash is { Stage: not ForeignSelectedCardSlashStage.ChoosingCard and not ForeignSelectedCardSlashStage.IssuingUse } && ValidSelectedForeignCardSlash(frame);
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 40)]
    internal bool IsSelectedForeignCardSlashMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.UseSelectedForeignCardAsSlash && frame.PendingMovementContinuation == pending && frame.ForeignSelectedCardSlash is { Stage: ForeignSelectedCardSlashStage.SettlementChildren or ForeignSelectedCardSlashStage.DrawChildren } && ValidSelectedForeignCardSlash(frame);
    internal bool IsSelectedForeignCardSlashMovementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch)
    {
        if (frame.ForeignSelectedCardSlash is not { Stage: ForeignSelectedCardSlashStage.SettlementChildren, SlashReturn: { } r } receipt || !ValidSelectedForeignCardSlash(frame) || batch.ParentFrameId != r.CardUseFrameId || batch.AwaitingProgramFrameId is not null || batch.OriginOwnerSeat != frame.OwnerSeat || batch.OriginSkillId != frame.SkillId || batch.OriginSkillInstanceId != frame.SkillInstanceId || batch.Movements.Count == 0)
            return false;
        return batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.CardId == r.CardId && m.CardKind == r.PrintedKind && (batch.Id == receipt.PaymentBatchId && m.Sequence > receipt.PaymentBefore && m.Sequence <= receipt.PaymentAfter && m.From == r.From && m.To == (r.GeneralWeapon ? CardLocation.OutsideGame : CardLocation.Processing) && m.Reason == CardMoveReasons.Use || m.Sequence > receipt.PaymentAfter && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished) || r.PrintedKind == CardKind.WoodenOx && r.From == CardLocation.Equipment(r.ActorSeat) && batch.ParentBatchId == receipt.PaymentBatchId && m.Sequence > receipt.PaymentBefore && m.Sequence <= receipt.PaymentAfter && m.From == CardLocation.WoodenOxGrain(r.ActorSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
    }

    internal bool IsSelectedForeignCardSlashHpChange(long resumeFrameId, HpChangeContext change) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == resumeFrameId)is { ForeignSelectedCardSlash: { Stage: ForeignSelectedCardSlashStage.SettlementChildren, SlashReturn: { } r } } frame && ValidSelectedForeignCardSlash(frame) && r.PrintedKind == CardKind.SilverLion && r.From == CardLocation.Equipment(r.ActorSeat) && change.ParentFrameId == r.CardUseFrameId && change.Kind == HpChangeKind.Recovery && change.SourceSeat == r.ActorSeat && change.TargetSeat == r.ActorSeat && change.Amount == 1;
    internal bool SelectedForeignCardSlashFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidSelectedForeignCardSlash(frame) || frame.ForeignSelectedCardSlash is not { } receipt)
            return false;
        if (receipt.Stage == ForeignSelectedCardSlashStage.UseIssued)
            return child is CardUseFrame use && use.ForeignSelectedCardSlashReturn == receipt.SlashReturn && IsSelectedForeignCardSlashUse(use);
        if (receipt.Stage is not (ForeignSelectedCardSlashStage.SettlementChildren or ForeignSelectedCardSlashStage.DrawChildren))
            return false;
        if (child is CardsMovedTriggerWindowFrame movement)
            return movement.Batch.Id == movement.Id && (receipt.Stage == ForeignSelectedCardSlashStage.SettlementChildren && movement.ResumeProgramFrameId == frame.Id && IsSelectedForeignCardSlashMovementBatch(frame, movement.Batch) || receipt.Stage == ForeignSelectedCardSlashStage.DrawChildren && movement.ResumeProgramFrameId is null && movement.Batch.ParentFrameId == frame.Id && movement.Batch.AwaitingProgramFrameId == frame.Id && movement.Batch.OriginOwnerSeat == frame.OwnerSeat && movement.Batch.OriginSkillId == frame.SkillId && movement.Batch.OriginSkillInstanceId == frame.SkillInstanceId && movement.Batch.Movements.Count > 0 && movement.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > receipt.DrawBefore && m.Sequence <= receipt.DrawAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "skill-program.foreign-selected-card-slash.outcome-draw"));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && IsSelectedForeignCardSlashHpChange(frame.Id, hp.Change);
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == frame.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == frame.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal bool ForeignSelectedCardSlashStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is ProgramSkillFrame frame && SelectedForeignCardSlashFirstChild(frame, child);
    internal ProgramSkillFrame? SelectedForeignCardSlashObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => root.ForeignSelectedCardSlash is { Stage: ForeignSelectedCardSlashStage.SettlementChildren or ForeignSelectedCardSlashStage.DrawChildren } && _coreForeignSelectedCardSlashReturnsOperations.SelectedForeignCardSlashFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 140)]
    internal bool IsSelectedForeignCardSlashProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SelectedForeignCardSlashObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1650)]
    internal bool HasSelectedForeignCardSlashDamageObserver(long id) => HasSelectedForeignCardSlashObserver(id, false);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 10)]
    internal bool HasSelectedForeignCardSlashBeforeDamageObserver(long id) => HasSelectedForeignCardSlashObserver(id, true);
    internal bool HasSelectedForeignCardSlashObserver(long id, bool before)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && (before ? f is BeforeDamageProgramWindowFrame : f is DamageTriggerWindowFrame));
        if (index < 0 || SelectedForeignCardSlashObserverRoot()is not { } root)
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        if (index > rootIndex)
            return true;
        for (var child = index + 1; child <= rootIndex; child++)
            if (!ForeignSelectedCardSlashStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreResponseCompletionOperations.ResponseCompletionStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]))
                return false;
        return index < rootIndex;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 10)]
    internal bool TryAdvanceSelectedForeignCardSlashSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || SelectedForeignCardSlashObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 120)]
    internal bool DistancePhysicalSlash120HasIssuedSelectedForeignCardSlashDistance(ProgramSlashDistanceQuery q) => HasIssuedSelectedForeignCardSlashDistance(q.ExistingUseFrameId, q.Actor.Seat);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 100)]
    internal bool WindowSettlement_IsSelectedForeignCardSlashMovementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsSelectedForeignCardSlashMovementBatch(frame, batch);
}

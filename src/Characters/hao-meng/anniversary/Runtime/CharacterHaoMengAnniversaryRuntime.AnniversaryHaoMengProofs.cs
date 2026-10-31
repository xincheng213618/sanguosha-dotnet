#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHaoMengAnniversaryRuntime
{
    internal bool HaoMengOriginal(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.SkillId != AnniversaryHaoMengComposition.Skill || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            return false;
        if (op == SkillProgramEffectOp.ResolveAnniversaryHaoMengXiongmangUse)
            return f.ActivationId == AnniversaryHaoMengComposition.Activation && f.TriggerId is null && f.WindowContext is null && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
        if (op != SkillProgramEffectOp.ResolveAnniversaryHaoMengXiongmangConsequence || f.TriggerId != AnniversaryHaoMengComposition.Consequence || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || HaoMengConsequenceUse(c)is not { AnniversaryHaoMengUse: { } o } use || o.Source.OwnerSeat != f.OwnerSeat || o.Source.SkillInstanceId != f.SkillInstanceId || o.Hash != f.GameplayHash || !_coreOriginalHandEntityObserversOperations.OriginalHandBindingStarted(f, SkillProgramTriggerWindow.CardUseCompleted))
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return at > 0 && _resolutionStackOperations._resolutionStack[at - 1] is ProgramCardTriggerWindowFrame parent && parent.Id == c.ParentFrameId && parent.ParentFrameId == use.Id && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) && c == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, parent.Candidates[parent.CandidateIndex]);
    }

    internal bool ValidHaoMeng(ProgramSkillFrame f)
    {
        if (f.AnniversaryHaoMeng is not { } r || f.InstructionIndex != 1 || !HaoMengOriginal(f, r.Operation) || r.Source != HaoMengSource(f) || r.Hash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Count > 4 || r.Cards.Distinct().Count() != r.Cards.Count || r.Targets.Distinct().Count() != r.Targets.Count || r.NativeBatches.Distinct().Count() != r.NativeBatches.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHaoMengStartedEvent>().Count(e => e == new AnniversaryHaoMengStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.ParentId, r.Turn)) != 1)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryHaoMengXiongmangConsequence)
            return r.Stage is AnniversaryHaoMengStage.HealthChildren or AnniversaryHaoMengStage.Completed && r.UseId == f.WindowContext!.CardUse!.ParentCardUseFrameId && r.ActionId == f.WindowContext.CardUse.CardActionId && r.Cards.Count == 0 && r.Targets.Count == 0 && r.NativeBatches.Count == 0 && !r.Returned;
        if (r.Stage == AnniversaryHaoMengStage.HealthChildren || r.Targets.Count > r.Cards.Count || r.Cards.Any(id => id <= 0) || r.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t) || t == f.OwnerSeat))
            return false;
        var cards = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHaoMengCardChosenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        var targets = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHaoMengTargetChosenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (cards.Length != r.Cards.Count || !cards.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, cards.Length)) || !cards.Select(e => e.CardId).SequenceEqual(r.Cards) || cards.Any(e => e.Suit == Suit.None || !Enum.IsDefined(e.Suit)) || cards.Select(e => e.Suit).Distinct().Count() != cards.Length || targets.Length != r.Targets.Count || !targets.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, targets.Length)) || !targets.Select(e => e.Target).SequenceEqual(r.Targets))
            return false;
        if (r.UseId == 0)
        {
            if (cards.Any(e => e.Suit != HaoMengHandSuit(f.OwnerSeat, e.CardId)))
                return false;
        }
        else
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHaoMengMaterialEvent>().Where(e => e.UseId == r.UseId).OrderBy(e => e.Index).ToArray();
            if (paid.Length != cards.Length || !paid.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, paid.Length)) || !paid.Select(e => e.Material.CardId).SequenceEqual(r.Cards) || !paid.Select(e => e.EffectiveSuit).SequenceEqual(cards.Select(e => e.Suit)))
                return false;
        }

        if (r.Stage is AnniversaryHaoMengStage.Cards or AnniversaryHaoMengStage.Targets or AnniversaryHaoMengStage.Issuing && (r.Cards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)) || r.UseId != 0 || r.ActionId != 0 || r.Returned || r.NativeBatches.Count != 0))
            return false;
        if (r.Stage is AnniversaryHaoMengStage.NativeSlash or AnniversaryHaoMengStage.NativeTail && (r.Cards.Count == 0 || r.Targets.Count == 0 || r.UseId <= f.Id || r.ActionId <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHaoMengUseIssuedEvent>().Where(e => e.Origin.UseId == r.UseId).ToArray()is not [var issue] || issue.Origin.ProducerFrameId != f.Id || issue.Origin.ActionId != r.ActionId || issue.Origin.Source != HaoMengNativeSource(f, r.Cards.Count) || issue.Origin.Hash != r.Hash))
            return false;
        return !r.Returned || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHaoMengUseReturnedEvent>().Count(e => e == new AnniversaryHaoMengUseReturnedEvent(f.Id, r.UseId, r.ActionId)) == 1;
    }
    internal bool HaoMengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryHaoMeng is { Stage: AnniversaryHaoMengStage.NativeTail or AnniversaryHaoMengStage.HealthChildren } r && e?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidHaoMeng(f);
    internal bool HaoMengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidHaoMeng(f) || f.AnniversaryHaoMeng is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryHaoMengStage.NativeSlash && use.AnniversaryHaoMengUse is { } o && o.ProducerFrameId == f.Id && o.UseId == r.UseId && o.ActionId == r.ActionId && o.Source == HaoMengNativeSource(f, r.Cards.Count) && IsHaoMengNativeUse(use);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && HaoMengMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && (global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || IsAnniversaryHaoMengSettlementBatch(f, moved.Batch)) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool HaoMengNativeEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        if (parent is CardUseFrame { AnniversaryHaoMengUse: not null } use && IsHaoMengNativeUse(use))
        {
            if (current is ResponseWindowFrame { Step: ResolutionFrameStep.AwaitingResponse, RequiredCardKind: CardKind.Dodge } response && use is { Step: ResolutionFrameStep.AwaitingResponse, CardAttack: { Active: true } attack })
                return response.ParentFrameId == use.Id && response.SourceSeat == attack.SourceSeat && response.ResponderSeat == attack.TargetSeat && response.IncomingCard == (attack.EffectiveCardKind ?? use.CardKind) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(response.IncomingCard);
            if (current is ProgramCardTriggerWindowFrame window)
                return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        }

        if (parent is ProgramCardTriggerWindowFrame card && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentFrameId)is { AnniversaryHaoMengUse: not null } native && IsHaoMengNativeUse(native) && current is ProgramSkillFrame f && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(card.Candidates[card.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(card, card.Candidates[card.CandidateIndex]);
        return false;
    }

    internal bool HaoMengSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || this.HaoMengNativeEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && this.HaoMengFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, d) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, d));
    internal ProgramSkillFrame? HaoMengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HaoMengFirstChild(f, c), (i) => this.HaoMengSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49700)]
    internal bool HaoMengDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HaoMengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49700)]
    internal bool AdvanceHaoMengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HaoMengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 49700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 49700)]
    internal bool HaoMengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryHaoMeng is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && HaoMengFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || HaoMengSuffix(parent)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }
}

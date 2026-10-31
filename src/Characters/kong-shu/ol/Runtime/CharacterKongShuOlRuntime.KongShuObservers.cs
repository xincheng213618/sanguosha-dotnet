#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongShuOlRuntime
{
    internal static bool KongShuMoving(KongShuStage stage) => global::CardGame.Core.RuntimeSkills.RuntimeRules_kong_shu_ol_KongShuObservers.KongShuMoving(stage);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27550)]
    internal void AssertKongShu(ProgramSkillFrame f)
    {
        if (f.KongShu is not { } r)
            return;
        if (!r.Paid || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || r.WindowId != context.ParentFrameId || r.Source != KongShuSource(f) || r.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !KongShuComposition.Handles(r.Op) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || !Enum.IsDefined(r.Stage) || r.Cursor < 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Payer) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected.Distinct().Count() != r.Selected.Count || r.Materials.Select(c => c.CardId).Distinct().Count() != r.Materials.Count || r.Grain.Distinct().Count() != r.Grain.Count || r.DrawRecipients is not System.Collections.IList { IsReadOnly: true } || r.DiscardRecipients is not System.Collections.IList { IsReadOnly: true } || r.Selected is not System.Collections.IList { IsReadOnly: true } || r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Grain is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.ProgramEventHistory<KongShuStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new KongShuStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner, r.Round))
            throw new InvalidOperationException("Kong Shu lost her exact owning trigger, immutable source or issued stage.");
        if (r.Op == SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit)
        {
            if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || _coreCardUseLifecycleOperations.LifecycleCardUse(r.NativeUseId)is not { LeiluanPayments: { } payments } use || payments.SingleOrDefault(p => p.Id == r.PaymentId)is not { Paid: true, LastHand: true } p || p.ActionId != r.ActionId || p.OwnerSeat != f.OwnerSeat || p.Source.SkillId != f.SkillId || p.Source.SkillInstanceId != f.SkillInstanceId || p.GameplayHash != f.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } parent || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) || !LeiluanActionMatches(p, parent.Action) || _coreProgramEventHistoryOperations.ProgramEventHistory<LeiluanBenefitIssuedEvent>().Where(e => e.PaymentId == r.PaymentId).ToArray()is not [var issue] || issue != new LeiluanBenefitIssuedEvent(f.Id, p.Id, p.UseId, p.ActionId, f.OwnerSeat) || r.Stage is not (KongShuStage.BenefitDraw or KongShuStage.BenefitDrawChildren or KongShuStage.BenefitChooseTrick or KongShuStage.BenefitGainChildren))
                throw new InvalidOperationException("Leiluan's benefit lost its unique completed cost or original payer.");
            AssertLeiluanPayments(use);
        }
        else if (r.Op == SkillProgramEffectOp.ResolveFuchaoCurrentTurnChain)
        {
            if (context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } action || action.ActorSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(action.EffectiveKind, [SkillProgramCardCategory.Basic]) || r.Stage != KongShuStage.ChainChildren)
                throw new InvalidOperationException("Fuchao chaining requires the final actual true-basic-use actor.");
        }
        else
        {
            if (context.Window != SkillProgramTriggerWindow.RoundEnding || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } parent || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || !_coreRoundEndingOperations.IsExactRoundEndingCandidate(parent.Candidates[parent.CandidateIndex], context) || r.Stage is not (KongShuStage.RoundDraw or KongShuStage.RoundDrawChildren or KongShuStage.RoundDiscard or KongShuStage.RoundDiscardChildren) || !_coreProgramEventHistoryOperations.ProgramEventHistory<KongShuDrawRecipientFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.RecipientSeat).SequenceEqual(r.DrawRecipients) || r.DrawRecipients.Count == 0 || r.DrawRecipients[0] != f.OwnerSeat || !_coreProgramEventHistoryOperations.ProgramEventHistory<KongShuDiscardRecipientFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.RecipientSeat).SequenceEqual(r.DiscardRecipients))
                throw new InvalidOperationException("Fuchao lost its real round-ending candidate or frozen draw/discard participants.");
        }

        if (!KongShuMoving(r.Stage))
        {
            if (f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Kong Shu retained an unissued movement continuation.");
            return;
        }

        if (f.PendingMovementContinuation is not { } pending || !IsKongShuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Kong Shu lost her exact paid movement return.");
        if (r.Stage is KongShuStage.BenefitDrawChildren or KongShuStage.RoundDrawChildren)
        {
            var requested = r.Stage == KongShuStage.BenefitDrawChildren ? 2 : 1;
            if (r.ActualDrawCount < 0 || r.ActualDrawCount > requested || _coreProgramEventHistoryOperations.ProgramEventHistory<KongShuDrawIssuedEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).ToArray()is not [var issue] || issue != new KongShuDrawIssuedEvent(f.Id, r.Cursor, r.Payer, requested, r.ActualDrawCount, r.Before, r.After) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == (r.Stage == KongShuStage.BenefitDrawChildren ? LeiluanDrawReason : FuchaoDrawReason)) != r.ActualDrawCount)
                throw new InvalidOperationException("Kong Shu lost the exact whole native draw invocation.");
        }
        else
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<KongShuMovementIssuedEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).ToArray()is not [var issue] || issue != new KongShuMovementIssuedEvent(f.Id, r.Cursor, r.Payer, r.Materials.Count, r.Before, r.After) || !_coreProgramEventHistoryOperations.ProgramEventHistory<KongShuMaterialFrozenEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).OrderBy(e => e.Index).Select(e => new KongShuEntity(e.CardId, e.Kind, e.From)).SequenceEqual(r.Materials) || !_coreProgramEventHistoryOperations.ProgramEventHistory<KongShuGrainFrozenEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).OrderBy(e => e.Index).Select(e => e.CardId).SequenceEqual(r.Grain) || _coreProgramEventHistoryOperations.ProgramEventHistory<KongShuMaterialMovedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor) != r.Materials.Count || r.Stage == KongShuStage.BenefitGainChildren && (r.Materials.Count != 1 || r.Materials[0].From != CardLocation.DiscardPile || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(r.Materials[0].Kind, [SkillProgramCardCategory.InstantTrick])) || r.Stage == KongShuStage.RoundDiscardChildren && (r.Materials.Count != r.DiscardCount || r.Materials.Any(c => c.From.OwnerSeat != r.Payer || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))))
                throw new InvalidOperationException("Kong Shu lost her exact complete native material invoice.");
        }

        if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && !IsKongShuPaidMovement(f, m)))
            throw new InvalidOperationException("Kong Shu's issued movement interval contains unrelated movement.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 480)]
    internal bool IsKongShuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.KongShu is { Paid: true } r && e?.Op == r.Op && KongShuMoving(r.Stage) && p == f.PendingMovementContinuation && p.SubjectSeat == r.Payer && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool IsKongShuPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.KongShu!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage is KongShuStage.BenefitDrawChildren or KongShuStage.RoundDrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == (r.Stage == KongShuStage.BenefitDrawChildren ? LeiluanDrawReason : FuchaoDrawReason) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        var discard = r.Stage == KongShuStage.RoundDiscardChildren;
        if (r.Materials.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == (discard ? _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon && m.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat)) && m.Reason.Value == (discard ? FuchaoDiscardReason : LeiluanGainReason) && _coreProgramEventHistoryOperations.ProgramEventHistory<KongShuMaterialMovedEvent>().Any(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.CardId == m.CardId && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
            return true;
        return discard && r.Grain.Contains(m.CardId) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(r.Payer) && m.From == CardLocation.WoodenOxGrain(r.Payer));
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 240)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 230)]
    internal bool IsKongShuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.KongShu is { Paid: true, Stage: KongShuStage.RoundDiscardChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == r.Payer && recovery.Attempt.TargetSeat == r.Payer && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == FuchaoDiscardReason && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(r.Payer));
    internal bool KongShuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.KongShu is not { Paid: true } r)
            return false;
        AssertKongShu(f);
        if (child is RecoveryReplacementFrame recovery)
            return IsKongShuRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == KongShuStage.RoundDiscardChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.Payer && hp.Change.TargetSeat == r.Payer && hp.Change.Amount == 1 && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(r.Payer));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window) && (r.Stage != KongShuStage.ChainChildren || state.OwnerSeat == r.TurnOwner);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsKongShuPaidMovement(f, m));
    }

    internal ProgramSkillFrame? KongShuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterKongShuOlOperations.KongShuFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1000)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1110)]
    internal bool HasKongShuDamageObserver(long id) => KongShuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1140)]
    internal bool HasKongShuBeforeDamageObserver(long id) => KongShuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 970)]
    internal bool IsKongShuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && KongShuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27550)]
    internal bool AdvanceKongShuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KongShuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

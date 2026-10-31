#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChengOlRuntime
{
    internal static bool ChenglieChildren(ChenglieStage s) => global::CardGame.Core.RuntimeSkills.RuntimeRules_ma_cheng_ol_ChenglieObservers.ChenglieChildren(s);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27560)]
    internal void AssertMaCheng(ProgramSkillFrame f)
    {
        if (f.Chenglie is not { } r)
            return;
        var use = RequireChenglieParent(f);
        var op = r.Outcome ? SkillProgramEffectOp.ResolveChenglieRedHolderOutcome : SkillProgramEffectOp.ResolveChenglieExtraTargetsAndHiddenCards;
        if (f.SkillId != "ol:chenglie" || f.TriggerId != (r.Outcome ? "red-holder-outcomes" : "extra-targets") || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || r.Source != ChenglieSource(f) || r.GameplayHash != f.GameplayHash || r.UseId != use.Id || r.ActionId != use.Action!.ActionId || !Enum.IsDefined(r.Stage) || r.Index < 0 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Count < 0 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new ChenglieStartedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.WindowId, r.SetupFrameId, r.Outcome, r.Original.Count) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.OwnerSeat == f.OwnerSeat) != 1)
            throw new InvalidOperationException("Chenglie lost its exact original source/hash, native mother, one instruction or started invoice.");
        if (r.Outcome)
        {
            if (use.ChenglieLease is not { } lease || r.SetupFrameId != lease.SetupFrameId || r.Source != (lease.Source with
            {
                BindingId = "red-holder-outcomes"
            }

            ) || !r.Original.SequenceEqual(lease.Original) || !r.Added.SequenceEqual(lease.Added) || r.Index > lease.Cards.Count || r.Stage is ChenglieStage.Targets or ChenglieStage.RevealChildren or ChenglieStage.Exchange or ChenglieStage.ExchangeChildren or ChenglieStage.Assignment or ChenglieStage.AssignmentChildren)
                throw new InvalidOperationException("Chenglie's issued resolver cannot replace the original issuer or attachment slots.");
            for (var i = 0; i < r.Index; i++)
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieOutcomeCompletedEvent>().Count(e => e.FrameId == f.Id && e.SetupFrameId == r.SetupFrameId && e.Index == i) != 1)
                    throw new InvalidOperationException("Chenglie skipped or repeated a red-holder outcome.");
        }
        else
        {
            if (r.SetupFrameId != f.Id || !_coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieTargetSlotEvent>().Where(e => e.FrameId == f.Id && !e.Added).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Original) || !_coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieTargetSlotEvent>().Where(e => e.FrameId == f.Id && e.Added).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Added) || r.Added.Count > 2 || r.Added.Distinct().Count() != r.Added.Count || r.Added.Any(s => r.Original.Contains(s) || s == f.OwnerSeat) || r.Index > r.Original.Count + r.Added.Count || r.Outcome || !r.Paid && r.Stage != ChenglieStage.Targets)
                throw new InvalidOperationException("Chenglie changed its original or additional native target queue.");
            if (r.Paid && (use.ChenglieLease is not { } lease || lease.SetupFrameId != f.Id || lease.Source != r.Source || !HasChenglieTargetTail(use)))
                throw new InvalidOperationException("Chenglie lost its paid original-use attachment lease.");
            var revealed = _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieRevealedCardEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
            if (!revealed.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, revealed.Length)) || !revealed.Select(e => e.CardId).SequenceEqual(r.Revealed) || r.Revealed.Distinct().Count() != r.Revealed.Count)
                throw new InvalidOperationException("Chenglie changed or duplicated a genuinely revealed original deck entity.");
            if (r.SwapHand is { } hand)
            {
                if (r.SwapTop is not { } top || !r.Revealed.Contains(top) || _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieSwapSelectedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var swap] || swap.HandCardId != hand || swap.TopCardId != top)
                    throw new InvalidOperationException("Chenglie cannot manufacture or change its secret exchange.");
            }
            else if (r.SwapTop is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieSwapSelectedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Chenglie retained an incomplete secret exchange.");
            var expected = r.Revealed.Select(id => r.SwapTop == id ? r.SwapHand!.Value : id).Except(use.ChenglieLease?.Cards.Select(c => c.CardId) ?? []).ToArray();
            if (!r.Pool.SequenceEqual(expected))
                throw new InvalidOperationException("Chenglie's remaining pool must contain precisely its original revealed cards after the one exact swap and assignments.");
        }

        if (ChenglieChildren(r.Stage))
        {
            if (f.PendingMovementContinuation is { } p && (p.BeforeCount != 0 || p.CoverageResultBind is not null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.SubjectSeat)))
                throw new InvalidOperationException("Chenglie lost its typed native movement participant.");
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieMovementPaidEvent>().Where(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index).ToArray();
            if (paid is not [var exact] || exact != new ChenglieMovementPaidEvent(f.Id, r.Stage, r.Index, r.Before, r.After, r.Count))
                throw new InvalidOperationException("Chenglie lost or repeated its exact completed movement invoice.");
            var moves = _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieCardMovedEvent>().Where(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index && e.Sequence > r.Before && e.Sequence <= r.After).ToArray();
            if (moves.Length != r.Count || moves.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason == e.Reason)))
                throw new InvalidOperationException("Chenglie's payment differs from the genuine native movement ledger.");
        }
        else if (f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Chenglie retained a movement continuation outside its actual children.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && r.Stage is ChenglieStage.Targets or ChenglieStage.Exchange or ChenglieStage.Assignment or ChenglieStage.Gift && (prompt.SkillPrompt?.SkillId != f.SkillId || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, ChenglieChoices(f))))
            throw new InvalidOperationException("Chenglie's secret/public menu changed its original legal choices.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 490)]
    internal bool IsMaChengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => f.Chenglie is { } r && ChenglieChildren(r.Stage) && pending == f.PendingMovementContinuation && pending.BeforeCount == 0 && pending.CoverageResultBind is null && e?.Op == (r.Outcome ? SkillProgramEffectOp.ResolveChenglieRedHolderOutcome : SkillProgramEffectOp.ResolveChenglieExtraTargetsAndHiddenCards);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 250)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 240)]
    internal bool IsMaChengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Chenglie is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement)
            return false;
        if (r.Stage == ChenglieStage.RecoveryChildren && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)?.ChenglieLease is { } lease && r.Index < lease.Cards.Count)
            return recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == lease.Cards[r.Index].HolderSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1;
        return r.Stage == ChenglieStage.GiftChildren && r.GiftId is { } id && r.GiftFrom is { Zone: CardZoneKind.Equipment, OwnerSeat: { } donor } && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && recovery.Attempt.SourceSeat == donor && recovery.Attempt.TargetSeat == donor && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == ChenglieGiftReason;
    }

    internal bool ChenglieOwnMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.Chenglie!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieCardMovedEvent>().Any(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index && e.CardId == m.CardId && e.From == m.From && e.To == m.To && e.Reason == m.Reason && e.Sequence == m.Sequence))
            return true;
        if (r.Stage == ChenglieStage.RevealChildren)
            return m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        return r.Stage == ChenglieStage.GiftChildren && r.GiftId is { } id && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.WoodenOx && r.GiftFrom is { Zone: CardZoneKind.Equipment, OwnerSeat: { } donor } && m.From == CardLocation.WoodenOxGrain(donor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }

    internal bool MaChengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Chenglie is not { } r || !ChenglieChildren(r.Stage))
            return false;
        AssertMaCheng(f);
        if (child is RecoveryReplacementFrame recovery)
            return IsMaChengRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => ChenglieOwnMovement(f, m));
    }

    internal ProgramSkillFrame? MaChengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterMaChengOlOperations.MaChengFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1010)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1120)]
    internal bool HasMaChengDamageObserver(long id) => MaChengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1150)]
    internal bool HasMaChengBeforeDamageObserver(long id) => MaChengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 980)]
    internal bool HasMaChengDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MaChengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27560)]
    internal bool AdvanceMaChengSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaChengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal bool HasChenglieCompletedUseObserver(long useId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == useId);
        if (index < 0 || index + 2 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame { Step: ResolutionFrameStep.Completed, ChenglieLease: { Settled: false } lease } use || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramCardTriggerWindowFrame { Continuation: ProgramCardContinuation.CompletedSlash or ProgramCardContinuation.CompletedCard } window || window.ParentFrameId != use.Id || _resolutionStackOperations._resolutionStack[index + 2] is not ProgramSkillFrame f || f.TriggerId != "red-holder-outcomes" || f.WindowContext is not { } c || ChenglieOutcomeUse(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), c)?.Id != use.Id)
            return false;
        return f.Chenglie is null ? f.InstructionIndex is 0 or 1 : f.Chenglie.Outcome && f.Chenglie.SetupFrameId == lease.SetupFrameId;
    }

    internal void CompleteChenglieTerminalCleanup(CardUseFrame use)
    {
        if (use.ChenglieLease is not { Settled: false } lease)
            return;
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            throw new InvalidOperationException("Chenglie's live whole-use outcome and cleanup must finish before native pop.");
        foreach (var card in lease.Cards)
            if (_cardZonesOperations._cardZones.GetLocation(card.CardId) == card.Location)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId), card.Location, CardLocation.DiscardPile, new(ChenglieCleanupReason));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ChenglieLease = u.ChenglieLease!with { Ready = true, Settled = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieLeaseSettledEvent(use.Id, lease.SetupFrameId, use.Id));
    }
}

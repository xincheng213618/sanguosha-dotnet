#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenQunBoundaryMobileRuntime
{
    internal static CardConversionSource BoundaryDingpinSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool BoundaryDingpinActiveTurn(int turn, int owner) => turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
    internal bool BoundaryDingpinCanTarget(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryDingpinTargetBannedEvent>().Any(e => e.Owner == owner && e.Target == target && BoundaryDingpinActiveTurn(e.Turn, e.TurnOwner));
    internal bool BoundaryDingpinCanDiscard(int owner, Card card)
    {
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
        var mask = 1 << (int)category;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.ActorSeat == owner && BoundaryDingpinActiveTurn(e.TurnNumber, e.TurnOwnerSeat) && (e.CardCategoryMask & mask) != 0))
            return false;
        foreach (var discard in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => BoundaryDingpinActiveTurn(e.TurnNumber, e.ActualTurnOwnerSeat)))
            foreach (var material in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == discard.InvocationId && e.From.OwnerSeat == owner))
            {
                if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryDingpinHeartExceptionEvent>().Any(e => e.Owner == owner && e.InvocationId == discard.InvocationId && e.CardId == material.CardId && e.CostSequence == material.CompletionSequence && BoundaryDingpinActiveTurn(e.Turn, e.TurnOwner)))
                    continue;
                if (CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId).Kind) == category)
                    return false;
            }

        return true;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 53600, SkillProgramEffectOp.PrepareBoundaryChenQunDingpin)]
    internal bool CanActivateBoundaryDingpin(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat;
    SkillProgramStepOutcome IBoundaryChenQunHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.PrepareBoundaryChenQunDingpin ? PrepareBoundaryDingpin(f) : ResolveBoundaryDingpin(f);
    bool IBoundaryChenQunSelection.CanDiscard(int owner, Card card) => BoundaryDingpinCanDiscard(owner, card);
    bool IBoundaryChenQunSelection.CanTarget(int owner, int target) => BoundaryDingpinCanTarget(owner, target);
    internal SkillProgramStepOutcome PrepareBoundaryDingpin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.BoundaryChenQunDingpin is not null || f.TriggerId is not null || f.ActivationId != BoundaryChenQunComposition.Activation || f.InstructionIndex != 1 || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats is not [var target] || !BoundaryDingpinCanTarget(f.OwnerSeat, target) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(id)is not { Zone: CardZoneKind.Hand or CardZoneKind.Equipment, OwnerSeat: { } owner } from || owner != f.OwnerSeat || !BoundaryDingpinCanDiscard(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))
            throw new InvalidOperationException("Dingpin requires one presently unused/undiscarded category, its own physical HE cost and a legal actual-Play target.");
        var r = new BoundaryDingpinReceipt(BoundaryDingpinSource(f), f.GameplayHash, phase, target, id, from, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryChenQunDingpin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDingpinPreparedEvent(f.Id, r.Source, r.Hash, phase, target, id, from, r.Category, r.Before));
        return SkillProgramStepOutcome.Continue;
    }

    internal (NativeLogicalDiscardInvocationRecordedEvent Invocation, NativeLogicalDiscardMaterialCompletedEvent Material)? BoundaryDingpinCost(ProgramSkillFrame f)
    {
        var r = f.BoundaryChenQunDingpin!;
        var candidates = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == f.Id && e.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}" && e.TurnNumber == r.Phase.TurnNumber && e.ActualTurnOwnerSeat == r.Phase.ActualTurnOwnerSeat && e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == f.OwnerSeat && e.SequenceBefore >= r.Before && e.ActualCount == 1).ToArray();
        if (candidates is not [var invocation])
            return null;
        var materials = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == invocation.InvocationId).ToArray();
        if (materials is not [var m] || m.CardId != r.CostId || m.From != r.From || m.CompletionSequence <= r.Before || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.CardId == m.CardId && move.Sequence == m.CompletionSequence && move.To == m.To && (move.From == m.From || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move) == m.From)))
            return null;
        return (invocation, m);
    }

    internal SkillProgramStepOutcome ResolveBoundaryDingpin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.BoundaryChenQunDingpin ?? throw new InvalidOperationException("Dingpin lost its paid original frame.");
        if (f.InstructionIndex != 5 || r.Stage != BoundaryDingpinStage.Prepared || BoundaryDingpinCost(f)is not { } paid || f.JudgmentOutcomeBindings?.SingleOrDefault(o => o.Name == BoundaryChenQunComposition.Outcome)is not { InstructionIndex: 4 } judgment || judgment.TargetSeat != r.Target || judgment.Reason != BoundaryChenQunComposition.Reason || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, judgment))
            throw new InvalidOperationException("Dingpin requires the exact completed native discard and finalized judgment outcome, even if its physical judgment card was claimed.");
        var black = judgment.Suit is Suit.Spade or Suit.Club;
        var count = black && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Target].HasHpValue ? Math.Clamp(_coreMatchParticipantsPlayersAccess._players[r.Target].Hp, 0, 3) : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryChenQunDingpin = r = r with { CostInvocationId = paid.Invocation.InvocationId, CostSequence = paid.Material.CompletionSequence, Judgment = judgment, Requested = count, Stage = black ? BoundaryDingpinStage.Drawing : BoundaryDingpinStage.Resolved } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDingpinResultEvent(f.Id, r.Target, judgment.JudgmentFrameId, judgment.Suit, r.CostInvocationId.Value, r.CostSequence.Value, count));
        if (judgment.Suit == Suit.Heart)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDingpinHeartExceptionEvent(f.Id, f.OwnerSeat, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat, paid.Invocation.InvocationId, r.CostId, paid.Material.CompletionSequence));
        if (!black)
            return SkillProgramStepOutcome.Continue;
        return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Target, count, CharacterChenQunBoundaryMobileDefinitions.BoundaryDingpinDrawReason);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryChenQunDingpin)]
    internal void ReturnBoundaryDingpinDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.BoundaryChenQunDingpin;
        if (r is not { Stage: BoundaryDingpinStage.Drawing, Draw: null, Judgment.Suit: Suit.Spade or Suit.Club } || f.InstructionIndex != 5 || f.NativeDraw is not null || outcome.RecipientSeat != r.Target || outcome.RequestedCount != r.Requested || outcome.SequenceBefore > outcome.SequenceAfter || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.ActualDrawIds.Count > r.Requested || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Target) && m.Reason == CharacterChenQunBoundaryMobileDefinitions.BoundaryDingpinDrawReason).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Dingpin received a different or repeated native bounded-HP draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryChenQunDingpin = r with { Stage = BoundaryDingpinStage.DrawChildren, Draw = outcome }, PendingMovementContinuation = new(r.Target, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDingpinDrawReturnedEvent(f.Id, r.Target, r.Requested, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryDingpinDraw(f.Id);
    }

    internal void ContinueBoundaryDingpinDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryChenQunDingpin: { Stage: BoundaryDingpinStage.DrawChildren } r } f || f.Id != id)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryDingpinTargetBannedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Dingpin cannot repeat the same paid black-result target restriction.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDingpinTargetBannedEvent(f.Id, f.OwnerSeat, r.Target, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryChenQunDingpin = r with { Stage = BoundaryDingpinStage.Resolved }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 53600)]
    internal bool ResumeBoundaryDingpin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryChenQunDingpin.Stage: BoundaryDingpinStage.DrawChildren } f || f.Id != id)
            return false;
        ContinueBoundaryDingpinDraw(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 53600)]
    internal bool ReturnBoundaryDingpinMovement(ProgramSkillFrame f)
    {
        if (f.BoundaryChenQunDingpin?.Stage != BoundaryDingpinStage.DrawChildren)
            return false;
        ContinueBoundaryDingpinDraw(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53600)]
    internal bool KeepBoundaryDingpinPaidFrame(ProgramSkillFrame f) => f.BoundaryChenQunDingpin is not null && BoundaryDingpinCost(f)is not null;
}

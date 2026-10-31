#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanYuOlRuntime
{
    internal bool ValidRuanYu(ProgramSkillFrame f)
    {
        if (f.RuanYu is not { } r)
            return false;
        if (f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || r.Source != RuanYuSource(f) || r.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !RuanYuComposition.Handles(r.Op) || r.ParentId != context.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Recipient) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<RuanYuStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Op == r.Op && e.ParentId == r.ParentId && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveXingzuoIssuedEndingExchange)
        {
            var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
            if (CurrentRuanYuEndingDebt(c, context)is not { } debt || r.DebtId != debt.DebtId || _coreProgramEventHistoryOperations.ProgramEventHistory<XingzuoEndingConsumedEvent>().Count(e => e.DebtId == debt.DebtId && e.FrameId == f.Id && e.ParentId == r.ParentId) != 1)
                return false;
        }
        else if (r.Op == SkillProgramEffectOp.ResolveMiaoxianUniqueRedUseDraw)
        {
            if (r.ActionId is not { } action || context.CardUse?.CardActionId != action || _coreProgramEventHistoryOperations.ProgramEventHistory<MiaoxianRedDrawIssuedEvent>().Count(e => e.FrameId == f.Id && e.ActionId == action && e.Source == r.Source && e.ActualTurn == r.ActualTurn) != 1 || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.ParentId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } candidate || candidate.OwnerSeat != f.OwnerSeat || candidate.SkillId != f.SkillId || candidate.TriggerId != f.TriggerId || candidate.SkillInstanceId != f.SkillInstanceId || candidate.GameplayHash != r.GameplayHash || !HasRuanYuRedPayment(window.Action, r.Source, r.GameplayHash, window.ParentFrameId))
                return false;
        }
        else
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
            if (context.Window != SkillProgramTriggerWindow.PlayPhaseStarting || context.SourceSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not PlayPhaseStartingBoundaryFrame start || start.Id != r.ParentId || start.OwnerSeat != f.OwnerSeat || start.ItemIndex < 0 || start.ItemIndex >= start.Items.Count || start.Items[start.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || context.OccurrenceIndex != candidate.OccurrenceIndex)
                return false;
        }

        if (r.SelectedHand.Distinct().Count() != r.SelectedHand.Count || r.SelectedBottom.Distinct().Count() != r.SelectedBottom.Count || r.OrderedBottom.Distinct().Count() != r.OrderedBottom.Count || r.ViewedBottom.Distinct().Count() != r.ViewedBottom.Count || r.ViewedBottom.Count > 3 || r.Entities.Select(c => c.CardId).Distinct().Count() != r.Entities.Count)
            return false;
        var moving = r.Stage is RuanYuStage.FrontChildren or RuanYuStage.EndingChildren or RuanYuStage.RedChildren;
        if (moving)
        {
            if (!r.Paid || f.PendingMovementContinuation is not { } p || !IsRuanYuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) || r.Step <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<RuanYuMovementPaidEvent>().Count(e => e.FrameId == f.Id && e.Step == r.Step && e.Stage == r.Stage && e.Before == r.Before && e.After == r.After && e.Count == r.ActualCount) != 1)
                return false;
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<RuanYuEntityMovedEvent>().Where(e => e.FrameId == f.Id && e.Step == r.Step).ToArray();
            if (paid.Any(e => e.Sequence <= r.Before || e.Sequence > r.After || !r.Entities.Any(c => c.CardId == e.CardId && c.Kind == e.Kind && c.From == e.From) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To && m.Reason.Value == RuanYuExchangeReason)) || r.Stage != RuanYuStage.RedChildren && paid.Length != r.ActualCount)
                return false;
        }
        else if (f.PendingMovementContinuation is not null)
            return false;
        if (r.Stage is RuanYuStage.LoseHpIssuing or RuanYuStage.LoseHpChildren)
        {
            if (!r.Paid || r.Op != SkillProgramEffectOp.ResolveXingzuoIssuedEndingExchange || r.SelectedHand.Count <= 3 || _coreProgramEventHistoryOperations.ProgramEventHistory<RuanYuMovementPaidEvent>().Count(e => e.FrameId == f.Id && e.Step == r.Step && e.Stage == RuanYuStage.EndingChildren && e.Before == r.Before && e.After == r.After && e.Count == r.ActualCount) != 1)
                return false;
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Where(e => e.FrameId == f.Id).ToArray();
            var own = _coreProgramEventHistoryOperations.ProgramEventHistory<XingzuoHpLossPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Stage == RuanYuStage.LoseHpIssuing ? paid.Length != 0 || own.Length != 0 : paid is not [var hp] || hp.SkillId != f.SkillId || hp.TargetSeat != f.OwnerSeat || hp.Amount != 1 || own is not [var receipt] || receipt.RemainingHp != hp.RemainingHp)
                return false;
        }

        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27430)]
    internal void AssertRuanYu(ProgramSkillFrame f)
    {
        if (f.RuanYu is not null && !ValidRuanYu(f))
            throw new InvalidOperationException("Ruan Yu lost his exact original source, native parent, private physical sets or once-paid stage.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 380)]
    internal bool IsRuanYuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.RuanYu is { Paid: true } r && r.Stage is RuanYuStage.FrontChildren or RuanYuStage.EndingChildren or RuanYuStage.RedChildren && effect?.Op == r.Op && pending == f.PendingMovementContinuation && pending.SubjectSeat == r.Recipient && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    internal bool RuanYuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.RuanYu is not { Paid: true } r || !ValidRuanYu(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == RuanYuStage.RedChildren ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MiaoxianDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle : _coreProgramEventHistoryOperations.ProgramEventHistory<RuanYuEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Sequence == m.Sequence && e.CardId == m.CardId && e.From == m.From && e.To == m.To)));
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == RuanYuStage.LoseHpChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.SourceSeat is null && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Loss && hp.Change.Amount == 1;
        if (child is DyingFrame dying)
            return r.Stage == RuanYuStage.LoseHpChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.Amount == 1 && e.RemainingHp <= 0) == 1;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? RuanYuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterRuanYuOlOperations.RuanYuFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 890)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1000)]
    internal bool HasRuanYuDamageObserver(long id) => RuanYuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1030)]
    internal bool HasRuanYuBeforeDamageObserver(long id) => HasRuanYuDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 860)]
    internal bool IsRuanYuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && RuanYuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27430)]
    internal bool AdvanceRuanYuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && RuanYuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

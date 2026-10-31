#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePairedColorResponseObserversRuntime
{
    internal CardLocation PairedColorPaidDestination(ProgramSkillFrame f, ProgramPairedColorDispositionReceipt r)
    {
        var intended = r.PaidGeneralWeapon && r.PaidFrom?.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : r.Obtain ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile;
        return r.PaidCardId is { } id && r.PaidFrom is { } from ? _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), from, intended) : intended;
    }

    internal string PairedColorPaidReason(ProgramPairedColorDispositionReceipt r) => r.Obtain ? "skill-program.paired-color-response.obtain" : "skill-program.paired-color-response.discard";
    internal bool ValidPairedColorDispositionReceipt(ProgramSkillFrame f)
    {
        if (f.PairedColorDisposition is not { } r || !_corePairedColorResponsesOperations.ExactPairedColorDispositionParent(f, out _, out var pair, out var counterpart) || r.InstructionIndex != f.InstructionIndex || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.Pair != pair || r.CounterpartSeat != counterpart || !Enum.IsDefined(r.Stage) || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects[0].StateId != r.StateId)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardResponseCompleted) != 1 || history.OfType<PairedColorDispositionStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new PairedColorDispositionStartedEvent(f.Id, r.Source, r.GameplayHash, r.StateId, pair.ResponseActionId, pair.PairedActionId, counterpart, r.Obtain) || history.OfType<PairedColorDispositionCompletedEvent>().Any(e => e.FrameId == f.Id))
            return false;
        var payments = history.OfType<PairedColorDispositionPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == PairedColorDispositionStage.ChoosingCard)
            return f.PendingMovementContinuation is null && payments.Length == 0 && r.PaidCardId is null && r.PaidKind is null && r.PaidFrom is null && !r.PaidGeneralWeapon && r.SequenceBefore == 0 && r.SequenceAfter == 0 && r.BatchId is null;
        if (r.PaidCardId is not { } id || r.PaidKind is not { } kind || r.PaidFrom is not { } from || from.OwnerSeat != counterpart || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.SequenceBefore < 0 || r.SequenceAfter <= r.SequenceBefore || r.BatchId is not { } batch || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != counterpart || payments is not [var paid] || paid != new PairedColorDispositionPaidEvent(f.Id, id, kind, from, PairedColorPaidDestination(f, r), r.PaidGeneralWeapon, r.SequenceBefore, r.SequenceAfter, batch))
            return false;
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        return moves is [var moved] && moved.CardId == id && moved.CardKind == kind && moved.From == from && moved.To == paid.To && moved.Reason.Value == PairedColorPaidReason(r);
    }

    internal bool IsPairedColorDispositionChoice(ProgramSkillFrame f, PendingDecision decision) => f.PairedColorDisposition is { Stage: PairedColorDispositionStage.ChoosingCard } r && ValidPairedColorDispositionReceipt(f) && decision.Kind == DecisionKind.ProgramTrigger && decision.PlayerSeat == f.OwnerSeat && decision.IsPrivate && decision.SkillPrompt?.SkillId == f.SkillId && decision.Choices.Count > 0 && decision.Choices.Count == _corePairedColorResponsesOperations.PairedColorDispositionChoices(f).Count && decision.Choices.Zip(_corePairedColorResponsesOperations.PairedColorDispositionChoices(f)).All(p => global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime.SameNameHandChoicesEqual(p.First, p.Second)) && decision.ValidCardIds.SequenceEqual(decision.Choices.SelectMany(c => c.Cards).Distinct()) && decision.ValidTargetSeats.Count == 0 && decision.TargetSeat == r.CounterpartSeat && decision.SourceSeat == f.OwnerSeat && decision.ValidContentIds.Count == 0 && decision.RequiredCardCount == 0 && decision.IncomingCard is null && decision.RequiredCardKind is null;

    internal bool PairedColorDispositionFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.PairedColorDisposition is not { Stage: PairedColorDispositionStage.MovementChildren } r || !ValidPairedColorDispositionReceipt(f))
            return false;
        bool PaidEquipment(CardKind kind) => r.PaidKind == kind && r.PaidFrom == CardLocation.Equipment(r.CounterpartSeat);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (moved.Batch.Id == r.BatchId && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == r.PaidCardId && m.From == r.PaidFrom && m.To == PairedColorPaidDestination(f, r) && m.Reason.Value == PairedColorPaidReason(r) || PaidEquipment(CardKind.WoodenOx) && moved.Batch.ParentBatchId == r.BatchId && m.Sequence > r.SequenceAfter && m.From == CardLocation.WoodenOxGrain(r.CounterpartSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return r.PaidFrom?.Zone == CardZoneKind.Equipment && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (!PaidEquipment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.CounterpartSeat && hp.Change.TargetSeat == r.CounterpartSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.CounterpartSeat && recovery.Attempt.TargetSeat == r.CounterpartSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == PairedColorPaidReason(r);
    }

    internal bool PairedColorDispositionStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is ProgramSkillFrame root && PairedColorDispositionFirstChild(root, child) || parent is ProgramCardTriggerWindowFrame window && child is ProgramSkillFrame { PairedColorDisposition: not null } program && program.WindowContext?.ParentFrameId == window.Id && ValidPairedColorDispositionReceipt(program);
    internal ProgramSkillFrame? PairedColorDispositionObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.PairedColorDispositionFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 40)]
    internal bool IsPairedColorDispositionDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && PairedColorDispositionObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 110)]
    internal bool TryAdvancePairedColorDispositionSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || PairedColorDispositionObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}

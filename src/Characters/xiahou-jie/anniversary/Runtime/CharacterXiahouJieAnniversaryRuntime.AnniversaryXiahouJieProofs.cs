#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouJieAnniversaryRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45500)]
    internal bool KeepPaidAnniversaryXiahouJie(ProgramSkillFrame f)
    {
        if (f.AnniversaryXiahouJie is null)
            return false;
        return true;
    }

    internal bool AnniversaryXiahouJieFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryXiahouJie is not { } r)
            return false;
        if (r.Stage == AnniversaryXiahouJieStage.Drawing && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return true;
        if (child is DyingFrame dying)
            return r.Stage == AnniversaryXiahouJieStage.HpChildren && r.HpPaid && r.HpAfter <= 0 && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.KillerSeat is null && dying.ResumesProgramSkill;
        if (child is DeathFrame death)
            return r.Stage == AnniversaryXiahouJieStage.DirectDeath && r.DeathMarks >= 5 && death.ParentFrameId == f.Id && death.VictimSeat == f.OwnerSeat && death.ReturnKind == DeathReturnKind.ProgramSkill;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.SourceSeat is null && (r.Stage == AnniversaryXiahouJieStage.HpChildren && r.HpPaid && hp.Change.Kind == HpChangeKind.Loss && hp.Change.Amount == 1 && hp.Change.HpAfter == r.HpAfter || r.Stage == AnniversaryXiahouJieStage.MaximumChildren && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.Amount == 1);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == AnniversaryXiahouJieStage.DrawChildren && moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && r.Draws.Any(d => m.Sequence > d.Outcome.SequenceBefore && m.Sequence <= d.Outcome.SequenceAfter));
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? AnniversaryXiahouJieObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterXiahouJieAnniversaryOperations.AnniversaryXiahouJieFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 45500)]
    internal bool HasAnniversaryXiahouJieDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnniversaryXiahouJieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 45500)]
    internal bool AdvanceAnniversaryXiahouJieObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryXiahouJieObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.LifecycleWindowCompleted, 45500)]
    internal bool CompleteAnniversaryXiahouJiePreparation(ProgramLifecycleTriggerWindowFrame parent)
    {
        if (parent.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || parent.Continuation != ProgramLifecycleContinuation.NormalTurnStart || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None || parent.CandidateIndex != parent.Candidates.Count || _resolutionStackOperations._resolutionStack.Count != 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXiahouJieStartedEvent>().Any(s => s.ParentId == parent.Id && s.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && s.TurnOwner == parent.OwnerSeat && parent.Candidates.Any(c => c.OwnerSeat == s.Source.OwnerSeat && c.SkillId == s.Source.SkillId && c.BindingId == s.Source.BindingId && c.SkillInstanceId == s.Source.SkillInstanceId && c.GameplayHash == s.Hash) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXiahouJieCompletedEvent>().Count(c => c.FrameId == s.FrameId) == 1))
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == parent.OwnerSeat))
            throw new InvalidOperationException("Liedan cannot end its actual preparation twice.");
        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None)
            _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat]);
        return true;
    }
}

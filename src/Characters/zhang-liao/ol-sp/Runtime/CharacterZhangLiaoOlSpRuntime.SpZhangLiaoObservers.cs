#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoOlSpRuntime
{
    internal bool SpZhangLiaoFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.SpZhangLiao is not { } r)
            return false;
        AssertSpZhangLiao(root);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.Reason.Value == SpZhangLiaoMovementReason(root));
        if (r.Stage == SpZhangLiaoStage.HighestChildren && r.VictimSeat is { } victim && r.Selected is [var card] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == card && m.From == CardLocation.Equipment(victim) && m.To == CardLocation.Hand(root.OwnerSeat) && m.CardKind == CardKind.SilverLion))
        {
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == victim && hp.Change.TargetSeat == victim && hp.Change.Amount == 1;
            if (child is RecoveryReplacementFrame recovery)
                return recovery.ParentFrameId == root.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, root.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason(SpZhangLiaoMovementReason(root)) && recovery.Attempt.SourceSeat == victim && recovery.Attempt.TargetSeat == victim && recovery.Attempt.Amount == 1;
        }

        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal string SpZhangLiaoMovementReason(ProgramSkillFrame f) => f.SpZhangLiao!.Stage switch
    {
        SpZhangLiaoStage.Payment => $"skill-program.{(f.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f))}.reveal",
        SpZhangLiaoStage.PaymentChildren => "skill-program.ol:mubing.DiscardSelected",
        SpZhangLiaoStage.ObtainChildren => "skill-program.ol:mubing.ObtainSelected",
        SpZhangLiaoStage.GiftChildren => "skill-program.ol:mubing.GiveSelected",
        SpZhangLiaoStage.HighestChildren => "skill-program.ol:ziqu.GiveSelected",
        SpZhangLiaoStage.CleanupChildren => "skill-program.ol:mubing.reveal-cleanup",
        _ => ""
    };
    internal ProgramSkillFrame? SpZhangLiaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterZhangLiaoOlSpOperations.SpZhangLiaoFirstChild(root, child), (child) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(child));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 330)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 440)]
    internal bool HasSpZhangLiaoDamageObserver(long id) => SpZhangLiaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && (id == root.WindowContext?.ParentFrameId || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 300)]
    internal bool IsSpZhangLiaoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SpZhangLiaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26070)]
    internal bool TryAdvanceSpZhangLiaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SpZhangLiaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}

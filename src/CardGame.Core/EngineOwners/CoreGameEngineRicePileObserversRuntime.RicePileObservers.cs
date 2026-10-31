#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRicePileObserversRuntime
{
    internal bool RiceFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.RicePile is not { } r)
            return false;
        _coreRicePileReceiptsOperations.RequireRiceReceipt(root);
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (root.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved)
        {
            var before = r.Stage == RicePileStage.DrawChildren ? r.DrawBefore : r.Before;
            var after = r.Stage == RicePileStage.DrawChildren ? r.DrawAfter : r.After;
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && (r.Stage == RicePileStage.DrawChildren ? m.Reason.Value == "skill-program.rice.draw" : r.Operation == SkillProgramEffectOp.ReplaceJudgmentFromPublicRice ? m.Reason == CardMoveReasons.ProgramJudgmentOldCard || m.Reason == CardMoveReasons.ProgramJudgmentReplace : m.Reason.Value == (r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? "skill-program.rice.store" : "skill-program.rice.obtain")));
        }

        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        var lion = r.CardIds.Select((id, index) => (id, from: r.From[index])).Any(p => p.from == CardLocation.Equipment(root.OwnerSeat) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == p.id && m.CardKind == CardKind.SilverLion && m.From == p.from));
        if (r.Stage != RicePileStage.RecoveryChildren && !lion)
            return false;
        var continuation = lion ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == continuation && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == continuation && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == 1 && (lion ? recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == "skill-program.rice.store" : recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && r.RecoveryIssued);
    }

    internal ProgramSkillFrame? RiceObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreRicePileObserversOperations.RiceFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 470)]
    internal bool TryAdvanceRiceSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && RiceObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

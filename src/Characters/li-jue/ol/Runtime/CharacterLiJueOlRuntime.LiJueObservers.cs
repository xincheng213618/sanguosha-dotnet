#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiJueOlRuntime
{
    internal bool LiJueFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueLiJue(root) || root.LiJue is not { } r)
            return false;
        if (child is DeathFrame died)
            return r.Stage == LiJueStage.MaximumHpChildren && r.MaxAfter == 0 && died.ParentFrameId == root.Id && died.VictimSeat == root.OwnerSeat;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == LiJueStage.MaximumHpChildren && root.PendingMovementContinuation is null && hp.Continuation == PostEventContinuation.Program && hp.ResumeFrameId == root.Id && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.ParentFrameId == root.Id;
        if (child is ProgramLifecycleTriggerWindowFrame skills)
            return skills.ResumeProgramFrameId == root.Id && skills.Window == SkillProgramTriggerWindow.SkillsChanged;
        if (child is not CardsMovedTriggerWindowFrame moved || r.Stage != LiJueStage.GainChildren)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == YisuanGainReason);
    }

    internal ProgramSkillFrame? LiJueObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.LiJueFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 270)]
    internal bool TryAdvanceLiJueSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiJueObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}

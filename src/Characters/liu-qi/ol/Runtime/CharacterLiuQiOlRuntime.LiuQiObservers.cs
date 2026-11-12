#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuQiOlRuntime
{
    internal bool LiuQiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueLiuQi(root) || child is not CardsMovedTriggerWindowFrame moved || root.LiuQi is not { } r)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == WenjiGiftReason);
    }

    internal ProgramSkillFrame? LiuQiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.LiuQiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 220)]
    internal bool TryAdvanceLiuQiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuQiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}

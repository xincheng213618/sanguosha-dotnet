#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSuFeiOlRuntime
{
    internal bool SuFeiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueSuFei(root) || root.SuFei is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        var before = r.Stage == SuFeiStage.DrawChildren ? r.DrawBefore : r.GiftBefore;
        var after = r.Stage == SuFeiStage.DrawChildren ? r.DrawAfter : r.GiftAfter;
        return r.Stage is SuFeiStage.DrawChildren or SuFeiStage.GiftChildren && moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && (r.Stage == SuFeiStage.DrawChildren ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == SuFeiDrawReason : m.CardId == r.DrawnId && m.From == CardLocation.Hand(root.OwnerSeat) && m.To == CardLocation.Hand(r.GiftTarget!.Value) && m.Reason.Value == SuFeiGiftReason));
    }

    internal ProgramSkillFrame? SuFeiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.SuFeiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 290)]
    internal bool TryAdvanceSuFeiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SuFeiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}

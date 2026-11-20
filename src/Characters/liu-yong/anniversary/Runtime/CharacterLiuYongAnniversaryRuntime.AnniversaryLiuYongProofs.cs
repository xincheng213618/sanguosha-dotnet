#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYongAnniversaryRuntime
{
    private static NativeDrawOutcome? CompletedFengxiangDraw(ProgramSkillFrame f) =>
        NativeDrawProducerRuntime.CompletedOperationDraw(f, f.InstructionIndex, SkillProgramEffectOp.ResolveAnniversaryLiuYongFengxiang, AnniversaryLiuYongDrawReason);

    private (long Before, long After)? AnniversaryLiuYongMovementRange(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryLiuYong is not { Stage: AnniversaryLiuYongStage.GiftChildren or AnniversaryLiuYongStage.DrawChildren } r || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || effect?.Op != (r.Mode == "gift" ? SkillProgramEffectOp.ResolveAnniversaryLiuYongZhuning : SkillProgramEffectOp.ResolveAnniversaryLiuYongFengxiang))
            return null;
        var draw = r.Stage == AnniversaryLiuYongStage.DrawChildren ? CompletedFengxiangDraw(f) : null;
        if (r.Stage == AnniversaryLiuYongStage.DrawChildren && draw is null)
            return null;
        var before = draw?.SequenceBefore ?? r.Before;
        var after = draw?.SequenceAfter ?? r.After;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray();
        if (draw is not null)
        {
            if (draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != 1 ||
                _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuYongDrawReturnedEvent>()
                    .Count(e => e == new AnniversaryLiuYongDrawReturnedEvent(f.Id, draw.AttemptFrameId, before, after, draw.ActualDrawIds.Count)) != 1 ||
                !draw.ActualDrawIds.All(id => moves.Count(m => m.CardId == id && m.From == CardLocation.DrawPile &&
                    m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason == AnniversaryLiuYongDrawReason) == 1))
                return null;
        }
        else if (!r.GiftPaid || r.Cards.Count != r.From.Count ||
            !r.Cards.Select((id, index) => (id, from: r.From[index])).All(x => moves.Count(m => m.CardId == x.id && m.From == x.from &&
                m.To == CardLocation.Hand(r.Recipient) && m.Reason == AnniversaryLiuYongGiftReason) == 1))
            return null;
        return (before, after);
    }

    internal bool AnniversaryLiuYongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryLiuYong is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryLiuYongStage.NativeUse && (IsAnniversaryLiuYongSlashUse(use) || IsAnniversaryLiuYongVirtualUse(use));
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryLiuYongStage.Drawing &&
                NativeDrawProducerRuntime.MatchesOperationAttempt(f, draw, SkillProgramEffectOp.ResolveAnniversaryLiuYongFengxiang, AnniversaryLiuYongDrawReason);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } pending &&
                AnniversaryLiuYongMovementRange(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) is { } range &&
                CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) &&
                moved.Batch.Movements.All(m => m.Sequence > range.Before && m.Sequence <= range.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is HappyQiaoHealthChoiceFrame health)
            return r.Stage == AnniversaryLiuYongStage.HealthChildren && _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(health, f);
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && (r.Stage == AnniversaryLiuYongStage.HealthChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.Recipient && recovery.Attempt.Amount == 1 && recovery.Return.Continuation == PostEventContinuation.Program || r.Stage == AnniversaryLiuYongStage.GiftChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Completion.MoveReason == AnniversaryLiuYongGiftReason && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(f.OwnerSeat) && m.Reason == AnniversaryLiuYongGiftReason && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == CardKind.SilverLion));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && (r.Stage == AnniversaryLiuYongStage.HealthChildren && hp.Change.TargetSeat == r.Recipient && hp.Continuation == PostEventContinuation.Program || r.Stage == AnniversaryLiuYongStage.GiftChildren && hp.Change.TargetSeat == f.OwnerSeat && hp.Continuation == PostEventContinuation.AwaitedProgramMovement);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? AnniversaryLiuYongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.AnniversaryLiuYongFirstChild(f, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 43400)]
    internal bool AnniversaryLiuYongDyingObserver() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && AnniversaryLiuYongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 43400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 43400)]
    internal bool AnniversaryLiuYongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryLiuYong is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && AnniversaryLiuYongFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || Enumerable.Range(parent + 2, child - parent - 1).All(i => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i))) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 43400)]
    internal bool AdvanceAnniversaryLiuYongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryLiuYongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

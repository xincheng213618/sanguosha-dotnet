#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunXuAnniversaryRuntime
{

    internal bool HasXunXuNativeFinishOwner(ProgramSkillFrame f, out CardUseFrame? use)
    {
        use = null;
        if (f.AnniversaryXunXu is not { Mode: "boqia", NativeUseId: > 0 } r)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 2 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramCardTriggerWindowFrame window || window.Id != r.ParentId || window.Action.ActionId != r.ActionId || window.Action.Type != CardActionType.Use || window.CompletedResponseReturn is not null || _resolutionStackOperations._resolutionStack[i - 2] is not CardUseFrame { Step: ResolutionFrameStep.Completed, Action: { } action } original || original.Id != r.NativeUseId || window.ParentFrameId != original.Id || action.ActionId != r.ActionId || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.PhysicalCards.Count != 3 || !action.PhysicalCards.Select(c => c.CardId).SequenceEqual(original.PhysicalCardIds ?? []) || action.ConversionChain.SingleOrDefault(s => s.SkillId == AnniversaryXunXuComposition.Boqia)is not { } source || source.OwnerSeat != f.OwnerSeat || source.SkillInstanceId != f.SkillInstanceId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuUseEvent>().Count(e => e.Source == source && e.ActionId == action.ActionId && e.Reward) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.Id) != 1)
            return false;
        use = original;
        return true;
    }

    internal bool IsXunXuFinishedMaterialBatch(CardUseFrame use, CardMovementBatchContext b) => use.Action is { Type: CardActionType.Use } a && b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.Movements.Count > 0 && b.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && b.Movements.All(m => m.Sequence > 0 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished && a.PhysicalCards.Any(c => c.CardId == m.CardId && c.CardKind == m.CardKind)) && b.Movements.Select(m => m.CardId).Distinct().Count() == b.Movements.Count;
    internal bool IsAnniversaryXunXuSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.AnniversaryXunXu is { Stage: "native-children" } r && HasXunXuNativeFinishOwner(f, out var use) && IsXunXuFinishedMaterialBatch(use!, b) && r.NativeFinishBatches.Any(frozen => frozen.Id == b.Id && frozen.ParentFrameId == b.ParentFrameId && frozen.AwaitingProgramFrameId == b.AwaitingProgramFrameId && frozen.Movements.SequenceEqual(b.Movements) && frozen.SourceCounts.SequenceEqual(b.SourceCounts) && (frozen.DestinationCounts ?? []).SequenceEqual(b.DestinationCounts ?? [])) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuNativeBatchEvent>().Count(e => e.FrameId == f.Id && e.UseId == use!.Id && e.ActionId == r.ActionId && e.BatchId == b.Id && e.Before == b.Movements.Min(m => m.Sequence) - 1 && e.After == b.Movements.Max(m => m.Sequence)) == 1;
    internal bool XunXuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryXunXu is not { Stage: "children" or "native-children" } r || effect?.Op is not (SkillProgramEffectOp.ResolveAnniversaryXunXuDiyin or SkillProgramEffectOp.ResolveAnniversaryXunXuBoqiaCompletion) || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        if (r.Stage == "native-children")
            return r.NativeFinishBatches.Count > 0 && HasXunXuNativeFinishOwner(f, out _);
        if (r.Draws.LastOrDefault()is { } d && d.SequenceBefore == r.Before && d.SequenceAfter == r.After)
            return true;
        var mode = r.Mode == "claim" ? "claim" : "discard";
        if (mode == "claim")
        {
            if (r.Gain is not { } gain || gain.Reason != XunXuGainReason || gain.Movements.Any(m => m.From != CardLocation.DiscardPile))
                return false;
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(gain, f.Id, r.Cards, CardLocation.Hand(r.Target), r.Before);
        }

        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuMovementEvent>().Count(e => e == new AnniversaryXunXuMovementEvent(f.Id, mode, r.Target, r.Cards.Count, r.Before, r.After)) == 1 && r.Cards.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && (mode == "claim" ? m.From == CardLocation.DiscardPile && (m.To == CardLocation.Hand(r.Target) || m.To == CardLocation.OutsideGame || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(r.Target))) && m.Reason == XunXuGainReason : (m.From == CardLocation.Hand(r.Target) || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(r.Target)) && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)) && m.Reason == XunXuDiscardReason)) == 1);
    }

    internal bool XunXuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryXunXu is not { } r)
            return false;
        if (child is HappyQiaoHealthChoiceFrame qiao)
            return r.Stage == "hp" && _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(qiao, f);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == "drawing" && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == r.Target && draw.Context.RequestedCount == r.Count && CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame settled && r.Stage == "native-children")
            return settled.Id == settled.Batch.Id && settled.ResumeProgramFrameId == f.Id && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(settled) && IsAnniversaryXunXuSettlementBatch(f, settled.Batch);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && XunXuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return f.PendingMovementContinuation is not null && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return f.PendingMovementContinuation is not null && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal ProgramSkillFrame? XunXuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.XunXuFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 46100)]
    internal bool XunXuDyingObserver() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && XunXuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 46100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 46100)]
    internal bool XunXuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryXunXu is null)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && j > i && XunXuFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && (j == i + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46100)]
    internal bool AdvanceXunXuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XunXuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1200)]
    internal bool WindowSettlement_IsAnniversaryXunXuSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsAnniversaryXunXuSettlementBatch(frame, batch);
}

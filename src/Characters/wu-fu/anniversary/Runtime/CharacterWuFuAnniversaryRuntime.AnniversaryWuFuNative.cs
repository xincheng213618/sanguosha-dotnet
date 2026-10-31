#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuFuAnniversaryRuntime
{
    internal bool CanWuFuSlash(int actor, int target) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor != target && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: [], countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null);
    internal void IssueWuFuSlash(ProgramSkillFrame f)
    {
        var r = f.AnniversaryWuFu!;
        var item = WuFuTask(f);
        if (!CanWuFuSlash(item.Actor, item.Target))
        {
            CompleteWuFuItem(f, false);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, AnniversaryWuFu = r with { Stage = AnniversaryWuFuStage.Issuing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreProgramVirtualSlashOffersOperations.BeginOfferedProgramVirtualSlash(f, item.Actor, item.Target);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46600)]
    internal void BindWuFuSlash(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is not { } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversaryWuFu: { Stage: AnniversaryWuFuStage.Issuing } r } f)
            return;
        var item = WuFuTask(f);
        if (!ValidWuFu(f) || item.Draw || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActorSeat != item.Actor || a.ProviderSeat != item.Actor || a.RequesterSeat is not null || !a.TargetSeats.SequenceEqual([item.Target]))
            throw new InvalidOperationException("Zhonge cannot substitute its ordered native Slash's actor, target or material.");
        var origin = new AnniversaryWuFuSlashOrigin(f.Id, r.Source, r.Hash, r.Cursor, use.Id, a.ActionId, item.Actor, item.Target);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryWuFu = r with { Stage = AnniversaryWuFuStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { AnniversaryWuFuSlash = origin });
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(item.Actor, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuSlashIssuedEvent(origin));
    }

    internal bool IsWuFuUse(CardUseFrame use)
    {
        if (use.AnniversaryWuFuSlash is not { } o || use.Id != o.UseId || use.CardId != 0 || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0 } a || a.ActionId != o.ActionId || a.ProviderSeat != o.Actor || a.RequesterSeat is not null || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is not ProgramSkillFrame { AnniversaryWuFu: { Stage: AnniversaryWuFuStage.NativeUse } r } f || !ValidWuFu(f) || o.Source != r.Source || o.Hash != f.GameplayHash || o.Cursor != r.Cursor || r.UseId != use.Id || r.ActionId != a.ActionId || o.Actor != WuFuTask(f).Actor || o.Target != WuFuTask(f).Target || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuSlashIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        return _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Actor, o.Actor);
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 46600)]
    internal bool FinishWuFuAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { AnniversaryWuFuSlash: not null } use || !IsWuFuUse(use))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(46600)]
    internal void ValidateWuFuUse(CardUseFrame use)
    {
        if (use.AnniversaryWuFuSlash is not null && (!IsWuFuUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Zhonge must finish its exact whole native Slash before the next ordered item.");
    }

    [NativeUseReturn(46600)]
    internal void ReturnWuFuUse(CardUseFrame use)
    {
        if (use.AnniversaryWuFuSlash is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.AnniversaryWuFu!;
        if (r.UseId != use.Id || r.ActionId != o.ActionId || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Zhonge cannot return another native use or return its own twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryWuFu = r with { Returned = true } });
        if (use.CardAttack is not { Active: true })
            CompleteWuFuSlash(o.FrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 46600)]
    internal bool ReturnWuFuAttack(GameEngine.AttackCompletionReceipt receipt)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryWuFu: { Stage: AnniversaryWuFuStage.NativeUse, Returned: true } r } f || r.UseId != receipt.ResolutionId)
            return false;
        if (receipt.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Zhonge's native attack returned to another producer.");
        CompleteWuFuSlash(f.Id);
        return true;
    }

    internal void CompleteWuFuSlash(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.AnniversaryWuFu!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuSlashReturnedEvent>().Any(e => e.FrameId == id && e.Cursor == r.Cursor))
            throw new InvalidOperationException("Zhonge must release each complete original native Slash exactly once.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuSlashReturnedEvent(id, r.Cursor, r.UseId, r.ActionId));
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var batch in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuNativeBatchEvent(id, r.UseId, batch.Id, batch.Movements.Min(m => m.Sequence) - 1, batch.Movements.Max(m => m.Sequence)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryWuFu = r with { Stage = AnniversaryWuFuStage.NativeChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueWuFu(id);
    }

    internal bool IsAnniversaryWuFuSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.AnniversaryWuFu is { Stage: AnniversaryWuFuStage.NativeChildren, Returned: true } r && ValidWuFu(f) && r.NativeBatches.Contains(b.Id) && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.Count > 0 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuNativeBatchEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.BatchId == b.Id && e.Before == b.Movements.Min(m => m.Sequence) - 1 && e.After == b.Movements.Max(m => m.Sequence)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1400)]
    internal bool WindowSettlement_IsAnniversaryWuFuSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsAnniversaryWuFuSettlementBatch(frame, batch);
}

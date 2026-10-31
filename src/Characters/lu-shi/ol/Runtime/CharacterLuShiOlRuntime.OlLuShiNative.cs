#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuShiOlRuntime
{
    internal int[] OlLuShiLeijieSlashTargets(ProgramSkillFrame f) => _coreTableOrderOperations.CurrentTableOrder().Select(seat => _coreMatchParticipantsPlayersAccess._players[seat]).Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p, CardKind.ThunderSlash, physicalSuit: Suit.None, effectiveRank: 0, ignoreDistance: true, countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, p.Seat, CardKind.ThunderSlash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, CardKind.ThunderSlash, Suit.None, null)).Select(p => p.Seat).ToArray();
    internal bool CanOlLuShiLeijieSlash(ProgramSkillFrame f, int targetSeat) => OlLuShiLeijieSlashTargets(f).Contains(targetSeat);
    internal void IssueOlLuShiLeijieSlash(ProgramSkillFrame f)
    {
        var r = f.OlLuShi!;
        var targetSeat = r.TargetSeat;
        if (r.Stage != OlLuShiStage.Ready || r.SlashCount >= 2 || !OlLuShiLeijieSlashTargets(f).Contains(targetSeat) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Leijie cannot issue an overlapping or illegal native thunder Slash.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r with { Stage = OlLuShiStage.NativeUse, SlashCount = r.SlashCount + 1, TargetSeat = targetSeat, NativeReturned = false } });
        var source = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, f.OwnerSeat, f.OwnerSeat, null, null, null, CardKind.ThunderSlash, [targetSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var origin = new OlLuShiSlashOrigin(f.Id, r.Source, r.Hash, r.SlashCount + 1, id, action.ActionId, targetSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = f.OlLuShi!with { Stage = OlLuShiStage.NativeUse, UseId = id, ActionId = action.ActionId } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, f.OwnerSeat, 0, CardKind.ThunderSlash, [targetSeat], PhysicalCardIds: []) { Action = action, UnlimitedUse = true, OlLuShiLeijie = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiSlashIssuedEvent(origin));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.ThunderSlash, f.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [targetSeat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, f.OwnerSeat, CardKind.ThunderSlash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(f.OwnerSeat, CardKind.ThunderSlash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, f.OwnerSeat, targetSeat, card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.ThunderSlash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, CardKind.ThunderSlash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, source);
        source.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(source.Seat, CardKind.ThunderSlash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.ThunderSlash, f.OwnerSeat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsOlLuShiLeijieNativeSlash(CardUseFrame use)
    {
        if (use.OlLuShiLeijie is not { } o || use.CardId != 0 || use.CardKind != CardKind.ThunderSlash || use.UnlimitedUse != true || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.ThunderSlash, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || use.Id != o.UseId || a.ActionId != o.ActionId || a.ActorSeat != o.Source.OwnerSeat || a.ProviderSeat != o.Source.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is not ProgramSkillFrame { OlLuShi: { Operation: SkillProgramEffectOp.ResolveOlLuShiLeijie, Stage: OlLuShiStage.NativeUse } r } f || r.Source != o.Source || r.Hash != o.Hash || r.SlashCount != o.Ordinal || r.TargetSeat != o.TargetSeat || r.UseId != use.Id || r.ActionId != a.ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<OlLuShiSlashIssuedEvent>().Count(e => e.Origin == o) != 1 || !HasOlLuShiJudgmentProof(f))
            return false;
        return _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Source.OwnerSeat, o.Source.OwnerSeat);
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 42701)]
    internal bool CompleteOlLuShiLeijieAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { OlLuShiLeijie: not null } use || !IsOlLuShiLeijieNativeSlash(use))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, CardKind.ThunderSlash));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(42701)]
    internal void ValidateOlLuShiLeijieSlash(CardUseFrame use)
    {
        if (use.OlLuShiLeijie is not null && (!IsOlLuShiLeijieNativeSlash(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Leijie requires its exact whole native ThunderSlash before returning.");
    }

    [NativeUseReturn(42701)]
    internal void ReturnOlLuShiLeijieSlash(CardUseFrame use)
    {
        if (use.OlLuShiLeijie is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.OlLuShi!;
        if (r.UseId != use.Id || r.ActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Leijie cannot repeat or skip a native use return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            CompleteOlLuShiLeijieSlash(o.FrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 42701)]
    internal bool ReturnOlLuShiLeijieAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlLuShi: { Stage: OlLuShiStage.NativeUse, NativeReturned: true } r } f || r.UseId != completion.ResolutionId)
            return false;
        if (completion.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Leijie's ThunderSlash returned to another parent.");
        CompleteOlLuShiLeijieSlash(f.Id);
        return true;
    }

    internal void CompleteOlLuShiLeijieSlash(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.OlLuShi!;
        if (!r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<OlLuShiSlashReturnedEvent>().Any(e => e.FrameId == id && e.Ordinal == r.SlashCount))
            throw new InvalidOperationException("Leijie cannot continue before its once-only native return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiSlashReturnedEvent(id, r.SlashCount, r.UseId, r.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = r with { Stage = OlLuShiStage.Children } });
        ContinueOlLuShi(id);
    }
}

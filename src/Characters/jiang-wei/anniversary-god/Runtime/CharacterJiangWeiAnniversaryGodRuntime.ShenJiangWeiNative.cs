#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiAnniversaryGodRuntime
{
    internal int[] ShenJwSlashTargets(ProgramSkillFrame f) => _coreTableOrderOperations.CurrentTableOrder().Select(seat => _coreMatchParticipantsPlayersAccess._players[seat]).Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p, CardKind.FireSlash, physicalSuit: Suit.None, effectiveRank: 0, countsTowardLimit: false)).Select(p => p.Seat).ToArray();
    internal void IssueShenJwSlash(ProgramSkillFrame f, int targetSeat)
    {
        var r = f.ShenJiangWei!;
        if (r.Stage != ShenJiangWeiStage.SlashChoice || r.SlashCount >= 9 || !ShenJwSlashTargets(f).Contains(targetSeat) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Pingxiang cannot issue an overlapping or illegal native fire Slash.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenJiangWei = r with { Stage = ShenJiangWeiStage.NativeIssuing, SlashCount = r.SlashCount + 1, TargetSeat = targetSeat, NativeReturned = false } });
        var source = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, f.OwnerSeat, f.OwnerSeat, null, null, null, CardKind.FireSlash, [targetSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var origin = new ShenJiangWeiSlashOrigin(f.Id, r.Source, r.GameplayHash, r.SlashCount + 1, id, action.ActionId, targetSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenJiangWei = f.ShenJiangWei!with { Stage = ShenJiangWeiStage.NativeUse, UseId = id, ActionId = action.ActionId } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, f.OwnerSeat, 0, CardKind.FireSlash, [targetSeat], PhysicalCardIds: []) { Action = action, UnlimitedUse = true, ShenJiangWeiPingxiang = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenJiangWeiSlashIssuedEvent(origin));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.FireSlash, f.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [targetSeat]));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, f.OwnerSeat, targetSeat, card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.FireSlash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, CardKind.FireSlash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, source);
        source.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(source.Seat, CardKind.FireSlash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.FireSlash, f.OwnerSeat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsShenJwNativeSlash(CardUseFrame use)
    {
        if (use.ShenJiangWeiPingxiang is not { } o || use.CardId != 0 || use.CardKind != CardKind.FireSlash || use.UnlimitedUse != true || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.FireSlash, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || use.Id != o.UseId || a.ActionId != o.ActionId || a.ActorSeat != o.Source.OwnerSeat || a.ProviderSeat != o.Source.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is not ProgramSkillFrame { ShenJiangWei: { Operation: SkillProgramEffectOp.ResolveShenJiangWeiPingxiang, Stage: ShenJiangWeiStage.NativeUse } r } f || r.Source != o.Source || r.GameplayHash != o.GameplayHash || r.SlashCount != o.Ordinal || r.TargetSeat != o.TargetSeat || r.UseId != use.Id || r.ActionId != a.ActionId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenJiangWeiSlashIssuedEvent>().Count(e => e.Origin == o) != 1 || !HasShenJwPaidPingxiang(f))
            return false;
        return _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Source.OwnerSeat, o.Source.OwnerSeat);
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 38400)]
    internal bool CompleteShenJwAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ShenJiangWeiPingxiang: not null } use || !IsShenJwNativeSlash(use))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, CardKind.FireSlash));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(38400)]
    internal void ValidateShenJwSlash(CardUseFrame use)
    {
        if (use.ShenJiangWeiPingxiang is not null && (!IsShenJwNativeSlash(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Pingxiang requires its exact whole native FireSlash before returning.");
    }

    [NativeUseReturn(38400)]
    internal void ReturnShenJwSlash(CardUseFrame use)
    {
        if (use.ShenJiangWeiPingxiang is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.ShenJiangWei!;
        if (r.UseId != use.Id || r.ActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Pingxiang cannot repeat or skip a native use return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenJiangWei = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            CompleteShenJwSlash(o.FrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 38400)]
    internal bool ReturnShenJwAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenJiangWei: { Stage: ShenJiangWeiStage.NativeUse, NativeReturned: true } r } f || r.UseId != completion.ResolutionId)
            return false;
        if (completion.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Pingxiang's FireSlash returned to another parent.");
        CompleteShenJwSlash(f.Id);
        return true;
    }

    internal void CompleteShenJwSlash(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.ShenJiangWei!;
        if (!r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenJiangWeiSlashReturnedEvent>().Any(e => e.FrameId == id && e.Ordinal == r.SlashCount))
            throw new InvalidOperationException("Pingxiang cannot continue before its once-only native return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenJiangWeiSlashReturnedEvent(id, r.SlashCount, r.UseId, r.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenJiangWei = r with { Stage = ShenJiangWeiStage.SlashChildren } });
        ContinueShenJw(id);
    }
}

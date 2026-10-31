#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYongAnniversaryRuntime
{
    internal IReadOnlyList<AnniversaryLiuYongPlan> AnniversaryLiuYongPlans(int owner)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var result = new List<AnniversaryLiuYongPlan>();
        foreach (var kind in new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash
        }

        )
            foreach (var target in _coreTableOrderOperations.CurrentTableOrder().Select(seat => _coreMatchParticipantsPlayersAccess._players[seat]).Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, p, kind, physicalSuit: Suit.None, effectiveRank: 0, countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, Suit.None, null)))
                result.Add(new(kind, LegalActionKind.Slash) { Targets = [target.Seat] });
        var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, physicalSuit: Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []);
        result.AddRange(_coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(actor, options, Suit.None, null, Suit.None, false).Where(o => CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(o.EffectiveCardKind) && (CoreGameEngineMouLuMengRuntime.IsDamageCardKind(o.EffectiveCardKind) || CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(o.EffectiveCardKind))).Select(o => new AnniversaryLiuYongPlan(o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind, o.NextActualUseAdjusted) { Targets = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(o.EffectiveCardKind, owner, o.TargetSeats) }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void IssueAnniversaryLiuYongUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuYong!;
        var p = r.Plan!;
        if (r.Stage != AnniversaryLiuYongStage.IssuingUse || !r.GiftPaid || !AnniversaryLiuYongPlans(f.OwnerSeat).Any(candidate => candidate == p || (candidate.Kind, candidate.ActionKind, candidate.TargetCardId, candidate.RequiredKind, candidate.Adjusted) == (p.Kind, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Adjusted) && candidate.Targets.SequenceEqual(p.Targets)))
            throw new InvalidOperationException("Zhuning requires its current accepted native card plan after the real gift.");
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            IssueAnniversaryLiuYongSlash(f);
            return;
        }

        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        var origin = new ProgramVirtualOrdinaryTrickOrigin(f.Id, f.InstructionIndex, 0, f.OwnerSeat, f.OwnerSeat, f.OwnerSeat, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, r.Turn, p.Kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, p.Kind, Suit.None, 0);
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, p.Targets, p.Kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!;
        f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { AnniversaryLiuYong = f.AnniversaryLiuYong!with { Stage = AnniversaryLiuYongStage.NativeUse, UseId = useId, ActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongNativeIssuedEvent(f.Id, useId, use.Action!.ActionId, p.Kind));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, use.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal void IssueAnniversaryLiuYongSlash(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuYong!;
        var p = r.Plan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[p.Targets.Single()];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, f.OwnerSeat, f.OwnerSeat, null, null, null, p.Kind, p.Targets, [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var origin = new AnniversaryLiuYongSlashOrigin(f.Id, r.Source, r.Hash, id, action.ActionId, p.Kind, target.Seat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r with { Stage = AnniversaryLiuYongStage.NativeUse, UseId = id, ActionId = action.ActionId } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, f.OwnerSeat, 0, p.Kind, p.Targets, PhysicalCardIds: []) { Action = action, UnlimitedUse = true, AnniversaryLiuYongSlash = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongNativeIssuedEvent(f.Id, id, action.ActionId, p.Kind));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, p.Kind, f.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, p.Targets));
        _characterLuMengMouOperations.RecordYingboCardUse(id, f.OwnerSeat, p.Kind);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(f.OwnerSeat, p.Kind);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, f.OwnerSeat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, p.Kind), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, target.Seat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, p.Targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsAnniversaryLiuYongVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: AnniversaryLiuYongComposition.Zhuning } origin || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == origin.ParentProgramFrameId)is not ProgramSkillFrame { AnniversaryLiuYong: { GiftPaid: true, Plan: { } p } r } f || r.Stage is not (AnniversaryLiuYongStage.IssuingUse or AnniversaryLiuYongStage.NativeUse) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind != p.Kind || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(p.Kind) || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } a || a.ProviderSeat != f.OwnerSeat || a.ActionId != origin.CardActionId || a.EffectiveKind != p.Kind || a.EffectiveSuit is not (null or Suit.None) || a.EffectiveRank is not (null or 0) || a.EffectiveIsRed == true || !a.TargetSeats.SequenceEqual(use.TargetSeats) || origin.OwnerSeat != f.OwnerSeat || origin.InitialActorSeat != f.OwnerSeat || origin.InitialTargetSeat != f.OwnerSeat || origin.BindingId != f.ActivationId || origin.SkillInstanceId != f.SkillInstanceId || origin.GameplayHash != f.GameplayHash || origin.InstructionIndex != 1 || origin.LifecycleFrameId != 0 || origin.TurnNumber != r.Turn || r.UseId != 0 && (r.UseId != use.Id || r.ActionId != a.ActionId))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, f.OwnerSeat, f.OwnerSeat) : a.ActorSeat == f.OwnerSeat;
    }

    internal bool IsAnniversaryLiuYongSlashUse(CardUseFrame use) => use.AnniversaryLiuYongSlash is { } o && use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && use.UnlimitedUse && use.CardKind == o.Kind && CoreNativeCardClassificationsRuntime.IsSlashCard(o.Kind) && use.Action is { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a && a.ActionId == o.ActionId && a.EffectiveKind == o.Kind && a.ProviderSeat == o.Source.OwnerSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is ProgramSkillFrame { AnniversaryLiuYong: { Stage: AnniversaryLiuYongStage.NativeUse, GiftPaid: true } r } f && r.Source == o.Source && r.Hash == o.Hash && r.UseId == use.Id && r.ActionId == a.ActionId && r.Plan?.Kind == o.Kind && r.Plan.Targets.SequenceEqual([o.TargetSeat]) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYongNativeIssuedEvent>().Count(e => e == new AnniversaryLiuYongNativeIssuedEvent(f.Id, use.Id, a.ActionId, o.Kind)) == 1 && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, f.OwnerSeat, f.OwnerSeat);
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 43400)]
    internal bool CompleteAnniversaryLiuYongSlash(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { AnniversaryLiuYongSlash: not null } use || !IsAnniversaryLiuYongSlashUse(use))
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

    [NativeUseValidation(43400)]
    internal void ValidateAnniversaryLiuYongSlash(CardUseFrame use)
    {
        if (use.AnniversaryLiuYongSlash is not null && (!IsAnniversaryLiuYongSlashUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Zhuning must return its exact completed native Slash.");
    }

    [NativeUseReturn(43400)]
    internal void ReturnAnniversaryLiuYongSlash(CardUseFrame use)
    {
        if (use.AnniversaryLiuYongSlash is { } origin)
            ReturnAnniversaryLiuYongNative(use, origin.FrameId);
    }

    internal bool TryReturnAnniversaryLiuYongVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: AnniversaryLiuYongComposition.Zhuning } origin)
            return false;
        ReturnAnniversaryLiuYongNative(use, origin.ParentProgramFrameId);
        return true;
    }

    internal void ReturnAnniversaryLiuYongNative(CardUseFrame use, long parentId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(parentId);
        var r = f.AnniversaryLiuYong!;
        if (r.Stage != AnniversaryLiuYongStage.NativeUse || r.UseId != use.Id || r.ActionId != use.Action?.ActionId || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardKind == r.Plan!.Kind) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYongNativeReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Zhuning cannot return another whole use or return it twice.");
        var attack = use.CardAttack is { Active: true };
        var caused = r.CausedDamage || use.CausedDamage;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r with { Stage = AnniversaryLiuYongStage.NativeReturned, HadAttack = attack, CausedDamage = caused } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongNativeReturnedEvent(f.Id, use.Id, r.ActionId, attack, caused));
        if (!attack)
            CompleteAnniversaryLiuYongUse(f);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 43400)]
    internal bool ReturnAnniversaryLiuYongSlashAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuYong: { Stage: AnniversaryLiuYongStage.NativeReturned } r } f || r.UseId != completion.ResolutionId || completion.ProgramFrameId != f.Id)
            return false;
        CompleteAnniversaryLiuYongUse(f);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 43400)]
    internal bool ReturnAnniversaryLiuYongVirtualAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuYong: { Stage: AnniversaryLiuYongStage.NativeReturned, HadAttack: true } r } f)
            return false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Zhuning must wait for the true native attack cleanup.");
        CompleteAnniversaryLiuYongUse(f);
        return true;
    }
}

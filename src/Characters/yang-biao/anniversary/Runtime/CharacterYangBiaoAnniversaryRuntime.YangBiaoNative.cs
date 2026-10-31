#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangBiaoAnniversaryRuntime
{
    internal IReadOnlyList<AnniversaryYangBiaoPlan> YangBiaoPlans(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYangBiao!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!actor.IsAlive || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var plans = new List<AnniversaryYangBiaoPlan>();
        if (_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, target, CardKind.Slash, physicalSuit: Suit.None, effectiveRank: 0, ignoreDistance: true, countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.Slash, Suit.None, null))
            plans.Add(new(CardKind.Slash, LegalActionKind.Slash) { Targets = [target.Seat] });
        plans.AddRange(_coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, CardKind.Dismantlement, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).Where(o => o.TargetSeats.SequenceEqual([target.Seat])).Select(o => new AnniversaryYangBiaoPlan(o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind) { Targets = o.TargetSeats }));
        // Juee expressly names one recipient, rather than all Five Grains recipients.
        if (_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile) > 0 && _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, CardKind.FiveGrains, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).Any() && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, CardKind.FiveGrains) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.FiveGrains, Suit.None, null))
            plans.Add(new(CardKind.FiveGrains, LegalActionKind.FiveGrains) { Targets = [target.Seat] });
        return Array.AsReadOnly(plans.ToArray());
    }

    internal static bool YangBiaoPlansEqual(AnniversaryYangBiaoPlan a, AnniversaryYangBiaoPlan b) => (a.Kind, a.ActionKind, a.TargetCardId, a.RequiredKind) == (b.Kind, b.ActionKind, b.TargetCardId, b.RequiredKind) && a.Targets.SequenceEqual(b.Targets);
    internal void NextYangBiaoUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYangBiao!;
        if (r.Cursor >= r.Budget || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            CompleteYangBiao(f);
            return;
        }

        var plans = YangBiaoPlans(f);
        var kinds = plans.Select(p => p.Kind).Distinct().ToArray();
        if (kinds.Length == 0)
        {
            CompleteYangBiao(f);
            return;
        }

        var before = _randomOperations._random.State;
        var kind = kinds[_randomOperations._random.Next(kinds.Length)];
        var after = _randomOperations._random.State;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYangBiao = r with { Stage = AnniversaryYangBiaoStage.UsePlan, RandomKinds = kinds, RandomBefore = before, RandomAfter = after, RandomKind = kind } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYangBiaoRandomCardEvent(f.Id, r.Cursor, kind));
        var selected = plans.Where(p => p.Kind == kind).ToArray();
        if (selected is [var only])
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYangBiao = f.AnniversaryYangBiao!with { Plan = only, Stage = AnniversaryYangBiaoStage.IssuingUse } });
            IssueYangBiaoUse(f);
        }
        else
            PublishYangBiao(f);
    }

    internal void IssueYangBiaoUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYangBiao!;
        var p = r.Plan!;
        if (!ValidYangBiao(f) || r.Stage != AnniversaryYangBiaoStage.IssuingUse || !YangBiaoPlans(f).Any(plan => YangBiaoPlansEqual(plan, p)))
            throw new InvalidOperationException("Juee lost its frozen currently legal fixed-recipient native plan.");
        if (p.Kind == CardKind.Slash)
        {
            IssueYangBiaoSlash(f);
            return;
        }

        var origin = new ProgramVirtualOrdinaryTrickOrigin(f.Id, f.InstructionIndex, 0, f.OwnerSeat, f.OwnerSeat, f.OwnerSeat, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, r.Turn, p.Kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, p.Kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, p.Targets, p.Kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { AnniversaryYangBiao = f.AnniversaryYangBiao!with { Stage = AnniversaryYangBiaoStage.NativeUse, UseId = id, ActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYangBiaoNativeIssuedEvent(f.Id, r.Cursor, id, use.Action!.ActionId, p.Kind, r.Target));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, use.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal void IssueYangBiaoSlash(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYangBiao!;
        var p = r.Plan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, p.Kind, p.Targets, [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYangBiao = r with { Stage = AnniversaryYangBiaoStage.NativeUse, UseId = id, ActionId = action.ActionId }, VirtualBasicDraft = new(f.InstructionIndex, id) });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, p.Kind, p.Targets, PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(f.Id, f.InstructionIndex), UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYangBiaoNativeIssuedEvent(f.Id, r.Cursor, id, action.ActionId, p.Kind, r.Target));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, p.Kind, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, p.Targets));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, p.Kind);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, p.Kind);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, p.Kind));
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, target.Seat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, p.Targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }
    internal bool IsAnniversaryYangBiaoVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: AnniversaryYangBiaoComposition.Juee } o || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ParentProgramFrameId)is not ProgramSkillFrame { AnniversaryYangBiao: { Plan: { } p } r } f || !ValidYangBiao(f) || r.Stage is not (AnniversaryYangBiaoStage.IssuingUse or AnniversaryYangBiaoStage.NativeUse) || p.Kind is not (CardKind.Dismantlement or CardKind.FiveGrains) || use.CardKind != p.Kind || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } a || a.ActionId != o.CardActionId || a.ProviderSeat != f.OwnerSeat || a.EffectiveKind != p.Kind || a.EffectiveSuit is not (null or Suit.None) || a.EffectiveRank is not (null or 0) || a.EffectiveIsRed == true || !a.TargetSeats.SequenceEqual(use.TargetSeats) || o.OwnerSeat != f.OwnerSeat || o.InitialActorSeat != f.OwnerSeat || o.InitialTargetSeat != f.OwnerSeat || o.BindingId != f.ActivationId || o.SkillInstanceId != f.SkillInstanceId || o.GameplayHash != f.GameplayHash || o.InstructionIndex != f.InstructionIndex || o.LifecycleFrameId != 0 || o.TurnNumber != r.Turn || r.UseId != 0 && (r.UseId != use.Id || r.ActionId != a.ActionId))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, f.OwnerSeat, f.OwnerSeat) : a.ActorSeat == f.OwnerSeat;
    }

    internal bool IsAnniversaryYangBiaoFiveGrainsUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardKind: CardKind.FiveGrains } use && IsAnniversaryYangBiaoVirtualUse(use);
    internal bool TryReturnAnniversaryYangBiaoVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: AnniversaryYangBiaoComposition.Juee } o)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ParentProgramFrameId);
        var r = f.AnniversaryYangBiao!;
        if (use.Step != ResolutionFrameStep.Completed || r.UseId != use.Id || r.ActionId != use.Action?.ActionId)
            throw new InvalidOperationException("Juee cannot return an unfinished or different native ordinary trick.");
        CompleteYangBiaoNative(f, use.CardAttack is { Active: true });
        return true;
    }

    internal void CompleteYangBiaoNative(ProgramSkillFrame f, bool? hadAttack = null)
    {
        var r = f.AnniversaryYangBiao!;
        if (!ValidYangBiao(f) || r.Stage != AnniversaryYangBiaoStage.NativeUse || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == 0 && e.CardKind == r.Plan!.Kind) != 1)
            throw new InvalidOperationException("Juee must return the same whole native Use once before advancing its ordinal.");
        // A basic Slash returns after its CardUse frame has been popped, so
        // ActiveCardAttack can no longer project it. Its native attack cleanup
        // still follows; issuing the next Use here would lose its new prompt
        // to that old cleanup. The original exact Slash plan proves this return.
        var attack = hadAttack ?? r.Plan!.Kind == CardKind.Slash;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYangBiao = r with { Stage = AnniversaryYangBiaoStage.ReturnedUse, HadAttack = attack } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYangBiaoNativeReturnedEvent(f.Id, r.Cursor, r.UseId, attack));
        if (!attack)
            FinishYangBiaoUse(f);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 43700)]
    internal bool ReturnYangBiaoAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryYangBiao: { Stage: AnniversaryYangBiaoStage.ReturnedUse, HadAttack: true, Plan.Kind: CardKind.Slash } r } f || completion.ResolutionId != r.UseId)
            return false;
        FinishYangBiaoUse(f);
        return true;
    }

    internal void FinishYangBiaoUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYangBiao!;
        if (!ValidYangBiao(f) || r.Stage != AnniversaryYangBiaoStage.ReturnedUse)
            throw new InvalidOperationException("Juee lost the returned ordinal's native completion.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYangBiao = r with { Stage = AnniversaryYangBiaoStage.NextUse, Cursor = r.Cursor + 1, Plan = null, UseId = 0, ActionId = 0, RandomKind = null, RandomKinds = [], HadAttack = false } });
        ContinueYangBiao(f.Id);
    }
}

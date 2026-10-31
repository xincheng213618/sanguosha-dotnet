#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuLiAnniversaryRuntime
{
    internal void EnsureLiuLiDamagePool(int owner)
    {
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiPoolNameEvent>().Any(e => e.OwnerSeat == owner))
            return;
        var pool = new[]
        {
            CardKind.Slash,
            CardKind.Duel,
            CardKind.BarbarianAssault,
            CardKind.ArrowBarrage,
            CardKind.FireAttack
        }.Concat(_cardZonesOperations._cardZones.CreateDiagnostics().Select(c => c.CardKind).Where(k => k is CardKind.WumiaoFlood or CardKind.QizhengXiangsheng)).Distinct().ToArray();
        foreach (var name in pool)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiPoolNameEvent(owner, name));
    }

    internal bool LiuLiNameChosen(int owner, CardKind kind) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiNameChosenEvent>().Any(e => e.Source.OwnerSeat == owner && e.Name == CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind) && LiuLiChosenProof(e));
    internal IReadOnlyList<AnniversaryLiuLiPlan> LiuLiPlans(int owner)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        var pool = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiPoolNameEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.Name).ToHashSet();
        var result = new List<AnniversaryLiuLiPlan>();
        if (pool.Contains(CardKind.Slash) && !LiuLiNameChosen(owner, CardKind.Slash))
            foreach (var kind in new[]
            {
                CardKind.Slash,
                CardKind.FireSlash,
                CardKind.ThunderSlash
            }

            )
                foreach (var target in _coreTableOrderOperations.CurrentTableOrder().Select(seat => _coreMatchParticipantsPlayersAccess._players[seat]).Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, p, kind, physicalSuit: Suit.None, effectiveRank: 0, ignoreDistance: true, countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, Suit.None, null)))
                    result.Add(new(kind, LegalActionKind.Slash) { Targets = [target.Seat] });
        var ordinary = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, physicalSuit: Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []);
        result.AddRange(_coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(actor, ordinary, Suit.None, null, Suit.None, false).Where(o => pool.Contains(o.EffectiveCardKind) && !LiuLiNameChosen(owner, o.EffectiveCardKind)).Select(o => new AnniversaryLiuLiPlan(o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind, o.NextActualUseAdjusted) { Targets = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(o.EffectiveCardKind, owner, o.TargetSeats) }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void IssueLiuLiUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuLi!;
        var p = r.Plan!;
        if (!ValidLiuLi(f) || r.Stage != AnniversaryLiuLiStage.IssuingUse || !LiuLiPlans(f.OwnerSeat).Any(plan => LiuLiPlansEqual(plan, p)))
            throw new InvalidOperationException("Dehua lost its currently legal unused damage-name native plan.");
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            IssueLiuLiSlash(f);
            return;
        }

        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        var origin = new ProgramVirtualOrdinaryTrickOrigin(f.Id, f.InstructionIndex, 0, f.OwnerSeat, f.OwnerSeat, f.OwnerSeat, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, r.Turn, p.Kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, p.Kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, p.Targets, p.Kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { AnniversaryLiuLi = f.AnniversaryLiuLi!with { Stage = AnniversaryLiuLiStage.NativeUse, UseId = id, ActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiNativeIssuedEvent(f.Id, id, use.Action!.ActionId, p.Kind));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, use.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal static bool LiuLiPlansEqual(AnniversaryLiuLiPlan a, AnniversaryLiuLiPlan b) => (a.Kind, a.ActionKind, a.TargetCardId, a.RequiredKind, a.Adjusted) == (b.Kind, b.ActionKind, b.TargetCardId, b.RequiredKind, b.Adjusted) && a.Targets.SequenceEqual(b.Targets);
    internal void IssueLiuLiSlash(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuLi!;
        var p = r.Plan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[p.Targets.Single()];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, p.Kind, p.Targets, [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.NativeUse, UseId = id, ActionId = action.ActionId }, VirtualBasicDraft = new(f.InstructionIndex, id) });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, p.Kind, p.Targets, PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(f.Id, f.InstructionIndex), UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiNativeIssuedEvent(f.Id, id, action.ActionId, p.Kind));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, p.Kind, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, p.Targets));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, p.Kind);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, p.Kind);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, p.Kind));
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, target.Seat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, p.Targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }
    internal bool IsAnniversaryLiuLiVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: AnniversaryLiuLiComposition.Dehua } o || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ParentProgramFrameId)is not ProgramSkillFrame { AnniversaryLiuLi: { Plan: { } p } r } f || !ValidLiuLi(f) || r.Stage is not (AnniversaryLiuLiStage.IssuingUse or AnniversaryLiuLiStage.NativeUse) || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(p.Kind) || use.CardKind != p.Kind || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } a || a.ActionId != o.CardActionId || a.ProviderSeat != f.OwnerSeat || a.EffectiveKind != p.Kind || a.EffectiveSuit is not (null or Suit.None) || a.EffectiveRank is not (null or 0) || a.EffectiveIsRed == true || !a.TargetSeats.SequenceEqual(use.TargetSeats) || o.OwnerSeat != f.OwnerSeat || o.InitialActorSeat != f.OwnerSeat || o.InitialTargetSeat != f.OwnerSeat || o.BindingId != f.ActivationId || o.SkillInstanceId != f.SkillInstanceId || o.GameplayHash != f.GameplayHash || o.InstructionIndex != f.InstructionIndex || o.LifecycleFrameId != 0 || o.TurnNumber != r.Turn || r.UseId != 0 && (r.UseId != use.Id || r.ActionId != a.ActionId))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, f.OwnerSeat, f.OwnerSeat) : a.ActorSeat == f.OwnerSeat;
    }

    internal bool TryReturnAnniversaryLiuLiVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: AnniversaryLiuLiComposition.Dehua } o)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ParentProgramFrameId);
        var r = f.AnniversaryLiuLi!;
        if (use.Step != ResolutionFrameStep.Completed || r.UseId != use.Id || r.ActionId != use.Action?.ActionId)
            throw new InvalidOperationException("Dehua cannot return another unfinished native ordinary trick.");
        CompleteLiuLiNative(f, use.CardAttack is { Active: true });
        return true;
    }

    internal void CompleteLiuLiNative(ProgramSkillFrame f, bool? hadAttack = null)
    {
        var r = f.AnniversaryLiuLi!;
        if (!ValidLiuLi(f) || r.Stage != AnniversaryLiuLiStage.NativeUse || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == 0 && e.CardKind == r.Plan!.Kind) != 1)
            throw new InvalidOperationException("Dehua must finish its same whole zero-entity native Use exactly once.");
        var attack = hadAttack ?? _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } a && a.ResolutionId == r.UseId;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.ReturnedUse, HadAttack = attack } });
        if (!attack)
            SettleLiuLiName(f);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 43300)]
    internal bool ReturnLiuLiAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuLi: { Stage: AnniversaryLiuLiStage.ReturnedUse, HadAttack: true } } f)
            return false;
        SettleLiuLiName(f);
        return true;
    }

    internal void SettleLiuLiName(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuLi!;
        var name = CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(r.Plan!.Kind);
        if (!ValidLiuLi(f) || r.Stage != AnniversaryLiuLiStage.ReturnedUse || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiNameChosenEvent>().Any(e => e.FrameId == f.Id) || LiuLiNameChosen(f.OwnerSeat, name))
            throw new InvalidOperationException("Dehua cannot settle a name twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiNameChosenEvent(f.Id, r.Source, r.Hash, name, r.UseId, r.ActionId));
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiPoolNameEvent>().Where(e => e.OwnerSeat == f.OwnerSeat).All(e => LiuLiNameChosen(f.OwnerSeat, e.Name)))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.SkillChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiDehuaCompletedEvent(f.Id, f.OwnerSeat, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiPoolNameEvent>().Count(e => e.OwnerSeat == f.OwnerSeat)));
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == AnniversaryLiuLiComposition.Dehua).ToArray())
                _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CharacterSkillsLostEvent(f.OwnerSeat, f.OwnerSeat, Array.AsReadOnly(new[] { AnniversaryLiuLiComposition.Dehua })));
            ContinueLiuLi(f.Id);
            return;
        }

        FinishLiuLi(f);
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoYouZhiMouMobileRuntime
{
    internal bool IsMouGyzExactCardQuery(CharacterState actor, int? selectedId)
    {
        if (IsMouGyzDeclarationQuery(actor, selectedId))
            return true;
        if (selectedId is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGuoYouZhi: { Stage: MouGuoYouZhiStage.UseChoice or MouGuoYouZhiStage.IssuingUse } r } f || r.ActorSeat != actor.Seat || !actor.IsAlive || id != r.CardId || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor.Seat) || !ValidMouGyz(f))
            return false;
        return r.Stage == MouGuoYouZhiStage.UseChoice || r.Native is { UseId: 0, ActionId: 0 } n && ValidMouGyzNative(n, f, false);
    }

    internal IReadOnlyList<LegalAction> MouGyzNativeActions(ProgramSkillFrame f)
    {
        if (f.MouGuoYouZhi is not { Stage: MouGuoYouZhiStage.UseChoice } r || !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(r.CardId) != CardLocation.Hand(r.ActorSeat))
            return[];
        return Array.AsReadOnly(_coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], false, r.CardId).Where(a => a.CardId == r.CardId && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.ConversionSource is null || _corePlayCardHistoryOperations.ViewAsRule(a.ConversionSource)is not { } rule || rule.InputCount == 1) && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill or LegalActionKind.RevealGeneral)).Select(a => a with { TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()), AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()), SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()), SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()), SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()), SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(t => (IReadOnlyList<int>)Array.AsReadOnly(t.ToArray())).ToArray()) }).ToArray());
    }

    internal void IssueMouGyzNativeUse(ProgramSkillFrame f, int index)
    {
        var r = f.MouGuoYouZhi!;
        var action = MouGyzNativeActions(f)[index];
        var receipt = new MouGuoYouZhiNativeReceipt(f.Id, r.Source, r.GameplayHash, f.InstructionIndex, r.ActorSeat, r.CardId, r.ActualTurn, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action, 0, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.IssuingUse, Native = receipt }, PendingMovementContinuation = null });
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], action);
    }

    internal bool SameMouGyzNative(MouGuoYouZhiNativeReceipt a, MouGuoYouZhiNativeReceipt b) => (a with
    {
        Action = b.Action
    }

    ) == b && a.Action.Kind == b.Action.Kind && a.Action.CardId == b.Action.CardId && a.Action.PlayedCardKind == b.Action.PlayedCardKind && a.Action.TargetCardId == b.Action.TargetCardId && a.Action.ConversionSource == b.Action.ConversionSource && a.Action.TargetSeats.SequenceEqual(b.Action.TargetSeats) && (a.Action.AdditionalConversionSources ?? []).SequenceEqual(b.Action.AdditionalConversionSources ?? []);
    internal bool ValidMouGyzNative(MouGuoYouZhiNativeReceipt n, ProgramSkillFrame f, bool bound) => f.MouGuoYouZhi is { Paid: true } r && r.Op == SkillProgramEffectOp.ResolveMouGuoYouZhiZhongchun && n.ProgramFrameId == f.Id && n.Source == r.Source && n.GameplayHash == f.GameplayHash && n.InstructionIndex == f.InstructionIndex && n.ActorSeat == r.ActorSeat && n.CardId == r.CardId && n.ActualTurn == r.ActualTurn && n.Action.CardId == n.CardId && ValidMouGyz(f) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiMovementEvent>().Count(e => e.FrameId == f.Id && e.CardId == n.CardId && e.From == CardLocation.DiscardPile && e.To == CardLocation.Hand(n.ActorSeat) && e.Reason.Value == MouGyzObtainReason) == 1 && (!bound || n.UseId > 0 && n.ActionId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiNativeIssuedEvent>().Count(e => e == new MouGuoYouZhiNativeIssuedEvent(f.Id, n.UseId, n.ActionId, n.ActorSeat, n.CardId)) == 1);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 32401)]
    internal void BindMouGyzNativeUse(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent declared || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { } use || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { MouGuoYouZhi: { Stage: MouGuoYouZhiStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } parent)
            return;
        var a = use.Action ?? throw new InvalidOperationException("Zhongchun native Use lost its actual action.");
        if (!ValidMouGyzNative(n, parent, false) || use.SourceSeat != n.ActorSeat || use.CardId != n.CardId || use.CardKind != (n.Action.PlayedCardKind ?? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[n.ActorSeat], _coreCardAttackStateOperations.GetAttackCard(n.CardId), n.Action.ConversionSource).Kind) || use.PhysicalCardIds is not [var id] || id != n.CardId || a.Type != CardActionType.Use || a.ActorSeat != n.ActorSeat || a.ProviderSeat != n.ActorSeat || a.RequesterSeat is not null || a.PhysicalCards is not [var cost] || cost.CardId != n.CardId || !a.ConversionChain.SequenceEqual((n.Action.ConversionSource is { } source ? new[] { source } : Array.Empty<CardConversionSource>()).Concat(n.Action.AdditionalConversionSources ?? [])) || !use.TargetSeats.SequenceEqual(MouGyzExpectedNativeTargets(n)))
            throw new InvalidOperationException("Zhongchun changed its original obtained entity, real actor/provider or target plan.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouGuoYouZhiUse = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiNativeIssuedEvent(parent.Id, use.Id, a.ActionId, n.ActorSeat, n.CardId));
    }

    internal IReadOnlyList<int> MouGyzExpectedNativeTargets(MouGuoYouZhiNativeReceipt r)
    {
        var a = r.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreCardAttackStateOperations.GetAttackCard(r.CardId), a.ConversionSource);
        if (a.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)is { SingleCardTrickUse: true } rule)
            return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, rule.OutputKind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _coreCardAttackStateOperations.GetAttackCard(r.CardId)), rule.ExcludeOwnerEffects, enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), hasActualColor: true, physicalCardIds: [r.CardId]).Where(o => o.ActionKind == a.Kind && o.TargetCardId == a.TargetCardId && (o.TargetSeats.SequenceEqual(a.TargetSeats) || a.ProgramActivationId == "red-additional-targets" && o.TargetSeats.SequenceEqual(a.TargetSeats.Take(o.TargetSeats.Count)))).OrderByDescending(o => o.TargetSeats.Count).Select(o => o.TargetSeats).First();
        if (_coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, a)is { } guandu)
            return guandu;
        if (_coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, a)is { } boat)
            return boat;
        if (a.Kind == LegalActionKind.Peach)
            return[actor.Seat];
        var effective = a.PlayedCardKind ?? card.Kind;
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(effective, actor.Seat, a.TargetSeats);
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(i => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, i)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, effective)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, effective, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return seats;
    }

    internal bool MouGyzNativeRidesOn(CardUseFrame use, ProgramSkillFrame parent) => use.MouGuoYouZhiUse is { } n && parent.MouGuoYouZhi is { Native: { } pending } && SameMouGyzNative(n, pending) && ValidMouGyzNative(n, parent, true) && use.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ActorSeat == n.ActorSeat && a.ProviderSeat == n.ActorSeat && a.PhysicalCards is [var cost] && cost.CardId == n.CardId;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 32401)]
    internal bool IsMouGyzNonPlayNative(long id, int actor) => (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouGuoYouZhiUse: { } n } use && n.ActorSeat == actor && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ProgramFrameId)is ProgramSkillFrame parent && MouGyzNativeRidesOn(use, parent);
    [NativeUseValidation(32401)]
    internal void AssertMouGyzNativeCompletion(CardUseFrame use)
    {
        if (use.MouGuoYouZhiUse is { } n && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ProgramFrameId)is not ProgramSkillFrame parent || !MouGyzNativeRidesOn(use, parent)))
            throw new InvalidOperationException("Zhongchun must complete its exact issued native Use.");
    }

    [NativeUseReturn(32401)]
    internal bool ReturnMouGyzNativeUse(CardUseFrame use)
    {
        if (use.MouGuoYouZhiUse is not { } n)
            return false;
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == use.Id) || use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ProgramFrameId)is not ProgramSkillFrame parent || !MouGyzNativeRidesOn(use, parent) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiNativeReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Zhongchun cannot return or pay its native entity twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouGuoYouZhi = parent.MouGuoYouZhi!with { Stage = MouGuoYouZhiStage.Returned } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiNativeReturnedEvent(parent.Id, use.Id, n.ActionId, n.ActorSeat, n.CardId));
        if (use.CardAttack is not { Active: true })
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 32401)]
    internal bool ResumeMouGyzAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGuoYouZhi: { Stage: MouGuoYouZhiStage.Returned, Native: not null } } f)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 32401)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 32401)]
    internal bool MouGyzTargetWindows(ProgramTargetCardWindowQuery q) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouGuoYouZhiZhongchun);
}

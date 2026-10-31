#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiaXuMouRuntime
{
    internal bool IsMouJxExactCardQuery(CharacterState actor, int? selected)
    {
        if (selected is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiaXu: { Stage: MouJiaXuStage.Use or MouJiaXuStage.IssuingUse, Yin: true } r } f || r.Holder != actor.Seat || !actor.IsAlive || !r.Remaining.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor.Seat) || !MouJiaXu.Valid(f))
            return false;
        return r.Stage == MouJiaXuStage.Use || r.Native is { UseId: 0, ActionId: 0 } n && n.FrameId == f.Id && n.Actor == actor.Seat && n.CardId == id;
    }

    internal IReadOnlyList<LegalAction> MouJxUseOptions(ProgramSkillFrame f)
    {
        if (f.MouJiaXu is not { Stage: MouJiaXuStage.Use, Yin: true } r || !_coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        return Array.AsReadOnly(r.Remaining.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(r.Holder)).SelectMany(id => _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[r.Holder], includeProgramActions: false, selectedPhysicalCardId: id)).Where(a => a.CardId is { } id && r.Remaining.Contains(id) && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.ConversionSource is null || _corePlayCardHistoryOperations.ViewAsRule(a.ConversionSource)is not { } rule || rule.InputCount == 1) && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral)).Select(a => a with { TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()), AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()), SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()), SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()), SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()), SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(t => (IReadOnlyList<int>)Array.AsReadOnly(t.ToArray())).ToArray()) }).ToArray());
    }

    internal IReadOnlyList<int> MouJxExpectedTargets(MouJiaXuNativeUse n)
    {
        var a = n.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.Actor];
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreCardAttackStateOperations.GetAttackCard(n.CardId), a.ConversionSource);
        if (a.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)is { SingleCardTrickUse: true } rule)
            return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, rule.OutputKind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _coreCardAttackStateOperations.GetAttackCard(n.CardId)), rule.ExcludeOwnerEffects, enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), hasActualColor: true, physicalCardIds: [n.CardId]).Where(o => o.ActionKind == a.Kind && o.TargetCardId == a.TargetCardId && (o.TargetSeats.SequenceEqual(a.TargetSeats) || a.ProgramActivationId == "red-additional-targets" && o.TargetSeats.SequenceEqual(a.TargetSeats.Take(o.TargetSeats.Count)))).OrderByDescending(o => o.TargetSeats.Count).Select(o => o.TargetSeats).First();
        if (_coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, a)is { } guandu)
            return guandu;
        if (_coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, a)is { } dragon)
            return dragon;
        if (a.Kind == LegalActionKind.Peach)
            return[actor.Seat];
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(n.Kind, n.Actor, a.TargetSeats);
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(n.Actor, offset)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, n.Kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, n.Kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.Actor, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(n.Actor, p.Seat, n.Kind))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    internal void IssueMouJxUse(ProgramSkillFrame f)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { Operation: SkillProgramEffectOp.ResolveMouJiaXuFumou, Yin: true, Stage: MouJiaXuStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r || n.Actor != r.Holder || !r.Remaining.Contains(n.CardId))
            throw new InvalidOperationException("Fumou requires its own issued displayed Hand Use.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiaXu = r with { Stage = MouJiaXuStage.Use } });
        var canonical = MouJxUseOptions(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)).SingleOrDefault(a => a.Kind == n.Action.Kind && a.CardId == n.CardId && a.PlayedCardKind == n.Action.PlayedCardKind && a.TargetCardId == n.Action.TargetCardId && a.ConversionSource == n.Action.ConversionSource && a.TargetSeats.SequenceEqual(n.Action.TargetSeats) && (a.AdditionalConversionSources ?? []).SequenceEqual(n.Action.AdditionalConversionSources ?? []));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f);
        if (canonical is null)
            throw new InvalidOperationException("Fumou's actual displayed Hand material or native legal Use expired.");
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[n.Actor], canonical);
    }

    internal void BindMouJxNativeUse(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { MouJiaXu: { Stage: MouJiaXuStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        var targets = MouJxExpectedTargets(n);
        if (!MouJiaXu.Valid(f) || !r.Yin || n.Actor != r.Holder || !r.Remaining.Contains(n.CardId) || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != n.Actor || a.ProviderSeat != n.Actor || a.RequesterSeat is not null || use.SourceSeat != n.Actor || use.CardId != n.CardId || use.CardKind != n.Kind || a.PhysicalCards is not [{ CardId: var id, From: var from }] || id != n.CardId || from != CardLocation.Hand(n.Actor) || use.PhysicalCardIds is not [var physical] || physical != n.CardId || !a.ConversionChain.SequenceEqual((n.Action.ConversionSource is { } primary ? new[] { primary } : Array.Empty<CardConversionSource>()).Concat(n.Action.AdditionalConversionSources ?? [])) || !use.TargetSeats.SequenceEqual(targets))
            throw new InvalidOperationException("Fumou changed the displayed entity, actual actor/provider, accepted conversion or canonical native targets.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId,
            Targets = targets
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiaXu = r with { Stage = MouJiaXuStage.NativeUse, Native = bound } });
        var policy = use.IssuedNoResponse ?? new IssuedCardNoResponse(a.ActionId, use.Id, n.Source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouJiaXuFumou = bound, UnlimitedUse = true, IssuedNoResponse = policy });
        if (use.IssuedNoResponse is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(policy));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiaXuUseIssuedEvent(f.Id, use.Id, a.ActionId, n.Actor, n.CardId));
    }

    internal bool MouJxNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => MouJiaXu.Valid(f) && u.MouJiaXuFumou is { } n && f.MouJiaXu is { Operation: SkillProgramEffectOp.ResolveMouJiaXuFumou, Yin: true, Stage: MouJiaXuStage.NativeUse, Native: { } p } r && n.Actor == r.Holder && r.Remaining.Contains(n.CardId) && MouJiaXuRuntime.SameUse(n, p) && n.FrameId == f.Id && n.Instruction == f.InstructionIndex && n.Source == f.MouJiaXu.Source && n.GameplayHash == f.GameplayHash && n.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && u.Id == n.UseId && u.SourceSeat == n.Actor && u.CardId == n.CardId && u.CardKind == n.Kind && u.TargetSeats.SequenceEqual(n.Targets) && u.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ActorSeat == n.Actor && a.ProviderSeat == n.Actor && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == n.CardId && from == CardLocation.Hand(n.Actor) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiaXuUseIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == u.Id && e.ActionId == a.ActionId && e.Actor == n.Actor && e.CardId == n.CardId) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiaXuShownEvent>().Count(e => e.FrameId == f.Id && e.Holder == n.Actor && e.CardId == n.CardId) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 36500)]
    internal bool MouJxOffTurnSlash(long id, int actor) => actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJiaXuFumou: { } n } u && n.Actor == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } parent && MouJxNativeRidesOn(u, parent);
    [NativeUseValidation(36500)]
    internal void AssertMouJxNativeUse(CardUseFrame u)
    {
        if (u.MouJiaXuFumou is { } n && (u.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != u.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || !MouJxNativeRidesOn(u, f)))
            throw new InvalidOperationException("Fumou must retire its exact issued native child after all effects.");
    }

    [NativeUseReturn(36500)]
    internal bool ReturnMouJxNativeUse(CardUseFrame u)
    {
        if (u.MouJiaXuFumou is not { } n)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || !MouJxNativeRidesOn(u, f) || u.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiaXuUseReturnedEvent>().Any(e => e.UseId == u.Id))
            throw new InvalidOperationException("Fumou cannot return another use or return its displayed entity twice.");
        MouJiaXu.Returned(n, u.CardAttack is { Active: true });
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 36500)]
    internal bool ResumeMouJxNativeUse()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiaXu: { Stage: MouJiaXuStage.ReturnedUse } } f)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}

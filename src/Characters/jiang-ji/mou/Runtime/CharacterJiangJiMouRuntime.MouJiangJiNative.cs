#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangJiMouRuntime
{
    internal bool IsMouJjExactCardQuery(CharacterState actor, int? selected)
    {
        if (selected is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiangJi: { Stage: MouJiangJiStage.Use or MouJiangJiStage.IssuingUse, Card: { } card } r } f || f.OwnerSeat != actor.Seat || !actor.IsAlive || card.Id != id || !r.Delivered || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor.Seat) || !MouJiangJi.Valid(f))
            return false;
        return r.Stage == MouJiangJiStage.Use || r.Native is { UseId: 0, ActionId: 0 } n && n.FrameId == f.Id && n.Actor == actor.Seat && n.CardId == id;
    }

    internal IReadOnlyList<LegalAction> MouJjUseOptions(ProgramSkillFrame f)
    {
        if (f.MouJiangJi is not { Stage: MouJiangJiStage.Use, Card: { } card, Delivered: true } || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(f.OwnerSeat))
            return[];
        return Array.AsReadOnly(_coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], includeProgramActions: false, selectedPhysicalCardId: card.Id).Where(a => a.CardId == card.Id && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.ConversionSource is null || _corePlayCardHistoryOperations.ViewAsRule(a.ConversionSource)is not { } rule || rule.InputCount == 1) && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral)).Select(a => a with { TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()), AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()), SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()), SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()), SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()), SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(t => (IReadOnlyList<int>)Array.AsReadOnly(t.ToArray())).ToArray()) }).ToArray());
    }

    internal IReadOnlyList<int> MouJjExpectedTargets(MouJiangJiNativeUse n)
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
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionOptionsPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(n.Actor, offset)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, n.Kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, n.Kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.Actor, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(n.Actor, p.Seat, n.Kind))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    internal void IssueMouJjUse(ProgramSkillFrame f)
    {
        if (!MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r)
            throw new InvalidOperationException("Shiju requires its own issued single gift Use.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiangJi = r with { Stage = MouJiangJiStage.Use } });
        var canonical = MouJjUseOptions(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)).SingleOrDefault(a => a.Kind == n.Action.Kind && a.CardId == n.CardId && a.PlayedCardKind == n.Action.PlayedCardKind && a.TargetCardId == n.Action.TargetCardId && a.ConversionSource == n.Action.ConversionSource && a.TargetSeats.SequenceEqual(n.Action.TargetSeats) && (a.AdditionalConversionSources ?? []).SequenceEqual(n.Action.AdditionalConversionSources ?? []));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f);
        if (canonical is null)
            throw new InvalidOperationException("Shiju's actual gifted Hand material or native legal Use expired.");
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[n.Actor], canonical);
    }

    internal void BindMouJjNativeUse(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { MouJiangJi: { Stage: MouJiangJiStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        var targets = MouJjExpectedTargets(n);
        if (!MouJiangJi.Valid(f) || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != n.Actor || a.ProviderSeat != n.Actor || a.RequesterSeat is not null || use.SourceSeat != n.Actor || use.CardId != n.CardId || use.CardKind != n.Kind || a.PhysicalCards is not [{ CardId: var id, From: var from }] || id != n.CardId || from != CardLocation.Hand(n.Actor) || use.PhysicalCardIds is not [var physical] || physical != n.CardId || !a.ConversionChain.SequenceEqual((n.Action.ConversionSource is { } primary ? new[] { primary } : Array.Empty<CardConversionSource>()).Concat(n.Action.AdditionalConversionSources ?? [])) || !use.TargetSeats.SequenceEqual(targets))
            throw new InvalidOperationException("Shiju changed the gifted entity, actual actor/provider, accepted conversion or canonical native targets.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId,
            Targets = targets
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiangJi = r with { Stage = MouJiangJiStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouJiangJiShiju = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangJiUseIssuedEvent(f.Id, use.Id, a.ActionId, n.CardId));
    }

    internal bool MouJjNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => MouJiangJi.Valid(f) && u.MouJiangJiShiju is { } n && f.MouJiangJi is { Stage: MouJiangJiStage.NativeUse, Native: { } p } && MouJiangJiRuntime.SameUse(n, p) && n.FrameId == f.Id && n.Instruction == f.InstructionIndex && n.Source == f.MouJiangJi.Source && n.GameplayHash == f.GameplayHash && n.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && u.Id == n.UseId && u.SourceSeat == n.Actor && u.CardId == n.CardId && u.CardKind == n.Kind && u.TargetSeats.SequenceEqual(n.Targets) && u.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ActorSeat == n.Actor && a.ProviderSeat == n.Actor && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == n.CardId && from == CardLocation.Hand(n.Actor) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangJiUseIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == u.Id && e.ActionId == a.ActionId && e.CardId == n.CardId) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 36300)]
    internal bool MouJjOffTurnSlash(long id, int actor) => actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJiangJiShiju: { } n } u && n.Actor == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } parent && MouJjNativeRidesOn(u, parent);
    [NativeUseValidation(36300)]
    internal void AssertMouJjNativeUse(CardUseFrame u)
    {
        if (u.MouJiangJiShiju is { } n && (u.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != u.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || !MouJjNativeRidesOn(u, f)))
            throw new InvalidOperationException("Shiju must retire its exact issued native child after all effects.");
    }

    [NativeUseReturn(36300)]
    internal bool ReturnMouJjNativeUse(CardUseFrame u)
    {
        if (u.MouJiangJiShiju is not { } n)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || !MouJjNativeRidesOn(u, f) || u.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangJiUseReturnedEvent>().Any(e => e.UseId == u.Id))
            throw new InvalidOperationException("Shiju cannot return another use or return its actual gift twice.");
        MouJiangJi.Returned(n, u.CardAttack is { Active: true });
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 36300)]
    internal bool ResumeMouJjNativeUse()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiangJi: { Stage: MouJiangJiStage.ReturnedUse } } f)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}

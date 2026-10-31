#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJuShouMouRuntime
{
    internal ProgramSkillFrame? MouJsUseQuery(CharacterState actor, int? selected = null)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJuShou: { Stage: MouJuShouStage.Use or MouJuShouStage.IssuingUse } r } f || r.Actor != actor.Seat || !actor.IsAlive || !MouJuShou.Valid(f) || selected is { } id && (!r.Bound.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor.Seat)))
            return null;
        return f;
    }

    internal bool IsMouJsExactCardQuery(CharacterState actor, int? selected) => MouJsUseQuery(actor, selected)is not null;
    internal IReadOnlyList<MouJsPlan> MouJsPlans(ProgramSkillFrame f)
    {
        if (f.MouJuShou is not { Stage: MouJuShouStage.Use } r || MouJsUseQuery(_coreMatchParticipantsPlayersAccess._players[r.Actor])?.Id != f.Id || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var bound = r.Bound.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(r.Actor)).ToHashSet();
        var plans = new List<MouJsPlan>();
        foreach (var id in bound)
            foreach (var a in _coreNativeLegalActionsOperations.BuildLegalActions(actor, includeProgramActions: false, selectedPhysicalCardId: id).Where(a => a.CardId == id && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.ConversionSource is null || _corePlayCardHistoryOperations.ViewAsRule(a.ConversionSource)is not { } rule || rule.InputCount == 1) && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral)))
                plans.Add(new("native", FreezeMouJsAction(a), a.PlayedCardKind ?? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), a.ConversionSource))) { Cards = [id], Targets = a.TargetSeats });
        if (_coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(actor))
            foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Where(pair => pair.All(c => bound.Contains(c.Id))))
                foreach (var kind in new[]
                {
                    CardKind.Slash
                }

                )
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, pair), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, pair), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, pair), pair.Select(c => c.Id).ToArray())))
                    {
                        var ids = pair.Select(c => c.Id).ToArray();
                        var a = new LegalAction(LegalActionKind.UseEquipmentEffect, null, target.Seat, "使用丈八蛇矛对" + target.Name + "使用杀", PlayedCardKind: kind, MinCardCount: 2, MaxCardCount: 2, MinTargetCount: 1, MaxTargetCount: 1, EquipmentKind: CardKind.ZhangbaSerpentSpear)
                        {
                            SelectableCardIds = ids,
                            SelectableTargetSeats = [target.Seat]
                        };
                        plans.Add(new("zhangba", FreezeMouJsAction(a), kind) { Cards = ids, Targets = [target.Seat] });
                    }

        foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Concat(new[] { CardKind.Peach, CardKind.Alcohol }))
            foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, kind, false).Where(p => p.Cards.Count > 1 && p.Cards.All(c => bound.Contains(c.Id))))
            {
                var ids = selection.Cards.Select(c => c.Id).ToArray();
                var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, selection.Cards);
                var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, selection.Cards);
                IEnumerable<IReadOnlyList<int>> targets = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, suit, color, physicalCardIds: ids)).Select(t => (IReadOnlyList<int>)new[] { t.Seat }) : kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, suit) ? new IReadOnlyList<int>[]
                {
                    new[]
                    {
                        actor.Seat
                    }
                }

                : kind == CardKind.Alcohol && _coreRoundDistinctBasicPlayOperations.CanUseRoundDistinctBasicAlcohol(actor, selection.Cards) ? new IReadOnlyList<int>[]
                {
                    Array.Empty<int>()
                }

                : Array.Empty<IReadOnlyList<int>>();
                foreach (var t in targets)
                {
                    var a = new LegalAction(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? LegalActionKind.Slash : kind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, ids[0], t.Count > 0 ? t[0] : null, "将本次所得牌当【" + CardCatalog.Get(kind).DisplayName + "】使用", PlayedCardKind: kind)
                    {
                        ConversionSource = selection.Source,
                        TargetSeats = t
                    };
                    plans.Add(new("multi", FreezeMouJsAction(a), kind) { Cards = ids, Targets = t });
                }
            }

        return Array.AsReadOnly(plans.ToArray());
    }

    internal static LegalAction FreezeMouJsAction(LegalAction a) => a with
    {
        TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()),
        AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()),
        SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()),
        SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()),
        SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()),
        SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(t => (IReadOnlyList<int>)Array.AsReadOnly(t.ToArray())).ToArray())
    };
    internal IReadOnlyList<int> MouJsExpectedTargets(MouJsNativeUse n)
    {
        if (n.Plan.Form != "native")
            return n.Plan.Targets;
        var a = n.Plan.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.Actor];
        var id = n.Plan.Cards.Single();
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreCardAttackStateOperations.GetAttackCard(id), a.ConversionSource);
        if (a.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)is { SingleCardTrickUse: true } rule)
            return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, rule.OutputKind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _coreCardAttackStateOperations.GetAttackCard(id)), rule.ExcludeOwnerEffects, enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), actualEffectiveColor: global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), hasActualColor: true, physicalCardIds: [id]).Where(o => o.ActionKind == a.Kind && o.TargetCardId == a.TargetCardId && (o.TargetSeats.SequenceEqual(a.TargetSeats) || a.ProgramActivationId == "red-additional-targets" && o.TargetSeats.SequenceEqual(a.TargetSeats.Take(o.TargetSeats.Count)))).OrderByDescending(o => o.TargetSeats.Count).Select(o => o.TargetSeats).First();
        if (_coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, a)is { } guandu)
            return guandu;
        if (_coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, a)is { } dragon)
            return dragon;
        if (a.Kind == LegalActionKind.Peach)
            return[actor.Seat];
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(n.Plan.Kind, n.Actor, a.TargetSeats);
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(i => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(n.Actor, i)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, n.Plan.Kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, n.Plan.Kind, card.Suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(n.Actor, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(n.Actor, p.Seat, n.Plan.Kind))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    internal void IssueMouJsUse(ProgramSkillFrame f)
    {
        if (!MouJuShou.Valid(f) || f.MouJuShou is not { Stage: MouJuShouStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r)
            throw new InvalidOperationException("Zuo Jun requires his exact accepted native Use intent.");
        var plan = n.Plan;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.Actor];
        if (!r.DeclarationPassed)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJuShou = r with { Stage = MouJuShouStage.Use } });
            var legal = MouJsPlans(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)).Any(p => MouJuShouRuntime.SamePlan(p, plan));
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f);
            if (!legal)
                throw new InvalidOperationException("Zuo Jun's native material or accepted targets expired.");
        }

        if (plan.Form == "native" && plan.Action.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null && !r.DeclarationPassed)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { MouJuShou = r with { Stage = MouJuShouStage.Declaration } });
            if (!_coreCardDeclarationsOperations.TryBeginCardDeclaration(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(plan.Cards.Single()), plan.Kind, source, _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.MouJuShouUse, n.Actor, action: plan.Action, finalKind: plan.Kind), plan.Targets))
                throw new InvalidOperationException("Zuo Jun cannot bypass an actual declaration challenge.");
            return;
        }

        if (plan.Form == "native")
        {
            _coreNativeActionDispatchOperations.ExecuteAction(actor, plan.Action);
            return;
        }

        var cards = plan.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        if (plan.Form == "zhangba")
        {
            _coreNativeSlashUseOperations.ResolveZhangbaSlash(actor, _coreMatchParticipantsPlayersAccess._players[plan.Targets.Single()], cards);
            return;
        }

        var conversion = plan.Action.ConversionSource ?? throw new InvalidOperationException("A multi-material Zuo Jun Use lost its native source.");
        if (_coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, plan.Cards, plan.Kind, false, conversion)is null)
            throw new InvalidOperationException("Zuo Jun's native multi-material conversion expired.");
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(plan.Kind))
        {
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[plan.Targets.Single()], cards[0], plan.Kind, actor.Seat, physicalCards: cards, conversionSource: conversion);
            return;
        }

        if (plan.Kind == CardKind.Peach)
        {
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, cards[0], "桃", CardKind.Peach, conversionSource: conversion, physicalCards: cards);
            return;
        }

        var useId = _coreNativeCardUseOperations.BeginCardUse(cards[0], actor.Seat, [], CardKind.Alcohol, physicalCardIds: plan.Cards, conversionSource: conversion);
        foreach (var c in cards)
            _corePhysicalMovementFacadesOperations.MoveCard(c, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, c), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        if (!_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(useId, actor.Seat))
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, new(cards[0].Id, SimpleCardUseEffect.Alcohol));
    }

    internal void BindMouJsNativeUse(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { MouJuShou: { Stage: MouJuShouStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        var targets = MouJsExpectedTargets(n);
        var source = n.Plan.Action.ConversionSource;
        if (!MouJuShou.Valid(f) || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != n.Actor || a.ProviderSeat != n.Actor || a.RequesterSeat is not null || use.SourceSeat != n.Actor || use.CardKind != n.Plan.Kind || !a.PhysicalCards.Select(p => p.CardId).Order().SequenceEqual(n.Plan.Cards.Order()) || a.PhysicalCards.Any(p => p.From != CardLocation.Hand(n.Actor)) || !use.PhysicalCardIds.Order().SequenceEqual(n.Plan.Cards.Order()) || !use.TargetSeats.SequenceEqual(targets) || source is not null && !a.ConversionChain.Contains(source))
            throw new InvalidOperationException("Zuo Jun changed his exact actual actor, original Hand materials, conversion source or native targets.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId,
            Targets = targets
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJuShou = r with { Stage = MouJuShouStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouJuShouZuoJun = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJsUseIssuedEvent(f.Id, use.Id, a.ActionId, n.Actor));
    }

    internal bool MouJsNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => MouJuShou.Valid(f) && u.MouJuShouZuoJun is { } n && f.MouJuShou is { Stage: MouJuShouStage.NativeUse, Native: { } p } && MouJuShouRuntime.SameUse(n, p) && n.FrameId == f.Id && n.Instruction == f.InstructionIndex && n.Source == f.MouJuShou.Source && n.GameplayHash == f.GameplayHash && n.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && u.Id == n.UseId && u.CardKind == n.Plan.Kind && u.TargetSeats.SequenceEqual(n.Targets) && u.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(u, n.Actor, n.Actor) && a.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(n.Plan.Cards.Order()) && a.PhysicalCards.All(c => c.From == CardLocation.Hand(n.Actor)) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouJsUseIssuedEvent>().Count(e => e == new MouJsUseIssuedEvent(f.Id, u.Id, a.ActionId, n.Actor)) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 36900)]
    internal bool MouJsOffTurnSlash(long id, int actor) => actor != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJuShouZuoJun: { } n } u && n.Actor == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } f && MouJsNativeRidesOn(u, f);
    [NativeUseValidation(36900)]
    internal void ValidateMouJsUse(CardUseFrame u)
    {
        if (u.MouJuShouZuoJun is { } n && (u.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != u.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || !MouJsNativeRidesOn(u, f)))
            throw new InvalidOperationException("Zuo Jun must retire only his exact once-issued completed native Use.");
    }

    [NativeUseReturn(36900)]
    internal bool ReturnMouJsUse(CardUseFrame u)
    {
        if (u.MouJuShouZuoJun is not { } n)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || !MouJsNativeRidesOn(u, f) || u.Step != ResolutionFrameStep.Completed)
            throw new InvalidOperationException("Zuo Jun cannot return a different or unfinished Use.");
        MouJuShou.Returned(n, u.CardAttack is { Active: true });
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 36900)]
    internal bool ResumeMouJsUse()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJuShou: { Stage: MouJuShouStage.ReturnedUse } } f)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}

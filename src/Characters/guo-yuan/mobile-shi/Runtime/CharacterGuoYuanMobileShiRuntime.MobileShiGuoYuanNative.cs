#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoYuanMobileShiRuntime
{
    internal bool IsShiGyHandQuery(CharacterState actor, int? id)
    {
        if (IsShiGyDeclarationQuery(actor, id))
            return true;
        return id is> 0 && actor.IsAlive && _cardZonesOperations._cardZones.GetLocation(id.Value) == CardLocation.Hand(actor.Seat) && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShiGuoYuan: { Stage: ShiGyStage.UseChoice or ShiGyStage.IssuingUse, Damaged: false } r } f && f.OwnerSeat == actor.Seat && ValidShiGy(f) && (r.Stage == ShiGyStage.UseChoice || r.Native is { UseId: 0, ActionId: 0 } n && n.CardId == id);
    }

    internal IReadOnlyList<LegalAction> ShiGyUseActions(ProgramSkillFrame f) => !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None ? [] : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).SelectMany(c => _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], false, c.Id).Where(a => a.CardId == c.Id && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.ConversionSource is null || _corePlayCardHistoryOperations.ViewAsRule(a.ConversionSource)is not { } rule || rule.InputCount == 1) && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral))).Select(ShiGyFreezeAction).ToArray();
    internal static LegalAction ShiGyFreezeAction(LegalAction a) => a with
    {
        TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()),
        AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()),
        SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()),
        SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()),
        SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()),
        SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(s => (IReadOnlyList<int>)Array.AsReadOnly(s.ToArray())).ToArray())
    };
    internal IReadOnlyList<PromptChoice> ShiGyUseChoices(ProgramSkillFrame f) => ShiGyUseActions(f).Select((a, i) => new PromptChoice(new($"shi-guo-yuan.{(f.Id)}.use.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, new Dictionary<string, string> { ["program-action"] = "shi-guo-yuan", ["branch"] = "native", ["index"] = i.ToString(CultureInfo.InvariantCulture) })).Append(new(new($"shi-guo-yuan.{(f.Id)}.use.skip"), "不使用手牌", [], [], new Dictionary<string, string> { ["program-action"] = "shi-guo-yuan", ["branch"] = "skip" })).ToArray();
    internal void AnswerShiGyUse(ProgramSkillFrame f, PromptChoice choice)
    {
        if (choice.Parameters["branch"] == "skip")
        {
            FinishShiGy(f);
            return;
        }

        var action = ShiGyUseActions(f)[int.Parse(choice.Parameters["index"], CultureInfo.InvariantCulture)];
        var r = f.ShiGuoYuan!;
        var n = new ShiGyNativeUse(f.Id, r.Source, r.Hash, r.Turn, f.OwnerSeat, action.CardId!.Value, action, 0, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiGuoYuan = r with { Stage = ShiGyStage.IssuingUse, Native = n, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], action);
    }

    internal static bool SameShiGyAction(LegalAction a, LegalAction b) => a.Kind == b.Kind && a.CardId == b.CardId && a.PlayedCardKind == b.PlayedCardKind && a.TargetCardId == b.TargetCardId && a.ProgramActivationId == b.ProgramActivationId && a.ConversionSource == b.ConversionSource && (a.AdditionalConversionSources ?? []).SequenceEqual(b.AdditionalConversionSources ?? []) && a.TargetSeats.SequenceEqual(b.TargetSeats);
    internal static bool SameShiGyNative(ShiGyNativeUse a, ShiGyNativeUse b) => a with
    {
        Action = b.Action
    }

    == b && SameShiGyAction(a.Action, b.Action);
    internal IReadOnlyList<int> ShiGyExpectedTargets(ShiGyNativeUse n)
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
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(a.PlayedCardKind ?? card.Kind, actor.Seat, a.TargetSeats);
        var kind = a.PlayedCardKind ?? card.Kind;
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(i => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, i)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 48100)]
    internal void BindShiGyNative(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { ShiGuoYuan: { Stage: ShiGyStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        if (!ValidShiGy(f) || use.CardId != n.CardId || use.SourceSeat != n.Actor || use.CardKind != (n.Action.PlayedCardKind ?? _coreCardAttackStateOperations.GetAttackCard(n.CardId).Kind) || a.ActorSeat != n.Actor || a.ProviderSeat != n.Actor || a.RequesterSeat is not null || a.PhysicalCards is not [var cost] || cost.CardId != n.CardId || cost.From != CardLocation.Hand(n.Actor) || !a.ConversionChain.SequenceEqual((n.Action.ConversionSource is { } source ? new[] { source } : []).Concat(n.Action.AdditionalConversionSources ?? [])) || !use.TargetSeats.SequenceEqual(ShiGyExpectedTargets(n)))
            throw new InvalidOperationException("Qingdao must issue the exact legal one-Hand entity with its native actor, targets and conversions.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShiGuoYuan = r with { Stage = ShiGyStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ShiGuoYuanUse = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyNativeIssuedEvent(f.Id, use.Id, a.ActionId, n.CardId));
    }

    internal bool IsShiGyNative(CardUseFrame use) => use.ShiGuoYuanUse is { } n && use.Id == n.UseId && use.CardId == n.CardId && use.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ProviderSeat == n.Actor && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, n.Actor, n.Actor) && a.PhysicalCards is [var cost] && cost.CardId == n.CardId && cost.From == CardLocation.Hand(n.Actor) && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == n.FrameId)is ProgramSkillFrame { ShiGuoYuan.Native: { } pending } parent && ValidShiGy(parent) && SameShiGyNative(n, pending) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiGyNativeIssuedEvent>().Count(e => e == new ShiGyNativeIssuedEvent(parent.Id, use.Id, a.ActionId, n.CardId)) == 1;
    internal bool HasShiGyUseDistance(int actor, IReadOnlyList<int>? ids, long? useId)
    {
        var use = useId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id) : _resolutionStackOperations._resolutionStack.LastOrDefault() as CardUseFrame;
        if (use is null || use.SourceSeat != actor || ids is { Count: > 0 } && !ids.SequenceEqual(use.PhysicalCardIds ?? []))
            return false;
        if (use.ShiGuoYuanUse is not null)
            return IsShiGyNative(use);
        // BeginCardUse has pushed its exact child before the declaration fact
        // binds the invoice. Target verification still needs this issued grant.
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { ShiGuoYuan: { Stage: ShiGyStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } } parent && ValidShiGy(parent) && n.Actor == actor && n.CardId == use.CardId && use.Action is { Type: CardActionType.Use, PhysicalCards: [var cost] } a && a.ActorSeat == actor && a.ProviderSeat == actor && cost.CardId == n.CardId && cost.From == CardLocation.Hand(actor) && a.ConversionChain.SequenceEqual((n.Action.ConversionSource is { } source ? new[] { source } : []).Concat(n.Action.AdditionalConversionSources ?? []));
    }

    [NativeUseValidation(48100)]
    internal void ValidateShiGyNative(CardUseFrame use)
    {
        if (use.ShiGuoYuanUse is not null && (use.Step != ResolutionFrameStep.Completed || !IsShiGyNative(use)))
            throw new InvalidOperationException("Qingdao lost its exact completed child use.");
    }

    [NativeUseReturn(48100)]
    internal void ReturnShiGyNative(CardUseFrame use)
    {
        if (use.ShiGuoYuanUse is not { } n)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        var r = f.ShiGuoYuan!;
        if (!IsShiGyNative(use) || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == n.CardId) != 1)
            throw new InvalidOperationException("Qingdao cannot return or repay an unfinished or different native use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyNativeReturnedEvent(f.Id, use.Id, n.ActionId));
        if (use.CardAttack is not { Active: true })
            SettleShiGyNative(f);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 48100)]
    internal bool ReturnShiGyAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiGuoYuan: { Stage: ShiGyStage.NativeUse, Returned: true } } f)
            return false;
        SettleShiGyNative(f);
        return true;
    }

    internal void SettleShiGyNative(ProgramSkillFrame f)
    {
        var r = f.ShiGuoYuan!;
        var n = r.Native!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(n.UseId)is not null)
            throw new InvalidOperationException("Qingdao still owns an unreturned use.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == n.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyNativeBatchEvent(f.Id, n.UseId, b.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.NativeTail, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueShiGy(f.Id);
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 48100)]
    internal bool ShiGyCardDistance(ProgramCardDistanceQuery q) => HasShiGyUseDistance(q.Actor.Seat, q.PhysicalCardIds, q.CardUseFrameId) || q.PhysicalCardIds is [var card] && IsShiGyHandQuery(q.Actor, card);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 48100)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 48100)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 48100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 48100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 48100)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 48100)]
    internal bool ShiGySlashDistance(ProgramSlashDistanceQuery q) => HasShiGyUseDistance(q.Actor.Seat, q.PhysicalCardIds ?? (q.PhysicalCard is { } c ? [c.Id] : null), q.ExistingUseFrameId) || IsShiGyHandQuery(q.Actor, q.PhysicalCardIds is [var card] ? card : q.PhysicalCard?.Id);
    internal bool IsShiGyOffTurnQuery(CharacterState actor, int? card) => (actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play) && IsShiGyHandQuery(actor, card);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 48100)]
    internal bool ShiGySlashQuota(ProgramSlashQuotaQuery q) => IsShiGyOffTurnQuery(q.Actor, q.PhysicalCardIds is [var card] ? card : q.PhysicalCard?.Id);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 48100)]
    internal bool ShiGyOffTurnSlash(long id, int actor) => (actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ShiGuoYuanUse: { } n } use && n.Actor == actor && IsShiGyNative(use);
    internal bool MatchesShiGyDeclaration(ProgramSkillFrame root, CardDeclarationFrame d) => root.ShiGuoYuan is { Stage: ShiGyStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r && ValidShiGy(root) && d.OwnerSeat == n.Actor && d.TurnNumber == r.Turn && d.Return is { Purpose: CardDeclarationPurpose.Use, UseAction: { } a } ret && ret.ParentFrameId == root.Id && ret.ParentKind == root.Kind && ret.ParentStep == root.Step && ret.ActorSeat == n.Actor && SameShiGyAction(a, n.Action) && d.Payment is { Claimed: false } p && p.DeclarationId == d.Id && p.OwnerFrameId == d.Id && p.ProviderSeat == n.Actor && p.ActorSeat == n.Actor && p.Cost.CardId == n.CardId && p.Cost.From == CardLocation.Hand(n.Actor) && n.Action.ConversionSource == p.Source && _corePlayCardHistoryOperations.ViewAsRule(p.Source)is { DeclarationValidation: not null } rule && rule.OutputKind == p.DeclaredKind && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardDeclarationCommittedEvent>().Count(e => e.DeclarationId == d.Id && e.OwnerSeat == n.Actor && e.ActorSeat == n.Actor && e.DeclaredKind == p.DeclaredKind && e.TargetSeats.SequenceEqual(a.TargetSeats)) == 1;
    internal bool IsShiGyDeclarationQuery(CharacterState actor, int? id) => id is { } card && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardDeclarationFrame { Stage: CardDeclarationStage.Applying, Succeeded: true } d && _resolutionStackOperations._resolutionStack.Count > 1 && _resolutionStackOperations._resolutionStack[^2] is ProgramSkillFrame root && MatchesShiGyDeclaration(root, d) && d.OwnerSeat == actor.Seat && d.Payment.Cost.CardId == card && _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, card) == d.Payment;
    internal bool TryReturnShiGyDeclaration(CardDeclarationFrame d, bool success)
    {
        if (success || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiGuoYuan: { Stage: ShiGyStage.IssuingUse } r } f || !MatchesShiGyDeclaration(f, d))
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.Movement, Native = null, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueShiGy(f.Id);
        return true;
    }
}

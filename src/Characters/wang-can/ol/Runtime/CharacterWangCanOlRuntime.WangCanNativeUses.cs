#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    internal IReadOnlyList<LegalAction> BuildDenglouNativeBasicUseOptions(ProgramSkillFrame f)
    {
        if (f.WangCan is not { Lease: { } lease } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var result = new List<LegalAction>();
        foreach (var id in r.Remaining.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == lease))
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            if (CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind) != SkillProgramCardCategory.Basic || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [id]))
                continue;
            var variants = new List<(CardKind Kind, CardConversionSource? Source)>
            {
                (card.Kind, null)
            };
            foreach (var kind in new[]
            {
                CardKind.Slash,
                CardKind.FireSlash,
                CardKind.ThunderSlash,
                CardKind.Peach,
                CardKind.Alcohol
            }

            )
                foreach (var conversion in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, forResponse: false))
                    if (_corePlayCardHistoryOperations.ViewAsRule(conversion)is { DeclarationValidation: null } rule && rule.InputCount == 1 && rule.SourceZones.Contains(lease.Zone))
                        variants.Add((kind, conversion));
            foreach (var(kind, conversion)in variants.Distinct())
            {
                var appearance = _characterWuJingOlOperations.WuJingLiubingUseCard(actor, card, kind, conversion, [id]);
                var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, appearance);
                var color = CoreGameEnginePublicPileColorsRuntime.SuitColor(suit);
                if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
                    continue;
                if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
                {
                    var targets = _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, suit, color, card.Rank, [id], conversionSource: conversion, actualUnconvertedPhysical: conversion is null) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, t.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, t.Seat, suit)).ToArray();
                    foreach (var target in targets)
                        result.Add(new(LegalActionKind.Slash, id, target.Seat, $"登楼：将【{(card.DisplayName)}】对{(target.Name)}使用", kind, TargetSeats: [target.Seat]) { ConversionSource = conversion });
                    var multiple = new List<LegalAction>();
                    _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(multiple, actor, card, targets, CardCatalog.Get(kind).DisplayName, kind);
                    result.AddRange(multiple.Select(a => a with { ConversionSource = conversion }));
                }
                else if (actor.HasHpValue && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, suit))
                {
                    if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor))
                        result.Add(new(LegalActionKind.Peach, id, actor.Seat, $"登楼：使用【桃】", kind, TargetSeats: [actor.Seat]) { ConversionSource = conversion });
                    if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && (_characterLuKangWumiaoOperations.HasWuLuKangDixianMaterialPrivilege(actor, kind, [id], conversion) || _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Alcohol, [id])))
                        result.Add(new(LegalActionKind.Alcohol, id, actor.Seat, "登楼：使用【酒】", kind, TargetSeats: [actor.Seat]) { ConversionSource = conversion });
                }
            }
        }

        return Array.AsReadOnly(result.Select(a => a with { TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()) }).ToArray());
    }

    internal void IssueDenglouNativeUse(ProgramSkillFrame f, LegalAction action)
    {
        RequireIssuedWangCan(f);
        var r = f.WangCan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var id = action.CardId!.Value;
        if (r.Stage != WangCanStage.DenglouChoose || !r.Remaining.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != r.Lease || !BuildDenglouNativeBasicUseOptions(f).Any(a => a.Kind == action.Kind && a.CardId == id && a.PlayedCardKind == action.PlayedCardKind && a.ConversionSource == action.ConversionSource && a.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("Denglou cannot replace the exact private basic entity or its actual legal use.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var kind = action.PlayedCardKind ?? card.Kind;
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, action.TargetSeats, kind, physicalCardIds: [id], conversionSource: action.ConversionSource ?? r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!;
        var receipt = new WangCanNativeReceipt(f.Id, r.Source, r.GameplayHash, r.Instruction, r.ActualTurn, r.Phase, r.WindowId, id, r.Lease!.Value, kind, action.ConversionSource, useId, use.Action!.ActionId, use.Action.ParentActionId)
        {
            Targets = action.TargetSeats
        };
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
        if (parent.WangCan != r)
            throw new InvalidOperationException("Denglou original private lease changed during native issuance.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { WangCan = r with { Stage = WangCanStage.DenglouNative, Native = receipt, Remaining = r.Remaining.Where(entity => entity != id).ToArray() } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { DenglouNative = receipt, DenglouCostBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DenglouCostAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DenglouCostDrained = false });
        _corePhysicalMovementFacadesOperations.MoveCard(card, receipt.From, CardLocation.Processing, CardMoveReasons.Use);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { DenglouCostAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangCanNativeIssuedEvent(f.Id, r.Source, r.GameplayHash, useId, receipt.ActionId, f.OwnerSeat, kind));
        ContinueDenglouNativePayment(useId);
    }

    internal bool IsDenglouNativeUse(CardUseFrame use) => use.DenglouNative is not null;
    internal bool DenglouNativeRidesOn(ProgramSkillFrame parent, CardUseFrame use) => parent.WangCan is { Stage: WangCanStage.DenglouNative, Native: { } n } r && use.DenglouNative == n && n.ProgramFrameId == parent.Id && n.Source == WangCanSource(parent) && n.GameplayHash == parent.GameplayHash && n.From == r.Lease && r.Entities.Contains(n.CardId) && !r.Remaining.Contains(n.CardId);
    [NativeUseValidation(90)]
    internal void ValidateDenglouNativeUseReturn(CardUseFrame use)
    {
        if (IsDenglouNativeUse(use))
            AssertDenglouNativeUse(use);
    }

    internal void AssertDenglouNativeUse(CardUseFrame use)
    {
        if (use.DenglouNative is not { } n || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == n.ProgramFrameId)is not { } parent || !DenglouNativeRidesOn(parent, use) || use.Id != n.UseId || use.Action is not { Type: CardActionType.Use } a || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, parent.OwnerSeat, parent.OwnerSeat) || a.ActionId != n.ActionId || a.ParentActionId != n.ParentActionId || a.EffectiveKind != n.EffectiveKind || use.CardId != n.CardId || use.CardKind != n.EffectiveKind || !a.PhysicalCards.Select(c => c.CardId).SequenceEqual([n.CardId]) || a.PhysicalCards.Single().From != n.From || !a.ConversionChain.SequenceEqual([n.Conversion ?? n.Source]) || !a.TargetSeats.SequenceEqual(use.TargetSeats) || use.DenglouCostBefore is not { } before || use.DenglouCostAfter is not { } after || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.CardId == n.CardId && m.From == n.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanNativeIssuedEvent>().Count(e => e.FrameId == parent.Id && e.Source == n.Source && e.GameplayHash == n.GameplayHash && e.UseId == use.Id && e.ActionId == n.ActionId && e.ActorSeat == parent.OwnerSeat && e.EffectiveKind == n.EffectiveKind) != 1)
            throw new InvalidOperationException("Denglou native use lost its exact private entity, source, original actor, native action or once-only payment.");
        RequireIssuedWangCan(parent);
    }

    [NativeFrameRoute(typeof(CardUseFrame), 150, typeof(NativeFrameRouteConditions), "DenglouPayment")]
    internal void ContinueDenglouNativePayment(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        AssertDenglouNativeUse(use);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.DenglouCardUse) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.DenglouCardUse) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { DenglouCostDrained = true });
        var n = use.DenglouNative!;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(n.CardId);
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !actor.IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, n.EffectiveKind);
            return;
        }

        if (n.EffectiveKind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Recovery, 1));
            return;
        }

        if (n.EffectiveKind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Alcohol));
            return;
        }

        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        var nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(id, actor);
        var amount = 1 + actor.AlcoholDamageBonus + nuzhan.DamageBonus;
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        if (targets.Count > 1)
        {
            var pending = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id, actor.Seat, card, n.EffectiveKind, false, amount, targets.ToArray(), false, false, n.Conversion ?? n.Source);
            _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = pending;
            var rule = _coreRuleQueriesOperations.EvaluateCardTargetCount(actor, n.EffectiveKind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardTargetCountAppliedEvent(id, actor.Seat, n.EffectiveKind, targets, Array.AsReadOnly(rule.Value.Contributions.Select(c => c.SourceId).ToArray())));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, n.EffectiveKind, actor.Seat, targets[0]));
            foreach (var target in targets)
                _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[target]);
            _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(pending);
            return;
        }

        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targets.Single(), card, damageAmount: amount, playedCardKind: n.EffectiveKind, physicalCards: [card], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets.Single()], n.EffectiveKind), programSkillCardUseFrameId: n.ProgramFrameId);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, n.EffectiveKind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, n.EffectiveKind, actor.Seat, targets.Single()));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    [NativeUseReturn(10)]
    internal bool CompleteDenglouUseReturn(CardUseFrame use)
    {
        if (!IsDenglouNativeUse(use))
            return false;
        ReturnDenglouNativeUse(use);
        return true;
    }

    internal void ReturnDenglouNativeUse(CardUseFrame use)
    {
        if (use.DenglouNative is not { } n || use.CardAttack is { Active: true })
            return;
        FinishDenglouNativeReturn(n);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 120)]
    internal bool ReturnRegisteredCompleteDenglouNativeAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.DenglouNative is null)
            return false;
        CompleteDenglouNativeAttack(completion);
        return true;
    }

    internal void CompleteDenglouNativeAttack(GameEngine.AttackCompletionReceipt completion) => FinishDenglouNativeReturn(completion.DenglouNative!);
    internal void FinishDenglouNativeReturn(WangCanNativeReceipt n)
    {
        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.ProgramFrameId);
        if (parent.WangCan is not { Stage: WangCanStage.DenglouNative, Native: { } issued } r || issued != n || _coreCardUseLifecycleOperations.LifecycleCardUse(n.UseId)is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == n.UseId) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanNativeReturnedEvent>().Any(e => e.FrameId == parent.Id && e.UseId == n.UseId))
            throw new InvalidOperationException("Denglou must return once after its entire native basic-card use.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangCanNativeReturnedEvent(parent.Id, n.UseId, n.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WangCan = r with { Stage = WangCanStage.Completed } });
    }

    internal bool DenglouNativePaymentEdge(int index)
    {
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not CardUseFrame { DenglouNative: { } n, DenglouCostDrained: false } use)
            return false;
        AssertDenglouNativeUse(use);
        var child = _resolutionStackOperations._resolutionStack[index];
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeDenglouUseFrameId == use.Id && moved.Batch.ParentFrameId == use.Id && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > use.DenglouCostBefore && m.Sequence <= use.DenglouCostAfter && m.CardId == n.CardId && m.From == n.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == use.Id && hp.Change.ParentFrameId == use.Id && hp.Continuation == PostEventContinuation.DenglouCardUse;
        return child is RecoveryReplacementFrame recovery && recovery.ParentFrameId == use.Id && recovery.Return.ResumeFrameId == use.Id && recovery.Return.Continuation == PostEventContinuation.DenglouCardUse;
    }
}

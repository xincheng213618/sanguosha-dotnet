#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanXiangAnniversaryRuntime
{
    internal IReadOnlyList<CardKind> YanXiangDamageNames() => Array.AsReadOnly(new[] { CardKind.Slash, CardKind.Duel, CardKind.BarbarianAssault, CardKind.ArrowBarrage, CardKind.FireAttack }.Concat(_cardZonesOperations._cardZones.CreateDiagnostics().Select(c => c.CardKind).Where(k => k is CardKind.WumiaoFlood or CardKind.QizhengXiangsheng)).Distinct().ToArray());
    internal static bool SameYanXiangPlan(AnniversaryYanXiangPlan a, AnniversaryYanXiangPlan b) => (a.CardId, a.From, a.Kind, a.ActionKind, a.TargetCardId, a.RequiredKind, a.Adjusted) == (b.CardId, b.From, b.Kind, b.ActionKind, b.TargetCardId, b.RequiredKind, b.Adjusted) && a.Targets.SequenceEqual(b.Targets);
    internal IReadOnlyList<AnniversaryYanXiangPlan> YanXiangUsePlans(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYanXiang!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var result = new List<AnniversaryYanXiangPlan>();
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.Name is not { } name || YanXiangQuotaForbidden(actor.Seat))
            return[];
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).OrderBy(c => c.Id).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [c.Id])))
        {
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, card);
            var color = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, card);
            if (name == CardKind.Slash)
            {
                var actions = new List<LegalAction>();
                foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                {
                    if (YanXiangColorBanned(actor.Seat, YanXiangUseColor(actor.Seat, kind, [card.Id], [r.Source])))
                        continue;
                    var targets = _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, suit, color, card.Rank, [card.Id], conversionSource: r.Source) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, t.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, t.Seat, suit)).ToArray();
                    foreach (var target in targets)
                        actions.Add(new(LegalActionKind.Slash, card.Id, target.Seat, "邃识", kind, TargetSeats: [target.Seat]));
                    _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, card, targets, "邃识", kind, r.Source);
                    _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, "邃识", kind, r.Source);
                }

                _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(actions, actor);
                result.AddRange(actions.Select(a => new AnniversaryYanXiangPlan(card.Id, from, a.PlayedCardKind!.Value, a.Kind, Adjusted: a.ProgramActivationId == "next-actual-use-target-adjustment") { Targets = a.TargetSeats }));
            }
            else if (!YanXiangColorBanned(actor.Seat, YanXiangUseColor(actor.Seat, name, [card.Id], [r.Source])))
                result.AddRange(_coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, name, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: true, conversionSource: r.Source).Select(o => new AnniversaryYanXiangPlan(card.Id, from, o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind, o.NextActualUseAdjusted) { Targets = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(o.EffectiveCardKind, actor.Seat, o.TargetSeats) }));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void IssueYanXiangUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYanXiang!;
        var p = r.Plan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId);
        if (r.Stage != AnniversaryYanXiangStage.IssuingUse || !ValidYanXiang(f) || !YanXiangUsePlans(f).Any(x => SameYanXiangPlan(x, p)))
            throw new InvalidOperationException("Suishi lost the recipient's own currently legal native material and targets.");
        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        var id = _coreNativeCardUseOperations.BeginCardUse(card, r.Actor, p.Targets, p.Kind, physicalCardIds: [card.Id], conversionSource: r.Source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.AnniversaryYanXiangUse is null)
            throw new InvalidOperationException("Suishi must bind its original actual native Use before paying its material.");
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            _coreNativeMaterialSlashOperations.CompleteNativeMaterialSlash(id, actor, [card], p.Kind, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, r.Source, _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[p.Targets[0]], p.Kind));
            return;
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, p.From, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, r.Actor, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46801)]
    internal void BindYanXiangUse(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversaryYanXiang: { Stage: AnniversaryYanXiangStage.IssuingUse, Plan: { } p } r } f)
            return;
        if (!ValidYanXiang(f) || use.CardId != p.CardId || use.SourceSeat != r.Actor || use.CardKind != p.Kind || a.ActorSeat != r.Actor || a.ProviderSeat != r.Actor || a.RequesterSeat is not null || a.PhysicalCards is not [{ CardId: var material, From: var from }] || material != p.CardId || from != p.From || !_cardZonesOperations._cardZones.Contains(from, material) || !a.ConversionChain.SequenceEqual([r.Source]) || !use.TargetSeats.SequenceEqual(p.Targets))
            throw new InvalidOperationException("Suishi changed its original recipient, HE material, declared damage name or native targets.");
        var origin = new AnniversaryYanXiangUseOrigin(f.Id, r.Source, r.Hash, r.Actor, p.CardId, p.From, p.Kind, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryYanXiang = r with { Stage = AnniversaryYanXiangStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { AnniversaryYanXiangUse = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYanXiangUseIssuedEvent(f.Id, use.Id, a.ActionId, r.Actor, p.CardId, p.Kind));
    }

    internal bool IsYanXiangNativeUse(CardUseFrame use) => use.AnniversaryYanXiangUse is { } o && use.Id == o.UseId && use.CardId == o.CardId && use.CardKind == o.Kind && use.Action is { Type: CardActionType.Use } a && a.ActionId == o.ActionId && a.ProviderSeat == o.Actor && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Actor, o.Actor) && a.EffectiveKind == o.Kind && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == o.CardId && from == o.From && use.PhysicalCardIds is [var physical] && physical == id && a.ConversionChain.SequenceEqual([o.Source]) && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is ProgramSkillFrame { AnniversaryYanXiang: { Plan: { } p } r } f && ValidYanXiang(f) && r.Source == o.Source && r.Hash == o.Hash && r.Actor == o.Actor && r.UseId == use.Id && r.ActionId == a.ActionId && p.CardId == o.CardId && p.From == o.From && p.Kind == o.Kind && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryYanXiangUseIssuedEvent>().Count(e => e == new AnniversaryYanXiangUseIssuedEvent(f.Id, use.Id, a.ActionId, o.Actor, o.CardId, o.Kind)) == 1;
    [NativeUseValidation(46800)]
    internal void ValidateYanXiangNativeReturn(CardUseFrame use)
    {
        if (use.AnniversaryYanXiangUse is not null && (use.Step != ResolutionFrameStep.Completed || !IsYanXiangNativeUse(use)))
            throw new InvalidOperationException("Suishi must return its own completed original native Use.");
    }

    [NativeUseReturn(46800)]
    internal void ReturnYanXiangUse(CardUseFrame use)
    {
        if (use.AnniversaryYanXiangUse is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.AnniversaryYanXiang!;
        if (!IsYanXiangNativeUse(use) || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == o.CardId) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == o.CardId && m.Sequence > r.Before && m.From == o.From && m.Reason == CardMoveReasons.Use && (m.To == CardLocation.Processing || m.To == CardLocation.OutsideGame && _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Any(d => d.CardId == m.CardId && d.MovementSequence == m.Sequence && d.ParentFrameId == use.Id))) != 1)
            throw new InvalidOperationException("Suishi cannot pay or retire another use, or complete the same use twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryYanXiang = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYanXiangUseReturnedEvent(f.Id, use.Id, o.ActionId));
        if (use.CardAttack is not { Active: true })
            SettleYanXiangUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 46800)]
    internal bool ReturnYanXiangAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryYanXiang: { Stage: AnniversaryYanXiangStage.NativeUse, Returned: true } } f)
            return false;
        SettleYanXiangUse(f);
        return true;
    }

    internal void SettleYanXiangUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYanXiang!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Suishi must await all native targets and its complete original use tail.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYanXiangNativeBatchEvent(f.Id, r.UseId, b.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYanXiang = r with { Stage = AnniversaryYanXiangStage.NativeTail, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueYanXiang(f.Id);
    }

    internal bool IsAnniversaryYanXiangSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.AnniversaryYanXiang is { Stage: AnniversaryYanXiangStage.NativeTail, Returned: true } r && ValidYanXiang(f) && r.NativeBatches.Contains(b.Id) && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.Count > 0 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryYanXiangNativeBatchEvent>().Count(e => e == new AnniversaryYanXiangNativeBatchEvent(f.Id, r.UseId, b.Id)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1500)]
    internal bool WindowSettlement_IsAnniversaryYanXiangSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsAnniversaryYanXiangSettlementBatch(frame, batch);
}

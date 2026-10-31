#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQiaoRuiAnniversaryRuntime
{
    internal IReadOnlyList<AnniversaryQiaoRuiPlan> QiaoRuiPlans(int ownerSeat, CardConversionSource source, IReadOnlyList<int> originalTargets)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var plans = new List<AnniversaryQiaoRuiPlan>();
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        foreach (var id in QiaoRuiMarkedHand(ownerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            if (from != CardLocation.Hand(ownerSeat) || _coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(ownerSeat, [id]))
                continue;
            var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, card);
            var color = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(ownerSeat, card);
            var targets = originalTargets.Distinct().Where(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && t != ownerSeat && _coreMatchParticipantsPlayersAccess._players[t].IsAlive && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(ownerSeat, t) && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], CardKind.Slash, suit, color, card.Rank, [id], conversionSource: source) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, t, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[t], CardKind.Slash, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(ownerSeat, t, suit)).Select(t => _coreMatchParticipantsPlayersAccess._players[t]).ToArray();
            var actions = targets.Select(t => new LegalAction(LegalActionKind.Slash, id, t.Seat, "扫围", CardKind.Slash, TargetSeats: [t.Seat])).ToList();
            _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, card, targets, "扫围", CardKind.Slash, source);
            _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, "扫围", CardKind.Slash, source);
            _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(actions, actor);
            foreach (var action in actions.Where(a => a.TargetSeats.Count > 0 && a.TargetSeats.All(t => targets.Any(p => p.Seat == t))))
                if (!plans.Any(p => p.CardId == id && p.Targets.SequenceEqual(action.TargetSeats) && p.Adjusted == (action.ProgramActivationId == "next-actual-use-target-adjustment")))
                    plans.Add(new(id, from, action.ProgramActivationId == "next-actual-use-target-adjustment") { Targets = action.TargetSeats });
        }

        return Array.AsReadOnly(plans.ToArray());
    }

    internal IReadOnlyList<AnniversaryQiaoRuiPlan> QiaoRuiPlans(ProgramSkillFrame f) => QiaoRuiOriginalSlash(f.WindowContext)is { Action: { } a } ? QiaoRuiPlans(f.OwnerSeat, f.AnniversaryQiaoRui!.Source, a.TargetSeats) : [];
    internal static bool SameQiaoRuiPlan(AnniversaryQiaoRuiPlan p, AnniversaryQiaoRuiPlan q) => p.CardId == q.CardId && p.From == q.From && p.Adjusted == q.Adjusted && p.Targets.SequenceEqual(q.Targets);
    internal PromptChoice QiaoRuiChoice(ProgramSkillFrame f, string verb, string value, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"qiao-rui.{(f.Id)}.{(verb)}.{(value)}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = QiaoRuiAction, ["verb"] = verb, ["value"] = value });
    internal IReadOnlyList<PromptChoice> QiaoRuiChoices(ProgramSkillFrame f) => Array.AsReadOnly(QiaoRuiPlans(f).Select((p, i) => QiaoRuiChoice(f, "use", i.ToString(CultureInfo.InvariantCulture), "将隘【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId).DisplayName + "】当【杀】使用", [p.CardId], p.Targets)).Append(QiaoRuiChoice(f, "decline", "done", "不使用扫围杀")).ToArray());
    internal void PublishQiaoRui(ProgramSkillFrame f)
    {
        if (QiaoRuiPlans(f).Count == 0)
        {
            FinishQiaoRui(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var choices = QiaoRuiChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "扫围：选择一张隘与原杀目标中的合法目标。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryQiaoRuiSaowei)]
    internal PromptChoice QiaoRuiAi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer(QiaoRuiAction)]
    internal void AnswerQiaoRui(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryQiaoRui: { Stage: AnniversaryQiaoRuiStage.SlashChoice } r } f || !ValidQiaoRui(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], QiaoRuiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Saowei rejects another marked material, original target group or native permission.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["verb"] == "decline")
        {
            FinishQiaoRui(f);
            return;
        }

        var plan = QiaoRuiPlans(f)[int.Parse(choice.Parameters["value"], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = r with { Stage = AnniversaryQiaoRuiStage.Issuing, Plan = plan, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        IssueQiaoRuiSlash(f);
    }

    internal void IssueQiaoRuiSlash(ProgramSkillFrame f)
    {
        var r = f.AnniversaryQiaoRui!;
        var p = r.Plan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (r.Stage != AnniversaryQiaoRuiStage.Issuing || !ValidQiaoRui(f) || !QiaoRuiPlans(f).Any(q => SameQiaoRuiPlan(p, q)))
            throw new InvalidOperationException("Saowei lost its exact current marked Hand payment and native Slash target group.");
        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, p.Targets, CardKind.Slash, physicalCardIds: [p.CardId], conversionSource: r.Source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.AnniversaryQiaoRuiUse is null)
            throw new InvalidOperationException("Saowei must bind the original physical native Slash before payment.");
        _coreNativeMaterialSlashOperations.CompleteNativeMaterialSlash(id, actor, [card], CardKind.Slash, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, r.Source, _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[p.Targets[0]], CardKind.Slash));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 49400)]
    internal void BindQiaoRuiSlash(CardUseDeclaredEvent e)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversaryQiaoRui: { Stage: AnniversaryQiaoRuiStage.Issuing, Plan: { } p } r } f)
            return;
        if (!ValidQiaoRui(f) || use.CardId != p.CardId || use.SourceSeat != f.OwnerSeat || use.CardKind != CardKind.Slash || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || a.PhysicalCards is not [{ CardId: var material, From: var from }] || material != p.CardId || from != p.From || !_cardZonesOperations._cardZones.Contains(from, material) || !a.ConversionChain.SequenceEqual([r.Source]) || !use.TargetSeats.SequenceEqual(p.Targets))
            throw new InvalidOperationException("Saowei cannot replace its original owner, marked entity, ordinary Slash or chosen original target group.");
        var o = new AnniversaryQiaoRuiUseOrigin(f.Id, r.Source, r.Hash, p.CardId, p.From, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryQiaoRui = r with { Stage = AnniversaryQiaoRuiStage.NativeSlash, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { AnniversaryQiaoRuiUse = o });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiUseIssuedEvent(o));
    }

    internal bool IsQiaoRuiNativeUse(CardUseFrame use) => use.AnniversaryQiaoRuiUse is { } o && use.Id == o.UseId && use.CardId == o.CardId && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.Action is { Type: CardActionType.Use } a && a.ActionId == o.ActionId && a.ProviderSeat == o.Source.OwnerSeat && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Source.OwnerSeat, o.Source.OwnerSeat) && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == o.CardId && from == o.From && use.PhysicalCardIds is [var physical] && physical == id && a.ConversionChain.SequenceEqual([o.Source]) && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is ProgramSkillFrame { AnniversaryQiaoRui: { Plan: { } p } r } f && ValidQiaoRui(f) && r.Source == o.Source && r.Hash == o.Hash && r.UseId == use.Id && r.ActionId == a.ActionId && p.CardId == o.CardId && p.From == o.From && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryQiaoRuiUseIssuedEvent>().Count(e => e.Origin == o) == 1;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 49401)]
    internal void ObserveQiaoRuiDamage(DamageAppliedEvent e)
    {
        if (e.Amount <= 0 || e.SourceCardUseResolutionId is not { } useId || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { AnniversaryQiaoRuiUse: { } o } use || !IsQiaoRuiNativeUse(use))
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == o.FrameId);
        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.TargetSeat == e.TargetSeat && d.Amount == e.Amount);
        if (damage is null)
            throw new InvalidOperationException("Saowei damage needs its real native damage frame.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryQiaoRui = f.AnniversaryQiaoRui!with { Damaged = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiDamageEvent(f.Id, useId, damage.Id, e.TargetSeat, e.Amount));
    }

    [NativeUseValidation(49400)]
    internal void ValidateQiaoRuiNativeReturn(CardUseFrame use)
    {
        if (use.AnniversaryQiaoRuiUse is not null && (use.Step != ResolutionFrameStep.Completed || !IsQiaoRuiNativeUse(use)))
            throw new InvalidOperationException("Saowei must return its wholly completed original native Slash.");
    }

    [NativeUseReturn(49400)]
    internal void ReturnQiaoRuiUse(CardUseFrame use)
    {
        if (use.AnniversaryQiaoRuiUse is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.AnniversaryQiaoRui!;
        if (!IsQiaoRuiNativeUse(use) || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == o.CardId) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == o.CardId && m.Sequence > r.Before && m.From == o.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) != 1)
            throw new InvalidOperationException("Saowei cannot pay another material or return the same use twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryQiaoRui = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiUseReturnedEvent(f.Id, use.Id, o.ActionId));
        if (use.CardAttack is not { Active: true })
            SettleQiaoRuiUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 49400)]
    internal bool ReturnQiaoRuiAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryQiaoRui: { Stage: AnniversaryQiaoRuiStage.NativeSlash, Returned: true } } f)
            return false;
        SettleQiaoRuiUse(f);
        return true;
    }

    internal void SettleQiaoRuiUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryQiaoRui!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Saowei must await every native target and its original material tail.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiNativeBatchEvent(f.Id, r.UseId, b.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = r with { Stage = AnniversaryQiaoRuiStage.NativeTail, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueQiaoRui(f.Id);
    }

    internal bool IsAnniversaryQiaoRuiSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.AnniversaryQiaoRui is { Stage: AnniversaryQiaoRuiStage.NativeTail, Returned: true } r && ValidQiaoRui(f) && r.NativeBatches.Contains(b.Id) && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.Count > 0 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryQiaoRuiNativeBatchEvent>().Count(e => e == new AnniversaryQiaoRuiNativeBatchEvent(f.Id, r.UseId, b.Id)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 2600)]
    internal bool WindowSettlement_IsAnniversaryQiaoRuiSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsAnniversaryQiaoRuiSettlementBatch(frame, batch);
}

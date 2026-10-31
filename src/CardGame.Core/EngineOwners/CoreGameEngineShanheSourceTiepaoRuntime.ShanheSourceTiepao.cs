#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceTiepaoRuntime
{
    SkillProgramStepOutcome IShanheSourceTiepaoHost.Begin(ProgramSkillFrame f) => BeginShanheSourceTiepao(f);
    internal GameEngine.IDamageAttempt? SourceTiepaoOriginalAttack(DamageTriggerWindowFrame window, long damageId, int amount)
    {
        if (window.ParentFrameId != damageId || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is not { } damage || damage.SourceSeat != window.SourceSeat || damage.TargetSeat != window.TargetSeat || damage.Amount != amount || amount <= 0)
            return null;
        // Resolve the suspended owner by the original DamageFrame's parent.
        // CurrentDamageAttempt may now belong to the counter-Slash child.
        var attack = _coreDamageArmorPaymentsOperations.Value.DamageArmorPaymentOwner(damage);
        return attack is { DamageWasApplied: true, IsSourceLess: false } && attack.ResolutionId == damage.ParentFrameId && attack.SourceSeat == damage.SourceSeat && attack.TargetSeat == damage.TargetSeat && attack.DamageAmount == damage.Amount && window.SourceCardId == attack.Card?.Id && window.SourceCard == attack.EffectiveCardKind ? attack : null;
    }

    internal bool SourceTiepaoDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == ShanheSourceTiepaoComposition.SkillId && w.Window == SkillProgramTriggerWindow.AfterDamageApplied && w.TargetSeat == c.OwnerSeat && w.Amount > 0 && w.SourceSeat is { } source && source != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && w.DamageFrameId is { } damageId && _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is { } parent && parent.Id == w.ParentFrameId && parent.ParentFrameId == damageId && SourceTiepaoOriginalAttack(parent, damageId, w.Amount)is { } attack && attack.SourceSeat == source && attack.TargetSeat == c.OwnerSeat;
    internal CardConversionSource SourceTiepaoConversion(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool SourceTiepaoMaterialLegal(int owner, int target, Card card, CardConversionSource source)
    {
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        var recipient = _coreMatchParticipantsPlayersAccess._players[target];
        return owner != target && actor.IsAlive && recipient.IsAlive && from.OwnerSeat == owner && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner, [card.Id]) && (from.Zone != CardZoneKind.Hand || !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(owner, SkillProgramCardTargetRestriction.SelfOnly) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, recipient, card, source, CardKind.Slash, physicalCardIds: [card.Id], excludedEquipmentId: from.Zone == CardZoneKind.Equipment ? card.Id : null) && _coreRuleQueriesOperations.CanSpendSlashUse(actor, recipient, ignoresCount: true, CardKind.Slash, card, excludedEquipmentId: from.Zone == CardZoneKind.Equipment ? card.Id : null, physicalCardIds: [card.Id]);
    }

    internal Card[] SourceTiepaoMaterials(int owner, int target, CardConversionSource source) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(c => SourceTiepaoMaterialLegal(owner, target, c, source)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24780, SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation)]
    internal bool CanOfferSourceTiepao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => SourceTiepaoDamage(c, w) && SourceTiepaoMaterials(c.OwnerSeat, w.SourceSeat!.Value, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId)).Length != 0;
    internal SkillProgramStepOutcome BeginShanheSourceTiepao(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceTiepao is not null || f.WindowContext is not { } w || !SourceTiepaoDamage(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), w))
            throw new InvalidOperationException("Source14075 lost its actual foreign damage parent.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceTiepao = new(w.ParentFrameId, w.DamageFrameId!.Value, w.SourceSeat!.Value) });
        var choices = SourceTiepaoChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "铁炮：将一张牌当杀反击伤害来源。", choices.SelectMany(c => c.Cards).ToArray(), [w.SourceSeat.Value], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SourceTiepaoChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceTiepao!;
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "source-tiepao",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        var choices = SourceTiepaoMaterials(f.OwnerSeat, r.TargetSeat, SourceTiepaoConversion(f)).Select(c => new PromptChoice(new($"source-tiepao.{(f.Id)}.{(c.Id)}"), $"将【{(c.DisplayName)}】当杀使用", [c.Id], [r.TargetSeat], Args("use"))).ToList();
        choices.Add(new(new($"source-tiepao.{(f.Id)}.decline"), "不使用杀", [], [], Args("decline")));
        return Array.AsReadOnly(choices.ToArray());
    }

    [ProgramAnswer("source-tiepao")]
    internal void AnswerSourceTiepao(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceTiepao: { UseId: null, Issuing: false } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !SourceTiepaoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Source14075 lost its original private physical material choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["branch"] == "decline" || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceTiepao = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var card = _coreCardAttackStateOperations.GetAttackCard(selected.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceTiepao = r with { Issuing = true, MaterialCardId = card.Id, MaterialFrom = from, MovementBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card, CardKind.Slash, f.OwnerSeat, physicalCards: [card], countsTowardSlashLimit: false, conversionSource: SourceTiepaoConversion(f), programSkillCardUseFrameId: f.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 24780)]
    internal void BindSourceTiepaoUse(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { } action } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { ShanheSourceTiepao: { Issuing: true, UseId: null } r } f)
            return;
        if (f.SkillId != ShanheSourceTiepaoComposition.SkillId || use.CardId != r.MaterialCardId || use.CardKind != CardKind.Slash || action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || !action.TargetSeats.SequenceEqual([r.TargetSeat]) || !action.ConversionChain.SequenceEqual([SourceTiepaoConversion(f)]) || action.PhysicalCards is not [var payment] || payment.CardId != r.MaterialCardId || payment.From != r.MaterialFrom)
            throw new InvalidOperationException("Source14075 must retain its original material, actor, source and fixed target.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShanheSourceTiepao = r with { Issuing = false, UseId = use.Id, ActionId = action.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceTiepaoIssuedEvent(f.Id, r.DamageId, use.Id, action.ActionId, f.OwnerSeat, r.TargetSeat, payment.CardId, payment.From));
    }

    [NativeUseReturn(24780)]
    internal void ReturnSourceTiepaoUse(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceTiepao: { } r } f || r.UseId != use.Id)
            return;
        if (r.Returned || use.Action?.ActionId != r.ActionId || use.CardId != r.MaterialCardId || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheSourceTiepaoIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.UseId == use.Id && e.ActionId == r.ActionId))
            throw new InvalidOperationException("Source14075 requires its exact once-completed native Slash return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceTiepao = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceTiepaoReturnedEvent(f.Id, use.Id, f.OwnerSeat, use.CausedDamage));
        if (use.CausedDamage)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceTiepaoDistanceGrantedEvent(f.Id, use.Id, f.OwnerSeat, r.TargetSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        if (use.CardAttack is not { Active: true })
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24780)]
    internal bool ResumeSourceTiepao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceTiepao: { } r } f || f.Id != id)
            return false;
        if (!r.Returned)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceTiepao = null });
        return false;
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 24780)]
    internal void SourceTiepaoDistance(ProgramDistanceQuery q, List<RuleQueryBaseTerm> terms)
    {
        if (q.Source.Seat == q.Target.Seat)
            return;
        foreach (var g in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheSourceTiepaoDistanceGrantedEvent>().Where(g => g.AffectedSeat == q.Source.Seat && g.OwnerSeat == q.Target.Seat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Any(t => t.ActorSeat == g.OwnerSeat && t.TurnNumber > g.ActualTurn)))
            terms.Add(new($"source-tiepao:{(g.ProgramFrameId)}:{(g.UseId)}", 1));
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation)]
    internal PromptChoice AiSourceTiepao(PendingDecision p) => p.Choices.First();
}

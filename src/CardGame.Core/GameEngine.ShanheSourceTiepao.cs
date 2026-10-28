using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheSourceTiepaoHost(GameEngine engine) : IShanheSourceTiepaoHost
    { public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => engine.BeginShanheSourceTiepao(f); }

    private IDamageAttempt? SourceTiepaoOriginalAttack(DamageTriggerWindowFrame window, long damageId, int amount)
    {
        if (window.ParentFrameId != damageId || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied ||
            _resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId) is not { } damage ||
            damage.SourceSeat != window.SourceSeat || damage.TargetSeat != window.TargetSeat || damage.Amount != amount || amount <= 0)
            return null;
        // Resolve the suspended owner by the original DamageFrame's parent.
        // CurrentDamageAttempt may now belong to the counter-Slash child.
        var attack = DamageArmorPaymentOwner(damage);
        return attack is { DamageWasApplied: true, IsSourceLess: false } && attack.ResolutionId == damage.ParentFrameId &&
            attack.SourceSeat == damage.SourceSeat && attack.TargetSeat == damage.TargetSeat && attack.DamageAmount == damage.Amount &&
            window.SourceCardId == attack.Card?.Id && window.SourceCard == attack.EffectiveCardKind ? attack : null;
    }

    private bool SourceTiepaoDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) =>
        c.SkillId == ShanheSourceTiepaoComposition.SkillId && w.Window == SkillProgramTriggerWindow.AfterDamageApplied &&
        w.TargetSeat == c.OwnerSeat && w.Amount > 0 && w.SourceSeat is { } source && source != c.OwnerSeat &&
        _players[source].IsAlive && w.DamageFrameId is { } damageId && ActiveDamageTrigger is { } parent &&
        parent.Id == w.ParentFrameId && parent.ParentFrameId == damageId &&
        SourceTiepaoOriginalAttack(parent, damageId, w.Amount) is { } attack &&
        attack.SourceSeat == source && attack.TargetSeat == c.OwnerSeat;

    private CardConversionSource SourceTiepaoConversion(ProgramSkillFrame f) =>
        new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private bool SourceTiepaoMaterialLegal(int owner, int target, Card card, CardConversionSource source)
    {
        var from = _cardZones.GetLocation(card.Id); var actor = _players[owner]; var recipient = _players[target];
        return owner != target && actor.IsAlive && recipient.IsAlive && from.OwnerSeat == owner &&
            from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !IsTurnPhysicalUseForbidden(owner, [card.Id]) &&
            (from.Zone != CardZoneKind.Hand || !IsTurnHandCardRestricted(actor, card)) &&
            !HasTurnCardTargetRestriction(owner, SkillProgramCardTargetRestriction.SelfOnly) &&
            CanUseSlashTarget(actor, recipient, card, source, CardKind.Slash, physicalCardIds: [card.Id],
                excludedEquipmentId: from.Zone == CardZoneKind.Equipment ? card.Id : null) &&
            CanSpendSlashUse(actor, recipient, ignoresCount: true, CardKind.Slash, card,
                excludedEquipmentId: from.Zone == CardZoneKind.Equipment ? card.Id : null, physicalCardIds: [card.Id]);
    }
    private Card[] SourceTiepaoMaterials(int owner, int target, CardConversionSource source) =>
        GetHand(_players[owner]).Concat(GetEquipment(_players[owner]))
            .Where(c => SourceTiepaoMaterialLegal(owner, target, c, source)).ToArray();

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24780,
        SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation)]
    private bool CanOfferSourceTiepao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) =>
        SourceTiepaoDamage(c, w) && SourceTiepaoMaterials(c.OwnerSeat, w.SourceSeat!.Value,
            new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId)).Length != 0;

    private SkillProgramStepOutcome BeginShanheSourceTiepao(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceTiepao is not null || f.WindowContext is not { } w ||
            !SourceTiepaoDamage(new(f.OwnerSeat, f.SkillId, GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), w))
            throw new InvalidOperationException("Source14075 lost its actual foreign damage parent.");
        ReplaceRuntimeTop(f = f with { ShanheSourceTiepao = new(w.ParentFrameId, w.DamageFrameId!.Value, w.SourceSeat!.Value) });
        var choices = SourceTiepaoChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "铁炮：将一张牌当杀反击伤害来源。",
            choices.SelectMany(c => c.Cards).ToArray(), [w.SourceSeat.Value], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> SourceTiepaoChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceTiepao!;
        Dictionary<string, string> Args(string branch) => new()
        { ["program-action"] = "source-tiepao", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch };
        var choices = SourceTiepaoMaterials(f.OwnerSeat, r.TargetSeat, SourceTiepaoConversion(f))
            .Select(c => new PromptChoice(new($"source-tiepao.{f.Id}.{c.Id}"), $"将【{c.DisplayName}】当杀使用", [c.Id], [r.TargetSeat], Args("use"))).ToList();
        choices.Add(new(new($"source-tiepao.{f.Id}.decline"), "不使用杀", [], [], Args("decline")));
        return Array.AsReadOnly(choices.ToArray());
    }
    [ProgramAnswer("source-tiepao")]
    private void AnswerSourceTiepao(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceTiepao: { UseId: null, Issuing: false } r } f ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || !SourceTiepaoChoices(f).Any(c => AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Source14075 lost its original private physical material choice.");
        ClearPendingDecision();
        if (selected.Parameters["branch"] == "decline" || _winner != Winner.None ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        { ReplaceRuntimeTop(f with { ShanheSourceTiepao = null }); AdvanceRuntimeProgram(f.Id); return; }
        var card = GetAttackCard(selected.Cards.Single()); var from = _cardZones.GetLocation(card.Id);
        ReplaceRuntimeTop(f = f with { ShanheSourceTiepao = r with { Issuing = true, MaterialCardId = card.Id,
            MaterialFrom = from, MovementBefore = _movementSequence } });
        ResolveSlashCore(_players[f.OwnerSeat], _players[r.TargetSeat], card, CardKind.Slash, f.OwnerSeat,
            physicalCards: [card], countsTowardSlashLimit: false, conversionSource: SourceTiepaoConversion(f), programSkillCardUseFrameId: f.Id);
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 24780)]
    private void BindSourceTiepaoUse(CardUseDeclaredEvent fact)
    {
        if (LifecycleCardUse(fact.ResolutionId) is not { Action: { } action } use) return;
        var index = _resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStack[index - 1] is not ProgramSkillFrame
                { ShanheSourceTiepao: { Issuing: true, UseId: null } r } f) return;
        if (f.SkillId != ShanheSourceTiepaoComposition.SkillId || use.CardId != r.MaterialCardId || use.CardKind != CardKind.Slash ||
            action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat ||
            !action.TargetSeats.SequenceEqual([r.TargetSeat]) || !action.ConversionChain.SequenceEqual([SourceTiepaoConversion(f)]) ||
            action.PhysicalCards is not [var payment] || payment.CardId != r.MaterialCardId || payment.From != r.MaterialFrom)
            throw new InvalidOperationException("Source14075 must retain its original material, actor, source and fixed target.");
        ReplaceRuntimeFrame(f.Id, f with { ShanheSourceTiepao = r with { Issuing = false, UseId = use.Id, ActionId = action.ActionId } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceTiepaoIssuedEvent(f.Id, r.DamageId, use.Id,
            action.ActionId, f.OwnerSeat, r.TargetSeat, payment.CardId, payment.From));
    }
    [NativeUseReturn(24780)]
    private void ReturnSourceTiepaoUse(CardUseFrame use)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceTiepao: { } r } f || r.UseId != use.Id) return;
        if (r.Returned || use.Action?.ActionId != r.ActionId || use.CardId != r.MaterialCardId ||
            LifecycleCardUse(use.Id) is not null || ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 ||
            !ProgramEventHistory<ShanheSourceTiepaoIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.UseId == use.Id && e.ActionId == r.ActionId))
            throw new InvalidOperationException("Source14075 requires its exact once-completed native Slash return.");
        ReplaceRuntimeTop(f with { ShanheSourceTiepao = r with { Returned = true } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceTiepaoReturnedEvent(f.Id, use.Id, f.OwnerSeat, use.CausedDamage));
        if (use.CausedDamage)
            AdvanceEventRulesAndQueueFact(new ShanheSourceTiepaoDistanceGrantedEvent(f.Id, use.Id, f.OwnerSeat, r.TargetSeat, _turnNumber));
        if (use.CardAttack is not { Active: true }) AdvanceRuntimeProgram(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 24780)]
    private bool ResumeSourceTiepao(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceTiepao: { } r } f || f.Id != id) return false;
        if (!r.Returned) return true;
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id)) return true;
        ReplaceRuntimeTop(f with { ShanheSourceTiepao = null }); return false;
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 24780)]
    private void AssertSourceTiepaoReceipt(ProgramSkillFrame f)
    {
        if (f.ShanheSourceTiepao is not { } r) return;
        if (f.SkillId != ShanheSourceTiepaoComposition.SkillId || f.InstructionIndex != 1 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } w ||
            w.TargetSeat != f.OwnerSeat || w.SourceSeat != r.TargetSeat || w.ParentFrameId != r.DamageWindowId ||
            w.DamageFrameId != r.DamageId || f.ParentFrameId != r.DamageWindowId || r.TargetSeat == f.OwnerSeat ||
            _resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(p => p.Id == r.DamageWindowId) is not { } parent ||
            parent.ParentFrameId != r.DamageId || SourceTiepaoOriginalAttack(parent, r.DamageId, w.Amount) is not { } original ||
            original.SourceSeat != r.TargetSeat || original.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Source14075 receipt lost its exact original received-damage window and participants.");
        var issued = ProgramEventHistory<ShanheSourceTiepaoIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        var returned = ProgramEventHistory<ShanheSourceTiepaoReturnedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.MaterialCardId is null)
        {
            if (r.MaterialFrom is not null || r.UseId is not null || r.ActionId is not null || r.Issuing || r.Returned ||
                r.MovementBefore != 0 || issued.Length != 0 || returned.Length != 0)
                throw new InvalidOperationException("An unpaid Source14075 choice cannot retain another native use.");
            return;
        }
        if (r.MaterialFrom is not { OwnerSeat: var owner, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } from || owner != f.OwnerSeat ||
            r.Issuing && (r.UseId is not null || r.ActionId is not null || r.Returned || issued.Length != 0 || returned.Length != 0))
            throw new InvalidOperationException("Source14075 lost its original prospective owned material.");
        if (r.Issuing) return;
        if (r.UseId is not { } useId || r.ActionId is not { } actionId ||
            issued is not [var invoice] || invoice.DamageId != r.DamageId || invoice.UseId != useId || invoice.ActionId != actionId ||
            invoice.OwnerSeat != f.OwnerSeat || invoice.TargetSeat != r.TargetSeat || invoice.CardId != r.MaterialCardId || invoice.From != from ||
            _cardMovements.Count(m => m.Sequence > r.MovementBefore && m.CardId == r.MaterialCardId && m.From == from && m.To == CardLocation.Processing &&
                m.Reason == CardMoveReasons.Use) != 1 ||
            (r.Returned ? returned is not [var completion] || completion.UseId != useId || completion.OwnerSeat != f.OwnerSeat ||
                LifecycleCardUse(useId) is not null || ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) != 1
                : returned.Length != 0 || LifecycleCardUse(useId)?.Action?.ActionId != actionId))
            throw new InvalidOperationException("Source14075 lost its exact once-paid material, native-use identity or original-frame return.");
    }
    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 24780)]
    private void SourceTiepaoDistance(ProgramDistanceQuery q, List<RuleQueryBaseTerm> terms)
    {
        if (q.Source.Seat == q.Target.Seat) return;
        foreach (var g in ProgramEventHistory<ShanheSourceTiepaoDistanceGrantedEvent>().Where(g =>
            g.AffectedSeat == q.Source.Seat && g.OwnerSeat == q.Target.Seat &&
            !ProgramEventHistory<TurnStartedEvent>().Any(t => t.ActorSeat == g.OwnerSeat && t.TurnNumber > g.ActualTurn)))
            terms.Add(new($"source-tiepao:{g.ProgramFrameId}:{g.UseId}", 1));
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceTiepaoRetaliation)]
    private PromptChoice AiSourceTiepao(PendingDecision p) => p.Choices.First();
}

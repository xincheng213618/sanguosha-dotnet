namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string OlSpZhujiu = "ol:zhujiu-3577", OlSpJinglei = "ol:jinglei-3578", OlSpBrewDisabled = "zhujiu-disabled";
    [ProgramCapability]
    private sealed class OlSpLiuBei664Capability(GameEngine engine) : IOlSpLiuBei664Host
    {
        public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op switch
        {
            SkillProgramEffectOp.UseOlSpLiuBeiZhujiu => engine.BeginOlSpZhujiu(frame),
            SkillProgramEffectOp.GrantOlSpLiuBeiXudai => engine.GrantOlSpXudai(frame),
            SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei when effect.StateId == "pay" => engine.PayOlSpJinglei(frame),
            _ => engine.BeginOlSpJingleiAdjustment(frame)
        };
    }
    private SkillProgramStepOutcome GrantOlSpXudai(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "ol:xudai-3576" || frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat || !_players[recipient].IsAlive)
            throw new InvalidOperationException("Xudai requires its original other recipient on the paid limited frame.");
        // Shared mutation and original paid frame; a limited grant is not an awakening.
        AcquireProgramSkillChange(PrepareProgramSkillChange(frame) with { Owner = _players[recipient] }, [OlSpZhujiu], announceAwakening: false);
        return SkillProgramStepOutcome.Continue;
    }

    private int OlSpAlcoholCount(int owner)
    {
        var events = CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(events, item => item is TurnStartedEvent turn && turn.TurnNumber == _turnNumber && turn.ActorSeat == _currentSeat);
        // Declaration facts cover physical, converted, quota-exempt and dying Alcohol Uses.
        return start < 0 ? 0 : events.Skip(start + 1).OfType<CardUseDeclaredEvent>()
            .Where(item => item.SourceSeat == owner && item.CardKind == CardKind.Alcohol).Select(item => item.ResolutionId).Distinct().Count();
    }
    private Card[] OlSpBrewMaterials(int owner) => GetHand(_players[owner]).Concat(GetEquipment(owner))
        .Where(card => !card.IsGeneralWeapon && !IsTurnHandCardRestricted(_players[owner], card) &&
            TargetRules.AllowsNativeUseMaterial(new(owner, CardKind.Alcohol, [card.Id], null))).ToArray();
    private bool CanOlSpBrew(CharacterState owner) => owner.IsAlive && _winner == Winner.None && _currentSeat == owner.Seat && _phase == TurnPhase.Play &&
        !owner.HasAlcoholEffect && CanUseJianjiOrdinaryAlcohol(owner, null) && !IsCardUseForbidden(owner.Seat, CardKind.Alcohol, CardActionType.Use) &&
        !IsDirectedCardTargetProhibited(owner.Seat, owner.Seat, CardKind.Alcohol) &&
        _skillRuntimeState.GetUsage(owner.Seat, OlSpZhujiu, OlSpBrewDisabled, SkillUsageScope.Turn) == 0 && OlSpBrewMaterials(owner.Seat).Length > OlSpAlcoholCount(owner.Seat);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 5431, SkillProgramEffectOp.UseOlSpLiuBeiZhujiu)]
    private bool CanOfferOlSpBrew(CharacterState owner) => CanOlSpBrew(owner);
    private bool CanOlSpDyingBrew(int owner, ProgramSkillWindowContext? context) => ActiveDying is { } dying &&
        context is { Window: SkillProgramTriggerWindow.SelfDyingResponse } && context.ParentFrameId == dying.Id &&
        context.TargetSeat == owner && dying.VictimSeat == owner && dying.ResponderSeat == owner &&
        _players[owner].IsAlive && _players[owner].Hp <= 0 && _winner == Winner.None &&
        !MouGuquPhysicalUseForbidden(owner, []) && !IsForeignTurnAlcoholUseForbidden(owner, CardKind.Alcohol, CardActionType.Use) &&
        !IsCardUseForbidden(owner, CardKind.Alcohol, CardActionType.Use) && !IsDirectedCardTargetProhibited(owner, owner, CardKind.Alcohol) &&
        _skillRuntimeState.GetUsage(owner, OlSpZhujiu, OlSpBrewDisabled, SkillUsageScope.Turn) == 0 && OlSpBrewMaterials(owner).Length > OlSpAlcoholCount(owner);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5431)]
    private bool CanOfferOlSpDyingBrew(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.UseOlSpLiuBeiZhujiu) ||
        candidate.SkillId == OlSpZhujiu && CanOlSpDyingBrew(candidate.OwnerSeat, context);
    private SkillProgramStepOutcome BeginOlSpZhujiu(ProgramSkillFrame frame)
    {
        var dying = frame.WindowContext?.Window == SkillProgramTriggerWindow.SelfDyingResponse;
        if (frame.OlSpLiuBei664 is not null || frame.SkillId != OlSpZhujiu ||
            !(dying ? CanOlSpDyingBrew(frame.OwnerSeat, frame.WindowContext) : CanOlSpBrew(_players[frame.OwnerSeat])))
            throw new InvalidOperationException("Zhujiu requires its current unpaid ordinary Alcohol allowance.");
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = new() { Minimum = checked(OlSpAlcoholCount(frame.OwnerSeat) + 1), TurnNumber = _turnNumber,
            DyingFrameId = dying ? ActiveDying!.Id : null } });
        PublishOlSpLiuBeiChoice(frame); return SkillProgramStepOutcome.AwaitChoice;
    }

    private bool HasOlSpZhujiu(int seat) => _players[seat].IsAlive && _players[seat].SkillGrants.Grants.Any(grant => grant.SkillId == OlSpZhujiu);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5432)]
    private bool CanOfferOlSpJinglei(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei && effect.StateId == "pay") ||
        candidate.SkillId == OlSpJinglei && ActiveDying is null && !_players.Any(player => player.IsAlive && player.Hp <= 0) &&
        _players.Any(player => HasOlSpZhujiu(player.Seat)) && context.Window == SkillProgramTriggerWindow.CardUseCompleted &&
        _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(window => window.Id == context.ParentFrameId &&
            window.Action.Type == CardActionType.Use && window.Action.EffectiveKind == CardKind.Alcohol);
    private SkillProgramStepOutcome PayOlSpJinglei(ProgramSkillFrame frame)
    {
        if (frame.SkillId != OlSpJinglei || frame.InstructionIndex != 1 || frame.OlSpLiuBei664 is not null || ActiveDying is not null ||
            _players.Any(player => player.IsAlive && player.Hp <= 0) || frame.WindowContext is not { } context ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId) is not { } parent ||
            parent.Action.Type != CardActionType.Use || parent.Action.EffectiveKind != CardKind.Alcohol)
            throw new InvalidOperationException("Jinglei requires its actual completed Alcohol and no current dying character.");
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = new() { Stage = "damage", TurnNumber = _turnNumber, UseId = parent.ParentFrameId } });
        AdvanceEventRulesAndQueueFact(new OlSpLiuBeiJingleiPaidEvent(frame.Id, frame.OwnerSeat, parent.ParentFrameId));
        return BeginProgramSkillDamage(frame, frame.OwnerSeat, 1, nature: DamageNature.Thunder, sourceLess: true);
    }
    private SkillProgramStepOutcome BeginOlSpJingleiAdjustment(ProgramSkillFrame frame)
    {
        if (frame.OlSpLiuBei664 is not { Stage: "damage" } || frame.InstructionIndex != 2 || frame.AttackAttempt is not null ||
            ProgramEventHistory<OlSpLiuBeiJingleiPaidEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat) != 1)
            throw new InvalidOperationException("Jinglei's adjustment requires its original already-resolved source-less thunder payment.");
        if (!_players.Any(player => HasOlSpZhujiu(player.Seat))) return SkillProgramStepOutcome.Continue;
        if (!_players[frame.OwnerSeat].IsAlive)
        {
            BeginOlSpHandAdjustment(frame, _players.First(player => HasOlSpZhujiu(player.Seat)).Seat);
            return SkillProgramStepOutcome.AwaitChild;
        }
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664 with { Stage = "recipient" } });
        PublishOlSpLiuBeiChoice(frame); return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> OlSpLiuBeiChoices(ProgramSkillFrame frame)
    {
        var draft = frame.OlSpLiuBei664!;
        PromptChoice Choice(string id, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) =>
            new(new($"sp-liubei.{frame.Id}.{id}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "ol-sp-liubei-664", ["stage"] = draft.Stage });
        if (draft.Stage == "recipient") return _players.Where(player => HasOlSpZhujiu(player.Seat))
            .Select(player => Choice("recipient." + player.Seat, "令" + player.Name + "调整手牌", targets: [player.Seat])).ToArray();
        if (draft.Stage == "discard") return GetHand(_players[draft.TargetSeat!.Value]).Where(card => !draft.Cards.Contains(card.Id))
            .Select(card => Choice("discard." + card.Id, "弃置【" + card.DisplayName + $"】（{draft.Cards.Count + 1}/{draft.Minimum}）", [card.Id])).ToArray();
        if (draft.Stage != "materials") throw new InvalidOperationException("SP Liu Bei has no unpaid selection at this stage.");
        var choices = OlSpBrewMaterials(frame.OwnerSeat).Where(card => !draft.Cards.Contains(card.Id))
            .Select(card => Choice("material." + card.Id, "选择【" + card.DisplayName + "】煮酒", [card.Id])).ToList();
        if (draft.Cards.Count >= draft.Minimum) choices.Add(Choice("use", $"以{draft.Cards.Count}张牌使用酒"));
        choices.Add(Choice("cancel", "取消煮酒")); return choices;
    }
    private void PublishOlSpLiuBeiChoice(ProgramSkillFrame frame)
    {
        var choices = OlSpLiuBeiChoices(frame); var draft = frame.OlSpLiuBei664!;
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Choices = choices } });
        var chooser = draft.Stage == "discard" ? draft.TargetSeat!.Value : frame.OwnerSeat;
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, chooser, draft.Stage == "materials" ? $"煮酒：选择至少{draft.Minimum}张牌。" :
            draft.Stage == "recipient" ? "惊雷：选择一名拥有煮酒的角色。" : "惊雷：弃置多于体力上限的手牌。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = choices, IsPrivate = draft.Stage != "recipient", SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.UseOlSpLiuBeiZhujiu, SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei)]
    private PromptChoice SelectAiOlSpLiuBei664(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (frame.OlSpLiuBei664 is not { } draft || _resolutionStack.LastOrDefault()?.Id != frame.Id ||
            decision.Kind != DecisionKind.ProgramTrigger || _pendingDecision?.PromptId != decision.PromptId ||
            decision.PlayerSeat != (draft.Stage == "discard" ? draft.TargetSeat : frame.OwnerSeat))
            throw new InvalidOperationException("SP Liu Bei AI lost its actual issued chooser and original program frame.");
        var current = OlSpLiuBeiChoices(frame);
        var legal = decision.Choices.Where(choice => draft.Choices.Any(issued => AssistedChoicesEqual([issued], [choice])) &&
            current.Any(candidate => AssistedChoicesEqual([candidate], [choice]))).ToArray();
        if (legal.Length == 0) throw new InvalidOperationException("SP Liu Bei AI has no currently legal issued choice.");
        if (draft.Stage == "recipient")
        {
            int Benefit(PromptChoice choice)
            {
                var seat = choice.Targets.Single(); var target = _players[seat]; var hand = GetHand(target).Count;
                var draw = Math.Max(0, Math.Min(5, target.MaxHp) - hand); var discard = Math.Max(0, hand - target.MaxHp);
                return seat == frame.OwnerSeat ? 2 * draw - discard :
                    discard + (OtherRevealedPublicTeamAlly(_players[frame.OwnerSeat], target) ? 2 * draw : -2 * draw);
            }
            return legal.OrderByDescending(Benefit).ThenBy(choice => choice.Targets.Single()).First();
        }
        if (draft.Stage == "discard")
            return legal.Where(choice => choice.Cards.Count == 1)
                .OrderBy(choice => CardCatalog.Get(GetAttackCard(choice.Cards[0]).Kind).HandKeepValue)
                .ThenBy(choice => choice.Cards[0]).First();
        if (draft.Stage != "materials") throw new InvalidOperationException("SP Liu Bei AI cannot choose during a paid native child.");
        var canPay = draft.DyingFrameId is not null ? CanOlSpDyingBrew(frame.OwnerSeat, frame.WindowContext) : CanOlSpBrew(_players[frame.OwnerSeat]);
        var materials = legal.Where(choice => choice.Cards.Count == 1).ToArray();
        if (!canPay || draft.Minimum - draft.Cards.Count > materials.Length)
            return legal.Single(choice => choice.Id.Value.EndsWith(".cancel", StringComparison.Ordinal));
        if (draft.Cards.Count >= draft.Minimum)
            return legal.Single(choice => choice.Id.Value.EndsWith(".use", StringComparison.Ordinal));
        return materials.OrderByDescending(choice => EffectiveSuit(_players[frame.OwnerSeat], GetAttackCard(choice.Cards[0])) == Suit.Club)
            .ThenBy(choice => CardCatalog.Get(GetAttackCard(choice.Cards[0]).Kind).HandKeepValue)
            .ThenBy(choice => choice.Cards[0]).First();
    }

    [ProgramAnswer("ol-sp-liubei-664")]
    private void AnswerOlSpLiuBei(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { OlSpLiuBei664: { } draft } frame ||
            draft.Choices.SingleOrDefault(choice => choice.Id == selected.Id) is not { } choice || !AssistedChoicesEqual([choice], [selected]) ||
            !OlSpLiuBeiChoices(frame).Any(current => AssistedChoicesEqual([choice], [current])))
            throw new InvalidOperationException("SP Liu Bei lost its issued selection and current owned entities.");
        ClearPendingDecision();
        if (draft.Stage == "recipient") { BeginOlSpHandAdjustment(frame, choice.Targets.Single()); return; }
        if (choice.Cards is [var card])
        {
            ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Cards = draft.Cards.Append(card).ToArray() } });
            if (draft.Stage == "discard" && frame.OlSpLiuBei664!.Cards.Count == draft.Minimum) { CommitOlSpJingleiDiscard(frame); return; }
            PublishOlSpLiuBeiChoice(frame); return;
        }
        if (choice.Id.Value.EndsWith(".cancel", StringComparison.Ordinal)) { ReplaceRuntimeTop(frame with { OlSpLiuBei664 = null }); FinishProgramSkill(GetActiveProgramFrame(frame.Id), false); return; }
        CommitOlSpZhujiu(frame);
    }

    private void CommitOlSpZhujiu(ProgramSkillFrame frame)
    {
        var draft = frame.OlSpLiuBei664!; var actor = _players[frame.OwnerSeat];
        var dying = draft.DyingFrameId is not null;
        if (!(dying ? CanOlSpDyingBrew(actor.Seat, frame.WindowContext) && ActiveDying?.Id == draft.DyingFrameId : CanOlSpBrew(actor)) ||
            draft.TurnNumber != _turnNumber || draft.Minimum != OlSpAlcoholCount(actor.Seat) + 1 || draft.Cards.Count < draft.Minimum ||
            draft.Cards.Distinct().Count() != draft.Cards.Count || draft.Cards.Any(id => !OlSpBrewMaterials(actor.Seat).Any(card => card.Id == id)))
            throw new InvalidOperationException("Zhujiu's exact materials or ordinary Wine allowance changed before payment.");
        var cards = draft.Cards.Select(id => GetAttackCard(id)).ToArray();
        var allClub = cards.All(card => EffectiveSuit(actor, card) == Suit.Club);
        var source = new CardConversionSource(frame.SkillId, frame.ActivationId!, actor.Seat, frame.SkillInstanceId);
        var id = BeginCardUse(cards[0], actor.Seat, [actor.Seat], CardKind.Alcohol, physicalCardIds: draft.Cards, conversionSource: source);
        ReplaceRuntimeFrame(frame.Id, frame with { OlSpLiuBei664 = draft with { Stage = "wine", AllClub = allClub, UseId = id,
            SequenceBefore = _movementSequence, Choices = [] } });
        AdvanceEventRulesAndQueueFact(new OlSpLiuBeiZhujiuIssuedEvent(frame.Id, actor.Seat, id, draft.Minimum, cards.Length, allClub, _turnNumber));
        foreach (var material in cards) MoveCard(material, _cardZones.GetLocation(material.Id), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        var paid = _resolutionStack.OfType<ProgramSkillFrame>().Single(parent => parent.Id == frame.Id);
        ReplaceRuntimeFrame(frame.Id, paid with { OlSpLiuBei664 = paid.OlSpLiuBei664! with { SequenceAfter = _movementSequence } });
        if (!dying) RecordMouZhaJiangAwareAlcoholDebit(id, actor);
        BeginSimpleCardUse(id, new(cards[0].Id, dying ? SimpleCardUseEffect.Recovery : SimpleCardUseEffect.Alcohol, 1));
    }
    [NativeUseReturn(5431)]
    private bool ReturnOlSpZhujiu(CardUseFrame use)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { OlSpLiuBei664: { Stage: "wine" } draft } frame || draft.UseId != use.Id) return false;
        if (frame.SkillId != OlSpZhujiu || use.CardKind != CardKind.Alcohol || use.SourceSeat != frame.OwnerSeat ||
            use.Action?.Type != CardActionType.Use || !(use.PhysicalCardIds ?? []).SequenceEqual(draft.Cards) ||
            ProgramEventHistory<OlSpLiuBeiZhujiuIssuedEvent>().Count(item => item.FrameId == frame.Id && item.UseId == use.Id) != 1)
            throw new InvalidOperationException("Zhujiu lost its original paid native multi-entity Wine return.");
        if (!draft.AllClub && draft.TurnNumber == _turnNumber)
        {
            _skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, OlSpZhujiu, OlSpBrewDisabled, SkillUsageScope.Turn, 1);
            AdvanceEventRulesAndQueueFact(new OlSpLiuBeiZhujiuDisabledEvent(frame.Id, frame.OwnerSeat, _turnNumber));
        }
        AdvanceEventRulesAndQueueFact(new OlSpLiuBeiZhujiuReturnedEvent(frame.Id, frame.OwnerSeat, use.Id, draft.DyingFrameId));
        ReplaceRuntimeTop(frame with { OlSpLiuBei664 = null }); AdvanceRuntimeProgram(frame.Id); return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 5431)]
    private bool ReturnOlSpZhujiuDyingBinding(ProgramSkillFrame frame, bool completed)
    {
        if (!completed || frame.SkillId != OlSpZhujiu || frame.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context ||
            ProgramEventHistory<OlSpLiuBeiZhujiuReturnedEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.DyingFrameId == context.ParentFrameId) != 1)
            return false;
        // Reuse the native dying return; only this already-paid, completed conversion
        // can re-enter the same dying occurrence with the now-increased material price.
        CompleteDyingProgramBinding(frame, completed);
        if (ActiveDying is { } dying && dying.Id == context.ParentFrameId && dying.VictimSeat == frame.OwnerSeat && _players[frame.OwnerSeat].Hp <= 0)
        {
            var key = SelfDyingBindingKey(frame.SkillId, frame.TriggerId!, frame.SkillInstanceId);
            UpdateDyingFrame(dying.Id, current => current with { AttemptedSelfDyingBindings = current.AttemptedSelfDyingBindings.Where(binding => binding != key).ToArray() });
            ClearPendingDecision(); SetDyingFrameStep(dying.Id, ResolutionFrameStep.AwaitingResponse); _status = EngineStatus.Running;
            ExposeHumanDyingPrompt();
        }
        return true;
    }

    private void BeginOlSpHandAdjustment(ProgramSkillFrame frame, int target)
    {
        if (!HasOlSpZhujiu(target)) throw new InvalidOperationException("Jinglei requires its selected living Zhujiu holder.");
        var hand = GetHand(_players[target]).Count; var maximum = _players[target].MaxHp;
        var draw = Math.Max(0, Math.Min(5, maximum) - hand);
        if (hand > maximum)
        {
            ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664! with { Stage = "discard", TargetSeat = target, Minimum = hand - maximum, Cards = [], Choices = [] } });
            PublishOlSpLiuBeiChoice(frame); return;
        }
        if (draw == 0) { CompleteOlSpAdjustment(frame); return; }
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664! with { Stage = "drawing", TargetSeat = target, Minimum = draw, Choices = [] } });
        BeginOperationProgramDraw(frame, target, draw, new("skill.ol-sp-liu-bei.jinglei.draw"));
    }
    private void CommitOlSpJingleiDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.OlSpLiuBei664!; var target = draft.TargetSeat!.Value;
        if (draft.Cards.Count != draft.Minimum || draft.Cards.Any(id => !GetHand(_players[target]).Any(card => card.Id == id))) throw new InvalidOperationException("Jinglei lost its selected actual hand discards.");
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Stage = "discarding", SequenceBefore = _movementSequence, Choices = [] } });
        foreach (var card in draft.Cards.Select(GetAttackCard).ToArray()) MoveCard(card, CardLocation.Hand(target), CardLocation.DiscardPile, new("skill.ol-sp-liu-bei.jinglei.discard"));
        frame = GetActiveProgramFrame(frame.Id); ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = frame.OlSpLiuBei664! with { SequenceAfter = _movementSequence } });
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) ContinueOlSpAdjustment(frame.Id);
    }
    private void ContinueOlSpAdjustment(long id)
    {
        var frame = GetActiveProgramFrame(id); var draft = frame.OlSpLiuBei664!;
        if (draft.Stage == "discarding" && draft.TargetSeat != frame.OwnerSeat && _players[frame.OwnerSeat].IsAlive)
        {
            if (draft.Cards.Any(card => _cardMovements.Count(move => move.Sequence > draft.SequenceBefore && move.Sequence <= draft.SequenceAfter && move.CardId == card &&
                move.From == CardLocation.Hand(draft.TargetSeat!.Value) && move.To == CardLocation.DiscardPile && move.Reason.Value == "skill.ol-sp-liu-bei.jinglei.discard") != 1))
                throw new InvalidOperationException("Jinglei can transfer only the exact entities actually discarded by this paid adjustment.");
            ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Stage = "giving" }, PendingMovementContinuation = null });
            foreach (var card in draft.Cards.Where(card => _cardZones.GetLocation(card) == CardLocation.DiscardPile).Select(GetAttackCard).ToArray())
                MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat), new("skill.ol-sp-liu-bei.jinglei.give"));
            if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.AwaitChild) return;
        }
        CompleteOlSpAdjustment(GetActiveProgramFrame(id));
    }
    private void CompleteOlSpAdjustment(ProgramSkillFrame frame)
    { ReplaceRuntimeTop(frame with { OlSpLiuBei664 = null, PendingMovementContinuation = null }); AdvanceRuntimeProgram(frame.Id); }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei)]
    private void ReturnOlSpJingleiDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.OlSpLiuBei664 is not { Stage: "drawing" } draft || outcome.RecipientSeat != draft.TargetSeat || outcome.RequestedCount != draft.Minimum)
            throw new InvalidOperationException("Jinglei's draw lost its selected holder and paid bounded refill.");
        ReplaceRuntimeTop(frame = frame with { OlSpLiuBei664 = draft with { Stage = "drawing-return" } });
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) CompleteOlSpAdjustment(frame);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 5431)]
    private bool IsOlSpJingleiMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) =>
        effect?.Op == SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei && frame.OlSpLiuBei664 is { Stage: "discarding" or "giving" or "drawing-return" } &&
        movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Resume, 5431)]
    private bool ResumeOlSpJingleiMovement(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { OlSpLiuBei664.Stage: "discarding" or "giving" or "drawing-return" } frame || frame.Id != id) return false;
        ContinueOlSpAdjustment(id); return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5431)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5431)]
    private bool CanContinuePaidOlSpLiuBei664(ProgramSkillFrame frame) => frame.OlSpLiuBei664 is { } draft &&
        (frame.SkillId == OlSpZhujiu && draft.Stage == "wine" && ProgramEventHistory<OlSpLiuBeiZhujiuIssuedEvent>()
            .Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.UseId == draft.UseId && item.PaidCount == draft.Cards.Count) == 1 ||
         frame.SkillId == OlSpJinglei && ProgramEventHistory<OlSpLiuBeiJingleiPaidEvent>()
            .Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.AlcoholUseId == draft.UseId) == 1);

    private bool IsOlSpZhujiuDyingRide(int index, DyingFrame dying)
    {
        if (index < 0 || index + 2 >= _resolutionStack.Count || _resolutionStack[index + 1] is not ProgramSkillFrame
                { SkillId: OlSpZhujiu, OlSpLiuBei664: { Stage: "wine" } draft } frame ||
            frame.OwnerSeat != dying.VictimSeat || dying.ResponderSeat != frame.OwnerSeat || draft.DyingFrameId != dying.Id ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dying.Id ||
            _resolutionStack[index + 2] is not CardUseFrame use || use.Id != draft.UseId || use.SourceSeat != frame.OwnerSeat || use.CardKind != CardKind.Alcohol ||
            use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != frame.OwnerSeat ||
            !(use.PhysicalCardIds ?? []).SequenceEqual(draft.Cards) || !action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(draft.Cards) ||
            draft.SequenceAfter <= draft.SequenceBefore || action.PhysicalCards.Any(cost => cost.From.OwnerSeat != frame.OwnerSeat ||
                cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardMovements.Count(move =>
                    move.Sequence > draft.SequenceBefore && move.Sequence <= draft.SequenceAfter && move.CardId == cost.CardId && move.From == cost.From &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) != 1) ||
            ProgramEventHistory<OlSpLiuBeiZhujiuIssuedEvent>().Count(item => item.FrameId == frame.Id && item.UseId == use.Id && item.PaidCount == draft.Cards.Count) != 1) return false;
        for (var cursor = index + 3; cursor < _resolutionStack.Count; cursor++)
        {
            var child = _resolutionStack[cursor]; var parent = _resolutionStack[cursor - 1];
            if (child is CardsMovedTriggerWindowFrame moved && parent.Id == use.Id && moved.Batch.ParentFrameId == use.Id &&
                moved.Batch.Movements.All(move => draft.Cards.Contains(move.CardId) && _cardMovements.Contains(move) &&
                    (move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use ||
                     move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished))) continue;
            if (!ParticipantHandRescueObserverRide(child, parent, use) && !PreventionDrawObserverEdge(cursor)) return false;
        }
        return true;
    }
}

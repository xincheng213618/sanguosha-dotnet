using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiZhLiezhi = ShiZangHongComposition.LiezhiSkill,
        ShiZhJuegu = ShiZangHongComposition.JueguSkill;

    [ProgramCapability]
    private sealed class ShiZangHongCapability(GameEngine engine) : IShiZangHongHost
    {
        public SkillProgramStepOutcome ResolveLiezhi(ProgramSkillFrame frame) => engine.BeginShiZhLiezhi(frame);
        public SkillProgramStepOutcome ResolveJuegu(ProgramSkillFrame frame) => engine.BeginShiZhJuegu(frame);
    }

    // ---- 烈志: once per turn the owner pays one maximum HP to virtually use a
    // Peach or Wine. The shared zero-entity virtual-basic machinery issues the
    // use; this skill only narrows the offered options to Peach and Wine.

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68710, SkillProgramEffectOp.ResolveShiZhLiezhi)]
    private bool CanOfferShiZhLiezhiActivation(CharacterState owner, string skill) =>
        skill != ShiZhLiezhi || ShiZhLiezhiOptions(owner.Seat).Any();

    private SkillProgramStepOutcome BeginShiZhLiezhi(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ZangHong is not null)
            throw new InvalidOperationException("Shi Zang Hong restarted a running operation.");
        if (f.SkillId != ShiZhLiezhi || f.TriggerId is not null || f.SelectedCardIds.Count != 0 ||
            f.SelectedTargetSeats.Count != 0 || !_players[f.OwnerSeat].IsAlive || _winner != Winner.None ||
            _phase != TurnPhase.Play || _currentSeat != f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        var options = ShiZhLiezhiOptions(f.OwnerSeat).ToArray();
        if (options.Length == 0)
            return SkillProgramStepOutcome.Continue;
        ChangeProgramMaximumHp(f, -1);
        AdvanceEventRulesAndQueueFact(new ShiZhLiezhiPaidEvent(f.Id, f.OwnerSeat, _players[f.OwnerSeat].MaxHp));
        PublishShiZhLiezhi(GetActiveProgramFrame(f.Id), options);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IEnumerable<(CardKind Kind, int Target)> ShiZhLiezhiOptions(int owner) =>
        VirtualBasicOptions(owner, anyRegisteredBasic: true)
            .Where(option => option.Kind is CardKind.Peach or CardKind.Alcohol);

    private void PublishShiZhLiezhi(ProgramSkillFrame f, (CardKind Kind, int Target)[] options)
    {
        var skill = _contentRegistry.GetSkill(f.SkillId);
        var choices = options.Select(option => new PromptChoice(
            new ChoiceId($"virtual-basic.{f.Id}.{option.Kind}:{option.Target}"),
            $"视为使用【{CardCatalog.Get(option.Kind).DisplayName}】",
            [], [option.Target],
            new Dictionary<string, string>
            {
                ["program-action"] = "virtual-basic",
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["basic-option"] = $"{option.Kind}:{option.Target}",
            })).ToArray();
        ReplaceRuntimeTop(f with { VirtualBasicDraft = new(f.InstructionIndex) });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            $"【{skill.Name}】：请选择视为使用的基本牌。", [], options.Select(option => option.Target).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices)
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    // ---- 据孤: after becoming a card target while unwounded, draw two cards
    // and then discard as many cards as this skill resolved this turn.

    private SkillProgramStepOutcome BeginShiZhJuegu(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ZangHong is not null)
            throw new InvalidOperationException("Shi Zang Hong restarted a running operation.");
        if (f.SkillId != ShiZhJuegu || f.TriggerId != ShiZangHongComposition.JueguTrigger ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        AdvanceEventRulesAndQueueFact(new ShiZhJueguUsedEvent(f.Id, f.OwnerSeat, _turnNumber));
        var discardCount = ProgramEventHistory<ShiZhJueguUsedEvent>()
            .Count(entry => entry.OwnerSeat == f.OwnerSeat && entry.Turn == _turnNumber);
        DrawCards(_players[f.OwnerSeat], 2, log: true);
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
        { ZangHong = new("juegu", ShiZhStage.Discarding) { DiscardCount = discardCount } });
        if (DrainShiZh(GetActiveProgramFrame(f.Id)))
            return SkillProgramStepOutcome.AwaitChild;
        ContinueShiZh(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void ContinueShiZh(ProgramSkillFrame f)
    {
        var r = f.ZangHong;
        if (r is null) return;
        var hand = GetHand(_players[f.OwnerSeat]);
        var remaining = Math.Min(r.DiscardCount - r.Discarded, hand.Count);
        if (remaining <= 0)
        { FinishShiZhOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        var choices = ShiZhDiscardChoices(f);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            $"【{skill.Name}】：请弃置 {remaining} 张手牌。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiZangHongComposition.ProgramAction)]
    private void AnswerShiZh(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ZangHong: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiZhDiscardChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Zang Hong choice lost its current frame or legal card.");
        ClearPendingDecision();
        var cardId = selected.Cards.Single();
        var from = _cardZones.GetLocation(cardId);
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        MoveCard(GetAdvancedCard(cardId), from, CardLocation.DiscardPile,
            new CardMoveReason(ShiZangHongComposition.JueguDiscardReason), disclosure: CardMovementDisclosure.FaceUp);
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ZangHong = r with { Discarded = r.Discarded + 1 } });
        if (DrainShiZh(GetActiveProgramFrame(f.Id)))
            return;
        ContinueShiZh(GetActiveProgramFrame(f.Id));
    }

    private IReadOnlyList<PromptChoice> ShiZhDiscardChoices(ProgramSkillFrame f) =>
        GetHand(_players[f.OwnerSeat]).Select(card => new PromptChoice(
            new ChoiceId($"shi-zang-hong.{f.Id}.discard-{card.Id}"),
            $"弃置【{card.DisplayName}】", [card.Id], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiZangHongComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = $"discard-{card.Id}",
            })).ToArray();

    private void FinishShiZhOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ZangHong = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiZh(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68700)]
    private bool ResumeShiZh(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ZangHong: { } } f || f.Id != id) return false;
        if (DrainShiZh(GetActiveProgramFrame(id))) return true;
        ContinueShiZh(GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68700)]
    private bool ReturnShiZhMovement(ProgramSkillFrame f) => f.ZangHong is not null && ResumeShiZh(f.Id);

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZhLiezhi)]
    private PromptChoice ChooseShiZhLiezhiAi(PendingDecision decision, ProgramSkillFrame f) =>
        decision.Choices.FirstOrDefault(choice =>
            choice.Parameters.GetValueOrDefault("basic-option")?.StartsWith("peach:", StringComparison.Ordinal) == true) ??
        decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZhJuegu)]
    private PromptChoice ChooseShiZhJueguAi(PendingDecision decision, ProgramSkillFrame f) =>
        decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("action") == "activate") ??
        decision.Choices[0];
}

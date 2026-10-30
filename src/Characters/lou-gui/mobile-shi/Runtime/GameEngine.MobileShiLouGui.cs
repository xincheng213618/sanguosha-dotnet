using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiLgGuanSha = ShiLouGuiComposition.GuanShaSkill, ShiLgJiYu = ShiLouGuiComposition.JiYuSkill;

    private bool HasShiLgCapability =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiLgGuanSha);

    [ProgramCapability]
    private sealed class ShiLouGuiCapability(GameEngine engine) : IShiLouGuiHost
    {
        public SkillProgramStepOutcome ResolveGuanSha(ProgramSkillFrame frame) => engine.BeginShiLgGuanSha(frame);
        public SkillProgramStepOutcome ResolveJiYu(ProgramSkillFrame frame) => engine.BeginShiLgJiYu(frame);
    }

    // ---- 灌沙: once per game at own play ending, replace every card with the
    // same number of random basic cards and raise this turn's hand limit by the
    // number of distinct gained names.

    private SkillProgramStepOutcome BeginShiLgGuanSha(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ShiLouGui is not null)
            throw new InvalidOperationException("Shi Lou Gui restarted a running operation.");
        if (f.SkillId != ShiLgGuanSha || f.TriggerId != ShiLouGuiComposition.GuanShaTrigger ||
            _winner != Winner.None || !_players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { ShiLouGui = new("guansha", _turnNumber, ShiLgStage.GuanShaAsk) });
        PublishShiLg(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiLgGuanSha(ProgramSkillFrame f)
    {
        var owner = _players[f.OwnerSeat];
        var owned = GetHand(owner).Select(card => card.Id)
            .Concat(GetEquipment(owner).Select(card => card.Id)).ToArray();
        foreach (var cardId in owned)
        {
            var card = _cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
            MoveCard(card, _cardZones.GetLocation(cardId), CardLocation.DiscardPile,
                new(ShiLouGuiComposition.GuanShaReplaceReason));
        }
        var gained = new List<int>();
        var names = new HashSet<CardKind>();
        while (gained.Count < owned.Length)
        {
            var candidates = _cardZones.CardsAt(CardLocation.DrawPile)
                .Where(card => CardCatalog.Get(card.Kind).CategoryName == "基本牌").ToArray();
            if (candidates.Length == 0) break;
            var card = candidates[_random.Next(candidates.Length)];
            MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat),
                new(ShiLouGuiComposition.GuanShaGainReason));
            gained.Add(card.Id);
            names.Add(ProgramBasicCardName(card.Kind));
        }
        AdvanceEventRulesAndQueueFact(new ShiLgGuanshaAppliedEvent(f.Id, f.OwnerSeat, _turnNumber,
            owned.Length, gained.Count, names.Count));
        AddLog("SkillTriggered",
            $"{owner.Name} 的“灌沙”将 {owned.Length} 张牌替换为 {gained.Count} 张随机基本牌，本回合手牌上限+{names.Count}。", f.OwnerSeat);
        FinishShiLgOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- 急御: once per play phase (twice when every earlier gained card has
    // been used), discard one hand card and gain one random card of each other
    // category.

    // Per-turn pairing by activation frame: the debit phase instance advances
    // underneath the skill, so the committed cost events are counted by turn and
    // each re-arm check reads the latest activation's own gains.
    private int ShiLgJiYuUsesThisPhase(int seat) =>
        ProgramEventHistory<ShiLgJiYuCostPaidEvent>()
            .Count(entry => entry.OwnerSeat == seat && entry.ActualTurn == _turnNumber);

    // A gained card counts as used once it is no longer in its owner's hand:
    // play resolution, equipping and zone transfers all consume it. Equipment
    // moves emit no CardMovedEvent, so the live card-zone state is the only
    // complete witness.
    private bool ShiLgJiYuGainsAllUsed(int seat)
    {
        var lastActivation = ProgramEventHistory<ShiLgJiYuCostPaidEvent>()
            .Where(entry => entry.OwnerSeat == seat && entry.ActualTurn == _turnNumber)
            .Select(entry => entry.FrameId)
            .LastOrDefault();
        if (lastActivation == 0) return false;
        var gained = ProgramEventHistory<ShiLgJiYuGainedEvent>()
            .Where(entry => entry.FrameId == lastActivation)
            .Select(entry => entry.CardId)
            .Distinct();
        return gained.All(cardId => _cardZones.GetLocation(cardId) is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner }
            || owner != seat);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68300, SkillProgramEffectOp.ResolveShiLgJiYu)]
    private bool CanStartShiLgJiYu(CharacterState owner, string skill)
    {
        if (skill != ShiLgJiYu) return true;
        return ShiLgJiYuUsesThisPhase(owner.Seat) switch
        {
            0 => true,
            1 => ShiLgJiYuGainsAllUsed(owner.Seat),
            _ => false,
        };
    }

    private SkillProgramStepOutcome BeginShiLgJiYu(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ShiLouGui is not null)
            throw new InvalidOperationException("Shi Lou Gui restarted a running operation.");
        if (f.SkillId != ShiLgJiYu || !CanStartShiLgJiYu(_players[f.OwnerSeat], f.SkillId) ||
            _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { ShiLouGui = new("jiyu", _turnNumber, ShiLgStage.JiYuCost)
        { PhaseId = _cardUseDebitPhaseInstanceId } });
        PublishShiLg(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiLgJiYuCost(ProgramSkillFrame f, int cardId)
    {
        var r = f.ShiLouGui!;
        if (_cardZones.GetLocation(cardId) is not { Zone: CardZoneKind.Hand, OwnerSeat: var seat } || seat != f.OwnerSeat)
            throw new InvalidOperationException("JiYu discarded a card outside its owner's current hand.");
        var card = _cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
        var category = CardCatalog.Get(card.Kind).CategoryName;
        MoveCard(card, _cardZones.GetLocation(cardId), CardLocation.DiscardPile, new(ShiLouGuiComposition.JiYuCostReason));
        AdvanceEventRulesAndQueueFact(new ShiLgJiYuCostPaidEvent(f.Id, f.OwnerSeat, _turnNumber, r.PhaseId, cardId));
        f = GetActiveProgramFrame(f.Id);
        var ordinal = 0;
        foreach (var other in new[] { "基本牌", "锦囊牌", "装备牌" }.Where(name => name != category))
        {
            var candidates = _cardZones.CardsAt(CardLocation.DrawPile)
                .Where(candidate => CardCatalog.Get(candidate.Kind).CategoryName == other).ToArray();
            if (candidates.Length == 0) continue;
            var picked = candidates[_random.Next(candidates.Length)];
            MoveCard(picked, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat),
                new(ShiLouGuiComposition.JiYuGainReason));
            AdvanceEventRulesAndQueueFact(new ShiLgJiYuGainedEvent(f.Id, f.OwnerSeat, _turnNumber, r.PhaseId,
                picked.Id, ++ordinal));
        }
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 弃置一张手牌，随机获得了两张其它类别的牌（急御）。", f.OwnerSeat);
        FinishShiLgOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- choice publication ---------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiLgChoices(ProgramSkillFrame f)
    {
        var r = f.ShiLouGui!;
        if (_winner != Winner.None || !_players[f.OwnerSeat].IsAlive) return [];
        switch (r.Operation, r.Stage)
        {
            case ("guansha", ShiLgStage.GuanShaAsk):
                return
                [
                    MakeShiLgChoice(f, "arm", "发动“灌沙”：将你的所有牌替换为等量随机基本牌"),
                    MakeShiLgChoice(f, "pass", "不发动“灌沙”"),
                ];
            case ("jiyu", ShiLgStage.JiYuCost):
                return GetHand(_players[f.OwnerSeat]).Select(card => new PromptChoice(
                    new($"shi-lou-gui.{f.Id}.cost.{card.Id}"),
                    $"弃置这张手牌（{DescribeShiLgCard(card)}）", [card.Id], [],
                    new Dictionary<string, string>
                    {
                        ["program-action"] = ShiLouGuiComposition.ProgramAction,
                        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                        ["branch"] = "cost", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture),
                    })).ToArray();
            default:
                return [];
        }
    }

    private PromptChoice MakeShiLgChoice(ProgramSkillFrame f, string branch, string label) =>
        new(new($"shi-lou-gui.{f.Id}.{branch}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiLouGuiComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });

    private static string DescribeShiLgCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{card.Suit}{card.Rank}";

    private void PublishShiLg(ProgramSkillFrame f)
    {
        var choices = ShiLgChoices(f);
        if (choices.Count == 0)
        { FinishShiLgOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            f.ShiLouGui!.Operation == "guansha" ? "灌沙：选择是否发动。" : "急御：选择要弃置的手牌。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiLouGuiComposition.ProgramAction)]
    private void AnswerShiLg(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiLouGui: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiLgChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Lou Gui choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("guansha", "arm"):
                ApplyShiLgGuanSha(GetActiveProgramFrame(f.Id));
                return;
            case ("guansha", "pass"):
                FinishShiLgOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("jiyu", "cost") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId):
                ApplyShiLgJiYuCost(GetActiveProgramFrame(f.Id), cardId);
                return;
            default:
                throw new InvalidOperationException("Shi Lou Gui answered an incompatible operation branch.");
        }
    }

    private void FinishShiLgOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiLouGui = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiLg(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68300)]
    private bool ResumeShiLg(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiLouGui: not null } f || f.Id != id) return false;
        if (DrainShiLg(GetActiveProgramFrame(id))) return true;
        PublishShiLg(GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68300)]
    private bool ReturnShiLgMovement(ProgramSkillFrame f) => f.ShiLouGui is not null && ResumeShiLg(f.Id);

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLgGuanSha)]
    private PromptChoice ChooseShiLgGuanShaAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLgJiYu)]
    private PromptChoice ChooseShiLgJiYuAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}

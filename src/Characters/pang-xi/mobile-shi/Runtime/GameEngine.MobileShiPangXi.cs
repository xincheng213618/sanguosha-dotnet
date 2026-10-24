using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiPxXuye = ShiPangXiComposition.XuyeSkill, ShiPxKuangxiang = ShiPangXiComposition.KuangxiangSkill;

    [ProgramCapability]
    private sealed class ShiPangXiCapability(GameEngine engine) : IShiPangXiHost
    {
        public SkillProgramStepOutcome ResolveXuye(ProgramSkillFrame frame) => engine.BeginShiPxXuye(frame);
        public SkillProgramStepOutcome ResolveKuangxiang(ProgramSkillFrame frame) => engine.BeginShiPxKuangxiang(frame);
        public SkillProgramStepOutcome ResolveRider(ProgramSkillFrame frame) => engine.BeginShiPxRider(frame);
        public SkillProgramStepOutcome ResolveSweep(ProgramSkillFrame frame) => engine.BeginShiPxSweep(frame);
    }

    // ---- facts -----------------------------------------------------------------

    private bool ShiPxIsGlobalMinimumHand(int seat)
    {
        var counts = _players.Where(player => player.IsAlive).Select(player => GetHand(player).Count).ToArray();
        return counts.Length > 0 && GetHand(_players[seat]).Count == counts.Min();
    }

    private bool ShiPxIsGlobalMaximumHand(int seat)
    {
        var counts = _players.Where(player => player.IsAlive).Select(player => GetHand(player).Count).ToArray();
        return counts.Length > 0 && GetHand(_players[seat]).Count == counts.Max();
    }

    // The active swap is derived from the committed ledger: a later swap
    // replaces the previous one, a rider answer or sweep clears it.
    private ShiPxKuangxiangSwappedEvent? ShiPxActiveSwap(int seat)
    {
        ShiPxKuangxiangSwappedEvent? swap = null;
        foreach (var payload in CompleteProgramEventHistory())
        {
            if (payload is ShiPxKuangxiangSwappedEvent { OwnerSeat: var owner } swapped && owner == seat)
                swap = swapped;
            else if (payload is ShiPxKuangxiangClearedEvent { OwnerSeat: var cleared } && cleared == seat ||
                     payload is ShiPxKuangxiangRiderEvent { OwnerSeat: var rider } && rider == seat)
                swap = null;
        }
        return swap;
    }

    private string? ShiPxRiderSide(ShiPxKuangxiangSwappedEvent swap, IReadOnlyList<CardMovementRecord> movements)
    {
        bool Emptied(IReadOnlyList<int> took, int holder) => took.Count > 0 &&
            movements.Any(movement => took.Contains(movement.CardId) && movement.From.Zone == CardZoneKind.Hand &&
                movement.From.OwnerSeat == holder) &&
            took.All(id => _cardZones.GetLocation(id) is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner } || owner != holder);
        if (Emptied(swap.OwnerTookIds, swap.OwnerSeat)) return "self";
        if (Emptied(swap.TargetTookIds, swap.TargetSeat)) return "target";
        return null;
    }

    // ---- 蓄业: once per turn, the global minimum-hand victim of damage draws
    // two; if that made their hand the global maximum, the owner tops one card
    // of their area onto the draw pile.

    private SkillProgramStepOutcome BeginShiPxXuye(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        if (f.SkillId != ShiPxXuye || f.TriggerId != ShiPangXiComposition.XuyeTrigger ||
            f.WindowContext?.TargetSeat is not { } victim || !_players[f.OwnerSeat].IsAlive ||
            !_players[victim].IsAlive || _winner != Winner.None || !ShiPxIsGlobalMinimumHand(victim) ||
            ProgramEventHistory<ShiPxXuyeDrawEvent>().Any(entry =>
                entry.OwnerSeat == f.OwnerSeat && entry.ActualTurn == _turnNumber))
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { PangXi = new("xuye", _turnNumber, ShiPxStage.XuyeAsk) { VictimSeat = victim } });
        PublishShiPx(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiPxXuyeDraw(ProgramSkillFrame f)
    {
        var r = f.PangXi!;
        DrawProgramCards(f.Id, r.VictimSeat, 2, null, null, SkillProgramCardSetVisibility.Private,
            new CardMoveReason(ShiPangXiComposition.XuyeDrawReason));
        AdvanceEventRulesAndQueueFact(new ShiPxXuyeDrawEvent(f.Id, f.OwnerSeat, _turnNumber, r.VictimSeat, 2));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 的“蓄业”令 {_players[r.VictimSeat].Name} 摸两张牌。", f.OwnerSeat, r.VictimSeat);
        f = GetActiveProgramFrame(f.Id);
        if (_players[r.VictimSeat].IsAlive && _winner == Winner.None && ShiPxIsGlobalMaximumHand(r.VictimSeat))
        {
            ReplaceRuntimeTop(f with { PangXi = f.PangXi! with { Stage = ShiPxStage.XuyePick } });
            ContinueShiPx(f.Id);
            return;
        }
        FinishShiPxOperation(GetActiveProgramFrame(f.Id));
    }

    private void ApplyShiPxTopPlace(ProgramSkillFrame f, int cardId, int holder)
    {
        var r = f.PangXi!;
        var card = _cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
        MoveCard(card, _cardZones.GetLocation(cardId), CardLocation.DrawPile,
            new CardMoveReason("skill-program.mobile:shi-xuye.top"));
        AdvanceEventRulesAndQueueFact(new ShiPxXuyeTopPlacedEvent(f.Id, f.OwnerSeat, _turnNumber, holder, cardId));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 将 {_players[holder].Name} 区域内的一张牌置于牌堆顶（蓄业）。", f.OwnerSeat, holder);
        FinishShiPxOperation(GetActiveProgramFrame(f.Id));
    }

    private static readonly CardZoneKind[] ShiPxHolderZones = [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment];

    private IReadOnlyList<int> ShiPxAreaCardIds(int seat) => ShiPxHolderZones
        .SelectMany(zone => _cardZones.CardsAt(new CardLocation(zone, seat)))
        .Select(card => card.Id).Distinct().ToArray();

    // ---- 匡襄: swap hands with a not-richer target, then until the owner's
    // next play phase starts, the first side that spends every swapped-in card
    // opens one self-targeted XuYe.

    private SkillProgramStepOutcome BeginShiPxKuangxiang(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        if (f.SkillId != ShiPxKuangxiang || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] ||
            !_players[f.OwnerSeat].IsAlive || !_players[target].IsAlive || _winner != Winner.None ||
            GetHand(_players[target]).Count > GetHand(_players[f.OwnerSeat]).Count)
            return SkillProgramStepOutcome.Continue;
        ApplyShiPxKuangxiangSwap(GetActiveProgramFrame(f.Id), target);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void ApplyShiPxKuangxiangSwap(ProgramSkillFrame f, int targetSeat)
    {
        var owner = _players[f.OwnerSeat];
        var ownerHandIds = GetHand(owner).Select(card => card.Id).ToArray();
        var targetHandIds = GetHand(_players[targetSeat]).Select(card => card.Id).ToArray();
        foreach (var cardId in ownerHandIds)
        {
            var card = _cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
            MoveCard(card, _cardZones.GetLocation(cardId), CardLocation.Hand(targetSeat),
                new CardMoveReason(ShiPangXiComposition.SwapGiveReason));
        }
        foreach (var cardId in targetHandIds)
        {
            var card = _cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
            MoveCard(card, _cardZones.GetLocation(cardId), CardLocation.Hand(f.OwnerSeat),
                new CardMoveReason(ShiPangXiComposition.SwapTakeReason));
        }
        AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangSwappedEvent(f.Id, f.OwnerSeat, _turnNumber, targetSeat,
            targetHandIds, ownerHandIds));
        AddLog("SkillTriggered", $"{owner.Name} 与 {_players[targetSeat].Name} 交换了手牌（匡襄）。", f.OwnerSeat, targetSeat);
        FinishShiPxOperation(GetActiveProgramFrame(f.Id));
    }

    private SkillProgramStepOutcome BeginShiPxRider(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        var swap = ShiPxActiveSwap(f.OwnerSeat);
        var side = swap is null || f.WindowContext?.MovementBatch is not { } batch ? null : ShiPxRiderSide(swap, batch.Movements);
        if (f.SkillId != ShiPxKuangxiang || f.TriggerId != ShiPangXiComposition.KuangxiangRiderTrigger ||
            swap is null || side is null || !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { PangXi = new("kuangxiang", _turnNumber, ShiPxStage.RiderAsk)
        { Side = side, VictimSeat = side == "self" ? f.OwnerSeat : swap.TargetSeat } });
        PublishShiPx(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiPxRiderArm(ProgramSkillFrame f)
    {
        var r = f.PangXi!;
        AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangRiderEvent(f.Id, f.OwnerSeat, _turnNumber, r.Side, 2));
        DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private,
            new CardMoveReason(ShiPangXiComposition.RiderDrawReason));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 对自己发动“蓄业”，摸两张牌。", f.OwnerSeat);
        f = GetActiveProgramFrame(f.Id);
        if (_winner == Winner.None && _players[f.OwnerSeat].IsAlive && ShiPxIsGlobalMaximumHand(f.OwnerSeat))
        {
            ReplaceRuntimeTop(f with { PangXi = f.PangXi! with { Stage = ShiPxStage.RiderPick } });
            ContinueShiPx(f.Id);
            return;
        }
        FinishShiPxOperation(GetActiveProgramFrame(f.Id));
    }

    private SkillProgramStepOutcome BeginShiPxSweep(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.PangXi is not null)
            throw new InvalidOperationException("Shi Pang Xi restarted a running operation.");
        if (f.SkillId != ShiPxKuangxiang || f.TriggerId != ShiPangXiComposition.KuangxiangSweepTrigger ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None || ShiPxActiveSwap(f.OwnerSeat) is null)
            return SkillProgramStepOutcome.Continue;
        AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangClearedEvent(f.Id, f.OwnerSeat, _turnNumber));
        FinishShiPxOperation(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    // ---- prompts --------------------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiPxChoices(ProgramSkillFrame f)
    {
        var r = f.PangXi!;
        if (_winner != Winner.None || !_players[f.OwnerSeat].IsAlive) return [];
        switch (r.Operation, r.Stage)
        {
            case ("xuye", ShiPxStage.XuyeAsk):
                return
                [
                    MakeShiPxChoice(f, "arm", $"发动“蓄业”：令 {_players[r.VictimSeat].Name} 摸两张牌"),
                    MakeShiPxChoice(f, "pass", "不发动“蓄业”"),
                ];
            case ("xuye", ShiPxStage.XuyePick):
                return ShiPxAreaCardIds(r.VictimSeat).Select(cardId => new PromptChoice(
                    new($"shi-pang-xi.{f.Id}.top.{cardId}"),
                    $"将这张牌置于牌堆顶（{DescribeShiPxCard(_cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId))}）",
                    [cardId], [],
                    new Dictionary<string, string>
                    {
                        ["program-action"] = ShiPangXiComposition.ProgramAction,
                        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                        ["branch"] = "top", ["card-id"] = cardId.ToString(CultureInfo.InvariantCulture),
                    })).ToArray();
            case ("kuangxiang", ShiPxStage.RiderAsk):
                return
                [
                    MakeShiPxChoice(f, "arm", "对自己发动“蓄业”：摸两张牌"),
                    MakeShiPxChoice(f, "pass", "不发动"),
                ];
            case ("kuangxiang", ShiPxStage.RiderPick):
                return ShiPxAreaCardIds(f.OwnerSeat).Select(cardId => new PromptChoice(
                    new($"shi-pang-xi.{f.Id}.self-top.{cardId}"),
                    $"将你的这张牌置于牌堆顶（{DescribeShiPxCard(_cardZones.CardsAt(_cardZones.GetLocation(cardId)).Single(item => item.Id == cardId))}）",
                    [cardId], [],
                    new Dictionary<string, string>
                    {
                        ["program-action"] = ShiPangXiComposition.ProgramAction,
                        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                        ["branch"] = "self-top", ["card-id"] = cardId.ToString(CultureInfo.InvariantCulture),
                    })).ToArray();
            default:
                return [];
        }
    }

    private PromptChoice MakeShiPxChoice(ProgramSkillFrame f, string branch, string label) =>
        new(new($"shi-pang-xi.{f.Id}.{branch}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiPangXiComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });

    private static string DescribeShiPxCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{card.Suit}{card.Rank}";

    private void PublishShiPx(ProgramSkillFrame f)
    {
        var choices = ShiPxChoices(f);
        if (choices.Count == 0)
        { FinishShiPxOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            f.PangXi!.Operation == "xuye" ? "蓄业：请选择。" : "匡襄：请选择。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiPangXiComposition.ProgramAction)]
    private void AnswerShiPx(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { PangXi: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiPxChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Pang Xi choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("xuye", "arm"):
                ApplyShiPxXuyeDraw(GetActiveProgramFrame(f.Id));
                return;
            case ("xuye", "pass"):
                FinishShiPxOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("xuye", "top") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId):
                ApplyShiPxTopPlace(GetActiveProgramFrame(f.Id), cardId, r.VictimSeat);
                return;
            case ("kuangxiang", "arm"):
                ApplyShiPxRiderArm(GetActiveProgramFrame(f.Id));
                return;
            case ("kuangxiang", "pass"):
                AdvanceEventRulesAndQueueFact(new ShiPxKuangxiangClearedEvent(f.Id, f.OwnerSeat, _turnNumber));
                FinishShiPxOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("kuangxiang", "self-top") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var selfCardId):
                ApplyShiPxTopPlace(GetActiveProgramFrame(f.Id), selfCardId, f.OwnerSeat);
                return;
            default:
                throw new InvalidOperationException("Shi Pang Xi answered an incompatible operation branch.");
        }
    }

    private void ContinueShiPx(long id) => PublishShiPx(GetActiveProgramFrame(id));

    private void FinishShiPxOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { PangXi = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiPx(ProgramSkillFrame f) => TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
        TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id) ||
        TryBeginAdvancedSkillsChanged(f.Id);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68500)]
    private bool ResumeShiPx(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { PangXi: not null } f || f.Id != id) return false;
        if (DrainShiPx(GetActiveProgramFrame(id))) return true;
        PublishShiPx(GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68500)]
    private bool ReturnShiPxMovement(ProgramSkillFrame f) => f.PangXi is not null && ResumeShiPx(f.Id);

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxXuye)]
    private PromptChoice ChooseShiPxXuyeAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxKuangxiang)]
    private PromptChoice ChooseShiPxKuangxiangAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxRider)]
    private PromptChoice ChooseShiPxRiderAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiPxSweep)]
    private PromptChoice ChooseShiPxSweepAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}

using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiLyHuilv = ShiLvYiComposition.HuilvSkill,
        ShiLySongyan = ShiLvYiComposition.SongyanSkill,
        ShiLyShishi = ShiLvYiComposition.ShishiSkill;

    [ProgramCapability]
    private sealed class ShiLvYiCapability(GameEngine engine) : IShiLvYiHost
    {
        public SkillProgramStepOutcome ResolveHuilv(ProgramSkillFrame frame) => engine.BeginShiLyHuilv(frame);
        public SkillProgramStepOutcome ResolveHuilvSettle(ProgramSkillFrame frame) => engine.BeginShiLyHuilvSettle(frame);
        public SkillProgramStepOutcome ResolveSongyan(ProgramSkillFrame frame) => engine.BeginShiLySongyan(frame);
    }

    // ---- 隳律: at each other player's play start the owner may grant one random
    // named-color card from the draw pile (each color once per round); at that
    // play end a used grant damages the actor, an unused grant outside the
    // discard pile damages the owner virtually and lets the owner re-give it.

    private ShiLyHuilvGrantedEvent? ShiLyOpenGrant(int ownerSeat, int actorSeat)
    {
        var settled = ProgramEventHistory<ShiLyHuilvSettledEvent>().Select(entry => entry.GrantFrameId).ToHashSet();
        return ProgramEventHistory<ShiLyHuilvGrantedEvent>().FirstOrDefault(entry =>
            entry.OwnerSeat == ownerSeat && entry.ActorSeat == actorSeat &&
            entry.Round == _roundNumber && !settled.Contains(entry.FrameId));
    }

    private SkillProgramStepOutcome BeginShiLyHuilv(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.LvYi is not null)
            throw new InvalidOperationException("Shi Lü Yi restarted a running operation.");
        if (f.SkillId != ShiLyHuilv || f.TriggerId != ShiLvYiComposition.HuilvStartTrigger ||
            f.WindowContext?.TargetSeat is not { } actor || actor == f.OwnerSeat ||
            !_players[f.OwnerSeat].IsAlive || !_players[actor].IsAlive || _winner != Winner.None ||
            _cardZones.Count(CardLocation.DrawPile) == 0)
            return SkillProgramStepOutcome.Continue;
        var redFree = !ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry =>
            entry.OwnerSeat == f.OwnerSeat && entry.Round == _roundNumber && entry.Red);
        var blackFree = !ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry =>
            entry.OwnerSeat == f.OwnerSeat && entry.Round == _roundNumber && !entry.Red);
        if (!redFree && !blackFree)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { LvYi = new("huilv", ShiLyStage.HuilvGrant) { ActorSeat = actor } });
        PublishShiLy(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiLyHuilvGrant(ProgramSkillFrame f, bool red)
    {
        var r = f.LvYi!;
        var pool = _cardZones.CardsAt(CardLocation.DrawPile)
            .Where(card => IsRedSuit(card.Suit) == red).ToArray();
        if (pool.Length == 0 || !_players[r.ActorSeat].IsAlive)
        { FinishShiLyOperation(GetActiveProgramFrame(f.Id)); return; }
        var card = pool[_random.Next(pool.Length)];
        MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(r.ActorSeat),
            new CardMoveReason(ShiLvYiComposition.HuilvGainReason), disclosure: CardMovementDisclosure.FaceUp);
        AdvanceEventRulesAndQueueFact(new ShiLyHuilvGrantedEvent(f.Id, f.OwnerSeat, r.ActorSeat, _roundNumber, red, card.Id));
        AddLog("SkillTriggered",
            $"{_players[f.OwnerSeat].Name} 的“隳律”令 {_players[r.ActorSeat].Name} 获得了{(red ? "红色" : "黑色")}牌 {DescribeShiLyCard(card)}。",
            f.OwnerSeat, r.ActorSeat);
        FinishShiLyOperation(GetActiveProgramFrame(f.Id));
    }

    private SkillProgramStepOutcome BeginShiLyHuilvSettle(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.LvYi is not null)
            throw new InvalidOperationException("Shi Lü Yi restarted a running operation.");
        if (f.SkillId != ShiLyHuilv || f.TriggerId != ShiLvYiComposition.HuilvEndTrigger ||
            f.WindowContext?.TargetSeat is not { } actor || !_players[f.OwnerSeat].IsAlive ||
            _winner != Winner.None || ShiLyOpenGrant(f.OwnerSeat, actor) is not { } grant)
            return SkillProgramStepOutcome.Continue;
        var used = _cardMovements.Any(movement => movement.CardId == grant.CardId &&
            movement.Reason == CardMoveReasons.Use && movement.From.OwnerSeat == actor);
        var inDiscard = _cardZones.GetLocation(grant.CardId).Zone == CardZoneKind.DiscardPile;
        ReplaceRuntimeTop(f = f with { LvYi = new("huilv", ShiLyStage.HuilvSettleDamage)
        {
            ActorSeat = actor, Red = grant.Red, CardId = grant.CardId,
            GrantFrameId = grant.FrameId, Used = used, ThenGive = !used && !inDiscard,
        } });
        if (!_players[f.OwnerSeat].IsAlive)
        {
            AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, grant.FrameId, f.OwnerSeat, actor, "expired", null));
            FinishShiLyOperation(GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChild;
        }
        if (used)
            BeginProgramSkillDamage(GetActiveProgramFrame(f.Id), actor, 1, explicitSourceSeat: f.OwnerSeat);
        else if (!inDiscard)
            BeginProgramSkillDamage(GetActiveProgramFrame(f.Id), f.OwnerSeat, 1, explicitSourceSeat: actor);
        else
            AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, grant.FrameId, f.OwnerSeat, actor, "discarded", null));
        if (!used && inDiscard)
            FinishShiLyOperation(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void CompleteShiLySettleDamage(ProgramSkillFrame f)
    {
        var r = f.LvYi!;
        if (!r.Used && r.ThenGive && _winner == Winner.None && _players[f.OwnerSeat].IsAlive &&
            ShiLyGiveTargets(r.ActorSeat).Count > 0 &&
            _cardZones.GetLocation(r.CardId).Zone is not (CardZoneKind.DiscardPile or CardZoneKind.OutsideGame))
        {
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { LvYi = r with { Stage = ShiLyStage.HuilvSettleGive } });
            ContinueShiLy(f.Id);
            return;
        }
        AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, r.GrantFrameId, f.OwnerSeat, r.ActorSeat,
            r.Used ? "damage" : "kept", null));
        FinishShiLyOperation(GetActiveProgramFrame(f.Id));
    }

    private IReadOnlyList<int> ShiLyGiveTargets(int holderSeat) => _players
        .Where(player => player.IsAlive && player.Seat != holderSeat).Select(player => player.Seat).ToArray();

    private void ApplyShiLyHuilvGive(ProgramSkillFrame f, int targetSeat)
    {
        var r = f.LvYi!;
        var card = _cardZones.CardsAt(_cardZones.GetLocation(r.CardId)).Single(item => item.Id == r.CardId);
        MoveCard(card, _cardZones.GetLocation(r.CardId), CardLocation.Hand(targetSeat),
            new CardMoveReason(ShiLvYiComposition.HuilvGiveReason), disclosure: CardMovementDisclosure.FaceUp);
        AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, r.GrantFrameId, f.OwnerSeat, r.ActorSeat,
            "given", targetSeat));
        AddLog("SkillTriggered",
            $"{_players[f.OwnerSeat].Name} 将“隳律”获得的牌交给了 {_players[targetSeat].Name}。", f.OwnerSeat, targetSeat);
        FinishShiLyOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- 耸言: at prep or after taking damage the owner may use a virtual
    // ExNihilo; during its resolution anyone who discarded this round may
    // convert a same-color hand card into a Nullification through the shared
    // response conversion machinery.

    private SkillProgramStepOutcome BeginShiLySongyan(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.LvYi is not null)
            throw new InvalidOperationException("Shi Lü Yi restarted a running operation.");
        if (f.SkillId != ShiLySongyan ||
            f.TriggerId is not (ShiLvYiComposition.SongyanPrepTrigger or ShiLvYiComposition.SongyanDamageTrigger) ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { LvYi = new("songyan", ShiLyStage.HuilvGrant) });
        PublishShiLy(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiLySongyanUse(ProgramSkillFrame f)
    {
        // The shared damage-trigger cursor forbids nested resolution windows,
        // so a SongYan armed by taking damage settles its draw directly; only
        // the prep-phase use opens the counterable window.
        if (f.WindowContext?.Window == SkillProgramTriggerWindow.AfterDamageApplied)
        {
            DrawCards(_players[f.OwnerSeat], 2, log: true);
            AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 发动“耸言”，视为使用【无中生有】。", f.OwnerSeat);
            FinishShiLyOperation(GetActiveProgramFrame(f.Id));
            return;
        }
        var card = new Card(0, CardKind.DrawTwo, Suit.None, 0);
        var id = BeginCardUse(card, f.OwnerSeat, [f.OwnerSeat], CardKind.DrawTwo, physicalCardIds: []);
        var actionId = LifecycleCardUse(id)!.Action!.ActionId;
        var outerProcessing = _cardZones.CardsAt(CardLocation.Processing).Select(card => card.Id).ToArray();
        UpdateLifecycleCardUse(id, use => use with
        { ShiLySongyanOrigin = new(f.Id, id, actionId, f.OwnerSeat, outerProcessing) });
        AdvanceEventRulesAndQueueFact(new ShiLySongyanUsedEvent(f.Id, f.OwnerSeat, _roundNumber, id));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 发动“耸言”，视为使用【无中生有】。", f.OwnerSeat);
        BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, LifecycleCardUse(id)!.TargetSeats,
            LegalActionKind.DrawTwo, playedCardKind: CardKind.DrawTwo);
    }

    // ---- prompts --------------------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiLyChoices(ProgramSkillFrame f)
    {
        var r = f.LvYi;
        if (r is null || _winner != Winner.None || !_players[f.OwnerSeat].IsAlive) return [];
        switch (r.Operation, r.Stage)
        {
            case ("huilv", ShiLyStage.HuilvGrant):
                var deck = _cardZones.CardsAt(CardLocation.DrawPile);
                var choices = new List<PromptChoice>();
                if (deck.Any(card => IsRedSuit(card.Suit)) &&
                    !ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry =>
                        entry.OwnerSeat == f.OwnerSeat && entry.Round == _roundNumber && entry.Red))
                    choices.Add(MakeShiLyChoice(f, "red", $"令 {_players[r.ActorSeat].Name} 从牌堆中获得一张红色牌"));
                if (deck.Any(card => !IsRedSuit(card.Suit)) &&
                    !ProgramEventHistory<ShiLyHuilvGrantedEvent>().Any(entry =>
                        entry.OwnerSeat == f.OwnerSeat && entry.Round == _roundNumber && !entry.Red))
                    choices.Add(MakeShiLyChoice(f, "black", $"令 {_players[r.ActorSeat].Name} 从牌堆中获得一张黑色牌"));
                choices.Add(MakeShiLyChoice(f, "pass", "不发动“隳律”"));
                return choices;
            case ("huilv", ShiLyStage.HuilvSettleGive):
                return ShiLyGiveTargets(r.ActorSeat)
                    .Select(seat => MakeShiLyChoice(f, $"give-{seat}", $"将该牌交给 {_players[seat].Name}"))
                    .Concat([MakeShiLyChoice(f, "keep", "保留该牌")]).ToArray();
            case ("songyan", _):
                return
                [
                    MakeShiLyChoice(f, "use", "视为使用【无中生有】"),
                    MakeShiLyChoice(f, "pass", "不发动“耸言”"),
                ];
            default:
                return [];
        }
    }

    private PromptChoice MakeShiLyChoice(ProgramSkillFrame f, string branch, string label) =>
        new(new($"shi-lv-yi.{f.Id}.{branch}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiLvYiComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });

    private static string DescribeShiLyCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{card.Suit}{card.Rank}";

    private void PublishShiLy(ProgramSkillFrame f)
    {
        var choices = ShiLyChoices(f);
        if (choices.Count == 0)
        { FinishShiLyOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            f.LvYi!.Operation == "huilv" ? "隳律：请选择。" : "耸言：请选择。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiLvYiComposition.ProgramAction)]
    private void AnswerShiLy(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { LvYi: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiLyChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Lü Yi choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, branch)
        {
            case ("huilv", "red"):
                ApplyShiLyHuilvGrant(GetActiveProgramFrame(f.Id), red: true);
                return;
            case ("huilv", "black"):
                ApplyShiLyHuilvGrant(GetActiveProgramFrame(f.Id), red: false);
                return;
            case ("huilv", "pass"):
                FinishShiLyOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("huilv", "keep"):
                AdvanceEventRulesAndQueueFact(new ShiLyHuilvSettledEvent(f.Id, r.GrantFrameId, f.OwnerSeat,
                    r.ActorSeat, "kept", null));
                FinishShiLyOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("huilv", var give) when give.StartsWith("give-", StringComparison.Ordinal) &&
                int.TryParse(give[5..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var giveSeat) &&
                ShiLyGiveTargets(r.ActorSeat).Contains(giveSeat):
                ApplyShiLyHuilvGive(GetActiveProgramFrame(f.Id), giveSeat);
                return;
            case ("songyan", "use"):
                ApplyShiLySongyanUse(GetActiveProgramFrame(f.Id));
                return;
            case ("songyan", "pass"):
                FinishShiLyOperation(GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Lü Yi answered an incompatible operation branch.");
        }
    }

    private void ContinueShiLy(long id) => PublishShiLy(GetActiveProgramFrame(id));

    private void FinishShiLyOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { LvYi = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiLy(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68600)]
    private bool ResumeShiLy(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { LvYi: { } r } f || f.Id != id) return false;
        if (r.Operation == "songyan")
        {
            FinishShiLyOperation(GetActiveProgramFrame(id));
            return true;
        }
        if (r.Stage == ShiLyStage.HuilvSettleDamage)
        {
            CompleteShiLySettleDamage(GetActiveProgramFrame(id));
            return true;
        }
        if (DrainShiLy(GetActiveProgramFrame(id))) return true;
        PublishShiLy(GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68600)]
    private bool ReturnShiLyMovement(ProgramSkillFrame f) => f.LvYi is { Operation: "huilv" } && ResumeShiLy(f.Id);

    // ---- 耸言 conversion gate --------------------------------------------------

    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 68610)]
    private bool ShiLySongyanConversionForbidden(ProgramViewAsLegalityQuery query)
    {
        if (query.Source.SkillId != ShiLySongyan) return false;
        if (!query.ForResponse) return true;
        if (_resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault() is not { } window ||
            window.EffectCardKind != CardKind.DrawTwo ||
            !ProgramEventHistory<ShiLySongyanUsedEvent>().Any(entry => entry.UseId == window.ParentFrameId) ||
            !query.Owner.IsAlive || query.CardIds is not [var cardId])
            return true;
        var location = _cardZones.GetLocation(cardId);
        if (location.OwnerSeat != query.Owner.Seat || location.Zone != CardZoneKind.Hand) return true;
        var red = IsRedSuit(EffectiveSuit(query.Owner, GetAdvancedCard(cardId)));
        return !ProgramEventHistory<ShiLyRoundDiscardedEvent>().Any(entry =>
            entry.Seat == query.Owner.Seat && entry.Round == _roundNumber && entry.Red == red);
    }

    // ---- observers --------------------------------------------------------------

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68620)]
    private void ObserveShiLyRoundDiscard(CardMovedEvent moved)
    {
        if (!HasShiLyCapability || moved.To.Zone != CardZoneKind.DiscardPile ||
            moved.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) ||
            moved.From.OwnerSeat is not { } holder ||
            !moved.Reason.Value.Contains("discard", StringComparison.Ordinal) ||
            !IsValidPlayerSeat(holder))
            return;
        AdvanceEventRulesAndQueueFact(new ShiLyRoundDiscardedEvent(holder, _roundNumber,
            moved.CardId, IsRedSuit(GetAdvancedCard(moved.CardId).Suit)));
    }

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLyHuilv)]
    private PromptChoice ChooseShiLyHuilvAi(PendingDecision decision, ProgramSkillFrame f) =>
        decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "pass") ?? decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLyHuilvSettle)]
    private PromptChoice ChooseShiLyHuilvSettleAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiLySongyan)]
    private PromptChoice ChooseShiLySongyanAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}

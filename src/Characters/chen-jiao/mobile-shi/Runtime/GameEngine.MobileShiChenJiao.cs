using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string QingYanSkill = ChenJiaoComposition.QingYanSkill,
        CeDuanSkill = ChenJiaoComposition.CeDuanSkill;

    private static CardConversionSource ChenJiaoSource(ProgramSkillFrame f) =>
        new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    [ProgramCapability]
    private sealed class ChenJiaoCapability(GameEngine engine) : IChenJiaoHost
    {
        public SkillProgramStepOutcome ResolveQingYan(ProgramSkillFrame frame) => engine.BeginChenJiao(frame);
        public SkillProgramStepOutcome ResolveCeDuan(ProgramSkillFrame frame) => engine.BeginChenJiao(frame);
        public SkillProgramStepOutcome SettleCeDuan(ProgramSkillFrame frame) => engine.SettleChenJiaoCeDuan(frame);
    }

    // 清严 arms only while it is not失效 and the hand can still pay the escalating
    // show count X = min(times activated this round + 1, 5).
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68100, SkillProgramEffectOp.ResolveChenJiaoQingYan)]
    private bool CanStartChenJiaoQingYan(CharacterState owner, string skill)
    {
        if (skill != QingYanSkill) return true;
        var ordinal = Math.Min(ChenJiaoQingYanUsesThisRound(owner.Seat) + 1, ChenJiaoComposition.MaximumOrdinal);
        return owner.IsAlive && !ChenJiaoQingYanLocked(owner.Seat) && GetHand(owner).Count >= ordinal;
    }

    private SkillProgramStepOutcome BeginChenJiao(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ChenJiao is not null) throw new InvalidOperationException("Chen Jiao restarted a running operation.");
        var r = f.SkillId switch
        {
            QingYanSkill => StartChenJiaoQingYan(f),
            CeDuanSkill => StartChenJiaoCeDuan(f),
            _ => throw new InvalidOperationException("Chen Jiao instruction has an incompatible owning skill."),
        };
        ReplaceRuntimeTop(f = f with { ChenJiao = r });
        ContinueChenJiao(f.Id);
        return r.Operation switch
        {
            "ceduan" when r.Stage == ChenJiaoStage.Issue => SkillProgramStepOutcome.AwaitChild,
            _ => SkillProgramStepOutcome.AwaitChoice,
        };
    }

    private ChenJiaoReceipt StartChenJiaoQingYan(ProgramSkillFrame f)
    {
        var ordinal = Math.Min(ChenJiaoQingYanUsesThisRound(f.OwnerSeat) + 1, ChenJiaoComposition.MaximumOrdinal);
        if (!_players[f.OwnerSeat].IsAlive || ChenJiaoQingYanLocked(f.OwnerSeat) || GetHand(_players[f.OwnerSeat]).Count < ordinal)
            throw new InvalidOperationException("QingYan requires its unlocked owner with enough hand cards.");
        return new("qingyan", _turnNumber) { Ordinal = ordinal, Round = _roundNumber };
    }

    private ChenJiaoReceipt StartChenJiaoCeDuan(ProgramSkillFrame f)
    {
        if (!_players[f.OwnerSeat].IsAlive || f.SelectedTargetSeats is not [var target] ||
            !IsValidPlayerSeat(target) || !_players[target].IsAlive)
            throw new InvalidOperationException("CeDuan requires its living owner and one living in-range target.");
        var holders = Enumerable.Range(0, _players.Count).Where(seat => _players[seat].IsAlive &&
            IsWithinAttackRange(target, seat) && GetHand(_players[seat]).Count > 0).ToArray();
        AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanStartedEvent(f.Id, _roundNumber, f.OwnerSeat, target, holders.Length));
        return new("ceduan", _turnNumber)
        { TargetSeat = target, Round = _roundNumber, Holders = Array.AsReadOnly(holders) };
    }

    // ---- choice publication -------------------------------------------------

    private IReadOnlyList<PromptChoice> ChenJiaoChoices(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        switch (r.Operation, r.Stage)
        {
            case ("qingyan", ChenJiaoStage.Show):
                return GetHand(_players[f.OwnerSeat]).Where(card => !r.ShownIds.Contains(card.Id))
                    .Select(card => new PromptChoice(
                        new($"shi-chen-jiao.{f.Id}.show.{card.Id}"),
                        $"展示这张手牌（{DescribeChenJiaoCard(card)}）", [card.Id], [],
                        new Dictionary<string, string>
                        {
                            ["program-action"] = ChenJiaoComposition.ProgramAction,
                            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                            ["branch"] = "show", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture),
                        })).ToArray();
            case ("qingyan", ChenJiaoStage.ChooseKind):
                return new[]
                {
                    MakeChenJiaoChoice(f, "kind", "dodge", "视为使用一张【闪】"),
                    MakeChenJiaoChoice(f, "kind", "nullification", "视为使用一张【无懈可击】"),
                };
            case ("ceduan", ChenJiaoStage.Show) when r.Cursor < r.Holders.Count:
                var holder = r.Holders[r.Cursor];
                return GetHand(_players[holder]).Select(card => new PromptChoice(
                    new($"shi-chen-jiao.{f.Id}.show.{holder}.{card.Id}"),
                    $"展示这张手牌（{DescribeChenJiaoCard(card)}）", [card.Id], [],
                    new Dictionary<string, string>
                    {
                        ["program-action"] = ChenJiaoComposition.ProgramAction,
                        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                        ["branch"] = "show", ["holder"] = holder.ToString(CultureInfo.InvariantCulture),
                        ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture),
                    })).ToArray();
            case ("ceduan", ChenJiaoStage.ChooseColor):
                return new[]
                {
                    MakeChenJiaoChoice(f, "color", "red", "选择红色"),
                    MakeChenJiaoChoice(f, "color", "black", "选择黑色"),
                };
            default:
                return [];
        }
    }

    private PromptChoice MakeChenJiaoChoice(ProgramSkillFrame f, string branch, string value, string label) =>
        new(new($"shi-chen-jiao.{f.Id}.{branch}.{value}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ChenJiaoComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch, ["value"] = value,
            });

    private static string DescribeChenJiaoCard(Card card) => card.Kind switch
    {
        CardKind.Slash => "杀", CardKind.Dodge => "闪", CardKind.Peach => "桃",
        _ => $"{card.Kind}",
    } + $"{card.Suit}{card.Rank}";

    private void PublishChenJiao(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        var choices = ChenJiaoChoices(f);
        var chooser = r.Operation switch
        {
            "ceduan" when r.Stage == ChenJiaoStage.Show => r.Holders[r.Cursor],
            _ => f.OwnerSeat,
        };
        if (choices.Count == 0 || _winner != Winner.None || !_players[f.OwnerSeat].IsAlive ||
            !_players[chooser].IsAlive)
        { FinishChenJiaoOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, chooser,
            r.Operation switch
            {
                "qingyan" when r.Stage == ChenJiaoStage.Show =>
                    $"清严：请选择第 {r.Cursor + 1}/{r.Ordinal} 张要展示的手牌。",
                "qingyan" => "清严：选择视为使用的牌。",
                _ => "策断：请选择要展示的手牌。",
            },
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = chooser, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ChenJiaoComposition.ProgramAction)]
    private void AnswerChenJiao(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ChenJiao: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ChenJiaoChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Chen Jiao choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("qingyan", "show") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId):
                ResolveChenJiaoQingYanShow(f, cardId);
                return;
            case ("qingyan", "kind"):
                ResolveChenJiaoQingYanKind(f, selected.Parameters["value"]);
                return;
            case ("ceduan", "show") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var showId) &&
                int.TryParse(selected.Parameters["holder"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var holder):
                ResolveChenJiaoCeDuanShow(f, holder, showId);
                return;
            case ("ceduan", "color"):
                ResolveChenJiaoCeDuanColor(f, selected.Parameters["value"]);
                return;
            default:
                throw new InvalidOperationException("Chen Jiao answered an incompatible operation branch.");
        }
    }

    private void ResolveChenJiaoQingYanShow(ProgramSkillFrame f, int cardId)
    {
        var r = f.ChenJiao!;
        if (_cardZones.GetLocation(cardId) is not { Zone: CardZoneKind.Hand, OwnerSeat: var seat } || seat != f.OwnerSeat ||
            r.ShownIds.Contains(cardId))
            throw new InvalidOperationException("QingYan showed a card outside its owner's current hand.");
        AdvanceEventRulesAndQueueFact(new ChenJiaoQingYanShownEvent(f.Id, r.Round, f.OwnerSeat, cardId,
            r.Ordinal, r.Cursor));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 展示了手牌（清严 {r.Cursor + 1}/{r.Ordinal}）。", f.OwnerSeat);
        SyncChenJiaoMarker(f.OwnerSeat);
        f = GetActiveProgramFrame(f.Id);
        ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao! with
        { Cursor = r.Cursor + 1, ShownIds = Array.AsReadOnly(r.ShownIds.Concat([cardId]).ToArray()) } });
        ContinueChenJiao(f.Id);
    }

    private void ResolveChenJiaoQingYanKind(ProgramSkillFrame f, string value)
    {
        var r = f.ChenJiao!;
        var kind = value switch
        {
            "dodge" => CardKind.Dodge,
            "nullification" => CardKind.Nullification,
            _ => throw new InvalidOperationException("QingYan chose an unsupported virtual card kind."),
        };
        var useId = ++_resolutionSequence;
        var action = CaptureFactionAction(new CardActionContext(++_cardActionSequence,
            _resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use,
            f.OwnerSeat, f.OwnerSeat, null, null, null, kind, [f.OwnerSeat], [], [ChenJiaoSource(f)],
            effectiveSuit: Suit.None, effectiveRank: 0));
        // Showing is not payment. Keep the real zero-entity Use on its original
        // skill parent so declared-use observers can inspect the actual action.
        ReplaceRuntimeTop(f = f with { ChenJiao = r with { Stage = ChenJiaoStage.Issue, Kind = kind, UseId = useId } });
        PushRuntimeFrame(new CardUseFrame(useId, f.OwnerSeat, 0, kind, [f.OwnerSeat], PhysicalCardIds: []) { Action = action });
        if (TracksPlayCardHistory) AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, kind, f.OwnerSeat));
        AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, f.OwnerSeat, f.OwnerSeat));
        AdvanceEventRulesAndQueueFact(new ChenJiaoQingYanUsedEvent(f.Id, r.Round, f.OwnerSeat, r.Ordinal, kind, useId));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 的“清严”视为使用了一张【{(kind == CardKind.Dodge ? "闪" : "无懈可击")}】。", f.OwnerSeat);
        TryMarkProgramUseCommitted(useId);
        RecordActualPlayPhaseUse(action);
        FinishCardUse(useId, new Card(0, kind, Suit.None, 0), kind);
    }

    [NativeUsePreparation(68100, 68100, nameof(ReturnChenJiaoQingYanUse))]
    private bool PrepareChenJiaoQingYanUse(CardUseFrame use)
    {
        if (use.CardId != 0 || use.CardKind is not (CardKind.Dodge or CardKind.Nullification) ||
            use.Action?.ConversionChain is not [var source] || source.SkillId != QingYanSkill) return false;
        var index = _resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (index < 1 || _resolutionStack[index - 1] is not ProgramSkillFrame { ChenJiao: { Operation: "qingyan", Stage: ChenJiaoStage.Issue } receipt } parent ||
            receipt.UseId != use.Id || receipt.Kind != use.CardKind || source != ChenJiaoSource(parent) ||
            _contentRegistry.GetSkill(parent.SkillId).Program?.GameplayHash != parent.GameplayHash ||
            use.SourceSeat != parent.OwnerSeat || use.Action is not { Type: CardActionType.Use } action ||
            action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat || action.RequesterSeat is not null ||
            action.EffectiveKind != use.CardKind || action.PhysicalCards.Count != 0 || use.PhysicalCardIds is not { Count: 0 } ||
            !action.TargetSeats.SequenceEqual([parent.OwnerSeat]) || !use.TargetSeats.SequenceEqual(action.TargetSeats) ||
            ProgramEventHistory<ChenJiaoQingYanUsedEvent>().Count(fact => fact.FrameId == parent.Id && fact.UseId == use.Id &&
                fact.Kind == use.CardKind && fact.OwnerSeat == parent.OwnerSeat && fact.Round == receipt.Round && fact.Ordinal == receipt.Ordinal) != 1 ||
            ProgramEventHistory<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.Id && fact.CardId == 0 && fact.CardKind == use.CardKind) != 1)
            throw new InvalidOperationException("QingYan lost its exact original zero-entity use and skill parent.");
        return true;
    }

    private void ReturnChenJiaoQingYanUse(CardUseFrame use)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ChenJiao: { Operation: "qingyan", Stage: ChenJiaoStage.Issue } receipt } parent ||
            receipt.UseId != use.Id || receipt.Kind != use.CardKind || use.Action?.ConversionChain is not [var source] || source != ChenJiaoSource(parent))
            throw new InvalidOperationException("QingYan cannot return another or already-completed native use.");
        FinishChenJiaoOperation(parent);
    }

    private void ResolveChenJiaoCeDuanShow(ProgramSkillFrame f, int holder, int cardId)
    {
        var r = f.ChenJiao!;
        if (r.Cursor >= r.Holders.Count || r.Holders[r.Cursor] != holder ||
            _cardZones.GetLocation(cardId) is not { Zone: CardZoneKind.Hand, OwnerSeat: var seat } || seat != holder)
            throw new InvalidOperationException("CeDuan show left its ordered holder or their current hand.");
        AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanShownEvent(f.Id, r.Round, holder, cardId, r.Cursor));
        AddLog("SkillTriggered", $"{_players[holder].Name} 展示了一张手牌（策断）。", holder);
        f = GetActiveProgramFrame(f.Id);
        ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao! with
        { Cursor = r.Cursor + 1, ShownIds = Array.AsReadOnly(r.ShownIds.Concat([cardId]).ToArray()) } });
        ContinueChenJiao(f.Id);
    }

    private void ResolveChenJiaoCeDuanColor(ProgramSkillFrame f, string value)
    {
        var color = value switch
        {
            "red" => Suit.Heart,
            "black" => Suit.Spade,
            _ => throw new InvalidOperationException("CeDuan chose an unsupported color."),
        };
        f = GetActiveProgramFrame(f.Id);
        ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao! with { Color = color } });
        CollectChenJiaoCeDuanMaterials(GetActiveProgramFrame(f.Id));
    }

    // ---- continuations ------------------------------------------------------

    private void ContinueChenJiao(long id)
    {
        if (_pendingDecision is not null || _resolutionStack.LastOrDefault() is not ProgramSkillFrame { ChenJiao: { } r } f || f.Id != id) return;
        if (r.Skip)
        { FinishChenJiaoOperation(GetActiveProgramFrame(id)); return; }
        switch (r.Operation, r.Stage)
        {
            case ("qingyan", ChenJiaoStage.Show) when r.Cursor < r.Ordinal:
                PublishChenJiao(GetActiveProgramFrame(id));
                return;
            case ("qingyan", ChenJiaoStage.Show):
                ReplaceRuntimeTop(GetActiveProgramFrame(id) with { ChenJiao = r with { Stage = ChenJiaoStage.ChooseKind } });
                PublishChenJiao(GetActiveProgramFrame(id));
                return;
            case ("qingyan", ChenJiaoStage.ChooseKind):
                PublishChenJiao(GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.Show) when r.Cursor < r.Holders.Count:
                PublishChenJiao(GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.Show):
                ContinueChenJiaoCeDuanColor(GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.ChooseColor):
                PublishChenJiao(GetActiveProgramFrame(id));
                return;
            case ("ceduan", ChenJiaoStage.Issue):
                if (DrainChenJiao(GetActiveProgramFrame(id))) return;
                ReplaceRuntimeTop(GetActiveProgramFrame(id) with { PendingMovementContinuation = null });
                IssueChenJiaoCeDuanSlash(GetActiveProgramFrame(id));
                return;
            default:
                throw new InvalidOperationException("Chen Jiao has an unknown continuation stage.");
        }
    }

    private void ContinueChenJiaoCeDuanColor(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        var shown = ChenJiaoCeDuanShownIdsThisActivation(f.Id)
            .Select(id => _cardZones.CardsAt(_cardZones.GetLocation(id)).Single(card => card.Id == id)).ToArray();
        var red = shown.Count(ChenJiaoIsRed);
        var black = shown.Length - red;
        if (red > 0 && red == black)
        {
            ReplaceRuntimeTop(f = f with { ChenJiao = r with { Stage = ChenJiaoStage.ChooseColor } });
            ContinueChenJiao(f.Id);
            return;
        }
        var color = red > black ? Suit.Heart : Suit.Spade;
        ReplaceRuntimeTop(f = f with { ChenJiao = r with { Color = color } });
        CollectChenJiaoCeDuanMaterials(GetActiveProgramFrame(f.Id));
    }

    // The card-use payment machinery pays one provider's owned entities, so the
    // chosen shown cards are collected into the owner's hand immediately before
    // the virtual Slash issues. Cards the owner showed from their own hand are
    // already in place and must not move.
    private void CollectChenJiaoCeDuanMaterials(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        var selected = ChenJiaoCeDuanShownIdsThisActivation(f.Id)
            .Where(id => _cardZones.CardsAt(_cardZones.GetLocation(id)).Single(card => card.Id == id) is var card &&
                (r.Color == Suit.Heart ? ChenJiaoIsRed(card) : !ChenJiaoIsRed(card)) &&
                _cardZones.GetLocation(id).Zone == CardZoneKind.Hand)
            .ToArray();
        var movable = selected.Where(id => _cardZones.GetLocation(id).OwnerSeat != f.OwnerSeat).ToArray();
        var damageBefore = ChenJiaoCeDuanDamageCount(f.OwnerSeat, r.TargetSeat);
        if (movable.Length > 0)
        {
            ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            MoveProgramCardsFromMultipleSources(movable, CardLocation.Hand(f.OwnerSeat), new(ChenJiaoComposition.CeDuanTakeReason));
            f = GetActiveProgramFrame(f.Id);
        }
        ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao! with
        { Stage = ChenJiaoStage.Issue, MaterialCount = selected.Length, DamageCountBefore = damageBefore } });
        ContinueChenJiao(f.Id);
    }

    private bool CanIssueChenJiaoSlash(int actor, int target) =>
        _players[actor].IsAlive && _players[target].IsAlive && actor != target &&
        IsWithinAttackRange(actor, target) &&
        !HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) &&
        !IsCardUseForbidden(actor, CardKind.Slash, CardActionType.Use) &&
        !IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) &&
        !IsCardTargetProhibited(_players[target], CardKind.Slash, Suit.None, null) &&
        !IsSlashProhibited(_players[target]) &&
        CanSpendSlashUse(_players[actor], _players[target], ignoresCount: true);

    private void IssueChenJiaoCeDuanSlash(ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        if (r.MaterialCount == 0 || !_players[f.OwnerSeat].IsAlive || !_players[r.TargetSeat].IsAlive ||
            !CanIssueChenJiaoSlash(f.OwnerSeat, r.TargetSeat))
        {
            AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanCompletedEvent(f.Id, 0, false, false));
            FinishChenJiaoOperation(GetActiveProgramFrame(f.Id));
            return;
        }
        var actor = _players[f.OwnerSeat];
        var targetSeat = r.TargetSeat;
        var useId = ++_resolutionSequence;
        var action = CaptureFactionAction(new CardActionContext(++_cardActionSequence,
            _resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use,
            actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [],
            Array.AsReadOnly(new[] { ChenJiaoSource(f) }),
            effectiveSuit: Suit.None, effectiveRank: 0));
        ReplaceRuntimeTop(f = f with { ChenJiao = f.ChenJiao! with { UseId = useId } });
        PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash,
            Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>()))
        { Action = action });
        if (TracksPlayCardHistory) AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, Array.AsReadOnly(new[] { targetSeat })));
        RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        RecordActualPlayPhaseUse(action);
        var attack = new CardAttackHandle(this, useId, actor.Seat, targetSeat, card: null,
            damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash,
            ignoresArmor: HasCardArmorBypass(actor, _players[targetSeat], CardKind.Slash),
            programSkillCardUseFrameId: f.Id);
        CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        ActiveCardAttack = attack;
        AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanUseIssuedEvent(f.Id, useId, actor.Seat, targetSeat,
            r.MaterialCount, r.DamageCountBefore));
        AddLog("CardUsed", $"{actor.Name} 将 {r.MaterialCount} 张展示牌当作一张不计入次数的【杀】对 {_players[targetSeat].Name} 使用。",
            actor.Seat, targetSeat);
        TryMarkProgramUseCommitted(useId);
        if (!TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats,
                ProgramCardContinuation.CommittedSlash))
            BeginSlashTargetResolution(attack);
    }

    private SkillProgramStepOutcome SettleChenJiaoCeDuan(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        var r = f.ChenJiao ?? throw new InvalidOperationException("CeDuan settle lost its original receipt.");
        if (r.Operation != "ceduan" || r.Stage != ChenJiaoStage.Issue)
            throw new InvalidOperationException("CeDuan settle ran outside its issued stage.");
        var damaged = r.UseId != 0 &&
            ChenJiaoCeDuanDamageCount(f.OwnerSeat, r.TargetSeat) > r.DamageCountBefore;
        var drew = false;
        if (damaged && _players[f.OwnerSeat].IsAlive && _winner == Winner.None)
        {
            DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private,
                new(ChenJiaoComposition.CeDuanTakeReason));
            drew = true;
        }
        AdvanceEventRulesAndQueueFact(new ChenJiaoCeDuanCompletedEvent(f.Id, r.UseId, damaged, drew));
        FinishChenJiaoOperation(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.Continue;
    }

    private void FinishChenJiaoOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ChenJiao = null, PendingMovementContinuation = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainChenJiao(ProgramSkillFrame f) => TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
        TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id) ||
        TryBeginAdvancedSkillsChanged(f.Id);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68100)]
    private bool ResumeChenJiao(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ChenJiao: { } r } f || f.Id != id) return false;
        switch (r.Operation, r.Stage)
        {
            case ("ceduan", ChenJiaoStage.Issue) when r.UseId != 0:
                // The child attack completion re-runs the program; its next
                // instruction is the typed settle step.
                return false;
            case ("ceduan", ChenJiaoStage.Issue):
                if (DrainChenJiao(GetActiveProgramFrame(id))) return true;
                ReplaceRuntimeTop(GetActiveProgramFrame(id) with { PendingMovementContinuation = null });
                IssueChenJiaoCeDuanSlash(GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainChenJiao(GetActiveProgramFrame(id))) return true;
                ContinueChenJiao(id);
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68100)]
    private bool ReturnChenJiaoMovement(ProgramSkillFrame f) => f.ChenJiao is not null && ResumeChenJiao(f.Id);

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChenJiaoQingYan)]
    private PromptChoice ChooseChenJiaoQingYanAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        if (r.Stage == ChenJiaoStage.ChooseKind) return decision.Choices[0];
        return decision.Choices[^1];
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChenJiaoCeDuan)]
    private PromptChoice ChooseChenJiaoCeDuanAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ChenJiao!;
        if (r.Stage == ChenJiaoStage.ChooseColor) return decision.Choices[0];
        return decision.Choices[0];
    }
}

using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiHcCujun = ShiHuangChongComposition.CujunSkill,
        ShiHcSili = ShiHuangChongComposition.SiliSkill;

    private bool HasShiHcCapability =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiHcCujun) ||
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiHcSili);

    [ProgramCapability]
    private sealed class ShiHuangChongCapability(GameEngine engine) : IShiHuangChongHost
    {
        public SkillProgramStepOutcome ResolveCujun(ProgramSkillFrame frame) => engine.BeginShiHcCujun(frame);
        public SkillProgramStepOutcome ResolveSili(ProgramSkillFrame frame) => engine.BeginShiHcSili(frame);
    }

    // ---- 促军: at the owner's discard phase start they may name any other
    // character; every living character inside that target's attack range
    // (excluding the target) simultaneously gives the target one hand card or
    // raises the target's next play-phase Slash limit by one. If the target
    // then holds fewer hand cards than the owner, it claims the damage cards
    // the owner discards from hand during this very discard phase.

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68750, SkillProgramEffectOp.ResolveShiHcCujun)]
    private bool CanOfferShiHcCujun(ProgramTriggerLegalityContext c)
    {
        if (c.Context.Window == SkillProgramTriggerWindow.DiscardPhaseStarting)
            return c.SkillId == ShiHcCujun && c.Owner.IsAlive && _winner == Winner.None &&
                _players.Any(player => player.IsAlive && player.Seat != c.OwnerSeat);
        if (c.Context.Window == SkillProgramTriggerWindow.DiscardPhaseEnded)
            return c.SkillId == ShiHcCujun && c.Owner.IsAlive && _winner == Winner.None &&
                ShiHcLedger().PendingClaim(c.OwnerSeat, _turnNumber) is not null;
        return false;
    }

    private SkillProgramStepOutcome BeginShiHcCujun(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.HuangChong is not null)
            throw new InvalidOperationException("Shi Huang Chong restarted a running operation.");
        if (f.SkillId != ShiHcCujun || f.SelectedCardIds is { Count: > 0 } || f.SelectedTargetSeats.Count != 0 ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        if (f.TriggerId == ShiHuangChongComposition.CujunClaimTrigger)
        {
            ReplaceRuntimeTop(f = f with { HuangChong = new("cujun", ShiHcStage.CujunClaimSettling) });
            SettleShiHcClaim(GetActiveProgramFrame(f.Id));
            return _resolutionStack.LastOrDefault() is ProgramSkillFrame { HuangChong: not null } settled
                && settled.Id == f.Id ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
        }
        if (f.TriggerId != ShiHuangChongComposition.CujunDiscardTrigger)
            return SkillProgramStepOutcome.Continue;
        var targets = _players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat)
            .Select(player => player.Seat).ToArray();
        if (targets.Length == 0)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { HuangChong = new("cujun", ShiHcStage.CujunTarget) });
        PublishShiHcChoices(GetActiveProgramFrame(f.Id), f.OwnerSeat,
            targets.Select(seat => MakeShiHcSeatChoice(GetActiveProgramFrame(f.Id), "target", seat,
                $"选择 {_players[seat].Name}")).ToArray(),
            $"【{GetShiHcSkillName(ShiHcCujun)}】：选择一名角色。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void AnswerShiHcCujunTarget(ProgramSkillFrame f, ShiHcReceipt r, string branch)
    {
        var target = int.Parse(branch["target-".Length..], CultureInfo.InvariantCulture);
        if (!_players[target].IsAlive)
            throw new InvalidOperationException("Shi Huang Chong named a dead CuJun target.");
        AdvanceEventRulesAndQueueFact(new ShiHcCujunStartedEvent(f.Id, f.OwnerSeat, target));
        var choosers = ShiHcRangeSeats(target).ToArray();
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = r with
        { Stage = ShiHcStage.CujunChoosers, TargetSeat = target, Choosers = choosers,
            Cursor = 0, GiftCards = Enumerable.Repeat(0, choosers.Length).ToArray() } });
        ContinueShiHcChoosers(GetActiveProgramFrame(f.Id));
    }

    // Characters inside the target's attack range other than the target itself.
    private IEnumerable<int> ShiHcRangeSeats(int seat)
    {
        var range = GetAttackRange(seat);
        return _players.Where(player => player.IsAlive && player.Seat != seat &&
                GetCombatDistance(seat, player.Seat) <= range)
            .Select(player => player.Seat).ToArray();
    }

    private void ContinueShiHcChoosers(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        while (r.Answered < r.Choosers.Length)
        {
            var chooser = r.Choosers[r.Answered];
            if (!_players[chooser].IsAlive)
            {
                ReplaceRuntimeTop(f = f with { HuangChong = r with { Answered = r.Answered + 1 } });
                r = f.HuangChong!;
                continue;
            }
            PublishShiHcChoices(f, chooser, ShiHcChooserChoices(f, chooser),
                $"【{GetShiHcSkillName(ShiHcCujun)}】：{_players[f.OwnerSeat].Name} 对 {_players[r.TargetSeat].Name} 发动促军，请选择一项。");
            return;
        }
        SettleShiHcCujun(f);
    }

    private IReadOnlyList<PromptChoice> ShiHcChooserChoices(ProgramSkillFrame f, int chooser)
    {
        var r = f.HuangChong!;
        var choices = new List<PromptChoice>();
        foreach (var card in GetHand(_players[chooser]))
            choices.Add(new PromptChoice(new($"shi-huang-chong.{f.Id}.gift-{r.Answered}-{card.Id}"),
                $"交给 {_players[r.TargetSeat].Name} 一张手牌", [card.Id], [],
                new Dictionary<string, string>
                {
                    ["program-action"] = ShiHuangChongComposition.ProgramAction,
                    ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                    ["branch"] = $"gift-{r.Answered}-{card.Id}",
                }));
        choices.Add(MakeShiHcBranchChoice(f, "slash-plus",
            $"令 {_players[r.TargetSeat].Name} 下个出牌阶段使用【杀】的次数+1"));
        return choices;
    }

    private void SettleShiHcCujun(ProgramSkillFrame f)
    {
        var r = f.HuangChong! with { Stage = ShiHcStage.CujunSettling, Cursor = 0 };
        ReplaceRuntimeTop(f = GetActiveProgramFrame(f.Id) with { HuangChong = r });
        while (r.Cursor < r.Choosers.Length)
        {
            var chooser = r.Choosers[r.Cursor];
            var gift = r.GiftCards[r.Cursor];
            ReplaceRuntimeTop(f = f with { HuangChong = r with { Cursor = r.Cursor + 1 } });
            r = f.HuangChong!;
            if (!_players[chooser].IsAlive || !_players[r.TargetSeat].IsAlive || _winner != Winner.None) continue;
            var moved = 0;
            if (gift > 0 && _cardZones.GetLocation(gift) is { Zone: CardZoneKind.Hand, OwnerSeat: var holder } && holder == chooser)
            {
                MoveCard(GetAdvancedCard(gift), CardLocation.Hand(chooser), CardLocation.Hand(r.TargetSeat),
                    new CardMoveReason(ShiHuangChongComposition.CujunGiftReason), disclosure: CardMovementDisclosure.FaceUp);
                moved = gift;
            }
            if (moved > 0)
                AdvanceEventRulesAndQueueFact(new ShiHcCujunGiftMovedEvent(f.Id, chooser, r.TargetSeat, moved,
                    _cardMovements.Count > 0 ? _cardMovements[^1].Sequence : 0));
            else
                AdvanceEventRulesAndQueueFact(new ShiHcCujunSlashGrantedEvent(f.Id, chooser, r.TargetSeat,
                    _turnNumber, _cardUseDebitPhaseInstanceId));
            if (DrainShiHc(f.Id)) return;
            f = GetActiveProgramFrame(f.Id);
            r = f.HuangChong!;
        }
        if (_players[r.TargetSeat].IsAlive && _players[f.OwnerSeat].IsAlive && _winner == Winner.None &&
            ShiHcHandCount(r.TargetSeat) < ShiHcHandCount(f.OwnerSeat))
        {
            AdvanceEventRulesAndQueueFact(new ShiHcCujunClaimIssuedEvent(f.Id, f.OwnerSeat, r.TargetSeat, _turnNumber));
            if (DrainShiHc(f.Id)) return;
        }
        FinishShiHcOperation(GetActiveProgramFrame(f.Id));
    }

    private int ShiHcHandCount(int seat) => GetHand(_players[seat]).Count;

    private void SettleShiHcClaim(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        var claim = ShiHcLedger().PendingClaim(f.OwnerSeat, _turnNumber) ??
            throw new InvalidOperationException("Shi Huang Chong lost its pending CuJun claim.");
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = r with { TargetSeat = claim.TargetSeat } });
        f = GetActiveProgramFrame(f.Id);
        r = f.HuangChong!;
        var discardPile = _cardZones.CardsAt(CardLocation.DiscardPile);
        var damageIds = discardPile.Where(card => ShiHuangChongComposition.IsDamageKind(card.Kind))
            .Select(card => card.Id).ToHashSet();
        var poolIds = _discardPhaseHandDiscardIds.Where(damageIds.Contains).ToArray();
        AdvanceEventRulesAndQueueFact(new ShiHcCujunClaimSettledEvent(claim.FrameId, f.Id, f.OwnerSeat,
            claim.TargetSeat, _turnNumber, Array.AsReadOnly(poolIds.ToArray())));
        foreach (var cardId in poolIds)
        {
            if (_cardZones.GetLocation(cardId) is not { Zone: CardZoneKind.DiscardPile }) continue;
            MoveCard(GetAdvancedCard(cardId), CardLocation.DiscardPile, CardLocation.Hand(claim.TargetSeat),
                new CardMoveReason(ShiHuangChongComposition.CujunClaimReason), disclosure: CardMovementDisclosure.FaceUp);
            if (DrainShiHc(f.Id)) return;
            f = GetActiveProgramFrame(f.Id);
        }
        FinishShiHcOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- 死励: once per turn after the owner's first health change they may
    // take a Slash from the discard pile (or the draw pile topmost Slash) that
    // cannot be answered while they use it, or raise their next draw phase by
    // one card. Either choice beishui-arms one lost HP at the start of their
    // next turn.

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68760, SkillProgramEffectOp.ResolveShiHcSili)]
    private bool CanOfferShiHcSili(ProgramTriggerLegalityContext c)
    {
        if (c.SkillId != ShiHcSili || !c.Owner.IsAlive || _winner != Winner.None) return false;
        return c.Context.Window switch
        {
            SkillProgramTriggerWindow.AfterHealthChanged => true,
            SkillProgramTriggerWindow.DrawPhaseStarting =>
                ShiHcLedger().PendingDrawGrants(c.OwnerSeat, _turnNumber).Count > 0,
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow =>
                ShiHcLedger().PendingBeishui(c.OwnerSeat).Count > 0,
            _ => false,
        };
    }

    private SkillProgramStepOutcome BeginShiHcSili(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.HuangChong is not null)
            throw new InvalidOperationException("Shi Huang Chong restarted a running operation.");
        if (f.SkillId != ShiHcSili || f.SelectedCardIds is { Count: > 0 } || f.SelectedTargetSeats.Count != 0 ||
            !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        switch (f.WindowContext?.Window)
        {
            case SkillProgramTriggerWindow.DrawPhaseStarting:
                return SettleShiHcSiliDraw(f);
            case SkillProgramTriggerWindow.TurnStartBeforeNormalFlow:
                return SettleShiHcBeishui(f);
            case SkillProgramTriggerWindow.AfterHealthChanged:
                return PublishShiHcSiliBranch(f);
            default:
                return SkillProgramStepOutcome.Continue;
        }
    }

    private SkillProgramStepOutcome PublishShiHcSiliBranch(ProgramSkillFrame f)
    {
        var choices = new List<PromptChoice>();
        if (FindShiHcSlashToGain() is { } slash)
        {
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
            { HuangChong = new("sili", ShiHcStage.SiliBranch) { SlashCardId = slash.Id } });
            choices.Add(MakeShiHcBranchChoice(GetActiveProgramFrame(f.Id), "gain-slash",
                "从牌堆或弃牌堆中获得一张【杀】，且你使用此【杀】不可被响应"));
        }
        else
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = new("sili", ShiHcStage.SiliBranch) });
        choices.Add(MakeShiHcBranchChoice(GetActiveProgramFrame(f.Id), "draw-plus", "下个摸牌阶段摸牌数+1"));
        PublishShiHcChoices(GetActiveProgramFrame(f.Id), f.OwnerSeat, choices.ToArray(),
            $"【{GetShiHcSkillName(ShiHcSili)}】：选择一项。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private Card? FindShiHcSlashToGain()
    {
        var discardSlash = _cardZones.CardsAt(CardLocation.DiscardPile)
            .LastOrDefault(card => card.Kind == CardKind.Slash);
        if (discardSlash is not null) return discardSlash;
        return _cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(card => card.Kind == CardKind.Slash);
    }

    private SkillProgramStepOutcome SettleShiHcSiliDraw(ProgramSkillFrame f)
    {
        var grants = ShiHcLedger().PendingDrawGrants(f.OwnerSeat, _turnNumber);
        if (grants.Count == 0)
            throw new InvalidOperationException("Shi Huang Chong lost its armed draw grant.");
        ReplaceRuntimeTop(f = f with { HuangChong = new("sili", ShiHcStage.SiliDrawSettling) });
        foreach (var grant in grants)
            AdvanceEventRulesAndQueueFact(new ShiHcSiliDrawConsumedEvent(f.Id, grant.FrameId, f.OwnerSeat, _turnNumber));
        AdjustProgramNormalDraw(GetActiveProgramFrame(f.Id), grants.Count);
        if (DrainShiHc(f.Id)) return SkillProgramStepOutcome.AwaitChild;
        FinishShiHcOperation(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.Continue;
    }

    private SkillProgramStepOutcome SettleShiHcBeishui(ProgramSkillFrame f)
    {
        var armed = ShiHcLedger().PendingBeishui(f.OwnerSeat);
        if (armed.Count == 0)
            throw new InvalidOperationException("Shi Huang Chong lost its armed beishui cost.");
        ReplaceRuntimeTop(f = f with { HuangChong = new("sili", ShiHcStage.SiliBeishuiSettling) });
        foreach (var entry in armed)
            AdvanceEventRulesAndQueueFact(new ShiHcSiliBeishuiConsumedEvent(f.Id, entry.FrameId, f.OwnerSeat, _turnNumber));
        var outcome = ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, armed.Count);
        if (DrainShiHc(f.Id)) return SkillProgramStepOutcome.AwaitChild;
        if (outcome == SkillProgramStepOutcome.AwaitChild) return SkillProgramStepOutcome.AwaitChild;
        FinishShiHcOperation(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.Continue;
    }

    private void AnswerShiHcSiliBranch(ProgramSkillFrame f, ShiHcReceipt r, string branch)
    {
        switch (branch)
        {
            case "gain-slash":
                var slash = r.SlashCardId > 0 ? FindShiHcSlashToGain() : null;
                if (slash is null || slash.Id != r.SlashCardId)
                    throw new InvalidOperationException("Shi Huang Chong lost its offered Slash.");
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = r with { SlashCardId = 0 } });
                MoveCard(GetAdvancedCard(slash.Id), _cardZones.GetLocation(slash.Id), CardLocation.Hand(f.OwnerSeat),
                    new CardMoveReason(ShiHuangChongComposition.SiliGainReason),
                    disclosure: _cardZones.GetLocation(slash.Id).Zone == CardZoneKind.DrawPile
                        ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
                AdvanceEventRulesAndQueueFact(new ShiHcSiliSlashArmedEvent(f.Id, f.OwnerSeat, slash.Id, _turnNumber));
                AdvanceEventRulesAndQueueFact(new ShiHcSiliInvokedEvent(f.Id, f.OwnerSeat, _turnNumber, "slash"));
                break;
            case "draw-plus":
                AdvanceEventRulesAndQueueFact(new ShiHcSiliDrawGrantedEvent(f.Id, f.OwnerSeat, _turnNumber,
                    _phase == TurnPhase.Draw && _currentSeat == f.OwnerSeat));
                AdvanceEventRulesAndQueueFact(new ShiHcSiliInvokedEvent(f.Id, f.OwnerSeat, _turnNumber, "draw"));
                break;
            default:
                throw new InvalidOperationException("Shi Huang Chong answered an incompatible SiLi branch.");
        }
        AdvanceEventRulesAndQueueFact(new ShiHcSiliBeishuiArmedEvent(f.Id, f.OwnerSeat, _turnNumber));
        if (DrainShiHc(f.Id)) return;
        FinishShiHcOperation(GetActiveProgramFrame(f.Id));
    }

    private void FinishShiHcOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiHc(long id) => TryBeginProgramChildWindows(id, PostEventContinuation.Program);

    // ---- ledger ------------------------------------------------------------

    private sealed record ShiHcLedgerGrant(ShiHcCujunSlashGrantedEvent Grant, int Index);
    private sealed record ShiHcLedgerPlayStart(int Seat, int Index);

    private sealed record ShiHcLedgerView(
        IReadOnlyList<ShiHcLedgerGrant> CujunGrants,
        IReadOnlyList<ShiHcLedgerPlayStart> PlayStarts,
        IReadOnlyList<ShiHcSiliSlashArmedEvent> ArmedSlashes,
        IReadOnlyList<ShiHcSiliDrawGrantedEvent> DrawGrants,
        IReadOnlyList<long> ConsumedDrawFrameIds,
        IReadOnlyList<ShiHcSiliBeishuiArmedEvent> BeishuiArmed,
        IReadOnlyList<long> ConsumedBeishuiFrameIds,
        IReadOnlyList<ShiHcCujunClaimIssuedEvent> Claims,
        IReadOnlyList<long> SettledClaimFrameIds)
    {
        internal static readonly ShiHcLedgerView Empty = new([], [], [], [], [], [], [], [], []);

        internal ShiHcCujunClaimIssuedEvent? PendingClaim(int ownerSeat, int turnNumber) => Claims.FirstOrDefault(claim =>
            claim.OwnerSeat == ownerSeat && claim.TurnNumber == turnNumber && !SettledClaimFrameIds.Contains(claim.FrameId));

        internal IReadOnlyList<ShiHcSiliDrawGrantedEvent> PendingDrawGrants(int ownerSeat, int turnNumber) => DrawGrants
            .Where(grant => grant.OwnerSeat == ownerSeat && !ConsumedDrawFrameIds.Contains(grant.FrameId) &&
                !(grant.GrantedInDrawPhase && grant.TurnNumber == turnNumber))
            .ToArray();

        internal IReadOnlyList<ShiHcSiliBeishuiArmedEvent> PendingBeishui(int ownerSeat) => BeishuiArmed
            .Where(entry => entry.OwnerSeat == ownerSeat && !ConsumedBeishuiFrameIds.Contains(entry.FrameId))
            .ToArray();

        internal bool IsSlashArmed(int seat, int cardId) => ArmedSlashes.Any(entry =>
            entry.OwnerSeat == seat && entry.CardId == cardId);
    }

    private ShiHcLedgerView? _shiHcLedgerCache;
    private int _shiHcLedgerCacheCount = -1;
    private IGameEvent? _shiHcLedgerCacheHead;
    private IGameEvent? _shiHcLedgerCacheTail;

    private ShiHcLedgerView ShiHcLedger()
    {
        if (!HasShiHcCapability) return ShiHcLedgerView.Empty;
        var history = CompleteProgramEventHistory().ToArray();
        var head = history.FirstOrDefault();
        var tail = history.LastOrDefault();
        if (_shiHcLedgerCache is not null && _shiHcLedgerCacheCount == history.Length &&
            ReferenceEquals(_shiHcLedgerCacheHead, head) && ReferenceEquals(_shiHcLedgerCacheTail, tail))
            return _shiHcLedgerCache;
        var grants = new List<ShiHcLedgerGrant>();
        var playStarts = new List<ShiHcLedgerPlayStart>();
        var armedSlashes = new List<ShiHcSiliSlashArmedEvent>();
        var drawGrants = new List<ShiHcSiliDrawGrantedEvent>();
        var consumedDraws = new List<long>();
        var beishuiArmed = new List<ShiHcSiliBeishuiArmedEvent>();
        var consumedBeishui = new List<long>();
        var claims = new List<ShiHcCujunClaimIssuedEvent>();
        var settledClaims = new List<long>();
        for (var index = 0; index < history.Length; index++)
        {
            switch (history[index])
            {
                case ShiHcCujunSlashGrantedEvent grant: grants.Add(new(grant, index)); break;
                case PhaseChangedEvent { Phase: TurnPhase.Play } play: playStarts.Add(new(play.ActorSeat, index)); break;
                case ShiHcSiliSlashArmedEvent armed: armedSlashes.Add(armed); break;
                case ShiHcSiliDrawGrantedEvent draw: drawGrants.Add(draw); break;
                case ShiHcSiliDrawConsumedEvent drawConsumed: consumedDraws.Add(drawConsumed.GrantedFrameId); break;
                case ShiHcSiliBeishuiArmedEvent beishui: beishuiArmed.Add(beishui); break;
                case ShiHcSiliBeishuiConsumedEvent beishuiConsumed: consumedBeishui.Add(beishuiConsumed.ArmedFrameId); break;
                case ShiHcCujunClaimIssuedEvent claim: claims.Add(claim); break;
                case ShiHcCujunClaimSettledEvent settled: settledClaims.Add(settled.ClaimFrameId); break;
            }
        }
        _shiHcLedgerCache = new ShiHcLedgerView(Array.AsReadOnly(grants.ToArray()), Array.AsReadOnly(playStarts.ToArray()),
            Array.AsReadOnly(armedSlashes.ToArray()),
            Array.AsReadOnly(drawGrants.ToArray()), Array.AsReadOnly(consumedDraws.ToArray()),
            Array.AsReadOnly(beishuiArmed.ToArray()), Array.AsReadOnly(consumedBeishui.ToArray()),
            Array.AsReadOnly(claims.ToArray()), Array.AsReadOnly(settledClaims.ToArray()));
        _shiHcLedgerCacheCount = history.Length;
        _shiHcLedgerCacheHead = head;
        _shiHcLedgerCacheTail = tail;
        return _shiHcLedgerCache;
    }

    // The target's next play phase after the grant is the first Play-phase fact
    // of that seat recorded after the grant; the bonus applies exactly while
    // that phase is running.
    private int ShiHcCujunSlashBonus(int seat)
    {
        if (!HasShiHcCapability || _winner != Winner.None || !IsValidPlayerSeat(seat) ||
            _phase != TurnPhase.Play || _currentSeat != seat)
            return 0;
        var view = ShiHcLedger();
        return view.CujunGrants.Count(grant => grant.Grant.TargetSeat == seat &&
            _cardUseDebitPhaseInstanceId > grant.Grant.GrantedPhaseInstanceId &&
            view.PlayStarts.Count(play => play.Seat == seat && play.Index > grant.Index) == 1);
    }

    private bool IsShiHcSiliSlashUnrespondable(long frameId) =>
        HasShiHcCapability && _winner == Winner.None &&
        _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId) is
            { Action: { Type: CardActionType.Use } action, SourceSeat: var seat } &&
        IsSlashCard(action.EffectiveKind) && action.PhysicalCards.Count == 1 &&
        ShiHcLedger().IsSlashArmed(seat, action.PhysicalCards[0].CardId);

    // ---- prompts -----------------------------------------------------------

    private string GetShiHcSkillName(string skillId) => _contentRegistry.GetSkill(skillId).Name;

    private PromptChoice MakeShiHcSeatChoice(ProgramSkillFrame f, string prefix, int seat, string label) =>
        new(new($"shi-huang-chong.{f.Id}.{prefix}-{seat}"), label, [], [seat],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiHuangChongComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = $"{prefix}-{seat}",
            });

    private PromptChoice MakeShiHcBranchChoice(ProgramSkillFrame f, string branch, string label) =>
        new(new($"shi-huang-chong.{f.Id}.{branch}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiHuangChongComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });

    private void PublishShiHcChoices(ProgramSkillFrame f, int answeringSeat, IReadOnlyList<PromptChoice> choices,
        string prompt)
    {
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, answeringSeat, prompt,
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = answeringSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _status = _players[answeringSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    private IReadOnlyList<PromptChoice> ShiHcCurrentChoices(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        return (r.Operation, r.Stage) switch
        {
            ("cujun", ShiHcStage.CujunTarget) => _players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat)
                .Select(player => MakeShiHcSeatChoice(f, "target", player.Seat, $"选择 {_players[player.Seat].Name}")).ToArray(),
            ("cujun", ShiHcStage.CujunChoosers) when r.Answered < r.Choosers.Length =>
                ShiHcChooserChoices(f, r.Choosers[r.Answered]),
            ("sili", ShiHcStage.SiliBranch) => ShiHcSiliBranchChoices(f, r),
            _ => [],
        };
    }

    private IReadOnlyList<PromptChoice> ShiHcSiliBranchChoices(ProgramSkillFrame f, ShiHcReceipt r)
    {
        var choices = new List<PromptChoice>();
        if (r.SlashCardId > 0 && FindShiHcSlashToGain() is { } slash && slash.Id == r.SlashCardId)
            choices.Add(MakeShiHcBranchChoice(f, "gain-slash",
                "从牌堆或弃牌堆中获得一张【杀】，且你使用此【杀】不可被响应"));
        choices.Add(MakeShiHcBranchChoice(f, "draw-plus", "下个摸牌阶段摸牌数+1"));
        return choices;
    }

    [ProgramAnswer(ShiHuangChongComposition.ProgramAction)]
    private void AnswerShiHc(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { HuangChong: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiHcCurrentChoices(f).Any(choice => choice.Id == selected.Id &&
                choice.Cards.SequenceEqual(selected.Cards) && choice.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Shi Huang Chong choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch.StartsWith("target-", StringComparison.Ordinal))
        { AnswerShiHcCujunTarget(GetActiveProgramFrame(f.Id), r, branch); return; }
        if (r.Operation == "cujun" && r.Stage == ShiHcStage.CujunChoosers &&
            branch.StartsWith("gift-", StringComparison.Ordinal))
        {
            var separator = branch.IndexOf('-', "gift-".Length);
            var cardId = int.Parse(branch[(separator + 1)..], CultureInfo.InvariantCulture);
            var gifts = r.GiftCards.ToArray();
            gifts[r.Answered] = cardId;
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = r with
            { Answered = r.Answered + 1, GiftCards = gifts } });
            ContinueShiHcChoosers(GetActiveProgramFrame(f.Id));
            return;
        }
        if (r.Operation == "cujun" && r.Stage == ShiHcStage.CujunChoosers && branch == "slash-plus")
        {
            ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { HuangChong = r with { Answered = r.Answered + 1 } });
            ContinueShiHcChoosers(GetActiveProgramFrame(f.Id));
            return;
        }
        if (r.Operation == "sili" && r.Stage == ShiHcStage.SiliBranch)
        { AnswerShiHcSiliBranch(GetActiveProgramFrame(f.Id), r, branch); return; }
        throw new InvalidOperationException("Shi Huang Chong answered an incompatible stage.");
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 68750)]
    private bool ResumeShiHcCujun(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { HuangChong: { } r } f || f.Id != id ||
            r.Operation != "cujun") return false;
        switch (r.Stage)
        {
            case ShiHcStage.CujunSettling:
                SettleShiHcCujun(GetActiveProgramFrame(id));
                return true;
            case ShiHcStage.CujunClaimSettling:
                SettleShiHcClaim(GetActiveProgramFrame(id));
                return true;
            case ShiHcStage.CujunChoosers when r.Answered >= r.Choosers.Length:
                SettleShiHcCujun(GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiHc(id)) return true;
                ContinueShiHcPrompt(GetActiveProgramFrame(id));
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 68760)]
    private bool ResumeShiHcSili(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { HuangChong: { } r } f || f.Id != id ||
            r.Operation != "sili") return false;
        switch (r.Stage)
        {
            case ShiHcStage.SiliDrawSettling or ShiHcStage.SiliBeishuiSettling:
                if (DrainShiHc(id)) return true;
                FinishShiHcOperation(GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiHc(id)) return true;
                ContinueShiHcPrompt(GetActiveProgramFrame(id));
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68750)]
    private bool ReturnShiHcMovement(ProgramSkillFrame f) =>
        f.HuangChong is { } r && (r.Operation != "cujun" ? ResumeShiHcSili(f.Id) : ResumeShiHcCujun(f.Id));

    private void ContinueShiHcPrompt(ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        switch (r.Operation, r.Stage)
        {
            case ("cujun", ShiHcStage.CujunTarget):
                PublishShiHcChoices(f, f.OwnerSeat, _players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat)
                        .Select(player => MakeShiHcSeatChoice(f, "target", player.Seat, $"选择 {_players[player.Seat].Name}"))
                        .ToArray(),
                    $"【{GetShiHcSkillName(ShiHcCujun)}】：选择一名角色。");
                return;
            case ("cujun", ShiHcStage.CujunChoosers) when r.Answered < r.Choosers.Length:
                PublishShiHcChoices(f, r.Choosers[r.Answered], ShiHcChooserChoices(f, r.Choosers[r.Answered]),
                    $"【{GetShiHcSkillName(ShiHcCujun)}】：{_players[f.OwnerSeat].Name} 对 {_players[r.TargetSeat].Name} 发动促军，请选择一项。");
                return;
            case ("sili", ShiHcStage.SiliBranch):
                PublishShiHcChoices(f, f.OwnerSeat, ShiHcSiliBranchChoices(f, r),
                    $"【{GetShiHcSkillName(ShiHcSili)}】：选择一项。");
                return;
            default:
                FinishShiHcOperation(f);
                return;
        }
    }

    // ---- AI ----------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHcCujun)]
    private PromptChoice ChooseShiHcCujunAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.HuangChong!;
        if (r.Stage == ShiHcStage.CujunChoosers)
        {
            // Choosers deterministically raise the target's Slash quota instead
            // of parting with a hand card.
            var slashPlus = decision.Choices.FirstOrDefault(choice =>
                choice.Parameters.GetValueOrDefault("branch") == "slash-plus");
            if (slashPlus is not null) return slashPlus;
        }
        return decision.Choices[0];
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiHcSili)]
    private PromptChoice ChooseShiHcSiliAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var gain = decision.Choices.FirstOrDefault(choice =>
            choice.Parameters.GetValueOrDefault("branch") == "gain-slash");
        if (gain is not null) return gain;
        return decision.Choices[0];
    }
}

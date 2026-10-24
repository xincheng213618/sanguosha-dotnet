namespace CardGame.Core;

/// <summary>
/// Optional match-local shop, economy and tactic progression. The feature never
/// receives the engine or owns a second command session or resolution stack.
/// </summary>
internal sealed partial class WushuangModeFeature(int seed, ContentRegistry definitions, IWushuangModeHost host)
{
    internal const string GrantSource = "mode:wushuang-shop";
    internal const string OriginalReplacementSource = "mode:wushuang-replaced-original";
    private const string WushuangGrantSource = GrantSource;
    private const string WushuangOriginalReplacementSource = OriginalReplacementSource;
    private readonly int _seed = seed;
    private readonly ContentRegistry _definitions = definitions;
    private readonly IWushuangModeHost _host = host;
    private readonly Dictionary<int, WushuangPlayerProgress> _players = [];
    private bool _wushuangOpeningCompleted;
    internal int ElapsedMilliseconds { get; set; }
    internal int RemainingMilliseconds => Math.Max(0, WushuangMode.TimeLimitMilliseconds - ElapsedMilliseconds);
    private int? WushuangRemainingMilliseconds => RemainingMilliseconds;
    private bool IsWushuangClockRunning => _host.ClockRunning;
    // Temporary native bridges use this same state; no engine-side copy exists.
    internal IReadOnlyDictionary<int, WushuangPlayerProgress> Players => _players;
    internal IReadOnlyList<int> TacticIds(int seat) => Array.AsReadOnly(_players.GetValueOrDefault(seat)?.Tactics.ToArray() ?? []);
    private bool IsValidSeat(int seat) => seat >= 0 && seat < _host.PlayerCount;
    internal WushuangIntermissionFrame? ActiveWushuangWindow(int seat) =>
        _host.Frames.LastOrDefault() is WushuangIntermissionFrame f && f.SeatCursor == seat ? f : null;
    internal bool HasWushuangIntermissionBoundary() => _host.Frames.FirstOrDefault() is WushuangIntermissionFrame f &&
        f.SeatCursor >= 0 && f.SeatCursor <= _host.PlayerCount && IsValidSeat(f.EndedSeat) &&
        _host.Frames.Count(frame => frame is WushuangIntermissionFrame) == 1;
    public WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat)
    {
        if (!IsValidSeat(viewerSeat)) return null;
        var p = _players.GetValueOrDefault(viewerSeat);
        var f = _host.Frames.OfType<WushuangIntermissionFrame>().LastOrDefault(frame => frame.SeatCursor == viewerSeat);
        var extra = _host.Player(viewerSeat).SkillGrants.Where(g => g.SourceId == WushuangGrantSource).Select(g => g.SkillId).ToArray();
        return new(viewerSeat, 1, 0, 0, p?.Tokens ?? 0, Array.AsReadOnly(extra),
            p?.Offers ?? Array.AsReadOnly(Array.Empty<WushuangShopOffer>()), CanUseWushuangShop(viewerSeat), p?.FreeRerolls > 0 ? 0 : 1, 0, WushuangRemainingMilliseconds!.Value, _host.Revision)
        {
            FirstNormalSeat = _host.FirstNormalSeat,
            ExtraSkillCapacity = WushuangExtraSkillCapacity(viewerSeat), FreeRerolls = p?.FreeRerolls ?? 0,
            ReplaceableOriginalSkillIds = Array.AsReadOnly(WushuangReplaceableOriginalSkills(viewerSeat).ToArray()),
            Phase = f is null ? WushuangShopPhase.Closed : f.Opening ? WushuangShopPhase.OpeningTactic : WushuangShopPhase.Shopping,
            WindowId = f?.Id ?? 0, IsLocked = p?.IsLocked == true,
            PurchasedOffers = p?.Purchased ?? Array.AsReadOnly(Array.Empty<WushuangShopOffer>()),
            TacticIds = Array.AsReadOnly(p?.Tactics.ToArray() ?? [])
        };
    }
    internal bool CanUseWushuangShop(int seat) => IsWushuangClockRunning && IsValidSeat(seat) && _host.Player(seat).IsHuman &&
        _host.Player(seat).IsAlive && ActiveWushuangWindow(seat) is not null &&
        _host.PendingDecision is { Kind: DecisionKind.ProgramTrigger } p && p.PlayerSeat == seat && p.SkillPrompt?.SkillId == WushuangMode.ModeId;
    internal void InitializeWushuangShops(GameStartedEvent started)
    {
        if (_players.Count != 0) return;
        foreach (var p in _host.Players) _players.Add(p.Seat, new());
    }
    internal void ProgressWushuangEconomy(TurnStartedEvent started)
    {
        if (_host.HasWinner || !_host.IsNormalTurn) return;
        foreach (var p in _host.Players.Where(p => p.IsAlive))
        {
            var tactics = _players[p.Seat].Tactics;
            var income = 1 + (tactics.Contains(2286) ? 1 : 0) + (tactics.Contains(2285) && p.Seat == started.ActorSeat ? 1 : 0) +
                (tactics.Contains(2281) && _host.HasReachedFiveTokens(p.Seat) ? 1 : 0);
            ChangeWushuangTokens(p.Seat, tactics.Contains(2366) ? income * 2 : income, "normal-turn-start");
            if (tactics.Contains(2290) && _players[p.Seat].FreeRerolls == 0) ChangeWushuangFreeRerolls(p.Seat, 1, "turn-start");
        }
    }
    internal void ProgressWushuangRound(RoundStartedEvent started)
    {
        if (!_host.HasWinner)
            foreach (var p in _host.Players.Where(p => p.IsAlive)) ChangeWushuangTokens(p.Seat, 1, "round-start");
    }
    internal bool TryBeginWushuangOpening()
    {
        if (!IsWushuangClockRunning || _wushuangOpeningCompleted || _players.Count != _host.PlayerCount) return false;
        foreach (var p in _host.Players) RefreshWushuangOffers(p.Seat, true);
        var f = new WushuangIntermissionFrame(_host.NextFrameId(), true, _host.CurrentSeat, false);
        _host.PushFrame(f); ContinueWushuangIntermission(f.Id); return true;
    }
    internal bool TryBeginWushuangShopping(int endedSeat, bool skipped)
    {
        if (!IsWushuangClockRunning || !_host.IsNormalTurn) return false;
        var f = new WushuangIntermissionFrame(_host.NextFrameId(), false, endedSeat, skipped);
        _host.PushFrame(f); ContinueWushuangIntermission(f.Id); return true;
    }
    internal void ContinueWushuangIntermission(long frameId)
    {
        if (_host.PendingDecision is not null || _host.Frames.LastOrDefault() is not WushuangIntermissionFrame f || f.Id != frameId) return;
        if (f.SeatCursor >= _host.PlayerCount || _host.HasWinner)
        {
            _host.PopFrame(f.Id, ResolutionFrameKind.WushuangIntermission);
            if (f.Opening) _wushuangOpeningCompleted = true;
            if (!_host.HasWinner)
            {
                // The in-turn preview must be the exact next ending's page.
                foreach (var next in _host.Players.Where(player => player.IsAlive))
                    if (!_players[next.Seat].IsLocked) RefreshWushuangOffers(next.Seat);
                if (!f.Opening) _host.ContinueAfterShopping(f.EndedSeat, f.Skipped);
            }
            return;
        }
        var p = _host.Player(f.SeatCursor); var progress = _players[p.Seat];
        if (!p.IsAlive) { AdvanceWushuangShopSeat(f); return; }
        if (!p.IsHuman)
        {
            if (_host.TryContinueAiShopping(p.Seat, f)) return;
            if (f.Opening)
            {
                var offer = progress.Offers.OrderByDescending(o => WushuangTactics.ById[o.TacticId!.Value].Cost).First();
                BuyWushuangOffer(p.Seat, offer, null, true);
                if (_host.TryBeginAcquisition(f.Id, offer)) return;
            }
            else if (RunWushuangAiShop(p, f.Id)) return;
            AdvanceWushuangShopSeat(f); return;
        }
        var choices = f.Opening ? progress.Offers.Select(o => new PromptChoice(new("wushuang.opening." + o.Id), o.Name, [], [],
            new Dictionary<string, string> { ["program-action"] = "wushuang-choice", ["offer-id"] = o.Id })).ToArray()
            : [new PromptChoice(new("wushuang.finish." + f.Id), "结束购买", [], [], new Dictionary<string, string> { ["program-action"] = "wushuang-choice" })];
        _host.SetPrompt(new(DecisionKind.ProgramTrigger, p.Seat, f.Opening ? "请选择一个初始战法" : "请选择购买商品", [], [])
        { PromptId = _host.NextPromptId(), Choices = Array.AsReadOnly(choices), IsPrivate = true,
            SkillPrompt = new(WushuangMode.ModeId, f.Opening ? "初始奖励" : "虎符商店", "单骑无双", "战法不占额外技能槽。") });
    }
    internal void AnswerWushuangChoice(PromptChoice choice)
    {
        if (_host.PendingDecision is not { } p || ActiveWushuangWindow(p.PlayerSeat) is not { } f)
            throw new InvalidOperationException("Wushuang choice lost its owning frame.");
        var offer = f.Opening ? _players[p.PlayerSeat].Offers.Single(o => o.Id == choice.Parameters["offer-id"]) : null;
        _host.ClearPrompt();
        if (offer is not null) BuyWushuangOffer(p.PlayerSeat, offer, null, true);
        if (offer is not null && _host.TryBeginAcquisition(f.Id, offer)) return;
        AdvanceWushuangShopSeat(f);
    }
    internal void AdvanceWushuangShopSeat(WushuangIntermissionFrame f)
    {
        // Commit the next seat before any movement children run: replay or recovery
        // must never pay this purchase or repeat this AI action a second time.
        _host.ReplaceFrame(f.Id, f with { SeatCursor = f.SeatCursor + 1 });
        _host.AdvanceState();
        if (_host.Frames.LastOrDefault()?.Id == f.Id) ContinueWushuangIntermission(f.Id);
    }
    internal void ChangeWushuangTokens(int seat, int delta, string reason)
    {
        var p = _players[seat]; p.Tokens = checked(p.Tokens + delta);
        if (p.Tokens < 0) throw new InvalidOperationException("Wushuang currency cannot be negative.");
        _host.PublishFact(new WushuangTokensChangedEvent(seat, delta, p.Tokens, reason));
    }
    internal void RefreshWushuangOffers(int seat, bool opening = false)
    {
        var p = _players[seat]; p.RefreshNumber++;
        var random = new DeterministicRandom(unchecked(_seed * 397 ^ seat * 7919 ^ p.RefreshNumber * 104729));
        var tactics = WushuangTactics.All.Where(t => t.IsImplemented && (opening || !t.StartOnly) && !p.Tactics.Contains(t.Id) &&
            (t.SkillId is null || _definitions.Skills.TryGetValue(t.SkillId, out var definition) && definition.ImplementationStatus == SkillImplementationStatus.Complete)).ToList();
        random.Shuffle(tactics); var offers = new List<WushuangShopOffer>();
        // A removed entry must not reorder all surviving opening candidates.
        if (opening) foreach (var t in tactics.Where(t => t.IsAvailableInShop).Take(3)) offers.Add(TacticOffer(t, offers.Count, 0));
        else
        {
            var owned = _host.EnabledSkillIds(seat).ToHashSet(StringComparer.Ordinal);
            var skills = WushuangCatalogue.Skills.Where(item => item.IsAvailableInShop && item.SkillId is { } id &&
                _definitions.Skills.TryGetValue(id, out var s) && s.ImplementationStatus == SkillImplementationStatus.Complete &&
                WushuangCatalogue.IsSkillAllowed(s) && !owned.Contains(id)).ToList();
            var candidates = skills.Select(skill => new WushuangShopOffer("", skill.Name, skill.Description,
                skill.Cost, skill.SkillId, WushuangOfferKind.Skill, 1) { Quality = skill.Quality })
                .Concat(tactics.Where(t => t.IsAvailableInShop).Select(t => TacticOffer(t, 0, t.Cost)))
                .Concat(WushuangCatalogue.Cards.Where(card => card.IsAvailableInShop && card.Kind is { } kind &&
                    (!card.IsEquipment || _host.EquipmentSlotCapacity(seat, _host.GetEquipmentSlot(kind)) > 0))
                    .Select(card => new WushuangShopOffer("", card.Name,
                        card.IsEquipment ? card.Description + "\n购买后立即装备，离开装备区销毁。" : "获得这张手牌。",
                        card.Cost, null, WushuangOfferKind.Card, 1)
                        { CardKind = card.Kind, CardOfferId = card.Id, Quality = card.Quality })).ToList();
            // The server's weights are not public. Local sampling gives every
            // eligible catalogue entry equal weight, without fixed type slots.
            random.Shuffle(candidates);
            offers.AddRange(candidates.Take(p.Tactics.Contains(2289) ? 5 : 4)
                .Select((offer, slot) => offer with { Id = Id(slot) }));
        }
        p.Offers = Array.AsReadOnly(offers.ToArray()); p.Purchased = Array.AsReadOnly(Array.Empty<WushuangShopOffer>());
        _host.PublishFact(new WushuangShopRefreshedEvent(seat, p.RefreshNumber));
        string Id(int slot) => $"wushuang:{seat}:{p.RefreshNumber}:{slot}:offer";
        WushuangShopOffer TacticOffer(WushuangTactic t, int slot, int cost) => new(Id(slot), t.Name, t.Description, cost, null, WushuangOfferKind.Tactic, 1) { TacticId = t.Id, Quality = t.Quality };
    }
    internal CommandResult SubmitBuyWushuangOffer(BuyWushuangOfferCommand command)
    {
        if (!CanUseWushuangShop(command.ActorSeat)) return _host.Reject(CommandErrorCode.IllegalAction, "当前不在虎符商店购买阶段。");
        var f = ActiveWushuangWindow(command.ActorSeat)!;
        var offer = _players[command.ActorSeat].Offers.SingleOrDefault(o => o.Id == command.OfferId);
        var error = ValidateWushuangPurchase(command.ActorSeat, offer, command.ReplaceSkillId);
        if (error is not null) return _host.Reject(CommandErrorCode.IllegalAction, error);
        return _host.Accept(() => { _host.ClearPrompt(); BuyWushuangOffer(command.ActorSeat, offer!, command.ReplaceSkillId, f.Opening);
            if (_host.TryBeginAcquisition(f.Id, offer!)) return;
            if (f.Opening)
            {
                AdvanceWushuangShopSeat(f);
                if (_host.AdvanceAfterHumanCommands) _host.AdvanceToHumanBoundary();
            }
            else CompleteWushuangShopAction(f.Id); });
    }
    internal string? ValidateWushuangPurchase(int seat, WushuangShopOffer? offer, string? replacement)
    {
        if (offer is null || offer.Cost > _players[seat].Tokens) return "商品已刷新或虎符不足。";
        if (offer.CardKind is { } unavailableCard && !WushuangCatalogue.IsCardAllowed(unavailableCard) ||
            offer.SkillId is { } unavailableSkill && !WushuangCatalogue.IsSkillAllowed(_definitions.GetSkill(unavailableSkill)) ||
            offer.TacticId is { } unavailableTactic && !WushuangTactics.ById[unavailableTactic].IsAvailableInShop)
            return "此商品不适用于单骑无双。";
        if (offer.TacticId is { } t && _players[seat].Tactics.Contains(t)) return "已拥有这个战法。";
        if (offer.CardKind is { } cardKind && _host.IsEquipment(cardKind) &&
            _host.EquipmentSlotCapacity(seat, _host.GetEquipmentSlot(cardKind)) == 0) return "对应装备栏已废除。";
        var extra = _host.Player(seat).SkillGrants.Where(g => g.SourceId == WushuangGrantSource).ToArray();
        if (offer.SkillId is null) return replacement is null ? null : "战法与卡牌不占额外技能槽。";
        if (_host.EnabledSkillIds(seat).Contains(offer.SkillId, StringComparer.Ordinal)) return "已拥有这个技能。";
        if (replacement is not null && !extra.Any(g => g.SkillId == replacement) && !WushuangReplaceableOriginalSkills(seat).Contains(replacement))
            return "请选择可替换的额外技能；一忘皆空允许替换武将初始技能。";
        return extra.Length >= WushuangExtraSkillCapacity(seat) && replacement is null ? "额外技能槽已满，请选择要替换的技能。" : null;
    }
    internal void BuyWushuangOffer(int seat, WushuangShopOffer offer, string? replacement, bool opening = false)
    {
        var p = _players[seat]; var player = _host.Player(seat);
        if (offer.Cost != 0) ChangeWushuangTokens(seat, -offer.Cost, "purchase");
        p.Offers = Array.AsReadOnly(p.Offers.Where(o => o.Id != offer.Id).ToArray()); p.Purchased = Array.AsReadOnly(p.Purchased.Append(offer).ToArray());
        if (offer.SkillId is { } skill)
        {
            var original = replacement is not null && WushuangReplaceableOriginalSkills(seat).Contains(replacement);
            if (replacement is not null)
                foreach (var g in _host.Player(seat).SkillGrants.Where(g => g.SkillId == replacement &&
                    (g.SourceId == WushuangGrantSource || original && IsWushuangOriginalGrant(g))).ToArray())
                    _host.RemoveSkillGrant(seat, g.GrantId);
            Grant(skill, original ? WushuangOriginalReplacementSource : WushuangGrantSource);
        }
        else if (offer.TacticId is { } t)
        {
            p.Tactics.Add(t); if (WushuangTactics.ById[t].SkillId is { } passive) Grant(passive, "mode:wushuang-tactic");
            if (t is 2287 or 2288 or 2369) ChangeWushuangTokens(seat, t == 2287 ? 1 : t == 2288 ? 2 : 5, "tactic-acquired");
            if (t is 2291 or 2292) ChangeWushuangFreeRerolls(seat, t == 2291 ? 2 : 4, "tactic-acquired");
            if (t == 2290 && p.FreeRerolls == 0) ChangeWushuangFreeRerolls(seat, 1, "tactic-acquired");
            _host.PublishFact(new WushuangTacticAcquiredEvent(seat, t, opening));
        }
        else if (offer.CardOfferId is { } cardOfferId) _host.PlaceShopCard(seat, offer.Id, WushuangCatalogue.Cards.Single(c => c.Id == cardOfferId));
        else if (offer.CardKind is { } kind) _host.PlaceFallbackCard(seat, kind);
        _host.PublishFact(new WushuangOfferBoughtEvent(seat, offer.Id, offer.SkillId, replacement, offer.Cost));
        var itemName = offer.CardKind is { } bought && !_host.IsEquipment(bought) ? "一张手牌" : $"【{offer.Name}】";
        _host.LogPurchase( $"{player.Name}{(opening ? "选择初始战法" : $"花费{offer.Cost}虎符购买")}{itemName}。", seat);
        void Grant(string id, string source)
        { var g = $"{source}:{seat}:{offer.Id}"; _host.GrantSkill(seat, new(g, id, g, source)); }
    }
    internal CommandResult SubmitRerollWushuangShop(RerollWushuangShopCommand c)
    {
        if (!CanUseWushuangShop(c.ActorSeat) || ActiveWushuangWindow(c.ActorSeat) is not { Opening: false } f ||
            _players[c.ActorSeat] is { FreeRerolls: 0, Tokens: < 1 })
            return _host.Reject(CommandErrorCode.IllegalAction, "当前不能刷新商店，或虎符不足。");
        return _host.Accept(() => { _host.ClearPrompt();
            if (_players[c.ActorSeat].FreeRerolls > 0) ChangeWushuangFreeRerolls(c.ActorSeat, -1, "refresh");
            else ChangeWushuangTokens(c.ActorSeat, -1, "refresh");
            RefreshWushuangOffers(c.ActorSeat); CompleteWushuangShopAction(f.Id); });
    }
    internal CommandResult SubmitUpgradeWushuangShop(UpgradeWushuangShopCommand c) => _host.Reject(CommandErrorCode.IllegalAction, "单骑无双没有经验或商店升级。");
    internal CommandResult SubmitFinishWushuangShop(FinishWushuangShopCommand c)
    {
        if (!CanUseWushuangShop(c.ActorSeat) || ActiveWushuangWindow(c.ActorSeat) is not { Opening: false } f)
            return _host.Reject(CommandErrorCode.IllegalAction, "请先选择初始战法，或等待商店开放。");
        return _host.Accept(() => { _host.ClearPrompt(); AdvanceWushuangShopSeat(f); if (_host.AdvanceAfterHumanCommands) _host.AdvanceToHumanBoundary(); });
    }
    internal CommandResult SubmitToggleWushuangShopLock(ToggleWushuangShopLockCommand c)
    {
        if (!CanUseWushuangShop(c.ActorSeat) || ActiveWushuangWindow(c.ActorSeat) is not { Opening: false }) return _host.Reject(CommandErrorCode.IllegalAction, "只能在购买阶段锁定商品。");
        return _host.Accept(() => { var p = _players[c.ActorSeat]; p.IsLocked = !p.IsLocked; _host.PublishFact(new WushuangShopLockChangedEvent(c.ActorSeat, p.IsLocked)); _host.AdvanceState(); });
    }
    internal void CompleteWushuangShopAction(long frameId)
    {
        _host.AdvanceState(); if (_host.Frames.LastOrDefault()?.Id == frameId) ContinueWushuangIntermission(frameId);
        if (_host.AdvanceAfterHumanCommands) _host.AdvanceToHumanBoundary();
    }
    internal bool RunWushuangAiShop(WushuangPlayerFacts player, long windowId)
    {
        var p = _players[player.Seat]; var extras = player.SkillGrants.Count(g => g.SourceId == WushuangGrantSource);
        var offer = p.Offers.OrderBy(o => o.Kind == WushuangOfferKind.Tactic ? 0 : o.Kind == WushuangOfferKind.Skill ? 1 : 2)
            .FirstOrDefault(o => !(o.SkillId is not null && extras >= WushuangExtraSkillCapacity(player.Seat)) && ValidateWushuangPurchase(player.Seat, o, null) is null);
        if (offer is null) return false;
        BuyWushuangOffer(player.Seat, offer, null);
        return _host.TryBeginAcquisition(windowId, offer);
    }
}

internal sealed class WushuangPlayerProgress
{
    internal int Tokens;
    internal int RefreshNumber;
    internal int FreeRerolls;
    internal bool IsLocked;
    internal readonly List<int> Tactics = [];
    internal IReadOnlyList<WushuangShopOffer> Offers = Array.AsReadOnly(Array.Empty<WushuangShopOffer>());
    internal IReadOnlyList<WushuangShopOffer> Purchased = Array.AsReadOnly(Array.Empty<WushuangShopOffer>());
}

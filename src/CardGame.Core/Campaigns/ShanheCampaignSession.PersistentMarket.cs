namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private bool _battleMarketSession;

    private sealed partial record RunState
    {
        public string? MarketNodeId { get; init; }
        public int MarketChapterNumber { get; init; }
        public int? MarketHealthMaximumHp { get; init; }
        public int MarketHealthBonusAtSync { get; init; }
    }

    public bool HasPersistentMarket => _options.PersistentMarket || _battleMarketSession;
    public IReadOnlyList<ShanheNodeDefinition> VisibleMapNodes => !_options.PersistentMarket ? Definition.Nodes :
        ShanheCampaignContent.Freeze(Definition.Nodes.Where(node => node.Kind != ShanheNodeKind.Shop)
            .Select(node => node with { NextNodeIds = ResolveVisibleNodeIds(node.NextNodeIds) }));

    public IReadOnlyList<string> ResolveVisibleNodeIds(IEnumerable<string> nodeIds)
    {
        var result = new List<string>();
        var visited = new HashSet<string>(StringComparer.Ordinal);
        void Visit(string id)
        {
            if (!visited.Add(id)) return;
            if (_options.PersistentMarket && _nodes[id].Kind == ShanheNodeKind.Shop)
                foreach (var next in _nodes[id].NextNodeIds) Visit(next);
            else result.Add(id);
        }
        foreach (var id in nodeIds) Visit(id);
        return ShanheCampaignContent.Freeze(result);
    }

    public static ShanheCampaignSession CreateBattleMarketSession(ShanheBattleMarketSetup setup,
        Func<ShanheBattleSetup, ContentRegistry> registryFactory)
    {
        var frozen = ShanheBattleMarketSetup.Freeze(setup);
        Require(frozen.Options.UseCodexRewardPool, "A battle market requires the current catalogue reward pool.");
        var session = new ShanheCampaignSession(frozen.Definition,
            frozen.Options with { PersistentMarket = false, EquippedTacticIds = [] }, registryFactory)
            { _battleMarketSession = true };
        session._state = session.ReadMarketTransfer(session._state, frozen.State) with
        {
            Phase = ShanheCampaignPhase.NodeChoice, CurrentNodeId = frozen.State.MarketNodeId,
            AvailableNodeIds = []
        };
        Require(session._state.Hp > 0 && session._state.Hp <= session.MarketMaximumHp(session._state),
            "A battle market must start from valid living campaign health.");
        session._snapshot = session.CreateSnapshot(session._state, 0);
        return session;
    }

    public ShanheBattleMarketTransfer CreateBattleMarketTransfer()
    {
        Require(HasPersistentMarket && _state.MarketNodeId is not null, "There is no persistent market to transfer.");
        return ExportMarketTransfer(_state, _battleMarketSession ? ContentHash :
            ShanheCampaignContent.GetContentHash(CreateMarketDefinition(_state)));
    }

    public string? ValidateMarketPurchase(string choiceId, string? replaceSkillId)
    {
        try { _ = PreparePersistentMarketPurchase(_state, choiceId, replaceSkillId); return null; }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or OverflowException or KeyNotFoundException)
        { return exception.Message; }
    }

    public string? ValidateMarketRefresh()
    {
        try { _ = RefreshPersistentMarket(_state); return null; }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or OverflowException or KeyNotFoundException)
        { return exception.Message; }
    }

    public string? ValidateMarketPurchase(string choiceId, string? replaceSkillId, int hp, int maxHp)
    {
        try { _ = PreparePersistentMarketPurchase(ProjectBattleMarketHealth(_state, hp, maxHp), choiceId, replaceSkillId); return null; }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or OverflowException or KeyNotFoundException)
        { return exception.Message; }
    }

    public string? ValidateMarketRefresh(int hp, int maxHp)
    {
        try { _ = RefreshPersistentMarket(ProjectBattleMarketHealth(_state, hp, maxHp)); return null; }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or OverflowException or KeyNotFoundException)
        { return exception.Message; }
    }

    public ShanheCampaignSnapshot CreateBattleMarketSnapshot(int hp, int maxHp) =>
        CreateSnapshot(ProjectBattleMarketHealth(_state, hp, maxHp), Revision);

    private RunState ProjectBattleMarketHealth(RunState state, int hp, int maxHp)
    {
        Require(_battleMarketSession && maxHp > 0 && hp <= maxHp,
            "Only a live battle market may project valid native health.");
        return state with { Hp = hp, MarketHealthMaximumHp = maxHp, MarketHealthBonusAtSync = state.Bonuses.MaxHp };
    }

    internal void SynchronizeBattleMarketHealth(int hp, int maxHp)
    {
        _state = ProjectBattleMarketHealth(_state, hp, maxHp);
        _snapshot = CreateSnapshot(_state, Revision);
    }

    private int MarketMaximumHp(RunState state) => _battleMarketSession && state.MarketHealthMaximumHp is { } maximum
        ? checked(maximum + state.Bonuses.MaxHp - state.MarketHealthBonusAtSync)
        : checked(_baseMaxHp + state.Bonuses.MaxHp);

    private bool CanTradePersistentMarket(RunState state) => HasPersistentMarket && state.MarketNodeId is not null &&
        (_battleMarketSession || state.Phase is ShanheCampaignPhase.Map or ShanheCampaignPhase.Reward or ShanheCampaignPhase.NodeChoice);

    private RunState MarketTradeState(RunState state) => state with
    { Phase = ShanheCampaignPhase.NodeChoice, CurrentNodeId = state.MarketNodeId };

    private static RunState ReturnMarketTrade(RunState original, RunState traded) => traded with
    { Phase = original.Phase, CurrentNodeId = original.CurrentNodeId, AvailableNodeIds = original.AvailableNodeIds };

    private RunState PreparePersistentMarketPurchase(RunState state, string choiceId, string? replaceSkillId)
    {
        Require(CanTradePersistentMarket(state), "This market is managed by the active battle or is not open.");
        Require(choiceId != "$leave" && state.ShopStock.Any(choice => choice.Id == choiceId && !choice.LeavesNode),
            "Choose a currently published persistent market product.");
        var traded = PrepareNodeOption(MarketTradeState(state), choiceId, replaceSkillId);
        traded = RestockPurchasedShopItem(traded, choiceId);
        return ReturnMarketTrade(state, traded);
    }

    private RunState RefreshPersistentMarket(RunState state)
    {
        Require(CanTradePersistentMarket(state), "This market is managed by the active battle or is not open.");
        return ReturnMarketTrade(state, RefreshShop(MarketTradeState(state)));
    }

    private RunState EnsurePersistentMarket(RunState state, int? chapter = null)
    {
        if (!_options.PersistentMarket || state.Phase is ShanheCampaignPhase.Won or ShanheCampaignPhase.Lost) return state;
        state = state with
        {
            AvailableNodeIds = ResolveVisibleNodeIds(state.AvailableNodeIds),
            OpeningResumeNodeIds = ResolveVisibleNodeIds(state.OpeningResumeNodeIds)
        };
        var chapterNumber = Math.Max(1, chapter ?? (state.Phase == ShanheCampaignPhase.Map && state.AvailableNodeIds.Count > 0
            ? state.AvailableNodeIds.Select(availableId => _nodes[availableId].ChapterNumber).Where(number => number > 0).DefaultIfEmpty(1).Min()
            : state.CurrentNodeId is { } currentNodeId ? _nodes[currentNodeId].ChapterNumber : 1));
        if (state.MarketNodeId is not null && state.MarketChapterNumber == chapterNumber) return state;
        var template = Definition.Nodes.FirstOrDefault(node => node.Kind == ShanheNodeKind.Shop && node.ChapterNumber == chapterNumber)
            ?? Definition.Nodes.FirstOrDefault(node => node.Kind == ShanheNodeKind.Shop);
        Require(template is not null, "A persistent expedition requires an authored market catalogue.");
        var next = state with
        {
            MarketNodeId = template!.Id, MarketChapterNumber = chapterNumber,
            ShopStock = [], PurchasedChoiceIds = [], ShopRefreshCount = 0, ShopFreeRefreshCount = 0,
            ShopPaidRefreshCount = 0, ShopRestockCount = 0, ShopTacticPurchases = 0,
            ShopEquipmentPurchases = 0, ShopCardPurchases = 0, ShopSkillPurchases = 0
        };
        return next with { ShopStock = CreateShopStock(next, template) };
    }

    private ShanheCampaignDefinition CreateMarketDefinition(RunState state)
    {
        var shop = _nodes[state.MarketNodeId!];
        var probe = Definition.Nodes.First(node => ShanheCampaignContent.IsBattle(node.Kind));
        return ShanheCampaignContent.FreezeAndValidate(Definition with
        {
            // This detached recipe only executes purchases. The owning expedition/battle keeps the source identity and growth contract.
            SourceDifficultyId = null, SourceEnemyGrowth = null, SourceEnemyGrowthHash = null, SourceGrowthPolicy = null,
            Id = Definition.Id + ":market:" + state.MarketChapterNumber,
            Name = Definition.Name + " · 常驻集市", StartingNodeIds = [shop.Id], RandomStartingNode = false,
            StartingGold = 0, ExtraSkillSlots = 0, StartingBonuses = new(),
            Nodes = [shop with { NextNodeIds = [probe.Id], ChapterNumber = state.MarketChapterNumber,
                Choices = ShanheCampaignContent.Freeze((shop.Choices ?? []).Select(choice => choice with { LeavesNode = false })) },
                probe with { NextNodeIds = [], RewardIds = [], BossRewards = null, AddSkillSlotsOnVictory = 0,
                    CategorizedRewards = false, RewardSkipGold = 0 }]
        });
    }

    private ShanheBattleMarketSetup CreateMarketSetup(RunState state)
    {
        var definition = CreateMarketDefinition(state);
        return ShanheBattleMarketSetup.Freeze(new(definition,
            _options with { PersistentMarket = false, EquippedTacticIds = [], SourceDifficultyId = null },
            ExportMarketTransfer(state, ShanheCampaignContent.GetContentHash(definition)))
            { BossBattle = state.CurrentNodeId is { } id && _nodes[id].Kind == ShanheNodeKind.BossBattle });
    }

    private ShanheBattleMarketTransfer ExportMarketTransfer(RunState state, string contentHash) =>
        ShanheBattleMarketTransfer.Freeze(new()
        {
            MarketContentHash = contentHash, MarketNodeId = state.MarketNodeId!, ChapterNumber = state.MarketChapterNumber,
            Hp = state.Hp, Gold = state.Gold, ExtraSkillSlots = state.ExtraSkillSlots, Bonuses = state.Bonuses,
            ExtraSkillIds = state.ExtraSkillIds, TacticIds = state.TacticIds, StartingCards = state.StartingCards,
            StartingEquipment = state.StartingEquipment, ShopStock = state.ShopStock, PurchasedChoiceIds = state.PurchasedChoiceIds,
            ShopRefreshCount = state.ShopRefreshCount, ShopFreeRefreshCount = state.ShopFreeRefreshCount,
            ShopPaidRefreshCount = state.ShopPaidRefreshCount, ShopRestockCount = state.ShopRestockCount,
            ShopTacticPurchases = state.ShopTacticPurchases, ShopEquipmentPurchases = state.ShopEquipmentPurchases,
            ShopCardPurchases = state.ShopCardPurchases, ShopSkillPurchases = state.ShopSkillPurchases
        });

    private RunState ReadMarketTransfer(RunState state, ShanheBattleMarketTransfer value)
    {
        var transfer = ShanheBattleMarketTransfer.Freeze(value);
        ShanheCampaignContent.ValidateMarketResources(transfer);
        var expectedHash = _battleMarketSession ? ContentHash : ShanheCampaignContent.GetContentHash(CreateMarketDefinition(state));
        var hasNode = _nodes.TryGetValue(transfer.MarketNodeId, out var node);
        Require(transfer.MarketContentHash == expectedHash && hasNode &&
            node!.Kind == ShanheNodeKind.Shop && transfer.ChapterNumber > 0 &&
            (_battleMarketSession || transfer.MarketNodeId == state.MarketNodeId && transfer.ChapterNumber == state.MarketChapterNumber),
            "The market transfer belongs to a different chapter or catalogue.");
        // A live purchase may finish while its owner is still dying. Campaign
        // finalization takes health from the independently replayed player.
        Require(transfer.Gold >= 0 && transfer.ExtraSkillSlots is >= 0 and <= 8 &&
            transfer.Bonuses is { MaxHp: >= 0 and <= 40, InitialHand: >= 0 and <= 20, Draw: >= 0 and <= 10, Slash: >= 0 and <= 10 } &&
            transfer.ExtraSkillIds.Count <= transfer.ExtraSkillSlots && transfer.ExtraSkillIds.Distinct(StringComparer.Ordinal).Count() == transfer.ExtraSkillIds.Count &&
            transfer.ExtraSkillIds.All(skill => Definition.Rewards.Any(reward => reward.SkillId == skill) || state.ExtraSkillIds.Contains(skill)) &&
            transfer.TacticIds.Count <= MaximumCommands && transfer.TacticIds.All(id => id is not null &&
                _rewards.TryGetValue(id, out var reward) &&
                (reward.TacticSkillId is not null || reward.OpeningSkillRewardIds is not null || reward.OpeningCompanionPool is not null ||
                    reward.CampaignEconomy is not null || id == Definition.TeamRankSetRewardId)) &&
            (Definition.TeamRankSetRewardId is not { } setId || transfer.TacticIds.Count(id => id == setId) <= 1) &&
            transfer.StartingCards.Count <= 24 && transfer.StartingEquipment.Count <= 8 && transfer.ShopStock.Count <= 512 &&
            transfer.PurchasedChoiceIds.Count <= MaximumCommands &&
            transfer.PurchasedChoiceIds.All(id => !string.IsNullOrWhiteSpace(id)) &&
            transfer.PurchasedChoiceIds.Distinct(StringComparer.Ordinal).Count() == transfer.PurchasedChoiceIds.Count,
            "Invalid market wallet, build or inventory seed.");
        var counts = new[] { transfer.ShopRefreshCount, transfer.ShopFreeRefreshCount, transfer.ShopPaidRefreshCount,
            transfer.ShopRestockCount, transfer.ShopTacticPurchases, transfer.ShopEquipmentPurchases,
            transfer.ShopCardPurchases, transfer.ShopSkillPurchases };
        Require(counts.All(count => count is >= 0 and <= MaximumCommands) &&
            transfer.ShopRefreshCount == transfer.ShopFreeRefreshCount + transfer.ShopPaidRefreshCount &&
            transfer.ShopStock.Select(choice => choice.Id).Distinct(StringComparer.Ordinal).Count() == transfer.ShopStock.Count &&
            transfer.ShopStock.All(choice => !choice.LeavesNode && (node!.Choices ?? []).Any(authored =>
                authored.RewardId == choice.RewardId && authored.GoldCost == choice.GoldCost && authored.HpCost == choice.HpCost)) &&
            transfer.StartingEquipment.Select(card => EquipmentCatalog.Get(card.Kind).Slot).Distinct().Count() == transfer.StartingEquipment.Count,
            "Invalid market shelf or transaction counters.");
        var imported = state with
        {
            MarketNodeId = transfer.MarketNodeId, MarketChapterNumber = transfer.ChapterNumber,
            Hp = transfer.Hp, Gold = transfer.Gold, ExtraSkillSlots = transfer.ExtraSkillSlots, Bonuses = transfer.Bonuses,
            ExtraSkillIds = transfer.ExtraSkillIds, TacticIds = transfer.TacticIds,
            StartingCards = transfer.StartingCards, StartingEquipment = transfer.StartingEquipment,
            ShopStock = transfer.ShopStock, PurchasedChoiceIds = transfer.PurchasedChoiceIds,
            ShopRefreshCount = transfer.ShopRefreshCount, ShopFreeRefreshCount = transfer.ShopFreeRefreshCount,
            ShopPaidRefreshCount = transfer.ShopPaidRefreshCount, ShopRestockCount = transfer.ShopRestockCount,
            ShopTacticPurchases = transfer.ShopTacticPurchases, ShopEquipmentPurchases = transfer.ShopEquipmentPurchases,
            ShopCardPurchases = transfer.ShopCardPurchases, ShopSkillPurchases = transfer.ShopSkillPurchases
        };
        if (Definition.TeamRankSetRewardId is not { } automaticSetId) return imported;
        var hasSet = imported.TacticIds.Contains(automaticSetId, StringComparer.Ordinal);
        var recomputed = ApplyAutomaticTeamRankSet(imported with
        { TacticIds = ShanheCampaignContent.Freeze(imported.TacticIds.Where(id => id != automaticSetId)) });
        Require(!hasSet || recomputed.TacticIds.Contains(automaticSetId, StringComparer.Ordinal),
            "An automatic team set requires its actual qualifying owned tactics.");
        // Keep the acquisition order of a valid transfer, while repairing an
        // otherwise valid qualified transfer which omitted the derived marker.
        return hasSet ? imported : recomputed;
    }
}

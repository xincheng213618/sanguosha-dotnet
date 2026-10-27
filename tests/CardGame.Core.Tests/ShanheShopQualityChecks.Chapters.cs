using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheShopQualityChecks
{
    public static void PublishedChapterWeightsExcludeHorsesAndFavorLaterRedStock()
    {
        var published = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        foreach (var catalog in new[] { published,
            StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId),
            StandardShanheCampaignCatalog.CreateSourceCodexDefinition(1) })
        {
            Need(catalog.ShopRarityPolicy == new ShanheShopRarityPolicy(new(0, 70, 20, 10), new(0, 0, 10, 90)),
                "Both current itineraries publish the same early premium and later red stock balance.");
            var horses = catalog.Rewards.Where(reward => (reward.StartingEquipment ?? []).Concat(reward.StartingCards ?? [])
                    .Any(card => EquipmentCatalog.IsEquipment(card.Kind) && EquipmentCatalog.Get(card.Kind).Slot is
                        EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse)).Select(reward => reward.Id).ToHashSet();
            Need(catalog.Nodes.Where(node => node.Kind == ShanheNodeKind.Shop)
                    .SelectMany(node => node.Choices!).All(choice => choice.RewardId is null || !horses.Contains(choice.RewardId)),
                "Mount definitions remain usable by battle content, while every current shop candidate pool excludes both horse slots.");
        }
        Need(StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.LocalEditionId).ShopRarityPolicy is null,
            "The explicit legacy local edition retains its original selection contract.");

        var definition = ChapterStockDefinition(published.ShopRarityPolicy!);
        var counts = new int[3, 5];
        for (var seed = 1; seed <= 80; seed++)
        {
            var session = ShanheCampaignSession.Create(definition, new(seed, Player) { UseCodexRewardPool = true }, _ => Registry.Value);
            for (var chapter = 1; chapter <= 3; chapter++)
            {
                Accept(session, new MoveShanheNodeCommand("shop" + chapter, session.Revision));
                Need(session.Snapshot.ShopItems.Count == 5 && session.Snapshot.ShopItems.Select(item => item.Reward!.Id).Distinct().Count() == 5,
                    "Quality weighting fills the normal shelf with distinct real products.");
                foreach (var item in session.Snapshot.ShopItems) counts[chapter - 1, item.Reward!.Rarity]++;
                Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
            }
        }
        Need(counts[0, 1] == 0 && counts[0, 2] is >= 240 and <= 320 && counts[0, 4] is >= 20 and <= 65 &&
                counts[1, 4] is >= 330 and <= 390 && counts[2, 4] is >= 330 and <= 390 &&
                Math.Abs(counts[1, 4] - counts[2, 4]) <= 40,
            "Across fixed seeds chapter one is mostly purple with a little red, while chapters two and three are similarly dominated by red.",
            Enumerable.Range(0, 3).Select(chapter => Enumerable.Range(1, 4).Select(rarity => counts[chapter, rarity]).ToArray()).ToArray());
    }

    public static void ChapterWeightsApplyToRefreshRestockRecoveryAndBattleMarkets()
    {
        var forced = new ShanheShopRarityPolicy(new(0, 100, 0, 0), new(0, 0, 0, 100));
        var definition = ChapterStockDefinition(forced);
        var perks = Sources(2252, 2291);
        definition = definition with { Rewards = definition.Rewards.Concat(perks).ToArray() };
        var options = new ShanheCampaignOptions(71, Player)
            { UseCodexRewardPool = true, EquippedTacticIds = perks.Select(reward => reward.Id).ToArray() };
        var session = ShanheCampaignSession.Create(definition, options, _ => Registry.Value);
        Accept(session, new MoveShanheNodeCommand("shop1", session.Revision));
        AllRarity(session, 2);
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        AllRarity(session, 2);
        var before = session.Snapshot;
        var bought = before.ShopItems[0];
        Accept(session, new ChooseShanheNodeOptionCommand(bought.ChoiceId, null, session.Revision));
        AllRarity(session, 2);
        Need(session.Snapshot.ShopRestockOnPurchase && session.Snapshot.ShopItems[0].ChoiceId != bought.ChoiceId &&
                session.Snapshot.ShopItems.Skip(1).Select(item => item.ChoiceId).SequenceEqual(before.ShopItems.Skip(1).Select(item => item.ChoiceId)) &&
                session.Snapshot.Gold == before.Gold - bought.GoldCost,
            "The same-slot replacement follows chapter quality while retaining payment, other positions and category price progression.");
        RejectUnchanged(session, new ChooseShanheNodeOptionCommand(bought.ChoiceId, null, session.Revision));
        var checkpoint = session.CreateCheckpoint();
        var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(checkpoint)), definition, _ => Registry.Value);
        Need(JsonSerializer.Serialize(restored.Snapshot) == JsonSerializer.Serialize(session.Snapshot),
            "Replaying the exact content preserves weighted initial, refreshed and replenished stock without rerolling.");
        for (var chapter = 2; chapter <= 3; chapter++)
        {
            Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
            Accept(session, new MoveShanheNodeCommand("shop" + chapter, session.Revision));
            AllRarity(session, 4);
        }

        var lockedDefinition = definition with { Rewards = definition.Rewards.Select(reward =>
            reward.Id.StartsWith("chapter-stock-", StringComparison.Ordinal)
                ? new ShanheRewardDefinition(reward.Id, reward.Name, StartingEquipment: [new(CardKind.Crossbow)]) { Rarity = reward.Rarity }
                : reward).ToArray() };
        var locked = ShanheCampaignSession.Create(lockedDefinition, options with
            { EquippedTacticIds = [], UnlockedRewardIds = definition.Rewards.Where(reward => reward.Rarity == 2).Select(reward => reward.Id).ToArray() }, _ => Registry.Value);
        Accept(locked, new MoveShanheNodeCommand("shop1", locked.Revision));
        Accept(locked, new ChooseShanheNodeOptionCommand("$leave", null, locked.Revision));
        Accept(locked, new MoveShanheNodeCommand("shop2", locked.Revision));
        AllRarity(locked, 2);

        var persistent = ShanheCampaignSession.Create(definition, options with { PersistentMarket = true }, _ => Registry.Value);
        AllRarity(persistent, 4); // Hidden shop nodes lead directly to the chapter-three battle.
        Accept(persistent, new MoveShanheNodeCommand("battle", persistent.Revision));
        var setup = persistent.PendingBattleSetup!.MarketSetup!;
        Need(setup.Definition.ShopRarityPolicy == forced && setup.State.ChapterNumber == 3,
            "The frozen battle market retains the policy and its actual chapter rather than falling back to the first shop.");
        var market = ShanheCampaignSession.CreateBattleMarketSession(setup, _ => Registry.Value);
        AllRarity(market, 4);
        Accept(market, new RefreshShanheShopCommand(market.Revision));
        AllRarity(market, 4);

        foreach (var invalid in new[]
        {
            new ShanheShopRarityPolicy(new(-1, 71, 20, 10), forced.LaterChapters),
            new ShanheShopRarityPolicy(new(0, 50, 0, 0), forced.LaterChapters),
            new ShanheShopRarityPolicy(forced.FirstChapter, null!)
        })
        {
            var rejected = false;
            try { _ = ShanheCampaignContent.FreezeAndValidate(definition with { ShopRarityPolicy = invalid }); }
            catch (ArgumentException) { rejected = true; }
            Need(rejected, "Malformed or incomplete chapter quality weights are rejected by the content loader.");
        }
        Need(ShanheCampaignContent.GetContentHash(definition) != ShanheCampaignContent.GetContentHash(definition with { ShopRarityPolicy = null }) &&
                !JsonSerializer.Serialize(definition with { ShopRarityPolicy = null }).Contains("ShopRarityPolicy", StringComparison.Ordinal),
            "The policy participates in the content hash, and omitted policy preserves the existing serialized definition shape.");
    }

    private static ShanheCampaignDefinition ChapterStockDefinition(ShanheShopRarityPolicy policy)
    {
        var goods = Enumerable.Range(1, 4).SelectMany(rarity => Enumerable.Range(1, 8).Select(index =>
            new ShanheRewardDefinition($"chapter-stock-{rarity}-{index}", $"军资{rarity}·{index}", Bonuses: new(MaxHp: 1))
                { Rarity = rarity })).ToArray();
        var choices = goods.Select(reward => Choice(reward, reward.Rarity * 100)).ToArray();
        return new("fixture:chapter-shop-stock", "分级集市", [Player], ["shop1"],
            Enumerable.Range(1, 3).Select(chapter => new ShanheNodeDefinition("shop" + chapter, "集市" + chapter,
                ShanheNodeKind.Shop, [chapter < 3 ? "shop" + (chapter + 1) : "battle"], Choices: choices) { ChapterNumber = chapter })
                .Append(new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Enemy)])]) { ChapterNumber = 3 }).ToArray(),
            goods, StartingGold: 10_000) { ShopRarityPolicy = policy };
    }

    private static void AllRarity(ShanheCampaignSession session, int rarity) =>
        Need(session.Snapshot.ShopItems.Count == 5 && session.Snapshot.ShopItems.All(item => item.Reward!.Rarity == rarity),
            "The selected chapter profile must govern every newly generated position when its requested tier has enough eligible stock.");
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheShopQualityChecks
{
    private const string Player = "fixture:shop-quality-player", Enemy = "fixture:shop-quality-enemy";
    private const string Tactic100 = "quality-tactic-100", Skill100 = "quality-skill-100", Material100 = "quality-material-100", All100 = "quality-all-100";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

    public static void BaseFiveSlotsAndOwnedExpansionUseRealCategoryStock()
    {
        foreach (var (perks, count) in new (string[] Perks, int Count)[]
        {
            ([], 5), (["tactic-economy-2136"], 6), (["tactic-economy-2137"], 7),
            (["tactic-economy-2136", "tactic-economy-2137"], 8)
        })
        {
            var session = Enter(71, perks);
            CountCategories(session, count, count, count, count);
            DistinctStock(session);
            Need(session.Snapshot.ShopItems.Count(item => item.Reward?.SkillId == SkillId(5)) <= 1,
                "Extra shelf positions cannot duplicate two catalogue aliases of the same acquired skill identity.");
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            CountCategories(session, count, count, count, count);
            DistinctStock(session);
            Need(session.Snapshot.Gold == 9900 && session.Snapshot.ShopItems.Where(item => item.Reward?.Id != "supply")
                .All(item => item.ChoiceId.StartsWith("$shop:1:", StringComparison.Ordinal)),
                "The real paid whole-shop refresh preserves the fixed category capacity while advancing every sampled choice generation.");
        }
        var definition = Definition();
        var sparse = definition with { Nodes = definition.Nodes.Select(node => node.Id == "shop"
            ? node with { Choices = node.Choices!.Where(choice => choice.RewardId is
                "supply" or "t1" or "t2" or "s5" or "s5-alias" or "e1" or "e2" or "c1" or "c2").ToArray() }
            : node).ToArray() };
        var shortShelf = Enter(71, ["tactic-economy-2136", "tactic-economy-2137"], definition: sparse);
        CountCategories(shortShelf, 2, 1, 2, 2);
        DistinctStock(shortShelf);
        Need(shortShelf.Snapshot.ShopItems.Count == 8 && shortShelf.Snapshot.ShopItems.Count(item => item.Reward?.SkillId == SkillId(5)) == 1,
            "Insufficient real supply leaves fewer positions, keeps fixed supplies, and never invents filler or duplicates the only skill identity.");

        static void CountCategories(ShanheCampaignSession session, int tactics, int skills, int equipment, int cards)
        {
            var expected = new[] { ('t', tactics), ('s', skills), ('e', equipment), ('c', cards) };
            foreach (var (category, count) in expected)
                Need(session.Snapshot.ShopItems.Count(item => item.Reward?.Id is { } id && id != "supply" && id[0] == category) == count,
                    "Each available category must use exactly five base positions plus the held source expansion, bounded by distinct real supply.");
        }
    }

    public static void ZeroAndForcedPromotionsKeepBaselineCategoryIsolationNearestTierAndAliases()
    {
        var baseline = Enter(71, []);
        var expected = new[] { "supply", "t2", "t6", "t7", "t3", "t1", "s3", "s7", "s5", "s2", "s8", "e2", "e8", "e4", "e6", "e7", "c5", "c1", "c3", "c2", "c8" };
        Need(Ids(baseline).SequenceEqual(expected), "Zero promotion retains the established mixed-category sampler at the fixed seed.", Ids(baseline));
        var cases = new[]
        {
            (Perk: Tactic100, Kinds: "t", Expected: new[] { "supply", "t5", "t6", "t7", "t3", "t1", "s3", "s7", "s5", "s2", "s8", "e2", "e8", "e4", "e6", "e7", "c5", "c1", "c3", "c2", "c8" }),
            (Perk: Skill100, Kinds: "s", Expected: new[] { "supply", "t2", "t6", "t7", "t3", "t1", "s6", "s7", "s5", "s2", "s8", "e2", "e8", "e4", "e6", "e7", "c5", "c1", "c3", "c2", "c8" }),
            (Perk: Material100, Kinds: "ec", Expected: new[] { "supply", "t2", "t6", "t7", "t3", "t1", "s3", "s7", "s5", "s2", "s8", "e5", "e8", "e4", "e6", "e7", "c7", "c6", "c5", "c2", "c8" })
        };
        foreach (var item in cases)
        {
            var promoted = Enter(71, [item.Perk]);
            Need(Ids(promoted).SequenceEqual(item.Expected) && promoted.Snapshot.ShopItems.Count == baseline.Snapshot.ShopItems.Count,
                "A forced promotion changes only its named category positions after the whole baseline is sampled, without consuming another category's RNG or adding positions.", Ids(promoted));
            for (var index = 0; index < expected.Length; index++)
            {
                var before = baseline.Snapshot.ShopItems[index];
                var after = promoted.Snapshot.ShopItems[index];
                if (before.Reward!.Id == "supply" || !item.Kinds.Contains(before.Reward.Id[0]) || before.Reward.Rarity is 0 or 4)
                    Need(after.ChoiceId == before.ChoiceId, "Supply, other categories, unknown rarity zero and top rarity four preserve their original positions and identities.");
                if (after.Reward!.Id != before.Reward.Id)
                    Need(after.Reward.Rarity > before.Reward.Rarity, "A successful promotion is strictly upward in the authored rarity, never a sidegrade or downgrade.");
                Need(after.GoldCost == AuthoredPrice(promoted, after.Reward!.Id), "Every resulting product quotes its own authored price, including higher-quality replacements.");
            }
            DistinctStock(promoted);
        }
        Need(cases[0].Expected[1] == "t5" && baseline.Snapshot.ShopItems[1].Reward!.Rarity == 1 &&
            Enter(71, [Tactic100]).Snapshot.ShopItems[1].Reward!.Rarity == 3,
            "When rarity two is absent, the common tactic reaches the next available epic tier; another common position stays unchanged once all higher identities occupy the shelf.");

        var all = Definition();
        var aliasOnly = all with { Nodes = all.Nodes.Select(node => node.Id == "shop"
            ? node with { Choices = node.Choices!.Where(choice => choice.RewardId is "supply" or "s5" or "s5-alias").ToArray() } : node).ToArray() };
        var sameIdentity = Enter(71, [Skill100], definition: aliasOnly);
        Need(Ids(sameIdentity).SequenceEqual(["supply", "s5"]) &&
            sameIdentity.Snapshot.ShopItems.Single(item => item.Reward!.Id == "s5").GoldCost == AuthoredPrice(sameIdentity, "s5"),
            "A lone skill cannot promote into its own higher-rarity alias or adopt that alias's higher price; a higher product must have another canonical identity.");

        foreach (var invalid in new[] { -1, 101 })
        foreach (var effect in new[]
        {
            new ShanheCampaignEconomy(ShopTacticPromotionPercent: invalid),
            new ShanheCampaignEconomy(ShopSkillPromotionPercent: invalid),
            new ShanheCampaignEconomy(ShopMaterialPromotionPercent: invalid)
        })
        {
            var definition = Definition();
            var invalidDefinition = definition with { Rewards = definition.Rewards.Append(new ShanheRewardDefinition("invalid-quality", "非法品质") { CampaignEconomy = effect }).ToArray() };
            var rejected = false;
            try { ShanheCampaignContent.FreezeAndValidate(invalidDefinition); }
            catch (ArgumentException) { rejected = true; }
            Need(rejected, "Each optional quality percentage rejects negative and above-one-hundred values at content loading.");
        }
    }

    public static void PromotedRestockAndFreePaidRefreshKeepOwnPricesSupplyAndAllowances()
    {
        var session = Enter(71, [All100, "tactic-economy-2252", "tactic-economy-2291", "tactic-economy-2091"]);
        var shelf = session.Snapshot;
        var index = shelf.ShopItems.ToList().FindIndex(item => item.Reward!.Id == "t5");
        Need(index == 1 && shelf.ShopItems[index].GoldCost == 391 && shelf.ShopFreeRefreshRemaining == 2 && shelf.ShopPaidRefreshRemaining == 1,
            "The initially promoted epic tactic costs its own 435 base with the real ten-percent discount, while real Restock and Free Refresh retain two free and one paid allowance.");
        var originalSupply = shelf.ShopItems.Single(item => item.Reward!.Id == "supply").ChoiceId;
        for (var purchase = 1; purchase <= 2; purchase++)
        {
            var before = session.Snapshot;
            var bought = before.ShopItems[index];
            Accept(session, new ChooseShanheNodeOptionCommand(bought.ChoiceId, null, session.Revision));
            var after = session.Snapshot;
            var replacement = after.ShopItems[index];
            Need(after.Gold == before.Gold - bought.GoldCost && after.Bonuses.MaxHp == before.Bonuses.MaxHp + 1 &&
                after.ShopItems.Count == before.ShopItems.Count && replacement.ChoiceId != bought.ChoiceId &&
                replacement.Reward!.Id == "t5" && replacement.Reward.Rarity == 3 &&
                replacement.GoldCost == (435 + purchase * 100) * 9 / 10 &&
                Enumerable.Range(0, before.ShopItems.Count).Where(slot => slot != index)
                    .All(slot => before.ShopItems[slot].ChoiceId == after.ShopItems[slot].ChoiceId) &&
                after.ShopFreeRefreshRemaining == 2 && after.ShopPaidRefreshRemaining == 1,
                "A real purchase replenishes only that slot: its common restock baseline promotes back to eligible epic t5, pays its own base plus the current category surcharge before discount, and never spends a refresh.",
                new { purchase, bought, replacement, after.Gold, after.ShopFreeRefreshRemaining, after.ShopPaidRefreshRemaining });
            DistinctStock(session);
            RejectUnchanged(session, new ChooseShanheNodeOptionCommand(bought.ChoiceId, null, session.Revision));
        }
        Need(shelf.ShopItems[index].GoldCost == 391 && shelf.Gold == 10_000,
            "A previously published promoted quote remains frozen through subsequent purchases and same-position quality replacements.");

        string[][] promotedTactics = [["t8", "t5", "t2", "t7", "t6"], ["t5", "t7", "t4", "t2", "t6"], ["t8", "t5", "t6", "t4", "t7"]];
        for (var generation = 1; generation <= 3; generation++)
        {
            var before = session.Snapshot;
            var cost = before.ShopRefreshCost;
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            var after = session.Snapshot;
            Need(after.Gold == before.Gold - cost && cost == (generation <= 2 ? 0 : 100) &&
                after.ShopFreeRefreshRemaining == Math.Max(0, 2 - generation) && after.ShopPaidRefreshRemaining == (generation < 3 ? 1 : 0) &&
                after.ShopItems.Single(item => item.Reward!.Id == "supply").ChoiceId == originalSupply &&
                Ids(session).Where(id => id[0] == 't').SequenceEqual(promotedTactics[generation - 1]) &&
                after.ShopItems.All(item => item.GoldCost == (AuthoredPrice(session, item.Reward!.Id) +
                    (item.Reward.Id[0] == 't' ? 200 : 0)) * 9 / 10),
                "Both free whole-shop generations and the paid generation apply promotion to newly sampled positions, preserve current Supply IDs and purchase surcharges, and consume only the corresponding refresh allowance.",
                new { generation, cost, Ids = Ids(session), after.Gold, after.ShopFreeRefreshRemaining, after.ShopPaidRefreshRemaining });
            DistinctStock(session);
        }
    }

    public static void ActualTwentyPercentSourcesKeepFixedSuccessFailureQuotesAndRejectedQueriesPure()
    {
        var perks = Sources(2213, 2214, 2215).Select(reward => reward.Id).ToArray();
        Need(perks.Length == 3, "The command fixture holds all three actual current-season twenty-percent source effects.");
        var expected = new[]
        {
            (Seed: 71, Ids: new[] { "supply", "t2", "t6", "t7", "t5", "t1", "s3", "s7", "s5", "s2", "s8", "e2", "e8", "e4", "e6", "e7", "c5", "c1", "c3", "c2", "c8" }),
            (Seed: 83, Ids: new[] { "supply", "t2", "t7", "t1", "t6", "t8", "s2", "s5", "s6", "s7", "s1", "e6", "e7", "e4", "e1", "e3", "c6", "c8", "c3", "c4", "c2" })
        };
        foreach (var item in expected)
        {
            var session = Enter(item.Seed, perks);
            Need(Ids(session).SequenceEqual(item.Ids),
                "Two fixed seeds exercise actual twenty-percent successes and failures without a statistical seed search or changing the original category sampler.",
                new { item.Seed, Actual = Ids(session), item.Ids });
            DistinctStock(session);
        }
        var sampled = Enter(83, perks);
        Need(sampled.Snapshot.ShopItems.Single(item => item.Reward!.Id == "s5").Reward!.Rarity == 2,
            "The real skill-shop source reaches the nearest available rarity-two skill even though its higher-rarity alias occurs first in the authored candidate list.");
        var quote = sampled.Snapshot;
        var quoteIds = Ids(sampled);
        var frozen = (IList<ShanheShopItemSnapshot>)quote.ShopItems;
        Need(frozen.IsReadOnly, "The final promoted stock is a read-only published collection.");
        var refusedMutation = false;
        try { frozen[0] = frozen[0]; }
        catch (NotSupportedException) { refusedMutation = true; }
        Need(refusedMutation, "Published final quality choices cannot be edited through an IList cast.");
        for (var query = 0; query < 3; query++)
            Need(ReferenceEquals(quote, sampled.Snapshot) && Ids(sampled).SequenceEqual(quoteIds), "Repeated pure snapshots retain the frozen final choices without sampling again.");
        RejectUnchanged(sampled, new ChooseShanheNodeOptionCommand("not-on-this-shelf", null, sampled.Revision));
        RejectUnchanged(sampled, new RefreshShanheShopCommand(sampled.Revision - 1));
        var untouched = Enter(83, perks);
        Accept(sampled, new RefreshShanheShopCommand(sampled.Revision));
        Accept(untouched, new RefreshShanheShopCommand(untouched.Revision));
        Need(Ids(sampled).SequenceEqual(Ids(untouched)) && sampled.Snapshot.Gold == untouched.Snapshot.Gold &&
            quote.ShopItems.Select(item => item.Reward!.Id).SequenceEqual(quoteIds) &&
            quote.ShopItems.All(item => sampled.Snapshot.ShopItems.All(current => current.ChoiceId != item.ChoiceId) || item.Reward!.Id == "supply"),
            "Queries, refused mutation and rejected commands cannot advance a hidden roll: the next real paid refresh matches an untouched same-seed run while the old published shelf remains intact.");

        var fixedBaseline = Enter(83, [], random: false);
        var fixedQuality = Enter(83, perks, random: false);
        Need(Ids(fixedBaseline).SequenceEqual(Ids(fixedQuality)) && fixedQuality.Snapshot.ShopRefreshRemaining == 0 &&
            fixedBaseline.Snapshot.ShopItems.Select(item => item.GoldCost).SequenceEqual(fixedQuality.Snapshot.ShopItems.Select(item => item.GoldCost)),
            "Owned quality sources leave the authored fixed-stock merchant and its original prices untouched.");
    }

    private static ShanheCampaignSession Enter(int seed, IReadOnlyList<string> perks, bool random = true, ShanheCampaignDefinition? definition = null)
    {
        var session = ShanheCampaignSession.Create(definition ?? Definition(), new(seed, Player)
            { UseCodexRewardPool = random, EquippedTacticIds = perks }, _ => Registry.Value);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        return session;
    }

    private static ShanheCampaignDefinition Definition()
    {
        var goods = new List<ShanheRewardDefinition>();
        foreach (var kind in new[] { 't', 's', 'e', 'c' })
        for (var index = 1; index <= 8; index++)
        {
            var rarity = (kind == 's' ? new[] { 1, 1, 1, 1, 2, 2, 4, 0 } : new[] { 1, 1, 1, 1, 3, 3, 4, 0 })[index - 1];
            var id = kind + index.ToString(System.Globalization.CultureInfo.InvariantCulture);
            goods.Add((kind switch
            {
                't' => new ShanheRewardDefinition(id, "成长" + index, Bonuses: new(MaxHp: 1)),
                's' => new ShanheRewardDefinition(id, "技能" + index, SkillId: SkillId(index)),
                'e' => new ShanheRewardDefinition(id, "装备" + index, StartingEquipment: [new(CardKind.Crossbow, Rank: index)]),
                _ => new ShanheRewardDefinition(id, "卡牌" + index, StartingCards: [new(CardKind.Dodge, Rank: index)])
            }) with { Rarity = rarity });
        }
        var alias = new ShanheRewardDefinition("s5-alias", "技能五高档别名", SkillId: SkillId(5)) { Rarity = 4 };
        var supply = new ShanheRewardDefinition("supply", "军医包", Heal: 1) { Rarity = 4 };
        var models = new[]
        {
            Model(Tactic100, new(ShopTacticPromotionPercent: 100)), Model(Skill100, new(ShopSkillPromotionPercent: 100)),
            Model(Material100, new(ShopMaterialPromotionPercent: 100)),
            Model(All100, new(ShopTacticPromotionPercent: 100, ShopSkillPromotionPercent: 100, ShopMaterialPromotionPercent: 100))
        };
        var sources = Sources(2213, 2214, 2215, 2252, 2291, 2091, 2136, 2137);
        // Baseline identity deduplication sees the low skill first. Promotion candidates
        // deliberately encounter its r4 alias before the same identity's authored r2.
        var choices = new[] { Choice(supply, 35), Choice(alias, 911) }.Concat(goods.Select(reward => Choice(reward,
            100 + reward.Rarity * 100 + int.Parse(reward.Id[1..], System.Globalization.CultureInfo.InvariantCulture) * 7))).ToArray();
        return new("fixture:shanhe-shop-quality", "真实商店品质提升", [Player], ["shop"],
            [new("shop", "品质集市", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Enemy)])])],
            goods.Append(alias).Append(supply).Concat(models).Concat(sources).ToArray(), StartingGold: 10_000, ExtraSkillSlots: 8);
    }

    private static string SkillId(int index) => "fixture:shop-quality-skill-" + index;
    private static ShanheRewardDefinition Model(string id, ShanheCampaignEconomy economy) => new(id, id) { CampaignEconomy = economy };
    private static ShanheRewardDefinition[] Sources(params int[] ids) => StandardShanheCampaignCatalog.CurrentEconomyTactics
        .Where(source => ids.Contains(source.SourceId)).Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description }).ToArray();
    private static ShanheNodeChoiceDefinition Choice(ShanheRewardDefinition reward, int price) =>
        new("buy-" + reward.Id, reward.Name, price, RewardId: reward.Id, LeavesNode: false);
    private static string[] Ids(ShanheCampaignSession session) => session.Snapshot.ShopItems.Select(item => item.Reward!.Id).ToArray();
    private static int AuthoredPrice(ShanheCampaignSession session, string id) => session.Definition.Nodes.Single(node => node.Id == "shop").Choices!.Single(choice => choice.RewardId == id).GoldCost;
    private static void DistinctStock(ShanheCampaignSession session)
    {
        var identities = session.Snapshot.ShopItems.Where(item => !item.IsPurchased).Select(item => item.Reward!.SkillId ?? item.Reward.Id).ToArray();
        Need(identities.Distinct(StringComparer.Ordinal).Count() == identities.Length, "Alias rewards cannot create two copies of one canonical acquisition identity in the actual shelf.");
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real quality-shop command."); }
    private static void RejectUnchanged(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var before = session.Snapshot;
        Need(!session.Submit(command).Accepted && ReferenceEquals(before, session.Snapshot),
            "A stale or absent-product command cannot pay, change allowance or roll another quality candidate.");
    }
    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-shop-quality", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var skills = Enumerable.Range(1, 8).Select(index => new { id = SkillId(index), revision = 1,
                modifiers = new[] { new { id = "native-fixture-range", query = "attackRange", operation = "add", value = 1, priority = 0 } } }).ToArray();
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = skills.ToDictionary(skill => skill.id, skill => new { name = skill.id, description = "实际可授予的商店技能" }) }));
            foreach (var entry in catalog.Programs) builder.AddSkill(new(entry.Key, entry.Key, "实际可授予的商店技能") { Program = entry.Value });
            builder.AddGeneral(new(Player, "品质商店购买者", "supporter", "standard:none", "shu", 4));
            builder.AddGeneral(new(Enemy, "守军", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new("fixture:shop-quality-deck", "小商店牌堆", 0, 0, [])
                { PhysicalCards = [new("standard:dodge", Suit.Club, 7)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "品质商店的小型战斗内容", 2, 2, new Dictionary<string, int>(), "fixture:shop-quality-deck",
                GeneralPoolIds: [Player, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

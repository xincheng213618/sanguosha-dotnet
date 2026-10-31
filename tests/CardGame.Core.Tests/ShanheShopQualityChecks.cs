using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheShopQualityChecks
{
    private const string Player = "fixture:shop-quality-player", Enemy = "fixture:shop-quality-enemy";
    private const string Tactic100 = "quality-tactic-100", Skill100 = "quality-skill-100", Material100 = "quality-material-100", All100 = "quality-all-100";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

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

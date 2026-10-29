using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static partial class ShanheShopQualityChecks
{
    public static void BalancedWhiteExclusionWolongSchoolsAndMarketMigration()
    {
        var wolongSource = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(source => source.SourceId == 2253);
        Need(wolongSource.RewardId == StandardShanheCampaignCatalog.CurrentWolongMapRewardId &&
                wolongSource is { Name: "卧龙图", Price: 300, Rarity: 2, Effect.FavorOwnedSchoolRewards: true } &&
                wolongSource.Description == "你将更容易获得你所在流派的战法" &&
                !JsonSerializer.Serialize(new ShanheCampaignEconomy()).Contains("FavorOwnedSchoolRewards", StringComparison.Ordinal) &&
                !JsonSerializer.Serialize(new ShanheBattleMarketTransfer()).Contains("BalancedRewardsEnabled", StringComparison.Ordinal),
            "The additive economy source retains the verified 2253 identity and original text; both omitted flags preserve historical JSON shape.");

        var definition = Definition();
        var wolong = Sources(2253).Single();
        definition = definition with { Rewards = definition.Rewards.Append(wolong).ToArray(),
            Nodes = definition.Nodes.Select(node => node.Kind == ShanheNodeKind.Shop
                ? node with { Choices = node.Choices!.Append(Choice(wolong, 300)).ToArray() } : node).ToArray() };
        foreach (var random in new[] { false, true })
        {
            var legacy = Open(definition, false, random, []);
            Need(legacy.Snapshot.ShopItems.All(item => item.Reward?.Id != wolong.Id) &&
                    legacy.Snapshot.ShopItems.Any(item => item.Reward?.Rarity == 1),
                "Historical authored and random shops keep their white products and exclude the new effect before random selection.");
            var balanced = Open(definition, true, random, ["tactic-economy-2252", "tactic-economy-2291", All100]);
            NoWhite(balanced);
            Need(balanced.Snapshot.ShopItems.Any(item => item.Reward?.Id == wolong.Id),
                "The verified purple Wolong source is obtainable in the new shop rule instead of being hidden by the white filter.");
            if (!random) continue;
            Accept(balanced, new RefreshShanheShopCommand(balanced.Revision));
            NoWhite(balanced);
            var bought = balanced.Snapshot.ShopItems.First(item => item.Reward?.Id.StartsWith("t", StringComparison.Ordinal) == true && !item.IsPurchased);
            var free = balanced.Snapshot.ShopFreeRefreshRemaining;
            var paid = balanced.Snapshot.ShopPaidRefreshRemaining;
            Accept(balanced, new ChooseShanheNodeOptionCommand(bought.ChoiceId, null, balanced.Revision));
            NoWhite(balanced);
            Need(balanced.Snapshot.ShopFreeRefreshRemaining == free && balanced.Snapshot.ShopPaidRefreshRemaining == paid,
                "Filtered replenishment and promotion preserve the existing real refresh allowances.");
        }

        var schools = StandardShanheCampaignCatalog.CurrentSchools.Take(2).ToArray();
        const string owned = "fixture:owned-school";
        var schoolOwner = Model(owned, new(ShopDiscountPercent: 1)) with
            { Rarity = 2, SchoolMembership = new(990001, schools[0].Id, 2) };
        var schoolDefinition = definition with
        {
            Schools = schools,
            Rewards = definition.Rewards.Select(reward => reward.Id == wolong.Id
                ? reward with { SchoolMembership = StandardShanheCampaignCatalog.CurrentTacticSchoolMembershipBySourceId[2253] }
                : reward.Id is "t1" or "t2" or "t3"
                ? Model(reward.Id, new(ShopDiscountPercent: 1)) with { Rarity = 1,
                    SchoolMembership = new(990010 + int.Parse(reward.Id[1..]), reward.Id == "t3" ? schools[1].Id : schools[0].Id, 1) }
                : reward).Append(schoolOwner).ToArray()
        };
        var matching = Open(schoolDefinition, true, false, [owned, wolong.Id]);
        var white = matching.Snapshot.ShopItems.Where(item => item.Reward?.Rarity == 1).ToArray();
        Need(white.Length == 2 && white.All(item => item.Reward!.SchoolMembership?.SchoolId == schools[0].Id),
            "Only a held Wolong map and an actually carried matching school allow white school tactics; other white tactics, skills, equipment and cards stay absent.");
        NoWhite(Open(schoolDefinition, true, false, [owned]));
        var mapOnly = Open(schoolDefinition, true, false, [wolong.Id]);
        Need(mapOnly.Snapshot.ShopItems.Where(item => item.Reward?.Rarity == 1)
                .All(item => item.Reward!.SchoolMembership?.SchoolId == schools[0].Id),
            "The actual source2253 membership counts as its carried resource-school tactic; it never favors an uncarried other school.");

        // Use the same purple tier in both schools so real stock selection, not
        // a rarity distribution or research permission, measures school bias.
        var weightedGoods = Enumerable.Range(1, 12).Select(index => Model("school-stock-" + index,
            new(ShopDiscountPercent: 1)) with { Rarity = 2,
                SchoolMembership = new(991000 + index, index <= 6 ? schools[0].Id : schools[1].Id, 2) }).ToArray();
        var weightedDefinition = schoolDefinition with
        {
            Rewards = schoolDefinition.Rewards.Concat(weightedGoods).ToArray(),
            Nodes = schoolDefinition.Nodes.Select(node => node.Kind == ShanheNodeKind.Shop
                ? node with { Choices = weightedGoods.Select(reward => Choice(reward, 300)).ToArray() } : node).ToArray()
        };
        var plainMatches = 0; var favoredMatches = 0;
        for (var seed = 1; seed <= 24; seed++)
        {
            plainMatches += Open(weightedDefinition, true, true, [owned], seed).Snapshot.ShopItems
                .Count(item => item.Reward!.SchoolMembership?.SchoolId == schools[0].Id);
            favoredMatches += Open(weightedDefinition, true, true, [owned, wolong.Id], seed).Snapshot.ShopItems
                .Count(item => item.Reward!.SchoolMembership?.SchoolId == schools[0].Id);
        }
        Need(favoredMatches > plainMatches + 8,
            "Across a fixed small seed set, the actual shop offers more carried-school tactics with Wolong; this tests a local weighting rule, not a claimed official probability.",
            new { plainMatches, favoredMatches });

        var options = new ShanheCampaignOptions(71, Player) { UseCodexRewardPool = true, PersistentMarket = true,
            EquippedTacticIds = ["tactic-economy-2252", "tactic-economy-2291"] };
        var migrated = ShanheCampaignSession.Create(definition, options, _ => Registry.Value);
        Accept(migrated, new RefreshShanheShopCommand(migrated.Revision));
        var purchase = migrated.Snapshot.ShopItems.First(item => item.Reward?.Id.StartsWith("t", StringComparison.Ordinal) == true);
        Accept(migrated, new BuyShanheMarketItemCommand(purchase.ChoiceId, null, migrated.Revision));
        var before = migrated.CreateBattleMarketTransfer();
        Need(before.ShopStock.Any(choice => choice.RewardId is { } id && definition.Rewards.Single(reward => reward.Id == id).Rarity == 1),
            "The migration fixture begins with an actual old white shelf after a real refresh and purchase.");
        Accept(migrated, new EnableShanheBalancedRewardsCommand(migrated.Revision));
        var after = migrated.CreateBattleMarketTransfer();
        NoWhite(migrated);
        Need(after.BalancedRewardsEnabled && !migrated.CreateCheckpoint().Options.UseBalancedJourneyRewards &&
                JsonSerializer.Serialize(before with { BalancedRewardsEnabled = true, ShopStock = after.ShopStock }) == JsonSerializer.Serialize(after),
            "Enabling the current rule changes only invalid shelf entries and its policy marker; money, health, quota, category price counters and purchased identities survive exactly.");
        var cold = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(migrated.CreateCheckpoint())),
            definition, _ => Registry.Value);
        Need(cold.HasBalancedJourneyRewards && JsonSerializer.Serialize(cold.Snapshot) == JsonSerializer.Serialize(migrated.Snapshot),
            "The migration command replays the same replacement shelf with the original false option and complete transaction state.");
        Accept(migrated, new MoveShanheNodeCommand("battle", migrated.Revision));
        var setup = migrated.PendingBattleSetup!.MarketSetup!;
        Need(!setup.Options.UseBalancedJourneyRewards && setup.State.BalancedRewardsEnabled,
            "A migrated old expedition exports its runtime policy into the frozen battle market even though its original options remain false.");
        var market = ShanheCampaignSession.CreateBattleMarketSession(setup, _ => Registry.Value);
        Need(market.HasBalancedJourneyRewards, "The real detached market imports the owning runtime reward policy.");
        NoWhite(market);
        Accept(market, new RefreshShanheShopCommand(market.Revision));
        NoWhite(market);
        Need(market.CreateBattleMarketTransfer().BalancedRewardsEnabled,
            "A child-market refresh retains the policy in its returned transfer.");

        ShanheCampaignSession Open(ShanheCampaignDefinition recipe, bool balanced, bool random, string[] perks, int seed = 71)
        {
            var session = ShanheCampaignSession.Create(recipe, new(seed, Player)
                { UseCodexRewardPool = random, UseBalancedJourneyRewards = balanced, EquippedTacticIds = perks }, _ => Registry.Value);
            Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
            return session;
        }

        static void NoWhite(ShanheCampaignSession session) => Need(session.Snapshot.ShopItems.All(item => item.Reward?.Rarity != 1),
            "Every real published shelf entry under the default current rule excludes white rewards.");
    }
}

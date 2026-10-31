using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheUnlimitedRefreshChecks
{
    private const string Member = "tactic-economy-2107", Free = "tactic-economy-2291", Restock = "tactic-economy-2252";
    private const string Final = "tactic-economy-2146";
    private static readonly string[] InitialSix = ["tactic-economy-2018", "tactic-economy-2092", "tactic-economy-2090",
        "tactic-economy-2017", Member, "tactic-economy-2137"];
    private static readonly string[] CompleteSchool = [.. InitialSix, Final];

    public static void CompletingResourceSchoolUnlocksMoreThanTwentyPaidRefreshes()
    {
        var session = Enter(Definition(3400), InitialSix);
        Need(!session.Snapshot.ShopPaidRefreshUnlimited && session.Snapshot.TacticIds.Count == 6 &&
            session.GetSchoolProgress().Single(p => p.School.Id == 1) is { IsComplete: false } partial &&
            partial.OwnedCounts.SequenceEqual([2, 2, 2, 0]),
            "Six real carried components leave the legendary requirement unfilled.");
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Need(session.Snapshot.Gold == 3300 && session.Snapshot.ShopPaidRefreshRemaining == 4,
            "The carried membership still has its real finite five-use allowance before school completion.");
        Buy(session, session.Snapshot.ShopItems.First(item => item.Reward?.Id.StartsWith("card-", StringComparison.Ordinal) == true), 100);
        var oldQuote = session.Snapshot;
        Buy(session, session.Snapshot.ShopItems.Single(item => item.Reward?.Id == Final), 700);
        Need(session.Snapshot.Gold == 2500 && session.Snapshot.TacticIds.Count == 7 &&
            session.GetSchoolProgress().Single(p => p.School.Id == 1).IsComplete && !oldQuote.ShopPaidRefreshUnlimited,
            "Buying the real missing legendary component completes 2/2/2/1 immediately without a separate bundle or old-quote mutation.");
        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var used = 1; used <= 25; used++)
        {
            UnlimitedBudget(session, 0, 100);
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Need(session.Snapshot.Gold == 2500 - used * 100 &&
                session.Snapshot.ShopItems.All(item => item.ChoiceId.StartsWith("$shop:" + (used + 1) + ":", StringComparison.Ordinal) &&
                    seen.Add(item.ChoiceId) && item.GoldCost == 200),
                "Every paid refresh beyond five and twenty pays 100 and preserves both same-shop category surcharges.");
            Need(session.Snapshot.ShopItems.All(item => item.Reward?.Id != "unsupported-bundle"),
                "School completion does not make unsupported placeholder bundles available.");
        }
        UnlimitedBudget(session, 0, 100); RejectUnchanged(session);
        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Need(!session.Snapshot.ShopPaidRefreshUnlimited, "The flag is an active shop quote, not a map refresh action.");
        Accept(session, new MoveShanheNodeCommand("next", session.Revision));
        UnlimitedBudget(session, 0, 100);
        Need(session.Snapshot.ShopItems.All(item => item.ChoiceId.StartsWith("$shop:0:", StringComparison.Ordinal) && item.GoldCost == 100),
            "A new shop resets its shelf counters and category prices while real held school components remain owned.");
        RejectUnchanged(session);
        var current = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        Need(current.Schools is { Count: 9 } && current.Schools.Single(s => s.Id == 1).EffectTacticId == 2230 &&
            current.Rewards.All(reward => reward.Id != StandardShanheCampaignCatalog.CurrentResourceSetRewardId) &&
            !StandardShanheCampaignCatalog.ShopMetadataByRewardId.ContainsKey(StandardShanheCampaignCatalog.CurrentResourceSetRewardId) &&
            current.Nodes.All(node => (node.Choices ?? []).All(c => c.RewardId != StandardShanheCampaignCatalog.CurrentResourceSetRewardId) &&
                (node.RewardIds ?? []).All(id => id != StandardShanheCampaignCatalog.CurrentResourceSetRewardId)),
            "The playable catalogue defines the derived school but exposes no purchasable or post-battle 9999 resource-set reward.");
    }

    public static void UnlimitedRefreshesKeepFreePriorityAndFiniteMembershipBoundaries()
    {
        var free = Enter(Definition(0), [.. CompleteSchool, Free]);
        for (var remaining = 2; remaining > 0; remaining--)
        {
            UnlimitedBudget(free, remaining, 0); Accept(free, new RefreshShanheShopCommand(free.Revision));
            Need(free.Snapshot.Gold == 0, "Free refreshes still precede school-granted paid refreshes with an empty wallet.");
        }
        UnlimitedBudget(free, 0, 100); RejectUnchanged(free);
        var shortWallet = Enter(Definition(99), CompleteSchool);
        UnlimitedBudget(shortWallet, 0, 100); RejectUnchanged(shortWallet);
        var fixedStock = Enter(Definition(0), CompleteSchool, randomStock: false);
        Need(fixedStock.GetSchoolProgress().Single(p => p.School.Id == 1).IsComplete &&
            !fixedStock.Snapshot.ShopPaidRefreshUnlimited && fixedStock.Snapshot.ShopRefreshRemaining == 0,
            "School completion does not add random refresh behavior to a fixed-stock merchant.");
        RejectUnchanged(fixedStock);
        foreach (var held in new[] { Array.Empty<string>(), new[] { Member } })
        {
            var finite = Enter(Definition(1000), held); var limit = held.Length == 0 ? 1 : 5;
            for (var remaining = limit; remaining > 0; remaining--)
            {
                Need(!finite.Snapshot.ShopPaidRefreshUnlimited && finite.Snapshot.ShopPaidRefreshRemaining == remaining &&
                    finite.Snapshot.ShopRefreshRemaining == remaining, "Ordinary one-use and membership five-use limits remain finite.");
                Accept(finite, new RefreshShanheShopCommand(finite.Revision));
            }
            Need(finite.Snapshot.Gold == 1000 - limit * 100, "Each finite refresh still costs exactly 100."); RejectUnchanged(finite);
        }
    }

    private static ShanheCampaignDefinition Definition(int gold)
    {
        var cards = Enumerable.Range(1, 6).Select(i => new ShanheRewardDefinition("card-" + i, "军资" + i,
            StartingCards: [new(CardKind.Dodge, Rank: i)])).ToArray();
        var training = new ShanheRewardDefinition("training", "行军操练", Bonuses: new(MaxHp: 1));
        var unsupported = new ShanheRewardDefinition("unsupported-bundle", "未接入组合", StartingCards: [new(CardKind.Slash)]);
        var goods = new[] { Source(2146), training }.Concat(cards).Append(unsupported).ToArray();
        var components = new[] { 2018, 2092, 2090, 2017, 2107, 2137, 2291 }.Select(Source);
        return ShopDefinition(goods.Concat(components).ToArray(), goods.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, reward.Id == Final ? 700 : reward.Id == "unsupported-bundle" ? 9999 : 100,
            RewardId: reward.Id, LeavesNode: false)).ToArray(), gold);
    }
    private static ShanheCampaignDefinition ShopDefinition(ShanheRewardDefinition[] rewards, ShanheNodeChoiceDefinition[] choices, int gold) =>
        new("fixture:unlimited-shop-refresh", "资源流商店", [ShanheCampaignChecks.Player], ["shop"],
            [new("shop", "第一店", ShanheNodeKind.Shop, ["next"], Choices: choices),
             new("next", "第二店", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])])],
            rewards, StartingGold: gold) { Schools = StandardShanheCampaignCatalog.CurrentSchools };
    private static ShanheRewardDefinition Source(int id)
    {
        var source = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(item => item.SourceId == id);
        return new(source.RewardId, source.Name) { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description,
            SchoolMembership = StandardShanheCampaignCatalog.CurrentTacticSchoolMembershipBySourceId.TryGetValue(id, out var member) ? member : null };
    }
    private static ShanheCampaignSession Enter(ShanheCampaignDefinition definition, string[]? tactics = null, bool randomStock = true)
    {
        tactics ??= [];
        var extra = tactics.Skip(ShanheCampaignOptions.MaximumEquippedTactics).ToArray();
        var grants = extra.Select((id, index) => new ShanheNodeDefinition("grant-" + index, "已取得的战法", ShanheNodeKind.Event,
            [index + 1 < extra.Length ? "grant-" + (index + 1) : "shop"],
            Choices: [new("take", "领取", RewardId: id)])).ToArray();
        definition = definition with { StartingNodeIds = [grants.Length > 0 ? grants[0].Id : "shop"], Nodes = grants.Concat(definition.Nodes).ToArray() };
        var session = ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { UseCodexRewardPool = randomStock, EquippedTacticIds = tactics.Take(ShanheCampaignOptions.MaximumEquippedTactics).ToArray() }, ShanheCampaignChecks.Registry);
        foreach (var grant in grants) { Accept(session, new MoveShanheNodeCommand(grant.Id, session.Revision)); Accept(session, new ChooseShanheNodeOptionCommand("take", null, session.Revision)); }
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision)); return session;
    }
    private static void UnlimitedBudget(ShanheCampaignSession s, int free, int cost) => Need(s.Snapshot.ShopPaidRefreshUnlimited &&
        s.Snapshot.ShopPaidRefreshRemaining == 0 && s.Snapshot.ShopFreeRefreshRemaining == free && s.Snapshot.ShopRefreshRemaining == free &&
        s.Snapshot.ShopRefreshCost == cost, "Completed school publishes uncapped paid uses separately from finite free uses and its next price.");
    private static void Buy(ShanheCampaignSession s, ShanheShopItemSnapshot item, int cost)
    { Need(item.CanPurchase && item.GoldCost == cost, "The real component keeps its quote."); Accept(s, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, s.Revision)); }
    private static void RejectUnchanged(ShanheCampaignSession s)
    { var before = s.Snapshot; Need(!s.Submit(new RefreshShanheShopCommand(s.Revision)).Accepted && ReferenceEquals(before, s.Snapshot), "Rejected refresh leaves wallet, quota and shelf unchanged."); }
    private static void Accept(ShanheCampaignSession s, ShanheCampaignCommand c)
    { var r = s.Submit(c); Need(r.Accepted, r.Error ?? "Rejected real school command."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}

using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static class ShanheStartingWealthChecks
{
    private const string Player = "classic:cao-cao";
    private static readonly (int SourceId, int StartingBalance)[] Tiers =
        [(2112, 300), (2139, 400), (2140, 600), (2141, 800)];

    public static void EquippedWealthCreditsEachTierOnceAcrossRealCampaignCommands()
    {
        var catalog = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var rewards = WealthRewards(catalog);
        var definition = Route(rewards, 100);
        foreach (var (sourceId, balance) in Tiers)
        {
            var id = "tactic-economy-" + sourceId;
            var session = Create(definition, new(71, Player) { UseCodexRewardPool = true, EquippedTacticIds = [id] });
            var first = session.Snapshot;
            var second = session.Snapshot;
            Require(first.Gold == balance && second.Gold == balance && session.Revision == 0 &&
                session.AcceptedCommands.Count == 0 && first.TacticIds.SequenceEqual([id]),
                "Each actual equipped wealth tier credits its published opening amount once on top of the hundred-coin expedition balance.");

            Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
            Require(session.Snapshot.Gold == balance && session.Snapshot.ShopRefreshCost == 100,
                "Entering a genuine refreshable shop cannot pay the opening tactic again.");
            Accept(session, new ChooseShanheNodeOptionCommand("supply", null, session.Revision));
            Require(session.Snapshot.Gold == balance - 10 && session.Snapshot.ShopItems.Single().IsPurchased,
                "The accepted ten-coin supply purchase uses the credited starting balance without another wealth grant.");
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Require(session.Snapshot.Gold == balance - 110 && session.Snapshot.ShopRefreshRemaining == 0,
                "A real shop refresh spends its hundred coins without retriggering the equipped opening credit.");
            Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
            Accept(session, new MoveShanheNodeCommand("story", session.Revision));
            Accept(session, new ChooseShanheNodeOptionCommand("march", null, session.Revision));
            Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
            Require(session.Snapshot.Gold == balance - 110 && session.Snapshot.TacticIds.SequenceEqual([id]) &&
                session.PendingBattleSetup!.TacticSkillIds.Count == 0,
                "Normal onward commands retain exactly the spent opening balance and do not manufacture a battle skill for expedition-only wealth.");
        }

        string[] equipped = [rewards[0].Id, rewards[^1].Id];
        var stacked = Create(definition, new(71, Player) { EquippedTacticIds = equipped });
        Require(equipped.Length <= ShanheCampaignOptions.MaximumEquippedTactics && stacked.Snapshot.Gold == 1000 &&
            stacked.Snapshot.TacticIds.SequenceEqual(equipped),
            "Two distinct genuine wealth tiers add their two-hundred and seven-hundred opening credits within the normal equipped limit.");
    }

    public static void StartingWealthRemainsEquipOnlyAndRejectsMidExpeditionGrants()
    {
        var source = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var rewards = WealthRewards(source);
        var ids = rewards.Select(reward => reward.Id).ToHashSet(StringComparer.Ordinal);
        var codex = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        foreach (var catalog in new[] { source, codex })
        {
            Require(ids.All(id => catalog.Rewards.Any(reward => reward.Id == id && reward.CampaignEconomy?.StartingGold > 0)) &&
                catalog.Nodes.Where(node => node.Kind == ShanheNodeKind.Shop)
                    .SelectMany(node => node.Choices ?? []).All(choice => choice.RewardId is not { } id || !ids.Contains(id)) &&
                catalog.Nodes.SelectMany(node => node.RewardIds ?? []).All(id => !ids.Contains(id)) &&
                catalog.Nodes.SelectMany(node => node.BossRewards?.Groups ?? []).SelectMany(group => group.RewardIds).All(id => !ids.Contains(id)),
                "Both genuine campaign catalogues retain starting wealth for loadout selection while excluding it from shops and ordinary or grouped battle rewards.");
        }
        foreach (var row in StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(row => ids.Contains(row.RewardId)))
        {
            var metadata = StandardShanheCampaignCatalog.ShopMetadataByRewardId[row.RewardId];
            Require(metadata.SourceRewardId == row.SourceId && metadata.Price == row.Price && metadata.Rarity == row.Rarity,
                "Equip-only wealth keeps its real source price and rarity for catalogue presentation.");
        }

        var forged = rewards.Select(reward => new ShanheNodeChoiceDefinition("grant-" + reward.Id, reward.Name,
            GoldCost: 200, HpCost: 1, RewardId: reward.Id, LeavesNode: false)).ToArray();
        var definition = Route(rewards, 1000) with
        {
            Nodes =
            [
                new("shop", "伪造途中商品", ShanheNodeKind.Shop, ["story"], Choices: forged),
                new("story", "伪造途中授予", ShanheNodeKind.Event, ["battle"],
                    Choices: forged.Select(choice => choice with { LeavesNode = true }).ToArray()),
                BattleNode()
            ]
        };
        var session = Create(definition, new(71, Player));
        foreach (var node in new[] { "shop", "story" })
        {
            Accept(session, new MoveShanheNodeCommand(node, session.Revision));
            Require(session.Snapshot.NodeChoices.Select(choice => choice.Id).SequenceEqual(["$leave"]) &&
                (node != "shop" || session.Snapshot.ShopItems.Count == rewards.Length) &&
                session.Snapshot.ShopItems.All(item => item.CanAfford && !item.CanPurchase),
                "Even an affordable authored shop or story option cannot advertise an opening-only tactic as a valid mid-expedition grant.");
            foreach (var choice in forged)
            {
                var before = session.Snapshot;
                var commandCount = session.AcceptedCommands.Count;
                Require(!session.Submit(new ChooseShanheNodeOptionCommand(choice.Id, null, session.Revision)).Accepted &&
                    session.Revision == before.Revision && session.AcceptedCommands.Count == commandCount &&
                    session.Snapshot.Gold == before.Gold && session.Snapshot.Hp == before.Hp && session.Snapshot.TacticIds.Count == 0,
                    "Submitting a forged opening-only grant must reject atomically before its gold or health cost is paid.");
            }
            Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        }
        Require(session.Snapshot.Gold == 1000 && session.Snapshot.TacticIds.Count == 0 &&
            session.Snapshot.AvailableNodes.Single().Id == "battle",
            "Rejecting all four fake products still permits ordinary route continuation with the original balance and no unearned tactics.");
    }

    private static ShanheRewardDefinition[] WealthRewards(ShanheCampaignDefinition catalog) => Tiers
        .Select(tier => catalog.Rewards.Single(reward => reward.Id == "tactic-economy-" + tier.SourceId)).ToArray();

    private static ShanheCampaignDefinition Route(IReadOnlyList<ShanheRewardDefinition> rewards, int gold) =>
        new("fixture:shanhe-starting-wealth", "开局家财", [Player], ["shop"],
        [
            new("shop", "途中集市", ShanheNodeKind.Shop, ["story"],
                Choices: [new("supply", "购入军粮", GoldCost: 10, LeavesNode: false)]),
            new("story", "继续行军", ShanheNodeKind.Event, ["battle"], Choices: [new("march", "继续行军")]),
            BattleNode()
        ], rewards, StartingGold: gold);

    private static ShanheNodeDefinition BattleNode() => new("battle", "前哨", ShanheNodeKind.Battle, [],
        [new("footman", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId)])]);

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, ShanheCampaignOptions options) =>
        ShanheCampaignSession.Create(definition, options, StandardShanheCampaignCatalog.CreateBattleRegistry);

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Require(result.Accepted, result.Error ?? "Starting-wealth campaign command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}

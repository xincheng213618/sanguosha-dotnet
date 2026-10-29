using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static class ShanheStartingWealthChecks
{
    private const string Player = "classic:cao-cao";
    private static readonly (int SourceId, int StartingBalance)[] Tiers =
        [(2112, 300), (2139, 400), (2140, 600), (2141, 800)];

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
        ], rewards, StartingGold: gold) { Schools = StandardShanheCampaignCatalog.CurrentSchools };

    private static ShanheNodeDefinition BattleNode() => new("battle", "前哨", ShanheNodeKind.Battle, [],
        [new("footman", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId)])]);

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, ShanheCampaignOptions options) =>
        ShanheCampaignSession.Create(definition, options, StandardShanheCampaignCatalog.CreateBattleRegistry);

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Require(result.Accepted, result.Error ?? "Starting-wealth campaign command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}

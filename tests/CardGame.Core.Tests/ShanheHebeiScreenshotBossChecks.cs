using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHebeiScreenshotBossChecks
{
    public static void ChapterTwoRandomBossUsesObservedPanelAndExistingShuangxiong()
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(2,
            StandardShanheCampaignCatalog.Returning2026EditionId);
        foreach (var nodeId in new[] { "lijue", "guosi" })
        {
            var node = definition.Nodes.Single(node => node.Id == nodeId);
            var fixture = new ShanheCampaignDefinition("fixture:hebei-boss-" + nodeId, "河北双雄首领",
                ["classic:guan-yu"], [nodeId], [node with { NextNodeIds = [] }], definition.Rewards) { Schools = definition.Schools };
            // Verified against the real three-entry pools: seed 127 chooses index 2
            // on both routes, retaining every alternative in the authored pool.
            var session = ShanheCampaignSession.Create(fixture, new(127, "classic:guan-yu"),
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var preview = session.PreviewEncounter(nodeId);
            Need(node.Kind == ShanheNodeKind.BossBattle && node.ChapterNumber == 2 &&
                node.BossRewards is { FixedGold: 500 } && preview.Id == nodeId + ":hebei-twin-heroes" &&
                ReferenceEquals(preview, session.PreviewEncounter(nodeId)) &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before,
                "Both real chapter-two boss pools select the observed Hebei recipe without a preview reroll or journal write.");
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(before), fixture,
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(restored.PreviewEncounter(nodeId).Id == preview.Id &&
                restored.Submit(new MoveShanheNodeCommand(nodeId, restored.Revision)).Accepted,
                "Cold campaign recovery and the actual challenge retain the randomly selected Hebei boss.");
            var setup = restored.PendingBattleSetup!;
            Need(setup.EnemyRoster is [{ GeneralId: StandardShanheS5YanliangWenchouPackage.YanliangWenchouId,
                    MaxHpBonus: 5, Hp: 9, DrawBonus: 3, SlashBonus: 1 }] &&
                setup.AllyRoster.Count == 0 && setup.BossTransition is null,
                "The setup carries the screenshot's 9/9, Draw5 and Slash2 without an inferred follower, equipment or second form.");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            Need(registry.Generals[setup.EnemyRoster[0].GeneralId].BaseHp == 4 &&
                game.CreateSnapshot(0).Players is [_, { Hp: 9, MaxHp: 9, HandCount: 4 } enemy] &&
                enemy.Skills!.Select(skill => skill.ContentId).SequenceEqual([StandardShanheS5YanliangWenchouPackage.ShuangxiongId]) &&
                game.Submit(new StartGameCommand()).Accepted && game.CreateSnapshot(0).CurrentSeat == 0,
                "The native battle overlays the observed panel on the unchanged 4/4 source body, reuses the complete existing Shuangxiong, and gives the player initiative.");
        }
        Need(definition.Nodes.Single(node => node.Id == "dagu").Encounters!
            .Single(encounter => encounter.Id == "dagu:yanliang-wenchou-s5").Enemies
            is [{ MaxHpBonus: 0, DrawBonus: 0, SlashBonus: 0 }],
            "The independent ordinary source encounter retains its original base panel.");
    }

    private static void Need(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
}

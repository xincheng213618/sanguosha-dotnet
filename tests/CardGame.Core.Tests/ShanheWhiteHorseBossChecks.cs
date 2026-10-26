using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWhiteHorseBossChecks
{
    public static void ChapterTwoScreenshotBossKeepsPreviewRosterAndNativeStats()
    {
        foreach (var difficulty in new[] { 1, 2, 3 })
        {
            var definition = StandardShanheCampaignCatalog.CreateDefinition(difficulty,
                StandardShanheCampaignCatalog.Returning2026EditionId);
            foreach (var nodeId in new[] { "lijue", "guosi" })
            {
                var node = definition.Nodes.Single(node => node.Id == nodeId);
                var encounter = node.Encounters!.Single(encounter => encounter.Id == nodeId + ":white-horse-from-dragon");
                Need(node.Kind == ShanheNodeKind.BossBattle && node.ChapterNumber == 2 &&
                    node.BossRewards is { FixedGold: 500 } && node.Encounters!.Any(other => other.BossTransition is not null) &&
                    encounter is { BossTransition: null, Enemies.Count: 2, Allies.Count: 0, VictoryGold: 500 } &&
                    encounter.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(
                        [StandardShanheS5GongsunZanPackage.GongsunZanId, StandardShanheS5CaorenZhaoyunPackage.ZhaoyunId]) &&
                    encounter.Enemies.Select(enemy => enemy.Hp).SequenceEqual(new int?[] { 8, 7 }) &&
                    encounter.Enemies.All(enemy => enemy.MaxHpBonus == 4 && enemy.DrawBonus == 1 && enemy.SlashBonus == 0),
                    "Both chapter-two boss routes offer the screenshot's complete 8/8 and 7/7 roster, draw three and Slash once; no inferred difficulty growth or transformation.");
                if (difficulty != 2) continue;

                // Verified seeds select White Horse in the current three-entry boss pools.
                // Shorten only the route, retaining its actual encounters and chapter rewards.
                var fixture = new ShanheCampaignDefinition("fixture:white-horse-" + nodeId, "白马从龙首领",
                    ["classic:guan-yu"], [nodeId], [node with { NextNodeIds = [] }], definition.Rewards);
                var session = ShanheCampaignSession.Create(fixture, new(nodeId == "lijue" ? 71 : 3, "classic:guan-yu"),
                    StandardShanheCampaignCatalog.CreateBattleRegistry);
                var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                var preview = session.PreviewEncounter(nodeId);
                Need(preview.Id == encounter.Id && ReferenceEquals(preview, session.PreviewEncounter(nodeId)) &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before,
                    "The existing random boss pool selects White Horse deterministically without a preview journal write or reroll.");
                var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(before), fixture,
                    StandardShanheCampaignCatalog.CreateBattleRegistry);
                Need(restored.PreviewEncounter(nodeId).Id == preview.Id &&
                    restored.Submit(new MoveShanheNodeCommand(nodeId, restored.Revision)).Accepted,
                    "Restoring and challenging the available boss retains the previewed candidate.");
                var setup = restored.PendingBattleSetup!;
                Need(setup.EnemyRoster.SequenceEqual(preview.Enemies) && setup.BossTransition is null,
                    "The actual battle receives the previewed roster without borrowing Li Jue or Guo Si's second form.");
                var game = StandardShanheCampaignCatalog.CreateBattle(setup);
                var players = game.CreateSnapshot(0).Players;
                Need(players.Count == 3 && players[1] is { Hp: 8, MaxHp: 8, HandCount: 4 } &&
                    players[2] is { Hp: 7, MaxHp: 7, HandCount: 4 } &&
                    players[1].Skills!.Select(skill => skill.ContentId).Contains(StandardShanheS5GongsunZanPackage.QiaomengId) &&
                    players[2].Skills!.Select(skill => skill.ContentId).Contains("sp:chongzhen") &&
                    game.Submit(new StartGameCommand()).Accepted && game.CreateSnapshot(0).CurrentSeat == 0,
                    "The native three-seat battle starts at the observed HP with the existing matched skills and player initiative.");
            }
            Need(definition.Nodes.Where(node => node.ChapterNumber != 2).SelectMany(node => node.Encounters ?? [])
                .All(encounter => !encounter.Id.EndsWith(":white-horse-from-dragon", StringComparison.Ordinal)),
                "The screenshot boss does not leak into the opening or final chapter pools.");
        }
        var legacy = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.LocalEditionId);
        Need(legacy.Nodes.SelectMany(node => node.Encounters ?? []).All(encounter =>
            !encounter.Id.EndsWith(":white-horse-from-dragon", StringComparison.Ordinal)),
            "The explicit 2024 local edition keeps its original boss recipes.");
    }

    private static void Need(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
}

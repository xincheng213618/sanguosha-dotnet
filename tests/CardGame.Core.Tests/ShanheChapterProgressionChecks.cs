using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheChapterProgressionChecks
{
    public static void ThreeChapterRecipesStartNativeBattlesWithinEightSeats()
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var encounters = definition.Nodes.SelectMany(node => (node.Encounters ?? []).Select(encounter => (node, encounter))).ToArray();
        var ordinary = encounters.Where(item => item.node.Kind == ShanheNodeKind.Battle).ToArray();
        var codex = StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        Need(ordinary.Length > 0 && ordinary.All(item => item.encounter.VictoryGold == 200) &&
            codex.Nodes.Where(node => node.Kind == ShanheNodeKind.Battle)
                .SelectMany(node => node.Encounters!).All(encounter => encounter.VictoryGold == 200),
            "Every current ordinary encounter has the configured 200-coin base victory reward in both expedition entrypoints.");
        Need(encounters.All(item => item.encounter.Enemies.Count + (item.encounter.Allies?.Count ?? 0) + 1 <= 8) &&
            encounters.Where(item => item.node.ChapterNumber == 1).All(item => item.encounter.Enemies.Count is 1 or 2),
            "First-chapter encounters keep complete one/two-enemy rosters, and player, companion and all enemies count toward eight seats.");
        var moved = encounters.Single(item => item.encounter.Id == "guangcheng:yuanshu-s5");
        Need(moved.node.Id == "dagu" && moved.node.ChapterNumber == 2 && moved.encounter.Enemies.Select(enemy => enemy.GeneralId)
                .SequenceEqual([StandardShanheS5YuanshuPackage.YuanyaoId, StandardShanheS5YuanshuPackage.YuanshuId, StandardShanheS5YuanshuPackage.YuanyinId]) &&
            moved.encounter.Enemies.All(enemy => enemy.MaxHpBonus == 0 && enemy.InitialHandBonus == 0 && enemy.DrawBonus == 0),
            "The original three-person Yuan roster moves intact to chapter two without silently trimming a follower or changing its source panel.");
        foreach (var final in definition.Nodes.Where(node => node.ChapterNumber == 3 && node.Kind == ShanheNodeKind.BossBattle))
        {
            var recipes = final.Encounters!;
            Need(recipes.All(encounter => encounter.BossTransition is { EnemyIndex: 0 }) &&
                recipes.Where(encounter => encounter.Id != "luoyang:dong-clan-screenshot")
                    .All(encounter => encounter.BossTransition?.DrawCards == (encounter.Id == "luoyang:gaichao-s5" ? 4 : 3)) &&
                recipes.Single(encounter => encounter.Id == final.Id + ":chapter-single").Enemies.Count == 1 &&
                recipes.Single(encounter => encounter.Id == final.Id + ":chapter-warband").Enemies.Count == 6 &&
                recipes.Single(encounter => encounter.Id == final.Id + ":chapter-encirclement") is { Enemies.Count: 7, Allies.Count: 0 } &&
                recipes.Single(encounter => encounter.Id == final.Id + ":chapter-support").Enemies.Select(enemy => enemy.GeneralId)
                    .Contains("classic:guo-jia") &&
                recipes.Single(encounter => encounter.Id == final.Id + ":chapter-support").Enemies.Select(enemy => enemy.GeneralId)
                    .Contains("classic:hua-tuo"),
                "Every final route offers a two-stage single boss, boss with five/six troops, and the explicitly local Guo Jia/Hua Tuo support combination.");
        }
        var representativeIds = new[] { "hulao:gu-jin", "huaxiong:encounter", "lijue:encounter", "dagu:chapter-troops",
            "huanyuan:chapter-troops", "luoyang:chapter-support", "henei:chapter-single" };
        foreach (var id in representativeIds)
        {
            var (node, encounter) = encounters.Single(item => item.encounter.Id == id);
            var fixture = new ShanheCampaignDefinition("fixture:chapter-progression-" + node.ChapterNumber, "三章实际阵容",
                ["classic:guan-yu"], ["battle"],
                [new("battle", node.Name, ShanheNodeKind.BossBattle, [], [encounter])], []);
            var session = ShanheCampaignSession.Create(fixture, new(83, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(session.Submit(new MoveShanheNodeCommand("battle", session.Revision)).Accepted, "Actual map entry prepares the chosen chapter recipe.");
            var setup = session.PendingBattleSetup!;
            Need(setup.BossTransition == encounter.BossTransition && setup.EnemyRoster.Count == encounter.Enemies.Count &&
                setup.AllyRoster.Count == (encounter.Allies?.Count ?? 0), "The native setup carries the whole roster and the exact second-stage configuration.");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            var snapshot = game.CreateSnapshot(0);
            Need(snapshot.Players.Count == 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count && snapshot.Players.Count <= 8 &&
                (setup.BossTransition is null || registry.Generals.ContainsKey(setup.BossTransition.GeneralId)),
                "The actual battle includes every occupied seat and loads any future boss form before the transition.");
            for (var index = 0; index < setup.EnemyRoster.Count; index++)
            {
                var enemy = setup.EnemyRoster[index];
                var general = registry.Generals[enemy.GeneralId];
                var player = snapshot.Players[1 + setup.AllyRoster.Count + index];
                Need(player.GeneralId == enemy.GeneralId && player.MaxHp == general.BaseHp + enemy.MaxHpBonus &&
                    player.Hp == player.MaxHp &&
                    player.HandCount == (enemy.StartingHand?.Count ?? 4) + enemy.InitialHandBonus + (enemy.StartingCards?.Count ?? 0),
                    $"Local recipe {id}, enemy {index} ({enemy.GeneralId}): actual {player.Hp}/{player.MaxHp} HP, {player.HandCount} cards; " +
                    $"expected full {general.BaseHp + enemy.MaxHpBonus} HP and {(enemy.StartingHand?.Count ?? 4) + enemy.InitialHandBonus + (enemy.StartingCards?.Count ?? 0)} cards.");
                if (enemy.GeneralId.StartsWith("shanhe:local-", StringComparison.Ordinal))
                    Need(general.PortraitKey == "supporter", "Locally authored troops use the supporter portrait rather than an unrelated named general.");
            }
            Need(game.Submit(new StartGameCommand()).Accepted, "All three chapter representatives enter the native command flow.");
        }
    }

    private static void Need(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
}

using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheDifficultyPreviewUiChecks
{
    public static void PublishedDifficultyPreviewUsesActualChallengeStats()
    {
        foreach (var difficulty in new[] { 1, 2, 3 })
        {
            var published = StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var entry = published.Nodes.Single(node => node.Id == "hulao-start");
            // Select an existing authored recipe directly. No seed search and no
            // claim that this local scaling is a complete official difficulty table.
            var encounter = entry.Encounters!.Single(item => item.Id.EndsWith(":opening:yibing", StringComparison.Ordinal));
            var definition = published with
            {
                Nodes = [entry with { NextNodeIds = [], Encounters = [encounter], RewardIds = null, EliteUpgradeNodeId = null }],
                StartingNodeIds = [entry.Id], Rewards = [], RandomStartingNode = false
            };
            var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu"),
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            using var vm = new MainViewModel(false, 270020, true, new MemorySaveStore(), useExpandedContent: true,
                shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
                { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
            vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            vm.SelectedShanheDifficulty = vm.ShanheDifficulties.Single(option => option.Value == difficulty);
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
            typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, difficulty);
            vm.OpenShanheCommand.Execute(null);
            var node = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.IsAvailable);
            var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            vm.PreviewShanheNodeCommand.Execute(node);
            var card = vm.ShanheEncounterEnemyCards.Single();
            var enemy = encounter.Enemies.Single();
            Program.Assert(vm.IsShanheEncounterPreviewOpen && card.Name == "张郃" &&
                vm.ShanheChapterProgressText.Contains($"难度 {difficulty}", StringComparison.Ordinal) &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before,
                "The selected local difficulty previews its existing authored encounter without changing the campaign.");

            vm.ChallengeShanheEncounterCommand.Execute(null);
            var game = Program.Engine(vm);
            var setup = game.CreateCheckpoint().Options.ShanheBattle!;
            var registry = game.ContentRegistry!;
            var general = registry.Generals[enemy.GeneralId];
            var expectedMaximum = general.BaseHp + enemy.MaxHpBonus;
            var expectedHp = enemy.Hp ?? general.InitialHp ?? expectedMaximum;
            var expectedOpening = (enemy.StartingHand?.Count ?? 4) + enemy.InitialHandBonus + (enemy.StartingCards?.Count ?? 0);
            var native = game.CreateSnapshot(0).Players.Single(player => player.Seat == 1);
            var players = (IReadOnlyList<CharacterState>)typeof(GameEngine).GetField("_players",
                BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
            var draw = (RuleQueryEvaluation)typeof(GameEngine).GetMethod("EvaluateDrawCount",
                BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(game, [players[1]])!;
            Program.Assert(vm.IsShanheBattleSession && setup.EnemyRoster.SequenceEqual(encounter.Enemies) &&
                native.GeneralId == enemy.GeneralId && native.Hp == expectedHp && native.MaxHp == expectedMaximum &&
                native.HandCount == expectedOpening && draw.Value is FiniteRuleQueryValue value && value.Value == 2 + enemy.DrawBonus &&
                card.Stats.Contains($"体力 {native.Hp}/{native.MaxHp}", StringComparison.Ordinal) &&
                card.Stats.Contains($"起手 {expectedOpening}", StringComparison.Ordinal) &&
                card.Stats.Contains($"摸牌阶段 {2 + enemy.DrawBonus}", StringComparison.Ordinal),
                "Every local difficulty's preview health, opening hand and draw quota match the actual challenged native encounter and its rule query.");
            var cold = GameReplay.Restore(game.CreateCheckpoint(), registry);
            Program.Assert(SnapshotJson.Serialize(cold.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "The difficulty-specific battle statistics survive the ordinary checkpoint replay contract.");
        }
    }
}

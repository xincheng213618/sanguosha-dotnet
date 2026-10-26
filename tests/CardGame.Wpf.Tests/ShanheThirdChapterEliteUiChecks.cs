using System.Reflection;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheThirdChapterEliteUiChecks
{
    public static void OptionalElitePreviewsCompleteRosterAndRewardsWithoutFinalSettlement()
    {
        foreach (var useCodex in new[] { false, true })
        {
            var published = useCodex
                ? StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
            var elite = published.Nodes.Single(node => node.Id == StandardShanheCampaignCatalog.ThirdChapterDongwuEliteId);
            // Retain the real camp branch, elite and all onward final routes.
            var definition = published with
            {
                Nodes = published.Nodes.Where(node => node.Id is "finalcamp" || node.Id == elite.Id || elite.NextNodeIds.Contains(node.Id)).ToArray(),
                StartingNodeIds = ["finalcamp"], RandomStartingNode = false
            };
            var session = ShanheCampaignSession.Create(definition,
                new ShanheCampaignOptions(71, "classic:guan-yu") { UseCodexRewardPool = useCodex },
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            Program.Assert(session.Submit(new MoveShanheNodeCommand("finalcamp", session.Revision)).Accepted &&
                session.Submit(new ChooseShanheNodeOptionCommand("rest", null, session.Revision)).Accepted,
                "The real last camp opens the optional elite alongside the three final routes.");
            using var vm = new MainViewModel(false, 270038, true, new MemorySaveStore(), useExpandedContent: true,
                shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
                { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
            vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
            typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, 2);
            vm.OpenShanheCommand.Execute(null);
            var node = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.Id == elite.Id && item.IsAvailable);
            var mapChoices = vm.ShanheMapNodes.Where(item => item.IsAvailable).ToArray();
            Program.Assert(mapChoices.Length == 4 && mapChoices.SelectMany((first, index) => mapChoices.Skip(index + 1)
                    .Select(second => Math.Abs(first.CenterX - second.CenterX) >= 136 || Math.Abs(first.CenterY - second.CenterY) >= 132)).All(separate => separate),
                "All four actual choices keep their 136 by 132 map buttons separately selectable.");
            var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            vm.PreviewShanheNodeCommand.Execute(node);
            var cards = vm.ShanheEncounterEnemyCards;
            Program.Assert(node.Id == elite.Id && node.KindLabel == "精英" &&
                vm.ShanheChapterTitle.Contains("第三章", StringComparison.Ordinal) &&
                vm.ShanheChapterObjectiveText.Contains("精英挑战或最终路线", StringComparison.Ordinal) &&
                vm.IsShanheEncounterPreviewOpen && cards.Select(card => card.Name).SequenceEqual(["黄盖", "韩当", "程普"]) &&
                cards.All(card => card.Portrait is ImageBrush && card.Stats.Contains("体力 5/5", StringComparison.Ordinal) &&
                    card.Stats.Contains("摸牌阶段 4 · 出杀次数 2", StringComparison.Ordinal)) &&
                !vm.ShanheEncounterEnemyDetail.Contains("二阶段", StringComparison.Ordinal),
                "Both campaign entry points show a third-chapter elite with all three real portraits and base 5/5, Draw4, Slash2 panels, without a boss second form.");
            Program.Assert(vm.ShanheEncounterRewardText.Contains("基础铜币 × 200", StringComparison.Ordinal) &&
                vm.ShanheEncounterRewardText.Contains("稀有", StringComparison.Ordinal) &&
                !vm.ShanheEncounterRewardText.Contains("最终结算", StringComparison.Ordinal) &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before && session.PendingBattleSetup is null,
                "The optional elite previews its normal copper and rare build rewards without claiming final settlement, journaling a move or starting battle.");
        }
    }
}

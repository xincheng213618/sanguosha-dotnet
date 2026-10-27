using System.IO;
using System.Windows.Controls;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void UnplayedOpeningRemainsClickableAfterFirstReward(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var definition = new ShanheCampaignDefinition("fixture:opening-detour-ui", "开局补战", [Player], ["left", "right"],
            [
                new("left", "村落", ShanheNodeKind.Battle, ["camp"], [new("left-guard", "村落驻军", [new(Enemy)], VictoryGold: 200)], ["tactic"])
                    { ChapterNumber = 1 },
                new("right", "戍寨", ShanheNodeKind.Battle, ["camp"], [new("right-guard", "戍寨驻军", [new(Enemy)], VictoryGold: 200)], ["tactic"])
                    { ChapterNumber = 1 },
                new("camp", "营地", ShanheNodeKind.Camp, ["next"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
                new("next", "下章", ShanheNodeKind.Camp, [], Choices: [new("continue", "继续")]) { ChapterNumber = 2 }
            ], [new("tactic", "战法·整备", Bonuses: new(Draw: 1))]) { AllowChapterDetours = true };
            var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
            Accept(session, new MoveShanheNodeCommand("left", session.Revision));
            WinPendingBattle(session);
            Attach(vm, session);
            Layout(panel);
            var reward = vm.ShanheRewardCards.Single();
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, reward), panel);
            Click((Button)panel.FindName("ShanheConfirmRewardButton"), panel);
            var other = vm.ShanheMapNodes.Single(node => node.Node.Id == "right");
            var completed = vm.ShanheMapNodes.Single(node => node.Node.Id == "left");
            Program.Assert(other.IsAvailable && completed.IsCompleted && !completed.IsAvailable &&
                    vm.ShanheMapNodes.Single(node => node.Node.Id == "camp").IsAvailable &&
                    vm.ShanheChapterObjectiveText.Contains("返回"),
                "Claiming the first opening reward keeps the unplayed opening and forward camp enabled on the actual map.");
            var before = Snapshot(session);
            var button = Bound(panel, vm.PreviewShanheNodeCommand, other.Node);
            Program.Assert(button.IsEnabled && button.Command!.CanExecute(button.CommandParameter) &&
                    session.PreviewEncounter(other.Node.Id).Id == "right-guard" && Snapshot(session) == before,
                "The retained opening button must keep its actual preview command enabled and its encounter available without changing the journal.");
            Layout(panel);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-unplayed-opening-detour.png"));
        }
        finally { panel.DataContext = null; }
    }
}

using System.IO;
using System.Windows.Controls;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void BossCardAndRareSkillDeclinesKeepTacticGroupMandatory(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var definition = new ShanheCampaignDefinition("fixture:shanhe-group-decline-ui", "首领奖励取舍", [Player], ["boss"],
            [
                new("boss", "渡口首领", ShanheNodeKind.BossBattle, ["home"], [new("guard", "守军", [new(Enemy)], VictoryGold: 500)])
                {
                    ChapterNumber = 1, RewardSkipGold = 100,
                    BossRewards = new(500, 1,
                    [new("tactics", "史诗战法", ["tactic"], 1), new("cards", "史诗手牌", ["card"], 1), new("skills", "稀有技能", ["skill"], 1)])
                },
                new("home", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])
            ],
            [
                new("tactic", "刀阵", Bonuses: new(Slash: 1)) { Rarity = 3 },
                new("card", "随身杀", StartingCards: [new(CardKind.Slash)]) { Rarity = 3 },
                new("skill", "传习·振奋", SkillId: Learned) { Rarity = 2 }
            ], StartingGold: 10);
            var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
            Accept(session, new MoveShanheNodeCommand("boss", session.Revision));
            WinPendingBattle(session);
            Attach(vm, session); Layout(panel);
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            var finish = (Button)panel.FindName("ShanheConfirmBossRewardButton");
            Open("cards");
            var skip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            var staleSkipButton = Bound(panel, vm.SelectShanheRewardCardCommand, skip);
            Program.Assert(skip.Category.Contains("史诗随身牌") && skip.BonusText.Contains("100") &&
                vm.ShanheRewardNotice.Contains("放弃") && vm.ShanheRewardNotice.Contains("100"),
                "The boss hand-card decline visibly identifies the real category, rarity and configured copper amount.");
            Click(staleSkipButton, panel);
            Program.Assert(confirm.IsEnabled && confirm.Content.ToString()!.Contains("铜"),
                "Selecting the group decline enables its explicit copper confirmation.");
            Click(confirm, panel);
            Program.Assert(vm.IsShanheBossRewardOpen && !finish.IsEnabled && session.Snapshot.Gold == 610 &&
                session.Snapshot.StartingCards.Count == 0 && vm.ShanheBossRewardGroups.Single(group => group.Id == "cards").IsResolved &&
                vm.ShanheBossRewardGroups.Single(group => group.Id == "cards").SelectedName.Contains("100") &&
                vm.ShanheBossRewardGroups.Where(group => group.Id != "cards").All(group => !group.IsResolved),
                "Confirming card compensation returns to the summary, marks only that group and leaves the next route closed.");
            var afterCard = Snapshot(session);
            RawClick(staleSkipButton); vm.SelectShanheRewardCardCommand.Execute(skip); vm.ConfirmShanheRewardCommand.Execute(null);
            Layout(panel);
            Program.Assert(Snapshot(session) == afterCard, "A retired group decline control cannot receive the payment twice.");
            Open("tactics");
            Program.Assert(vm.ShanheRewardCards.Count == 1 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                !vm.ShanheRewardNotice.Contains("放弃"), "Boss tactics have an actual mandatory choice without a compensation card or notice.");
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, vm.ShanheRewardCards.Single()), panel);
            Click(confirm, panel);
            Open("skills");
            skip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            Program.Assert(skip.Category.Contains("稀有额外技能") && skip.BonusText.Contains("100"),
                "The rare-skill decline visibly uses its specified hundred-coin price.");
            Program.Assert(Program.Find<TextBlock>((ItemsControl)panel.FindName("ShanheRewardCardRows"))
                .Any(text => text.Text == "稀有 · 额外技能"),
                "The rendered reward card must label its actual rare quality beside the skill category.");
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, skip), panel);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-boss-rare-skill-decline.png"));
            Click(confirm, panel);
            Program.Assert(finish.IsEnabled && session.Snapshot.Gold == 710 && session.Snapshot.ExtraSkillIds.Count == 0 &&
                session.Snapshot.Bonuses.Slash == 1 && session.Snapshot.CompletedNodeIds.Count == 0,
                "A rare-skill decline pays once, keeps the mandatory tactic and waits for the final boss summary confirmation.");
            Click(finish, panel);
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
                session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Select(command => command.GroupId).SequenceEqual(["cards", "skills"]),
                "The real controls journal the two exact group declines and open the route only after final confirmation.");

            void Open(string id) => Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand,
                vm.ShanheBossRewardGroups.Single(group => group.Id == id)), panel);
        }
        finally { panel.DataContext = null; }
    }
}

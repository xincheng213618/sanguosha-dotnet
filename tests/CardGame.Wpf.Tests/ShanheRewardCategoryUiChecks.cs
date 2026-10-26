using System.IO;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void CategoriesFourChoicesBonusOnlyAndThreeBossGroups(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var session = WinCategorizedEliteSession();
            Attach(vm, session); Layout(panel);
            var rewardOverlay = (FrameworkElement)panel.FindName("ShanheRewardOverlay");
            var bossOverlay = (FrameworkElement)panel.FindName("ShanheBossRewardOverlay");
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            var finish = (Button)panel.FindName("ShanheConfirmBossRewardButton");
            var ordinaryRevision = Program.Engine(vm).Revision;
            var category = vm.ShanheRewardCards.Single(card => card.RewardCategoryChoice == ShanheRewardCategory.Tactic);
            var retiredCategoryButton = Bound(panel, vm.SelectShanheRewardCardCommand, category);
            Program.Assert(vm.IsShanheRewardOpen && rewardOverlay.Visibility == Visibility.Visible &&
                vm.ShanheRewardCards.Count(card => card.RewardCategoryChoice is not null) == 2 &&
                vm.ShanheRewardCards.All(card => !card.IsSkip) && session.Snapshot.RewardOffers.Count == 0 &&
                vm.ShanheRewardHeading.Contains("类", StringComparison.Ordinal) &&
                confirm.Content.ToString()!.Contains("类", StringComparison.Ordinal) && !confirm.IsEnabled,
                "An actual elite victory starts with bound category cards; compensation requires committing an eligible category.");
            var before = Snapshot(session);
            Click(retiredCategoryButton, panel);
            Program.Assert(category.IsSelected && confirm.IsEnabled && Snapshot(session) == before,
                "Selecting a category previews it; only the bound confirmation commits the category choice.");
            Click(confirm, panel);
            Program.Assert(session.Snapshot.RewardCategories.Count == 0 && session.Snapshot.RewardOffers.Count == 4 &&
                vm.ShanheRewardCards.Count == 4 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                vm.ShanheRewardCards.All(card => card.RewardCategoryChoice is null) && !confirm.IsEnabled &&
                session.AcceptedCommands.OfType<ChooseShanheRewardCategoryCommand>().Single().Category == ShanheRewardCategory.Tactic,
                "The equipped category tactic opens four actual frozen tactic candidates with no tactic decline route.");
            before = Snapshot(session);
            RawClick(retiredCategoryButton);
            vm.SelectShanheRewardCardCommand.Execute(category);
            vm.ConfirmShanheRewardCommand.Execute(null);
            Layout(panel);
            Program.Assert(Snapshot(session) == before && vm.SelectedShanheRewardCard is null &&
                !vm.SelectShanheRewardCardCommand.CanExecute(category) && !confirm.IsEnabled,
                "The retired category card cannot select or reopen the category after its candidates have appeared.");

            var primary = vm.ShanheRewardCards.First(card => !card.IsSkip);
            var primaryButton = Bound(panel, vm.SelectShanheRewardCardCommand, primary);
            Click(primaryButton, panel);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-reward-category-four-choices.png"));
            var gold = session.Snapshot.Gold;
            Click(confirm, panel);
            var bonus = session.Snapshot.BossRewardSummary;
            var layout = (Grid)panel.FindName("ShanheBossRewardCardsLayout");
            var groupRows = (ItemsControl)panel.FindName("ShanheBossRewardGroupRows");
            var goldCard = (FrameworkElement)panel.FindName("ShanheBossFixedGoldCard");
            var slotCard = (FrameworkElement)panel.FindName("ShanheBossFixedSkillSlotsCard");
            var subtitle = (TextBlock)panel.FindName("ShanheBossRewardSubtitle");
            Program.Assert(bonus is { IsBonusOnly: true, CanConfirm: false } && bonus.Groups.Single().Offers.Count == 4 &&
                session.Snapshot.Phase == ShanheCampaignPhase.Reward && session.Snapshot.AvailableNodes.Count == 0 &&
                session.Snapshot.Gold == gold && vm.IsShanheBonusRewardOnly && vm.IsShanheBossRewardOpen &&
                !vm.HasShanheBossFixedRewards && !vm.IsShanheRewardOpen &&
                rewardOverlay.Visibility == Visibility.Collapsed && bossOverlay.Visibility == Visibility.Visible &&
                goldCard.Visibility == Visibility.Collapsed && slotCard.Visibility == Visibility.Collapsed &&
                layout.ColumnDefinitions[0].ActualWidth == 0 && layout.ColumnDefinitions[2].ActualWidth == 0 &&
                Math.Abs(groupRows.ActualWidth - layout.ActualWidth) < 1 && groupRows.ActualWidth > 1000 &&
                vm.ShanheBossRewardTitle.Contains("追 加", StringComparison.Ordinal) &&
                subtitle.Text.Contains("追加", StringComparison.Ordinal) &&
                !subtitle.Text.Contains("技能槽", StringComparison.Ordinal) &&
                !finish.Content.ToString()!.Contains("章", StringComparison.Ordinal) && !finish.IsEnabled,
                "Claiming the primary reward opens the actual bonus-only summary, collapses both fixed reward cards and columns, and gives its group the full width without a next-chapter promise.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-reward-bonus-only-overview.png"));
            before = Snapshot(session);
            RawClick(primaryButton);
            vm.SelectShanheRewardCardCommand.Execute(primary);
            RawClick(finish);
            Program.Assert(Snapshot(session) == before && vm.IsShanheBossRewardOpen,
                "A retired primary card and an early continuation cannot bypass the pending additional reward.");

            var group = vm.ShanheBossRewardGroups.Single();
            Program.Assert(group.SelectedName.Contains("4", StringComparison.Ordinal),
                "The overview labels its actual four candidates without assuming a three-choice group.");
            Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand, group), panel);
            Program.Assert(vm.ShanheRewardCards.Count == 4 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                vm.ShanheRewardHeading.Contains("追 加", StringComparison.Ordinal) &&
                vm.ShanheRewardNotice.Contains("4", StringComparison.Ordinal) &&
                confirm.Content.ToString()!.Contains("追 加", StringComparison.Ordinal),
                "Opening the additional group shows its four real candidates and uses additional-reward wording without ordinary skip compensation.");
            var extra = vm.ShanheRewardCards.First();
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, extra), panel);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-reward-bonus-four-choices.png"));
            Click(confirm, panel);
            Program.Assert(vm.IsShanheBossRewardOpen && vm.ShanheBossRewardGroups.Single().IsResolved && finish.IsEnabled &&
                session.Snapshot.TacticIds.Contains(extra.Id) && session.Snapshot.Phase == ShanheCampaignPhase.Reward &&
                session.Snapshot.CompletedNodeIds.Count == 0 && session.Snapshot.Gold == gold,
                "The additional claim grants its actual tactic once and returns to the summary before route continuation.");
            Click(finish, panel);
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
                session.Snapshot.CompletedNodeIds.SequenceEqual(["battle"]) && session.Snapshot.BossRewardSummary is null &&
                !vm.IsShanheRewardOpen && !vm.IsShanheBossRewardOpen && session.Snapshot.Gold == gold &&
                session.AcceptedCommands.OfType<ChooseShanheRewardCommand>().Count() == 1 &&
                session.AcceptedCommands.OfType<ChooseShanheBossRewardCommand>().Count() == 1 &&
                session.AcceptedCommands.OfType<ConfirmShanheBossRewardCommand>().Count() == 1 &&
                Program.Engine(vm).Revision == ordinaryRevision,
                "The bonus summary confirmation completes the current node exactly once and opens the actual next route.");

            CheckThreeBossGroups(vm, panel, output);
        }
        finally { panel.DataContext = null; }
    }

    private static void CheckThreeBossGroups(MainViewModel vm, ShanheCampaignPanel panel, string output)
    {
        var session = CreateThreeBossSession(includeBonusSkill: true);
        Accept(session, new MoveShanheNodeCommand("boss1", session.Revision));
        WinPendingBattle(session);
        Attach(vm, session); Layout(panel);
        var layout = (Grid)panel.FindName("ShanheBossRewardCardsLayout");
        var rows = (ItemsControl)panel.FindName("ShanheBossRewardGroupRows");
        Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-boss-reward-three-groups.png"));
        Program.Assert(session.Snapshot.BossRewardSummary is { IsBonusOnly: false } summary && summary.Groups.Count == 3 &&
            vm.IsShanheBossRewardOpen && vm.HasShanheBossFixedRewards &&
            ((FrameworkElement)panel.FindName("ShanheBossFixedGoldCard")).Visibility == Visibility.Visible &&
            ((FrameworkElement)panel.FindName("ShanheBossFixedSkillSlotsCard")).Visibility == Visibility.Visible &&
            layout.ColumnDefinitions[0].ActualWidth > 0 && layout.ColumnDefinitions[2].ActualWidth > 0 &&
            rows.ActualWidth > layout.ActualWidth * 0.55,
            "A real boss victory with the bonus-skill tactic keeps both fixed credits and expands the middle column for its three reward groups.");
        foreach (var group in vm.ShanheBossRewardGroups)
        {
            var button = Bound(panel, vm.OpenShanheBossRewardGroupCommand, group);
            var title = Program.Find<TextBlock>(button).Single(text => text.Text == group.Name);
            Program.Assert(button.ActualWidth >= 240 && title.TextWrapping == TextWrapping.Wrap &&
                title.DesiredSize.Height <= title.ActualHeight + 1 && button.IsEnabled,
                $"Every actual group remains wide enough for its wrapped title and bound selection button in the three-group overview. " +
                $"Group '{group.Id}': button width={button.ActualWidth:F1}, title desired={title.DesiredSize}, " +
                $"title actual={title.ActualWidth:F1}x{title.ActualHeight:F1}, enabled={button.IsEnabled}, " +
                $"layout width={layout.ActualWidth:F1}, group column={rows.ActualWidth:F1}.");
        }
    }

    private static ShanheCampaignSession WinCategorizedEliteSession()
    {
        var primary = Enumerable.Range(1, 4).Select(index => new ShanheRewardDefinition("primary-tactic-" + index,
            "行军操练" + index, Bonuses: new(MaxHp: index)) { Description = "本次征程体力上限增加。" }).ToArray();
        var bonus = Enumerable.Range(1, 4).Select(index => new ShanheRewardDefinition("bonus-tactic-" + index,
            "稀有军阵" + index) { TacticSkillId = "fixture:shanhe-ui-tactic" + index, Rarity = 2,
                Description = "追加获得一项稀有战法，后续战斗攻击范围加一。" }).ToArray();
        var cards = new ShanheRewardDefinition[]
        {
            new("card-slash", "随身杀", StartingCards: [new(CardKind.Slash)]),
            new("card-crossbow", "随身连弩", StartingEquipment: [new(CardKind.Crossbow)])
        };
        var sources = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => source.SourceId is 2013 or 2017 or 2018).ToArray();
        var economy = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description });
        var definition = new ShanheCampaignDefinition("fixture:shanhe-ui-categorized-elite", "精英凯旋", [Player], ["battle"],
        [
            new("battle", "渡口精英", ShanheNodeKind.EliteBattle, ["home"],
                [new("gate", "精英守军", [new(Enemy)], VictoryGold: 7)], primary.Concat(cards).Select(reward => reward.Id).ToArray())
                { CategorizedRewards = true, RewardSkipGold = 100, StoryText = "守军已退，整理此战收获。" },
            new("home", "河畔营火", ShanheNodeKind.Camp, [], Choices: [new("finish", "继续前行")])
        ], primary.Concat(cards).Concat(bonus).Concat(economy).ToArray(), StartingGold: 10, ExtraSkillSlots: 1);
        var session = ShanheCampaignSession.Create(definition, new(71, Player)
            { EquippedTacticIds = sources.Select(source => source.RewardId).ToArray() }, _ => FixtureRegistry.Value);
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        WinPendingBattle(session);
        return session;
    }
}

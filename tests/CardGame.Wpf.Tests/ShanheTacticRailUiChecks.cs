using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ellipse = System.Windows.Shapes.Ellipse;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheTacticRailUiChecks
{
    public static void OwnedTacticsUseSharedBadgesAndPreserveActiveSkillEntries(string output)
    {
        var catalogue = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var dragon = catalogue.Rewards.Single(reward => reward.TacticSkillId == StandardShanheDrawThresholdPackage.ShenlongBaiwei2SkillId)
            with { SchoolMembership = null };
        var active = new ShanheRewardDefinition("fixture:active-tactic", "主动战法入口")
            { TacticSkillId = "classic:zhiheng", Rarity = 2 };
        var definition = new ShanheCampaignDefinition("fixture:shanhe-tactic-rail", "战法展示", ["classic:guan-yu"], ["battle"],
            [new("battle", "驻军", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("classic:ma-chao", MaxHpBonus: 10)])])],
            [dragon, active]);
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu")
            { EquippedTacticIds = [dragon.Id, active.Id] }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var vm = new MainViewModel(false, 270028, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        Set("_shanheSession", session);
        vm.OpenShanheCommand.Execute(null);
        vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable));
        var game = Program.Engine(vm);
        var setup = session.PendingBattleSetup!;
        Program.Assert(vm.IsShanheBattleSession && setup.TacticSkillIds.Contains(dragon.TacticSkillId!) &&
            game.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == dragon.TacticSkillId) &&
            vm.HumanSkillCards.All(skill => skill.ContentId != dragon.TacticSkillId) &&
            vm.HumanSkillCards.Any(skill => skill.ContentId == active.TacticSkillId) &&
            session.Snapshot.ExtraSkillIds.Count == 0 && vm.ShanheExtraSkillCards.Count == 0,
            "A real owned passive tactic retains its native rule, leaves the ordinary skill rail, consumes no extra skill slot and keeps active tactic entries usable.");
        var badge = vm.HumanPlayer!.WushuangTactics.Single(item => item.SkillId == dragon.TacticSkillId);
        var sourceId = StandardShanheCampaignCatalog.ShopMetadataByRewardId[dragon.Id].SourceRewardId;
        var nativeSkill = game.ContentRegistry!.GetSkill(dragon.TacticSkillId!);
        var nativeRule = nativeSkill.Description;
        var nativeBadge = new WushuangTacticViewModel(sourceId);
        Program.Assert(vm.HumanPlayer.WushuangTactics.Count == 2 && badge.Id == sourceId && badge.Name == dragon.Name &&
            badge.Tooltip.StartsWith(dragon.Name + "\n", StringComparison.Ordinal) &&
            !badge.Tooltip.StartsWith("战法 · 战法", StringComparison.Ordinal) &&
            badge.Tooltip.Contains(nativeRule, StringComparison.Ordinal) && badge.Icon is BitmapSource { PixelWidth: > 0 } &&
            badge.Frame is { IsFrozen: true } && ReferenceEquals(badge.Icon, new WushuangTacticViewModel(sourceId).Icon),
            "Shanhe uses the shared badge artwork and frame while keeping its own content name and executable rule description.");
        Program.Assert(dragon.Name == "战法·" + nativeSkill.Name && badge.DisplayName == nativeSkill.Name &&
                badge.DisplayName != badge.Name && !nativeBadge.Name.StartsWith("战法", StringComparison.Ordinal) &&
                nativeBadge.DisplayName == nativeBadge.Name,
            "The published prefixed dragon keeps its full acquisition identity and native rules while its compact caption uses the actual skill name; unprefixed native tactic names remain intact.");

        var window = new MainWindow(vm); window.ApplyTemplate();
        try
        {
            var root = (FrameworkElement)window.Content;
            var renderPath = Path.Combine(output, "shanhe-owned-tactic-badges.png");
            Program.Render(root, 1440, 880, renderPath);
            var rail = (ScrollViewer)window.FindName("HumanWushuangTactics");
            var items = Program.Find<ItemsControl>(rail).Single();
            var displayed = items.Items.Cast<WushuangTacticViewModel>().Single(item => item.SkillId == dragon.TacticSkillId);
            var icon = Program.Find<Ellipse>(items).Single(ellipse => ellipse.DataContext is WushuangTacticViewModel tactic &&
                tactic.SkillId == dragon.TacticSkillId && ellipse.Fill is ImageBrush);
            var frame = Program.Find<Image>(items).Single(image => image.DataContext is WushuangTacticViewModel tactic &&
                tactic.SkillId == dragon.TacticSkillId && ReferenceEquals(image.Source, displayed.Frame));
            var caption = Program.Find<TextBlock>(items).Single(text => ReferenceEquals(text.DataContext, displayed));
            Program.Assert(rail.Visibility == Visibility.Visible && rail.ActualHeight > 0 && rail.ActualWidth > 0 &&
                icon.ActualWidth > 0 && icon.ActualHeight > 0 && frame.ActualWidth > 0 && frame.ActualHeight > 0 &&
                icon.Fill is ImageBrush { ImageSource: BitmapSource { PixelWidth: > 0 } } brush &&
                ReferenceEquals(brush.ImageSource, displayed.Icon) && displayed.Id == sourceId && File.Exists(renderPath),
                "The actual table renders owned campaign tactics through the same badge rail used by single-player Wushuang.");
            Program.Assert(caption.Visibility == Visibility.Visible && caption.ActualWidth > 0 && caption.ActualHeight > 0 &&
                    caption.Text == nativeSkill.Name && caption.Text == displayed.DisplayName && displayed.Name == dragon.Name &&
                    displayed.Tooltip.Contains(nativeRule, StringComparison.Ordinal),
                "The actual dragon badge visibly uses its short native caption while its model name and full rule tooltip retain the original identity.");
            var unprefixed = items.Items.Cast<WushuangTacticViewModel>().Single(item => item.SkillId == active.TacticSkillId);
            var unprefixedCaption = Program.Find<TextBlock>(items).Single(text => ReferenceEquals(text.DataContext, unprefixed));
            Program.Assert(unprefixed.Name == active.Name && unprefixed.DisplayName == active.Name && unprefixedCaption.Text == active.Name &&
                    unprefixedCaption.Visibility == Visibility.Visible && unprefixedCaption.ActualWidth > 0 && unprefixedCaption.ActualHeight > 0,
                "A name containing the tactic category word in its middle remains visibly intact when it has no category prefix.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }

        // The same executable rule may be granted as an ordinary extra skill.
        // Only actual tactic ownership is sufficient to move it into the badge rail.
        var ordinaryGrant = setup with { TacticSkillIds = [], ExtraSkillIds = [dragon.TacticSkillId!] };
        var ordinaryGame = StandardShanheCampaignCatalog.CreateBattle(ordinaryGrant);
        Program.Assert(ordinaryGame.Submit(new StartGameCommand()).Accepted, "The same rule can start through an ordinary extra-skill grant.");
        Set("_shanheSession", null);
        Set("_shanheTableGame", ordinaryGame); Set("_shanheBattleGame", ordinaryGame); Set("_shanheTableSetup", ordinaryGrant);
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, [ordinaryGame]);
        Program.Assert(vm.HumanSkillCards.Any(skill => skill.ContentId == dragon.TacticSkillId) && !vm.HumanPlayer!.HasWushuangTactics,
            "Names and locked tags cannot misclassify an ordinary acquired skill as a campaign tactic.");

        void Set(string field, object? value) => typeof(MainViewModel).GetField(field,
            BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, value);
    }
}

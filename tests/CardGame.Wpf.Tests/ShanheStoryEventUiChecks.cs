using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheStoryEventUiChecks
{
    public static void RecruitmentSkillChoicesAndBattleButtons(string output)
    {
        var production = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var source = production.Nodes.Single(node => node.Id == "village");
        var market = production.Nodes.First(node => node.Kind == ShanheNodeKind.Shop);
        foreach (var storyId in new[] { StandardShanheCampaignCatalog.RecruitmentStoryId, "story:jianluojinpao", "story:shiyoudaru",
            "story:huangjin-elite", StandardShanheCampaignCatalog.RareTacticStoryId })
        {
            var tacticStory = storyId == StandardShanheCampaignCatalog.RareTacticStoryId;
            var story = (source.StoryEvents ?? []).Concat(source.SupplementalStoryEvents ?? []).Single(story => story.Id == storyId);
            var battleIds = story.Choices.Select(choice => choice.BattleNodeId).OfType<string>().ToHashSet();
            foreach (var battle in production.Nodes.Where(node => battleIds.Contains(node.Id)).ToArray())
                if (battle.EliteUpgradeNodeId is { } eliteId) battleIds.Add(eliteId);
            var definition = new ShanheCampaignDefinition("fixture:story-ui", "奇遇", ["classic:guan-yu"], ["village"],
                new[] { source with { NextNodeIds = [market.Id], StoryEvents = [story], SupplementalStoryEvents = null },
                    market with { NextNodeIds = ["after"] },
                    new ShanheNodeDefinition("after", "后续战斗", ShanheNodeKind.Battle, [],
                        [new("after", "后续战斗", [new("classic:cao-cao")])]) }
                .Concat(production.Nodes.Where(node => battleIds.Contains(node.Id)).Select(node => node with
                { NextNodeIds = node.EliteUpgradeNodeId is { } eliteId ? ["after", eliteId] : ["after"] }))
                // This fixture opens one story directly, rather than publishing a
                // partial six-step chapter. Keep its real upgrade profiles and rewards.
                .Select(node => node with { ChapterRouteId = null, ChapterRole = ShanheChapterNodeRole.None,
                    ChapterProgressStep = 0, RequiredChapterProgress = 0 }).ToArray(),
                production.Rewards, StartingGold: storyId == StandardShanheCampaignCatalog.RecruitmentStoryId ? 399 : 600, ExtraSkillSlots: 2)
                { Schools = production.Schools };
            var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu")
                { UseCodexRewardPool = true, PersistentMarket = true, UseBalancedJourneyRewards = tacticStory },
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            Program.Assert(session.Submit(new MoveShanheNodeCommand("village", session.Revision)).Accepted, "The actual event is revealed through a map command.");
            session = ShanheCampaignSession.Restore(session.CreateCheckpoint(), definition,
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            using var model = new MainViewModel(false, 71, true, new MemorySaveStore(),
                shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, session);
            typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
            var panel = new ShanheCampaignPanel { DataContext = model };
            try
            {
                Layout(panel);
                Program.Assert(model.ShanheContextTitle == story.Name && model.ShanheChoices.Count == 3,
                    "The context shows the revealed story title and its three real options.");
                var context = (Border)panel.FindName("ShanheContextPanel");
                Program.Assert(model.IsShanheShopActive && context.Visibility == Visibility.Visible && context.ActualHeight > 0,
                    "An available persistent market must not hide the pending story choices on the journey map.");
                var tacticOffers = tacticStory ? session.Snapshot.NodeChoices
                    .Where(option => option.Id.StartsWith("story-tactic:", StringComparison.Ordinal)).ToArray() : [];
                if (tacticStory)
                    Program.Assert(tacticOffers.Length == 2 && tacticOffers.Select(option => option.RewardId).Distinct().Count() == 2 &&
                            tacticOffers.All(option => story.TacticRewardIds!.Contains(option.RewardId!) &&
                                production.Rewards.Single(reward => reward.Id == option.RewardId) is { Rarity: 2, SkillId: null } &&
                                StandardShanheCampaignCatalog.ShopMetadataByRewardId.TryGetValue(option.RewardId!, out var metadata) &&
                                ShanheArt.Get(metadata.SourceRewardId) is BitmapSource { PixelWidth: > 0 }),
                        "The tactic story reveals two distinct real rare tactics from its typed source pool, each with its original loaded artwork.");
                var choice = tacticStory ? model.ShanheChoices.Single(item => item.Id == tacticOffers[0].Id) :
                    model.ShanheChoices.Single(item => item.Id == (storyId switch
                {
                    StandardShanheCampaignCatalog.RecruitmentStoryId => "recruit-normal",
                    "story:jianluojinpao" => "recruit-xunyu-elite",
                    "story:shiyoudaru" => "decline-story-skills",
                    _ => "fight-elite"
                }));
                if (storyId == StandardShanheCampaignCatalog.RecruitmentStoryId)
                    Program.Assert(!model.ShanheChoices.Single(item => item.Id == "recruit-elite").IsEnabled &&
                        model.ShanheChoices.Single(item => item.Id == "recruit-elite").Description.Contains("回火"),
                        "The unaffordable paid version remains visible with its actual extra skill and a disabled button.");
                Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-story-" + storyId.Replace(':', '-') + ".png"));
                var button = Program.Find<Button>(panel).Single(button => ReferenceEquals(button.Command, model.ChooseShanheOptionCommand) && ReferenceEquals(button.CommandParameter, choice));
                typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
                Layout(panel);
                if (storyId is StandardShanheCampaignCatalog.RecruitmentStoryId or "story:jianluojinpao")
                    Program.Assert(model.ShanheRecruitedCompanionCards.Count == 1 &&
                        model.ShanheRecruitedCompanionCards.Single().Name.Contains(storyId == "story:jianluojinpao" ? "荀彧 · 精英" : "蔡文姬 · 普通"),
                        "The HUD displays the actual recruited name and tier.");
                else if (storyId == "story:shiyoudaru")
                    Program.Assert(session.Snapshot.Gold == 810 && session.Snapshot.ExtraSkillIds.Count == 0, "The real decline button credits exactly 210 copper.");
                else if (tacticStory)
                {
                    var rewardId = tacticOffers[0].RewardId!;
                    var sourceId = StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId].SourceRewardId;
                    var hudTactic = model.ShanheJourneyTactics.Single(item => item.Id == sourceId);
                    var hud = (ShanheJourneyLoadout)panel.FindName("ShanhePlayerHudHost");
                    var icon = Program.Find<Image>(hud).Single(image => image.DataContext is ShanheJourneyTacticSlot
                        { Tactic: { } tactic } && tactic.Id == sourceId && ReferenceEquals(image.Source, tactic.Icon));
                    Program.Assert(session.Snapshot.TacticIds.SequenceEqual([rewardId]) && model.ShanheTacticCards.Single().Id == rewardId &&
                            session.Snapshot.Gold == 600 && session.Snapshot.ExtraSkillIds.Count == 0 &&
                            session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                            hudTactic.Icon is BitmapSource { PixelWidth: > 0 } &&
                            ReferenceEquals(hudTactic.Icon, ShanheArt.Get(sourceId)) &&
                            hud.Visibility == Visibility.Visible && icon.Visibility == Visibility.Visible &&
                            icon.ActualWidth > 0 && icon.ActualHeight > 0,
                        "Clicking the genuine rare tactic button grants its real owned identity once, uses no skill slot or copper, and visibly adds its original icon to the lower HUD.");
                    Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-story-rare-tactic-claimed-hud.png"));
                    var once = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                    model.ChooseShanheOptionCommand.Execute(choice);
                    Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == once,
                        "Executing the stale tactic choice cannot grant another copy, credit the decline reward, or advance the journal again.");
                }
                else
                    Program.Assert(model.IsShanheBattleActive && model.IsShanheBattleSession && session.PendingBattleSetup is not null,
                        "Clicking combat starts the native table immediately.");
                Program.Assert(!model.ChooseShanheOptionCommand.CanExecute(choice), "An old story button cannot pay or reward twice.");
            }
            finally { panel.DataContext = null; }
        }
    }

    public static void PendingChoicesRemainVisibleWithMarket()
    {
        foreach (var kind in new[] { ShanheNodeKind.Event, ShanheNodeKind.Camp, ShanheNodeKind.Shop })
        {
            var definition = new ShanheCampaignDefinition("fixture:node-choice-ui", "地点选择", ["classic:guan-yu"], ["stop"],
                [new("stop", "当前地点", kind, ["market"], Choices: [new("continue", "继续行军")]),
                 new("market", "集市", ShanheNodeKind.Shop, ["after"],
                     Choices: [new("heal", "补给", GoldCost: 10, RewardId: "supplies", LeavesNode: false)]),
                 new("after", "后续战斗", ShanheNodeKind.Battle, [], [new("after", "后续战斗", [new("classic:cao-cao")])])],
                [new("supplies", "补给", Heal: 1)], StartingGold: 50);
            var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu")
                { UseCodexRewardPool = true, PersistentMarket = kind != ShanheNodeKind.Shop },
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            Program.Assert(session.Submit(new MoveShanheNodeCommand("stop", session.Revision)).Accepted,
                "Enter the pending location through a real map command.");
            using var model = new MainViewModel(false, 71, true, new MemorySaveStore(),
                shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, session);
            typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(model, null);
            var panel = new ShanheCampaignPanel { DataContext = model };
            try
            {
                Layout(panel);
                var context = (Border)panel.FindName("ShanheContextPanel");
                Program.Assert(model.IsShanheShopNodeActive == (kind == ShanheNodeKind.Shop) &&
                    context.Visibility == (kind == ShanheNodeKind.Shop ? Visibility.Collapsed : Visibility.Visible),
                    "Only an actual shop node hides the context; a permanent market cannot hide event or camp choices.");
                if (kind == ShanheNodeKind.Shop) continue;
                Program.Assert(model.IsShanheShopActive && !model.IsShanheStoryEventOpen && context.ActualHeight > 0,
                    "Generic events and camps stay usable while permanent goods remain available.");
                var choice = model.ShanheChoices.Single();
                var button = Program.Find<Button>(context).Single(button => ReferenceEquals(button.CommandParameter, choice));
                Program.Assert(button.IsEnabled && button.ActualWidth > 0 && button.ActualHeight > 0,
                    "The pending action must have a visible enabled button with nonempty layout.");
                typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
                Layout(panel);
                Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                    session.Snapshot.AvailableNodes.Single().Id == "after" && context.Visibility == Visibility.Collapsed,
                    "Clicking the shown action completes the location and removes the context from the map.");
            }
            finally { panel.DataContext = null; }
        }
    }

    private static void Layout(FrameworkElement root)
    { root.Measure(new Size(1440, 880)); root.Arrange(new Rect(0, 0, 1440, 880)); root.UpdateLayout(); }
}

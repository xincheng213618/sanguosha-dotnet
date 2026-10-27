using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void ChapterRewardsShowSourceArtworkBeforeAndAfterClaiming(string output)
    {
        var catalog = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            foreach (var chapter in new[] { 1, 2 })
            {
                var original = catalog.Nodes.Single(node => node.Id == (chapter == 1 ? "huaxiong" : "guosi"));
                var groups = original.BossRewards!.Groups.Select(group => group with
                    { RewardIds = group.RewardIds.Take(3).ToArray() }).ToArray();
                var rewards = groups.SelectMany(group => group.RewardIds.Select((id, index) =>
                {
                    var reward = catalog.Rewards.Single(item => item.Id == id);
                    // Keep catalogue identity, quality and art; execute the small verified native fixture.
                    return reward.SkillId is not null
                        ? new ShanheRewardDefinition(id, reward.Name, SkillId: "fixture:shanhe-ui-reward-skill" + (index + 1))
                            { Rarity = 3, Description = reward.Description }
                        : reward.StartingCards is { Count: > 0 } ? reward
                        : new ShanheRewardDefinition(id, reward.Name)
                            { TacticSkillId = "fixture:shanhe-ui-tactic" + (index + 1), Rarity = 3, Description = reward.Description };
                })).ToArray();
                var definition = new ShanheCampaignDefinition("fixture:chapter-artwork-" + chapter, "章节奖励", [Player], ["boss"],
                    [new("boss", "章节首领", ShanheNodeKind.BossBattle, [], [new("guard", "守军", [new(Enemy)])])
                        { ChapterNumber = chapter, ChapterTitle = original.ChapterTitle, BossRewards = new(500, 1, groups) }], rewards);
                var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
                Accept(session, new MoveShanheNodeCommand("boss", session.Revision));
                WinPendingBattle(session);
                Attach(vm, session);
                Layout(panel);
                Program.Assert(vm.ShanheBossRewardGroups.Select(group => group.Id).SequenceEqual(chapter == 1
                        ? new[] { "tactics", "cards" } : ["tactics", "skills"]),
                    "The real catalogue supplies epic cards in chapter one and epic skills in chapter two.");
                foreach (var originalGroup in groups)
                {
                    var group = vm.ShanheBossRewardGroups.Single(item => item.Id == originalGroup.Id);
                    var isSkill = group.Id == "skills";
                    Program.Assert(group.Artworks.Count == 3 && group.Artworks.All(art => isSkill
                            ? !art.HasImage && art.Glyph.Length > 0 : art.Image is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 }),
                        "Unclaimed groups preview their real tactic icons and physical card faces; skills retain a named badge when no source art exists.");
                    AssertOverviewArtwork(group);
                    Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand, group), panel);
                    Program.Assert(vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(card =>
                            card.Rarity == 3 && (isSkill || card.Artwork is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 }) &&
                            (group.Id == "cards" ? card.Summary == "获得卡牌" : card.Summary != "获得卡牌")),
                        "Epic selectors display the source artwork without replacing tactic effect text with a physical-card summary.");
                    if (!isSkill)
                        foreach (var candidate in vm.ShanheRewardCards)
                            Program.Assert(Program.Find<Image>(Bound(panel, vm.SelectShanheRewardCardCommand, candidate)).Any(image =>
                                    image.Name == "ShanheRewardArtwork" && ReferenceEquals(image.Source, candidate.Artwork) &&
                                    image.Visibility == Visibility.Visible && image.ActualWidth > 0 && image.ActualHeight > 0),
                                "The actual candidate template must bind and lay out each decoded reward image.");
                    var chosen = vm.ShanheRewardCards[0];
                    Click(Bound(panel, vm.SelectShanheRewardCardCommand, chosen), panel);
                    Click((Button)panel.FindName("ShanheConfirmRewardButton"), panel);
                    var resolved = vm.ShanheBossRewardGroups.Single(item => item.Id == originalGroup.Id);
                    Program.Assert(resolved.IsResolved && resolved.Artworks.Count == 1 &&
                            (isSkill ? session.Snapshot.ExtraSkillIds.Count == 1 : ReferenceEquals(resolved.Artworks[0].Image, chosen.Artwork)) &&
                            (group.Id != "cards" || session.Snapshot.StartingCards.Count == 1),
                        "Claimed groups retain exactly the selected reward's artwork and grant its actual skill or physical card.");
                    AssertOverviewArtwork(resolved);
                }
                Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-chapter-" + chapter + "-reward-artwork.png"));
            }
        }
        finally { panel.DataContext = null; }

        void AssertOverviewArtwork(ShanheBossRewardGroup group)
        {
            var button = Bound(panel, vm.OpenShanheBossRewardGroupCommand, group);
            foreach (var art in group.Artworks.Where(art => art.HasImage))
                Program.Assert(Program.Find<Image>(button).Any(image => image.Name == "ShanheBossRewardArtwork" &&
                        ReferenceEquals(image.Source, art.Image) && image.Visibility == Visibility.Visible &&
                        image.ActualWidth > 0 && image.ActualHeight > 0),
                    "Both unclaimed and claimed overview groups must render their decoded source images instead of a generic choice placeholder.");
        }
    }
}

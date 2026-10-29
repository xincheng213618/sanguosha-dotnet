using CardGame.Core.Campaigns;
using CardGame.Core;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Resources;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

internal static partial class ShanheRewardUiChecks
{
    public static void ChapterRouteStagesExtraBattlesAndStatusRemainAccurate(string output)
    {
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
            shanheCodexStore: new MemoryShanheCodexStore());
        var definition = ChapterRouteMapFixture();
        foreach (var extra in new[] { 0, 1, 2 })
        {
            var session = ShanheCampaignSession.Create(definition, new(71, Player)
            { EquippedTacticIds = extra == 0 ? [] : ["march" + extra] }, _ => FixtureRegistry.Value);
            var before = Snapshot(session);
            Attach(vm, session);
            AssertChapterBossArtwork(0);
            AssertChapterRouteFlags(0);
            Program.Assert(vm.ShanheChapterStepCount == 6 && vm.ShanheChapterMainProgress == 0 &&
                    vm.ShanheChapterProgressSlots.Count == 6 && vm.ShanheMapNodes.Count(node => node.IsAvailable) == 2 &&
                    vm.ShanheMapNodes.Where(node => node.IsAvailable).Select(node => node.SceneLandmark).ToHashSet().SetEquals(["Cave", "Platform"]) &&
                    vm.ShanheMapNodes.Count(node => node.IsExtraBattle) == extra && vm.ShanheMapScenery.Count == 2 - extra &&
                    vm.ShanheMapLinks.Count == 0 && Snapshot(session) == before,
                "A chapter begins at two geographic battle landmarks with six mainline slots; optional pursuits never enlarge the mainline target or write a command.");
            var markers = vm.ShanheMapNodes.ToArray();
            Program.Assert(markers.All(node => node.X >= 0 && node.Y >= 65 && node.X + 136 <= 1000 && node.Y + 132 <= 560 &&
                        node.EnemyText.Length == 0 && !node.DetailText.Contains("秘密介绍", StringComparison.Ordinal)) &&
                    markers.SelectMany((node, index) => markers.Skip(index + 1).Select(other =>
                        node.X + node.HitWidth <= other.X || other.X + other.HitWidth <= node.X ||
                        node.Y + node.HitHeight <= other.Y || other.Y + other.HitHeight <= node.Y)).All(separate => separate),
                "The map keeps geographic markers separate, leaves enemy introductions inside the selected preview and hides future location details.");
        }
        var progress = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
        Win("opening-2");
        Attach(vm, progress);
        AssertChapterBossArtwork(1);
        AssertChapterRouteFlags(1);
        Program.Assert(progress.ChapterProgress == 1, "The first opening victory advances only one mainline step.");
        foreach (var story in new[] { "story-1", "story-2", "story-3" })
        {
            Accept(progress, new MoveShanheNodeCommand(story, progress.Revision));
            Accept(progress, new ChooseShanheNodeOptionCommand("continue", null, progress.Revision));
            Attach(vm, progress);
            AssertChapterBossArtwork(progress.ChapterProgress);
            AssertChapterRouteFlags(progress.ChapterProgress);
        }
        Attach(vm, progress);
        Program.Assert(vm.ShanheJourneyHandCards.Single() is { Kind: CardKind.Slash, SuitGlyph: "♥", Rank: "7" } &&
                vm.ShanheJourneySkillSlots.Count(slot => slot.IsPrinted && !slot.IsEmpty) == 1 &&
                vm.ShanheJourneySkillSlots.Count(slot => slot.IsEmpty) == progress.Snapshot.ExtraSkillSlots,
            "The journey loadout retains the real carried card face, suit and rank and separates printed skills from empty learned-skill slots.");
        Program.Assert(vm.ShanheChapterMainProgress == 4 && vm.ShanheMapNodes.Single(node => node.Node.Id == "opening-1").IsAvailable &&
                vm.ShanheMapNodes.Single(node => node.Node.Id == "main").SceneLandmark == "Platform" &&
                vm.ShanheMapNodes.Single(node => node.Node.Id == "main").IsAvailable &&
                vm.ShanheMapNodes.All(node => node.Node.Id != "opening-2") &&
                vm.ShanheMapNodes.Count(node => node.IsPlayerLocation) == 1 &&
                vm.ShanheMapNodes.Single(node => node.IsPlayerLocation).Node.Id == "story-3",
            "The main battle reuses the completed opening landmark while preserving the last visited story location's single player pin and the other real optional opening.");
        Win("main");
        Attach(vm, progress);
        AssertChapterBossArtwork(5);
        AssertChapterRouteFlags(5);
        var rest = vm.ShanheMapNodes.Single(node => node.Node.Id == "rest");
        Program.Assert(rest is { IsAvailable: true, SceneLandmark: "Bridge" } && vm.ShanheChapterMainProgress == 5 &&
                vm.ShanheMapNodes.Count(node => node.IsPlayerLocation) == 1 &&
                vm.ShanheMapNodes.Single(node => node.IsPlayerLocation) is { SceneLandmark: "Platform", Node.Id: "main" },
            "Rest reuses the completed bridge encounter after five steps while the player pin stays at the main battle just visited rather than jumping to the available rest.");
        vm.PreviewShanheNodeCommand.Execute(rest.Node);
        Program.Assert(progress.Snapshot.Phase == ShanheCampaignPhase.NodeChoice && vm.ShanheChoices.Count > 0 &&
                !vm.IsShanheEncounterPreviewOpen && vm.ShanheChapterMainProgress == 5,
            "Selecting rest reveals its existing choices and waits for their completion before advancing the mainline.");
        vm.ChooseShanheOptionCommand.Execute(vm.ShanheChoices.Single(choice => choice.Id == "continue"));
        AssertChapterBossArtwork(6);
        AssertChapterRouteFlags(6);
        Program.Assert(vm.ShanheChapterMainProgress == 6 && vm.ShanheChapterProgressSlots.All(slot => slot.IsCompleted) &&
                vm.ShanheMapNodes.Single(node => node.IsBoss).IsAvailable &&
                vm.ShanheMapNodes.Single(node => node.Node.Id == "optional").IsAvailable &&
                vm.ShanheMapNodes.Single(node => node.Node.Id == "opening-1").IsAvailable &&
                vm.ShanheMapNodes.Single(node => node.Node.Id == "optional").SceneLandmark == "Cave" &&
                vm.ShanheMapNodes.Count(node => node.IsPlayerLocation) == 1 &&
                vm.ShanheMapNodes.Single(node => node.IsPlayerLocation) is { SceneLandmark: "Bridge", Node.Id: "rest" },
            "Six completed mainline choices open the boss and both skippable battles while preserving the single player pin at the rest landmark just visited.");
        Program.Assert(vm.ShanheChapterObjectiveText == "主线已满，可挑战首领；剩余2场可选战斗、1场擂台",
            "The full six-slot route names only the two still-available ordinary battles and arena, without inventing an unequipped pursuit.");
        Win("opening-1");
        Attach(vm, progress);
        Program.Assert(vm.ShanheChapterMainProgress == 6 && vm.ShanheMapNodes.Single(node => node.IsBoss).IsAvailable,
            "Completing a retained optional opening keeps the six-slot progress full and the boss selectable.");
        Program.Assert(vm.ShanheChapterObjectiveText == "主线已满，可挑战首领；剩余1场可选战斗、1场擂台",
            "Completing the actual unchosen opening removes it from the remaining optional-battle count.");
        Win("optional"); Attach(vm, progress);
        Program.Assert(vm.ShanheChapterObjectiveText == "主线已满，可挑战首领；剩余1场擂台",
            "After both ordinary optional battles, the objective retains only the actual available arena.");
        Win("arena"); Attach(vm, progress);
        Program.Assert(progress.Snapshot.AvailableNodes is [{ ChapterRole: ShanheChapterNodeRole.Boss }] &&
                vm.ShanheChapterMainProgress == 6 && vm.ShanheChapterObjectiveText == "主线已满，可直接挑战首领",
            "When only the real boss remains available, the map stops suggesting already-completed optional battles or an arena.");
        foreach (var region in new[] { (Id: 3, Name: "函谷关", Chapter: 2), (Id: 10, Name: "洛阳", Chapter: 2), (Id: 14, Name: "崤函道", Chapter: 3) })
        {
            var geography = definition with
            {
                Nodes = definition.Nodes.Select(node => node with
                {
                    ChapterRouteId = "source-region-" + region.Id, ChapterNumber = region.Chapter,
                    ChapterTitle = $"第{region.Chapter}章 · {region.Name}"
                }).ToArray()
            };
            progress = ShanheCampaignSession.Create(geography, new(71, Player) { EquippedTacticIds = ["march2"] }, _ => FixtureRegistry.Value);
            Attach(vm, progress);
            using var chapterHeaderHost = region.Id == 3 ? new NativeResponseStatusChecks.NativeHost(vm) : null;
            if (chapterHeaderHost is not null)
                VerifyNativeChapterHeader(chapterHeaderHost, 0, "shanhe-native-chapter-header-empty");
            var map = vm.ShanheMapNodes.ToArray();
            Program.Assert(map.All(node => !node.ShortName.Any(char.IsDigit)) &&
                    (region.Id == 10 || map.SelectMany((node, index) => map.Skip(index + 1).Select(other =>
                        node.X + node.HitWidth <= other.X || other.X + other.HitWidth <= node.X ||
                        node.Y + node.HitHeight <= other.Y || other.Y + other.HitHeight <= node.Y)).All(separate => separate)),
                "Screenshot-backed regions use geographic names and keep the separated reference landmarks apart; Luoyang's transparent sprite overlap is checked in the real window below.");
            if (region.Id == 14)
                Program.Assert(map.Single(node => node.Node.Id == "opening-1") is { ShortName: "蒲坂", NativeLandmark.CityId: 5141 } &&
                        map.Single(node => node.Node.Id == "opening-2").ShortName == "河北" &&
                        map.Single(node => node.Node.Id == "story-1").ShortName == "曹阳" &&
                        map.Single(node => node.Node.Id == "story-2").ShortName == "陕津" &&
                        map.Single(node => node.Node.Id == "extra-1").ShortName == "湖县" &&
                        map.Single(node => node.Node.Id == "extra-2").ShortName == "弘农" &&
                        map.Single(node => node.IsBoss) is { ShortName: "潼关", NativeLandmark.CityId: 5148 } &&
                        Math.Abs(map.Single(node => node.IsBoss).X - 50d * 1000 / 1220) < .01 &&
                        Math.Abs(map.Single(node => node.IsBoss).Y - 373d * 1000 / 1220) < .01,
                    "The supplied Xiaohan map retains its named towns and Tongguan's actual southwest position.");
            else if (region.Id == 3)
                Program.Assert(map.Single(node => node.Node.Id == "story-2").ShortName == "中条山" &&
                        map.Single(node => node.Node.Id == "extra-2").ShortName == "湖县" && map.Single(node => node.IsBoss).ShortName == "函谷关",
                    "The supplied Hangu map uses Zhongtiao Mountain and Hu County without copying those names into other regions.");
            Win(region.Id == 10 ? "opening-2" : "opening-1");
            Attach(vm, progress);
            AssertChapterRouteFlags(1);
            if (chapterHeaderHost is not null)
            {
                chapterHeaderHost.Capture(output, "shanhe-native-route-flags-after-opening", 1120);
                VerifyNativeRouteFlags(chapterHeaderHost, 1);
            }
            foreach (var id in new[] { "story-1", "story-2" })
            {
                Accept(progress, new MoveShanheNodeCommand(id, progress.Revision));
                Accept(progress, new ChooseShanheNodeOptionCommand("continue", null, progress.Revision));
            }
            Attach(vm, progress);
            var panel = new ShanheCampaignPanel { DataContext = vm };
            try
            {
                Program.Render(panel, 1120, 780, System.IO.Path.Combine(output, $"shanhe-native-region-{region.Id}-market.png"));
                var journey = Program.Find<ShanheJourneyMap>(panel).Single();
                var market = (Button)journey.FindName("ShanheMapMarketButton");
                Program.Assert(vm.ShanheChapterMainProgress == 3 && market.Visibility == Visibility.Visible && market.IsEnabled &&
                        ReferenceEquals(market.Command, vm.OpenShanheMarketCommand),
                    "The original source market icon becomes usable at the real third mainline step.");
                if (chapterHeaderHost is not null)
                    VerifyNativeChapterHeader(chapterHeaderHost, 3, "shanhe-native-chapter-header-half");
                var revision = progress.Revision;
                market.Command.Execute(null);
                Program.Assert(vm.IsShanheMarketOpen && progress.Revision == revision && progress.ChapterProgress == 3,
                    "The native map shop opens the existing market without spending a mainline action.");
                vm.CloseShanheMarketCommand.Execute(null);
                Accept(progress, new MoveShanheNodeCommand("story-3", progress.Revision));
                Accept(progress, new ChooseShanheNodeOptionCommand("continue", null, progress.Revision));
                Win("main");
                Attach(vm, progress);
                Program.Render(panel, 1120, 780, System.IO.Path.Combine(output, $"shanhe-native-region-{region.Id}-camp.png"));
                Program.Assert(Program.Find<Image>(journey).Count(image => image.Name == "CampMarker" && image.Visibility == Visibility.Visible) == 1,
                    "At step five the actual rest landmark shows the original camp marker.");
                AssertChapterRouteFlags(5);
                vm.PreviewShanheNodeCommand.Execute(vm.ShanheMapNodes.Single(node => node.Node.Id == "rest").Node);
                vm.ChooseShanheOptionCommand.Execute(vm.ShanheChoices.Single(choice => choice.Id == "continue"));
                Program.Render(panel, 1120, 780, System.IO.Path.Combine(output, $"shanhe-native-region-{region.Id}-boss.png"));
                var bossButton = Program.Find<Button>(journey).Single(button => button.DataContext is ShanheJourneyNode { IsBoss: true });
                Program.Assert(vm.ShanheChapterMainProgress == 6 && ((Image)panel.FindName("ShanheChapterBossEmblem")).Opacity == 1 &&
                        Program.Find<Image>(bossButton).Single(image => image.Name == "BossMask").Opacity == 1,
                    "Finishing native rest lights both the header emblem and the real map boss while optional battles remain available.");
                if (region.Id == 3)
                {
                    var host = chapterHeaderHost!;
                    vm.OpenShanheCommand.Execute(null);
                    VerifyNativeChapterHeader(host, 6, "shanhe-native-hangu-empty-companion-hit");
                    var nativePanel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
                    var nativeMap = Program.Find<ShanheJourneyMap>(nativePanel).Single(control => control.IsVisible);
                    var loadout = Program.Find<ShanheJourneyLoadout>(nativePanel).Single(control => control.IsVisible);
                    var companions = (ScrollViewer)loadout.FindName("ShanheJourneyCompanionScroll");
                    var city = Program.Find<ShanheMapNodeButton>(nativeMap).Single(button =>
                        button.DataContext is ShanheJourneyNode { IsAvailable: true, NativeLandmark.CityId: 5037 });
                    // The lower cave pixel (60,70) is fully opaque. Pixel (40,70)
                    // lies in this original PNG's transparent padding and must not hit it.
                    var cityNode = (ShanheJourneyNode)city.DataContext;
                    var landmark = cityNode.NativeLandmark!;
                    var sourcePoint = new Point(landmark.X + 60, landmark.Y + 70);
                    var sourcePixel = new byte[4];
                    var pixels = new System.Windows.Media.Imaging.FormatConvertedBitmap(
                        (System.Windows.Media.Imaging.BitmapSource)cityNode.NativeArtwork!,
                        System.Windows.Media.PixelFormats.Bgra32, null, 0);
                    pixels.CopyPixels(new Int32Rect(60, 70, 1, 1), sourcePixel, 4, 0);
                    var mapPoint = new Point(sourcePoint.X * 1000d / 1220, sourcePoint.Y * 1000d / 1220);
                    var targetPoint = nativeMap.TranslatePoint(mapPoint, host.Root);
                    var formerCompanionCover = new Rect(companions.TranslatePoint(new Point(), host.Root), new Size(210, 73));
                    var beforeHits = Snapshot(progress);
                    var hit = host.Root.InputHitTest(targetPoint) as DependencyObject;
                    Program.Assert(vm.ShanheStartingCompanionCards.Count == 0 && vm.ShanheRecruitedCompanionCards.Count == 0 &&
                            companions.ActualWidth == 0 && companions.ActualHeight == 0 && sourcePixel[3] == 255 &&
                            formerCompanionCover.Contains(targetPoint) &&
                            hit is not null && (ReferenceEquals(hit, city) || city.IsAncestorOf(hit)) &&
                            city.Command!.CanExecute(city.CommandParameter) && Snapshot(progress) == beforeHits && !vm.IsShanheEncounterPreviewOpen,
                        $"With no companions, a real opaque lower Hangu cave pixel inside the former invisible HUD rectangle must reach its own available city preview without submitting a command. " +
                        $"root={host.Root.RenderSize}, panel={nativePanel.RenderSize}, map={nativeMap.RenderSize}, " +
                        $"source={sourcePoint}, target={targetPoint}, formerCover={formerCompanionCover}, inside={formerCompanionCover.Contains(targetPoint)}, " +
                        $"alpha={sourcePixel[3]}, companions={companions.RenderSize}, starting={vm.ShanheStartingCompanionCards.Count}, recruited={vm.ShanheRecruitedCompanionCards.Count}, " +
                        $"hit={hit?.GetType().Name}/{(hit as FrameworkElement)?.Name}, ownsHit={hit is not null && (ReferenceEquals(hit, city) || city.IsAncestorOf(hit))}, " +
                        $"canPreview={city.Command!.CanExecute(city.CommandParameter)}, unchanged={Snapshot(progress) == beforeHits}, previewOpen={vm.IsShanheEncounterPreviewOpen}.");
                }
                if (region.Id == 10)
                {
                    using var host = new NativeResponseStatusChecks.NativeHost(vm);
                    vm.OpenShanheCommand.Execute(null);
                    host.Capture(output, "shanhe-native-region-10-hit-targets", 1120);
                    var nativePanel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
                    var nativeMap = Program.Find<ShanheJourneyMap>(nativePanel).Single(mapControl => mapControl.IsVisible);
                    Program.Assert(nativePanel.IsVisible && nativeMap.IsDescendantOf(host.Root),
                        "The native hit-test regression must target the visible expedition map inside the real main-window Shanhe panel.");
                    var first = Program.Find<ShanheMapNodeButton>(nativeMap).Single(button =>
                        button.DataContext is ShanheJourneyNode { Node.Id: "opening-1" });
                    var extra = Program.Find<ShanheMapNodeButton>(nativeMap).Single(button =>
                        button.DataContext is ShanheJourneyNode { Node.Id: "extra-2" });
                    var firstNode = (ShanheJourneyNode)first.DataContext;
                    var extraNode = (ShanheJourneyNode)extra.DataContext;
                    Program.Assert(firstNode is { IsAvailable: true, NativeLandmark.CityId: 5101 } &&
                            extraNode is { IsAvailable: true, NativeLandmark.CityId: 5107 } &&
                            new Rect(firstNode.X, firstNode.Y, firstNode.HitWidth, firstNode.HitHeight).IntersectsWith(
                                new Rect(extraNode.X, extraNode.Y, extraNode.HitWidth, extraNode.HitHeight)),
                        "After choosing Luoyang's second opening and completing six steps, the retained first opening and daily pursuit must both be actionable despite their original sprite rectangles overlapping.");
                    var beforeHits = Snapshot(progress);
                    var beforeRevision = progress.Revision;
                    var blank = Hit(new Point(253, 311));
                    Program.Assert(!BelongsTo(blank, first) && !BelongsTo(blank, extra),
                        $"The transparent overlap between native cities 5101 and 5107 must not hit either node; actual target={blank?.GetType().Name}.");
                    AssertNativeHit(first, firstNode, new Point(181, 255));
                    AssertNativeHit(extra, extraNode, new Point(349, 373));
                    Program.Assert(progress.Revision == beforeRevision && Snapshot(progress) == beforeHits &&
                            progress.PendingBattleSetup is null && !vm.IsShanheEncounterPreviewOpen,
                        "Native landmark hit testing must preserve the campaign revision, command journal and battle boundary.");

                    DependencyObject? Hit(Point world) => host.Root.InputHitTest(nativeMap.TranslatePoint(
                        new Point(world.X * 1000d / 1220, world.Y * 1000d / 1220), host.Root)) as DependencyObject;
                    static bool BelongsTo(DependencyObject? hit, Button button) => hit is not null &&
                        (ReferenceEquals(hit, button) || button.IsAncestorOf(hit));
                    void AssertNativeHit(ShanheMapNodeButton button, ShanheJourneyNode node, Point world)
                    {
                        var hit = Hit(world);
                        Program.Assert(button.IsVisible && button.IsEnabled && BelongsTo(hit, button) &&
                                ReferenceEquals(button.Command, vm.PreviewShanheNodeCommand) &&
                                ReferenceEquals(button.CommandParameter, node.Node) &&
                                ReferenceEquals(node.Node, vm.ShanheMapNodes.Single(item => item.Node.Id == node.Node.Id).Node) &&
                                button.Command.CanExecute(button.CommandParameter),
                            $"The opaque native sprite at {world} must hit its own original available preview button ({node.Node.Id}); actual target={hit?.GetType().Name}.");
                    }
                }
            }
            finally { panel.DataContext = null; }
            if (region.Id == 3)
            {
                var host = chapterHeaderHost!;
                AssertRemainingObjective(host, "主线已满，可挑战首领；剩余2场可选战斗、1场擂台、2场追击", "all");
                Win("opening-2"); Attach(vm, progress);
                AssertRemainingObjective(host, "主线已满，可挑战首领；剩余1场可选战斗、1场擂台、2场追击", "one-optional");
                Win("optional"); Attach(vm, progress);
                AssertRemainingObjective(host, "主线已满，可挑战首领；剩余1场擂台、2场追击", "arena-pursuits");
                Win("arena"); Attach(vm, progress);
                AssertRemainingObjective(host, "主线已满，可挑战首领；剩余2场追击", "pursuits");
                Win("extra-1"); Attach(vm, progress);
                AssertRemainingObjective(host, "主线已满，可挑战首领；剩余1场追击", "last-pursuit");
                Win("extra-2"); Attach(vm, progress);
                Program.Assert(progress.Snapshot.AvailableNodes is [{ ChapterRole: ShanheChapterNodeRole.Boss }] &&
                        vm.ShanheChapterMainProgress == 6,
                    "Completing both daily-travel pursuits leaves the same full six-slot progress and exactly one available boss.");
                AssertRemainingObjective(host, "主线已满，可直接挑战首领", "boss-only");
            }
        }

        // Reuse the retained non-route fixture: a chapter title alone does not
        // establish the six-step contract used by Source2026 routes.
        var legacy = CreateThreeBossSession();
        Attach(vm, legacy);
        var legacyBefore = Snapshot(legacy);
        var legacyPanel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            Layout(legacyPanel);
            Program.Assert(vm.ShanheChapterProgressTarget == 0 && vm.ShanheChapterProgressSlots.Count == 0 &&
                    ((ShanheChapterProgressStrip)legacyPanel.FindName("ShanheChapterProgressMeter")).Visibility == Visibility.Collapsed &&
                    ((Image)legacyPanel.FindName("ShanheChapterBossEmblem")).Visibility == Visibility.Collapsed &&
                    ((TextBlock)legacyPanel.FindName("ShanheChapterProgressText")).Text == vm.ShanheChapterProgressText &&
                    vm.ShanheChapterProgressText.Contains("程", StringComparison.Ordinal) && Snapshot(legacy) == legacyBefore,
                "A retained chapter without route metadata keeps its actual x/y journey count and hides the six-step meter and readiness emblem without changing the campaign.");
        }
        finally { legacyPanel.DataContext = null; }

        void AssertRemainingObjective(NativeResponseStatusChecks.NativeHost host, string expected, string stage)
        {
            var before = Snapshot(progress);
            var game = Program.Engine(vm);
            var ordinaryBefore = GameCheckpointJson.Serialize(game.CreateCheckpoint());
            foreach (var width in new[] { 1120, 1440 })
            {
                host.Capture(output, "shanhe-chapter-remaining-" + stage, width);
                var panel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
                var banner = (Border)panel.FindName("ShanheChapterObjectiveBanner");
                var text = (TextBlock)panel.FindName("ShanheChapterObjective");
                Program.Assert(vm.ShanheChapterMainProgress == 6 && vm.ShanheChapterObjectiveText == expected &&
                        banner.IsVisible && text.IsVisible && text.Text == expected && text.FontSize >= 14 &&
                        new Rect(host.Root.RenderSize).Contains(banner.TransformToAncestor(host.Root).TransformBounds(new Rect(banner.RenderSize))),
                    "The actual native map objective follows the current remaining battles, arena and pursuits at either viewport width: " + stage);
                ShanheResponsiveMarketUiChecks.AssertTextGlyphsWithin(text, banner,
                    "The complete remaining-chapter objective", checkAncestorClips: true);
            }
            Program.Assert(Snapshot(progress) == before && ReferenceEquals(Program.Engine(vm), game) &&
                    GameCheckpointJson.Serialize(game.CreateCheckpoint()) == ordinaryBefore && !vm.IsShanheEncounterPreviewOpen,
                "Reading and resizing the remaining-challenge objective cannot start another encounter or change either checkpoint.");
        }

        void VerifyNativeChapterHeader(NativeResponseStatusChecks.NativeHost host, int completed, string label)
        {
            var before = Snapshot(progress);
            var ordinaryRevision = Program.Engine(vm).Revision;
            // Finish at the compact size used by the existing opaque-city hit test.
            foreach (var width in new[] { 1440, 1120 })
            {
                host.Capture(output, label, width);
                var panel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
                var header = (FrameworkElement)panel.FindName("ShanheChapterHeader");
                var heading = (TextBlock)panel.FindName("ShanheChapterHeadingText");
                var caption = (TextBlock)panel.FindName("ShanheChapterProgressText");
                var meter = (ShanheChapterProgressStrip)panel.FindName("ShanheChapterProgressMeter");
                var emblem = (Image)panel.FindName("ShanheChapterBossEmblem");
                var track = (Image)meter.FindName("ProgressTrack");
                var fill = (Image)meter.FindName("ProgressFill");
                var clip = (RectangleGeometry)meter.FindName("FillClip");
                var frontier = (Border)meter.FindName("Frontier");
                var dividers = ((Canvas)meter.FindName("ProgressDividers")).Children.OfType<Image>().ToArray();
                Program.Assert(vm.ShanheChapterProgressTarget == 6 && vm.ShanheChapterMainProgress == completed &&
                        meter.IsVisible && emblem.IsVisible && meter.Value == completed &&
                        !meter.IsHitTestVisible && !meter.Focusable && !emblem.IsHitTestVisible && emblem.Opacity == 1 &&
                        emblem.Stretch == Stretch.Uniform && ReferenceEquals(emblem.Source, vm.ShanheChapterBossArtwork) &&
                        heading.Text == vm.ShanheChapterTitle && caption.Text == vm.ShanheChapterProgressText &&
                        AutomationProperties.GetHelpText(meter) == vm.ShanheChapterProgressText &&
                        clip.Rect == new Rect(0, 0, 516d * completed / 6, 38) && ReferenceEquals(fill.Clip, clip) &&
                        dividers.Length == 5 && dividers.Select((divider, index) =>
                            Canvas.GetLeft(divider) == 86 * (index + 1) - 2 && divider.IsVisible).All(value => value) &&
                        frontier.Visibility == (completed is > 0 and < 6 ? Visibility.Visible : Visibility.Collapsed) &&
                        (frontier.Visibility != Visibility.Visible || Canvas.GetLeft(frontier) == 86 * completed - 1),
                    $"The real {width}-wide chapter header projects {completed}/6 onto the original six-part strip, with an opaque alpha-preserving boss emblem and only an intermediate frontier highlight.");
                AssertOriginalMapImage(track, "rogue_day_pro_bg.png");
                AssertOriginalMapImage(fill, "rogue_day_pro_bar.png");
                foreach (var divider in dividers) AssertOriginalMapImage(divider, "rogue_day_split.png");
                AssertChapterBossArtwork(completed);

                var headerBounds = Bounds(header); headerBounds.Inflate(.5, .5);
                var children = new FrameworkElement[] { heading, caption, meter, emblem };
                var childBounds = children.Select(Bounds).ToArray();
                var actions = new[] { "ShanheRulesButton", "ShanheOpenMarketButton", "ShanheCloseButton", "ShanheEndButton" }
                    .Select(name => (Button)panel.FindName(name)).Where(button => button.IsVisible).ToArray();
                var actionBounds = actions.Select(Bounds).ToArray();
                Program.Assert(new Rect(host.Root.RenderSize).Contains(Bounds(header)) &&
                        childBounds.All(rect => rect.Width > 0 && rect.Height > 0 && headerBounds.Contains(rect)) &&
                        childBounds.SelectMany((rect, index) => childBounds.Skip(index + 1).Select(other => !rect.IntersectsWith(other))).All(separate => separate) &&
                        actions.Length == 4 && actionBounds.All(rect => new Rect(host.Root.RenderSize).Contains(rect) && !rect.IntersectsWith(Bounds(header))) &&
                        actionBounds.SelectMany((rect, index) => actionBounds.Skip(index + 1).Select(other => !rect.IntersectsWith(other))).All(separate => separate),
                    $"At {width} pixels the full chapter title, real progress caption, meter and boss remain inside their header, apart from one another and all four top actions. Header={Bounds(header)}; children={string.Join("; ", childBounds)}; actions={string.Join("; ", actionBounds)}.");
                foreach (var action in actions.Where(button => button.IsEnabled))
                {
                    var hit = host.Root.InputHitTest(action.TranslatePoint(new Point(action.ActualWidth / 2, action.ActualHeight / 2), host.Root)) as DependencyObject;
                    Program.Assert(hit is not null && (ReferenceEquals(hit, action) || action.IsAncestorOf(hit)),
                        $"The chapter art must leave the enabled {action.Name} reachable in the actual {width}-wide native window.");
                }
                VerifyNativeRouteFlags(host, completed);
                Rect Bounds(FrameworkElement element) => element.TransformToAncestor(host.Root).TransformBounds(new Rect(element.RenderSize));
            }
            Program.Assert(Snapshot(progress) == before && Program.Engine(vm).Revision == ordinaryRevision && !vm.IsShanheEncounterPreviewOpen,
                "Projecting, resizing and hit testing chapter progress never advances a node, changes resources or touches the ordinary game.");
        }

        void AssertChapterRouteFlags(int completed)
        {
            Program.Assert(vm.ShanheChapterMainProgress == completed && vm.ShanheMapNodes.All(node =>
                    node.RouteFlagText == ExpectedRouteFlag(node, completed) &&
                    node.HasRouteFlag == (ExpectedRouteFlag(node, completed).Length > 0) &&
                    node.EnemyText.Length == 0 && (!node.IsAvailable ? node.RouteSourceText.Length == 0 : true)),
                "Route badges classify only real available destinations: either opening advances the first mainline step, the unchosen opening then becomes optional, and locked or completed locations expose no enemy or route action.");
            if (completed == 0)
                Program.Assert(vm.ShanheMapNodes.Count(node => node is { HasRouteFlag: true, RouteFlagText: "主线" }) == 2 &&
                        vm.ShanheMapNodes.Where(node => node.IsExtraBattle).All(node => !node.HasRouteFlag && !node.IsAvailable),
                    "Both opening choices are mainline at zero progress; daily travel I or II adds owned but still locked pursuit locations.");
            if (completed == 1)
                Program.Assert(vm.ShanheMapNodes.Count(node => node is { HasRouteFlag: true, RouteFlagText: "主线" }) == 1 &&
                        vm.ShanheMapNodes.Count(node => node is { HasRouteFlag: true, RouteFlagText: "可选", IsBattle: true }) == 1,
                    "After one opening victory the next encounter is mainline and exactly the retained opening is an optional battle.");
            if (completed == 6)
                Program.Assert(vm.ShanheMapNodes.Where(node => node.IsExtraBattle).All(node => node is { IsAvailable: true, HasRouteFlag: true, RouteFlagText: "追击" }) &&
                        vm.ShanheMapNodes.All(node => node.RouteFlagText != "主线") &&
                        vm.ShanheMapNodes.Single(node => node.IsBoss) is { IsAvailable: true, HasRouteFlag: false },
                    "At six mainline steps owned pursuits are actionable, optional fights remain optional, and the unlocked boss keeps its own emblem instead of a mainline badge.");
        }

        string ExpectedRouteFlag(ShanheJourneyNode node, int completed)
        {
            if (!node.IsAvailable || node.IsCompleted || node.IsBoss) return string.Empty;
            var role = definition.Nodes.Single(item => item.Id == node.Node.Id).ChapterRole;
            return role switch
            {
                ShanheChapterNodeRole.ExtraBattle => "追击",
                ShanheChapterNodeRole.OpeningBattle => completed == 0 ? "主线" : "可选",
                ShanheChapterNodeRole.Arena or ShanheChapterNodeRole.OptionalBattle => "可选",
                ShanheChapterNodeRole.Event or ShanheChapterNodeRole.Rest or ShanheChapterNodeRole.MainBattle => "主线",
                _ => string.Empty
            };
        }

        void VerifyNativeRouteFlags(NativeResponseStatusChecks.NativeHost host, int completed)
        {
            var before = ShanheCampaignJson.Serialize(progress.CreateCheckpoint());
            var ordinaryRevision = Program.Engine(vm).Revision;
            AssertChapterRouteFlags(completed);
            var panel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
            var map = Program.Find<ShanheJourneyMap>(panel).Single(control => control.IsVisible);
            var buttons = Program.Find<ShanheMapNodeButton>(map).Where(button => button.DataContext is ShanheJourneyNode).ToArray();
            Program.Assert(buttons.Length == vm.ShanheMapNodes.Count,
                "Native route labels must be verified on every realized original map destination.");
            if (completed == 6)
                Program.Assert(vm.ShanheMapNodes.Count(node => node.RouteFlagText == "追击") == 2 &&
                        vm.ShanheMapNodes.Count(node => node.RouteFlagText == "可选") == 3,
                    "The existing daily-travel-II native fixture retains both unlocked pursuits alongside the unchosen opening, optional arena and final optional battle.");
            foreach (var button in buttons)
            {
                var node = (ShanheJourneyNode)button.DataContext;
                var flag = (Border)button.Template.FindName("RouteFlag", button);
                var expected = ExpectedRouteFlag(node, completed);
                Program.Assert(flag.Visibility == (expected.Length == 0 ? Visibility.Collapsed : Visibility.Visible) &&
                        flag.Child is TextBlock text && text.Text == expected,
                    $"The actual route label for {node.Node.Id} must show only its current {completed}/6 classification.");
                if (expected.Length == 0) continue;
                var point = flag.TranslatePoint(new Point(flag.ActualWidth / 2, flag.ActualHeight / 2), host.Root);
                var bounds = flag.TransformToAncestor(host.Root).TransformBounds(new Rect(flag.RenderSize));
                var hit = host.Root.InputHitTest(point) as DependencyObject;
                var caption = (TextBlock)flag.Child;
                var flagInButton = flag.TransformToAncestor(button).TransformBounds(new Rect(flag.RenderSize));
                var textInFlag = caption.TransformToAncestor(flag).TransformBounds(new Rect(caption.RenderSize));
                var lastCharacter = caption.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
                var clippedFragments = new List<string>();
                foreach (var fragment in new FrameworkElement[] { flag, caption })
                    for (DependencyObject? ancestor = fragment; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
                    {
                        if (ancestor is FrameworkElement owner && LayoutInformation.GetLayoutClip(owner) is { } clip)
                        {
                            var fragmentBounds = ReferenceEquals(fragment, owner) ? new Rect(fragment.RenderSize)
                                : fragment.TransformToAncestor(owner).TransformBounds(new Rect(fragment.RenderSize));
                            if (!Fits(fragmentBounds, clip.Bounds))
                                clippedFragments.Add($"{fragment.GetType().Name}@{owner.GetType().Name}/{owner.Name}: bounds={fragmentBounds}, clip={clip.Bounds}");
                        }
                        if (ReferenceEquals(ancestor, host.Root)) break;
                    }
                Program.Assert(Fits(flagInButton, new Rect(button.RenderSize)) && Fits(textInFlag, new Rect(flag.RenderSize)) &&
                        !lastCharacter.IsEmpty && lastCharacter.Height > 0 && Fits(lastCharacter, new Rect(caption.RenderSize)) &&
                        clippedFragments.Count == 0,
                    $"The complete {expected} badge, including its final text line, must fit the real {node.Node.Id} button and every ancestor layout clip at progress {completed}, width {host.Root.ActualWidth:0}. " +
                    $"flagInButton={flagInButton}, button={button.RenderSize}, textInFlag={textInFlag}, flag={flag.RenderSize}, lastCharacter={lastCharacter}, text={caption.RenderSize}, clipped={string.Join("; ", clippedFragments)}.");
                Program.Assert(flag.IsVisible && flag.ActualWidth > 0 && flag.ActualHeight > 0 &&
                        new Rect(host.Root.RenderSize).Contains(bounds) && button.IsEnabled &&
                        hit is not null && (ReferenceEquals(hit, button) || button.IsAncestorOf(hit)) &&
                        ReferenceEquals(button.Command, vm.PreviewShanheNodeCommand) && ReferenceEquals(button.CommandParameter, node.Node) &&
                        button.Command!.CanExecute(button.CommandParameter),
                    $"The visible {expected} label at progress {completed}, width {host.Root.ActualWidth:0}, belongs to its real available preview target {node.Node.Id}, including original transparent sprite padding. " +
                    $"flag={bounds}, center={point}, hit={hit?.GetType().Name}/{(hit as FrameworkElement)?.Name}.");
            }
            Program.Assert(ShanheCampaignJson.Serialize(progress.CreateCheckpoint()) == before &&
                    Program.Engine(vm).Revision == ordinaryRevision && progress.PendingBattleSetup is null && !vm.IsShanheEncounterPreviewOpen,
                "Reading route labels and hit testing their real center points preserves the journal, resources, progress and battle boundary.");

            static bool Fits(Rect inner, Rect outer) => !inner.IsEmpty && !outer.IsEmpty &&
                inner.Left >= outer.Left - .5 && inner.Top >= outer.Top - .5 &&
                inner.Right <= outer.Right + .5 && inner.Bottom <= outer.Bottom + .5;
        }

        void AssertChapterBossArtwork(int completed)
        {
            var original = AssetImages.Load("Assets/Shanhe/Map/rogue_boss_icon.png");
            var actual = vm.ShanheChapterBossArtwork as BitmapSource;
            Program.Assert(vm.ShanheChapterMainProgress == completed && original is not null && actual is not null &&
                    actual.PixelWidth == original.PixelWidth && actual.PixelHeight == original.PixelHeight,
                "Every real mainline step preserves the original boss emblem dimensions.");
            var sourcePixels = Pixels(original!); var actualPixels = Pixels(actual!);
            var unchangedAlpha = true; var grayscale = true; var changedOpaqueColor = false; var transparentPadding = false;
            for (var index = 0; index < sourcePixels.Length; index += 4)
            {
                unchangedAlpha &= sourcePixels[index + 3] == actualPixels[index + 3];
                grayscale &= actualPixels[index] == actualPixels[index + 1] && actualPixels[index + 1] == actualPixels[index + 2];
                transparentPadding |= sourcePixels[index + 3] == 0;
                changedOpaqueColor |= sourcePixels[index + 3] > 0 &&
                    (sourcePixels[index] != actualPixels[index] || sourcePixels[index + 1] != actualPixels[index + 1] || sourcePixels[index + 2] != actualPixels[index + 2]);
            }
            Program.Assert(unchangedAlpha && transparentPadding &&
                    (completed < 6 ? grayscale && changedOpaqueColor : actualPixels.AsSpan().SequenceEqual(sourcePixels)),
                "Steps zero through five use the original boss pixels in grayscale with every alpha byte retained; only the real sixth step restores the complete original color image.");
        }

        void AssertOriginalMapImage(Image image, string file)
        {
            var original = AssetImages.Load("Assets/Shanhe/Map/" + file);
            var actual = image.Source as BitmapSource;
            Program.Assert(original is not null && actual is not null && actual.PixelWidth == original.PixelWidth &&
                    actual.PixelHeight == original.PixelHeight && Pixels(actual).AsSpan().SequenceEqual(Pixels(original)),
                "The actual chapter strip paints the verified original map artwork: " + file);
        }

        static byte[] Pixels(BitmapSource image)
        {
            var stride = image.PixelWidth * 4; var pixels = new byte[stride * image.PixelHeight];
            new FormatConvertedBitmap(image, PixelFormats.Bgra32, null, 0).CopyPixels(pixels, stride, 0);
            return pixels;
        }

        void Win(string id)
        {
            Accept(progress, new MoveShanheNodeCommand(id, progress.Revision));
            WinPendingBattle(progress);
            Accept(progress, new ChooseShanheRewardCommand("tactic", null, progress.Revision));
        }
    }

    private static ShanheCampaignDefinition ChapterRouteMapFixture()
    {
        const string route = "fixture:chapter-route";
        ShanheNodeDefinition Fight(string id, ShanheChapterNodeRole role, int step, int required,
            IReadOnlyList<string> next, ShanheNodeKind kind = ShanheNodeKind.Battle) =>
            new(id, "关隘·" + id, kind, next, [new("encounter:" + id, "驻军" + id, [new(Enemy)], VictoryGold: 200)], ["tactic"])
            { ChapterNumber = 1, ChapterTitle = "第一章·行军路线", ChapterRouteId = route, ChapterRole = role,
                ChapterProgressStep = step, RequiredChapterProgress = required, StoryText = "秘密介绍：仅在选中后展示。" };
        ShanheNodeDefinition Event(string id, int step, IReadOnlyList<string> next) =>
            new(id, "地点·" + id, step == 6 ? ShanheNodeKind.Camp : ShanheNodeKind.Event, next,
                Choices: [new("continue", "继续", RewardId: step == 2 ? "hand" : null)])
            { ChapterNumber = 1, ChapterTitle = "第一章·行军路线", ChapterRouteId = route,
                ChapterRole = step == 6 ? ShanheChapterNodeRole.Rest : ShanheChapterNodeRole.Event,
                ChapterProgressStep = step, RequiredChapterProgress = step - 1, StoryText = "秘密介绍：仅在选中后展示。" };
        return new("fixture:chapter-route-map", "六步行军", [Player], ["opening-1", "opening-2"],
        [
            Fight("opening-1", ShanheChapterNodeRole.OpeningBattle, 1, 0, ["story-1"]),
            Fight("opening-2", ShanheChapterNodeRole.OpeningBattle, 1, 0, ["story-1"]),
            Event("story-1", 2, ["story-2"]), Event("story-2", 3, ["market", "arena", "story-3"]),
            new("market", "集市", ShanheNodeKind.Shop, ["story-3"], Choices: [new("continue", "继续")])
                { ChapterNumber = 1, ChapterTitle = "第一章·行军路线", ChapterRouteId = route,
                    ChapterRole = ShanheChapterNodeRole.Market, RequiredChapterProgress = 3 },
            Fight("arena", ShanheChapterNodeRole.Arena, 0, 3, ["story-3"], ShanheNodeKind.EliteBattle),
            Event("story-3", 4, ["main"]), Fight("main", ShanheChapterNodeRole.MainBattle, 5, 4, ["rest"]),
            Event("rest", 6, ["optional", "extra-1", "boss"]),
            Fight("optional", ShanheChapterNodeRole.OptionalBattle, 0, 6, ["boss"]),
            Fight("extra-1", ShanheChapterNodeRole.ExtraBattle, 0, 6, ["extra-2", "boss"]) with { ExtraChapterBattleIndex = 1 },
            Fight("extra-2", ShanheChapterNodeRole.ExtraBattle, 0, 6, ["boss"]) with { ExtraChapterBattleIndex = 2 },
            Fight("boss", ShanheChapterNodeRole.Boss, 0, 6, [], ShanheNodeKind.BossBattle)
        ],
        [
            new("tactic", "战法·整备", Bonuses: new(Draw: 1)) { Rarity = 2 },
            new("hand", "随身杀", StartingCards: [new(CardKind.Slash, Suit.Heart, 7)]),
            new("march1", "日行千里Ⅰ") { CampaignEconomy = new(ExtraChapterBattles: 1) },
            new("march2", "日行千里Ⅱ") { CampaignEconomy = new(ExtraChapterBattles: 2) }
        ]);
    }
}

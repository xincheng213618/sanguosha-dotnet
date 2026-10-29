using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void ChapterRewardsShowSourceArtworkBeforeAndAfterClaiming(string output)
    {
        var catalog = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var tacticSkillIds = catalog.Nodes.Where(node => node.Id is "huaxiong" or "guosi")
            .SelectMany(node => node.BossRewards!.Groups.Single(group => group.Id == "tactics").RewardIds.Take(3))
            .Select(id => catalog.Rewards.Single(reward => reward.Id == id).TacticSkillId!)
            .Distinct(StringComparer.Ordinal).ToArray();
        // Resolve the published providers without starting their encounters.
        // The actual victory still uses the existing small two-seat fixture.
        var requirements = new ShanheBattleSetup(71, 1, "reward-artwork-providers", "classic:guan-yu", 4,
            0, 0, 0, 0, [], [], [], [new("classic:cao-cao")], [], new string('b', 64))
        { TacticSkillIds = tacticSkillIds };
        var publishedRegistry = StandardShanheCampaignCatalog.CreateBattleRegistry(requirements);
        var artworkRegistry = ContentRegistry.Build(new StandardContentPackage(), new StandardTeamModePackage(),
            new RewardBattlePackage(), new NativeLongRewardPackage(tacticSkillIds.Select(publishedRegistry.GetSkill).ToArray()));
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: artworkRegistry, shanheSaveStore: store,
            shanheCodexStore: new MemoryShanheCodexStore());
        using var native = new NativeResponseStatusChecks.NativeHost(vm);
        var panel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
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
                    // Tactics retain their complete published reward and native
                    // rule. Only the separate extra-skill group uses the fixture.
                    return reward.SkillId is not null
                        ? new ShanheRewardDefinition(id, reward.Name, SkillId: "fixture:shanhe-ui-reward-skill" + (index + 1))
                            { Rarity = 3, Description = reward.Description }
                        : reward;
                })).ToArray();
                var definition = new ShanheCampaignDefinition("fixture:chapter-artwork-" + chapter, "章节奖励", [Player], ["boss"],
                    [new("boss", "章节首领", ShanheNodeKind.BossBattle, [], [new("guard", "守军", [new(Enemy)])])
                        { ChapterNumber = chapter, ChapterTitle = original.ChapterTitle, BossRewards = new(500, 1, groups) }], rewards);
                var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => artworkRegistry);
                Accept(session, new MoveShanheNodeCommand("boss", session.Revision));
                WinPendingBattle(session, artworkRegistry);
                Attach(vm, session);
                RenderHost(null);
                AssertFixedOverviewArtwork();
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
                    AssertOverviewArtwork(group, session);
                    Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand, group), native.Root);
                    RenderHost(null);
                    var overlay = (FrameworkElement)panel.FindName("ShanheRewardOverlay");
                    Program.Assert(vm.IsShanheRewardOpen && !vm.IsShanheBossRewardOpen && overlay.IsVisible,
                        "The actual loaded reward selector is visible in its native window before reading a tactic.");
                    Program.Assert(vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(card =>
                            card.Rarity == 3 && (isSkill || card.Artwork is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 }) &&
                            (group.Id == "cards" ? card.Summary == "获得卡牌" : card.Summary != "获得卡牌")),
                        "Epic selectors display the source artwork without replacing tactic effect text with a physical-card summary.");
                    foreach (var candidate in vm.ShanheRewardCards)
                    {
                        var sharedCards = Program.Find<ShanheRewardChoiceCard>(Bound(panel, vm.SelectShanheRewardCardCommand, candidate)).ToArray();
                        Program.Assert(sharedCards.Length == 1 && ReferenceEquals(sharedCards[0].Artwork, candidate.Artwork),
                            "Each actual reward candidate uses one shared reward-choice card with its own catalogue artwork.");
                        var shared = sharedCards[0];
                        AssertSharedRewardChoiceFrame(shared, candidate.Rarity);
                        var authoredReward = definition.Rewards.Single(reward => reward.Id == candidate.Id);
                        var expectedType = group.Id == "tactics" ? 2 : group.Id == "skills" ? 3 : 4;
                        var typeArtwork = WushuangArt.Get($"rogue_award_type_icon_{expectedType}.png");
                        Program.Assert(candidate.Name == authoredReward.Name && shared.Title == candidate.DisplayName &&
                                shared.FallbackText == candidate.DisplayName && candidate.TypeIconIndex == expectedType &&
                                shared.TypeIconIndex == expectedType && ReferenceEquals(shared.TypeArtwork, typeArtwork) &&
                                Program.Find<Image>(shared).Any(image => ReferenceEquals(image.Source, typeArtwork) &&
                                    image.IsVisible && image.ActualWidth > 0 && image.ActualHeight > 0) &&
                                !shared.Title.StartsWith("战法·", StringComparison.Ordinal) &&
                                !shared.Title.StartsWith("传习·", StringComparison.Ordinal),
                            "Actual published tactic, skill and physical-card candidates retain their authored name and identity while their visible native card uses its real type icon and a display-only title.");
                        foreach (var grade in new[] { " I", " II", " III", "Ⅰ", "Ⅱ", "Ⅲ" })
                            if (authoredReward.Name.EndsWith(grade, StringComparison.Ordinal))
                                Program.Assert(shared.Title.EndsWith(grade, StringComparison.Ordinal),
                                    "The actual published reward grade remains complete in its visible title: " + authoredReward.Name);
                        if (group.Id == "tactics")
                        {
                            var published = catalog.Rewards.Single(reward => reward.Id == candidate.Id);
                            var retained = definition.Rewards.Single(reward => reward.Id == candidate.Id);
                            var nativeRule = publishedRegistry.GetSkill(published.TacticSkillId!);
                            var text = (TextBlock)shared.FindName("ChoiceDescriptionText");
                            Program.Assert(retained.TacticSkillId == published.TacticSkillId && retained.Description == published.Description &&
                                    artworkRegistry.GetSkill(published.TacticSkillId!).Description == nativeRule.Description &&
                                    candidate.Description == published.Description &&
                                    candidate.RuleDescription == nativeRule.Description && candidate.Summary == nativeRule.Description &&
                                    shared.Description == nativeRule.Description && text.Text == nativeRule.Description &&
                                    text.IsVisible && PresentationSource.FromVisual(text) is System.Windows.Interop.HwndSource &&
                                    text.ActualWidth > 0 && text.ActualHeight > 0,
                                "Each published tactic keeps its own original acquisition identity and visibly renders its complete authoritative native effect rather than a substituted fixture rule: " +
                                JsonSerializer.Serialize(new { candidate.Id, published.TacticSkillId, RetainedTacticSkillId = retained.TacticSkillId,
                                    PublishedDescription = published.Description, RetainedDescription = retained.Description,
                                    RegisteredDescription = artworkRegistry.GetSkill(published.TacticSkillId!).Description,
                                    candidate.Description, candidate.RuleDescription, candidate.Summary,
                                    SharedDescription = shared.Description, Expected = nativeRule.Description, Actual = text.Text,
                                    text.IsVisible, text.ActualWidth, text.ActualHeight }));
                        }
                        if (!isSkill)
                            Program.Assert(Program.Find<Image>(shared).Any(image =>
                                    image.Name == "ChoiceArtwork" && ReferenceEquals(image.Source, candidate.Artwork) &&
                                    image.Visibility == Visibility.Visible && image.ActualWidth > 0 && image.ActualHeight > 0),
                                "The shared candidate card must visibly render the exact decoded tactic icon or physical card face.");
                    }
                    if (group.Id == "tactics")
                        RenderHost(Path.Combine(output, $"shanhe-chapter-{chapter}-tactic-choice-full.png"));
                    else RenderHost(Path.Combine(output, $"shanhe-chapter-{chapter}-{group.Id}-type-icons.png"));
                    var chosen = vm.ShanheRewardCards[0];
                    Click(Bound(panel, vm.SelectShanheRewardCardCommand, chosen), native.Root);
                    AssertRewardEndAndReturnRemainSeparate(native, vm, session, chosen, store);
                    Click((Button)panel.FindName("ShanheConfirmRewardButton"), native.Root);
                    var resolved = vm.ShanheBossRewardGroups.Single(item => item.Id == originalGroup.Id);
                    Program.Assert(resolved.IsResolved && resolved.Artworks.Count == 1 &&
                            (isSkill ? session.Snapshot.ExtraSkillIds.Count == 1 : ReferenceEquals(resolved.Artworks[0].Image, chosen.Artwork)) &&
                            (group.Id != "cards" || session.Snapshot.StartingCards.Count == 1),
                        "Claimed groups retain exactly the selected reward's artwork and grant its actual skill or physical card.");
                    if (group.Id == "tactics")
                        Program.Assert(session.Snapshot.TacticIds.Contains(chosen.Id),
                            "Confirming the published tactic grants that exact native tactic identity, preserving its displayed real effect.");
                    AssertOverviewArtwork(resolved, session);
                }
                RenderHost(Path.Combine(output, "shanhe-chapter-" + chapter + "-reward-artwork.png"));
            }
        }
        finally { panel.DataContext = null; }
        NativeLongRewardRulesScrollWithoutClaiming(output);

        void RenderHost(string? path)
        {
            native.Root.Width = 1440; native.Root.Height = 880;
            native.Window.SizeToContent = SizeToContent.WidthAndHeight;
            native.Window.UpdateLayout();
            native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            native.Root.UpdateLayout();
            Program.Render(native.Root, 1440, 880, path);
            if (path is null || !vm.IsShanheRewardOpen) return;
            using var stream = File.OpenRead(path);
            var captured = new FormatConvertedBitmap(BitmapFrame.Create(stream, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad), PixelFormats.Bgra32, null, 0);
            foreach (var card in Program.Find<ShanheRewardChoiceCard>((ItemsControl)panel.FindName("ShanheRewardCardRows")))
            {
                var frame = (Image)card.FindName("FrameArtworkImage");
                var point = frame.TranslatePoint(new Point(frame.ActualWidth * .2, frame.ActualHeight * .9), native.Root);
                var sample = new Int32Rect((int)point.X - 2, (int)point.Y - 2, 5, 5);
                Program.Assert(sample.X >= 0 && sample.Y >= 0 && sample.X + sample.Width <= captured.PixelWidth &&
                    sample.Y + sample.Height <= captured.PixelHeight, "Every real reward frame is inside the actual captured native viewport.");
                var pixels = new byte[100]; captured.CopyPixels(sample, pixels, 20, 0);
                var painted = Enumerable.Range(0, 25).Count(index => pixels[index * 4 + 3] > 240 &&
                    pixels[index * 4 + 2] > 170 && pixels[index * 4 + 1] > 130 && pixels[index * 4] > 90);
                Program.Assert(painted >= 20,
                    $"The exported reward PNG paints each pale native card body at its actual transformed position, rather than only measuring an invisible card: {card.Title}; point={point}; painted={painted}.");
            }
        }

        void AssertOverviewArtwork(ShanheBossRewardGroup group, ShanheCampaignSession session)
        {
            var button = Bound(panel, vm.OpenShanheBossRewardGroupCommand, group);
            var shared = Program.Find<ShanheRewardChoiceCard>(button).Single();
            AssertSharedRewardChoiceFrame(shared, group.Rarity);
            Program.Assert(shared.ArtworkContent is not null && shared.Heading == group.Name &&
                    shared.Title == group.OverviewTitle && shared.Description == group.OverviewDescription &&
                    shared.Footer == group.OverviewFooter && shared.TypeIconIndex == group.TypeIconIndex &&
                    (group.IsResolved ? !button.IsEnabled && shared.Description == group.DisplaySelectedName : button.IsEnabled),
                "Unclaimed and claimed source reward groups reuse the same original quality-frame control, actual group state and exact selected reward name without dimming the completed illustration.");
            if (group.IsResolved)
                Program.Assert(group.SelectedName == session.Snapshot.BossRewardSummary!.Groups.Single(item => item.Id == group.Id).SelectedReward!.Name &&
                        group.Artworks.Single().Name == group.DisplaySelectedName,
                    "The claimed reward's authoritative name remains unchanged while its overview caption and artwork label use the same display-only name.");
            foreach (var art in group.Artworks.Where(art => art.HasImage))
                Program.Assert(Program.Find<Image>(button).Any(image => image.Name == "ShanheBossRewardArtwork" &&
                        ReferenceEquals(image.Source, art.Image) && image.Visibility == Visibility.Visible &&
                        image.ActualWidth > 0 && image.ActualHeight > 0),
                    "Both unclaimed and claimed overview groups must render their decoded source images instead of a generic choice placeholder.");
        }

        void AssertFixedOverviewArtwork()
        {
            var gold = Program.Find<ShanheRewardChoiceCard>((FrameworkElement)panel.FindName("ShanheBossFixedGoldCard")).Single();
            var slots = Program.Find<ShanheRewardChoiceCard>((FrameworkElement)panel.FindName("ShanheBossFixedSkillSlotsCard")).Single();
            AssertSharedRewardChoiceFrame(gold, 3);
            AssertSharedRewardChoiceFrame(slots, 2);
            Program.Assert(ReferenceEquals(gold.Artwork, vm.ShanheBossGoldArtwork) &&
                    gold.Artwork is BitmapSource { PixelWidth: 100, PixelHeight: 100 } &&
                    ReferenceEquals(slots.Artwork, vm.ShanheBossSkillSlotsArtwork) &&
                    slots.Artwork is BitmapSource { PixelWidth: 116, PixelHeight: 116 } &&
                    gold.Title == vm.ShanheBossFixedGoldText && slots.Title == vm.ShanheBossSkillSlotsText &&
                    gold.Footer == "已入账" && slots.Footer == "已生效" &&
                    ReferenceEquals(((Image)gold.FindName("ChoiceArtwork")).Source, gold.Artwork) &&
                    ReferenceEquals(((Image)slots.FindName("ChoiceArtwork")).Source, slots.Artwork),
                "The existing fixed boss credits show their true current values with the original two-copper-coin and skill-slot artwork, inside the shared native frame rather than local geometric substitutes.");
        }
    }

    private static void NativeLongRewardRulesScrollWithoutClaiming(string output)
    {
        var source = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId)
            .Rewards.Single(reward => reward.Id == "original-yeyan");
        var nativeSkill = StandardShanheCampaignCatalog.PlayerRosterRegistry.Skills[source.SkillId!];
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardTeamModePackage(),
            new RewardBattlePackage(), new NativeLongRewardPackage(nativeSkill));
        var definition = new ShanheCampaignDefinition("fixture:long-native-reward", "原生长规则奖励", [Player], ["battle"],
            [new("battle", "渡口守军", ShanheNodeKind.Battle, ["camp"],
                [new("guard", "守军", [new(Enemy)])], [source.Id], RewardChoiceCount: 1),
             new("camp", "营地", ShanheNodeKind.Camp, [], Choices: [new("continue", "继续")])],
            [source], ExtraSkillSlots: 1);
        var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => registry);
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        WinPendingBattle(session, registry);
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(), contentRegistry: registry,
            shanheSaveStore: store, shanheCodexStore: new MemoryShanheCodexStore());
        Attach(vm, session);
        using var native = new NativeResponseStatusChecks.NativeHost(vm);
        native.Root.Width = 1120; native.Root.Height = 780;
        native.Window.SizeToContent = SizeToContent.WidthAndHeight;
        Pump();
        var panel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
        var objective = (TextBlock)panel.FindName("ShanheChapterObjective");
        Program.Assert(!session.Snapshot.CanSkipReward && vm.ShanheRewardCards.All(reward => !reward.IsSkip) &&
                vm.ShanheChapterObjectiveText == vm.ShanheRewardNotice && objective.Text == vm.ShanheRewardNotice &&
                objective.IsVisible && objective.Text.Contains("必须领取", StringComparison.Ordinal) &&
                !objective.Text.Contains("放弃", StringComparison.Ordinal),
            "The native mandatory original-skill reward shows the same actual claim requirement in its visible chapter objective as in its selector, without advertising a nonexistent compensation route.");
        var candidate = vm.ShanheRewardCards.Single(card => card.Id == source.Id);
        var button = Bound(panel, vm.SelectShanheRewardCardCommand, candidate);
        var card = Program.Find<ShanheRewardChoiceCard>(button).Single();
        var scroll = (ScrollViewer)card.FindName("ChoiceDescriptionScroll");
        var text = (TextBlock)card.FindName("ChoiceDescriptionText");
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var saved = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
        var ordinary = Program.Engine(vm).Revision;
        Program.Assert(text.Text == nativeSkill.Description && scroll.ScrollableHeight > 0 &&
                text.LineStackingStrategy == LineStackingStrategy.BlockLineHeight &&
                Math.Abs(scroll.ViewportHeight - 6 * text.LineHeight) < 1 &&
                Math.Abs(scroll.ExtentHeight / text.LineHeight - Math.Round(scroll.ExtentHeight / text.LineHeight)) < .05 &&
                !scroll.Focusable && vm.SelectedShanheRewardCard is null,
            "The real native Yeyan rule uses six complete 35-pixel lines per viewport and overflows by whole lines without truncation or an initial selected reward.");
        native.FocusKeyboard();
        Program.Assert(button.Focus(), "The actual long-rule reward button accepts native keyboard focus without activating the desktop window.");
        Pump();
        var focusOutline = (System.Windows.Shapes.Rectangle)card.FindName("ChoiceKeyboardFocusOutline");
        var focusBounds = focusOutline.TransformToAncestor(card).TransformBounds(new Rect(focusOutline.RenderSize));
        Program.Assert(button.IsKeyboardFocused && card.IsChoiceFocused && focusOutline.IsVisible &&
                PresentationSource.FromVisual(focusOutline) is System.Windows.Interop.HwndSource &&
                !focusOutline.IsHitTestVisible && focusOutline.Stroke is not null && focusOutline.StrokeThickness > 0 &&
                focusBounds.Width > 0 && focusBounds.Height > 0 && focusBounds.Left >= -.7 && focusBounds.Top >= -.7 &&
                focusBounds.Right <= card.ActualWidth + .7 && focusBounds.Bottom <= card.ActualHeight + .7,
            "A keyboard-focused reward uses the shared visible on-card focus outline, bounded by its native frame and transparent to pointer input.");
        Unchanged("Keyboard focus only identifies the reward being read; it cannot stage or claim it.");
        Program.Assert(ReadKey(button, Key.PageDown) && scroll.VerticalOffset > 0,
            "PageDown on the actual focused reward button reads the original overflowing rule while its body scroller remains unfocusable.");
        var pageOffset = scroll.VerticalOffset;
        Unchanged("Keyboard paging does not select, claim or save a reward.");
        Program.Assert(ReadKey(button, Key.PageUp) && scroll.VerticalOffset < pageOffset,
            "PageUp on the same reward returns toward the start of its complete original rule.");
        Program.Assert(ReadKey(button, Key.End) && scroll.VerticalOffset + scroll.ViewportHeight >= scroll.ExtentHeight - 1,
            "End on the focused reward reveals the end of its full original rule.");
        var keyboardLastCharacter = text.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
        var keyboardLastBounds = text.TransformToAncestor(scroll).TransformBounds(keyboardLastCharacter);
        Program.Assert(!keyboardLastCharacter.IsEmpty && keyboardLastBounds.Top >= -.7 &&
                keyboardLastBounds.Bottom <= scroll.ActualHeight + .7 && keyboardLastBounds.Left >= -.7 &&
                keyboardLastBounds.Right <= scroll.ActualWidth + .7,
            "Keyboard End leaves the last actual original rule character completely readable inside the shared card.");
        Program.Render(native.Root, 1120, 780, Path.Combine(output, "shanhe-reward-native-yeyan-keyboard-end.png"));
        Program.Assert(ReadKey(button, Key.Home) && scroll.VerticalOffset < .7,
            "Home restores the first original rule line without moving keyboard focus away from the reward owner.");
        foreach (var key in new[] { Key.Left, Key.Right })
            Program.Assert(!ReadKey(button, key) && scroll.VerticalOffset < .7,
                "Shared reward reading does not intercept the owner's ordinary directional navigation: " + key);
        Program.Assert(!ReadKey(button, Key.End, ModifierKeys.Control) && scroll.VerticalOffset < .7,
            "Modified reading keys remain available to the existing modal shortcut owner.");
        Unchanged("All four reading keys and untouched directional shortcuts leave the pending reward, both games and its saved journal unchanged.");
        var endButton = (Button)panel.FindName("ShanheEndButton");
        Program.Assert(endButton.Focus(), "The existing expedition end button receives native keyboard focus without executing its command.");
        Pump();
        Program.Assert(endButton.IsKeyboardFocused && !card.IsChoiceFocused &&
                focusOutline.Visibility == Visibility.Collapsed,
            "The shared keyboard outline disappears when focus leaves its reward owner without masquerading as a selected reward.");
        Unchanged("Moving focus to the existing expedition end button retains the unresolved reward and cannot end or save the expedition.");
        Program.Assert(button.Focus(), "The original long-rule reward remains reachable after moving focus to the existing expedition end button.");
        Pump();
        Program.Assert(card.IsChoiceFocused && !endButton.IsKeyboardFocused && focusOutline.IsVisible,
            "Returning native keyboard focus restores only the original card's shared focus outline.");
        Program.Render(native.Root, 1120, 780, Path.Combine(output, "shanhe-reward-native-yeyan-rule-top.png"));
        var body = RewardChoiceBodyHit(native.Root, card);
        var wheel = new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, -120)
            { RoutedEvent = Mouse.PreviewMouseWheelEvent };
        body.RaiseEvent(wheel);
        if (!wheel.Handled) { wheel.RoutedEvent = Mouse.MouseWheelEvent; body.RaiseEvent(wheel); }
        Pump();
        Program.Assert(wheel.Handled && scroll.VerticalOffset > 0,
            "A wheel event hit on actual on-card rule text scrolls the native description instead of the enclosing reward overlay.");
        Unchanged("Reading a reward with the wheel cannot select or claim it.");
        scroll.ScrollToVerticalOffset(0); Pump();
        var bar = Program.Find<ScrollBar>(scroll).Single(item => item.Orientation == Orientation.Vertical && item.IsVisible);
        var track = (Track)bar.Template.FindName("PART_Track", bar);
        Program.Assert(track.Thumb is not null && bar.IsHitTestVisible && scroll.IsHitTestVisible,
            "The actual shared reward body exposes a live native scrollbar and thumb through the outer selection button.");
        // The system theme enables its track only while the bar is under the
        // pointer. This hidden HWND cannot acquire the desktop mouse hover.
        var mouseOverKey = (DependencyPropertyKey)typeof(UIElement).GetField("IsMouseOverPropertyKey",
            BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var wasMouseOver = bar.IsMouseOver;
        bar.SetValue(mouseOverKey, true);
        try
        {
            Pump();
            var thumbPoint = track.Thumb!.TranslatePoint(new Point(track.Thumb.ActualWidth / 2, track.Thumb.ActualHeight / 2), native.Root);
            var thumbHit = native.Root.InputHitTest(thumbPoint) as Visual;
            Program.Assert(track.IsEnabled && track.Thumb.IsEnabled && thumbHit is not null &&
                    (ReferenceEquals(thumbHit, track.Thumb) || thumbHit.IsDescendantOf(track.Thumb)),
                $"A hovered description thumb must receive its real input hit instead of the card; hit={thumbHit?.GetType().Name}, trackEnabled={track.IsEnabled}, point={thumbPoint}.");
            track.Thumb.RaiseEvent(new DragStartedEventArgs(0, 0) { RoutedEvent = Thumb.DragStartedEvent });
            track.Thumb.RaiseEvent(new DragDeltaEventArgs(0, 30) { RoutedEvent = Thumb.DragDeltaEvent });
            track.Thumb.RaiseEvent(new DragCompletedEventArgs(0, 30, false) { RoutedEvent = Thumb.DragCompletedEvent });
            Pump();
            Program.Assert(scroll.VerticalOffset > 0, "Dragging the real reward scrollbar thumb scrolls its complete original rule.");
            Unchanged("Dragging the rule scrollbar cannot select or claim its reward.");
            scroll.ScrollToVerticalOffset(0); Pump();
            var page = track.IncreaseRepeatButton!;
            Program.Assert(page.Command is RoutedCommand, "The native reward track keeps its routed page-down command.");
            ((RoutedCommand)page.Command).Execute(page.CommandParameter, page.CommandTarget ?? page);
            Pump();
            var lastCharacter = text.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
            var lastBounds = text.TransformToAncestor(scroll).TransformBounds(lastCharacter);
            Program.Assert(scroll.VerticalOffset > 0 && scroll.VerticalOffset + scroll.ViewportHeight >= scroll.ExtentHeight - 1 &&
                    !lastCharacter.IsEmpty && lastBounds.Top >= -.7 && lastBounds.Bottom <= scroll.ActualHeight + .7 &&
                    lastBounds.Left >= -.7 && lastBounds.Right <= scroll.ActualWidth + .7,
                $"Paging the real long rule reveals its final original character inside the card; last={lastBounds}, viewport={scroll.RenderSize}.");
            Unchanged("Paging the rule cannot select or claim its reward.");
        }
        finally { bar.SetValue(mouseOverKey, wasMouseOver); Pump(); }
        Program.Render(native.Root, 1120, 780, Path.Combine(output, "shanhe-reward-native-yeyan-rule-bottom.png"));
        ClickRewardChoiceBody(native.Root, card, button);
        Pump();
        Program.Assert(ReferenceEquals(vm.SelectedShanheRewardCard, candidate) && candidate.IsSelected &&
                vm.ConfirmShanheRewardCommand.CanExecute(null) &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved && Program.Engine(vm).Revision == ordinary,
            "A body-text release reaches the real outer card button and stages only its selection; the scroller cannot swallow that click or grant the skill.");
        Program.Assert(button.Focus(), "The selected reward keeps its separate keyboard reading owner.");
        Pump();
        foreach (var key in new[] { Key.PageDown, Key.PageUp, Key.End, Key.Home })
        {
            Program.Assert(ReadKey(button, key) && ReferenceEquals(vm.SelectedShanheRewardCard, candidate) &&
                    candidate.IsSelected && vm.ConfirmShanheRewardCommand.CanExecute(null) && session.Snapshot.ExtraSkillIds.Count == 0 &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                    (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved && Program.Engine(vm).Revision == ordinary,
                "Reading the already-selected original reward retains the same local draft and cannot execute its separate confirmation: " + key);
        }
        Click((Button)panel.FindName("ShanheConfirmRewardButton"), panel);
        Program.Assert(session.Snapshot.ExtraSkillIds.SequenceEqual([source.SkillId!]) && !vm.IsShanheRewardOpen,
            "Only the separate actual confirmation grants the selected original Yeyan skill once.");

        void Pump()
        {
            native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            native.Root.UpdateLayout();
        }
        bool ReadKey(UIElement owner, Key key, ModifierKeys modifiers = ModifierKeys.None)
        {
            var input = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(owner)!, Environment.TickCount, key)
                { RoutedEvent = Keyboard.PreviewKeyDownEvent };
            TableShortcutChecks.WithModifiers(modifiers, () => { owner.RaiseEvent(input); return true; });
            Pump();
            return input.Handled;
        }
        void Unchanged(string message) => Program.Assert(vm.SelectedShanheRewardCard is null &&
            !vm.ConfirmShanheRewardCommand.CanExecute(null) && session.Snapshot.ExtraSkillIds.Count == 0 &&
            ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
            (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved && Program.Engine(vm).Revision == ordinary, message);
    }

    private static UIElement RewardChoiceBodyHit(FrameworkElement root, ShanheRewardChoiceCard card)
    {
        var scroll = (ScrollViewer)card.FindName("ChoiceDescriptionScroll");
        var text = (TextBlock)card.FindName("ChoiceDescriptionText");
        var fixedPoint = scroll.TranslatePoint(new Point(scroll.ActualWidth / 2, Math.Min(30, scroll.ActualHeight / 2)), root);
        var fixedHit = root.InputHitTest(fixedPoint) as DependencyObject;
        // Camp cards can have an empty description while keeping this real text
        // hit surface. There is no character to sample in that case.
        if (text.Text.Length == 0 && ReferenceEquals(fixedHit, text)) return text;
        var lastHit = fixedHit;
        var lastPoint = fixedPoint;
        var visibleCharacters = 0;
        // A real glyph hit can be an inline Run instead of a UIElement. Sample
        // a completely visible character and require this exact TextBlock owner.
        var cursor = text.ContentStart.GetInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
        for (var examined = 0; cursor is not null && examined <= text.Text.Length + 2; examined++)
        {
            var next = cursor.GetNextInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
            if (next is null) break;
            var before = cursor.GetCharacterRect(System.Windows.Documents.LogicalDirection.Forward);
            var after = next.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
            cursor = next;
            if (before.IsEmpty || after.IsEmpty || Math.Abs(before.Top - after.Top) > .7 || Math.Abs(before.Left - after.Left) < 1) continue;
            var glyph = new Rect(Math.Min(before.Left, after.Left), before.Top,
                Math.Abs(after.Left - before.Left), before.Height);
            var visible = text.TransformToAncestor(scroll).TransformBounds(glyph);
            if (visible.Left < 0 || visible.Top < 0 || visible.Right > scroll.ViewportWidth + .7 ||
                visible.Bottom > scroll.ViewportHeight + .7) continue;
            visibleCharacters++;
            lastPoint = text.TranslatePoint(new Point(glyph.Left + glyph.Width / 2, glyph.Top + glyph.Height / 2), root);
            lastHit = root.InputHitTest(lastPoint) as DependencyObject;
            for (var owner = lastHit; owner is not null; owner = Parent(owner))
                if (ReferenceEquals(owner, text)) return text;
        }
        Program.Assert(false,
            "A real visible description character must hit its own rule TextBlock instead of another surface. " +
            $"fixedPoint={fixedPoint}, fixedHit={Chain(fixedHit)}; glyphPoint={lastPoint}, glyphHit={Chain(lastHit)}, " +
            $"visibleCharacters={visibleCharacters}; root={root.RenderSize}, card={card.RenderSize}, scroll={scroll.RenderSize}, " +
            $"text={text.RenderSize}, offset={scroll.VerticalOffset}, extent={scroll.ExtentWidth}x{scroll.ExtentHeight}, " +
            $"viewport={scroll.ViewportWidth}x{scroll.ViewportHeight}.");
        return text;

        static DependencyObject? Parent(DependencyObject node) => node is ContentElement content
            ? ContentOperations.GetParent(content) ?? (content as FrameworkContentElement)?.Parent
            : node is Visual ? VisualTreeHelper.GetParent(node) : LogicalTreeHelper.GetParent(node);
        static string Chain(DependencyObject? node)
        {
            var names = new List<string>();
            for (; node is not null; node = Parent(node))
                names.Add(node.GetType().Name + (node is FrameworkElement element ? ":" + element.Name : ""));
            return string.Join(" > ", names);
        }
    }

    private static void ClickRewardChoiceBody(FrameworkElement root, ShanheRewardChoiceCard card, Button? button = null)
    {
        var body = RewardChoiceBodyHit(root, card);
        var reachedCard = false;
        MouseButtonEventHandler observe = (_, input) => reachedCard |= !input.Handled;
        card.AddHandler(UIElement.MouseLeftButtonDownEvent, observe, true);
        try
        {
            var down = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
                { RoutedEvent = Mouse.PreviewMouseDownEvent };
            body.RaiseEvent(down);
            if (!down.Handled) { down.RoutedEvent = Mouse.MouseDownEvent; body.RaiseEvent(down); }
            Program.Assert(reachedCard, "The description ScrollViewer must leave a body press unhandled on its way to the card owner.");
            // This hidden HWND does not change the desktop mouse state. Supply only
            // ButtonBase's native pressed state, then exercise its actual release route.
            if (button is not null) SetPressed(true);
            var up = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
                { RoutedEvent = Mouse.PreviewMouseUpEvent };
            body.RaiseEvent(up);
            if (!up.Handled) { up.RoutedEvent = Mouse.MouseUpEvent; body.RaiseEvent(up); }
        }
        finally
        {
            card.RemoveHandler(UIElement.MouseLeftButtonDownEvent, observe);
            if (button is not null) SetPressed(false);
        }

        void SetPressed(bool value) => typeof(ButtonBase).GetMethod("SetIsPressed", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(button, [value]);
    }

    private static void AssertRewardEndAndReturnRemainSeparate(NativeResponseStatusChecks.NativeHost native, MainViewModel vm,
        ShanheCampaignSession session, ShanheRewardCard selected, MemoryShanheCampaignSaveStore store)
    {
        var panel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
        foreach (var size in new[] { new Size(1120, 780), new Size(1440, 880) })
        {
            native.Root.Width = size.Width; native.Root.Height = size.Height;
            native.Window.SizeToContent = SizeToContent.WidthAndHeight;
            native.Window.UpdateLayout();
            native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            native.Root.UpdateLayout();
            Program.Render(native.Root, (int)size.Width, (int)size.Height, null);
            var end = (Button)panel.FindName("ShanheEndButton");
            var back = (Button)panel.FindName("ShanheRewardBackButton");
            var endBounds = end.TransformToAncestor(panel).TransformBounds(new Rect(end.RenderSize));
            var backBounds = back.TransformToAncestor(panel).TransformBounds(new Rect(back.RenderSize));
            var objective = (FrameworkElement)panel.FindName("ShanheChapterObjectiveBanner");
            var objectiveBounds = objective.TransformToAncestor(panel).TransformBounds(new Rect(objective.RenderSize));
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            var confirmBounds = confirm.TransformToAncestor(panel).TransformBounds(new Rect(confirm.RenderSize));
            Program.Assert(end.Visibility == Visibility.Visible && back.Visibility == Visibility.Visible &&
                    endBounds.Width > 0 && backBounds.Width > 0 && endBounds.Top >= 0 && backBounds.Top >= 0 &&
                    endBounds.Right <= size.Width && backBounds.Right <= size.Width &&
                    endBounds.Bottom <= size.Height && backBounds.Bottom <= size.Height && !endBounds.IntersectsWith(backBounds) &&
                    !backBounds.IntersectsWith(objectiveBounds) && !backBounds.IntersectsWith(confirmBounds) &&
                    Math.Abs(backBounds.Top + backBounds.Height / 2 - (confirmBounds.Top + confirmBounds.Height / 2)) < 1,
                "The return action shares the reward footer with its separate confirmation target, stays clear of the chapter objective and expedition end action, and remains fully visible at both supported window sizes: " +
                    JsonSerializer.Serialize(new { size, endBounds, backBounds, objectiveBounds, confirmBounds }));
            var caption = Program.Find<TextBlock>(back).Single(text => text.Text == back.Content?.ToString());
            var presenter = Program.Find<ContentPresenter>(back).Single(content => content.IsAncestorOf(caption));
            var captionBounds = caption.TransformToAncestor(presenter).TransformBounds(new Rect(caption.RenderSize));
            var characters = new List<Rect>();
            var cursor = caption.ContentStart.GetInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
            for (var examined = 0; cursor is not null && examined <= caption.Text.Length + 2; examined++)
            {
                var next = cursor.GetNextInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
                if (next is null) break;
                var before = cursor.GetCharacterRect(System.Windows.Documents.LogicalDirection.Forward);
                var after = next.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
                cursor = next;
                if (before.IsEmpty || after.IsEmpty || Math.Abs(before.Top - after.Top) > .7 || Math.Abs(before.Left - after.Left) < 1) continue;
                var character = new Rect(Math.Min(before.Left, after.Left), before.Top, Math.Abs(after.Left - before.Left), before.Height);
                characters.Add(caption.TransformToAncestor(presenter).TransformBounds(character));
            }
            var backHit = CenterHits(back);
            var endHit = CenterHits(end);
            var layoutClips = new List<(FrameworkElement Owner, Rect Bounds)>();
            for (DependencyObject? ancestor = caption; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
            {
                if (ancestor is FrameworkElement element && LayoutInformation.GetLayoutClip(element) is { } clip)
                    layoutClips.Add((element, clip.Bounds));
                if (ReferenceEquals(ancestor, back)) break;
            }
            var captionUnclipped = layoutClips.All(entry =>
            {
                var bounds = ReferenceEquals(entry.Owner, caption) ? new Rect(caption.RenderSize)
                    : caption.TransformToAncestor(entry.Owner).TransformBounds(new Rect(caption.RenderSize));
                return !entry.Bounds.IsEmpty && bounds.Left >= entry.Bounds.Left - .7 && bounds.Top >= entry.Bounds.Top - .7 &&
                    bounds.Right <= entry.Bounds.Right + .7 && bounds.Bottom <= entry.Bounds.Bottom + .7;
            });
            Program.Assert(caption.IsVisible && presenter.IsVisible && captionBounds.Width > 0 && captionBounds.Height > 0 &&
                    FitsCaption(captionBounds) && characters.Count == caption.Text.Count(character => !char.IsWhiteSpace(character)) &&
                    characters.Count > 0 && characters.All(FitsCaption) && captionUnclipped && backHit && endHit,
                "Every actual return-caption character fits its live ContentPresenter and both distinct button centers receive their own real input hits at either window size: " +
                JsonSerializer.Serialize(new { size, backBounds, endBounds, captionBounds, Caption = caption.RenderSize,
                    Presenter = presenter.RenderSize, characters, captionUnclipped,
                    Clips = layoutClips.Select(entry => new { entry.Owner.Name, Bounds = entry.Bounds.ToString() }), backHit, endHit }));

            bool FitsCaption(Rect bounds) => bounds.Left >= -.7 && bounds.Top >= -.7 &&
                bounds.Right <= presenter.ActualWidth + .7 && bounds.Bottom <= presenter.ActualHeight + .7;
            bool CenterHits(Button target)
            {
                var point = target.TranslatePoint(new Point(target.ActualWidth / 2, target.ActualHeight / 2), panel);
                for (var hit = panel.InputHitTest(point) as DependencyObject; hit is not null; hit = hit is ContentElement content
                        ? ContentOperations.GetParent(content) ?? (content as FrameworkContentElement)?.Parent
                        : hit is Visual ? VisualTreeHelper.GetParent(hit) : LogicalTreeHelper.GetParent(hit))
                    if (ReferenceEquals(hit, target)) return true;
                return false;
            }
        }
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var saved = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
        var ordinary = Program.Engine(vm).Revision;
        Click((Button)panel.FindName("ShanheEndButton"), native.Root);
        Program.Assert(vm.IsShanheEndConfirmationOpen && ReferenceEquals(vm.SelectedShanheRewardCard, selected),
            "Opening expedition end confirmation keeps the existing selected reward draft.");
        Click((Button)panel.FindName("ShanheCancelEndButton"), native.Root);
        Program.Assert(!vm.IsShanheEndConfirmationOpen && vm.IsShanheRewardOpen &&
                ReferenceEquals(vm.SelectedShanheRewardCard, selected) && selected.IsSelected &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved && Program.Engine(vm).Revision == ordinary,
            "Canceling expedition end returns to the same reward draft without claiming, saving or changing either game.");
    }

    private sealed class NativeLongRewardPackage(params ContentSkillDefinition[] skills) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-long-reward", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var skill in skills) builder.AddSkill(skill);
        }
    }

    private static void AssertSharedRewardChoiceFrame(ShanheRewardChoiceCard card, int rarity)
    {
        var expected = WushuangArt.Get($"rogue_award_quality_bg_{rarity}.png");
        Program.Assert(card.Rarity == rarity && ReferenceEquals(card.FrameArtwork, expected) &&
                card.FrameArtwork is BitmapSource { PixelWidth: 500, PixelHeight: 768 } &&
                Program.Find<Image>(card).Any(image => image.Name == "FrameArtworkImage" &&
                    ReferenceEquals(image.Source, expected) && image.Visibility == Visibility.Visible && image.ActualWidth > 0 && image.ActualHeight > 0),
            "Rest and reward cards render the same official 500-by-768 quality-frame family at their actual rarity.");
        if (!string.IsNullOrWhiteSpace(card.Footer))
        {
            var footer = (TextBlock)card.FindName("ChoiceFooterText");
            var frame = Program.Find<Image>(card).Single(image => image.Name == "FrameArtworkImage");
            var footerBounds = footer.TransformToAncestor(card).TransformBounds(new Rect(footer.RenderSize));
            var frameBounds = frame.TransformToAncestor(card).TransformBounds(new Rect(frame.RenderSize));
            Program.Assert(footer.Text == card.Footer && footerBounds.Width > 0 && footerBounds.Height > 0 &&
                    footerBounds.Left >= frameBounds.Left - .7 && footerBounds.Top >= frameBounds.Top - .7 &&
                    footerBounds.Right <= frameBounds.Right + .7 && footerBounds.Bottom <= frameBounds.Bottom + .7,
                "The actual camp growth value and reward footer remain entirely inside their unchanged source frame.");
        }
    }
}

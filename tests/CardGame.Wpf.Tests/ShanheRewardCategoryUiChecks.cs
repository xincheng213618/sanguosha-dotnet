using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public static void CategoriesFourChoicesBonusOnlyAndThreeBossGroups(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: store,
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
            AssertObjective(vm.ShanheRewardNotice, "The bound chapter objective immediately describes the actual category selector.");
            Program.Assert(vm.ShanheChapterObjectiveText.Contains("类别", StringComparison.Ordinal) &&
                    !vm.ShanheChapterObjectiveText.Contains("放弃", StringComparison.Ordinal),
                "An uncommitted reward category cannot advertise compensation before its actual candidates are known.");
            var before = Snapshot(session);
            ReadOnlyUi(() => Click(retiredCategoryButton, panel), "Staging a category leaves both games, its full command journal and the saved expedition unchanged.");
            Program.Assert(category.IsSelected && confirm.IsEnabled && Snapshot(session) == before,
                "Selecting a category previews it; only the bound confirmation commits the category choice.");
            Click(confirm, panel);
            Program.Assert(session.Snapshot.RewardCategories.Count == 0 && session.Snapshot.RewardOffers.Count == 4 &&
                vm.ShanheRewardCards.Count == 4 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                vm.ShanheRewardCards.All(card => card.RewardCategoryChoice is null) && !confirm.IsEnabled &&
                session.AcceptedCommands.OfType<ChooseShanheRewardCategoryCommand>().Single().Category == ShanheRewardCategory.Tactic,
                "The equipped category tactic opens four actual frozen tactic candidates with no tactic decline route.");
            AssertObjective(vm.ShanheRewardNotice, "The category confirmation immediately refreshes the bound chapter objective to its actual tactic candidates.");
            Program.Assert(!session.Snapshot.CanSkipReward && vm.ShanheChapterObjectiveText.Contains("必须领取", StringComparison.Ordinal) &&
                    !vm.ShanheChapterObjectiveText.Contains("放弃", StringComparison.Ordinal),
                "The selected mandatory tactic category cannot retain the category selector's text or invent a decline entitlement.");
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
            ReadOnlyUi(() => Click(primaryButton, panel), "Staging the mandatory tactic keeps the current reward and expedition journals intact until confirmation.");
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
            AssertObjective(vm.ShanheBossRewardNotice, "The primary claim immediately changes the bound objective to the pending bonus summary.");
            Program.Assert(vm.ShanheChapterObjectiveText.Contains("1", StringComparison.Ordinal) &&
                    vm.ShanheChapterObjectiveText.Contains("追加", StringComparison.Ordinal) &&
                    vm.ShanheChapterObjectiveText.Contains("继续行军", StringComparison.Ordinal) &&
                    !vm.ShanheChapterObjectiveText.Contains("下一章", StringComparison.Ordinal),
                "The bonus summary reports its one unresolved group and promises only the actual continuation of the current chapter.");
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
            ReadOnlyUi(() => Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand, group), panel),
                "Opening the actual bonus group is a local presentation action and does not submit or save a campaign command.");
            Program.Assert(vm.ShanheRewardCards.Count == 4 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                vm.ShanheRewardHeading.Contains("追 加", StringComparison.Ordinal) &&
                vm.ShanheRewardNotice.Contains("4", StringComparison.Ordinal) &&
                confirm.Content.ToString()!.Contains("追 加", StringComparison.Ordinal),
                "Opening the additional group shows its four real candidates and uses additional-reward wording without ordinary skip compensation.");
            AssertObjective(vm.ShanheRewardNotice, "Opening a mandatory bonus group immediately refreshes the real objective binding without a campaign revision.");
            Program.Assert(vm.ShanheChapterObjectiveText.Contains("必须领取", StringComparison.Ordinal) &&
                    !vm.ShanheChapterObjectiveText.Contains("放弃", StringComparison.Ordinal),
                "The actual mandatory bonus group advertises no ordinary reward compensation.");
            ReadOnlyUi(() => Click((Button)panel.FindName("ShanheRewardBackButton"), panel),
                "Returning from the bonus selector to its summary leaves the frozen reward and both journals unchanged.");
            AssertObjective(vm.ShanheBossRewardNotice, "Returning to the summary immediately restores its unresolved-group objective.");
            ReadOnlyUi(() => Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand, vm.ShanheBossRewardGroups.Single()), panel),
                "Reopening the same pending bonus group does not choose or claim it.");
            AssertObjective(vm.ShanheRewardNotice, "Reopening the bonus selector updates the objective again without depending on a campaign command notification.");
            var extra = vm.ShanheRewardCards.First();
            ReadOnlyUi(() => Click(Bound(panel, vm.SelectShanheRewardCardCommand, extra), panel),
                "Staging the extra tactic retains the full pending journal and saved expedition.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-reward-bonus-four-choices.png"));
            Click(confirm, panel);
            Program.Assert(vm.IsShanheBossRewardOpen && vm.ShanheBossRewardGroups.Single().IsResolved && finish.IsEnabled &&
                session.Snapshot.TacticIds.Contains(extra.Id) && session.Snapshot.Phase == ShanheCampaignPhase.Reward &&
                session.Snapshot.CompletedNodeIds.Count == 0 && session.Snapshot.Gold == gold,
                "The additional claim grants its actual tactic once and returns to the summary before route continuation.");
            AssertObjective(vm.ShanheBossRewardNotice, "Resolving the final bonus group refreshes the bound objective before route continuation.");
            Program.Assert(vm.ShanheChapterObjectiveText.Contains("已处理", StringComparison.Ordinal) &&
                    vm.ShanheChapterObjectiveText.Contains("继续行军", StringComparison.Ordinal) &&
                    !vm.ShanheChapterObjectiveText.Contains("待处理", StringComparison.Ordinal),
                "An entirely resolved bonus summary requests only its remaining confirmation and no longer claims a reward group is pending.");
            Click(finish, panel);
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
                session.Snapshot.CompletedNodeIds.SequenceEqual(["battle"]) && session.Snapshot.BossRewardSummary is null &&
                !vm.IsShanheRewardOpen && !vm.IsShanheBossRewardOpen && session.Snapshot.Gold == gold &&
                session.AcceptedCommands.OfType<ChooseShanheRewardCommand>().Count() == 1 &&
                session.AcceptedCommands.OfType<ChooseShanheBossRewardCommand>().Count() == 1 &&
                session.AcceptedCommands.OfType<ConfirmShanheBossRewardCommand>().Count() == 1 &&
                Program.Engine(vm).Revision == ordinaryRevision,
                "The bonus summary confirmation completes the current node exactly once and opens the actual next route.");

            CheckThreeBossGroups(vm, panel, store, output);

            void AssertObjective(string expected, string message) => Program.Assert(
                ((TextBlock)panel.FindName("ShanheChapterObjective")).Text == vm.ShanheChapterObjectiveText &&
                vm.ShanheChapterObjectiveText == expected, message);
            void ReadOnlyUi(Action action, string message)
            {
                var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                var saved = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
                var ordinary = Program.Engine(vm).Revision;
                action();
                Layout(panel);
                Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                    (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved && Program.Engine(vm).Revision == ordinary, message);
            }
        }
        finally { panel.DataContext = null; }
    }

    private static void CheckThreeBossGroups(MainViewModel vm, ShanheCampaignPanel panel, MemoryShanheCampaignSaveStore store, string output)
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
        var checkpointBeforeLayout = Snapshot(session);
        var saveBeforeLayout = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
        var ordinary = Program.Engine(vm);
        var ordinaryBeforeLayout = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        using var native = new NativeResponseStatusChecks.NativeHost(vm);
        var nativePanel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
        foreach (var size in new[] { new Size(1120, 780), new Size(1440, 880) })
        {
            native.Root.Width = size.Width; native.Root.Height = size.Height;
            native.Window.SizeToContent = SizeToContent.WidthAndHeight;
            native.Window.UpdateLayout();
            native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            native.Root.UpdateLayout();
            var path = Path.Combine(output, $"shanhe-boss-native-three-groups-{size.Width:0}.png");
            Program.Render(native.Root, (int)size.Width, (int)size.Height, path);
            Program.Assert(native.Root.RenderSize == size && vm.ShanheBossRewardGroups.Count == 3 &&
                    vm.ShanheBossRewardGroups.All(group => !group.IsResolved),
                "The native overview retains all three original pending groups at each exact supported viewport.");
            var frames = new List<Rect>();
            foreach (var name in new[] { "ShanheBossFixedGoldCard", "ShanheBossFixedSkillSlotsCard" })
            {
                var fixedCard = (FrameworkElement)nativePanel.FindName(name);
                Program.Assert(fixedCard.IsVisible, "The real three-group overview keeps each credited fixed reward visible.");
                var shared = Program.Find<ShanheRewardChoiceCard>(fixedCard).Single();
                AssertNativeFrame(shared, fixedCard);
            }
            foreach (var group in vm.ShanheBossRewardGroups)
            {
                var button = Bound(nativePanel, vm.OpenShanheBossRewardGroupCommand, group);
                var shared = Program.Find<ShanheRewardChoiceCard>(button).Single();
                Program.Assert(shared.Rarity == group.Rarity && shared.Heading == group.Name &&
                        ReferenceEquals(button.Command, vm.OpenShanheBossRewardGroupCommand) &&
                        ReferenceEquals(button.CommandParameter, group) && button.IsEnabled && button.Command.CanExecute(group),
                    "Every native overview group keeps its own rarity, complete heading and actual group-opening command.");
                AssertNativeFrame(shared, button);
                var title = Program.Find<TextBlock>(shared).Single(text => text.Text == group.Name);
                AssertWholeHeading(title, shared);
                var point = button.TranslatePoint(new Point(button.ActualWidth / 2, button.ActualHeight / 2), native.Root);
                Program.Assert(OwnsHit(button, native.Root.InputHitTest(point) as DependencyObject),
                    "The center of each actual overview button remains an input hit on that same group, without opening or claiming it.");
            }
            for (var first = 0; first < frames.Count; first++)
                for (var second = first + 1; second < frames.Count; second++)
                    Program.Assert(!frames[first].IntersectsWith(frames[second]),
                        "All five actual native reward frames remain separate rather than overlapping adjacent fixed or selectable rewards.");
            using (var stream = File.OpenRead(path))
            {
                var captured = new FormatConvertedBitmap(BitmapFrame.Create(stream, BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad), PixelFormats.Bgra32, null, 0);
                foreach (var frame in frames)
                {
                    var sample = new Int32Rect((int)(frame.Left + frame.Width * .2) - 2,
                        (int)(frame.Top + frame.Height * .9) - 2, 5, 5);
                    Program.Assert(sample.X >= 0 && sample.Y >= 0 && sample.X + sample.Width <= captured.PixelWidth &&
                            sample.Y + sample.Height <= captured.PixelHeight,
                        "Each real overview card body lies within the exported native PNG.");
                    var pixels = new byte[100]; captured.CopyPixels(sample, pixels, 20, 0);
                    var painted = Enumerable.Range(0, 25).Count(index => pixels[index * 4 + 3] > 240 &&
                        pixels[index * 4 + 2] > 170 && pixels[index * 4 + 1] > 130 && pixels[index * 4] > 90);
                    Program.Assert(painted >= 20,
                        "Each exported overview PNG visibly paints all five pale native reward-card bodies at their measured positions.");
                }
            }
            Program.Assert(Snapshot(session) == checkpointBeforeLayout &&
                    (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saveBeforeLayout &&
                    ReferenceEquals(Program.Engine(vm), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryBeforeLayout,
                "Opening and resizing the real native overview does not submit a campaign or table command, claim a group, or write the saved expedition.");

            void AssertNativeFrame(ShanheRewardChoiceCard shared, FrameworkElement owner)
            {
                var image = (Image)shared.FindName("FrameArtworkImage");
                var bounds = image.TransformToAncestor(native.Root).TransformBounds(new Rect(image.RenderSize));
                var ownerBounds = owner.TransformToAncestor(native.Root).TransformBounds(new Rect(owner.RenderSize));
                var ancestors = new List<object>();
                for (DependencyObject? ancestor = image; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
                {
                    if (ancestor is FrameworkElement element)
                    {
                        var fragment = ReferenceEquals(element, image) ? new Rect(image.RenderSize)
                            : image.TransformToAncestor(element).TransformBounds(new Rect(image.RenderSize));
                        var layoutClip = LayoutInformation.GetLayoutClip(element);
                        ancestors.Add(new
                        {
                            Type = element.GetType().Name, element.Name, element.IsVisible, element.IsEnabled,
                            Actual = element.RenderSize.ToString(), Desired = element.DesiredSize.ToString(),
                            FrameBounds = fragment.ToString(), LayoutClip = layoutClip?.Bounds.ToString(),
                            FitsLayoutClip = layoutClip is null || Fits(fragment, layoutClip.Bounds),
                            ExplicitClip = element.Clip?.Bounds.ToString(),
                            FitsExplicitClip = element.Clip is null || Fits(fragment, element.Clip.Bounds)
                        });
                    }
                    if (ReferenceEquals(ancestor, native.Root)) break;
                }
                var bitmap = image.Source as BitmapSource;
                Program.Assert(shared.IsVisible && image.IsVisible && shared.Rarity is >= 1 and <= 4 &&
                        PresentationSource.FromVisual(image) is System.Windows.Interop.HwndSource &&
                        image.Source is BitmapSource { PixelWidth: 500, PixelHeight: 768 } &&
                        ReferenceEquals(image.Source, shared.FrameArtwork) && bounds.Width > 100 && bounds.Height > 150 &&
                        Math.Abs(bounds.Width / bounds.Height - 500d / 768) < .002 &&
                        Fits(bounds, ownerBounds) && Fits(bounds, new Rect(size)),
                    "Every fixed and selectable overview reward renders the original 500-by-768 frame without stretching, cropping or leaving the native viewport: " +
                    JsonSerializer.Serialize(new
                    {
                        Group = (owner.DataContext as ShanheBossRewardGroup)?.Id,
                        Viewport = size.ToString(), shared.Heading, shared.Title, shared.Rarity,
                        SharedVisible = shared.IsVisible, ImageVisible = image.IsVisible,
                        Hwnd = PresentationSource.FromVisual(image) is System.Windows.Interop.HwndSource,
                        Source = bitmap is null ? null : new { bitmap.PixelWidth, bitmap.PixelHeight },
                        SourceMatchesSharedFrame = ReferenceEquals(image.Source, shared.FrameArtwork),
                        FrameBounds = bounds.ToString(), OwnerBounds = ownerBounds.ToString(),
                        SharedSize = shared.RenderSize.ToString(), ImageSize = image.RenderSize.ToString(),
                        Ratio = (bounds.Width / bounds.Height).ToString("G17"),
                        ExpectedRatio = (500d / 768).ToString("G17"),
                        FitsOwner = Fits(bounds, ownerBounds), FitsViewport = Fits(bounds, new Rect(size)), ancestors
                    }));
                AssertSharedRewardChoiceFrame(shared, shared.Rarity);
                frames.Add(bounds);
            }

            void AssertWholeHeading(TextBlock title, ShanheRewardChoiceCard shared)
            {
                var glyphs = new List<Rect>();
                var cursor = title.ContentStart.GetInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
                for (var examined = 0; cursor is not null && examined <= title.Text.Length + 2; examined++)
                {
                    var next = cursor.GetNextInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
                    if (next is null) break;
                    var before = cursor.GetCharacterRect(System.Windows.Documents.LogicalDirection.Forward);
                    var after = next.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
                    var segment = new System.Windows.Documents.TextRange(cursor, next).Text;
                    cursor = next;
                    if (string.IsNullOrWhiteSpace(segment) || before.IsEmpty || after.IsEmpty ||
                        Math.Abs(before.Top - after.Top) > .7 || Math.Abs(before.Left - after.Left) < 1) continue;
                    glyphs.Add(new Rect(Math.Min(before.Left, after.Left), before.Top, Math.Abs(after.Left - before.Left), before.Height));
                }
                var clipping = new List<string>();
                foreach (var glyph in glyphs)
                    for (DependencyObject? ancestor = title; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
                    {
                        if (ancestor is FrameworkElement element && LayoutInformation.GetLayoutClip(element) is { } clip)
                        {
                            var bounds = ReferenceEquals(element, title) ? glyph : title.TransformToAncestor(element).TransformBounds(glyph);
                            if (!Fits(bounds, clip.Bounds)) clipping.Add(element.GetType().Name + "/" + element.Name +
                                ": glyph=" + bounds + ", layoutClip=" + clip.Bounds);
                        }
                        if (ReferenceEquals(ancestor, native.Root)) break;
                    }
                var frame = (Image)shared.FindName("FrameArtworkImage");
                var frameInCard = frame.TransformToAncestor(shared).TransformBounds(new Rect(frame.RenderSize));
                Program.Assert(title.IsVisible && title.TextWrapping == TextWrapping.Wrap &&
                        glyphs.Count == title.Text.Count(character => !char.IsWhiteSpace(character)) && glyphs.Count > 0 &&
                        glyphs.All(glyph => Fits(glyph, new Rect(title.RenderSize)) &&
                            Fits(title.TransformToAncestor(shared).TransformBounds(glyph), frameInCard)) && clipping.Count == 0,
                    "Every original group-heading glyph remains fully visible inside its shared native frame and all actual ancestor layout clips at each viewport: " +
                    JsonSerializer.Serialize(new { title.Text, Size = size.ToString(), Glyphs = glyphs.Select(glyph => glyph.ToString()), clipping }));
            }
        }

        static bool Fits(Rect bounds, Rect owner) => bounds.Width > 0 && bounds.Height > 0 &&
            bounds.Left >= owner.Left - .7 && bounds.Top >= owner.Top - .7 &&
            bounds.Right <= owner.Right + .7 && bounds.Bottom <= owner.Bottom + .7;
        static bool OwnsHit(Button button, DependencyObject? hit)
        {
            for (var owner = hit; owner is not null; owner = owner is ContentElement content
                    ? ContentOperations.GetParent(content) ?? (content as FrameworkContentElement)?.Parent
                    : owner is Visual ? VisualTreeHelper.GetParent(owner) : LogicalTreeHelper.GetParent(owner))
                if (ReferenceEquals(owner, button)) return true;
            return false;
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

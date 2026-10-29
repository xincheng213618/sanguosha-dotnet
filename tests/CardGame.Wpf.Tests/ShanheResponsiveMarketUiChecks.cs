using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheResponsiveMarketUiChecks
{
    public static void CampaignFillsMainWindowClientArea(string output)
    {
        using var vm = Create();
        var owner = new MainWindow(vm);
        try
        {
            owner.ApplyTemplate();
            vm.OpenShanheCommand.Execute(null);
            var root = (FrameworkElement)owner.Content;
            var overlay = (Border)owner.FindName("ShanheOverlay");
            var panel = (ShanheCampaignPanel)owner.FindName("ShanhePanel");
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 900), new Size(1600, 1000) })
            {
                Layout(root, size);
                Fills(panel, overlay);
                Fills((Grid)panel.FindName("CampaignSurface"), panel);
                var close = (Button)panel.FindName("ShanheCloseButton");
                var bounds = Bounds(close, root);
                Program.Assert(bounds.Left >= 0 && bounds.Right <= size.Width + 1 && bounds.Bottom <= size.Height + 1,
                    "The main window retains its return action inside the client area at every supported size.");
            }
            Program.Render(root, 1440, 900, Path.Combine(output, "shanhe-campaign-client-fill-1440x900.png"));
            Layout(root, new(1120, 780));
            Program.Render(root, 1120, 780, Path.Combine(output, "shanhe-campaign-client-fill-1120x780.png"));
            var revision = Program.Engine(vm).Revision;
            Program.Assert(ReferenceEquals(FocusSurface(owner, vm), overlay) &&
                KeyboardNavigation.GetTabNavigation(overlay) == KeyboardNavigationMode.Cycle &&
                TableShortcutChecks.Input(owner, Key.Enter, ModifierKeys.Control) &&
                !TableShortcutChecks.Input(owner, Key.Enter) && Program.Engine(vm).Revision == revision,
                "The main-window map owns modal keyboard input and leaves the covered ordinary table unchanged.");
            Click((Button)panel.FindName("ShanheCloseButton"));
            Layout(root, new(1120, 780));
            Program.Assert(!vm.IsShanheOpen && Program.Engine(vm).Revision == revision && owner.OwnedWindows.Count == 0,
                "Returning from the campaign uses the same main window and preserves the covered table.");
        }
        finally { owner.Close(); }
    }

    public static void FloatingMarketPagesAndReturnsToMainWindowMapOrTable(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = Create(store);
        var session = NewShop();
        Attach(vm, session);
        var owner = new MainWindow(vm);
        try
        {
            ShowControlledHost(owner, new(1440, 900));
            vm.OpenShanheCommand.Execute(null);
            var root = (FrameworkElement)owner.Content;
            Layout(root, new(1440, 900));
            var campaign = (ShanheCampaignPanel)owner.FindName("ShanhePanel");
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 900) })
            {
                AssertMapGoldCounter(vm, session, root, campaign, size);
                Program.Render(root, (int)size.Width, (int)size.Height,
                    Path.Combine(output, $"shanhe-map-wallet-{size.Width:0}x{size.Height:0}.png"));
            }
            AssertObjectiveTabVisibility(vm, session, root, campaign);
            var tabs = (TabControl)campaign.FindName("ShanheJourneyTabs");
            var mapIndex = tabs.SelectedIndex;
            var market = (ShanheMarketOverlay)owner.FindName("ShanheMarketHost");
            var shop = (ShanheShopPanel)market.FindName("MarketHost");
            Click((Button)campaign.FindName("ShanheOpenMarketButton"));
            Layout(root, new(1440, 900));
            var marketOverlay = (Border)owner.FindName("ShanheMarketOverlay");
            var gold = ((TextBlock)shop.FindName("MarketGold")).Text;
            var equipment = ((TextBlock)shop.FindName("MarketEquipmentSummary")).Text;
            var focusMatches = ReferenceEquals(FocusSurface(owner, vm), marketOverlay);
            var marketCoversMap = vm.IsShanheMarketOpen && vm.IsShanheOpen && tabs.SelectedIndex == mapIndex &&
                marketOverlay.Visibility == Visibility.Visible && shop.ActualWidth > 0 && focusMatches &&
                gold == vm.ShanheMarketGoldText && equipment == vm.ShanheMarketEquipmentText;
            var diagnostic = Path.Combine(output, "shanhe-market-open-diagnostic-1440x900.png");
            if (!marketCoversMap) Program.Render(root, 1440, 900, diagnostic);
            Program.Assert(marketCoversMap,
                "Opening the bound permanent market covers the current map, owns keyboard shortcuts and publishes the market's own wallet and equipment summary. Actual: " +
                JsonSerializer.Serialize(new
                {
                    vm.IsShanheOpen, vm.IsShanheMarketOpen, ExpectedTabIndex = mapIndex, TabIndex = tabs.SelectedIndex,
                    OverlayVisibility = marketOverlay.Visibility.ToString(), shop.ActualWidth, shop.ActualHeight,
                    Gold = gold, ExpectedGold = vm.ShanheMarketGoldText, Equipment = equipment, ExpectedEquipment = vm.ShanheMarketEquipmentText,
                    FocusSurfaceMatches = focusMatches, FocusSurface = FocusSurface(owner, vm).Name,
                    WindowServiceMatches = ReferenceEquals(Window.GetWindow(marketOverlay), owner),
                    WindowVisualAncestorMatches = ReferenceEquals(VisualWindowAncestor(marketOverlay), owner), DiagnosticImage = diagnostic
                }));
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 900) })
            {
                AssertMarketHealthLayout(vm, session, store, shop, root, size);
                Program.Render(root, (int)size.Width, (int)size.Height,
                    Path.Combine(output, $"shanhe-market-health-map-{size.Width:0}.png"));
            }
            Layout(root, new(1440, 900));
            Program.Render(root, 1440, 900, Path.Combine(output, "shanhe-market-floating-1440x900.png"));
            CheckFiveCardsAndPage(shop, root, new(1440, 900), session, store, output);
            CheckMaterialSummaries(vm, shop, root, new(1440, 900));
            Program.Render(root, 1440, 900, Path.Combine(output, "shanhe-market-floating-1440x900.png"));
            Click((Button)shop.FindName("MarketCloseButton")); Layout(root, new(1440, 900));
            Program.Assert(!vm.IsShanheMarketOpen && vm.IsShanheOpen && tabs.SelectedIndex == mapIndex,
                "The visible close action dismisses only the market and returns to the unchanged underlying map.");

            var battleNode = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == "battle" && node.IsAvailable);
            Program.Assert(vm.SelectShanheNodeCommand.CanExecute(battleNode),
                "The same fresh expedition must expose its next real encounter before checking the table's market entry.");
            var previewJournal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var previewGame = Program.Engine(vm);
            var previewGameJournal = GameCheckpointJson.Serialize(previewGame.CreateCheckpoint());
            vm.PreviewShanheNodeCommand.Execute(battleNode); Layout(root, new(1440, 900));
            Program.Assert(vm.IsShanheEncounterPreviewOpen && vm.ShanheChapterProgressTarget == 0 &&
                    vm.ShanheEncounterRouteHint.Length == 0 &&
                    ((Border)campaign.FindName("ShanheEncounterRouteBanner")).Visibility == Visibility.Collapsed &&
                    ((TextBlock)campaign.FindName("ShanheEncounterRouteHint")).Text.Length == 0,
                "The actual legacy merchant route keeps its encounter preview without inventing a six-step or optional-battle explanation.");
            vm.CloseShanheEncounterPreviewCommand.Execute(null); Layout(root, new(1440, 900));
            Program.Assert(!vm.IsShanheEncounterPreviewOpen && ReferenceEquals(Program.Engine(vm), previewGame) &&
                    GameCheckpointJson.Serialize(previewGame.CreateCheckpoint()) == previewGameJournal &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == previewJournal,
                "Opening and closing the legacy route hint is read-only before the same real encounter starts.");
            vm.SelectShanheNodeCommand.Execute(battleNode); Layout(root, new(1440, 900));
            var tableSurface = FocusSurface(owner, vm); var revision = Program.Engine(vm).Revision;
            Program.Assert(vm.IsShanheBattleActive && vm.IsShanheBattleSession && !vm.IsShanheOpen && !vm.IsNewGameSetupOpen &&
                ReferenceEquals(tableSurface, owner.FindName("TableSurface")),
                "Entering the encounter must establish the active campaign table and dismiss the initial lobby before its real entry can be clicked.");
            Click((Button)owner.FindName("ShanheMarketButton")); Layout(root, new(1440, 900));
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 900) })
            {
                AssertMarketHealthLayout(vm, session, store, shop, root, size);
                Program.Render(root, (int)size.Width, (int)size.Height,
                    Path.Combine(output, $"shanhe-market-health-battle-{size.Width:0}.png"));
            }
            Program.Assert(vm.IsShanheMarketOpen && !vm.IsShanheOpen &&
                TableShortcutChecks.Input(owner, Key.Escape) && !vm.IsShanheMarketOpen && !vm.IsShanheOpen &&
                ReferenceEquals(FocusSurface(owner, vm), tableSurface) && Program.Engine(vm).Revision == revision,
                "The active campaign table's permanent market entry and Escape return to that same table surface without opening the map or submitting a battle command.");

            vm.OpenShanheCommand.Execute(null);
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 900) })
            {
                AssertMapGoldCounter(vm, session, root, campaign, size);
                Program.Render(root, (int)size.Width, (int)size.Height,
                    Path.Combine(output, $"shanhe-map-wallet-resume-{size.Width:0}x{size.Height:0}.png"));
            }
            vm.OpenShanheMarketCommand.Execute(null);
            Layout(root, new(1120, 780));
            CheckFiveCardsAndPage(shop, root, new(1120, 780), session, store, output);
            Program.Render(root, 1120, 780, Path.Combine(output, "shanhe-market-floating-1120x780-fifth.png"));
            Program.Assert(TableShortcutChecks.Input(owner, Key.Escape) && !vm.IsShanheMarketOpen && vm.IsShanheOpen &&
                tabs.SelectedIndex == mapIndex && ReferenceEquals(FocusSurface(owner, vm), owner.FindName("ShanheOverlay")) &&
                Program.Engine(vm).Revision == revision,
                "At the minimum main-window size, Escape returns from the actual merchant to the same map without changing its live campaign battle.");
            var sameBattle = Program.Engine(vm);
            var campaignJournal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var battleJournal = GameCheckpointJson.Serialize(sameBattle.CreateCheckpoint());
            Click((Button)campaign.FindName("ShanheResumeBattleButton")); Layout(root, new(1120, 780));
            Program.Assert(!vm.IsShanheOpen && vm.IsShanheBattleSession && ReferenceEquals(Program.Engine(vm), sameBattle) &&
                    ReferenceEquals(FocusSurface(owner, vm), tableSurface) && sameBattle.Revision == revision &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == campaignJournal &&
                    GameCheckpointJson.Serialize(sameBattle.CreateCheckpoint()) == battleJournal,
                "The fixed toolbar's real resume action returns to the same active battle and table without replaying, spending or issuing either game's command.");
        }
        finally { owner.Close(); }
    }

    private static void CheckFiveCardsAndPage(ShanheShopPanel shop, FrameworkElement root, Size size,
        ShanheCampaignSession session, MemoryShanheCampaignSaveStore store, string output)
    {
        Layout(root, size);
        var vm = (MainViewModel)shop.DataContext;
        Program.Assert(vm.ShanheMarketGoods.Count() == 5, "The real fixed merchant fixture must publish all five goods in its selected category.");
        var artwork = Program.Find<Image>(shop).ToArray();
        Program.Assert(vm.ShanheMarketGoods.All(product => product.TacticImage is not null &&
            artwork.Any(image => ReferenceEquals(image.Source, product.TacticImage))),
            "Every real economic source renders its existing cached catalogue icon rather than a text placeholder. Actual: " +
            JsonSerializer.Serialize(new
            {
                AppContext.BaseDirectory, shop.Visibility, shop.ActualWidth, shop.ActualHeight, ImageCount = artwork.Length,
                Products = vm.ShanheMarketGoods.Select(product => new
                {
                    product.Id, product.Name, HasTacticImage = product.TacticImage is not null,
                    BoundImages = artwork.Where(image => ReferenceEquals(image.DataContext, product)).Select(image => new
                    { HasSource = image.Source is not null, IsOriginal = ReferenceEquals(image.Source, product.TacticImage) })
                })
            }));
        var scroll = (ScrollViewer)shop.FindName("MarketCardsScroll");
        var previous = (Button)shop.FindName("MarketPreviousPageButton");
        var next = (Button)shop.FindName("MarketNextPageButton");
        scroll.ScrollToLeftEnd(); Layout(root, size);
        Program.Assert(scroll.ScrollableWidth > 0 && !previous.IsEnabled && next.IsEnabled,
            "Five readable-width cards overflow the actual viewport and enable the forward page arrow rather than shrinking into a fixed row.");
        var beforeReading = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var savedBeforeReading = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
        var game = Program.Engine(vm);
        var gameBeforeReading = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var marketSnapshot = vm.IsShanheBattleActive ? game.CreateShanheMarketSnapshot(0) ?? session.Snapshot : session.Snapshot;
        foreach (var product in vm.ShanheMarketGoods)
        {
            var source = marketSnapshot.ShopItems.FirstOrDefault(item => item.ChoiceId == product.Id)?.Reward ??
                session.Definition.Rewards.Single(reward => reward.Id == session.Definition.Nodes
                    .Where(node => node.Kind == ShanheNodeKind.Shop).SelectMany(node => node.Choices ?? [])
                    .Single(choice => choice.Id == product.Id).RewardId);
            var rarity = StandardShanheCampaignCatalog.ShopMetadataByRewardId[source.Id].Rarity;
            var quality = rarity switch { 2 => "稀有", 3 => "史诗", 4 => "传说", _ => "普通" };
            var card = Program.Find<Border>(shop).Single(border => ReferenceEquals(border.DataContext, product) && border.ToolTip is ToolTip);
            var resourceHint = Program.Find<TextBlock>(shop).Single(text => text.Name == "MarketResourceHint" && ReferenceEquals(text.DataContext, product));
            var productGrid = (Grid)resourceHint.Parent;
            Program.Assert(!product.HasResourceHint && product.ResourceHint.Length == 0 &&
                    resourceHint.Text == product.ResourceHint && resourceHint.Visibility == Visibility.Collapsed && resourceHint.FontSize >= 12 &&
                    Grid.GetRow(resourceHint) == 2 && productGrid.RowDefinitions.Count == 3 &&
                    Math.Abs(productGrid.RowDefinitions[1].ActualHeight - 60) <= 1 &&
                    Math.Abs(productGrid.RowDefinitions[2].ActualHeight - 36) <= 1,
                "The actually funded zero-health-cost products retain a separate reserved resource-hint row without inventing any shortage or changing the original sixty-pixel purchase row.");
            var shared = Program.Find<ShanheRewardChoiceCard>(card).Single();
            var frame = (Image)shared.FindName("FrameArtworkImage");
            var nativeFrameBounds = Bounds(frame, card);
            Program.Assert(product.Rarity == rarity && shared.Rarity == rarity &&
                    shared.Heading == quality + " · 战法" && shared.Title == product.DisplayName && shared.Description == product.ShopSummary &&
                    frame.Source is BitmapSource { PixelWidth: 500, PixelHeight: 768 } &&
                    ReferenceEquals(frame.Source, WushuangArt.Get($"rogue_award_quality_bg_{rarity}.png")) &&
                    frame.IsVisible && PresentationSource.FromVisual(frame) is HwndSource &&
                    Math.Abs(nativeFrameBounds.Width - 258) <= 1 && Math.Abs(nativeFrameBounds.Height - 396.288) <= 1 &&
                    Math.Abs(nativeFrameBounds.Width / nativeFrameBounds.Height - 500d / 768) < .002 &&
                    Program.Find<Image>(shared).Any(image => ReferenceEquals(image.Source, product.TacticImage) && image.IsVisible &&
                        image.ActualWidth > 0 && image.ActualHeight > 0),
                "Every real economic product retains its original catalogue illustration and quality in the same unwarped native reward frame, without compressing the fixed-width market row: " +
                JsonSerializer.Serialize(new { product.Id, product.Name, product.Rarity, ExpectedRarity = rarity, shared.Heading,
                    Bounds = nativeFrameBounds.ToString(), FrameSize = frame.RenderSize.ToString(), CardSize = card.RenderSize.ToString() }));
        }
        CheckMarketTrackPaging(scroll, root, size);
        var summaries = Program.Find<TextBlock>(shop).Where(block =>
            vm.ShanheMarketGoods.Any(product => product.ShopSummary == block.Text)).ToArray();
        Program.Assert(vm.ShanheMarketGoods.All(product => summaries.Any(block => block.Text == product.ShopSummary)) &&
            summaries.All(block => block.TextWrapping == TextWrapping.Wrap),
            "The actual card body renders its core effect without repeated acquisition or implementation notes.");
        CheckRuleTooltip(shop, vm.ShanheMarketGoods.First(), root);
        CheckMarketKeyboardReading(vm.ShanheMarketGoods.First(), shop, root, size, output);
        for (var page = 0; page < 8 && next.IsEnabled; page++) { Click(next); Layout(root, size); }
        var fifth = vm.ShanheMarketGoods.Last();
        var buy = Program.Find<Button>(shop).Single(button => ReferenceEquals(button.CommandParameter, fifth));
        var bounds = Bounds(buy, scroll);
        Program.Assert(!next.IsEnabled && previous.IsEnabled && scroll.HorizontalOffset > 0 &&
            bounds.Left >= -1 && bounds.Right <= scroll.ViewportWidth + 1 &&
            bounds.Top >= -1 && bounds.Bottom <= scroll.ViewportHeight + 1,
            "Actual forward-arrow clicks reach the fifth product and keep its original bound purchase control fully inside the viewport.");
        var fifthFace = Program.Find<Border>(shop).Single(border => ReferenceEquals(border.DataContext, fifth) && border.ToolTip is ToolTip);
        var fifthFrame = (Image)Program.Find<ShanheRewardChoiceCard>(fifthFace).Single().FindName("FrameArtworkImage");
        var frameBounds = Bounds(fifthFrame, scroll);
        Program.Assert(frameBounds.Left >= -1 && frameBounds.Top >= -1 && frameBounds.Right <= scroll.ViewportWidth + 1 &&
                frameBounds.Bottom <= scroll.ViewportHeight + 1 && !frameBounds.IntersectsWith(bounds) &&
                ReferenceEquals(buy.Command, vm.RequestShanheShopPurchaseCommand) && ReferenceEquals(buy.CommandParameter, fifth) &&
                Program.Find<TextBlock>(buy).Any(text => text.Text == fifth.PriceText) &&
                Program.Find<TextBlock>(buy).Any(text => text.Text == fifth.Status),
            "Paging to the final real product keeps its entire native card and separate unchanged purchase control visible without overlapping the illustration.");
        foreach (var text in Program.Find<TextBlock>(buy).Where(text => !string.IsNullOrWhiteSpace(text.Text)))
            AssertTextGlyphsWithin(text, buy, "The actual paged market purchase price and status", checkAncestorClips: true);
        var fifthHint = Program.Find<TextBlock>(shop).Single(text => text.Name == "MarketResourceHint" && ReferenceEquals(text.DataContext, fifth));
        var fifthProductGrid = (Grid)fifthHint.Parent;
        var hintRow = fifthProductGrid.RowDefinitions[2];
        var hintBounds = fifthProductGrid.TransformToAncestor(scroll).TransformBounds(new Rect(0,
            fifthProductGrid.RowDefinitions.Take(2).Sum(row => row.ActualHeight), fifthProductGrid.ActualWidth, hintRow.ActualHeight));
        Program.Assert(hintBounds.Left >= -1 && hintBounds.Top >= bounds.Bottom - 1 &&
                hintBounds.Right <= scroll.ViewportWidth + 1 && hintBounds.Bottom <= scroll.ViewportHeight + 1 &&
                !hintBounds.IntersectsWith(frameBounds),
            "The last actual product's reserved resource-hint row stays below its complete purchase control and inside the real minimum-width viewport.");
        Click(previous); Layout(root, size);
        Program.Assert(next.IsEnabled, "The backward page arrow can return from the last product.");
        scroll.ScrollToRightEnd(); Layout(root, size);
        Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == beforeReading &&
                (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == savedBeforeReading &&
                ReferenceEquals(Program.Engine(vm), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == gameBeforeReading,
            "Native card rendering, track paging, keyboard rule reading and focus traversal leave the campaign, save and current ordinary or active battle journals untouched.");
    }

    private static void AssertMarketHealthLayout(MainViewModel vm, ShanheCampaignSession session,
        MemoryShanheCampaignSaveStore store, ShanheShopPanel shop, FrameworkElement root, Size size)
    {
        var campaignBefore = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var savedBefore = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
        var game = Program.Engine(vm);
        var gameBefore = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var expected = vm.IsShanheBattleActive ? game.CreateShanheMarketSnapshot(0)! : session.Snapshot;
        Layout(root, size);
        var gold = (TextBlock)shop.FindName("MarketGold");
        var health = (TextBlock)shop.FindName("MarketHealth");
        var close = (Button)shop.FindName("MarketCloseButton");
        var equipment = (TextBlock)shop.FindName("MarketEquipmentSummary");
        var goldFace = (FrameworkElement)gold.Parent;
        var healthFace = (FrameworkElement)health.Parent;
        var goldBounds = Bounds(goldFace, root);
        var healthBounds = Bounds(healthFace, root);
        var closeBounds = Bounds(close, root);
        var equipmentBounds = Bounds(equipment, root);
        Program.Assert(vm.IsShanheMarketOpen && health.IsVisible && gold.IsVisible && close.IsVisible &&
                health.FontSize >= 12 && health.Text == $"体力  {expected.Hp}/{expected.MaxHp}" &&
                health.Text == vm.ShanheMarketHealthText && gold.Text == $"铜币  {expected.Gold}" && gold.Text == vm.ShanheMarketGoldText &&
                BindingOperations.GetBinding(health, TextBlock.TextProperty)?.Path.Path == nameof(MainViewModel.ShanheMarketHealthText),
            "The actual native market header shows complete readable current health and copper from the current battle's market projection or the actual map session, through its live health binding.");
        var targets = new[] { goldBounds, healthBounds, closeBounds };
        Program.Assert(targets.All(bounds => bounds.Width > 0 && bounds.Height > 0 && bounds.Left >= 0 && bounds.Top >= 0 &&
                    bounds.Right <= size.Width + 1 && bounds.Bottom <= size.Height + 1 && !bounds.IntersectsWith(equipmentBounds)) &&
                !goldBounds.IntersectsWith(healthBounds) && !goldBounds.IntersectsWith(closeBounds) && !healthBounds.IntersectsWith(closeBounds),
            "The automatically sized wallet, health and close targets remain separate and do not overlap the left equipment summary at either native viewport: " +
            JsonSerializer.Serialize(new { Size = size.ToString(), Gold = goldBounds.ToString(), Health = healthBounds.ToString(),
                Close = closeBounds.ToString(), Equipment = equipmentBounds.ToString(), health.Text }));
        AssertTextGlyphsWithin(gold, goldFace, "The native merchant copper wallet", checkAncestorClips: true);
        AssertTextGlyphsWithin(health, healthFace, "The native merchant current-health wallet", checkAncestorClips: true);
        AssertTextGlyphsWithin(equipment, (FrameworkElement)equipment.Parent, "The native merchant equipment summary", checkAncestorClips: true);
        var closeCaption = Program.Find<TextBlock>(close).Single(text => text.Text == close.Content?.ToString());
        AssertTextGlyphsWithin(closeCaption, close, "The separate native merchant close action", checkAncestorClips: true);
        Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == campaignBefore &&
                (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == savedBefore &&
                ReferenceEquals(Program.Engine(vm), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == gameBefore,
            "Reading and laying out the current-health wallet never pays, heals, advances either game or writes the saved expedition.");
    }

    private static void CheckMarketTrackPaging(ScrollViewer scroll, FrameworkElement root, Size size)
    {
        scroll.ScrollToLeftEnd(); Layout(root, size);
        var bar = Program.Find<ScrollBar>(scroll).Single(bar => ReferenceEquals(bar.TemplatedParent, scroll) && bar.Orientation == Orientation.Horizontal);
        bar.ApplyTemplate();
        var track = (Track)bar.Template.FindName("PART_Track", bar);
        Program.Assert(bar.IsVisible && track.IsVisible && track.Thumb is { IsVisible: true } &&
                ReferenceEquals(track.DecreaseRepeatButton?.Command, ScrollBar.PageLeftCommand) &&
                ReferenceEquals(track.IncreaseRepeatButton?.Command, ScrollBar.PageRightCommand),
            "The actual overflowing market shelf keeps a native horizontal thumb and correctly routed backward and forward track-page commands.");
        Invoke(track.IncreaseRepeatButton!);
        var afterForward = scroll.HorizontalOffset;
        Program.Assert(afterForward > 0, "The actual market track's page-right command moves the real product viewport.");
        Invoke(track.DecreaseRepeatButton!);
        Program.Assert(scroll.HorizontalOffset < afterForward,
            "The actual market track's page-left command returns through the same original goods without a trade.");
        scroll.ScrollToLeftEnd(); Layout(root, size);

        void Invoke(RepeatButton button)
        {
            Program.Assert(button.IsEnabled && button.Command is RoutedCommand command && command.CanExecute(button.CommandParameter, button),
                "A real enabled market track button owns its page command before invoking the native route.");
            ((RoutedCommand)button.Command).Execute(button.CommandParameter, button);
            Layout(root, size);
        }
    }

    private static void CheckMarketKeyboardReading(ShanheShopItem product, ShanheShopPanel shop, FrameworkElement root, Size size, string output)
    {
        var owner = (MainWindow)Window.GetWindow(root);
        var card = Program.Find<Border>(shop).Single(border => ReferenceEquals(border.DataContext, product) && border.ToolTip is ToolTip);
        var buy = Program.Find<Button>(shop).Single(button => ReferenceEquals(button.CommandParameter, product));
        var tooltip = (ToolTip)card.ToolTip;
        tooltip.Placement = PlacementMode.AbsolutePoint; tooltip.HorizontalOffset = -30000; tooltip.VerticalOffset = -30000;
        tooltip.Opened += Conceal;
        var handle = new WindowInteropHelper(owner).Handle;
        try
        {
            SetFocus(handle);
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
            Program.Assert(!IsWindowVisible(handle) && GetForegroundWindow() != handle && card.Focusable &&
                    ToolTipService.GetShowsToolTipOnKeyboardFocus(card) == true && card.Focus(),
                "The existing hidden native market host provides a real keyboard-reading target without exposing or activating a desktop window.");
            Layout(root, size);
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
            tooltip.UpdateLayout();
            var rules = Program.Find<TextBlock>(tooltip).Single(text => text.Text == product.ShopTooltip);
            Program.Assert(ReferenceEquals(Keyboard.FocusedElement, card) && tooltip.IsOpen && tooltip.IsVisible &&
                    PresentationSource.FromVisual(tooltip) is HwndSource && rules.IsVisible && rules.Text.Length > 0 &&
                    rules.TextWrapping == TextWrapping.Wrap && rules.ActualWidth > 0 && rules.ActualHeight > 0 &&
                    tooltip.MaxWidth <= 460 && tooltip.ActualWidth is > 0 and <= 460,
                "Focusing the actual product face opens its own complete original rules in the existing bounded tooltip without clicking Buy.");
            foreach (var key in new[] { Key.Enter, Key.Space })
                TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
                {
                    var input = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(card)!, Environment.TickCount, key)
                        { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                    card.RaiseEvent(input);
                    if (!input.Handled) { input.RoutedEvent = Keyboard.KeyDownEvent; card.RaiseEvent(input); }
                    Program.Assert(ReferenceEquals(Keyboard.FocusedElement, card),
                        "Enter and Space on the read-only product face do not activate its separate purchase button.");
                    return true;
                });
            Program.Render(root, (int)size.Width, (int)size.Height, Path.Combine(output, $"shanhe-market-native-keyboard-{size.Width:0}.png"));
            Program.Assert(!TableShortcutChecks.Input(owner, Key.Tab) && card.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)),
                "Tab remains ordinary native focus traversal from product inspection to its separate purchase action.");
            Layout(root, size);
            Program.Assert(ReferenceEquals(Keyboard.FocusedElement, buy) && buy.IsEnabled &&
                    ReferenceEquals(buy.CommandParameter, product) && ReferenceEquals(buy.Command, ((MainViewModel)shop.DataContext).RequestShanheShopPurchaseCommand),
                "Native Tab reaches the same product's independently bound Buy button without executing it.");
        }
        finally
        {
            ((Button)shop.FindName("MarketCloseButton")).Focus();
            tooltip.IsOpen = false;
            tooltip.Opened -= Conceal;
        }

        void Conceal(object sender, RoutedEventArgs args)
        {
            if (PresentationSource.FromVisual(tooltip) is HwndSource source) ShowWindow(source.Handle, 0);
        }
    }

    private static void CheckMaterialSummaries(MainViewModel vm, ShanheShopPanel shop, FrameworkElement root, Size size)
    {
        var limited = new ShanheShopItem("summary-limits", "受限效果", "每轮限一次，弃置一张牌后造成1点伤害，目标不能响应。占用一个额外技能槽。",
            "", 0, "", null, Category: "额外技能");
        Program.Assert(limited.ShopSummary == "每轮限一次，弃置一张牌后造成1点伤害，目标不能响应。",
            "Concise presentation removes repeated slot prose while retaining the actual frequency, discard payment and response restriction.");
        foreach (var category in new[] { "卡牌", "装备" })
        {
            vm.SelectedShanheMarketCategory = category; Layout(root, size);
            var product = vm.ShanheMarketGoods.Single();
            var kind = category == "卡牌" ? CardKind.Slash : CardKind.Crossbow;
            Program.Assert(product.ShopSummary == "获得卡牌" && product.ShopTooltip == CardCatalog.Get(kind).Description &&
                Program.Find<TextBlock>(shop).Any(block => ReferenceEquals(block.DataContext, product) && block.Text == "获得卡牌"),
                "Physical cards and equipment display only the acquisition summary while preserving their actual card effect for hover.");
            CheckRuleTooltip(shop, product, root);
        }
        vm.SelectedShanheMarketCategory = "战法"; Layout(root, size);
    }

    private static void CheckRuleTooltip(ShanheShopPanel shop, ShanheShopItem product, FrameworkElement root)
    {
        var card = Program.Find<Border>(shop).Single(border => ReferenceEquals(border.DataContext, product) && border.ToolTip is ToolTip);
        var tooltip = (ToolTip)card.ToolTip;
        tooltip.PlacementTarget = card;
        tooltip.Opened += ConcealNativePopup;
        try
        {
            tooltip.IsOpen = true;
            tooltip.ApplyTemplate();
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
            tooltip.UpdateLayout();
            var scroll = (ScrollViewer)tooltip.Content;
            var rule = (TextBlock)scroll.Content;
            var actualRule = Program.Find<TextBlock>(tooltip).Any(block => ReferenceEquals(block, rule));
            Program.Assert(rule.Text == product.ShopTooltip && rule.Text.Length > 0 && rule.TextWrapping == TextWrapping.Wrap &&
                tooltip.IsOpen && tooltip.IsVisible && System.Windows.PresentationSource.FromVisual(tooltip) is HwndSource &&
                tooltip.ContentTemplate is null && actualRule && tooltip.ActualWidth is > 0 and <= 460 &&
                tooltip.MaxWidth <= 460 && scroll.MaxHeight <= 480 && scroll.ActualHeight is > 0 and <= 480 &&
                scroll.ViewportWidth > 0 && rule.ActualWidth <= scroll.ViewportWidth + 1 &&
                (rule.ActualHeight <= scroll.ViewportHeight + 1 || scroll.ScrollableHeight > 0) &&
                scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto,
                "Opening the actual card tooltip exposes the complete native rule in a bounded wrapping and scrollable surface. Actual: " +
                JsonSerializer.Serialize(new
                {
                    product.Id, ExpectedText = product.ShopTooltip, ActualText = rule.Text,
                    DataContextMatches = ReferenceEquals(tooltip.DataContext, product),
                    PlacementTargetMatches = ReferenceEquals(tooltip.PlacementTarget, card),
                    tooltip.IsOpen, tooltip.IsVisible, HasPopupHost = System.Windows.PresentationSource.FromVisual(tooltip) is not null,
                    HasContentTemplate = tooltip.ContentTemplate is not null, ActualRuleInVisualTree = actualRule,
                    TooltipWidth = tooltip.ActualWidth, TooltipHeight = tooltip.ActualHeight,
                    ScrollWidth = scroll.ActualWidth, ScrollHeight = scroll.ActualHeight,
                    scroll.ViewportWidth, scroll.ViewportHeight, scroll.ExtentWidth, scroll.ExtentHeight,
                    scroll.ScrollableHeight, RuleWidth = rule.ActualWidth, RuleHeight = rule.ActualHeight,
                    Wrapping = rule.TextWrapping.ToString(), Scrollbar = scroll.VerticalScrollBarVisibility.ToString()
                }));
        }
        finally { tooltip.IsOpen = false; tooltip.Opened -= ConcealNativePopup; }

        void ConcealNativePopup(object sender, RoutedEventArgs args)
        {
            if (System.Windows.PresentationSource.FromVisual(tooltip) is HwndSource source)
                ShowWindow(source.Handle, 0);
        }
    }

    private static MainViewModel Create(MemoryShanheCampaignSaveStore? campaignStore = null) => new(false, 83, true, new MemorySaveStore(), useExpandedContent: true,
        preferencesStore: new MemoryPlayerPreferencesStore(),
        shanheSaveStore: campaignStore ?? new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
        { IsSoundEnabled = false, IsMusicEnabled = false, IsMotionEnabled = false };
    private static ShanheCampaignSession NewShop()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheWealthPackage());
        int[] sourceIds = [2090, 2146, 2291, 2292, 2181];
        var sources = sourceIds.Select(id => StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(source => source.SourceId == id)).ToArray();
        var materials = new[] { "codex-card-4001", "codex-card-4062" }.Select(id =>
        {
            var face = StandardShanheCampaignCatalog.ShopMetadataByRewardId[id];
            var card = new ShanheStartingCard(face.CardKind!.Value, face.Suit!.Value, face.Rank!.Value);
            return new ShanheRewardDefinition(id, CardCatalog.Get(card.Kind).DisplayName,
                StartingCards: EquipmentCatalog.IsEquipment(card.Kind) ? null : [card],
                StartingEquipment: EquipmentCatalog.IsEquipment(card.Kind) ? [card] : null)
                { Description = CardCatalog.Get(card.Kind).Description, Rarity = face.Rarity };
        });
        var rewards = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Description = source.Description, Rarity = source.Rarity }).Concat(materials).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:responsive-market", "集市布局", ["standard:guan-yu"], ["shop"],
            [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices: rewards.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                    GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[reward.Id].Price,
                    RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray()),
             new("battle", "下一关", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("standard:zhang-fei")])])],
            rewards, StartingGold: 1_000_000);
        var session = ShanheCampaignSession.Create(definition, new(83, "standard:guan-yu")
            { UseCodexRewardPool = true, PersistentMarket = true,
              UnlockedRewardIds = rewards.Select(reward => reward.Id).ToArray() }, _ => registry);
        Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.Gold == 1_000_000 && session.Snapshot.ShopItems.Count == 7 &&
            session.Snapshot.ShopItems.Count(product => product.Reward?.CampaignEconomy is not null) == 5,
            "A new permanent-market expedition publishes five real economic tactics and two native material faces while leaving the map open.");
        return session;
    }

    private static void AssertMapGoldCounter(MainViewModel vm, ShanheCampaignSession session,
        FrameworkElement root, ShanheCampaignPanel campaign, Size size)
    {
        var before = session.Snapshot;
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var ordinary = Program.Engine(vm);
        var ordinaryJournal = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        var expectedGold = vm.IsShanheBattleActive ? ordinary.CreateShanheMarketSnapshot(0)?.Gold ?? before.Gold : before.Gold;
        Layout(root, size);
        AssertObjectiveBanner(vm, root, campaign);
        var toolbar = (StackPanel)campaign.FindName("ShanheMapToolbar");
        var counter = (Border)campaign.FindName("ShanheMapGoldCounter");
        var value = (TextBlock)campaign.FindName("ShanheMapGoldValue");
        var counterBounds = Bounds(counter, root);
        var valueBounds = Bounds(value, counter);
        var coin = Program.Find<Image>(counter).Single();
        var lastCharacter = value.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
        var lastBounds = value.TransformToAncestor(counter).TransformBounds(lastCharacter);
        Program.Assert(toolbar.IsVisible && toolbar.IsEnabled && toolbar.IsHitTestVisible && counter.IsVisible &&
                !counter.IsHitTestVisible && !counter.Focusable && value.Text == vm.ShanheGoldText &&
                value.Text == $"铜币   {expectedGold}" && value.FontSize == 18 &&
                counter.ActualHeight >= 35 && counter.ActualHeight <= 37 &&
                counterBounds.Width >= 139 && counterBounds.Width <= 217 && new Rect(root.RenderSize).Contains(counterBounds) &&
                valueBounds.Width > 0 && valueBounds.Height > 0 && valueBounds.Left >= -.7 && valueBounds.Top >= -.7 &&
                valueBounds.Right <= counter.ActualWidth + .7 && valueBounds.Bottom <= counter.ActualHeight + .7 &&
                !lastCharacter.IsEmpty && lastBounds.Top >= -.7 && lastBounds.Bottom <= counter.ActualHeight + .7 &&
                lastBounds.Left >= -.7 && lastBounds.Right <= counter.ActualWidth + .7 &&
                coin.Width == 28 && coin.Height == 28 && coin.Source is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
                coin.Source.ToString()!.EndsWith("/14021801.png", StringComparison.Ordinal),
            $"The actual map keeps its source coin and complete million-copper wallet readable at {size}; counter={counterBounds}, text={valueBounds}, last={lastBounds}, actual={value.Text}.");
        var protectedElements = new[] { "ShanheChapterObjectiveBanner", "ShanheChapterHeader", "ShanheRulesButton", "ShanheOpenMarketButton", "ShanheCloseButton", "ShanheEndButton", "ShanheResumeBattleButton" }
            .Select(name => (FrameworkElement)campaign.FindName(name)).Where(element => element.IsVisible).ToArray();
        Program.Assert(protectedElements.All(element => !counterBounds.IntersectsWith(Bounds(element, root))) &&
                Program.Find<Button>(campaign).Where(button => button.Name == "ShanheMapMarketButton" && button.IsVisible)
                    .All(button => !counterBounds.IntersectsWith(Bounds(button, root))),
            "The fixed wallet stays separate from the chapter objective, title and every fixed map/header action.");
        var resume = (Button)campaign.FindName("ShanheResumeBattleButton");
        if (resume.IsVisible)
        {
            var resumeBounds = Bounds(resume, root);
            var hit = root.InputHitTest(resume.TranslatePoint(new Point(resume.ActualWidth / 2, resume.ActualHeight / 2), root)) as DependencyObject;
            Program.Assert(resume.IsEnabled && ReferenceEquals(resume.Command, vm.ResumeShanheBattleCommand) &&
                    resume.Command?.CanExecute(resume.CommandParameter) == true && new Rect(root.RenderSize).Contains(resumeBounds) &&
                    !resumeBounds.IntersectsWith(Bounds((FrameworkElement)campaign.FindName("ShanheChapterObjectiveBanner"), root)) &&
                    !resumeBounds.IntersectsWith(Bounds((FrameworkElement)campaign.FindName("ShanheChapterHeader"), root)) &&
                    !resumeBounds.IntersectsWith(counterBounds) && hit is not null && (ReferenceEquals(hit, resume) || resume.IsAncestorOf(hit)),
                "The active battle's fixed resume button stays fully visible, separate from the objective/title/wallet, and receives its own input hit.");
        }
        foreach (var point in new[]
        {
            counter.TranslatePoint(new Point(counter.ActualWidth / 2, counter.ActualHeight / 2), root),
            counter.TranslatePoint(new Point(-6, counter.ActualHeight / 2), root)
        })
        {
            var hit = root.InputHitTest(point) as DependencyObject;
            Program.Assert(hit is null || !ReferenceEquals(hit, toolbar) && !toolbar.IsAncestorOf(hit),
                "The read-only wallet and blank toolbar edge pass pointer hits to the existing map instead of forming an invisible action surface.");
        }
        Program.Assert(ReferenceEquals(session.Snapshot, before) && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                ReferenceEquals(Program.Engine(vm), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
            "Reading and laying out the map wallet submits no campaign or battle command and changes neither checkpoint.");
    }

    private static void AssertObjectiveBanner(MainViewModel vm, FrameworkElement root, ShanheCampaignPanel campaign)
    {
        var banner = (Border)campaign.FindName("ShanheChapterObjectiveBanner");
        var text = (TextBlock)campaign.FindName("ShanheChapterObjective");
        var tabs = (TabControl)campaign.FindName("ShanheJourneyTabs");
        var bannerBounds = Bounds(banner, root);
        var mapBounds = Bounds((ScrollViewer)campaign.FindName("ShanheMapScroll"), root);
        var tabBounds = tabs.Items.Cast<TabItem>().Select(tab => Bounds(tab, root)).ToArray();
        var actions = new[] { "ShanheChapterHeader", "ShanheRulesButton", "ShanheOpenMarketButton", "ShanheCloseButton",
            "ShanheEndButton", "ShanheMapToolbar" }.Select(name => (FrameworkElement)campaign.FindName(name))
            .Where(element => element.IsVisible).ToArray();
        Program.Assert(banner.IsVisible && !banner.IsHitTestVisible && !banner.Focusable && text.IsVisible &&
                PresentationSource.FromVisual(text) is HwndSource && text.Text == vm.ShanheChapterObjectiveText && text.FontSize >= 14 &&
                new Rect(root.RenderSize).Contains(bannerBounds) && !bannerBounds.IntersectsWith(mapBounds) &&
                tabBounds.Length == 4 && tabBounds.All(bounds => !bounds.IntersectsWith(bannerBounds)) &&
                actions.All(element => !Bounds(element, root).IntersectsWith(bannerBounds)),
            $"The complete read-only objective stays in the vacant tab-header band, separate from all four tabs, map pixels and fixed actions: banner={bannerBounds}, map={mapBounds}, tabs={string.Join(';', tabBounds)}.");
        AssertTextGlyphsWithin(text, banner, "The native map objective");
        var foreground = text.Foreground as SolidColorBrush;
        var background = banner.Background as SolidColorBrush;
        Program.Assert(foreground is not null && background is not null,
            "The objective's actual foreground and backdrop must provide measurable contrast.");
        // Linear alpha over pure white is a conservative luminance upper bound
        // for this dark banner over any normal source image or header pixel.
        var backdropAlpha = background!.Color.A / 255d * background.Opacity;
        var localBackdropLuminance = Luminance(background.Color) * backdropAlpha + 1 - backdropAlpha;
        var opacity = 1d;
        for (DependencyObject? ancestor = banner; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
        {
            if (ancestor is UIElement element) opacity *= element.Opacity;
            if (ReferenceEquals(ancestor, root)) break;
        }
        var backdropLuminance = localBackdropLuminance * opacity + 1 - opacity;
        var textAlpha = foreground!.Color.A / 255d * foreground.Opacity * text.Opacity;
        var textLuminance = (Luminance(foreground.Color) * textAlpha + localBackdropLuminance * (1 - textAlpha)) * opacity + 1 - opacity;
        var contrast = (textLuminance + .05) / (backdropLuminance + .05);
        Program.Assert(textLuminance > backdropLuminance && contrast >= 7,
            $"The actual objective brushes retain at least 7:1 contrast even over the brightest possible image: foreground={foreground.Color}, background={background.Color}, opacity={opacity:R}, contrast={contrast:R}.");
        var hit = root.InputHitTest(banner.TranslatePoint(new Point(banner.ActualWidth / 2, banner.ActualHeight / 2), root)) as DependencyObject;
        Program.Assert(hit is null || !ReferenceEquals(hit, banner) && !banner.IsAncestorOf(hit),
            "The objective's entire decorated background remains transparent to input.");

        static double Luminance(Color color)
        {
            static double Linear(byte channel) { var value = channel / 255d; return value <= .04045 ? value / 12.92 : Math.Pow((value + .055) / 1.055, 2.4); }
            return .2126 * Linear(color.R) + .7152 * Linear(color.G) + .0722 * Linear(color.B);
        }
    }

    private static void AssertObjectiveTabVisibility(MainViewModel vm, ShanheCampaignSession session, FrameworkElement root, ShanheCampaignPanel campaign)
    {
        var tabs = (TabControl)campaign.FindName("ShanheJourneyTabs");
        var banner = (Border)campaign.FindName("ShanheChapterObjectiveBanner");
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var game = Program.Engine(vm);
        var gameJournal = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        try
        {
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 900) })
            foreach (var index in new[] { 1, 3 })
            {
                tabs.SelectedIndex = index; Layout(root, size);
                Program.Assert(tabs.SelectedIndex == index && banner.Visibility == Visibility.Collapsed && !vm.IsShanheMarketOpen,
                    "The map objective hides on the codex and journey-history pages instead of covering those pages' headings.");
                tabs.SelectedIndex = 0; Layout(root, size);
                AssertObjectiveBanner(vm, root, campaign);
            }
        }
        finally { tabs.SelectedIndex = 0; Layout(root, new(1440, 900)); }
        Program.Assert(ReferenceEquals(Program.Engine(vm), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == gameJournal &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal,
            "Changing only map, codex and history presentation never spends resources or submits a campaign or battle command.");
    }

    internal static void AssertTextGlyphsWithin(TextBlock text, FrameworkElement container, string context, bool checkAncestorClips = false)
    {
        var glyphs = new List<Rect>();
        var clipped = new List<string>();
        var cursor = text.ContentStart.GetInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
        for (var examined = 0; cursor is not null && examined <= text.Text.Length + 2; examined++)
        {
            var next = cursor.GetNextInsertionPosition(System.Windows.Documents.LogicalDirection.Forward);
            if (next is null) break;
            var characterText = new System.Windows.Documents.TextRange(cursor, next).Text;
            var before = cursor.GetCharacterRect(System.Windows.Documents.LogicalDirection.Forward);
            var after = next.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
            cursor = next;
            if (string.IsNullOrWhiteSpace(characterText) || before.IsEmpty || after.IsEmpty ||
                Math.Abs(before.Top - after.Top) > .7 || Math.Abs(before.Left - after.Left) < 1) continue;
            var glyph = new Rect(Math.Min(before.Left, after.Left), before.Top, Math.Abs(after.Left - before.Left), before.Height);
            glyphs.Add(text.TransformToAncestor(container).TransformBounds(glyph));
            if (checkAncestorClips)
                for (DependencyObject? ancestor = text; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
                {
                    if (ancestor is not FrameworkElement element) continue;
                    var bounds = ReferenceEquals(text, element) ? glyph : text.TransformToAncestor(element).TransformBounds(glyph);
                    var layoutClip = LayoutInformation.GetLayoutClip(element);
                    foreach (var clip in new[] { layoutClip, element.Clip }.Where(clip => clip is not null))
                        if (bounds.Left < clip!.Bounds.Left - .7 || bounds.Top < clip.Bounds.Top - .7 ||
                            bounds.Right > clip.Bounds.Right + .7 || bounds.Bottom > clip.Bounds.Bottom + .7)
                            clipped.Add(element.GetType().Name + "/" + element.Name + ": glyph=" + bounds + ", clip=" + clip.Bounds);
                }
        }
        var epsilon = 1 / VisualTreeHelper.GetDpi(container).DpiScaleX;
        Program.Assert(text.Visibility == Visibility.Visible && text.ActualWidth > 0 && text.ActualHeight > 0 &&
                glyphs.Count == text.Text.Count(character => !char.IsWhiteSpace(character)) && glyphs.Count > 0 &&
                glyphs.All(glyph => glyph.Left >= -epsilon && glyph.Top >= -epsilon &&
                    glyph.Right <= container.ActualWidth + epsilon && glyph.Bottom <= container.ActualHeight + epsilon) && clipped.Count == 0,
            context + " renders every complete character inside its actual viewport: " + JsonSerializer.Serialize(new
                { text.Text, text.RenderSize, Container = container.RenderSize, Glyphs = glyphs.Select(glyph => glyph.ToString()).ToArray(), clipped }));
    }
    private static void Attach(MainViewModel vm, ShanheCampaignSession session)
    {
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, null);
    }
    private static FrameworkElement FocusSurface(MainWindow window, MainViewModel vm) =>
        (FrameworkElement)typeof(MainWindow).GetMethod("CurrentFocusSurface", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(window, [vm])!;
    private static Window? VisualWindowAncestor(DependencyObject element)
    {
        for (var current = System.Windows.Media.VisualTreeHelper.GetParent(element); current is not null;
             current = System.Windows.Media.VisualTreeHelper.GetParent(current))
            if (current is Window window) return window;
        return null;
    }
    private static Rect Bounds(FrameworkElement element, FrameworkElement ancestor) => ReferenceEquals(element, ancestor)
        ? new Rect(element.RenderSize) : element.TransformToAncestor(ancestor).TransformBounds(new Rect(element.RenderSize));
    private static void Fills(FrameworkElement element, FrameworkElement ancestor)
    {
        var bounds = Bounds(element, ancestor);
        Program.Assert(Math.Abs(bounds.Left) <= 1 && Math.Abs(bounds.Top) <= 1 &&
            Math.Abs(bounds.Width - ancestor.ActualWidth) <= 1 && Math.Abs(bounds.Height - ancestor.ActualHeight) <= 1,
            "The campaign fills the entire available client rectangle at this aspect ratio without an outer fixed-size or uniform-scaling gap.");
    }
    private static void Click(Button button)
    {
        Program.Assert(button.IsEnabled && button.Command?.CanExecute(button.CommandParameter) != false,
            "The actual layout control must be available before its click.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
    private static void Layout(FrameworkElement root, Size size)
    {
        if (Window.GetWindow(root) is { IsLoaded: true } window)
        {
            // A real Window may re-arrange its content during the Render pump.
            // Keep the screenshot and popup targets at the requested client size.
            root.Width = size.Width; root.Height = size.Height;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.UpdateLayout();
        }
        root.Measure(size); root.Arrange(new Rect(new Point(), size)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.Render); root.UpdateLayout();
    }

    private static void ShowControlledHost(Window window, Size clientSize)
    {
        window.ShowActivated = false; window.ShowInTaskbar = false;
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.Left = -30000; window.Top = -30000;
        var root = (FrameworkElement)window.Content;
        root.Width = clientSize.Width; root.Height = clientSize.Height;
        window.SizeToContent = SizeToContent.WidthAndHeight;
        var handle = new WindowInteropHelper(window).EnsureHandle();
        const int extendedStyle = -20;
        const int noActivate = 0x08000000;
        SetWindowLong(handle, extendedStyle, GetWindowLong(handle, extendedStyle) | noActivate);
        var previousParent = SetParent(handle, new nint(-3));
        var parentError = Marshal.GetLastPInvokeError();
        Program.Assert((previousParent != 0 || parentError == 0) && (GetWindowLong(handle, extendedStyle) & noActivate) != 0,
            "The market fixture must own a message-only, non-activating HWND before loading its actual Window.");
        window.Show(); ShowWindow(handle, 0);
        window.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
        Layout(root, clientSize);
        Program.Assert(window.IsLoaded && window.IsVisible && root.IsVisible &&
            (System.Windows.PresentationSource.FromVisual(root) as HwndSource)?.Handle == handle &&
            !IsWindowVisible(handle) && GetForegroundWindow() != handle,
            "The market fixture must load its real WPF source without exposing or activating its native desktop window.");
    }

    [DllImport("user32.dll", SetLastError = true)] private static extern nint SetParent(nint child, nint parent);
    [DllImport("user32.dll")] private static extern bool ShowWindow(nint handle, int command);
    [DllImport("user32.dll")] private static extern bool IsWindowVisible(nint handle);
    [DllImport("user32.dll")] private static extern nint GetForegroundWindow();
    [DllImport("user32.dll")] private static extern nint SetFocus(nint handle);
    [DllImport("user32.dll", EntryPoint = "GetWindowLongW")] private static extern int GetWindowLong(nint handle, int index);
    [DllImport("user32.dll", EntryPoint = "SetWindowLongW")] private static extern int SetWindowLong(nint handle, int index, int value);
}

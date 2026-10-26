using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
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
        using var vm = Create();
        Attach(vm, NewShop());
        var owner = new MainWindow(vm);
        try
        {
            ShowControlledHost(owner, new(1440, 900));
            vm.OpenShanheCommand.Execute(null);
            var root = (FrameworkElement)owner.Content;
            Layout(root, new(1440, 900));
            var campaign = (ShanheCampaignPanel)owner.FindName("ShanhePanel");
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
            Program.Render(root, 1440, 900, Path.Combine(output, "shanhe-market-floating-1440x900.png"));
            CheckFiveCardsAndPage(shop, root, new(1440, 900));
            CheckMaterialSummaries(vm, shop, root, new(1440, 900));
            Program.Render(root, 1440, 900, Path.Combine(output, "shanhe-market-floating-1440x900.png"));
            Click((Button)shop.FindName("MarketCloseButton")); Layout(root, new(1440, 900));
            Program.Assert(!vm.IsShanheMarketOpen && vm.IsShanheOpen && tabs.SelectedIndex == mapIndex,
                "The visible close action dismisses only the market and returns to the unchanged underlying map.");

            var battleNode = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == "battle" && node.IsAvailable);
            Program.Assert(vm.SelectShanheNodeCommand.CanExecute(battleNode),
                "The same fresh expedition must expose its next real encounter before checking the table's market entry.");
            vm.SelectShanheNodeCommand.Execute(battleNode); Layout(root, new(1440, 900));
            var tableSurface = FocusSurface(owner, vm); var revision = Program.Engine(vm).Revision;
            Program.Assert(vm.IsShanheBattleActive && vm.IsShanheBattleSession && !vm.IsShanheOpen && !vm.IsNewGameSetupOpen &&
                ReferenceEquals(tableSurface, owner.FindName("TableSurface")),
                "Entering the encounter must establish the active campaign table and dismiss the initial lobby before its real entry can be clicked.");
            Click((Button)owner.FindName("ShanheMarketButton")); Layout(root, new(1440, 900));
            Program.Assert(vm.IsShanheMarketOpen && !vm.IsShanheOpen &&
                TableShortcutChecks.Input(owner, Key.Escape) && !vm.IsShanheMarketOpen && !vm.IsShanheOpen &&
                ReferenceEquals(FocusSurface(owner, vm), tableSurface) && Program.Engine(vm).Revision == revision,
                "The active campaign table's permanent market entry and Escape return to that same table surface without opening the map or submitting a battle command.");

            vm.OpenShanheCommand.Execute(null);
            Layout(root, new(1120, 780));
            vm.OpenShanheMarketCommand.Execute(null);
            Layout(root, new(1120, 780));
            CheckFiveCardsAndPage(shop, root, new(1120, 780));
            Program.Render(root, 1120, 780, Path.Combine(output, "shanhe-market-floating-1120x780-fifth.png"));
            Program.Assert(TableShortcutChecks.Input(owner, Key.Escape) && !vm.IsShanheMarketOpen && vm.IsShanheOpen &&
                tabs.SelectedIndex == mapIndex && ReferenceEquals(FocusSurface(owner, vm), owner.FindName("ShanheOverlay")) &&
                Program.Engine(vm).Revision == revision,
                "At the minimum main-window size, Escape returns from the actual merchant to the same map without changing its live campaign battle.");
        }
        finally { owner.Close(); }
    }

    private static void CheckFiveCardsAndPage(ShanheShopPanel shop, FrameworkElement root, Size size)
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
        var summaries = Program.Find<TextBlock>(shop).Where(block =>
            vm.ShanheMarketGoods.Any(product => product.ShopSummary == block.Text)).ToArray();
        Program.Assert(vm.ShanheMarketGoods.All(product => summaries.Any(block => block.Text == product.ShopSummary)) &&
            summaries.All(block => block.TextWrapping == TextWrapping.Wrap),
            "The actual card body renders its core effect without repeated acquisition or implementation notes.");
        CheckRuleTooltip(shop, vm.ShanheMarketGoods.First(), root);
        for (var page = 0; page < 8 && next.IsEnabled; page++) { Click(next); Layout(root, size); }
        var fifth = vm.ShanheMarketGoods.Last();
        var buy = Program.Find<Button>(shop).Single(button => ReferenceEquals(button.CommandParameter, fifth));
        var bounds = Bounds(buy, scroll);
        Program.Assert(!next.IsEnabled && previous.IsEnabled && scroll.HorizontalOffset > 0 &&
            bounds.Left >= -1 && bounds.Right <= scroll.ViewportWidth + 1 &&
            bounds.Top >= -1 && bounds.Bottom <= scroll.ViewportHeight + 1,
            "Actual forward-arrow clicks reach the fifth product and keep its original bound purchase control fully inside the viewport.");
        Click(previous); Layout(root, size);
        Program.Assert(next.IsEnabled, "The backward page arrow can return from the last product.");
        scroll.ScrollToRightEnd(); Layout(root, size);
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

    private static MainViewModel Create() => new(false, 83, true, new MemorySaveStore(), useExpandedContent: true,
        preferencesStore: new MemoryPlayerPreferencesStore(),
        shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
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
            rewards, StartingGold: 2000);
        var session = ShanheCampaignSession.Create(definition, new(83, "standard:guan-yu")
            { UseCodexRewardPool = true, PersistentMarket = true,
              UnlockedRewardIds = rewards.Select(reward => reward.Id).ToArray() }, _ => registry);
        Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.ShopItems.Count == 7 &&
            session.Snapshot.ShopItems.Count(product => product.Reward?.CampaignEconomy is not null) == 5,
            "A new permanent-market expedition publishes five real economic tactics and two native material faces while leaving the map open.");
        return session;
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
    [DllImport("user32.dll", EntryPoint = "GetWindowLongW")] private static extern int GetWindowLong(nint handle, int index);
    [DllImport("user32.dll", EntryPoint = "SetWindowLongW")] private static extern int SetWindowLong(nint handle, int index, int value);
}

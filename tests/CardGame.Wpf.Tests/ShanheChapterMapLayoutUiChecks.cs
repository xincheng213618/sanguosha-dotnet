using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Automation;
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

internal static partial class ShanheChapterMapLayoutUiChecks
{
    public static void ReadableBoundedRouteAndNativeNodeButtons(string output)
    {
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        model.StartShanheCommand.Execute(null);
        Program.Assert(((ShanheCampaignSession)typeof(MainViewModel).GetField("_shanheSession",
                BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(model)!).CreateCheckpoint().Options.UseSourceSceneCityBindings,
            "A new source expedition enables the same native scene-city binding used by its geographic map.");
        var panel = new ShanheCampaignPanel { DataContext = model };
        try
        {
            var allRegions = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(25);
            foreach (var layout in new[]
            {
                (Size: new Size(1120, 780), HasExtraBattles: false, Region: 0),
                (Size: new Size(1440, 880), HasExtraBattles: false, Region: 0),
                (Size: new Size(1440, 880), HasExtraBattles: true, Region: 0),
                (Size: new Size(1120, 780), HasExtraBattles: false, Region: 3),
                (Size: new Size(1440, 880), HasExtraBattles: true, Region: 3),
                (Size: new Size(1120, 780), HasExtraBattles: true, Region: 14),
                (Size: new Size(1440, 880), HasExtraBattles: true, Region: 14)
            }.Concat(StandardShanheSourceRouteCatalog.Season5.Regions.Where(region => region.SourceUniqueId is not (3 or 14))
                .SelectMany(region => new[] { new Size(1120, 780), new Size(1440, 880) }
                    .Select(size => (Size: size, HasExtraBattles: true, Region: region.SourceUniqueId)))))
            {
                var size = layout.Size;
                if (layout.HasExtraBattles || layout.Region > 0)
                {
                    var tier = layout.Region > 0 ? 25 : 1;
                    var definition = layout.Region > 0 ? allRegions : StandardShanheCampaignCatalog.CreateSourceCodexDefinition(tier);
                    if (layout.Region > 0) definition = ShanheCampaignContent.FreezeAndValidate(definition with
                    {
                        StartingNodeIds = definition.Nodes.Where(node => node.ChapterRouteId == "source-region-" + layout.Region &&
                            node.ChapterRole == ShanheChapterNodeRole.OpeningBattle).Select(node => node.Id).ToArray(),
                        Nodes = definition.Nodes.Where(node => node.ChapterRouteId == "source-region-" + layout.Region)
                            .Select(node => node with { NextNodeIds = node.NextNodeIds.Where(id => definition.Nodes.Any(next =>
                                next.Id == id && next.ChapterRouteId == node.ChapterRouteId)).ToArray() }).ToArray()
                    });
                    var travel = StandardShanheCampaignCatalog.CurrentDailyTravel2RewardId;
                    var session = ShanheCampaignSession.Create(definition, new(71, model.SelectedShanheGeneral!.Id)
                    {
                        SourceDifficultyId = tier, UseCodexRewardPool = true, PersistentMarket = true,
                        UseSourceSceneCityBindings = true,
                        EquippedTacticIds = layout.HasExtraBattles ? [travel] : [], UnlockedRewardIds = [travel]
                    }, StandardShanheCampaignCatalog.CreateBattleRegistry);
                    const BindingFlags hidden = BindingFlags.Instance | BindingFlags.NonPublic;
                    typeof(MainViewModel).GetField("_shanheSession", hidden)!.SetValue(model, session);
                    typeof(MainViewModel).GetField("_shanheRunDifficulty", hidden)!.SetValue(model, tier);
                    typeof(MainViewModel).GetMethod("RefreshShanhePresentation", hidden)!.Invoke(model, null);
                }
                var suffix = layout.Region == 14 ? "-xiaohan" : layout.Region == 3 ? "-hangu" : layout.Region > 0 ? "-region-" + layout.Region : layout.HasExtraBattles ? "-daily-travel-ii" : string.Empty;
                var image = Path.Combine(output, $"shanhe-chapter-route-{size.Width:0}x{size.Height:0}{suffix}.png");
                Program.Render(panel, (int)size.Width, (int)size.Height, layout.Region is 0 or 2 or 3 or 4 or 14 ? image : null);
                panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
                panel.UpdateLayout();
                var map = Program.Find<ShanheJourneyMap>(panel).Single();
                var buttons = Program.Find<Button>(map).Where(button => button.DataContext is ShanheJourneyNode).ToArray();
                Program.Assert(model.IsShanheRunInProgress && buttons.Length == model.ShanheMapNodes.Count && buttons.Length > 0,
                    "The actual chapter map must realize exactly its projected journey nodes.");
                if (layout.HasExtraBattles)
                    Program.Assert(model.ShanheMapNodes.Count(node => node.IsExtraBattle) == 2 &&
                            model.ShanheMapNodes.Where(node => node.IsExtraBattle).All(node => !node.IsAvailable &&
                                !node.HasRouteFlag && node.RouteFlagText.Length == 0) &&
                            Program.Find<Border>(map).Where(border => border.Name == "RouteFlag" &&
                                border.DataContext is ShanheJourneyNode { IsExtraBattle: true }).All(border => border.Visibility == Visibility.Collapsed),
                        "Daily travel II retains both owned extra battles at chapter start, but locked pursuits do not display an actionable route badge.");
                if (layout.Region > 0)
                {
                    var background = (Image)map.FindName("NativeMapBackground");
                    Program.Assert(model.HasShanheNativeMap && !model.IsShanheMapScrollable &&
                            background.Source is BitmapSource { PixelWidth: 1220, PixelHeight: 732 } &&
                            background.Visibility == Visibility.Visible,
                        "Screenshot-backed source regions use their original full map and source coordinate plane without scrolling.");
                    foreach (var button in buttons)
                    {
                        var node = (ShanheJourneyNode)button.DataContext;
                        var art = node.NativeLandmark!;
                        var picture = Program.Find<Image>(button).Single(image => image.Name == "NativeLandmarkImage");
                        Program.Assert(node.HasNativeArtwork && art.Artwork is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
                                ReferenceEquals(picture.Source, node.NativeArtwork) && picture.ActualWidth > 0 && picture.ActualHeight > 0 &&
                                Math.Abs(node.X - art.X * 1000 / 1220) < .01 && Math.Abs(node.Y - art.Y * 1000 / 1220) < .01 &&
                                Program.Find<Border>(button).Single(border => border.Name == "NamePlate").Visibility == Visibility.Collapsed,
                            "Each native city uses the original source position and baked nameplate, without a second generic plaque.");
                    }
                    var market = (Button)map.FindName("ShanheMapMarketButton");
                    Program.Assert(market.Visibility == Visibility.Collapsed && ReferenceEquals(market.Command, model.OpenShanheMarketCommand),
                        "The source market icon keeps the real shop command and remains hidden until the chapter's third mainline step.");
                }
                var viewport = new Rect(0, 0, map.ActualWidth, map.ActualHeight);
                var bounds = buttons.Select(button => button.TransformToAncestor(map).TransformBounds(new Rect(button.RenderSize))).ToArray();
                var clientBounds = buttons.Select(button => button.TransformToAncestor(panel).TransformBounds(new Rect(button.RenderSize))).ToArray();
                Program.Assert(bounds.All(rectangle => rectangle.Width > 70 && rectangle.Height > 70 &&
                        rectangle.Left >= -.5 && rectangle.Top >= -.5 && rectangle.Right <= viewport.Right + .5 && rectangle.Bottom <= viewport.Bottom + .5) &&
                        clientBounds.All(rectangle => rectangle.Width >= 90 && rectangle.Height >= 70 &&
                            rectangle.Left >= -.5 && rectangle.Top >= -.5 && rectangle.Right <= size.Width + .5 && rectangle.Bottom <= size.Height + .5) &&
                        (model.HasShanheNativeMap || bounds.SelectMany((rectangle, index) => bounds.Skip(index + 1).Select(other => rectangle.IntersectsWith(other))).All(overlap => !overlap)),
                    $"In region {layout.Region}, at {size.Width:0}x{size.Height:0}, every chapter node retains a readable button within the map. Diagnostic: {image}");
                foreach (var button in buttons)
                {
                    var node = (ShanheJourneyNode)button.DataContext;
                    var routeFlag = (Border)button.Template.FindName("RouteFlag", button);
                    Program.Assert(routeFlag.Visibility == (node.HasRouteFlag ? Visibility.Visible : Visibility.Collapsed) &&
                            routeFlag.Child is TextBlock flagText && flagText.Text == node.RouteFlagText &&
                            (!node.HasRouteFlag || node.IsAvailable && (node.RouteFlagText is "主线" or "可选" or "追击")) &&
                            (!node.IsCompleted && !node.IsBoss && node.IsAvailable || !node.HasRouteFlag) &&
                            node.EnemyText.Length == 0,
                        "Each real template follows its available node's route classification; future, completed and boss locations do not gain action badges or reveal enemy introductions.");
                    if (model.HasShanheNativeMap)
                        Program.Assert(button is ShanheMapNodeButton &&
                                button.Template.FindName("NodeVisual", button) is Grid { IsHitTestVisible: false },
                            "Every original map uses the shared transparent-pixel target without a rectangular child intercepting it; real pointer hits are checked in the loaded native-window route flow.");
                    Program.Assert(button.IsEnabled == node.IsAvailable &&
                            ReferenceEquals(button.Command, model.PreviewShanheNodeCommand) && ReferenceEquals(button.CommandParameter, node.Node) &&
                            (!node.IsAvailable || button.Command!.CanExecute(button.CommandParameter)),
                        "Node availability must match the bound native preview command at each supported window size.");
                    Program.Assert(!string.IsNullOrWhiteSpace(AutomationProperties.GetName(button)) &&
                            button.ToolTip is string description && description.Contains(node.Node.Name, StringComparison.Ordinal) &&
                            ToolTipService.GetShowOnDisabled(button),
                        "Every future, current and available destination must retain a readable identity and rule description.");
                }
                if (layout.Region == 0 && !layout.HasExtraBattles && size.Width == 1120)
                {
                    const BindingFlags hidden = BindingFlags.Instance | BindingFlags.NonPublic;
                    var session = (ShanheCampaignSession)typeof(MainViewModel).GetField("_shanheSession", hidden)!.GetValue(model)!;
                    var beforePreview = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                    var ordinaryRevision = Program.Engine(model).Revision;
                    var opening = buttons.First(button => button.DataContext is ShanheJourneyNode { IsAvailable: true, IsBattle: true, RouteFlagText: "主线" });
                    opening.Command!.Execute(opening.CommandParameter);
                    panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
                    var routeBanner = (Border)panel.FindName("ShanheEncounterRouteBanner");
                    var routeHint = (TextBlock)panel.FindName("ShanheEncounterRouteHint");
                    var story = (TextBlock)panel.FindName("ShanheEncounterStoryText");
                    Program.Assert(model.IsShanheEncounterPreviewOpen && model.ShanheEncounterRouteHint.Contains("主线第1格", StringComparison.Ordinal) &&
                            routeBanner.Visibility == Visibility.Visible && routeHint.Text == model.ShanheEncounterRouteHint &&
                            story.Text == model.ShanheEncounterDescription && !string.IsNullOrWhiteSpace(story.Text) &&
                            routeHint.TextWrapping == TextWrapping.Wrap && story.TextWrapping == TextWrapping.Wrap &&
                            ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == beforePreview && session.PendingBattleSetup is null,
                        "Opening the existing natural source battle displays its separate mainline route hint and complete original enemy introduction without entering battle or spending a campaign command.");
                    model.CloseShanheEncounterPreviewCommand.Execute(null);
                    panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
                    Program.Assert(!model.IsShanheEncounterPreviewOpen && Program.Engine(model).Revision == ordinaryRevision &&
                            ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == beforePreview,
                        "Closing the natural source route preview leaves campaign resources, the journal and the ordinary game unchanged.");
                }
            }
        }
        finally { panel.DataContext = null; }
        VerifyWideNativeMap(output);
        VerifyNativeSceneCities(output);
    }

    private static void VerifyWideNativeMap(string output)
    {
        const int seed = 2; // The unchanged source entry lottery selects Hulao.
        using var model = new MainViewModel(false, seed, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        model.StartShanheCommand.Execute(null);
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(1);
        var session = ShanheCampaignSession.Create(source, new(seed, model.SelectedShanheGeneral!.Id)
        {
            SourceDifficultyId = 1, UseCodexRewardPool = true, PersistentMarket = true, PreferSourceOrdinaryEncounters = true,
            UseSourceSceneCityBindings = true
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        const BindingFlags hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        typeof(MainViewModel).GetField("_shanheSession", hidden)!.SetValue(model, session);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", hidden)!.SetValue(model, 1);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", hidden)!.Invoke(model, null);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        model.OpenShanheCommand.Execute(null);
        var checkpoint = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var ordinary = Program.Engine(model);
        var ordinaryCheckpoint = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        var chromeWidth = host.Window.ActualWidth - host.Root.ActualWidth;
        var chromeHeight = host.Window.ActualHeight - host.Root.ActualHeight;
        host.Root.Width = 2560; host.Root.Height = 780;
        host.Window.SizeToContent = SizeToContent.Manual;
        host.Window.Width = 2560 + chromeWidth; host.Window.Height = 780 + chromeHeight;
        host.Window.UpdateLayout();
        host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
        var imagePath = Path.Combine(output, "shanhe-native-map-wide-2560x780.png");
        // Keep the complete failure image before checking clipping or pointer hits.
        Program.Render(host.Root, 2560, 780, imagePath);
        Console.WriteLine($"Native wide map: root={host.Root.RenderSize}, window={host.Window.ActualWidth:F2}x{host.Window.ActualHeight:F2}, image={imagePath}.");
        var panel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
        var map = Program.Find<ShanheJourneyMap>(panel).Single(control => control.IsVisible);
        var scroll = (ScrollViewer)panel.FindName("ShanheMapScroll");
        var viewport = Program.Find<ScrollContentPresenter>(scroll).Single(presenter => ReferenceEquals(presenter.TemplatedParent, scroll));
        var buttons = Program.Find<ShanheMapNodeButton>(map).Where(button => button.DataContext is ShanheJourneyNode { IsAvailable: true }).ToArray();
        Program.Assert(Math.Abs(host.Root.ActualWidth - 2560) < .75 && Math.Abs(host.Root.ActualHeight - 780) < .75 &&
                panel.IsVisible && model.HasShanheNativeMap &&
                !model.IsShanheMapScrollable && viewport.ActualWidth > 2000 && viewport.ActualHeight > 0 &&
                buttons.Select(button => ((ShanheJourneyNode)button.DataContext).Node.Id).Order().SequenceEqual(new[] { "source-city-5041", "source-city-5042" }) &&
                buttons.All(button => source.Nodes.Single(node => node.Id == ((ShanheJourneyNode)button.DataContext).Node.Id).ChapterRole == ShanheChapterNodeRole.OpeningBattle),
            "The wide native case must show the actual Source 1 Hulao opening and both legal source entrances in a loaded 2560-by-780 client, without substituting a local map.");
        var failures = new List<string>();
        foreach (var button in buttons)
        {
            var node = (ShanheJourneyNode)button.DataContext;
            var art = node.NativeLandmark!;
            var picture = Program.Find<Image>(button).Single(image => image.Name == "NativeLandmarkImage");
            var marker = (Image)button.Template.FindName("BattleMarker", button);
            Program.Assert(node.HasNativeArtwork && Math.Abs(node.X - art.X * 1000d / 1220) < .01 &&
                    Math.Abs(node.Y - art.Y * 1000d / 1220) < .01 && ReferenceEquals(picture.Source, node.NativeArtwork) &&
                    button.IsVisible && button.IsEnabled && ReferenceEquals(button.Command, model.PreviewShanheNodeCommand) &&
                    ReferenceEquals(button.CommandParameter, node.Node) && button.Command!.CanExecute(button.CommandParameter),
                "Wide layout must retain each original source coordinate and the actual available node's bound executable preview command.");
            InspectImage(picture, button, node.Node.Id + " sprite");
            InspectImage(marker, button, node.Node.Id + " battle marker");
        }
        Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == checkpoint && session.PendingBattleSetup is null &&
                !model.IsShanheEncounterPreviewOpen && ReferenceEquals(Program.Engine(model), ordinary) &&
                GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryCheckpoint,
            "Native wide-map rendering, geometry and pointer hit queries must leave both campaign and ordinary-game checkpoints unchanged without opening or challenging an encounter.");
        Program.Assert(failures.Count == 0,
            "Every available native opening sprite and battle marker must remain inside the real map viewport and ancestor clips, with opaque pixels reaching its executable node. " +
            string.Join("; ", failures) + ". Diagnostic: " + imagePath);

        void InspectImage(Image image, ShanheMapNodeButton button, string label)
        {
            Program.Assert(image.IsVisible && image.Source is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 },
                "An available source entrance must render its real sprite and battle-marker PNG: " + label);
            var bitmap = (BitmapSource)image.Source;
            BitmapSource pixels = bitmap.Format == PixelFormats.Bgra32 ? bitmap : new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0);
            var stride = checked(pixels.PixelWidth * 4);
            var bgra = new byte[checked(stride * pixels.PixelHeight)];
            pixels.CopyPixels(bgra, stride, 0);
            var left = pixels.PixelWidth; var top = pixels.PixelHeight; var right = -1; var bottom = -1;
            var point = new Point(); var distance = double.MaxValue;
            for (var y = 0; y < pixels.PixelHeight; y++)
                for (var x = 0; x < pixels.PixelWidth; x++)
                {
                    if (bgra[y * stride + x * 4 + 3] <= 16) continue;
                    left = Math.Min(left, x); top = Math.Min(top, y); right = Math.Max(right, x); bottom = Math.Max(bottom, y);
                    var candidateDistance = Math.Pow(x + .5 - pixels.PixelWidth / 2d, 2) + Math.Pow(y + .5 - pixels.PixelHeight / 2d, 2);
                    if (bgra[y * stride + x * 4 + 3] > 220 && candidateDistance < distance)
                    { distance = candidateDistance; point = new(x + .5, y + .5); }
                }
            Program.Assert(right >= left && bottom >= top && distance < double.MaxValue,
                "Native hit checks require an actual opaque PNG pixel: " + label);
            var content = VisualTreeHelper.GetContentBounds(image);
            if (content.IsEmpty) content = new Rect(image.RenderSize);
            var opaque = new Rect(content.Left + left * content.Width / pixels.PixelWidth,
                content.Top + top * content.Height / pixels.PixelHeight,
                (right - left + 1) * content.Width / pixels.PixelWidth, (bottom - top + 1) * content.Height / pixels.PixelHeight);
            var viewportBounds = image.TransformToAncestor(viewport).TransformBounds(opaque);
            if (!Fits(viewportBounds, new Rect(viewport.RenderSize))) failures.Add($"{label} outside map viewport: {viewportBounds} / {viewport.RenderSize}");
            for (DependencyObject? ancestor = image; ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
            {
                if (ancestor is FrameworkElement element)
                {
                    var fragment = ReferenceEquals(element, image) ? opaque : image.TransformToAncestor(element).TransformBounds(opaque);
                    if (LayoutInformation.GetLayoutClip(element) is { } layoutClip && !Fits(fragment, layoutClip.Bounds))
                        failures.Add($"{label} clipped by {element.GetType().Name} {element.Name} layout: {fragment} / {layoutClip.Bounds}");
                    if (element.Clip is { } clip && !Fits(fragment, clip.Bounds))
                        failures.Add($"{label} clipped by {element.GetType().Name} {element.Name} explicit clip: {fragment} / {clip.Bounds}");
                    if (element.ClipToBounds && !Fits(fragment, new Rect(element.RenderSize)))
                        failures.Add($"{label} clipped by {element.GetType().Name} {element.Name} bounds: {fragment} / {element.RenderSize}");
                }
                if (ReferenceEquals(ancestor, host.Root)) break;
            }
            var imagePoint = new Point(content.Left + point.X * content.Width / pixels.PixelWidth,
                content.Top + point.Y * content.Height / pixels.PixelHeight);
            var rootPoint = image.TranslatePoint(imagePoint, host.Root);
            var hit = host.Root.InputHitTest(rootPoint) as DependencyObject;
            if (hit is null || !(ReferenceEquals(hit, button) || button.IsAncestorOf(hit)))
                failures.Add($"{label} opaque hit {rootPoint} reached {hit?.GetType().Name ?? "nothing"}");
        }

        static bool Fits(Rect fragment, Rect bounds)
        {
            bounds.Inflate(.75, .75);
            return bounds.Contains(fragment);
        }
    }
}

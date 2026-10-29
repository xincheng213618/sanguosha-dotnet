using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheTacticOpeningLayoutUiChecks
{
    public static void OpeningActionsRemainReachableBelowTactics(string output)
    {
        var catalogue = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var dragon = catalogue.Rewards.Single(reward => reward.TacticSkillId == StandardShanheDrawThresholdPackage.ShenlongBaiwei2SkillId);
        var tacticIds = new[] { dragon.Id, "tactic-economy-2089", "tactic-economy-2090", "tactic-economy-2091", "tactic-economy-2107", "tactic-economy-2092" };
        var rewards = tacticIds.Select(id => catalogue.Rewards.Single(reward => reward.Id == id) with { SchoolMembership = null }).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:shanhe-tactic-opening", "战法起手布局", ["classic:guan-yu"], ["battle"],
            [new("battle", "驻军", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("classic:ma-chao", MaxHpBonus: 10)])])], rewards);
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu") { EquippedTacticIds = tacticIds },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var vm = new MainViewModel(false, 270028, true, new MemorySaveStore(), confirmOpeningHand: true, useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        vm.OpenShanheCommand.Execute(null);
        vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable));
        for (var step = 0; step < 30 && !vm.CanRerollOpeningHand && vm.CanStepAi; step++) vm.StepAiCommand.Execute(null);
        var game = Program.Engine(vm);
        Program.Assert(vm.IsShanheBattleSession && vm.CanRerollOpeningHand && game.State.TurnNumber == 0 &&
            vm.HumanPlayer!.WushuangTactics.Count == 6, "The real campaign opening must retain all six owned tactics before hand confirmation.");

        using var host = new NativeResponseStatusChecks.NativeHost(vm);
        var root = host.Root;
        var window = host.Window;
        var rail = (ScrollViewer)window.FindName("HumanWushuangTactics");
        var actionBar = (FrameworkElement)window.FindName("ActionBar");
        foreach (var (width, height) in new[] { (1120, 740), (1440, 880) })
        {
            Capture(width, height, "opening");
            AssertSeparated();
            VerifyKeyboardTacticRules(width, height);
            foreach (var name in new[] { "RerollOpeningHandButton", "ChangeOpeningHandButton", "ConfirmOpeningHandButton" }) AssertHit(Button(name));
            var oldCards = vm.Hand.Select(card => card.Id).ToArray();
            var commandCount = game.AcceptedCommands.Count;
            Click(Button("RerollOpeningHandButton"));
            Program.Assert(game.AcceptedCommands.Count == commandCount + 1 && game.AcceptedCommands.Last() is AnswerPromptCommand
                { Choice: var reroll } && reroll == new ChoiceId("setup.opening.reroll") && vm.CanRerollOpeningHand &&
                vm.Hand.Count == oldCards.Length && !vm.Hand.Select(card => card.Id).SequenceEqual(oldCards),
                "The reachable random-reroll button must submit a real command and change the hand while retaining confirmation.");
            Click(Button("ChangeOpeningHandButton"));
            Capture(width, height, "opening-editor");
            AssertSeparated();
            Program.Assert(vm.IsOpeningHandEditorOpen, "The reachable change-hand button must open the real editor.");
            var target = vm.Hand.Single(card => card.IsSelected);
            var replacement = vm.OpeningCardChoices.First(choice => choice.Kind != target.Kind);
            vm.SelectedOpeningCardChoice = replacement;
            root.UpdateLayout();
            AssertHit(Button("ReplaceOpeningCardButton"));
            commandCount = game.AcceptedCommands.Count;
            Click(Button("ReplaceOpeningCardButton"));
            Program.Assert(game.AcceptedCommands.Count == commandCount + 1 && game.AcceptedCommands.Last() is AnswerPromptCommand answer &&
                answer.Choice == replacement.ChoiceId && vm.Hand.Count == oldCards.Length && vm.Hand.All(card => card.Id != target.Id) &&
                vm.CanRerollOpeningHand && game.State.TurnNumber == 0,
                "The actual replacement control must exchange the selected card through the current opening prompt.");
            Click(Button("ChangeOpeningHandButton"));
        }

        // Exercise presentation-only paging in this same tree without creating another battle.
        var items = (ItemsControl)window.FindName("HumanTacticItems");
        var viewport = (ScrollViewer)window.FindName("HumanTacticsViewport");
        var toggle = Button("ToggleHumanTacticsButton");
        var previous = Button("PreviousHumanTacticsButton");
        var next = Button("NextHumanTacticsButton");
        var binding = BindingOperations.GetBindingBase(items, ItemsControl.ItemsSourceProperty)!;
        try
        {
            var badges = WushuangTactics.All.Where(tactic => tactic.IsImplemented).Take(12)
                .Select(tactic => new WushuangTacticViewModel(tactic.Id)).ToArray();
            Program.Assert(badges.Length == 12, "The paging boundary fixture needs twelve real tactic badges.");
            items.ItemsSource = badges.Take(11).ToArray();
            Capture(1440, 880, "opening-eleven");
            AssertSeparated();
            Program.Assert(VisibleBadges().SequenceEqual(badges.Take(11)) && !toggle.IsVisible,
                "Exactly eleven tactics must stay in one visible row without requiring expansion.");
            items.ItemsSource = badges;
            Capture(1440, 880, "opening-collapsed");
            AssertSeparated();
            Program.Assert(items.Items.Count == 12 && VisibleBadges().SequenceEqual(badges.Take(3)),
                "Twelve owned tactics must retain every item while showing only three by default.");
            AssertHit(toggle, requiresCommand: false);
            Click(toggle);
            Capture(1440, 880, "opening-expanded");
            AssertSeparated();
            Program.Assert(VisibleBadges().SequenceEqual(badges.Take(11)) && next.IsEnabled && !previous.IsEnabled,
                "Expanding the same row must expose the first eleven tactics and enable only forward paging.");
            items.ItemsSource = badges.ToArray();
            Capture(1440, 880, "opening-expanded-refreshed");
            Program.Assert(VisibleBadges().SequenceEqual(badges.Take(11)),
                "Refreshing the owned tactic projection must preserve the player's expanded strip.");
            AssertHit(next, requiresCommand: false);
            Click(next);
            Capture(1440, 880, "opening-last-page");
            AssertSeparated();
            var lastPage = VisibleBadges();
            Program.Assert(lastPage.Length is > 0 and <= 11 && lastPage[^1] == badges[^1] && !next.IsEnabled && previous.IsEnabled,
                "Forward paging must reach the final tactic without showing more than eleven or enabling another page.");
            AssertHit(previous, requiresCommand: false);
            Click(previous);
            Capture(1440, 880, "opening-first-page");
            Program.Assert(VisibleBadges().SequenceEqual(badges.Take(11)) && !previous.IsEnabled,
                "Backward paging must return to the first eleven tactics.");
            AssertHit(toggle, requiresCommand: false);
            Click(toggle);
            Capture(1440, 880, "opening-recollapsed");
            AssertSeparated();
            Program.Assert(VisibleBadges().Length == 3, "Collapsing the expanded row must again display only three tactics.");
            foreach (var name in new[] { "RerollOpeningHandButton", "ChangeOpeningHandButton", "ConfirmOpeningHandButton" }) AssertHit(Button(name));
        }
        finally { BindingOperations.SetBinding(items, ItemsControl.ItemsSourceProperty, binding); }

        Capture(1440, 880, "opening-confirm");
        AssertHit(Button("ConfirmOpeningHandButton"));
        var beforeConfirm = game.AcceptedCommands.Count;
        Click(Button("ConfirmOpeningHandButton"));
        Program.Assert(!vm.CanRerollOpeningHand && game.AcceptedCommands.Count == beforeConfirm + 1 &&
            game.AcceptedCommands.Last() is AnswerPromptCommand { Choice: var confirm } && confirm == new ChoiceId("setup.opening.confirm"),
            "The actual confirmation button must accept the current hand and leave opening confirmation.");
        for (var step = 0; step < 80 && !vm.CanEndTurn && vm.CanStepAi; step++) vm.StepAiCommand.Execute(null);
        Program.Assert(vm.CanEndTurn && vm.HumanPlayer!.WushuangTactics.Count == 6,
            "The same forward-only battle must reach its ordinary human play controls with six tactics intact.");
        foreach (var (width, height) in new[] { (1120, 740), (1440, 880) })
        {
            Capture(width, height, "play");
            AssertSeparated();
            AssertHit(Button("EndTurnButton"));
            foreach (var button in Program.Find<Button>((FrameworkElement)window.FindName("PrimaryActionButtons")).Where(button => button.IsVisible))
                Program.Assert(!Overlaps(Bounds(rail), Bounds(button)), "The tactic rail must also leave ordinary action buttons unobstructed.");
        }
        VerifyFocusedTacticDoesNotConfirmDraft();

        Button Button(string name) => (Button)window.FindName(name);
        Rect Bounds(FrameworkElement element) => element.TransformToVisual(root).TransformBounds(new Rect(element.RenderSize));
        static bool Overlaps(Rect first, Rect second) { var overlap = Rect.Intersect(first, second); return !overlap.IsEmpty && overlap.Width > 0 && overlap.Height > 0; }
        void AssertSeparated() => Program.Assert(rail.IsVisible && rail.ActualWidth > 0 && rail.ActualHeight > 0 &&
            rail.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled && rail.HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled &&
            !Overlaps(Bounds(rail), Bounds(actionBar)),
            $"The real tactic viewport must stay above the action dock: rail={Bounds(rail)}, actions={Bounds(actionBar)}, root={root.RenderSize}.");
        WushuangTacticViewModel[] VisibleBadges()
        {
            var row = Enumerable.Range(0, items.Items.Count).Select(index =>
                (Item: (WushuangTacticViewModel)items.Items[index], Container: (FrameworkElement)items.ItemContainerGenerator.ContainerFromIndex(index)!)).ToArray();
            Program.Assert(row.All(item => item.Container is not null) && row.Length > 0 &&
                row.Max(item => Bounds(item.Container).Top) - row.Min(item => Bounds(item.Container).Top) < 1,
                "Every tactic must be arranged on the same horizontal row, including the clipped items.");
            var clip = Bounds(viewport);
            return row.Where(item => { var bounds = Bounds(item.Container); return clip.Contains(new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2)); })
                .Select(item => item.Item).ToArray();
        }
        void AssertHit(Button button, bool requiresCommand = true)
        {
            var hit = root.InputHitTest(button.TranslatePoint(new Point(button.ActualWidth / 2, button.ActualHeight / 2), root)) as DependencyObject;
            Program.Assert(button.IsVisible && button.IsEnabled && button.ActualWidth > 0 && button.ActualHeight > 0 &&
                (!requiresCommand || button.Command?.CanExecute(button.CommandParameter) == true) && hit is not null &&
                (ReferenceEquals(button, hit) || button.IsAncestorOf(hit)),
                $"The real {button.Name} center must receive input at {root.RenderSize}; hit={hit?.GetType().Name}.");
        }
        static void Click(Button button) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        void VerifyFocusedTacticDoesNotConfirmDraft()
        {
            var actions = game.GetHumanLegalActions();
            var action = actions.Where(candidate => candidate.CardId is { } id && candidate.ConversionSource is null &&
                    candidate.TargetCardId is null && candidate.TargetSeats.Count <= 1 &&
                    (candidate.Kind is LegalActionKind.Slash or LegalActionKind.Duel or LegalActionKind.Peach or LegalActionKind.DrawTwo or
                        LegalActionKind.Alcohol or LegalActionKind.Equip or LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden) &&
                    vm.Hand.Any(card => card.Id == id && card.IsPlayable))
                .OrderBy(candidate => candidate.TargetSeats.Count == 1 ? 0 : 1).FirstOrDefault();
            Program.Assert(action is not null,
                "The retained real opening must supply a confirmable physical-card draft without changing its seed: hand=" +
                string.Join(",", vm.Hand.Select(card => $"{card.Id}:{card.Kind}:{card.IsPlayable}")) + "; legal=" +
                string.Join(",", actions.Select(candidate => $"{candidate.Kind}:{candidate.CardId}:[{string.Join(',', candidate.TargetSeats)}]")));
            var card = vm.Hand.Single(item => item.Id == action!.CardId);
            vm.SelectCardCommand.Execute(card);
            foreach (var seat in action!.TargetSeats)
            {
                var target = vm.Seats.Single(item => item.Seat == seat);
                if (!target.IsSelectedTarget) vm.SelectTargetCommand.Execute(target);
            }
            Program.Assert(card.IsSelected && vm.CanConfirmSelected,
                $"The real {action.Kind} draft must already be executable before focusing a read-only tactic badge.");
            var before = Boundary();
            var items = (ItemsControl)window.FindName("HumanTacticItems");
            var badge = Program.Find<FrameworkElement>(items).First(element => element.Name == "TacticBadge");
            var tooltipBinding = BindingOperations.GetBindingBase(badge, FrameworkElement.ToolTipProperty)!;
            var tooltip = new ToolTip { Content = badge.ToolTip, PlacementTarget = badge,
                Placement = PlacementMode.AbsolutePoint, HorizontalOffset = -30000, VerticalOffset = -30000 };
            tooltip.Opened += Conceal;
            badge.ToolTip = tooltip;
            try
            {
                foreach (var (width, height) in new[] { (1120, 740), (1440, 880) })
                {
                    Capture(width, height, "play-tactic-draft");
                    host.FocusKeyboard();
                    if (!ReferenceEquals(Keyboard.FocusedElement, badge))
                        Program.Assert(items.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)),
                            "The same real battle can focus a tactic while an executable physical-card draft is retained.");
                    root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); window.UpdateLayout();
                    Program.Assert(ReferenceEquals(Keyboard.FocusedElement, badge) && badge.DataContext is WushuangTacticViewModel &&
                            Program.Find<Border>(badge).Single(border => border.Name == "TacticKeyboardFocus").IsVisible && vm.CanConfirmSelected,
                        "The visible tactic focus, rather than a selected card or target button, owns Enter and Space.");
                    foreach (var key in new[] { Key.Enter, Key.Space })
                        Program.Assert(TableShortcutChecks.Input(window, key) && vm.CanConfirmSelected && Boundary() == before &&
                                ReferenceEquals(Keyboard.FocusedElement, badge),
                            $"{key} on a focused tactic must consume only the read-only inspection key and preserve the exact payable hand/target draft, journal and campaign at {width}px.");
                }
            }
            finally
            {
                Button("EndTurnButton").Focus(); tooltip.IsOpen = false; tooltip.Opened -= Conceal;
                BindingOperations.SetBinding(badge, FrameworkElement.ToolTipProperty, tooltipBinding);
                vm.ClearSelectionCommand.Execute(null);
            }

            string Boundary() => game.Revision + "|" + CommandJson.Serialize(game.AcceptedCommands) + "|" +
                SnapshotJson.Serialize(game.CreateSnapshot(0)) + "|" + ShanheCampaignJson.Serialize(session.CreateCheckpoint()) + "|" +
                string.Join(",", vm.Hand.Select(item => item.Id + ":" + item.IsSelected)) + "|" +
                string.Join(",", vm.Seats.Where(seat => seat.IsSelectedTarget).Select(seat => seat.Seat));
        }
        void VerifyKeyboardTacticRules(int width, int height)
        {
            var items = (ItemsControl)window.FindName("HumanTacticItems");
            var viewport = (ScrollViewer)window.FindName("HumanTacticsViewport");
            var badges = Program.Find<FrameworkElement>(items).Where(element => element.Name == "TacticBadge").ToArray();
            var expected = vm.HumanPlayer!.WushuangTactics;
            var before = Boundary();
            Program.Assert(badges.Length == 6 && badges.Select(badge => badge.DataContext).SequenceEqual(expected) &&
                badges.All(badge => badge.Focusable && ToolTipService.GetShowsToolTipOnKeyboardFocus(badge) == true),
                "Every real owned tactic, including the final badge, must provide a keyboard entry for its complete native rules.");
            var tooltips = badges.Select(badge => new
            {
                Badge = badge,
                Binding = BindingOperations.GetBindingBase(badge, FrameworkElement.ToolTipProperty)!,
                Delay = badge.ReadLocalValue(ToolTipService.InitialShowDelayProperty),
                Tooltip = new ToolTip { Content = badge.ToolTip, PlacementTarget = badge,
                    Placement = PlacementMode.AbsolutePoint, HorizontalOffset = -30000, VerticalOffset = -30000 }
            }).ToArray();
            try
            {
                foreach (var item in tooltips)
                {
                    Program.Assert(item.Binding is not null && Equals(item.Tooltip.Content, ((WushuangTacticViewModel)item.Badge.DataContext).Tooltip),
                        "The native keyboard tooltip retains the actual badge's bound full rules.");
                    // Keep the real service's keyboard opening path, changing only test placement and delay.
                    item.Tooltip.Opened += Conceal;
                    item.Badge.ToolTip = item.Tooltip;
                    ToolTipService.SetInitialShowDelay(item.Badge, 0);
                }
                host.FocusKeyboard();
                Program.Assert(Button("ConfirmOpeningHandButton").Focus(), "The owned native window accepts keyboard focus before browsing tactics.");
                NativeTab();
                Program.Assert(InputManager.Current.MostRecentInputDevice is KeyboardDevice,
                    "The tooltip check must enter through the owned HWND's native keyboard provider.");
                if (!ReferenceEquals(Keyboard.FocusedElement, badges[0]))
                    Program.Assert(items.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)),
                        "Native focus traversal can enter the first actual tactic badge.");
                for (var index = 0; index < badges.Length; index++)
                {
                    if (index > 0) NativeTab();
                    Pump();
                    var badge = badges[index];
                    var tactic = expected[index];
                    var tooltip = tooltips[index].Tooltip;
                    var focus = Program.Find<Border>(badge).Single(border => border.Name == "TacticKeyboardFocus");
                    var clip = viewport.TransformToVisual(root).TransformBounds(new Rect(viewport.RenderSize));
                    var badgeBounds = Bounds(badge);
                    var focusIsCorrect = ReferenceEquals(Keyboard.FocusedElement, badge) && focus.IsVisible && clip.Contains(badgeBounds) &&
                            badges.Where(other => !ReferenceEquals(other, badge)).All(other =>
                                Program.Find<Border>(other).Single(border => border.Name == "TacticKeyboardFocus").Visibility == Visibility.Collapsed);
                    var focusDiagnostic = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        Index = index,
                        ActualKeyboardType = Keyboard.FocusedElement?.GetType().FullName,
                        ActualKeyboardName = (Keyboard.FocusedElement as FrameworkElement)?.Name,
                        ActualKeyboardDataContext = (Keyboard.FocusedElement as FrameworkElement)?.DataContext?.ToString(),
                        BadgeBounds = new[] { badgeBounds.X, badgeBounds.Y, badgeBounds.Width, badgeBounds.Height, badgeBounds.Right, badgeBounds.Bottom },
                        ClipBounds = new[] { clip.X, clip.Y, clip.Width, clip.Height, clip.Right, clip.Bottom },
                        FocusIsVisible = focus.IsVisible,
                        OtherBorderVisibilities = badges.Where(other => !ReferenceEquals(other, badge)).Select(other => new
                        {
                            other.Name,
                            DataContext = other.DataContext?.ToString(),
                            Visibility = Program.Find<Border>(other).Single(border => border.Name == "TacticKeyboardFocus").Visibility.ToString()
                        }).ToArray()
                    });
                    if (!focusIsCorrect)
                        Program.Render(root, width, height, Path.Combine(output, $"shanhe-tactics-keyboard-focus-failed-{index + 1}-{width}.png"));
                    Program.Assert(focusIsCorrect,
                        $"Native Tab exposes the entire real tactic and moves the visible focus frame without activating it: {tactic.Name} at {width}px. " + focusDiagnostic);
                    Program.Assert(tooltip.IsOpen && PresentationSource.FromVisual(tooltip) is HwndSource,
                        $"Keyboard focus automatically opens the real native tactic tooltip without setting IsOpen: {tactic.Name}; " +
                        $"device={InputManager.Current.MostRecentInputDevice?.GetType().Name}, focused={Keyboard.FocusedElement}.");
                    var text = Program.Find<TextBlock>(tooltip).Single(block => block.Text == tactic.Tooltip);
                    Program.Assert(text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0 &&
                            new Rect(tooltip.RenderSize).Contains(text.TransformToVisual(tooltip).TransformBounds(new Rect(text.RenderSize))) &&
                            (tactic.SkillId is not { } skillId || game.ContentRegistry!.Skills.TryGetValue(skillId, out var skill) &&
                                text.Text.Contains(skill.Description, StringComparison.Ordinal)),
                        "The automatically opened tooltip visibly renders the complete actual native rule, rather than only a tactic name or a UIElement type.");
                    if (index == 0 || index == badges.Length - 1)
                    {
                        var label = index == 0 ? "first" : "last";
                        Program.Render(tooltip, (int)Math.Ceiling(tooltip.ActualWidth), (int)Math.Ceiling(tooltip.ActualHeight),
                            Path.Combine(output, $"shanhe-tactics-keyboard-rule-{label}-{width}.png"));
                        Capture(width, height, "keyboard-focus-" + label);
                    }
                    Program.Assert(Boundary() == before,
                        "Keyboard navigation and reading a native tactic tooltip preserve opening cards, the battle journal and campaign checkpoint.");
                    tooltip.IsOpen = false;
                }
                Button("ConfirmOpeningHandButton").Focus(); Pump();
                Program.Assert(badges.All(badge => Program.Find<Border>(badge).Single(border => border.Name == "TacticKeyboardFocus").Visibility == Visibility.Collapsed) &&
                    Boundary() == before, "Leaving the tactic strip clears its focus frame without confirming the opening hand.");
            }
            finally
            {
                foreach (var item in tooltips)
                {
                    item.Tooltip.IsOpen = false; item.Tooltip.Opened -= Conceal;
                    BindingOperations.SetBinding(item.Badge, FrameworkElement.ToolTipProperty, item.Binding);
                    if (item.Delay == DependencyProperty.UnsetValue) item.Badge.ClearValue(ToolTipService.InitialShowDelayProperty);
                    else item.Badge.SetValue(ToolTipService.InitialShowDelayProperty, item.Delay);
                }
            }

            string Boundary() => game.Revision + "|" + CommandJson.Serialize(game.AcceptedCommands) + "|" +
                SnapshotJson.Serialize(game.CreateSnapshot(0)) + "|" + ShanheCampaignJson.Serialize(session.CreateCheckpoint()) + "|" +
                string.Join(",", vm.Hand.Select(card => card.Id + ":" + card.IsSelected));
            void Pump() { root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); window.UpdateLayout(); root.UpdateLayout(); }
            void NativeTab()
            {
                var source = (HwndSource)PresentationSource.FromVisual(root)!;
                var message = new MSG { hwnd = source.Handle, message = 0x0100,
                    wParam = (nint)KeyInterop.VirtualKeyFromKey(Key.Tab), lParam = (nint)1, time = Environment.TickCount };
                ((IKeyboardInputSink)source).TranslateAccelerator(ref message, ModifierKeys.None);
                message.message = 0x0101; message.lParam = (nint)unchecked((int)0xc0000001);
                ((IKeyboardInputSink)source).TranslateAccelerator(ref message, ModifierKeys.None);
                Pump();
            }
        }
        void Capture(int width, int height, string label)
        {
            root.Width = width; root.Height = height; window.SizeToContent = SizeToContent.WidthAndHeight; window.UpdateLayout();
            Program.Render(root, width, height, Path.Combine(output, $"shanhe-tactics-{label}-{width}.png"));
            Program.Assert(root.RenderSize == new Size(width, height), "The loaded fixture must preserve the requested viewport.");
        }
    }

    private static void Conceal(object sender, RoutedEventArgs args)
    { if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowWindow(source.Handle, 0); }
    [DllImport("user32.dll")] private static extern bool ShowWindow(nint handle, int command);
}

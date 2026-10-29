using System.IO;
using System.Windows;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void RestCardsStageOneNativeGrowthAndRestoreWithoutSpending(string output)
    {
        var basis = ChapterRouteMapFixture();
        var definition = ShanheCampaignContent.FreezeAndValidate(basis with
        {
            Nodes = basis.Nodes.Select(node => node.Id != "rest" ? node : node with
            {
                Choices = [new("rest-draw", "摸牌数+1", RewardId: "rest-draw"), new("rest-hp", "体力上限+1", RewardId: "rest-hp")]
            }).ToArray(),
            Rewards = basis.Rewards.Concat(new ShanheRewardDefinition[]
            {
                new("rest-draw", "摸牌成长", Bonuses: new(Draw: 1)), new("rest-hp", "体力上限成长", Bonuses: new(MaxHp: 1))
            }).ToArray()
        });
        var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
        Win("opening-1");
        foreach (var id in new[] { "story-1", "story-2", "story-3" })
        {
            Accept(session, new MoveShanheNodeCommand(id, session.Revision));
            Accept(session, new ChooseShanheNodeOptionCommand("continue", null, session.Revision));
        }
        Win("main");
        Accept(session, new MoveShanheNodeCommand("rest", session.Revision));
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = Create(store);
        Attach(vm, session);
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 880) })
            {
                Program.Render(panel, (int)size.Width, (int)size.Height, Path.Combine(output, $"shanhe-rest-choice-{size.Width:0}.png"));
                var host = Program.Find<ShanheRestChoicePanel>(panel).Single();
                var list = (ListBox)host.FindName("RestChoiceList");
                var confirm = (Button)host.FindName("ConfirmRestChoiceButton");
                var cards = Program.Find<ListBoxItem>(list).ToArray();
                var rectangles = cards.Select(card => card.TransformToAncestor(panel).TransformBounds(new Rect(card.RenderSize))).ToArray();
                Program.Assert(vm.IsShanheRestChoiceOpen && list.Items.Count == 2 && cards.Length == 2 &&
                        !confirm.IsEnabled && !((Grid)panel.FindName("CampaignContent")).IsEnabled &&
                        ((Border)panel.FindName("ShanheContextPanel")).Visibility == Visibility.Collapsed &&
                        rectangles.All(rectangle => rectangle.Width > 200 && rectangle.Height > 200 && rectangle.Left >= 0 && rectangle.Top >= 0 &&
                            rectangle.Right <= size.Width && rectangle.Bottom <= size.Height) && !rectangles[0].IntersectsWith(rectangles[1]),
                    "The real rest overlay presents two separate growth cards within both window sizes, owns the modal and waits for a selection.");
                var sharedCards = Program.Find<ShanheRewardChoiceCard>(list).ToArray();
                Program.Assert(sharedCards.Length == 2,
                    "Both real rest choices use the same reward-choice card control as tactic rewards.");
                foreach (var card in sharedCards) AssertSharedRewardChoiceFrame(card, 2);
            }
            var restHost = Program.Find<ShanheRestChoicePanel>(panel).Single();
            var choices = (ListBox)restHost.FindName("RestChoiceList");
            var before = session.Snapshot;
            choices.SelectedItem = vm.ShanheRestChoices.Single(choice => choice.Glyph == "摸");
            choices.SelectedItem = vm.ShanheRestChoices.Single(choice => choice.Glyph == "体");
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
            Program.Assert(ReferenceEquals(before, session.Snapshot) && session.ChapterProgress == 5 &&
                    vm.ConfirmShanheRestChoiceCommand.CanExecute(null) &&
                    Program.Find<ShanheRewardChoiceCard>(choices).Count(card => card.IsSelected) == 1,
                "Changing the selected card is a reversible UI draft and grants no growth or mainline progress before confirmation.");
            Program.Render(panel, 1120, 780, Path.Combine(output, "shanhe-rest-choice-selected.png"));
            using (var native = new NativeResponseStatusChecks.NativeHost(vm))
            {
                native.Capture(output, "shanhe-rest-choice-native", 1120);
                native.FocusKeyboard();
                var nativeRest = Program.Find<ShanheRestChoicePanel>(native.Root).Single(control => control.IsVisible);
                var nativeChoices = (ListBox)nativeRest.FindName("RestChoiceList");
                var nativePanel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
                var objective = (TextBlock)nativePanel.FindName("ShanheChapterObjective");
                const string campObjective = "营地：摸牌数／体力上限二选一，确认后继续行军";
                Program.Assert(objective.IsVisible && objective.Text == campObjective && objective.Text == vm.ShanheChapterObjectiveText &&
                        BindingOperations.GetBinding(objective, TextBlock.TextProperty)?.Path.Path == nameof(MainViewModel.ShanheChapterObjectiveText),
                    "The actual native campaign header explains the pending camp's two growth choices instead of a generic event action.");
                nativeChoices.SelectedItem = null;
                native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                native.Root.UpdateLayout();
                var healthChoice = vm.ShanheRestChoices.Single(choice => choice.Glyph == "体");
                var healthCard = Program.Find<ShanheRewardChoiceCard>(nativeChoices)
                    .Single(card => ReferenceEquals(card.DataContext, healthChoice));
                var healthScroll = (ScrollViewer)healthCard.FindName("ChoiceDescriptionScroll");
                var healthBody = RewardChoiceBodyHit(native.Root, healthCard);
                var wheel = new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, -120)
                    { RoutedEvent = Mouse.PreviewMouseWheelEvent };
                healthBody.RaiseEvent(wheel);
                if (!wheel.Handled) { wheel.RoutedEvent = Mouse.MouseWheelEvent; healthBody.RaiseEvent(wheel); }
                native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                Program.Assert(vm.SelectedShanheRestChoice is null && !healthScroll.Focusable && healthScroll.IsHitTestVisible &&
                        ReferenceEquals(session.Snapshot, before) && !vm.ConfirmShanheRestChoiceCommand.CanExecute(null),
                    "Reading a real camp card with the wheel retains the unselected draft and grants no growth.");
                ClickRewardChoiceBody(native.Root, healthCard);
                native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                Program.Assert(ReferenceEquals(vm.SelectedShanheRestChoice, healthChoice) &&
                        vm.ConfirmShanheRestChoiceCommand.CanExecute(null) && ReferenceEquals(session.Snapshot, before) &&
                        objective.Text == campObjective,
                    "The real camp card's body press reaches its native ListBoxItem and stages the selected growth without confirming it.");
                AssertCampKeyboardDraft();
                nativeChoices.Focus();
                var checkpoint = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                var saved = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
                var selected = vm.SelectedShanheRestChoice;
                var ordinaryRevision = Program.Engine(vm).Revision;
                var escape = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(nativeChoices)!,
                    Environment.TickCount, Key.Escape) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                TableShortcutChecks.WithModifiers(ModifierKeys.None, () => { nativeChoices.RaiseEvent(escape); return true; });
                native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                Program.Assert(escape.Handled && vm.IsShanheOpen && vm.IsShanheRestChoiceOpen &&
                        ReferenceEquals(vm.SelectedShanheRestChoice, selected) && ReferenceEquals(session.Snapshot, before) &&
                        ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == checkpoint &&
                        (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved &&
                        Program.Engine(vm).Revision == ordinaryRevision,
                    "Escape belongs to the pending camp choice: it preserves the selected draft and leaves both games and the save untouched instead of exiting the expedition.");

                void AssertCampKeyboardDraft()
                {
                    var checkpointBeforeKeys = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                    var saveBeforeKeys = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
                    var acceptedBeforeKeys = session.AcceptedCommands.Count;
                    var ordinaryBeforeKeys = Program.Engine(vm).Revision;
                    var owners = Program.Find<ListBoxItem>(nativeChoices).ToArray();
                    var drawOwner = owners.Single(item => item.DataContext is ShanheRestChoiceCard { Glyph: "摸" });
                    var healthOwner = owners.Single(item => ReferenceEquals(item.DataContext, healthChoice));
                    var drawCard = Program.Find<ShanheRewardChoiceCard>(drawOwner).Single();
                    var nativeConfirm = (Button)nativeRest.FindName("ConfirmRestChoiceButton");
                    Program.Assert(drawOwner.Focus(), "The actual native camp item accepts keyboard focus for reading its growth choice.");
                    FlushKeys();
                    AssertFocus(drawOwner, drawCard, healthCard);
                    PressArrow(Key.Right);
                    AssertFocus(healthOwner, healthCard, drawCard);
                    Program.Assert(ReferenceEquals(vm.SelectedShanheRestChoice, healthChoice) && healthCard.IsSelected && !drawCard.IsSelected,
                        "Right-arrow navigation still selects the adjacent health choice through its real native ListBox behavior.");
                    PressArrow(Key.Left);
                    AssertFocus(drawOwner, drawCard, healthCard);
                    Program.Assert(vm.SelectedShanheRestChoice is { Glyph: "摸" } && drawCard.IsSelected && !healthCard.IsSelected,
                        "Left-arrow navigation still returns to the draw-growth draft instead of being consumed by shared card reading.");
                    PressArrow(Key.Right);
                    AssertFocus(healthOwner, healthCard, drawCard);
                    native.Capture(output, "shanhe-rest-choice-keyboard", 1120);
                    Program.Assert(!TableShortcutChecks.Input(native.Window, Key.Tab) &&
                            healthOwner.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)),
                        "The focused final camp card leaves Tab to native traversal toward the separate confirmation action.");
                    FlushKeys();
                    Program.Assert(ReferenceEquals(Keyboard.FocusedElement, nativeConfirm) &&
                            !drawCard.IsChoiceFocused && !healthCard.IsChoiceFocused &&
                            !((System.Windows.Shapes.Rectangle)drawCard.FindName("ChoiceKeyboardFocusOutline")).IsVisible &&
                            !((System.Windows.Shapes.Rectangle)healthCard.FindName("ChoiceKeyboardFocusOutline")).IsVisible &&
                            healthCard.IsSelected && !drawCard.IsSelected &&
                            ReferenceEquals(vm.SelectedShanheRestChoice, healthChoice) &&
                            ReferenceEquals(session.Snapshot, before) && session.ChapterProgress == 5 &&
                            session.AcceptedCommands.Count == acceptedBeforeKeys &&
                            ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == checkpointBeforeKeys &&
                            (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saveBeforeKeys &&
                            Program.Engine(vm).Revision == ordinaryBeforeKeys,
                        "Tab removes only the shared keyboard outline while preserving the selected growth draft, six-step progress, both games and the save until actual confirmation.");

                    void PressArrow(Key key)
                    {
                        var owner = Keyboard.FocusedElement as ListBoxItem;
                        Program.Assert(owner is not null, "A native camp item owns each direction key.");
                        TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
                        {
                            var input = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(owner!)!,
                                Environment.TickCount, key) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                            owner!.RaiseEvent(input);
                            Program.Assert(!input.Handled, "The shared reward frame passes Left and Right through to its native camp list.");
                            input.RoutedEvent = Keyboard.KeyDownEvent;
                            owner.RaiseEvent(input);
                            Program.Assert(input.Handled, "The actual camp ListBox handles its native card-selection direction key.");
                            return true;
                        });
                        FlushKeys();
                    }

                    void AssertFocus(ListBoxItem owner, ShanheRewardChoiceCard focused, ShanheRewardChoiceCard other)
                    {
                        var outline = (System.Windows.Shapes.Rectangle)focused.FindName("ChoiceKeyboardFocusOutline");
                        var bounds = outline.TransformToAncestor(focused).TransformBounds(new Rect(outline.RenderSize));
                        Program.Assert(ReferenceEquals(Keyboard.FocusedElement, owner) && focused.IsChoiceFocused &&
                                outline.IsVisible && outline.ActualWidth > 0 && outline.ActualHeight > 0 &&
                                bounds.Left >= -.7 && bounds.Top >= -.7 && bounds.Right <= focused.ActualWidth + .7 &&
                                bounds.Bottom <= focused.ActualHeight + .7 && !other.IsChoiceFocused &&
                                !((System.Windows.Shapes.Rectangle)other.FindName("ChoiceKeyboardFocusOutline")).IsVisible,
                            "Each real camp choice shows the same visible shared keyboard-focus outline only while its native item owns focus.");
                    }

                    void FlushKeys()
                    {
                        native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                        native.Root.UpdateLayout();
                    }
                }
                var cold = ShanheCampaignSession.Restore(session.CreateCheckpoint(), definition, _ => FixtureRegistry.Value);
                using (var restored = Create())
                {
                    Attach(restored, cold);
                    Program.Assert(restored.IsShanheRestChoiceOpen && restored.SelectedShanheRestChoice is null &&
                            !restored.ConfirmShanheRestChoiceCommand.CanExecute(null) && cold.ChapterProgress == 5,
                        "Cold recovery retains the unfinished native rest choice without persisting or paying its local selection draft.");
                }
                var confirmButton = (Button)nativeRest.FindName("ConfirmRestChoiceButton");
                Program.Assert(confirmButton.Command!.CanExecute(null), "The selected real rest card enables its confirmation command.");
                RawClick(confirmButton);
                native.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                native.Window.UpdateLayout();
                Program.Assert(!vm.IsShanheRestChoiceOpen && session.ChapterProgress == 6 && session.Snapshot.MaxHp == before.MaxHp + 1 &&
                        session.Snapshot.Hp == before.Hp && session.Snapshot.Bonuses.Draw == before.Bonuses.Draw,
                    "One actual confirmation grants only the selected health ceiling, preserves health and draw, and opens the sixth-step boss.");
                Program.Assert(objective.IsVisible && objective.Text == vm.ShanheChapterObjectiveText && objective.Text != campObjective &&
                        objective.Text.Contains("主线已满", StringComparison.Ordinal) && objective.Text.Contains("首领", StringComparison.Ordinal),
                    "The same live native header updates immediately from camp selection to the now-complete six-step boss objective only after actual confirmation.");
                native.Capture(output, "shanhe-rest-confirmed-objective", 1120);
                var accepted = session.AcceptedCommands.Count;
                vm.ConfirmShanheRestChoiceCommand.Execute(null);
                Program.Assert(session.AcceptedCommands.Count == accepted, "A stale confirmation cannot pay out rest growth twice.");
            }
        }
        finally { panel.DataContext = null; }

        MainViewModel Create(IShanheCampaignSaveStore? campaignStore = null) => new(false, 270018, true, new MemorySaveStore(), contentRegistry: FixtureRegistry.Value,
            shanheSaveStore: campaignStore ?? new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        void Win(string id)
        {
            Accept(session, new MoveShanheNodeCommand(id, session.Revision));
            WinPendingBattle(session);
            Accept(session, new ChooseShanheRewardCommand("tactic", null, session.Revision));
        }
    }
}

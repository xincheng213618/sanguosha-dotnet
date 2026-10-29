using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class BattleLogUiChecks
{
    public static void SeatsFilterNamedHistoryAndKeepStyledScrolling(string output)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 8, HumanSeat = 0, HumanRole = Role.Rebel,
            UseInteractiveSetup = false, AdvanceAfterHumanCommands = false
        }, ContentRegistry.Build(new StandardContentPackage()));
        Program.Assert(game.Submit(new StartGameCommand()).Accepted, "The log fixture must start.");
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var model = new MainViewModel(false, 31, true, store,
            historyStore: new MemoryMatchHistoryStore(), preferencesStore: new MemoryPlayerPreferencesStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model);
        Program.Assert(!model.HasSaveError && model.HumanPlayer?.Seat == 0, model.SaveStatus);
        var actors = model.Seats.OrderBy(seat => seat.DisplaySeatNumber).ToArray();
        var human = actors.Single(seat => seat.IsHuman);
        Program.Assert(human.DisplaySeatNumber != 1, "The fixture must cover a human displayed away from the first seat.");
        var opponents = actors.Where(seat => !seat.IsHuman).ToArray();
        var source = opponents[0];
        var target = opponents[1];
        var provider = opponents[2];
        var playerNames = Program.Engine(model).CreateSnapshot(0).Players.ToDictionary(player => player.Seat, player => player.Name);
        var next = model.FilteredBattleLog.Max(entry => entry.Sequence) + 1;
        var first = next;
        var addLog = typeof(MainViewModel).GetMethod("OnLogAdded", BindingFlags.Instance | BindingFlags.NonPublic)!;
        Add("CardUsed", $"{Name(source)} 对 {Name(target)} 使用【杀】。", source.Seat, target.Seat);
        Add("Damage", $"{Name(human)} 受到 {Name(source)} 造成的 1 点伤害。", source.Seat, human.Seat);
        Add("SkillTriggered", $"{Name(provider)} 响应【激将】，由 {Name(source)} 对 {Name(target)} 使用【杀】。", source.Seat, target.Seat);
        Add("Rules", "【你】与【AI 2】是原文中的标题。");

        var window = new MainWindow(model);
        var root = (FrameworkElement)window.Content;
        try
        {
            Program.Render(root, 1120, 780, Path.Combine(output, "battle-log-all-seats.png"));
            var filters = (ItemsControl)window.FindName("TableBattleLogSeats");
            var log = (ListBox)window.FindName("TableBattleLog");
            Program.Assert(ReferenceEquals(filters.ItemsSource, model.BattleLogSeats) &&
                ReferenceEquals(log.ItemsSource, model.FilteredBattleLog), "The table sidebar must expose the shared seat filters and public log.");
            Program.Assert(model.BattleLogSeats.Take(9).Select(option => option.Name)
                .SequenceEqual(new[] { "全部" }.Concat(Enumerable.Range(1, 8).Select(number => $"{number}号位"))),
                "All seats must follow 全部 in the table's displayed order, including the human away from the first position.");
            var visibleButtons = Program.Find<Button>(filters).Where(button => button.Visibility == Visibility.Visible).ToArray();
            Program.Assert(visibleButtons.Select(button => button.Content).SequenceEqual(new[] { "全", "主", "二", "三", "四", "五", "六", "七", "八" }) && visibleButtons.All(button =>
                button.TranslatePoint(new Point(), filters).X + button.ActualWidth <= filters.ActualWidth + 1) &&
                visibleButtons.All(button => Math.Abs(button.TranslatePoint(new Point(), filters).Y - visibleButtons[0].TranslatePoint(new Point(), filters).Y) < 1) &&
                visibleButtons.Skip(1).Select(button => button.ToolTip).SequenceEqual(Enumerable.Range(1, 8).Select(number => $"{number}号位")),
                "The narrow sidebar must fit all nine compact tabs on one line and expose the full seat number on hover.");
            var sidebar = (FrameworkElement)window.FindName("TableSidebar");
            Program.Assert(visibleButtons[0].TranslatePoint(new Point(), sidebar).X >= 12 &&
                Program.Find<TextBlock>(log).Where(text => text.DataContext is BattleLogEntryViewModel)
                    .All(text => text.TranslatePoint(new Point(), sidebar).X >= 12),
                "Seat tabs and log text must leave a visible inset from the sidebar frame.");
            var scroller = Program.Find<ScrollViewer>(log).Single();
            var scrollbar = Program.Find<ScrollBar>(log).Single(bar => bar.Orientation == Orientation.Vertical);
            var track = (Track)scrollbar.Template.FindName("PART_Track", scrollbar);
            Program.Assert(scrollbar.ActualWidth <= 10 && track.Thumb is not null && scroller.ScrollableHeight > 0,
                $"The log scrollbar must use a narrow draggable track with overflowing public history (width={scrollbar.ActualWidth}, thumb={track.Thumb is not null}, scrollable={scroller.ScrollableHeight}).");
            scroller.ScrollToEnd();
            Program.Render(root, 1120, 780, null);
            var atEnd = scroller.VerticalOffset;
            var pageUp = track.DecreaseRepeatButton;
            ((System.Windows.Input.RoutedCommand)pageUp.Command).Execute(pageUp.CommandParameter, pageUp.CommandTarget ?? pageUp);
            Program.Render(root, 1120, 780, null);
            Program.Assert(scroller.VerticalOffset < atEnd, "Clicking the styled scrollbar track must still page through public history.");
            scroller.ScrollToTop();
            Program.Render(root, 1120, 780, null);
            track.Thumb!.RaiseEvent(new DragStartedEventArgs(0, 0) { RoutedEvent = Thumb.DragStartedEvent });
            track.Thumb.RaiseEvent(new DragDeltaEventArgs(0, 30) { RoutedEvent = Thumb.DragDeltaEvent });
            track.Thumb.RaiseEvent(new DragCompletedEventArgs(0, 30, false) { RoutedEvent = Thumb.DragCompletedEvent });
            Program.Render(root, 1120, 780, null);
            Program.Assert(scroller.VerticalOffset > 0, "Dragging the custom log thumb must scroll the real list.");
            scroller.ScrollToTop();
            Program.Render(root, 1120, 780, Path.Combine(output, "battle-log-all-seats.png"));
            var richLogs = Program.Find<BattleLogTextBlock>(log).ToArray();
            Program.Assert(richLogs.Length > 0 && richLogs.All(block =>
                string.Concat(block.Inlines.OfType<Run>().Select(run => run.Text)) == block.Entry!.Message) &&
                richLogs.Any(block => block.Inlines.OfType<Run>().Select(run => run.Foreground).Distinct().Count() > 1),
                "Colored log runs must preserve the exact public text while distinguishing participant names and card titles.");
            var reused = new BattleLogTextBlock { Entry = new(1, 1, "Recovery", "damage", "", $"{Display(human)} 回复 1 点体力。", human.Seat, human.Seat)
                { ParticipantNames = [Display(human)] } };
            var recoveryColor = ((SolidColorBrush)reused.Inlines.OfType<Run>().Single(run => run.Text == "回复 1 点体力").Foreground).Color;
            reused.Entry = new(2, 1, "Damage", "damage", "", $"{Display(target)} 受到 1 点火焰伤害。", source.Seat, target.Seat)
                { ParticipantNames = [Display(target)] };
            var damageColor = ((SolidColorBrush)reused.Inlines.OfType<Run>().Single(run => run.Text == "1 点火焰伤害").Foreground).Color;
            Program.Assert(recoveryColor != damageColor && !reused.Text.Contains("回复", StringComparison.Ordinal),
                "Recycled rich log controls must replace old text and use distinct recovery and damage colors.");
            var all = Rows();
            Program.Assert(model.FilteredBattleLog.Where(entry => entry.Sequence < first &&
                (entry.ActorSeat is not null || entry.TargetSeat is not null))
                    .All(entry => !entry.Message.Contains("AI ", StringComparison.Ordinal) && !entry.Message.Contains("号位", StringComparison.Ordinal)),
                "Existing engine-authored history must also replace raw player names rather than portrait display names.");
            Program.Assert(all[0].Message == "【你】与【AI 2】是原文中的标题。" &&
                all[3].Message == $"{Display(source)} 对 {Display(target)} 使用【杀】。" &&
                all[2].Message == $"{Display(human)} 受到 {Display(source)} 造成的 1 点伤害。" &&
                model.GameLog.Any(message => message.EndsWith(all[2].Message, StringComparison.Ordinal)),
                "Public history must use general names for AI-to-AI and incoming damage while preserving literal titles.");

            Click($"seat:{target.Seat}");
            Program.Assert(Rows().Select(entry => entry.Sequence).SequenceEqual(new[] { first + 2, first }),
                "Selecting a seat must include its received actions and exclude unrelated events.");
            Program.Assert(Program.Find<Button>(filters).Single(button => Equals(button.CommandParameter, $"seat:{target.Seat}")).Tag is true,
                "The selected seat button must remain visibly selected after filtering.");
            Add("CardUsed", $"{Name(human)} 对 {Name(target)} 使用【火杀】。", human.Seat, target.Seat);
            Add("CardsDrawn", $"{Name(provider)} 摸了 1 张牌。", provider.Seat);
            Program.Assert(Rows().Select(entry => entry.Sequence).SequenceEqual(new[] { first + 4, first + 2, first }),
                "New public entries must respect the active seat filter.");
            model.SelectBattleLogCategoryCommand.Execute("damage");
            Program.Assert(Rows().Length == 0, "Seat and category filters must apply together.");
            Click($"seat:{human.Seat}");
            Program.Assert(Rows().Single().Sequence == first + 1, "The human seat must use the same received-action filter as other seats.");
            model.SelectBattleLogCategoryCommand.Execute("all");
            Click($"seat:{provider.Seat}");
            Program.Assert(Rows().Select(entry => entry.Sequence).SequenceEqual(new[] { first + 5, first + 2 }),
                "A third public participant must be included even when it is outside the primary actor/target pair.");

            model.ToggleLogCommand.Execute(null);
            Program.Render(root, 1120, 780, null);
            Program.Assert(ReferenceEquals(((ListBox)window.FindName("FilteredBattleLogList")).ItemsSource, log.ItemsSource),
                "The detail view must show the same filtered public history as the sidebar.");
            model.ToggleLogCommand.Execute(null);
            Click("all");
            Program.Assert(Rows().Length == 6, "全部 must restore all retained public entries.");

            var engine = Program.Engine(model);
            var before = engine.CreateSnapshot(human.Seat);
            var rawLog = engine.Log.ToArray();
            var order = before.TableOrder!.ToArray();
            var sourceIndex = Array.IndexOf(order, source.Seat);
            var targetIndex = Array.IndexOf(order, target.Seat);
            (order[sourceIndex], order[targetIndex]) = (order[targetIndex], order[sourceIndex]);
            typeof(MainViewModel).GetMethod("Refresh", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(model, [before with { TableOrder = Array.AsReadOnly(order) }]);
            Program.Assert(Rows().Single(entry => entry.Sequence == first).Message ==
                $"{Display(source)} 对 {Display(target)} 使用【杀】。" && engine.Log.SequenceEqual(rawLog),
                "Seat movement must preserve named public history and participant filters without changing stored facts.");
            Click($"seat:{target.Seat}");
            Program.Render(root, 1120, 780, Path.Combine(output, "battle-log-selected-seat.png"));
            typeof(MainViewModel).GetMethod("Refresh", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(model, [before with { Players = Array.AsReadOnly(before.Players.Select(player =>
                    player.Seat == source.Seat ? player with { GeneralName = "不可见将名", IsGeneralPublic = false } : player).ToArray()) }]);
            Program.Assert(Rows().Single(entry => entry.Sequence == first).Message.StartsWith("暗将", StringComparison.Ordinal) &&
                model.FilteredBattleLog.All(entry => !entry.Message.Contains("不可见将名", StringComparison.Ordinal) &&
                    !entry.ParticipantNames.Contains("不可见将名")),
                "Named log projection must not expose a concealed general even when a diagnostic snapshot contains its name.");
            LegacySaveFixtures.Load(model);
            Program.Assert(!model.HasSaveError && model.SelectedBattleLogSeat == "all" &&
                model.SelectedBattleLogCategory == "all" && model.FilteredBattleLog.All(entry => entry.Sequence < first) &&
                model.FilteredBattleLog.Where(entry => entry.ActorSeat == human.Seat || entry.TargetSeat == human.Seat)
                    .All(entry => !entry.Message.StartsWith("你", StringComparison.Ordinal) && !entry.Message.Contains("号位", StringComparison.Ordinal)),
                "Loading a match must start at 全部 and rebuild named history from its existing public log.");
            VerifyShanheHistoricalNames();
            Program.AssertNoBindingErrors();

            void Click(string id)
            {
                filters.UpdateLayout();
                var button = Program.Find<Button>(filters).Single(button => Equals(button.CommandParameter, id));
                Program.Assert(button.Command?.CanExecute(button.CommandParameter) == true, "The visible log filter must have a usable command.");
                button.Command!.Execute(button.CommandParameter);
            }
        }
        finally { window.Content = null; window.Close(); }

        BattleLogEntryViewModel[] Rows() => model.FilteredBattleLog.Where(entry => entry.Sequence >= first).ToArray();
        string Name(SeatViewModel seat) => playerNames[seat.Seat];
        string Display(SeatViewModel seat) => seat.GeneralName + (seat.IsHuman ? "（你）" : "");
        void Add(string type, string message, int? actor = null, int? targetSeat = null) =>
            addLog.Invoke(model, [new GameLogEntry(next++, 1, type, message, actor, targetSeat)]);

        void VerifyShanheHistoricalNames()
        {
            var engine = Program.Engine(model);
            var before = engine.CreateSnapshot(model.HumanPlayer!.Seat);
            var actor = before.Players.Single(player => player.IsHuman);
            var boss = before.Players.First(player => !player.IsHuman && player.GeneralName != actor.GeneralName);
            var second = engine.ContentRegistry.Generals.Values.First(general =>
                before.Players.All(player => player.GeneralId != general.Id && player.GeneralName != general.Name) &&
                !general.Name.Contains(boss.GeneralName, StringComparison.Ordinal) && !boss.GeneralName.Contains(general.Name, StringComparison.Ordinal));
            var rawLog = engine.Log.ToArray();
            var rawEvents = engine.Events.ToArray();
            var checkpoint = GameCheckpointJson.Serialize(engine.CreateCheckpoint());
            var historyField = typeof(MainViewModel).GetField("_battleLogShanheStageHistory", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var originalHistory = historyField.GetValue(model);
            var build = typeof(MainViewModel).GetMethod("BuildShanheBattleLogStageHistory", BindingFlags.Static | BindingFlags.NonPublic)!;
            var project = typeof(MainViewModel).GetMethod("ProjectBattleLogEntry", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var refresh = typeof(MainViewModel).GetMethod("Refresh", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var damage = new DamageAppliedEvent(actor.Seat, boss.Seat, 1, 0);
            var entered = new ShanheBossStageStartedEvent(boss.Seat, boss.GeneralId, second.Id, 1, 4, 4);
            // Deliberately different counters, with the text announcement after
            // the new body's draw, rule out cross-stream or announcement cuts.
            GameLogEntry[] logs =
            [
                new(10, 1, "CardUsed", $"{actor.Name} 对 {boss.Name} 使用【杀】。", actor.Seat, boss.Seat),
                new(20, 1, "Damage", $"{boss.Name} 受到 {actor.Name} 造成的 1 点伤害，剩余 0 点体力。", actor.Seat, boss.Seat),
                new(30, 1, "CardsDrawn", $"{boss.Name} 摸了 4 张牌。", boss.Seat),
                new(40, 1, "ShanheBossTransformed", $"首领进入第二阶段【{second.Name}】。", boss.Seat)
            ];
            EventEnvelope[] facts = [Envelope(10000, 7, damage), Envelope(10002, 7, entered)];
            var history = Build(logs, facts);
            var boundary = ((System.Collections.IEnumerable)history).Cast<object>().Single();
            Program.Assert((int)boundary.GetType().GetProperty("LastPreviousFormLogSequence")!.GetValue(boundary)! == 20,
                "A public stage identity pair correlates its native lethal damage occurrence to the independent log counter, before the new-body draw and late announcement.");
            Program.Assert(Count(Build(logs, [facts[0], facts[1] with { Revision = 8 }])) == 0 &&
                Count(Build(logs, [facts[0], Envelope(10001, 7, new ProgramSkillHpLostEvent(1, "test", boss.Seat, 1, 0)), facts[1]])) == 0 &&
                Count(Build(logs, [facts[0], Envelope(10001, 7, damage), facts[1]])) == 0 &&
                Count(Build(logs.Select(log => log.Type == "Damage" ? log with { Message = log.Message.Replace("1 点伤害", "2 点伤害", StringComparison.Ordinal) } : log).ToArray(), facts)) == 0 &&
                Count(Build(logs.Select(log => log.Type == "Damage" ? log with { ActorSeat = boss.Seat } : log).ToArray(), facts)) == 0,
                "Historical names fall back for a stale command, non-damage final health cause, missing occurrence, changed public amount or mismatched actor; no stage boundary is guessed.");
            var sourceLess = damage with { SourceLess = true };
            var sourceLessLogs = logs.Select(log => log.Type == "Damage" ? log with
            {
                ActorSeat = null, Message = $"{boss.Name} 受到 1 点无来源伤害，剩余 0 点体力。"
            } : log).ToArray();
            Program.Assert(Count(Build(sourceLessLogs, [Envelope(10000, 7, sourceLess), facts[1]])) == 1 &&
                Count(Build(sourceLessLogs, facts)) == 0,
                "Source-less damage correlates only with its own public null-actor result and never borrows an attributed hit.");
            var withoutDamage = logs.Where(log => log.Type != "Damage").ToArray();
            Program.Assert(Count(Build(withoutDamage,
                [Envelope(10000, 7, new ProgramSkillHpLostEvent(1, "test", boss.Seat, 1, 0)), facts[1]])) == 0 &&
                Count(Build(withoutDamage,
                [Envelope(10000, 7, new MaximumHpChangedEvent(boss.Seat, -1, 0, "test") { HpBeforeClamp = 1 }), facts[1]])) == 0,
                "Legacy HP-loss and maximum-HP-loss stage facts without an explicit cut or corresponding pre-transition result log retain the safe projection rather than borrow a damage or late announcement cut.");
            var marked = entered with { LastPreviousFormLogSequence = 10 };
            var hpLossHistory = Build(withoutDamage,
                [Envelope(10000, 7, new ProgramSkillHpLostEvent(1, "test", boss.Seat, 1, 0)), Envelope(10002, 7, marked)]);
            var maximumLossHistory = Build(withoutDamage.Where(log => log.Type != "CardsDrawn").ToArray(),
                [Envelope(10000, 7, new MaximumHpChangedEvent(boss.Seat, -1, 0, "test") { HpBeforeClamp = 1 }), Envelope(10002, 7, marked)]);
            Program.Assert(Count(hpLossHistory) == 1 && Cut(hpLossHistory) == 10 && Count(maximumLossHistory) == 1 && Cut(maximumLossHistory) == 10 &&
                Count(Build([], [Envelope(10002, 7, marked with { TurnNumber = 0, LastPreviousFormLogSequence = 0 })])) == 1 &&
                Count(Build(logs, [facts[0], Envelope(10001, 7, damage), Envelope(10002, 7, marked)])) == 1,
                "An explicit pre-replacement log cut supports HP loss, maximum-HP loss without a new-body draw, startup with no prior logs and unrelated unmatched damage occurrences without guessing an event/log correspondence.");
            Program.Assert(Count(Build(logs, [facts[0], Envelope(10002, 7, marked with { LastPreviousFormLogSequence = -1 })])) == 0 &&
                Count(Build(logs, [facts[0], Envelope(10002, 7, marked with { LastPreviousFormLogSequence = 41 })])) == 0 &&
                Count(Build(logs, [Envelope(10002, 7, marked with { BossSeat = boss.Seat + 100 })])) == 0 &&
                Count(Build(logs, [Envelope(10002, 7, marked with { GeneralId = boss.GeneralId })])) == 0 &&
                Count(Build(logs, [Envelope(10002, 7, marked), Envelope(10003, 7, marked)])) == 0,
                "A present invalid cut, missing seat, unchanged identity or duplicate same-seat transition cannot borrow an otherwise valid legacy damage boundary.");
            try
            {
                var visible = before with { Players = Array.AsReadOnly(before.Players.Select(player => player.Seat == boss.Seat
                    ? player with { GeneralId = second.Id, GeneralName = second.Name } : player).ToArray()) };
                refresh.Invoke(model, [visible]); historyField.SetValue(model, history);
                var attack = Project(logs[0]); var hit = Project(logs[1]); var draw = Project(logs[2]);
                Program.Assert(attack.Message.Contains(boss.GeneralName, StringComparison.Ordinal) &&
                    hit.Message.StartsWith(boss.GeneralName + " 受到", StringComparison.Ordinal) &&
                    hit.ParticipantNames.Contains(boss.GeneralName) && draw.Message.StartsWith(second.Name + " 摸了", StringComparison.Ordinal) &&
                    draw.ParticipantNames.Contains(second.Name),
                    "The actual log projector preserves the first body's prior attack target and lethal damage name while naming the new body's draw correctly.");
                var rebuilt = Build(logs, facts); historyField.SetValue(model, rebuilt);
                var rebuiltHit = Project(logs[1]);
                Program.Assert(Count(rebuilt) == 1 && rebuiltHit.Message == hit.Message &&
                    rebuiltHit.ParticipantNames.SequenceEqual(hit.ParticipantNames),
                    "Rebuilding history from the same public log and typed events preserves its deterministic historical projection.");
                foreach (var explicitHistory in new[] { hpLossHistory, maximumLossHistory })
                {
                    historyField.SetValue(model, explicitHistory);
                    Program.Assert(Project(logs[0]).Message.Contains(boss.GeneralName, StringComparison.Ordinal) &&
                        Project(logs[2]).Message.StartsWith(second.Name + " 摸了", StringComparison.Ordinal),
                        "The same real name projector consumes explicit HP-loss and maximum-HP-loss boundaries for both historical and post-stage rows.");
                }
                var other = visible.Players.First(player => player.Seat != boss.Seat && player.Seat != actor.Seat);
                refresh.Invoke(model, [visible with { Players = Array.AsReadOnly(visible.Players.Select(player => player.Seat == other.Seat
                    ? player with { GeneralName = boss.GeneralName } : player).ToArray()) }]);
                historyField.SetValue(model, history);
                var duplicate = Project(logs[1]);
                var qualified = $"{boss.GeneralName}（{model.Seats.Single(seat => seat.Seat == boss.Seat).DisplaySeatNumber}）";
                Program.Assert(duplicate.Message.StartsWith(qualified + " 受到", StringComparison.Ordinal) && duplicate.ParticipantNames.Contains(qualified),
                    "Historical names participate in the existing duplicate-name seat qualification and colored participant labels.");
                refresh.Invoke(model, [visible with { Players = Array.AsReadOnly(visible.Players.Select(player => player.Seat == boss.Seat
                    ? player with { GeneralName = "后续公开形态" } : player).ToArray()) }]);
                historyField.SetValue(model, hpLossHistory);
                Program.Assert(Project(logs[0]).Message.Contains(boss.GeneralName, StringComparison.Ordinal) &&
                    Project(logs[2]).Message.StartsWith("后续公开形态 摸了", StringComparison.Ordinal),
                    "Only pre-cut rows use the previous form; later rows retain the current projection instead of forcing a later non-stage identity back to the second form.");
                foreach (var concealed in new[] { false, true })
                {
                    refresh.Invoke(model, [visible with { Players = Array.AsReadOnly(visible.Players.Select(player => player.Seat == boss.Seat
                        ? player with { GeneralName = "不可见历史将名", IsGeneralPublic = concealed, IdentityConcealed = concealed } : player).ToArray()) }]);
                    historyField.SetValue(model, hpLossHistory);
                    var hidden = Project(logs[1]);
                    Program.Assert(hidden.Message.StartsWith(concealed ? "隐匿武将" : "暗将", StringComparison.Ordinal) &&
                        !hidden.Message.Contains(boss.GeneralName, StringComparison.Ordinal) && !hidden.Message.Contains(second.Name, StringComparison.Ordinal) &&
                        !hidden.ParticipantNames.Any(name => name.StartsWith(boss.GeneralName, StringComparison.Ordinal) || name.StartsWith(second.Name, StringComparison.Ordinal)),
                        "Current player-view concealment overrides both historical and current stage identities in text and participant highlighting.");
                }
            }
            finally { refresh.Invoke(model, [before]); historyField.SetValue(model, originalHistory); }
            Program.Assert(engine.Log.SequenceEqual(rawLog) && engine.Events.SequenceEqual(rawEvents) &&
                GameCheckpointJson.Serialize(engine.CreateCheckpoint()) == checkpoint,
                "Historical name correlation and privacy reprojection leave original logs, typed event history and checkpoint/replay content untouched.");

            object Build(IReadOnlyList<GameLogEntry> entries, IReadOnlyList<EventEnvelope> events) => build.Invoke(null, [entries, events, before.Players])!;
            static int Count(object value) => ((System.Collections.IEnumerable)value).Cast<object>().Count();
            static int Cut(object value)
            {
                var item = ((System.Collections.IEnumerable)value).Cast<object>().Single();
                return (int)item.GetType().GetProperty("LastPreviousFormLogSequence")!.GetValue(item)!;
            }
            static EventEnvelope Envelope(long sequence, long revision, IGameEvent payload) => new(new(sequence), null, sequence, revision, "ui-history", payload);
            BattleLogEntryViewModel Project(GameLogEntry entry) => (BattleLogEntryViewModel)project.Invoke(model,
                [new BattleLogEntryViewModel(entry.Sequence, entry.TurnNumber, entry.Type, "all", "", entry.Message, entry.ActorSeat, entry.TargetSeat)])!;
        }
    }
}

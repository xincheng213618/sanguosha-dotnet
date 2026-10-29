using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class WushuangUiChecks
{
    public static void MoLuBuOpeningAndNormalInitiativeAreVisible(string output)
    {
        var registry = StandardWushuangContentRegistry.Create();
        foreach (var seed in new[] { 17, 1 })
        {
            var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(seed) with
                { AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
            Accept(game, new StartGameCommand());
            for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            Accept(game, new RequestGeneralRosterCommand(0, true, game.Revision, game.PendingDecision!.PromptId));
            Accept(game, new SelectGeneralCommand(0, "ol:mo-lu-bu", game.Revision, game.PendingDecision!.PromptId));
            var store = new MemorySaveStore();
            store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
            using var vm = Create(new MemorySaveStore(), store, registry);
            LegacySaveFixtures.Load(vm, wushuang: true);
            Drive(vm, () => vm.IsWushuangOpening && vm.IsWushuangShopInteractive);
            var firstSeat = Program.Engine(vm).CreateWushuangSnapshot(0)!.FirstNormalSeat;
            vm.BuyWushuangOfferCommand.Execute(vm.WushuangOffers.First());
            Drive(vm, () => Program.Engine(vm).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            Program.Assert(vm.RoundText.Contains("夺炁") && vm.RoundText.Contains("额外出牌") && vm.TurnHeadline.Contains("夺炁额外出牌阶段") &&
                vm.Seats.Single(s => s.Seat == firstSeat).SeatLabel.Contains("先手") &&
                vm.Seats.Single(s => s.Seat != firstSeat).SeatLabel.Contains("后手"),
                "The opening names Duoqi explicitly and the seat labels show actual normal initiative even while Lu Bu temporarily acts first.");
            var window = new MainWindow(vm);
            try
            {
                Program.Render((FrameworkElement)window.Content, 1440, 880, Path.Combine(output, $"mo-lu-bu-opening-{firstSeat}.png"));
                vm.EndTurnCommand.Execute(null);
                Drive(vm, () => Program.Engine(vm).Events.Count(e => e.Payload is TurnStartedEvent) >= 2);
                Program.Assert(!vm.RoundText.Contains("夺炁") && !vm.TurnHeadline.Contains("夺炁额外出牌阶段") &&
                    vm.Seats.Single(s => s.Seat == firstSeat).SeatLabel.Contains("先手"),
                    "The phase indication expires at the actual normal return while the first actor label stays stable.");
            }
            finally { window.Close(); }
        }
    }

    public static void EntryShopAndIndependentColdRestore(string output)
    {
        foreach (var tactic in WushuangTactics.All)
        {
            var offer = new WushuangShopOfferViewModel("artwork", tactic.Name, tactic.Description, tactic.Cost, null, true, 0)
                { Kind = WushuangOfferKind.Tactic, TacticId = tactic.Id, Quality = tactic.Quality };
            Program.Assert(offer.Artwork is BitmapSource { IsFrozen: true, PixelWidth: > 0, PixelHeight: > 0 } &&
                ReferenceEquals(offer.Artwork, new WushuangTacticViewModel(tactic.Id).Icon),
                $"Tactic {tactic.Id} ({tactic.Name}) has a decoded local icon shared by shop offers and table badges.");
        }
        var ordinary = new MemorySaveStore();
        var wushuang = new MemorySaveStore();
        using var vm = Create(ordinary, wushuang);
        LegacySaveFixtures.Save(vm);
        var ordinaryManual = GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint);
        var window = new MainWindow(vm); window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            Layout(root);
            var lobby = Program.Find<HomeLobby>(root).Single();
            var entry = (Button)lobby.FindName("WushuangBattleButton");
            Program.Assert(vm.IsNewGameSetupOpen && entry.Visibility == Visibility.Visible && ReferenceEquals(entry.Command, vm.OpenWushuangModeCommand),
                "The home lobby exposes a visible, real single-player duel entry.");
            entry.Command.Execute(null);
            Program.Assert(vm.IsWushuangModeSelection && vm.IsModeConfigurationOpen && !vm.IsIdentityModeSelection &&
                !vm.IsTeamModeSelection && vm.TeamModeSetupText.Contains("两个额外技能"),
                "The duel has its own setup with one AI, a time limit and two extra skills, without an incorrect team or identity picker.");
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-setup.png"));
            vm.StartNewGameFromLobbyCommand.Execute(null);
            vm.ContinueFromIdentityRevealCommand.Execute(null);
            Drive(vm, () => vm.IsGeneralSelectionPending);
            vm.SelectGeneralChoiceCommand.Execute(vm.GeneralChoices.First());
            Drive(vm, () => Program.Engine(vm).CreateWushuangSnapshot(0)?.CanShop == true);
            var game = Program.Engine(vm);
            Program.Assert(game.CreateSnapshot(0).Players.Count == 2 && vm.TableModeText.Contains("1v1") &&
                vm.IdentityObjective.Contains("AI") && vm.WindowTitle.Contains("单骑无双") && vm.WushuangClockText.Contains(':') &&
                vm.WushuangClockCaption.Contains("超时比体力") && vm.WushuangRulesText.Contains("当前体力较高的一方获胜") &&
                game.WushuangRemainingMilliseconds is > 840_000 and <= WushuangMode.TimeLimitMilliseconds,
                "The entry starts a real two-seat engine and the table identifies its single-player duel and fifteen-minute countdown.");
            Layout(root);
            var panel = Program.Find<WushuangShopPanel>(root).Single();
            Program.Assert(vm.IsWushuangShopOpen && panel.Visibility == Visibility.Visible && vm.WushuangOffers.Count == 3 && vm.IsWushuangOpening &&
                vm.WushuangSkillSlots.Count == 2 && vm.WushuangSkillSlots.All(slot => slot.SkillId is null),
                "The selected game automatically opens a free three-choice initial tactic panel.");
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-opening.png"));
            var originalOffers = vm.WushuangOffers.ToArray();
            try
            {
                vm.WushuangOffers.Clear();
                foreach (var id in new[] { 2295, 2045, 2209 })
                {
                    var tactic = WushuangTactics.ById[id];
                    vm.WushuangOffers.Add(new("artwork-" + id, tactic.Name, tactic.Description, 0, null, true, game.Revision)
                        { Kind = WushuangOfferKind.Tactic, TacticId = id, Quality = tactic.Quality, IsOpening = true });
                }
                Layout(root);
                var artworkGrids = Program.Find<Grid>(panel).Where(grid => grid.Name == "WushuangTacticArtwork").ToArray();
                Program.Assert(artworkGrids.Length == vm.WushuangOffers.Count && artworkGrids.All(grid =>
                    ReferenceEquals(Program.Find<Image>(grid).Last().Source, ((WushuangShopOfferViewModel)grid.DataContext).Artwork)),
                    "The previously blank opening choices bind their actual tactic icon above the quality frame.");
                Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-tactic-icons.png"));
            }
            finally
            {
                vm.WushuangOffers.Clear();
                foreach (var savedOffer in originalOffers) vm.WushuangOffers.Add(savedOffer);
            }
            var initialTactic = vm.WushuangOffers.First(offer => offer.TacticId is { } id && WushuangTactics.ById[id].SkillId is not null);
            vm.BuyWushuangOfferCommand.Execute(initialTactic);
            Program.Assert(!game.IsFaulted && !vm.IsWushuangShopOpen &&
                (vm.CanStepAi || game.PendingDecision is { } next && next.SkillPrompt?.SkillId != WushuangMode.ModeId),
                "Selecting the free tactic retires the modal and leaves either a runnable engine or a genuine next human decision.");
            Drive(vm, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Layout(root);
            var tacticRail = (ScrollViewer)window.FindName("HumanWushuangTactics");
            var dock = (FrameworkElement)window.FindName("HandDock");
            var badge = vm.HumanPlayer!.WushuangTactics.Single();
            Program.Assert(badge.Id == initialTactic.TacticId && badge.Icon is not null && badge.Frame is { IsFrozen: true } &&
                vm.HumanSkillCards.All(skill => skill.ContentId != WushuangTactics.ById[badge.Id].SkillId) &&
                game.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == WushuangTactics.ById[badge.Id].SkillId) &&
                vm.WushuangSkillSlots.All(slot => slot.SkillId is null),
                "A real passive tactic keeps its executable rule but renders as an art badge, never as a skill button or an extra skill slot.");
            Program.Assert(tacticRail.Visibility == Visibility.Visible && tacticRail.ActualHeight > 40 &&
                tacticRail.TranslatePoint(new Point(), root).X > root.ActualWidth / 2 &&
                tacticRail.TranslatePoint(new Point(0, tacticRail.ActualHeight), root).Y <= dock.TranslatePoint(new Point(), root).Y &&
                vm.Seats.Single(seat => !seat.IsHuman).WushuangTactics.Count > 0,
                "Human tactic badges sit at the right above the hand dock; the opponent has a separate public tactic rail.");
            var opposingRail = Program.Find<ScrollViewer>(root).Single(view => view.Name == "OpponentWushuangTactics" &&
                view.DataContext is SeatViewModel { IsHuman: false });
            Program.Assert(opposingRail.ActualWidth > 40 && opposingRail.ActualHeight > 40 &&
                opposingRail.TranslatePoint(new Point(), root).X > 0,
                "The opponent's rail has real layout outside the portrait rather than collapsing under a large positive margin.");
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-tactics-table.png"));
            vm.OpenWushuangShopCommand.Execute(null); Layout(root);
            var previewIds = vm.WushuangOffers.Select(item => item.Id).ToArray();
            Program.Assert(vm.IsWushuangShopOpen && !vm.IsWushuangShopInteractive && previewIds.Length == 4 &&
                vm.WushuangOffers.All(item => item.IsReadOnly && !item.IsEnabled) && vm.WushuangPromptText == "暂时无法购买商品" &&
                !vm.RerollWushuangShopCommand.CanExecute(null) && !vm.ToggleWushuangShopLockCommand.CanExecute(null),
                "During play the actual upcoming page is visible, with every transaction disabled.");
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-preview.png"));
            vm.CloseWushuangShopCommand.Execute(null);
            Drive(vm, () => Program.Engine(vm).CreateWushuangSnapshot(0) is { CanShop: true, Phase: WushuangShopPhase.Shopping, Tokens: >= 2 });
            Program.Assert(vm.WushuangOffers.Select(item => item.Id).SequenceEqual(previewIds),
                "The next buying window uses the previewed goods without rolling a different page.");
            Drive(vm, () => HasAffordableSkillOffer(vm));
            var offer = vm.WushuangOffers.First(item => item.IsEnabled && item.SkillId is not null);
            var beforeTokens = game.CreateWushuangSnapshot(0)!.Tokens;
            vm.BuyWushuangOfferCommand.Execute(offer);
            Program.Assert(vm.IsWushuangShopOpen && game.CreateWushuangSnapshot(0)!.Tokens == beforeTokens - offer.Cost &&
                vm.WushuangSkillSlots.Count(slot => slot.SkillId is not null) == 1 &&
                game.CreateWushuangSnapshot(0)!.CanShop && !vm.BuyWushuangOfferCommand.CanExecute(offer) &&
                vm.WushuangOffers.Count == 4 && vm.WushuangOffers.Single(item => item.Id == offer.Id).IsPurchased,
                "One purchase pays once, keeps its sold card in position and leaves the shopping prompt open.");
            Method("FlushWushuangClock").Invoke(vm, null);
            var continueClockGuard = Field("_wushuangClockSubmitting").GetValue(vm);
            Field("_wushuangClockSubmitting").SetValue(vm, true);
            ((DispatcherTimer)Field("_wushuangClockTimer").GetValue(vm)!).Stop();
            try
            {
                LegacySaveFixtures.Save(vm, GameSaveSlot.Automatic);
                Program.Assert(vm.FlushPendingSave() && !vm.ContinueWushuangCommand.CanExecute(null),
                    "The existing shop must have a valid independent automatic checkpoint before its next committed action.");
                var continueWrites = wushuang.WriteCount;
                var ordinaryWrites = ordinary.WriteCount;
                var savedCommands = game.AcceptedCommands.Count;
                var previousAutomatic = GameCheckpointJson.Serialize(wushuang.Read(GameSaveSlot.Automatic).Checkpoint);
                vm.ToggleWushuangShopLockCommand.Execute(null);
                Program.Assert(game.CreateWushuangSnapshot(0)!.IsLocked, "The visible lock commits to the native private shop state.");
                ((DispatcherTimer)Field("_saveTimer").GetValue(vm)!).Stop();
                var pendingViews = Enumerable.Range(0, game.CreateSnapshot(0).Players.Count)
                    .Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray();
                var pendingCommands = CommandJson.Serialize(game.AcceptedCommands);
                var pendingRemaining = game.WushuangRemainingMilliseconds;
                Program.Assert(game.AcceptedCommands.Count == savedCommands + 1 && game.AcceptedCommands.Last() is ToggleWushuangShopLockCommand &&
                    Field("_autoSavePending").GetValue(vm) is false && wushuang.WriteCount == continueWrites &&
                    GameCheckpointJson.Serialize(wushuang.Read(GameSaveSlot.Automatic).Checkpoint) == previousAutomatic,
                    "The real shop-lock command must be accepted once while its automatic save is still pending and the previous checkpoint remains on disk.");
                LegacySaveFixtures.Save(vm, GameSaveSlot.Automatic);
                LegacySaveFixtures.Load(vm, GameSaveSlot.Automatic, wushuang: true);
                game = Program.Engine(vm);
                Console.WriteLine($"Wushuang pending Continue: saved={savedCommands}, pending={savedCommands + 1}, restored={game.AcceptedCommands.Count}, " +
                    $"independent writes={continueWrites}->{wushuang.WriteCount}, ordinary writes={ordinaryWrites}->{ordinary.WriteCount}.");
                Program.Assert(!vm.HasSaveError && vm.IsWushuangTable && game.CreateWushuangSnapshot(0)!.IsLocked &&
                    CommandJson.Serialize(game.AcceptedCommands) == pendingCommands && game.WushuangRemainingMilliseconds == pendingRemaining &&
                    Enumerable.Range(0, pendingViews.Length).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).SequenceEqual(pendingViews) &&
                    wushuang.WriteCount == continueWrites + 1 && ordinary.WriteCount == ordinaryWrites,
                    "Continuing the duel must first persist and restore its latest committed lock, every private view, native journal and remaining time without writing the ordinary store.");
            }
            finally
            {
                Field("_wushuangClockSubmitting").SetValue(vm, continueClockGuard);
                Method("SyncWushuangClockAfterCommand").Invoke(vm, null);
            }
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-shop.png"));
            var sidebar = (FrameworkElement)window.FindName("TableSidebar");
            var shopEnd = panel.TranslatePoint(new Point(panel.ActualWidth, panel.ActualHeight), root);
            Program.Assert(shopEnd.X <= sidebar.TranslatePoint(new Point(), root).X &&
                shopEnd.Y <= dock.TranslatePoint(new Point(), root).Y &&
                ((FrameworkElement)window.FindName("ActionBar")).Opacity == 0,
                "The shop scales inside the battlefield, aligned above the hand dock and to the left of the log sidebar.");
            var priceSlots = Program.Find<Grid>(panel).Where(grid => grid.Name == "WushuangPriceSlot").ToArray();
            Program.Assert(priceSlots.Length == 4 &&
                priceSlots.Select(slot => slot.TranslatePoint(new Point(), panel).Y).Distinct().Count() == 1,
                "All four price and sold-state slots share one baseline, independent of offer type and description length.");
            var footerIcons = new[] { "WushuangEndIcon", "WushuangLockIcon", "WushuangRefreshIcon" }
                .Select(name => (FrameworkElement)panel.FindName(name)).ToArray();
            Program.Assert(footerIcons.Select(icon => icon.TranslatePoint(new Point(), panel).Y).Distinct().Count() == 1,
                "Ending, lock and refresh icons share one footer baseline; the token cost has its own row.");
            var refresh = (Button)panel.FindName("WushuangRerollButton");
            var locked = (Button)panel.FindName("WushuangLockButton");
            Program.Assert(refresh.TranslatePoint(new Point(), panel).Y > panel.ActualHeight - 130 &&
                locked.TranslatePoint(new Point(), panel).X < refresh.TranslatePoint(new Point(), panel).X &&
                vm.WushuangOffers.Where(item => item.Cost == 0 && !item.IsPurchased).All(item => item.PriceText == "免费" && !item.ShowsPriceIcon) &&
                vm.WushuangOffers.Where(item => !item.IsAffordable).All(item => item.PriceForeground == "#F04436"),
                "Footer controls sit at the lower right, free goods use a label without a token icon and unaffordable prices are red.");
            var prompt = game.PendingDecision!.PromptId;
            Field("_wushuangShopOpenedAt").SetValue(vm, Stopwatch.GetTimestamp() - Stopwatch.Frequency * 31);
            Method("OnWushuangClockTick").Invoke(vm, [null, EventArgs.Empty]);
            Program.Assert(game.PendingDecision?.PromptId != prompt,
                "The twenty-second intermission timeout commits finish and continues the native game flow.");
            LegacySaveFixtures.Save(vm);
            Program.Assert(wushuang.GetSavedAt(GameSaveSlot.Manual) is not null && !vm.HasManualSave &&
                GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint) == ordinaryManual,
                "The shared Save action routes to the duel's independent manual file and preserves the ordinary manual game.");
            var saved = SnapshotJson.Serialize(game.CreateSnapshot(0));
            using (var cold = Create(ordinary, wushuang))
            {
                LegacySaveFixtures.Load(cold, wushuang: true);
                Program.Assert(!cold.HasSaveError && cold.IsWushuangTable &&
                    SnapshotJson.Serialize(Program.Engine(cold).CreateSnapshot(0)) == saved &&
                    cold.HumanPlayer!.WushuangTactics.SequenceEqual(vm.HumanPlayer!.WushuangTactics) &&
                    cold.HumanSkillCards.All(skill => skill.ContentId != WushuangTactics.ById[badge.Id].SkillId) &&
                    cold.WushuangSkillSlots.Single(slot => slot.SkillId is not null).SkillId == offer.SkillId,
                    "A new desktop configured with ordinary content cold-restores the exact duel journal, acquired skill and remaining time.");
            }
            vm.OpenLobbyCategoryCommand.Execute("identity");
            vm.StartNewGameFromLobbyCommand.Execute(null);
            Program.Assert(!vm.IsWushuangTable &&
                GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint) == ordinaryManual,
                "Leaving the duel starts an independent mode while preserving offline fixture files.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    public static void FullSkillSlotsRequireVisibleReplacement(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages: [new QuietFixture()]);
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(17) with
            { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 100; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Accept(game, new SelectGeneralCommand(0, "fixture:wushuang-ui-human", game.Revision, game.PendingDecision!.PromptId));
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var vm = Create(new MemorySaveStore(), store, registry);
        LegacySaveFixtures.Load(vm, wushuang: true);
        Drive(vm, () => HasAffordableSkillOffer(vm));
        game = Program.Engine(vm);
        vm.OpenWushuangShopCommand.Execute(null);
        for (var slotNumber = 0; slotNumber < 2; slotNumber++)
        {
            var current = vm.WushuangOffers.First(item => item.SkillId is not null && item.IsEnabled);
            vm.BuyWushuangOfferCommand.Execute(current);
            vm.CloseWushuangShopCommand.Execute(null);
            Drive(vm, () => HasAffordableSkillOffer(vm));
        }
        Program.Assert(game.CreateWushuangSnapshot(0)!.ExtraSkillIds.Count == 2,
            "The bounded native fixture buys two real skills over consecutive shopping windows.");
        var replacement = vm.WushuangOffers.First(offer => offer.SkillId is not null && offer.IsEnabled);
        Program.Assert(replacement.RequiresReplacement, "A full rack marks the skill card before a replacement purchase is selected.");
        var shop = new WushuangShopPanel { DataContext = vm };
        Program.Render(shop, 1240, 650, Path.Combine(output, "wushuang-full-slots.png"));
        var before = game.CreateWushuangSnapshot(0)!;
        vm.BuyWushuangOfferCommand.Execute(replacement);
        Program.Assert(vm.IsWushuangReplacementPending && vm.WushuangReplacementTitle.Contains(replacement.Name) &&
            game.CreateWushuangSnapshot(0)!.Tokens == before.Tokens &&
            game.CreateWushuangSnapshot(0)!.ExtraSkillIds.SequenceEqual(before.ExtraSkillIds),
            "Selecting a third skill opens the explicit replacement UI before any payment or skill removal.");
        var window = new MainWindow(vm); window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            Layout(root);
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-replacement.png"));
            var slot = vm.WushuangSkillSlots.First();
            vm.ReplaceWushuangSkillCommand.Execute(slot);
            Program.Assert(!vm.IsWushuangReplacementPending && game.CreateWushuangSnapshot(0)!.ExtraSkillIds.Count == 2 &&
                !game.CreateWushuangSnapshot(0)!.ExtraSkillIds.Contains(slot.SkillId!) &&
                game.CreateWushuangSnapshot(0)!.ExtraSkillIds.Contains(replacement.SkillId!) &&
                game.CreateWushuangSnapshot(0)!.Tokens == before.Tokens - replacement.Cost,
                "Confirming one visible owned slot replaces only that grant and pays exactly once, keeping the two-skill limit.");
            Program.Assert(TableShortcutChecks.Input(window, Key.Escape) && !vm.IsWushuangShopOpen,
                "Escape closes the modal shop and returns keyboard ownership to the normal table.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    public static void FreeRefreshAndExpandedSlotsFollowNativeSnapshot(string output)
    {
        foreach (var (seed, tactic) in new[] { (148, 2291), (57, 2293) })
        {
            var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages: [new QuietFixture()]);
            var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(seed) with
                { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
            Accept(game, new StartGameCommand());
            for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            Accept(game, new SelectGeneralCommand(0, "fixture:wushuang-ui-human", game.Revision, game.PendingDecision!.PromptId));
            var store = new MemorySaveStore();
            store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
            using var vm = Create(new MemorySaveStore(), store, registry);
            LegacySaveFixtures.Load(vm, wushuang: true);
            Drive(vm, () => vm.IsWushuangOpening && vm.IsWushuangShopInteractive);
            vm.BuyWushuangOfferCommand.Execute(vm.WushuangOffers.Single(o => o.TacticId == tactic));
            Drive(vm, () => Program.Engine(vm).CreateWushuangSnapshot(0) is { CanShop: true, Phase: WushuangShopPhase.Shopping });
            game = Program.Engine(vm);
            if (tactic == 2291)
            {
                var tokens = game.CreateWushuangSnapshot(0)!.Tokens;
                Program.Assert(vm.WushuangRerollPriceText == "免费" && !vm.WushuangRerollShowsToken && vm.WushuangRerollText.Contains('2'),
                    "A native free refresh grant updates the visible price, token icon and remaining-count tooltip.");
                vm.RerollWushuangShopCommand.Execute(null);
                vm.RerollWushuangShopCommand.Execute(null);
                Program.Assert(vm.WushuangRerollPriceText == "1" && vm.WushuangRerollShowsToken &&
                    game.CreateWushuangSnapshot(0)!.Tokens == tokens,
                    "After two native free commands the UI returns to the paid price without spending currency.");
            }
            else Program.Assert(vm.WushuangSkillSlots.Count == 3 && vm.WushuangSkillSlotsText.Contains("/3"),
                "The expanded native capacity exposes a real third selectable skill slot.");
        }
    }

    public static void MixedShopEquipmentShowsItsNativePurchase(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages: [new QuietFixture()]);
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(80) with
            { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Accept(game, new SelectGeneralCommand(0, "fixture:wushuang-ui-human", game.Revision, game.PendingDecision!.PromptId));
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var vm = Create(new MemorySaveStore(), store, registry);
        LegacySaveFixtures.Load(vm, wushuang: true);
        Drive(vm, () => vm.IsWushuangOpening && vm.IsWushuangShopInteractive);
        vm.BuyWushuangOfferCommand.Execute(vm.WushuangOffers.First());
        Drive(vm, () => Program.Engine(vm).CreateWushuangSnapshot(0) is { CanShop: true, Phase: WushuangShopPhase.Shopping });
        var equipment = vm.WushuangOffers.FirstOrDefault(o => o.IsEquipment && o.IsEnabled);
        Program.Assert(vm.WushuangOffers.Count(o => o.Kind == WushuangOfferKind.Skill) == 2 &&
            vm.WushuangOffers.Count(o => o.Kind == WushuangOfferKind.Card) == 1,
            "The verified mixed page contains two skills and one card, proving there is no fixed type quota.");
        Program.Assert(equipment is { CardOfferId: not null, TypeText: "装备", Artwork: not null } &&
            equipment.DetailText.Contains("立即装备") && equipment.DetailText.Contains("销毁"),
            "The real random page exposes an enabled equipment card with its source identity, artwork and immediate-equip/destruction explanation.");
        var panel = new WushuangShopPanel { DataContext = vm };
        Program.Render(panel, 1240, 650, Path.Combine(output, "wushuang-mixed-equipment-shop.png"));
        Program.Assert(Program.Find<TextBlock>(panel).Any(text => text.Text == "装备"),
            "The actual offer template labels equipment rather than presenting it as an ordinary hand-card purchase.");
        vm.BuyWushuangOfferCommand.Execute(equipment);
        Program.Assert(!Program.Engine(vm).IsFaulted && Program.Engine(vm).CreateSnapshot(0).Players[0].Equipment
            .Any(card => card.Kind == equipment!.CardKind) && vm.WushuangOffers.Single(o => o.Id == equipment!.Id).IsPurchased,
            "The visible buy command equips the native card and retains its sold position.");
        Program.Assert(vm.WushuangOffers.Where(o => o.IsEquipment).All(o => o.CardKind is { } kind &&
            WushuangCatalogue.IsCardAllowed(kind) && EquipmentCatalog.Get(kind).Slot is EquipmentSlot.Weapon or EquipmentSlot.Armor),
            "The visible equipment shop contains basic weapons and armor, without treasures or mode-exclusive equipment.");
    }

    public static void ActiveTacticKeepsAnExecutableEntry(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages: [new QuietFixture("standard:peach_garden")]);
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(31) with
            { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Accept(game, new SelectGeneralCommand(0, "fixture:wushuang-ui-human", game.Revision, game.PendingDecision!.PromptId));
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var vm = Create(new MemorySaveStore(), store, registry);
        LegacySaveFixtures.Load(vm, wushuang: true);
        Drive(vm, () => vm.IsWushuangOpening && vm.IsWushuangShopInteractive);
        vm.BuyWushuangOfferCommand.Execute(vm.WushuangOffers.Single(o => o.TacticId == 2365));
        Drive(vm, () => Program.Engine(vm).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        game = Program.Engine(vm);
        var chip = vm.HumanSkillCards.Single(s => s.ContentId == "wushuang:tactic-2365");
        Program.Assert(chip.IsAvailable && vm.HumanPlayer!.WushuangTactics.Any(t => t.Id == 2365) &&
            game.CreateWushuangSnapshot(0)!.ExtraSkillIds.Count == 0,
            "An active tactic keeps its public badge and a real executable entry while consuming no extra skill slot.");
        vm.ActivateHumanSkillCommand.Execute(chip);
        Drive(vm, () => Program.Engine(vm).ResolutionStack.OfType<ProgramSkillFrame>().Any(f =>
            f.SkillId == "wushuang:tactic-2365" && f.OwnedCardSelection is not null));
        Program.Assert(game.PendingDecision?.SkillPrompt?.SkillId == "wushuang:tactic-2365",
            "Clicking the zero-input entry reaches its native physical-card selection before any actual recast payment.");
    }

    public static void SanchenSelectionSurvivesClockRefresh(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages:
            [new QuietFixture("standard:peach_garden", "ol:sanchen")]);
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(31) with
            { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Accept(game, new SelectGeneralCommand(0, "fixture:wushuang-ui-human", game.Revision, game.PendingDecision!.PromptId));
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var vm = Create(new MemorySaveStore(), store, registry);
        LegacySaveFixtures.Load(vm, wushuang: true);
        Drive(vm, () => Program.Engine(vm).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        game = Program.Engine(vm);
        vm.ActivateHumanSkillCommand.Execute(vm.HumanSkillCards.Single(skill => skill.ContentId == "ol:sanchen"));
        vm.SelectTargetCommand.Execute(vm.Seats.Single(seat => seat.Seat == 0));
        vm.ConfirmSelectedCommand.Execute(null);
        Drive(vm, () => vm.IsOwnedCardSelectionPending);
        Program.Assert(vm.ChoiceSnapshot.PendingDecision is { RequiredCardCount: 3, SkillPrompt.SkillId: "ol:sanchen" },
            "Sanchen must reach its native three-card discard draft in the timed duel.");
        var promptId = game.PendingDecision!.PromptId;
        var moves = game.CardMovements.Count;
        var cards = vm.Hand.Where(card => card.IsPlayable).Take(3).ToArray();
        var window = new MainWindow(vm);
        var root = (FrameworkElement)window.Content;
        try
        {
            Layout(root);
            var items = (ItemsControl)window.FindName("HandCards");
            var panel = Program.Find<HandPanel>(items).Single();
            var press = typeof(HandPanel).GetMethod("PressCardAt", BindingFlags.NonPublic | BindingFlags.Instance)!;
            for (var index = 0; index < cards.Length; index++)
            {
                Press(cards[index]);
                TickAndAssert(cards.Take(index + 1).Select(card => card.Id).ToArray());
            }
            Program.Assert(vm.CanConfirmSelected, "All three persistent selections must enable confirmation.");
            Press(cards[1]);
            TickAndAssert([cards[0].Id, cards[2].Id]);
            Program.Assert(!vm.CanConfirmSelected, "Clicking a selected card again must cancel only that card.");
            Press(cards[1]);
            TickAndAssert(cards.Select(card => card.Id).ToArray());
            Program.Render(root, 1440, 880, Path.Combine(output, "wushuang-sanchen-persistent-selection.png"));
            vm.ConfirmSelectedCommand.Execute(null);
            Program.Assert(cards.All(card => game.CardMovements.Skip(moves).Any(move =>
                    move.CardId == card.Id && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile)) &&
                vm.SelectedTargetCardIds.Count == 0 && vm.Hand.All(card => !card.IsSelected),
                "Confirmation must discard the exact selected cards and retire their local highlights.");

            void Press(CardViewModel card)
            {
                var container = (FrameworkElement)items.ItemContainerGenerator.ContainerFromItem(card);
                var point = container.TranslatePoint(new Point(8, 80), panel);
                Program.Assert(press.Invoke(panel, [point]) is true, "The real hand press must select its physical slot.");
                var button = Program.Find<Button>(container).Single();
                button.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                    { RoutedEvent = UIElement.MouseLeftButtonUpEvent });
            }

            void TickAndAssert(int[] selected)
            {
                var revision = game.Revision;
                Field("_wushuangClockCommittedMilliseconds").SetValue(vm, -5000L);
                Method("OnWushuangClockTick").Invoke(vm, [null, EventArgs.Empty]);
                Layout(root);
                Program.Assert(game.Revision > revision && game.PendingDecision?.PromptId == promptId &&
                    game.CardMovements.Count == moves && vm.SelectedTargetCardIds.ToHashSet().SetEquals(selected),
                    "A real clock debit must retain the same unpaid draft and its exact selected IDs.");
                Program.Assert(vm.Hand.Where(card => card.IsSelected).Select(card => card.Id).ToHashSet().SetEquals(selected),
                    "Clock reconciliation cleared Sanchen's hand highlights while retaining its selected count.");
                foreach (var card in cards)
                {
                    var container = (FrameworkElement)items.ItemContainerGenerator.ContainerFromItem(card);
                    var surface = Program.Find<CardLiftSurface>(container).Single();
                    var outline = Program.Find<Border>(container).Single(border => border.Name == "CardSelectedOutline");
                    var check = Program.Find<Border>(container).Single(border => border.Name == "SelectedCheck");
                    var isSelected = selected.Contains(card.Id);
                    Program.Assert(surface.IsSelected == isSelected && outline.Opacity == (isSelected ? 1 : 0) &&
                        check.Visibility == (isSelected ? Visibility.Visible : Visibility.Collapsed) &&
                        (!isSelected || surface.RenderTransform.Value.OffsetY == -18),
                        "Released cards must keep their selected lift, outline and check mark after a clock refresh.");
                }
            }
        }
        finally { window.Content = null; window.Close(); }
    }

    public static void TacticCountsWrapWithoutClipping(string output)
    {
        using var vm = Create(new MemorySaveStore(), new MemorySaveStore());
        vm.OpenWushuangModeCommand.Execute(null);
        vm.StartNewGameFromLobbyCommand.Execute(null);
        vm.ContinueFromIdentityRevealCommand.Execute(null);
        Drive(vm, () => vm.IsGeneralSelectionPending);
        vm.SelectGeneralChoiceCommand.Execute(vm.GeneralChoices.First());
        Drive(vm, () => vm.IsWushuangOpening && vm.IsWushuangShopInteractive);
        vm.BuyWushuangOfferCommand.Execute(vm.WushuangOffers.First());
        Drive(vm, () => Program.Engine(vm).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        using var host = new NativeResponseStatusChecks.NativeHost(vm);
        CheckTacticRailCounts(vm, host.Window, host.Root, output);
    }

    private static void CheckTacticRailCounts(MainViewModel vm, MainWindow window, FrameworkElement root, string output)
    {
        var human = vm.HumanPlayer!;
        var opponent = vm.Seats.Single(seat => !seat.IsHuman);
        var originalHuman = human.WushuangTactics;
        var originalOpponent = opponent.WushuangTactics;
        var notify = typeof(ObservableObject).GetMethod("RaisePropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var clock = (DispatcherTimer)Field("_wushuangClockTimer").GetValue(vm)!;
        var wasRunning = clock.IsEnabled;
        var wasSubmitting = Field("_wushuangClockSubmitting").GetValue(vm);
        clock.Stop();
        Field("_wushuangClockSubmitting").SetValue(vm, true);
        try
        {
            var tactics = WushuangTactics.All.Take(20).Select(tactic => new WushuangTacticViewModel(tactic.Id)).ToArray();
            foreach (var (width, height) in new[] { (1120, 740), (1440, 880), (1920, 1080) })
            {
                root.Width = width;
                root.Height = height;
                window.SizeToContent = SizeToContent.WidthAndHeight;
                double? bottom = null;
                foreach (var count in new[] { 1, 5, 12, 20, 0 })
                {
                    human.WushuangTactics = tactics.Take(count).ToArray();
                    opponent.WushuangTactics = tactics.Take(count == 12 ? 10 : count).ToArray();
                    notify.Invoke(human, [string.Empty]);
                    notify.Invoke(opponent, [string.Empty]);
                    Program.Render(root, width, height, count is 12 or 20
                        ? Path.Combine(output, $"wushuang-tactics-{count}-{width}.png") : null);
                    var rail = (ScrollViewer)window.FindName("HumanWushuangTactics");
                    var opposingRail = Program.Find<ScrollViewer>(root).Single(view => view.Name == "OpponentWushuangTactics" &&
                        ReferenceEquals(view.DataContext, opponent));
                    if (count == 0)
                    {
                        Program.Assert(rail.Visibility == Visibility.Collapsed && !opposingRail.IsVisible,
                            "Empty tactic rails must leave no visible badges.");
                        continue;
                    }
                    CheckHumanRail();
                    CheckRail(opposingRail, opponent.WushuangTactics, 5, bounded: true);
                    foreach (var icon in Program.Find<System.Windows.Shapes.Ellipse>(opposingRail).Where((_, index) => index == 0 || index == ((ItemsControl)opposingRail.Content).Items.Count - 1))
                    {
                        // The bounded opponent rail must expose its lower rows through real scrolling.
                        opposingRail.ScrollToVerticalOffset(0);
                        icon.BringIntoView();
                        window.UpdateLayout();
                        root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                        var hit = root.InputHitTest(icon.TranslatePoint(new Point(icon.ActualWidth / 2, icon.ActualHeight / 2), root));
                        Program.Assert(hit is DependencyObject element && opposingRail.IsAncestorOf(element),
                            $"Both the first and final tactic rows must remain reachable by pointer input: {opposingRail.Name}, " +
                            $"count={count}, width={width}, tactic={icon.DataContext}, visible={icon.IsVisible}, hit={hit}.");
                    }
                    opposingRail.ScrollToTop();
                    var currentBottom = rail.TranslatePoint(new Point(0, rail.ActualHeight), root).Y;
                    bottom ??= currentBottom;
                    var dock = (FrameworkElement)window.FindName("HandDock");
                    Program.Assert(Math.Abs(currentBottom - bottom.Value) < 0.1 &&
                        currentBottom <= dock.TranslatePoint(new Point(), root).Y &&
                        rail.TranslatePoint(new Point(), root).Y >= 0 &&
                        opposingRail.TranslatePoint(new Point(), root).X >= 0,
                        $"At {width}px, the human single row must keep a fixed bottom above the hand dock, with opponent tactics inside the table.");
                }
            }
        }
        finally
        {
            human.WushuangTactics = originalHuman;
            opponent.WushuangTactics = originalOpponent;
            notify.Invoke(human, [string.Empty]);
            notify.Invoke(opponent, [string.Empty]);
            Field("_wushuangClockSubmitting").SetValue(vm, wasSubmitting);
            if (wasRunning) clock.Start();
            Layout(root);
        }

        void CheckHumanRail()
        {
            var rail = (ScrollViewer)window.FindName("HumanWushuangTactics");
            var items = (ItemsControl)window.FindName("HumanTacticItems");
            var viewport = (ScrollViewer)window.FindName("HumanTacticsViewport");
            var toggle = (Button)window.FindName("ToggleHumanTacticsButton");
            var previous = (Button)window.FindName("PreviousHumanTacticsButton");
            var next = (Button)window.FindName("NextHumanTacticsButton");
            var panel = Program.Find<StackPanel>(items).Single(element => element.IsItemsHost);
            var expected = human.WushuangTactics;
            var commands = Program.Engine(vm).AcceptedCommands.Count;
            Program.Assert(items.Items.Cast<WushuangTacticViewModel>().SequenceEqual(expected) &&
                panel.Orientation == Orientation.Horizontal && Math.Abs(panel.ActualHeight - 66) < 0.1 &&
                Math.Abs(rail.ActualHeight - 66) < 0.1 && Math.Abs(viewport.ActualHeight - 66) < 0.1 &&
                rail.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled && rail.HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled &&
                viewport.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled && viewport.HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden,
                "The human rail retains every owned tactic in one fixed-height horizontal row, with paging instead of wrapping.");
            for (var index = 0; index < expected.Count; index++)
            {
                var container = (FrameworkElement)items.ItemContainerGenerator.ContainerFromIndex(index);
                var position = container.TranslatePoint(new Point(), panel);
                Program.Assert(Math.Abs(position.X - index * 62) < 0.1 && Math.Abs(position.Y) < 0.1,
                    "Clipped human tactics must retain their source order on the same single row.");
                CheckBadge(container, expected[index]);
            }

            var visible = CheckPage();
            if (expected.Count <= 11)
            {
                Program.Assert(!toggle.IsVisible && !previous.IsVisible && !next.IsVisible && visible.SequenceEqual(expected),
                    "The one- and five-tactic cases fit directly without folding or paging controls.");
            }
            else
            {
                Program.Assert(toggle.IsVisible && !previous.IsVisible && !next.IsVisible &&
                    visible.SequenceEqual(expected.Take(3)) && viewport.HorizontalOffset < 0.1,
                    "Twelve or twenty tactics retain every item while initially showing only the first three.");
                Click(toggle);
                visible = CheckPage();
                Program.Assert(visible.SequenceEqual(expected.Take(visible.Length)) && visible.Length <= 11 &&
                    previous.IsVisible && next.IsVisible && !previous.IsEnabled && next.IsEnabled,
                    "Expansion exposes the first page of at most eleven tactics and enables only forward paging.");
                var seen = visible.ToHashSet();
                for (var page = 0; next.IsEnabled && page < expected.Count; page++)
                {
                    var offset = viewport.HorizontalOffset;
                    Click(next);
                    visible = CheckPage();
                    Program.Assert(viewport.HorizontalOffset > offset, "The real next-page button advances the human tactic viewport.");
                    seen.UnionWith(visible);
                }
                Program.Assert(seen.SetEquals(expected) && visible[^1] == expected[^1] && !next.IsEnabled && previous.IsEnabled,
                    "Real forward paging reaches every owned tactic and the final icon without exposing another page.");
                for (var page = 0; previous.IsEnabled && page < expected.Count; page++) Click(previous);
                var firstPage = CheckPage();
                Program.Assert(viewport.HorizontalOffset < 0.1 && !previous.IsEnabled &&
                    firstPage.SequenceEqual(expected.Take(firstPage.Length)),
                    "Backward paging returns to the first single-row page.");
                Click(toggle);
                Program.Assert(CheckPage().SequenceEqual(expected.Take(3)) && viewport.HorizontalOffset < 0.1 &&
                    !previous.IsVisible && !next.IsVisible,
                    "Collapsing again restores the first three tactics without removing the remaining owned items.");
            }
            Program.Assert(Program.Engine(vm).AcceptedCommands.Count == commands,
                "Inspecting and paging passive tactic badges must not submit gameplay commands.");

            WushuangTacticViewModel[] CheckPage()
            {
                var clip = viewport.TransformToVisual(root).TransformBounds(new Rect(viewport.RenderSize));
                var containers = Enumerable.Range(0, items.Items.Count)
                    .Select(index => (FrameworkElement)items.ItemContainerGenerator.ContainerFromIndex(index)).Where(container =>
                    {
                        var bounds = container.TransformToVisual(root).TransformBounds(new Rect(container.RenderSize));
                        return clip.Contains(new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2));
                    }).ToArray();
                Program.Assert(containers.Length is > 0 and <= 11,
                    "Every human tactic page exposes between one and eleven single-row badges.");
                foreach (var container in new[] { containers[0], containers[^1] }.Distinct())
                {
                    var badge = Program.Find<FrameworkElement>(container).Single(element => element.Name == "TacticBadge");
                    AssertInput(Program.Find<System.Windows.Shapes.Ellipse>(badge).Single(), badge);
                }
                return containers.Select(container => (WushuangTacticViewModel)container.DataContext).ToArray();
            }
            void Click(Button button)
            {
                Program.Assert(button.IsVisible && button.IsEnabled, "Tactic paging requires its actual available control.");
                AssertInput(button, button);
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
                window.UpdateLayout();
                root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
                window.UpdateLayout();
            }
            void AssertInput(FrameworkElement target, FrameworkElement owner)
            {
                var hit = root.InputHitTest(target.TranslatePoint(new Point(target.ActualWidth / 2, target.ActualHeight / 2), root));
                Program.Assert(target.IsVisible && target.ActualWidth > 0 && target.ActualHeight > 0 && hit is DependencyObject element &&
                    (ReferenceEquals(owner, element) || owner.IsAncestorOf(element)),
                    $"The actual human tactic icon or page control must receive pointer input at {root.RenderSize}: {target.Name}, hit={hit}.");
            }
        }

        static void CheckBadge(FrameworkElement container, WushuangTacticViewModel tactic)
        {
            var badge = Program.Find<FrameworkElement>(container).Single(element => element.Name == "TacticBadge");
            Program.Assert(badge.IsHitTestVisible && badge.Focusable && Equals(badge.ToolTip, tactic.Tooltip) &&
                tactic.Tooltip.Contains(WushuangTactics.ById[tactic.Id].Description, StringComparison.Ordinal),
                "Each named tactic badge retains input access and its complete actual source-rule tooltip.");
        }

        static void CheckRail(ScrollViewer rail, IReadOnlyList<WushuangTacticViewModel> tactics, int columns, bool bounded = false)
        {
            var items = (ItemsControl)rail.Content;
            var panel = Program.Find<WrapPanel>(items).Single();
            var rows = (tactics.Count + columns - 1) / columns;
            Program.Assert(items.Items.Cast<WushuangTacticViewModel>().SequenceEqual(tactics) &&
                Math.Abs(panel.ActualHeight - rows * 66) < 0.1 &&
                rail.VerticalScrollBarVisibility == (bounded ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled) &&
                Math.Abs(rail.ActualHeight - Math.Min(rows, bounded ? 2 : rows) * 66) < 0.1 &&
                Math.Abs(rail.ScrollableHeight - (bounded ? Math.Max(0, rows - 2) * 66 : 0)) < 0.1,
                $"All {tactics.Count} tactics must retain order and fit {columns} columns; only opponent rows beyond two may scroll.");
            for (var index = 0; index < tactics.Count; index++)
            {
                var container = (FrameworkElement)items.ItemContainerGenerator.ContainerFromIndex(index);
                var position = container.TranslatePoint(new Point(), panel);
                Program.Assert(Math.Abs(position.X - index % columns * 62) < 0.1 &&
                    Math.Abs(position.Y - index / columns * 66) < 0.1,
                    "Each tactic must keep its icon/name cell and full hover description on the expected row.");
                CheckBadge(container, tactics[index]);
            }
        }
    }

    private static MainViewModel Create(IGameSaveStore ordinary, IGameSaveStore wushuang, ContentRegistry? registry = null) =>
        new(false, 17, true, ordinary, contentRegistry: registry, wushuangSaveStore: wushuang,
            shanheSaveStore: new MemoryShanheCampaignSaveStore())
        { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
    public static void JinzhiSelectsMaterialsBeforeBasicCard(string output)
    {
        const string skillId = "wushuang:skill-3252";
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages:
            [new QuietFixture("standard:dodge", skillId, jinzhiFixture: true)]);
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(31) with
            { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Accept(game, new SelectGeneralCommand(0, "fixture:wushuang-ui-human", game.Revision, game.PendingDecision!.PromptId));
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var vm = Create(new MemorySaveStore(), store, registry);
        LegacySaveFixtures.Load(vm, wushuang: true);
        Drive(vm, () => Program.Engine(vm).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        game = Program.Engine(vm);
        var window = new MainWindow(vm);
        try
        {
            var chip = vm.HumanSkillCards.Single(skill => skill.ContentId == skillId);
            Program.Assert(chip.IsAvailable && !chip.IsLocked && chip.TypeText.Contains("主动", StringComparison.Ordinal) &&
                !vm.SupplementalActiveSkillActions.Any(action => action.ProgramSkillId == skillId),
                "Jinzhi has one optional active rail entry, without a lock or per-material duplicate buttons.");
            for (var ordinal = 1; ordinal <= 3; ordinal++)
            {
                var outputKind = new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash }[ordinal - 1];
                var revision = game.Revision;
                var moves = game.CardMovements.Count;
                vm.ActivateHumanSkillCommand.Execute(vm.HumanSkillCards.Single(skill => skill.ContentId == skillId));
                Program.Assert(vm.IsActiveSkillSelectionPending && !vm.CanConfirmSelected && vm.CanCancelAction &&
                    vm.CardNameSelectionChoices.Count == 0 && game.Revision == revision,
                    "Opening Jinzhi selects only its unpaid materials, before exposing card names.");
                var materials = vm.Hand.Where(card => card.IsPlayable).GroupBy(card => card.SuitGlyph)
                    .SelectMany(group => group.Take(1)).Take(ordinal).ToList();
                materials.AddRange(vm.Hand.Where(card => card.IsPlayable && !materials.Contains(card)).Take(ordinal - materials.Count));
                foreach (var material in materials) vm.SelectCardCommand.Execute(material);
                Program.Assert(vm.HasCardNameSelection && !vm.CanConfirmSelected &&
                    vm.CardNameSelectionChoices.Count == vm.CardNameSelectionChoices.Select(choice => choice.Parameters["played-kind"]).Distinct().Count() &&
                    new[] { "Slash", "FireSlash", "ThunderSlash" }.All(kind => vm.CardNameSelectionChoices.Any(choice => choice.Parameters["played-kind"] == kind)),
                    "An exact same-color set exposes one choice per basic type, including both elemental Slashes; choosing materials alone cannot commit.");
                var staleChoice = vm.CardNameSelectionChoices.First(choice => choice.Parameters["played-kind"] == "Slash");
                vm.SelectCardCommand.Execute(materials[0]);
                vm.SelectEquipmentPlayChoiceCommand.Execute(staleChoice);
                Program.Assert(!vm.CanConfirmSelected && !vm.HasCardNameSelection && game.CardMovements.Count == moves,
                    "Changing the material draft retires its name choices without paying any cards.");
                vm.SelectCardCommand.Execute(materials[0]);
                if (ordinal == 2)
                {
                    Field("_wushuangClockCommittedMilliseconds").SetValue(vm, -5000L);
                    Method("OnWushuangClockTick").Invoke(vm, [null, EventArgs.Empty]);
                    Program.Assert(vm.Hand.Where(card => card.IsSelected).Select(card => card.Id).ToHashSet().SetEquals(materials.Select(card => card.Id)) &&
                        vm.HasCardNameSelection && game.CardMovements.Count == moves,
                        "A clock commit preserves the exact unpaid two-card selection and its basic-card chooser.");
                    Program.Render((FrameworkElement)window.Content, 1440, 880, Path.Combine(output, "wushuang-jinzhi-basic-choice.png"));
                }
                var choice = vm.CardNameSelectionChoices.Single(item => item.Parameters["played-kind"] == outputKind.ToString());
                vm.SelectEquipmentPlayChoiceCommand.Execute(choice);
                Program.Assert(vm.CanConfirmSelected && game.CardMovements.Count == moves &&
                    vm.Hand.Count(card => card.IsSelected) == ordinal,
                    "Choosing a basic card retains its materials, selects the sole legal target, and still makes no payment.");
                vm.CancelActionCommand.Execute(null);
                Program.Assert(!vm.IsActiveSkillSelectionPending && game.CardMovements.Count == moves &&
                    game.GetHumanActionView().JinzhiMaterials?.RequiredCount == ordinal,
                    "Cancel after choosing the card name spends neither cards nor the next activation ordinal.");
                vm.ActivateHumanSkillCommand.Execute(vm.HumanSkillCards.Single(skill => skill.ContentId == skillId));
                foreach (var material in materials) vm.SelectCardCommand.Execute(vm.Hand.Single(card => card.Id == material.Id));
                vm.SelectEquipmentPlayChoiceCommand.Execute(vm.CardNameSelectionChoices.Single(item => item.Parameters["played-kind"] == outputKind.ToString()));
                vm.ConfirmSelectedCommand.Execute(null);
                Drive(vm, () => Program.Engine(vm).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
                var paid = game.Events.Select(item => item.Payload).OfType<WushuangJinzhiPaidEvent>().ToArray();
                Program.Assert(paid.Length == ordinal && paid.Last().Ordinal == ordinal &&
                    paid.Last().Action.EffectiveKind == outputKind && paid.Last().Action.EffectiveRank is null &&
                    paid.Last().Action.EffectiveSuit == Suit.None && paid.Last().Action.EffectiveIsRed is null &&
                    paid.Last().Action.PhysicalCards.Select(card => card.CardId).ToHashSet().SetEquals(materials.Select(card => card.Id)),
                    "Only final confirmation pays the exact selected material set once and advances its round ordinal.");
                Program.Assert(game.Events.Select(item => item.Payload).OfType<CardUseDeclaredEvent>()
                    .Where(use => use.SourceSeat == 0).All(use => use.PublicCard is null),
                    "The generated basic card never displays the discarded material's printed suit or rank.");
                if (ordinal == 2)
                {
                    var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                    Program.Assert(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
                        restored.GetHumanActionView().JinzhiMaterials?.RequiredCount == 3,
                        "Full-roster cold replay preserves the colorless, rankless native use and its next material count.");
                }
            }
            var before = game.CardMovements.Count;
            Program.Assert(game.GetHumanActionView().JinzhiMaterials is { RequiredCount: 4 } available && available.CardIds.Count < 4,
                "The bounded fixture reaches an actual insufficient-material fourth activation.");
            vm.ActivateHumanSkillCommand.Execute(vm.HumanSkillCards.Single(skill => skill.ContentId == skillId));
            foreach (var card in vm.Hand.Where(card => card.IsPlayable).ToArray()) vm.SelectCardCommand.Execute(card);
            vm.ConfirmSelectedCommand.Execute(null);
            Program.Assert(vm.IsActiveSkillSelectionPending && !vm.CanConfirmSelected && vm.CanCancelAction &&
                vm.CardNameSelectionChoices.Count == 0 && game.CardMovements.Count == before,
                "Insufficient materials still allow entering and selecting, while confirmation remains disabled and cancellation remains available.");
            Program.Render((FrameworkElement)window.Content, 1440, 880, Path.Combine(output, "wushuang-jinzhi-insufficient.png"));
            vm.CancelActionCommand.Execute(null);
            Program.Assert(!vm.HasSelection && game.CardMovements.Count == before, "Cancelling the insufficient draft is a no-op for Core.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    private static FieldInfo Field(string name) => typeof(MainViewModel).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static bool HasAffordableSkillOffer(MainViewModel vm) =>
        Program.Engine(vm).CreateWushuangSnapshot(0) is { CanShop: true, Phase: WushuangShopPhase.Shopping } &&
        vm.WushuangOffers.Any(offer => offer.IsEnabled && !offer.IsPurchased && offer.SkillId is not null);
    private static MethodInfo Method(string name) => typeof(MainViewModel).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static void Layout(FrameworkElement root)
    { root.Measure(new Size(1440, 880)); root.Arrange(new Rect(0, 0, 1440, 880)); root.UpdateLayout(); Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); }
    private static void Drive(MainViewModel vm, Func<bool> done)
    {
        for (var step = 0; step < 180; step++)
        {
            if (done()) return;
            var game = Program.Engine(vm);
            GameCommand command;
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard } play)
                command = new EndPlayPhaseCommand(0, game.Revision, play.PromptId);
            else if (game.PendingDecision is { Kind: DecisionKind.DiscardCards } discard)
                command = new DiscardCardsCommand(0, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, game.Revision);
            else if (game.PendingDecision is { } prompt && prompt.Kind != DecisionKind.SelectGeneral)
            {
                var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "skip") ??
                    prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") == "pass") ?? prompt.Choices.First();
                command = new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision);
            }
            else command = new AdvanceOneStepCommand(game.Revision);
            var result = (CommandResult)Method("SubmitCommand").Invoke(vm, [command])!;
            Program.Assert(result.Accepted, result.Error?.Message ?? "The bounded duel UI fixture must reach its native boundary.");
        }
        throw new InvalidOperationException("The bounded duel UI fixture missed its native play boundary.");
    }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Program.Assert(result.Accepted, result.Error?.Message ?? "Native duel fixture rejected its command."); }

    private sealed class QuietFixture(string cardId = "standard:dodge", string humanSkillId = "classic:qianxun", bool jinzhiFixture = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("wushuang-ui-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var source = StandardWushuangContentRegistry.Create();
            builder.CopyModeFeatureFactoriesFrom(source);
            foreach (var skill in source.Skills.Values.Where(s =>
                s.Id.StartsWith("wushuang:", StringComparison.Ordinal) || s.Id.StartsWith("shanhe:", StringComparison.Ordinal) ||
                s.Id.StartsWith("hulao:", StringComparison.Ordinal) || s.Id.StartsWith("shenwu:", StringComparison.Ordinal) ||
                s.Id.StartsWith("ghost:", StringComparison.Ordinal))) builder.AddSkill(skill);
            builder.AddGeneral(new("fixture:wushuang-ui-human", "无双主将", "", humanSkillId, "qun", BaseHp: 20,
                AdditionalSkillIds: jinzhiFixture ? ["classic:paoxiao"] : []));
            builder.AddGeneral(new("fixture:wushuang-ui-ai", "无双对手", "", "classic:qianxun", "qun", BaseHp: 20));
            builder.AddDeck(new("fixture:wushuang-ui-deck", "Bounded native duel UI", 4, 0, [])
            { PhysicalCards = Enumerable.Range(0, 160).Select(index => new ContentDeckPhysicalCard(cardId,
                jinzhiFixture && index % 2 == 0 ? Suit.Diamond : Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(WushuangMode.ModeId, "无双界面验证", 2, 2, new Dictionary<string, int>(),
                "fixture:wushuang-ui-deck", 2, ["fixture:wushuang-ui-human", "fixture:wushuang-ui-ai"], ContentModeKind.Team,
                new Dictionary<string, int> { [WushuangMode.PlayerTeamId] = 1, [WushuangMode.AiTeamId] = 1 }));
        }
    }
}

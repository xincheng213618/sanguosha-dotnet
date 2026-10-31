using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class MultiCardDraftUiChecks
{
    public static void DenseHandStaysInOneRowAndPreservesNativeSelection(string output)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(timed: false, initialHand: 78));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0,
            HumanRole = Role.Lord, ModeId = Fixture.PlainMode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand()); ReachSetup(game);
        Accept(game, new SelectGeneralCommand(0, Fixture.Owner, game.Revision, game.PendingDecision!.PromptId));
        using var model = new MainViewModel(false, game.Seed, false, new MemorySaveStore(), contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, [game]);
        DriveToPlay(model);
        game = Program.Engine(model);
        Program.Assert(model.Hand.Count == 78, "The real native fixture must deal all 78 physical cards before exercising dense hand input.");
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        var items = (ItemsControl)host.Window.FindName("HandCards");
        var viewport = (HandScrollViewer)host.Window.FindName("HandViewport");
        host.Capture(output, "dense-hand-baseline", 1120);
        var panel = Program.Find<HandPanel>(items).Single();
        var press = typeof(HandPanel).GetMethod("PressCardAt", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var resolve = typeof(HandPanel).GetMethod("CardButtonAt", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var move = typeof(HandPanel).GetMethod("UpdatePointer", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var boundary = (Revision: game.Revision, Commands: game.AcceptedCommands.Count, Moves: game.CardMovements.Count);
        foreach (var width in new[] { 1120, 1920 })
        {
            host.Capture(output, "dense-hand-single-row", width);
            var containers = Containers();
            var origins = containers.Select(card => card.TranslatePoint(new Point(), panel)).ToArray();
            Program.Assert(viewport.ScrollableWidth <= .5 && viewport.HorizontalOffset == 0 &&
                containers.All(card => Math.Abs(card.TranslatePoint(new Point(), panel).Y - origins[0].Y) < .1) &&
                origins.Zip(origins.Skip(1), (left, right) => right.X - left.X).All(step => step > 0 && step < 36 * panel.CardScale) &&
                origins[^1].X + containers[^1].ActualWidth <= viewport.ViewportWidth + .5,
                "All 78 cards stay in order on one compressed row inside the actual viewport at each table scale.");
            for (var index = 0; index < model.Hand.Count; index++)
                Program.Assert(ReferenceEquals(((Button?)resolve.Invoke(panel, [PointAt(index)]))?.CommandParameter, model.Hand[index]),
                    "Every exposed strip must resolve its own physical entity even when the strip is only a few pixels wide.");
            Press(39); AssertRaised(39);
            host.Capture(output, "dense-hand-selected", width);
            Program.Assert(model.Hand[39].IsSelected && model.Hand.Count(card => card.IsSelected) == 1,
                "A genuine compressed-strip press selects exactly its own card.");
            model.ClearSelectionCommand.Execute(null);
            move.Invoke(panel, [new Point(-10, 80)]); host.Root.UpdateLayout();
            Program.Assert(model.Hand.All(card => !card.IsSelected) && containers.All(card => Panel.GetZIndex(card) == 0) &&
                Program.Find<CardLiftSurface>(containers[39]).Single().RenderTransform.Value.OffsetY == 0 &&
                containers.Select(card => card.TranslatePoint(new Point(), panel)).SequenceEqual(origins) &&
                (game.Revision, game.AcceptedCommands.Count, game.CardMovements.Count) == boundary,
                "Cancel restores the ordinary lift and stacking at the exact original slots without committing gameplay.");
        }
        host.Capture(output, "dense-hand-multi-selection", 1120);
        model.ActivateHumanSkillCommand.Execute(model.HumanSkillCards.Single(skill => skill.ContentId == "classic:zhiheng"));
        host.Root.UpdateLayout();
        Press(0); Press(1); Press(77); AssertRaised(77);
        host.Capture(output, "dense-hand-multi-selected", 1120);
        var selected = new[] { model.Hand[0].Id, model.Hand[1].Id, model.Hand[77].Id };
        Program.Assert(model.Hand.Where(card => card.IsSelected).Select(card => card.Id).ToHashSet().SetEquals(selected) &&
            (game.Revision, game.AcceptedCommands.Count, game.CardMovements.Count) == boundary,
            "Adjacent dense cards and the tail remain independently selectable in the original native multi-card draft.");
        model.CancelActionCommand.Execute(null);
        move.Invoke(panel, [new Point(-10, 80)]); host.Root.UpdateLayout();
        Program.Assert(!model.HasSelection && Containers().All(card => Panel.GetZIndex(card) == 0) &&
            (game.Revision, game.AcceptedCommands.Count, game.CardMovements.Count) == boundary,
            "Canceling the entire dense multi-card draft restores its unpaid cards and ordinary stacking.");
        model.ActivateHumanSkillCommand.Execute(model.HumanSkillCards.Single(skill => skill.ContentId == "classic:zhiheng"));
        host.Root.UpdateLayout();
        Press(0); Press(77);
        var payment = model.Hand.Where(card => card.IsSelected).Select(card => card.Id).ToHashSet();
        Click((Button)host.Window.FindName("PlayCardButton")); DriveToPlay(model);
        var paid = game.AcceptedCommands.Skip(boundary.Commands).OfType<UseProgramSkillCommand>().Single();
        Program.Assert(payment.Count == 2 && paid.SkillId == "classic:zhiheng" && paid.CardIds.ToHashSet().SetEquals(payment) &&
            payment.All(id => game.CardMovements.Skip(boundary.Moves).Count(movement => movement.CardId == id && movement.To == CardLocation.DiscardPile) == 1) &&
            !model.HasSelection,
            "Explicit confirmation submits the original native command and pays exactly the two selected physical cards once.");

        FrameworkElement[] Containers() => Enumerable.Range(0, items.Items.Count)
            .Select(index => (FrameworkElement)items.ItemContainerGenerator.ContainerFromIndex(index)).ToArray();
        Point PointAt(int index)
        {
            var containers = Containers();
            var strip = index + 1 < containers.Length ? containers[index + 1].TranslatePoint(new Point(), containers[index]).X : containers[index].ActualWidth;
            return containers[index].TranslatePoint(new Point(Math.Min(8, strip / 2), 80 * panel.CardScale), panel);
        }
        void Press(int index)
        {
            Program.Assert(press.Invoke(panel, [PointAt(index)]) is true, "The actual hand panel must handle the dense physical strip.");
            Program.Find<Button>(Containers()[index]).Single().RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                { RoutedEvent = UIElement.MouseLeftButtonUpEvent });
            host.Root.UpdateLayout();
        }
        void AssertRaised(int index)
        {
            var surface = Program.Find<CardLiftSurface>(Containers()[index]).Single();
            Program.Assert(surface.IsSelected && surface.RenderTransform.Value.OffsetY == -18,
                "The selected card keeps the shared full-size face and selected lift.");
            foreach (var local in new[] { new Point(3, 3), new Point(106, 3), new Point(3, 151), new Point(106, 151) })
            {
                var hit = VisualTreeHelper.HitTest(panel, surface.TranslatePoint(local, panel))?.VisualHit;
                while (hit is not null && hit is not Button) hit = VisualTreeHelper.GetParent(hit);
                Program.Assert(ReferenceEquals((hit as Button)?.CommandParameter, model.Hand[index]),
                    "The selected face must render in front across its complete artwork, beyond its compressed strip.");
            }
        }
    }

    public static void ActiveDiscardSurvivesReleaseAndClock(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(additionalPackages: [new Fixture()]);
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(31) with
            { UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        ReachSetup(game);
        Accept(game, new SelectGeneralCommand(0, Fixture.Owner, game.Revision, game.PendingDecision!.PromptId));
        var store = Store(game);
        using var model = new MainViewModel(false, 31, true, new MemorySaveStore(), contentRegistry: registry,
            wushuangSaveStore: store, shanheSaveStore: new MemoryShanheCampaignSaveStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model, wushuang: true);
        Program.Assert(!model.HasSaveError, model.SaveStatus);
        DriveToPlay(model);
        game = Program.Engine(model);
        model.SelectCardCommand.Execute(model.Hand.First(card => card.Kind == CardKind.Crossbow));
        model.ConfirmSelectedCommand.Execute(null);
        DriveToPlay(model);
        var equipmentId = model.HumanEquipmentSlots.Single(slot => slot.Card?.Kind == CardKind.Crossbow).Card!.Id;
        var cards = model.Hand.Take(3).Select(card => card.Id).ToArray();
        Program.Assert(cards.Length == 3, "The native Zhiheng scene needs three independent hand materials plus an equipped weapon.");
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Layout(host.Root);
        model.ActivateHumanSkillCommand.Execute(model.HumanSkillCards.Single(skill => skill.ContentId == "classic:zhiheng"));
        var prompt = game.PendingDecision!.PromptId;
        var moves = game.CardMovements.Count;
        var skillUses = game.AcceptedCommands.OfType<UseProgramSkillCommand>().Count();
        PressRelease(host, cards[0]); Tick([cards[0]], false);
        PressRelease(host, cards[1]); Tick([cards[0], cards[1]], false);
        model.SelectActiveEquipmentCommand.Execute(model.HumanEquipmentSlots.Single(slot => slot.Card?.Id == equipmentId));
        Tick([cards[0], cards[1]], true);
        PressRelease(host, cards[0]); Tick([cards[1]], true);
        PressRelease(host, cards[2]); Tick([cards[1], cards[2]], true);
        Program.Assert(!QuickConfirm(host.Window, model.Hand.Single(card => card.Id == cards[2])) &&
            game.CardMovements.Count == moves && game.AcceptedCommands.OfType<UseProgramSkillCommand>().Count() == skillUses,
            "Double-click confirmation must remain disabled for a complete active multi-card draft.");
        model.ActivateHumanSkillCommand.Execute(model.HumanSkillCards.Single(skill => skill.ContentId == Fixture.OwnedSkill));
        Tick([cards[1], cards[2]], true);
        Program.Assert(model.SelectionArea.ConfirmText == "发动制衡", "Clicking another skill must not transfer or pay Zhiheng's material draft.");
        model.CancelActionCommand.Execute(null);
        Program.Assert(!model.HasSelection && model.Hand.All(card => !card.IsSelected) &&
            model.HumanEquipmentSlots.All(slot => !slot.IsSelected) && game.CardMovements.Count == moves &&
            game.AcceptedCommands.OfType<UseProgramSkillCommand>().Count() == skillUses,
            "Cancel must clear the whole mixed draft without paying any selected entity.");
        model.ActivateHumanSkillCommand.Execute(model.HumanSkillCards.Single(skill => skill.ContentId == "classic:zhiheng"));
        PressRelease(host, cards[0]); PressRelease(host, cards[2]);
        model.SelectActiveEquipmentCommand.Execute(model.HumanEquipmentSlots.Single(slot => slot.Card?.Id == equipmentId));
        Tick([cards[0], cards[2]], true);
        Program.Render(host.Root, 1120, 740, Path.Combine(output, "active-zhiheng-persistent-multiselect.png"));
        Click((Button)host.Window.FindName("PlayCardButton"));
        var paid = game.AcceptedCommands.OfType<UseProgramSkillCommand>().Skip(skillUses).Single();
        var expected = new[] { cards[0], cards[2], equipmentId };
        Program.Assert(paid.SkillId == "classic:zhiheng" && paid.CardIds.ToHashSet().SetEquals(expected) &&
            expected.All(id => game.CardMovements.Skip(moves).Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1) &&
            !model.HasSelection, "One real confirmation must discard every exact selected hand/equipment entity once.");
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Program.Assert(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
            "The actual mixed Zhiheng payment and clock commands must restore through the ordinary journal.");

        void Tick(int[] selected, bool weapon)
        {
            var revision = game.Revision;
            Field("_wushuangClockCommittedMilliseconds").SetValue(model, -5000L);
            Method("OnWushuangClockTick").Invoke(model, [null, EventArgs.Empty]);
            Layout(host.Root);
            Program.Assert(game.Revision > revision && game.PendingDecision?.PromptId == prompt &&
                game.CardMovements.Count == moves && game.AcceptedCommands.OfType<UseProgramSkillCommand>().Count() == skillUses &&
                model.Hand.Where(card => card.IsSelected).Select(card => card.Id).ToHashSet().SetEquals(selected) &&
                model.HumanEquipmentSlots.Single(slot => slot.Card?.Id == equipmentId).IsSelected == weapon && model.CanConfirmSelected,
                "A committed native clock refresh must retain the unpaid active hand/equipment set, including its reversals.");
            AssertHandInk(host, selected);
        }
    }

    public static void MultiCardPromptsRequireExplicitConfirmation(string output)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(timed: false));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0,
            HumanRole = Role.Lord, ModeId = Fixture.PlainMode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand()); ReachSetup(game);
        Accept(game, new SelectGeneralCommand(0, Fixture.Owner, game.Revision, game.PendingDecision!.PromptId));
        for (var step = 0; game.PendingDecision?.Kind != DecisionKind.PlayCard && step < 40; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Program.Assert(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
            "The fixed four-player owned scene must reach its actual human Play boundary before opening the selector.");
        Accept(game, new UseProgramSkillCommand(0, Fixture.OwnedSkill, "select", [], [], game.Revision, game.PendingDecision!.PromptId));
        using (var model = Restore(game, registry))
        using (var host = new NativeResponseStatusChecks.NativeHost(model))
        {
            Layout(host.Root);
            Program.Assert(model.IsOwnedCardSelectionPending && !model.IsCardPaymentSelectionPending,
                "The owned regression must use the actual native two-card selector.");
            VerifyExplicit(host, model.Hand.Where(card => card.IsPlayable).Take(2).Select(card => card.Id).ToArray(), "owned");
        }
        var witness = StoneAxeScenario.FindHumanTrigger(seed: 1);
        using (var model = Restore(witness.Game, witness.Game.ContentRegistry))
        using (var host = new NativeResponseStatusChecks.NativeHost(model))
        {
            Layout(host.Root);
            Program.Assert(model.IsCardPaymentSelectionPending && !model.IsOwnedCardSelectionPending,
                "The payment regression must reach the real Stone Axe multi-card payment.");
            var handIds = model.Hand.Select(card => card.Id).ToHashSet();
            var choice = model.SkillChoices.First(item => item.Cards.Count == 2 && item.Cards.All(handIds.Contains));
            VerifyExplicit(host, choice.Cards.ToArray(), "payment");
        }

        void VerifyExplicit(NativeResponseStatusChecks.NativeHost host, int[] ids, string kind)
        {
            var model = host.Model;
            var game = Program.Engine(model);
            Program.Assert(ids.Length == 2, "A multi-card confirmation regression needs two real hand entities.");
            foreach (var id in ids) PressRelease(host, id);
            var revision = game.Revision;
            var commands = game.AcceptedCommands.Count;
            var moves = game.CardMovements.Count;
            var prompt = game.PendingDecision!.PromptId;
            Program.Assert(model.CanConfirmSelected, "The exact native multi-card set must first enable primary confirmation.");
            Program.Assert(!QuickConfirm(host.Window, model.Hand.Single(card => card.Id == ids[1])) &&
                game.Revision == revision && game.AcceptedCommands.Count == commands && game.CardMovements.Count == moves &&
                game.PendingDecision?.PromptId == prompt && model.Hand.Count(card => card.IsSelected) == 2,
                "The shared double-click handler must leave a complete multi-card prompt unpaid and editable.");
            PressRelease(host, ids[1]);
            Program.Assert(!model.CanConfirmSelected && model.Hand.Single(card => card.Id == ids[0]).IsSelected &&
                !model.Hand.Single(card => card.Id == ids[1]).IsSelected && game.Revision == revision,
                "A second ordinary press must reverse only that selected entity instead of paying the full set.");
            PressRelease(host, ids[1]);
            Program.Render(host.Root, 1120, 740, Path.Combine(output, $"multi-card-{kind}-explicit-confirmation.png"));
            Click((Button)host.Window.FindName("PlayCardButton"));
            Program.Assert(game.AcceptedCommands.Count > commands &&
                ids.All(id => game.CardMovements.Skip(moves).Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1),
                "Primary confirmation must still submit the original native answers and pay both exact cards once.");
        }
    }

    private static void PressRelease(NativeResponseStatusChecks.NativeHost host, int id)
    {
        Layout(host.Root);
        var card = host.Model.Hand.Single(item => item.Id == id);
        var items = (ItemsControl)host.Window.FindName("HandCards");
        var panel = Program.Find<HandPanel>(items).Single();
        var container = (FrameworkElement)items.ItemContainerGenerator.ContainerFromItem(card);
        var index = items.Items.IndexOf(card);
        var strip = index + 1 < items.Items.Count
            ? ((FrameworkElement)items.ItemContainerGenerator.ContainerFromIndex(index + 1)).TranslatePoint(new Point(), container).X
            : container.ActualWidth;
        var point = container.TranslatePoint(new Point(Math.Min(8, strip / 2), 80 * panel.CardScale), panel);
        Program.Assert(typeof(HandPanel).GetMethod("PressCardAt", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(panel, [point]) is true,
            "The real hand panel must accept its current physical card's press.");
        Program.Find<Button>(container).Single().RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            { RoutedEvent = UIElement.MouseLeftButtonUpEvent });
    }
    private static void AssertHandInk(NativeResponseStatusChecks.NativeHost host, int[] selected)
    {
        var items = (ItemsControl)host.Window.FindName("HandCards");
        foreach (var card in host.Model.Hand)
        {
            var container = (FrameworkElement)items.ItemContainerGenerator.ContainerFromItem(card);
            var surface = Program.Find<CardLiftSurface>(container).Single();
            var check = Program.Find<Border>(container).Single(border => border.Name == "SelectedCheck");
            var outline = Program.Find<Border>(container).Single(border => border.Name == "CardSelectedOutline");
            var expected = selected.Contains(card.Id);
            Program.Assert(surface.IsSelected == expected && check.Visibility == (expected ? Visibility.Visible : Visibility.Collapsed) &&
                outline.Opacity == (expected ? 1 : 0) && (!expected || surface.RenderTransform.Value.OffsetY == -18),
                "Released selections must retain their lift, check and outline after the clock refresh.");
        }
    }
    private static bool QuickConfirm(MainWindow window, object item) =>
        (bool)typeof(MainWindow).GetMethod("TryQuickConfirm", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(window, [item])!;
    private static MainViewModel Restore(GameEngine game, ContentRegistry registry)
    {
        var model = new MainViewModel(false, game.Seed, true, Store(game), contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model);
        Program.Assert(!model.HasSaveError, model.SaveStatus);
        return model;
    }
    private static MemorySaveStore Store(GameEngine game)
    {
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        return store;
    }
    private static void ReachSetup(GameEngine game)
    { for (var step = 0; game.PendingDecision?.Kind != DecisionKind.SelectGeneral && step < 40; step++) Accept(game, new AdvanceOneStepCommand(game.Revision)); }
    private static void DriveToPlay(MainViewModel model)
    {
        for (var step = 0; step < 100; step++)
        {
            var game = Program.Engine(model);
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            var prompt = game.CreateSnapshot(0).PendingDecision;
            GameCommand command = prompt is { } own
                ? new AnswerPromptCommand(0, own.PromptId, (own.Choices.FirstOrDefault(choice =>
                    choice.Parameters.GetValueOrDefault("program-action") == "skip") ?? own.Choices.First()).Id, game.Revision)
                : new AdvanceOneStepCommand(game.Revision);
            var result = (CommandResult)Method("SubmitCommand").Invoke(model, [command])!;
            Program.Assert(result.Accepted, result.Error?.Message ?? "The fixed native timed scene did not reach Play.");
        }
        throw new InvalidOperationException("The fixed timed multi-card scene missed its Play boundary.");
    }
    private static MethodInfo Method(string name) => typeof(MainViewModel).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!;
    private static FieldInfo Field(string name) => typeof(MainViewModel).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!;
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Program.Assert(result.Accepted, result.Error?.Message ?? "Native multi-card fixture rejected its command."); }
    private static void Click(Button button) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    private static void Layout(FrameworkElement root)
    { root.Width = 1120; root.Height = 740; root.Measure(new Size(1120, 740)); root.Arrange(new Rect(0, 0, 1120, 740)); root.UpdateLayout(); Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); }

    private sealed class Fixture(bool timed = true, string zhihengSkill = "classic:zhiheng", int initialHand = 4) : IGameContentPackage
    {
        internal const string Owner = "fixture:multi-card-draft-owner";
        internal const string OwnedSkill = "fixture:multi-card-draft-owned";
        internal const string PlainMode = "identity:multi-card-draft";
        public PackageManifest Manifest { get; } = new("fixture-multi-card-draft", new(1, 0, 0),
            [new("standard", StandardContentPackage.CurrentVersion)]);
        public void Register(IContentRegistryBuilder builder)
        {
            if (timed)
            {
                var source = StandardWushuangContentRegistry.Create();
                builder.CopyModeFeatureFactoriesFrom(source);
                foreach (var skill in source.Skills.Values.Where(skill =>
                    skill.Id.StartsWith("wushuang:", StringComparison.Ordinal) || skill.Id.StartsWith("shanhe:", StringComparison.Ordinal) ||
                    skill.Id.StartsWith("hulao:", StringComparison.Ordinal) || skill.Id.StartsWith("shenwu:", StringComparison.Ordinal) ||
                    skill.Id.StartsWith("ghost:", StringComparison.Ordinal))) builder.AddSkill(skill);
            }
            else CharacterFixtureContent.Register(builder, zhihengSkill == "boundary:zhiheng" ? "boundary:sun-quan" : "classic:sun-quan");
            var catalog = SkillProgramCatalog.Load($$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[{"id":"{{{{OwnedSkill}}}}","revision":1,
                 "activations":[{"id":"select","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                 "targetKind":"anyLiving","usesPerTurn":1,"effects":[
                  {"op":"selectOwnedCards","target":"owner","amount":2,"zones":["hand"],"resultBind":"chosen"},
                  {"op":"moveBoundCards","target":"owner","sourceBind":"chosen","destination":"discardPile"}]}]}]}
                """, $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{"{{{{OwnedSkill}}}}":{"name":"区域选择","description":"选择两张真实手牌后一次弃置。"}}}
                """);
            builder.AddSkill(new(OwnedSkill, "区域选择", "选择两张真实手牌后一次弃置。")
                { Program = catalog.Programs[OwnedSkill], ProgramPresentation = catalog.Presentations[OwnedSkill] });
            builder.AddGeneral(new(Owner, "制衡选择", "supporter", zhihengSkill, "wu", BaseHp: 20, AdditionalSkillIds: [OwnedSkill]));
            var peers = timed ? new[] { "fixture:multi-card-draft-ai" } : Enumerable.Range(1, 3)
                .Select(index => $"fixture:multi-card-draft-ai-{index}").ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "对手", "supporter", "standard:none", "wei", BaseHp: 20));
            builder.AddDeck(new("fixture:multi-card-draft-deck", "多选实际牌堆", initialHand, 0, [])
                { PhysicalCards = Enumerable.Range(0, Math.Max(160, initialHand * (peers.Length + 1) + 16))
                    .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, 7)).ToArray() });
            builder.AddMode(timed
                ? new(WushuangMode.ModeId, "多选计时场景", 2, 2, new Dictionary<string, int>(), "fixture:multi-card-draft-deck", 2,
                    [Owner, "fixture:multi-card-draft-ai"], ContentModeKind.Team,
                    new Dictionary<string, int> { [WushuangMode.PlayerTeamId] = 1, [WushuangMode.AiTeamId] = 1 })
                : new(PlainMode, "多选区域场景", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                    "fixture:multi-card-draft-deck", 4, [Owner, .. peers]));
        }
    }
}

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class RevealedSubsetUiChecks
{
    private const string Chengxiang = "boundary:chengxiang-current";

    public static void SharedPrivateCountAndPublicSuitDrafts(string output)
    {
        foreach (var character in new[] { "classic:li-dian", "classic:shen-lu-meng" })
        {
            var (source, registry) = Create(character);
            Reach(source, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(source, source.CreateSnapshot(0).PendingDecision!.Choices.First(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Reach(source, IsSubset);
            using var model = Restore(source, registry);
            using var host = new NativeResponseStatusChecks.NativeHost(model);
            var game = Program.Engine(model);
            var prompt = game.CreateSnapshot(0).PendingDecision!;
            var choice = prompt.Choices.First();
            var privateView = character == "classic:li-dian";
            Program.Assert(model.IsRevealedSubsetSelectionPending && !model.CanConfirmSelected && !model.CanCancelAction &&
                model.CenterSkillChoices.Count == 0 && (privateView ? model.PrivatelyViewedCards.Count == 4 : model.PublicRevealedCards.Count == 5),
                $"Xunxun and Shelie retain mandatory counts and source: {character}; subset={model.IsRevealedSubsetSelectionPending}, confirm={model.CanConfirmSelected}, cancel={model.CanCancelAction}, center={model.CenterSkillChoices.Count}, private={model.PrivatelyViewedCards.Count}, public={model.PublicRevealedCards.Count}; prompt={prompt.Prompt}.");
            if (privateView)
                Program.Assert(Enumerable.Range(1, 3).All(seat => game.CreateSnapshot(seat).PrivateRevealedCards?.Count is null or 0),
                    "Making private Xunxun faces interactive must not expose them to other players.");
            foreach (var id in choice.Cards) ClickCard(host, id);
            var faces = model.PublicRevealedCards.Concat(model.PrivatelyViewedCards).ToArray();
            Program.Assert(model.CanConfirmSelected && faces.Count(card => card.IsSelected) == choice.Cards.Count &&
                faces.Where(card => !card.IsSelected).All(card => !model.SelectRevealedCardCommand.CanExecute(card)),
                "The full exact-count or one-per-suit choice blocks every extension violating its combined constraints.");
            if (!privateView)
            {
                var pool = game.CreateSnapshot(0).PublicRevealedCards;
                var duplicatedSuit = pool.GroupBy(card => card.Suit).First(group => group.Count() > 1).Key;
                var picked = choice.Cards.First(id => pool.Single(card => card.Id == id).Suit != duplicatedSuit);
                ClickCard(host, picked);
                var duplicate = pool.First(card => card.Suit == duplicatedSuit && !choice.Cards.Contains(card.Id));
                Program.Assert(!model.SelectRevealedCardCommand.CanExecute(faces.Single(card => card.Id == duplicate.Id)),
                    "A duplicate suit remains blocked even while another suit is missing.");
                ClickCard(host, picked);
            }
            model.ClearSelectionCommand.Execute(null);
            Program.Assert(!model.HasSelection && !model.CanConfirmSelected && game.PendingDecision!.PromptId == prompt.PromptId,
                "Clearing a mandatory subset resets only the draft and leaves the native prompt open.");
            foreach (var id in choice.Cards.Reverse()) ClickCard(host, id);
            host.Capture(output, privateView ? "xunxun-private-direct-two" : "shelie-public-distinct-suits", 1120);
            Click((Button)host.Window.FindName("PlayCardButton"));
            Program.Assert(game.AcceptedCommands.OfType<AnswerPromptCommand>().Last().Choice == choice.Id,
                "Click order does not alter the exact published subset submitted at confirmation.");
            if (privateView)
            {
                Reach(game, p => p.DeckReorder is { BottomOnly: true }); Refresh(model);
                Program.Assert(model.IsDeckReorderPending && !model.HasDeckTopRow && model.DeckBottomCards.Count == 2 &&
                    !model.CanCancelAction && model.CanConfirmSelected && choice.Cards.All(id =>
                        game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == id)),
                    "Standard Xunxun obtains its two selected cards first, then orders only the remaining bottom cards.");
                model.MoveDeckOrderCardLeftCommand.Execute(model.DeckBottomCards.Last());
                var bottom = model.DeckBottomCards.Reverse().Select(card => card.Id).ToArray();
                var orderPrompt = game.CreateSnapshot(0).PendingDecision!;
                host.Capture(output, "standard-xunxun-remaining-bottom-order", 1120);
                Click((Button)host.Window.FindName("PlayCardButton"));
                Program.Assert(game.AcceptedCommands.OfType<AnswerPromptCommand>().Last().Choice ==
                    orderPrompt.Choices.Single(candidate => candidate.Cards.SequenceEqual(bottom)).Id,
                    "The bottom-only row submits one original permutation with the correct bottom-first order.");
                Reach(game, p => p.Kind == DecisionKind.PlayCard);
                var zones = typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
                var pile = (IEnumerable<Card>)zones.GetType().GetMethod("CardsAt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                    .Invoke(zones, [CardLocation.DrawPile])!;
                Program.Assert(pile.Take(2).Select(card => card.Id).SequenceEqual(bottom),
                    "Standard Xunxun places the actual unselected cards at the bottom in the displayed order.");
            }
        }
    }

    public static void TopAndBottomRowsExactAndFlexible(string output)
    {
        foreach (var character in new[] { "boundary:li-dian", "boundary:zhuge-liang" })
        {
            var (source, registry) = Create(character);
            Reach(source, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(source, source.CreateSnapshot(0).PendingDecision!.Choices.First(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Reach(source, p => p.Kind == DecisionKind.ProgramTopReorder);
            var exact = character == "boundary:li-dian";
            // Exercise a genuine saved accepted prefix, not only a fresh ordering prompt.
            if (exact) Answer(source, source.CreateSnapshot(0).PendingDecision!.Choices.First(c => c.Cards.Count == 1));
            using var model = Restore(source, registry);
            using var host = new NativeResponseStatusChecks.NativeHost(model);
            var game = Program.Engine(model);
            var view = game.CreateSnapshot(0).PendingDecision!.DeckReorder!;
            var commands = game.AcceptedCommands.Count;
            Program.Assert(model.IsDeckReorderPending && model.DeckTopCards.Count == view.Cards.Count &&
                model.DeckBottomCards.Count == 0 && !model.CanCancelAction && model.CenterSkillChoices.Count == 0 &&
                model.CanConfirmSelected == !exact && Enumerable.Range(1, 3).All(seat => game.CreateSnapshot(seat).PendingDecision is null),
                "Exact Xunxun and flexible Guanxing share two private rows, preserve mandatory placement and hide raw stage choices.");
            var locked = view.TopCardIds.ToHashSet();
            var movable = model.DeckTopCards.Where(card => !locked.Contains(card.Id)).ToArray();
            foreach (var card in movable.Take(exact ? 2 : movable.Length)) Transfer(card.Id);
            Program.Assert(model.CanConfirmSelected && model.DeckBottomCards.Count == (exact ? 2 : view.Cards.Count) &&
                locked.All(id => !model.TransferDeckOrderCardCommand.CanExecute(model.DeckTopCards.Single(card => card.Id == id))),
                "Exact top counts gate confirmation and accepted cards cannot be moved again after cold restore.");
            model.MoveDeckOrderCardLeftCommand.Execute(model.DeckBottomCards.Last());
            var moved = model.DeckBottomCards.First();
            Program.Assert(typeof(MainViewModel).GetMethod("MoveDeckOrderCard", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(model, [moved, true, model.DeckBottomCards.Count]) is true && model.DeckBottomCards.Last() == moved,
                "The drag destination path supports explicit in-row insertion before confirmation.");
            Program.Assert(game.AcceptedCommands.Count == commands, "Moving between rows and reordering remain local until confirmation.");
            Program.Render(host.Root, 1120, 820, null);
            var picker = (FrameworkElement)host.Window.FindName("DeckReorderSelection");
            var topRow = Program.Find<Border>(picker).Single(border => border.Name == "DeckTopRow");
            var bottomRow = Program.Find<Border>(picker).Single(border => border.Name == "DeckBottomRow");
            Program.Assert(topRow.IsVisible && bottomRow.IsVisible &&
                topRow.TranslatePoint(new Point(), picker).Y < bottomRow.TranslatePoint(new Point(), picker).Y,
                "The native view exposes the deck-bottom row below the deck-top row even when one row is empty.");
            host.Capture(output, exact ? "xunxun-top-bottom-rows" : "guanxing-all-bottom-row", 1120);
            var expectedTop = model.DeckTopCards.Select(card => card.Id).ToArray();
            var expectedBottom = model.DeckBottomCards.Reverse().Select(card => card.Id).ToArray();
            var acceptedBefore = game.AcceptedCommands.OfType<AnswerPromptCommand>().Count();
            Click((Button)host.Window.FindName("PlayCardButton"));
            var answers = game.AcceptedCommands.OfType<AnswerPromptCommand>().Skip(acceptedBefore).ToArray();
            var expected = expectedTop.Skip(view.TopCardIds.Count).Select(id => new ChoiceId($"program-top-order.{view.FrameId}.top.{id}"))
                .Concat(exact ? [] : [new ChoiceId($"program-top-order.{view.FrameId}.finish-top")])
                .Concat(expectedBottom.Select(id => new ChoiceId($"program-top-order.{view.FrameId}.bottom.{id}"))).ToArray();
            Program.Assert(answers.Select(answer => answer.Choice).SequenceEqual(expected) && !model.IsDeckReorderPending,
                "One confirm reuses exact current native choices, retaining top order and reversing the visual bottom row into bottom-first commands.");
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Program.Assert(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "The two-row arrangement restores through the ordinary accepted command journal.");

            void Transfer(int id)
            {
                Program.Render(host.Root, 1120, 820, null);
                var button = Program.Find<Button>(pickerRoot()).Single(button => button.Command == model.TransferDeckOrderCardCommand &&
                    button.CommandParameter is CardViewModel card && card.Id == id);
                Program.Assert(button.IsEnabled, "The uncommitted arrangement card must be movable."); Click(button);
            }
            FrameworkElement pickerRoot() => (FrameworkElement)host.Window.FindName("DeckReorderSelection");
        }
    }

    private static (GameEngine, ContentRegistry) Create(string character)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(character));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanRole = Role.Lord,
            UseInteractiveSetup = true, ModeId = Fixture.Mode, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, p => p.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, Fixture.Owner, game.Revision, game.PendingDecision!.PromptId));
        return (game, registry);
    }
    private static bool IsSubset(PendingDecision prompt) => prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-subset");
    private static void Reach(GameEngine game, Func<PendingDecision, bool> predicate)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt && predicate(prompt)) return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The fixed subset fixture missed its native prompt.");
    }
    private static void Answer(GameEngine game, PromptChoice choice) => Accept(game,
        new AnswerPromptCommand(0, game.PendingDecision!.PromptId, choice.Id, game.Revision));
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Program.Assert(result.Accepted, result.Error?.Message ?? "Subset fixture command rejected."); }
    private static MainViewModel Restore(GameEngine game, ContentRegistry registry)
    {
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        var model = new MainViewModel(false, 31, true, store, contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model);
        Program.Assert(!model.HasSaveError, model.SaveStatus);
        return model;
    }
    private static void Refresh(MainViewModel model) => typeof(MainViewModel)
        .GetMethod("RefreshCurrentView", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, null);
    private static Button CardButton(NativeResponseStatusChecks.NativeHost host, int id)
    {
        Program.Render(host.Root, 1120, 740, null);
        return Program.Find<Button>(host.Root).Single(button => button.IsVisible && button.Command == host.Model.SelectRevealedCardCommand &&
            button.CommandParameter is CardViewModel card && card.Id == id);
    }
    private static void ClickCard(NativeResponseStatusChecks.NativeHost host, int id)
    { var button = CardButton(host, id); Program.Assert(button.IsEnabled, "The current native card face must be selectable."); Click(button); }
    private static void Click(Button button) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);

    private sealed class Fixture(string character) : IGameContentPackage
    {
        internal const string Mode = "identity:revealed-subset-ui";
        internal const string Owner = "fixture:revealed-subset-owner";
        internal const string Driver = "fixture:revealed-subset-driver";
        public PackageManifest Manifest { get; } = new("fixture:revealed-subset-ui", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, character == "boundary:zhuge-liang" ? [character, "classic:zhuge-liang"] : [character]);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                "activations":[{"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                "effects":[{"op":"damage","target":"owner","amount":1}]}]}]}
                """, $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{"{{{{Driver}}}}":{"name":"伤害驱动","description":"真实伤害"}}}
                """);
            builder.AddSkill(new(Driver, "伤害驱动", "真实伤害") { Program = catalog.Programs[Driver] });
            var skill = character switch { "classic:li-dian" => "classic:xunxun", "classic:shen-lu-meng" => "classic:shelie",
                "boundary:li-dian" => "boundary:xunxun", "boundary:zhuge-liang" => "boundary:guanxing-current", _ => Chengxiang };
            builder.AddGeneral(new(Owner, "选牌场景", "supporter", skill, "wei", 20, [Driver]));
            var peers = Enumerable.Range(1, 3).Select(i => $"fixture:revealed-subset-ai-{i}").ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "对手", "supporter", "standard:none", "wu", 20));
            builder.AddDeck(new("fixture:revealed-subset-deck", "固定真实选牌", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 100).Select(i => new ContentDeckPhysicalCard("standard:slash", (Suit)(i % 4), 4)).ToArray() });
            builder.AddMode(new(Mode, "选择牌子集", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 },
                "fixture:revealed-subset-deck", 4, [Owner, .. peers]));
        }
    }
}

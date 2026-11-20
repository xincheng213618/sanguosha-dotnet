using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OwnDiscardZongxuanChecks
{
    private const string Driver = "fixture:own-zongxuan-discard", Earlier = "fixture:own-zongxuan-use";
    private const string Discard = "skill-program.fixture:own-zongxuan-discard.SelectAndMoveOwnedCard";
    private const string EarlierDraw = "skill-program.fixture:own-zongxuan-use.Draw";
    private const string Mode = "identity:classic-own-zongxuan-entry", Deck = "fixture:own-zongxuan-deck";

    internal static void ClassicZongxuanOnlyPlacesItsOriginalDiscardEntryCold() =>
        OnlyPlacesItsOriginalDiscardEntryCold("classic:yu-fan", "classic:zongxuan");

    internal static void MobileBoundaryZongxuanOnlyPlacesItsOriginalDiscardEntryCold() =>
        OnlyPlacesItsOriginalDiscardEntryCold("boundary:yu-fan-mobile", "boundary:zongxuan-mobile");

    private static void OnlyPlacesItsOriginalDiscardEntryCold(string hero, string skill)
    {
        var (game, content) = Start(hero);
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Driver });
        var card = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier } &&
            prompt.Choices.Any(choice => Action(choice) == "activate"));

        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Discard);
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId);
        var placementReason = $"skill-program.{skill}.PutDiscardedCardsOnDrawPileTop";
        Require(original.From == CardLocation.Hand(1) && original.To == CardLocation.DiscardPile &&
            window.Candidates.Skip(window.CandidateIndex + 1).Any(candidate => candidate.OwnerSeat == 1 && candidate.SkillId == skill),
            $"{skill} follows the earlier trigger on the same genuine owned discard batch.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1 } && prompt.SkillPrompt?.SkillId == skill);
        Activate(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1 } && prompt.SkillPrompt?.SkillId == skill &&
            prompt.Choices.Any(choice => Action(choice) == "discard-top-select" && choice.Cards.SequenceEqual([card])));
        Answer(control, choice => Action(choice) == "discard-top-select" && choice.Cards.SequenceEqual([card]));
        Answer(control, choice => Action(choice) == "discard-top-finish");
        Settle(control);
        var placed = control.CardMovements.Single(move => move.Reason.Value == placementReason);
        Require(placed.CardId == card && placed.From == CardLocation.DiscardPile && placed.To == CardLocation.DrawPile &&
            placed.Sequence > original.Sequence && control.CreateCardZoneDiagnostics().Where(item => item.Location == CardLocation.DrawPile)
                .MaxBy(item => item.ZoneIndex)!.CardId == card,
            "Zongxuan can actually place the available original discard at the top of the draw pile.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier } && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([2])));
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Earlier } &&
            prompt.Choices.Any(choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([card])));
        var reshuffled = game.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.Reshuffle);
        var drawn = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == EarlierDraw);
        Require(reshuffled.From == CardLocation.DiscardPile && reshuffled.To == CardLocation.DrawPile && reshuffled.Sequence > original.Sequence &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(2) && drawn.Sequence > reshuffled.Sequence,
            "The earlier movement child really reshuffles the only discarded entity and draws it to the requested Slash user.");

        game = Cold(game, content);
        Answer(game, choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([card]));
        FinishSlash(game, skill);
        var declared = game.Events.Select(entry => entry.Payload).OfType<CardUseDeclaredEvent>().Single(fact => fact.CardId == card);
        var cleanup = game.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        Require(declared is { SourceSeat: 2, CardKind: CardKind.Slash } &&
            game.Events.Select(entry => entry.Payload).OfType<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == declared.ResolutionId) &&
            cleanup.From == CardLocation.Processing && cleanup.To == CardLocation.DiscardPile &&
            cleanup.BatchId != original.BatchId && cleanup.Sequence > drawn.Sequence,
            "Native Slash cleanup returns the same entity at a different entry after the original discard was removed.");

        if (P(game)?.SkillPrompt?.SkillId == skill)
        {
            Require(game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last().Batch.Id == original.BatchId,
                "Any remaining Zongxuan invocation belongs to the original genuine discard rather than the native use cleanup.");
            Activate(game);
            Reach(game, prompt => prompt.SkillPrompt?.SkillId == skill || prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            if (P(game)?.SkillPrompt?.SkillId == skill)
            {
                Answer(game, choice => Action(choice) == "discard-top-select" && choice.Cards.SequenceEqual([card]));
                Answer(game, choice => Action(choice) == "discard-top-finish");
            }
        }
        Settle(game);
        Require(game.CardMovements.All(move => move.Reason.Value != placementReason) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"{skill} may not place the newer use cleanup entry {cleanup.Sequence} via its own old discard entry {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static void FinishSlash(GameEngine game, string skill)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || prompt.SkillPrompt?.SkillId == skill)
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} while settling the native Slash.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not finish at a public decision boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the frozen batch and the pending action in all four seat views.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start(string hero)
    {
        string[] generals = ["fixture:own-zongxuan-driver", hero, "fixture:own-zongxuan-user", "fixture:own-zongxuan-peer"];
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(generals));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }

        Require(!game.CreateCardZoneDiagnostics().Any(item => item.Location == CardLocation.DrawPile ||
            item.Location == CardLocation.DiscardPile), "The opening hands exhaust the fixed deck before the first real discard.");
        return (game, content);
    }

    private sealed class Fixture(string[] generals) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-own-zongxuan-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new OwnerBuilder(builder, generals[1]), generals[1]);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                        "cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Earlier}}}","revision":1,"triggers":[{"id":"draw-and-use","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["{{{Discard}}}"],"priority":100,"optional":true,
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                        {"op":"draw","target":"selectedTarget","amount":1},
                        {"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"slash"}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "弃牌", description = "令指定角色弃置一张手牌。" },
                    [Earlier] = new { name = "先摸再用", description = "选择另一名角色摸一张牌，再令其对你使用杀。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });

            builder.AddGeneral(new(generals[0], "弃牌发起者", "supporter", Driver, "qun", 8));
            foreach (var id in generals.Skip(2))
                builder.AddGeneral(new(id, "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "八张实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 8)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "自身纵玄原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: generals));
        }
    }

    private sealed class OwnerBuilder(IContentRegistryBuilder inner, string hero) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == hero ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Earlier] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }
}

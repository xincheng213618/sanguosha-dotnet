using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ProgramStepReceiptChecks
{
    private const string LogicalGive = "fixture:logical-give-consumer", PhysicalGive = "fixture:physical-give-consumer",
        LogicalGiveDriver = "fixture:logical-give-driver", LogicalGiveOwner = "fixture:logical-give-owner";

    private static void DeclaredNativeGiveUsesLogicalOccurrencesAndOriginalMovementWindowsCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new LogicalGiveFixture());
        Need(!registry.Skills.ContainsKey(Tuicheng) && !registry.Skills.ContainsKey(Jiyuan),
            "The shared logical Give fixture loads ordinary custom declarations without either legacy character consumer.");
        var setup = new ShanheBattleSetup(7, 1, "logical-give", LogicalGiveOwner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Spade, 8), new(CardKind.Dodge, Suit.Club, 9)], [],
            [new(GivePeer, Hp: 8) { StartingHand = [] }, new(GivePeer, Hp: 8) { StartingHand = [] }],
            [new(GivePeer, Hp: 8) { StartingHand = [] }], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(game, new StartGameCommand()); Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var original = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        Accept(game, new UseProgramSkillCommand(0, LogicalGiveDriver, "give-two", [], [1], game.Revision, P(game)!.PromptId));
        foreach (var cardId in original.Take(2))
        {
            Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
            Need(P(game)!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards")
                    .All(choice => choice.Cards.Count == 1),
                "The exact two-card owned selection is an incremental one-entity prompt, not a combined card-set answer.");
            Answer(game, choice => choice.Cards.SequenceEqual([cardId]));
        }
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == LogicalGive);
        var grouped = E<NativeCardsGivenEvent>(game).Single();
        var context = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == LogicalGive).WindowContext!;
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == context.ParentFrameId);
        Need(grouped is { Giver: 0, Recipient: 1, Count: 2 } && grouped.Receipt is { Movements.Count: 2 } &&
            grouped.Receipt.Movements.Select(move => move.CardId).SequenceEqual(original.Take(2)) &&
            context.NativeGive is { } captured && captured.Id == grouped.Id && captured.Movements.Select(move => (move.Sequence, move.CardId, move.From, move.To, move.Reason))
                .SequenceEqual(grouped.Receipt.Movements.Select(move => (move.Sequence, move.CardId, move.From, move.To, move.Reason))) &&
            context.SourceSeat == 0 && context.TargetSeat == 1 &&
            context.Amount == 2 && context.Facts?.MovedCardCount == 2 && context.OccurrenceIndex == 0 && context.MovementIndex == 1 &&
            window.Batch.Movements[1].Sequence == grouped.CompletionSequence && window.Batch.ParentFrameId == grouped.ProducerId &&
            window.Candidates.Count(candidate => candidate.SkillId == LogicalGive) == 1 &&
            window.Candidates.Count(candidate => candidate.SkillId == PhysicalGive) == 2 &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == grouped.ProducerId),
            "Two actual entities produce one logical Give candidate with delivered count two, its own logical ordinal and the original batch's physical completion index.");
        game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        var indexes = new List<int>();
        for (var index = 0; index < 2; index++)
        {
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == PhysicalGive);
            var physical = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == PhysicalGive).WindowContext!;
            Need(physical.ParentFrameId == window.Id && physical.NativeGive is null && physical.Facts?.MovedCardCount == 2 && physical.MovementIndex is not null,
                "Physical per-card triggers retain their original batch count and individual movement index rather than a logical Give context.");
            indexes.Add(physical.MovementIndex!.Value);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(indexes.SequenceEqual([0, 1]) && Completed(game, LogicalGive) == 1 && Completed(game, PhysicalGive) == 2,
            "Command cold recovery completes one logical Give and both physical card observations exactly once.");
        Accept(game, new UseProgramSkillCommand(0, LogicalGiveDriver, "deck-give", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == LogicalGive);
        var deck = E<NativeCardsGivenEvent>(game).Last();
        var deckContext = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == LogicalGive).WindowContext!;
        Need(deck is { Giver: 0, Recipient: 1, Count: 1 } && deck.Receipt is { Movements: [var fromDeck] } &&
            fromDeck.From == CardLocation.DrawPile && fromDeck.To == CardLocation.Hand(1) &&
            deckContext.NativeGive?.Id == deck.Id && deckContext.SourceSeat == 0 && deckContext.TargetSeat == 1 &&
            deckContext.Amount == 1 && deckContext.Facts?.MovedCardCount == 1 && deckContext.OccurrenceIndex == 0 &&
            deckContext.MovementBatch!.Movements[deckContext.MovementIndex!.Value].Sequence == deck.CompletionSequence &&
            Completed(game, PhysicalGive) == 2,
            "A native deck Give preserves its logical giver despite having no physical source owner, and does not invent a Hand-loss occurrence.");
        game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Accept(game, new UseProgramSkillCommand(0, LogicalGiveDriver, "plain-move", [], [2], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        Answer(game, choice => choice.Cards.SequenceEqual([original[2]]));
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == PhysicalGive);
        Need(E<NativeCardsGivenEvent>(game).Count() == 2 && Completed(game, LogicalGive) == 2,
            "An ordinary move still produces its physical observer but cannot synthesize logical Give intent.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(Completed(game, LogicalGive) == 2 && Completed(game, PhysicalGive) == 3 &&
            game.CardMovements.Any(move => move.CardId == original[2] && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(2)),
            "Both independent occurrence models keep their exact counts through the final true ordinary movement.");
        _ = NativeGiveCold(game, registry);
    }

    private static int Completed(GameEngine game, string skill) => E<ProgramBindingResolvedEvent>(game)
        .Count(fact => fact.SkillId == skill && fact.Activated && fact.Completed);

    private sealed class LogicalGiveFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-logical-give", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{LogicalGiveDriver}}","revision":1,"activations":[
                    {"id":"give-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":2,"maximumCards":2,"resultBind":"gift"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"gift","destination":"selectedTargetHand","giveBy":"owner"},{"op":"awaitBoundCardMovements","target":"owner"}]},
                    {"id":"deck-give","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"giveDrawPileBottomCard","target":"selectedTarget"}]},
                    {"id":"plain-move","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":1,"maximumCards":1,"resultBind":"moved"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"moved","destination":"selectedTargetHand"},{"op":"awaitBoundCardMovements","target":"owner"}]}]},
                  {"id":"{{LogicalGive}}","revision":1,"triggers":[{"id":"logical","window":"cardsMoved","subject":"owner","movementOccurrence":"perNativeGive","optional":false,"priority":100,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"logical","options":[{"id":"continue"}]}]}]},
                  {"id":"{{PhysicalGive}}","revision":1,"triggers":[{"id":"physical","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perCard","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"physical","options":[{"id":"continue"}]}]}]},
                  {"id":"{{GiveQuiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills =
                    new Dictionary<string, object>
                    {
                        [LogicalGiveDriver] = new { name = "Shared Give driver", description = "Physical and logical sources" },
                        [LogicalGive] = new { name = "Logical Give", description = "Once per native Give", optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" } },
                        [PhysicalGive] = new { name = "Physical movement", description = "Once per actual Hand card", optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" } },
                        [GiveQuiet] = new { name = "Quiet", description = "Fixed opening cards" }
                    } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(LogicalGiveOwner, "Shared giver", "", LogicalGiveDriver, "wu", 4, [LogicalGive, PhysicalGive, GiveQuiet]));
            builder.AddGeneral(new(GivePeer, "Fixed peer", "", GiveQuiet, "qun", 8));
            const string deck = "fixture:logical-give-deck";
            builder.AddDeck(new(deck, "Fixed source deck", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 9)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Logical Give occurrences", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [LogicalGiveOwner, GivePeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

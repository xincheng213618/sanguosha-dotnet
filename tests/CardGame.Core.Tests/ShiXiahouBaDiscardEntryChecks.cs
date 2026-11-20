using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShiXiahouBaDiscardEntryChecks
{
    private const string Hero = "mobile:shi-xiahou-ba", Poxi = "mobile:shi-poxi";
    private const string Driver = "fixture:poxi-discard-driver", Claim = "fixture:poxi-earlier-claim";
    private const string Mode = "identity:poxi-discard-entry", Deck = "fixture:poxi-deck";
    private const string Drop = "skill-program.fixture:poxi-discard-driver.SelectAndMoveOwnedCard";
    private const string Take = "skill-program.fixture:poxi-earlier-claim.ClaimMovedCards";
    private const string Deliver = "skill-program.fixture:poxi-earlier-claim.SelectAndMoveOwnedCard";
    private const string PoxiUse = "skill-program.mobile-shi-xiahou-ba.use";
    private static readonly string[] Generals = ["fixture:poxi-claimant", Hero, "fixture:poxi-recipient", "fixture:poxi-peer"];

    internal static void PoxiCannotUseTheSameSlashFromALaterNativeCleanupCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Driver });
        var slash = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([slash]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => Action(choice) == "activate"));
        var original = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Drop);
        var batch = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId);
        Require(original.From == CardLocation.Hand(1) && original.To == CardLocation.DiscardPile &&
            batch.Candidates.Skip(batch.CandidateIndex + 1).Any(candidate => candidate.OwnerSeat == 1 && candidate.SkillId == Poxi),
            "The actual discarded Slash queues Poxi on the same batch behind the earlier claimant.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Poxi });
        var selected = control.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Poxi).ShiXiahouBa;
        Require(selected is { CardId: var material, DiscardMovementSequence: var entry } && material == slash && entry == original.Sequence,
            "Poxi selects the exact original entry when it is still available.");
        Answer(control, choice => choice.Cards.SequenceEqual([slash]) && choice.Targets.SequenceEqual([3]));
        Require(FinishSlash(control) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The real Poxi use returns to the original Play phase.");
        var issued = Facts<ShiXbPoxiNativeIssuedEvent>(control).Single();
        var paid = control.CardMovements.Single(move => move.Reason.Value == PoxiUse);
        Require(issued.Source.OwnerSeat == 1 && issued.CardId == slash && issued.TargetSeat == 3 &&
            paid.CardId == slash && paid.From == CardLocation.DiscardPile && paid.To == CardLocation.Processing &&
            Facts<ShiXbPoxiNativeReturnedEvent>(control).Single().UseId == issued.UseId &&
            Facts<CardUseFinishedEvent>(control).Any(fact => fact.ResolutionId == issued.UseId && fact.CardId == slash),
            "The original entry pays and completes one real Poxi Slash.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([2])));
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([slash])));
        Answer(game, choice => choice.Cards.SequenceEqual([slash]));
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => Action(choice) == "request-slash"));
        var took = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Take);
        var delivered = game.CardMovements.Last(move => move.CardId == slash);
        Require(took.From == CardLocation.DiscardPile && took.To == CardLocation.Hand(0) && took.Sequence > original.Sequence &&
            delivered.Reason.Value == Deliver && delivered.To == CardLocation.Hand(2) && delivered.Sequence > took.Sequence,
            "The earlier claimant obtains and forwards that physical Slash before the old Poxi candidate runs.");

        game = Cold(game, content);
        Answer(game, choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([slash]));
        var next = FinishSlash(game);
        var declared = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == slash);
        var cleanup = game.CardMovements.Single(move => move.CardId == slash && move.Reason == CardMoveReasons.UseFinished);
        Require(declared is { SourceSeat: 2, CardKind: CardKind.Slash } && cleanup.From == CardLocation.Processing &&
            cleanup.To == CardLocation.DiscardPile && cleanup.BatchId != original.BatchId && cleanup.Sequence > delivered.Sequence &&
            Facts<CardUseFinishedEvent>(game).Any(fact => fact.ResolutionId == declared.ResolutionId && fact.CardId == slash),
            "A normal native Slash produces a separate, newer cleanup entry for the same card.");
        Require(next is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && !Facts<ShiXbPoxiNativeIssuedEvent>(game).Any() &&
            game.CardMovements.All(move => move.Reason.Value != PoxiUse) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == slash).Location == CardLocation.DiscardPile,
            $"Poxi may not use the cleanup entry {cleanup.Sequence} through the older discard entry {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision FinishSlash(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || prompt.SkillPrompt?.SkillId == Poxi)
                return prompt;
            Require(prompt.Kind == DecisionKind.RespondDodge, "Only the native Slash defence may interrupt this discard window.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not finish at a public decision boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the pending decision for each seat and the exact physical entries.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-xiahou-ba-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                        "cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-poxi","window":"discardPileReceived","subject":"owner",
                    "discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{Drop}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},
                        {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                          "zones":["hand"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true},
                        {"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"slash"}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "弃牌", description = "令指定角色弃置一张手牌。" },
                    [Claim] = new { name = "先行领取", description = "领取弃牌，交给另一名角色，然后令其使用杀。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "先行领取者", "supporter", Driver, "qun", 8, [Claim]));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "迫袭原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

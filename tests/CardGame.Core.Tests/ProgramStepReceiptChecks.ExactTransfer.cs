using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class ProgramStepReceiptChecks
{
    private const string ExactTransfer = "fixture:exact-transfer", ExactWatch = "fixture:exact-transfer-watch",
        ExactDemand = "fixture:exact-transfer-demand", ExactOwner = "fixture:exact-transfer-owner",
        ExactPeer = "fixture:exact-transfer-peer", ExactIssuer = "fixture:exact-transfer-issuer";
    private const string ExactTransferReason = "skill-program." + ExactTransfer + ".SelectAndMoveOwnedCard";
    private const string ExactMode = "fixture:exact-transfer-identity";

    private static void ExactForeignTransferSelectsBeforeOneRequestAndFallsBackAtomicallyCold()
    {
        foreach (var scenario in new[] { "full", "short", "retained" })
        {
            var retained = scenario == "retained";
            var eligibleCount = scenario == "short" ? 1 : 2;
            var openingCount = retained ? 3 : eligibleCount;
            var registry = ContentRegistry.Build(new StandardContentPackage(), new ExactTransferFixture(openingCount, retained));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
                HumanRole = Role.Lord, ModeId = ExactMode, HumanSeats = [0, 1],
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            Accept(game, new StartGameCommand()); ReachExactOpeningPlay(game, retained);
            Need(game.CreateSnapshot(0).Players[0].HandCount == openingCount && game.CreateSnapshot(1).Players[1].HandCount == openingCount,
                "The legal multi-human identity fixture deals fixed two, one or three original entities before any transfer.");
            if (retained) game = IssueExactTransferRetainedReplacement(game, registry);
            var blocked = game.CreateSnapshot(0).Players[0].Hand.First().Id;
            var eligible = game.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).ToArray();
            Accept(game, new UseProgramSkillCommand(0, ExactTransfer, "transfer", [blocked], [1], game.Revision, P(game)!.PromptId));
            if (eligibleCount == 2)
            {
                Reach(game, () => P(game) is { PlayerSeat: 1, SkillPrompt.SkillId: ExactTransfer });
                var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == ExactTransfer);
                var frameId = frame.Id;
                Need(P(game)!.IsPrivate && game.CreateSnapshot(1).PendingDecision?.RequiredCardCount == 2 &&
                    P(game)!.Choices.SelectMany(choice => choice.Cards).ToHashSet().SetEquals(eligible) &&
                    !P(game)!.Choices.SelectMany(choice => choice.Cards).Contains(blocked) &&
                    game.CreateSnapshot(0).PendingDecision is null && game.CreateSnapshot(2).PendingDecision is null &&
                    eligible.All(id => !game.CreateSnapshot(0).Players[1].Hand.Any(card => card.Id == id)) &&
                    !game.CardMovements.Any(move => move.Reason.Value == ExactTransferReason),
                    "The foreign holder alone chooses from its own legal HE entities, excluding the known binding without exposing its other Hand identities or moving a cost early.");
                var before = game.CardMovements.Count;
                Answer(game, choice => choice.Cards.SequenceEqual([eligible[0]]));
                var partial = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frameId);
                Need(P(game) is { PlayerSeat: 1, SkillPrompt.SkillId: ExactTransfer } &&
                    game.CreateSnapshot(1).PendingDecision?.RequiredCardCount == 1 &&
                    P(game)!.Choices.Where(choice => choice.Cards.Count > 0).SelectMany(choice => choice.Cards).Distinct().Count() == 1 &&
                    game.CardMovements.Count == before && partial.SelectedCardPayment is null &&
                    partial.OwnedCardSelection is { CardOwnerSeat: 1, RequiredCount: 2, SelectedCardIds.Count: 1 } &&
                    !partial.CardSetBindings.Any(binding => binding.Name == "accepted"),
                    "The first real selection stays on the original frame and pays no partial movement or accepted result before the exact set is complete.");
                var draft = JsonSerializer.Serialize<ResolutionFrame>(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frameId));
                game = NativeGiveCold(game, registry);
                Need(JsonSerializer.Serialize<ResolutionFrame>(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frameId)) == draft &&
                    game.CardMovements.Count == before && game.CreateSnapshot(0).PendingDecision is null,
                    "The true command prefix reconstructs the typed partial foreign draft and its private authority without moving its first selected card.");
                if (retained)
                {
                    Answer(game, choice => choice.Cards.SequenceEqual([eligible[1]]));
                    Reach(game, () => P(game)?.SkillPrompt?.SkillId == ExactTransfer &&
                        P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "after"));
                    var settled = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frameId);
                    Need(settled.SelectedCardPayment is { MovementCommitted: true, GainRequestSettlement: { MovementBatchId: null, Movements.Count: 0, RetainedGains.Count: 2 } request } &&
                        request.ProducerFrameId == frameId && request.RetainedGains.Select(receipt => receipt.Plan.CardId).ToHashSet().SetEquals(eligible) &&
                        request.RetainedGains.All(receipt => receipt.Plan.From == CardLocation.Hand(1) &&
                            receipt.Plan.RequestedTo == CardLocation.Hand(0) && receipt.Plan.ActualTo == CardLocation.Hand(1)) &&
                        settled.HappyWeiYanRetainedGains?.Count == 2 && game.CardMovements.Count == before &&
                        !E<NativeCardsGivenEvent>(game).Any(fact => fact.Giver == 1 && fact.Recipient == 0) &&
                        eligible.All(id => !game.CreateSnapshot(0).Players[1].Hand.Any(card => card.Id == id)),
                        "A real issued Gujing replacement settles both accepted equipment requests in their original hand with typed retained receipts, no movement batch, no actual Give and no private-material leak.");
                }
                else
                {
                    Answer(game, choice => choice.Cards.SequenceEqual([eligible[1]]));
                    Reach(game, () => P(game)?.SkillPrompt?.SkillId == ExactWatch);
                    var given = E<NativeCardsGivenEvent>(game).Single(fact => fact.Giver == 1 && fact.Recipient == 0);
                    var batch = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(window =>
                        window.Batch.Movements.Any(move => move.Sequence == given.CompletionSequence)).Batch;
                    Need(given.Count == 2 && given.ProducerId == frameId && given.Receipt is { Movements.Count: 2 } &&
                        given.Receipt.Movements.Select(move => move.CardId).ToHashSet().SetEquals(eligible) &&
                        batch.Movements.Count(move => move.To == CardLocation.Hand(0) && eligible.Contains(move.CardId)) == 2 &&
                        batch.ParentFrameId == frameId &&
                        game.CardMovements.Count(move => move.Reason.Value == ExactTransferReason && move.To == CardLocation.Hand(0)) == 2,
                        "Completing the exact foreign set issues one native request and one logical Give containing both actual deliveries in its original physical batch.");
                    game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "delivered");
                }
            }
            else
            {
                Reach(game, () => P(game)?.SkillPrompt?.SkillId == ExactTransfer &&
                    P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "after"));
                Need(!game.CardMovements.Any(move => move.Reason.Value == ExactTransferReason) &&
                    !E<NativeCardsGivenEvent>(game).Any(fact => fact.Giver == 1) &&
                    game.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).ToHashSet().SetEquals(eligible.Append(blocked)),
                    "One legal card plus an excluded card is insufficient for two: no partial foreign selection or movement is paid and the holder keeps every entity.");
            }
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == ExactTransfer &&
                P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "after"));
            var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == ExactTransfer);
            var accepted = parent.CardSetBindings.Single(binding => binding.Name == "accepted");
            Need(accepted.CardIds.Count == (eligibleCount == 2 ? 2 : 0) &&
                (eligibleCount != 2 || accepted.CardIds.ToHashSet().SetEquals(eligible)) &&
                !game.CardMovements.Any(move => move.Reason.Value == "skill-program." + ExactTransfer + ".Draw"),
                "The original instruction returns an accepted selected set or a real empty binding before the recipe decides whether to run its fallback.");
            var settledFrame = retained ? JsonSerializer.Serialize<ResolutionFrame>(parent) : null;
            game = NativeGiveCold(game, registry);
            if (retained)
                Need(JsonSerializer.Serialize<ResolutionFrame>(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == parent.Id)) == settledFrame,
                    "A real command checkpoint reconstructs both typed retained settlements and the accepted nonempty binding before the fallback decision.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "after");
            Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Need(E<ProgramSkillResolvedEvent>(game).Count(fact => fact.FrameId == parent.Id && fact.Completed) == 1 &&
                E<ProgramDrawCompletedEvent>(game).Count(fact => fact.FrameId == parent.Id && fact.RequestedCount == 2 && fact.ActualDrawCount == 2) == (eligibleCount == 2 ? 0 : 1) &&
                E<NativeCardsGivenEvent>(game).Count(fact => fact.Giver == 1 && fact.Recipient == 0) == (eligibleCount == 2 && !retained ? 1 : 0),
                "The exact set never also draws; a shortfall draws the original requested two once after cold recovery, without a reduced payment or fabricated Give.");
            _ = NativeGiveCold(game, registry);
        }
    }

    private static void ReachExactOpeningPlay(GameEngine game, bool retained)
    {
        for (var step = 0; step < 240; step++)
        {
            var prompt = P(game);
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
            if (prompt is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 or 1 })
            {
                var general = prompt.PlayerSeat == 0 ? ExactOwner : retained ? ExactIssuer : ExactPeer;
                Need(prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("general-id", choice.Id.Value) == general),
                    "The fixed identity catalogue must offer the intended local human general.");
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, general, game.Revision, prompt.PromptId));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The fixed multi-human exact-transfer fixture did not reach its opening Play.");
    }

    private static GameEngine IssueExactTransferRetainedReplacement(GameEngine game, ContentRegistry registry)
    {
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Accept(game, new UseProgramSkillCommand(1, "happy:gujing", "bone-path", [], [0], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == "happy:gujing" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option")?.StartsWith("card:", StringComparison.Ordinal) == true));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option")?.StartsWith("card:", StringComparison.Ordinal) == true);
        Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "slot:Weapon") == true);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "slot:Weapon");
        Reach(game, () => P(game)?.Choices.Any(choice => choice.Targets.SequenceEqual([0]) &&
            choice.Parameters.GetValueOrDefault("option")?.StartsWith("abolish:", StringComparison.Ordinal) == true) == true);
        Answer(game, choice => choice.Targets.SequenceEqual([0]) && choice.Parameters.GetValueOrDefault("option")?.StartsWith("abolish:", StringComparison.Ordinal) == true);
        Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "finish") == true);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "finish");
        Reach(game, () => P(game) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Need(E<HappyWeiYanGujingIssuedEvent>(game).Single().Source.OwnerSeat == 1 &&
            E<HappyWeiYanGujingAbolishedEvent>(game).Single() is { Holder: 0, Slot: EquipmentSlot.Weapon } &&
            game.CreateSnapshot(1).Players[1].Hand is { Count: 2 } cards && cards.All(card => card.Kind == CardKind.Crossbow),
            "Real Gujing commands spend one original weapon to issue the replacement and abolish the receiving slot, leaving two actual issuer-hand weapons for the exact request.");
        game = NativeGiveCold(game, registry);
        Accept(game, new EndPlayPhaseCommand(1, game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        return game;
    }

    private sealed class ExactTransferFixture(int openingCount, bool retained) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-exact-transfer", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            if (retained)
            {
                var replacement = CharacterFixtureContent.EmbeddedProgramBundle("happy-shen-wei-yan");
                builder.AddSkill(new("happy:gujing", replacement.Presentations["happy:gujing"].Name, replacement.Presentations["happy:gujing"].Description)
                    { Program = replacement.Programs["happy:gujing"], ProgramPresentation = replacement.Presentations["happy:gujing"] });
            }
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{ExactTransfer}}}","revision":1,"activations":[{"id":"transfer","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                    {"op":"captureSelectedCards","target":"owner","resultBind":"blocked"},
                    {"op":"captureCards","target":"owner","resultBind":"forbidden","cardsExpr":["boundCards","blocked"]},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"blocked","destination":"selectedTargetHand","giveBy":"owner"},{"op":"awaitBoundCardMovements","target":"owner"},
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand","equipment"],
                      "countExpr":2,"exceptBind":"forbidden","onInsufficient":"bindEmpty","destination":"ownerHand","giveBy":"materialOwner","resultBind":"accepted","awaitMovementTriggers":true},
                    {"op":"chooseOption","target":"owner","resultBind":"after","options":[{"id":"after"}]},
                    {"op":"draw","target":"owner","amount":2,"condition":{"kind":"expression","expr":["==",["boundCount","accepted"],0]},"awaitMovementTriggers":true}]}]},
                  {"id":"{{{ExactWatch}}}","revision":1,"triggers":[
                    {"id":"delivered","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{{ExactTransferReason}}}"],"movementOccurrence":"perBatch","optional":false,
                      "effects":[{"op":"chooseOption","target":"owner","resultBind":"delivered","options":[{"id":"delivered"}]}]},
                    {"id":"native","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
                  {"id":"{{{ExactDemand}}}","revision":1,"triggers":[{"id":"fact-demand","window":"cardsMoved","subject":"owner","movementOccurrence":"perNativeGive","optional":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
                  {"id":"{{{GiveQuiet}}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                {
                    [ExactTransfer] = new { name = "Exact foreign transfer", description = "One accepted request or atomic empty result", optionLabels = new Dictionary<string, string> { ["after"] = "After selection" } },
                    [ExactWatch] = new { name = "Actual delivery", description = "One physical child batch", optionLabels = new Dictionary<string, string> { ["delivered"] = "Delivered" } },
                    [ExactDemand] = new { name = "Shared fact demand", description = "Capture actual native Give facts" },
                    [GiveQuiet] = new { name = "Quiet", description = "Fixed private hands" }
                } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(ExactOwner, "Original program owner", "", ExactTransfer, "wu", 4, [ExactWatch, GiveQuiet]));
            builder.AddGeneral(new(ExactPeer, "Private foreign holder", "", GiveQuiet, "qun", 8));
            if (retained) builder.AddGeneral(new(ExactIssuer, "Real replacement issuer", "", GiveQuiet, "qun", 8, ["happy:gujing"]));
            var peers = new[] { ExactPeer + "-2", ExactPeer + "-3" };
            foreach (var peer in peers) builder.AddGeneral(new(peer, "Fixed identity peer", "", GiveQuiet, "wei", 8));
            const string deck = "fixture:exact-transfer-deck";
            builder.AddDeck(new(deck, "Fixed exact-transfer entities", openingCount, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(index => new ContentDeckPhysicalCard(retained ? "standard:crossbow" : "standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ExactMode, "Exact foreign transfer", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [ExactOwner, retained ? ExactIssuer : ExactPeer, .. peers]));
        }
    }
}

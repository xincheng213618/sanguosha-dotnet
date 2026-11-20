using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShanheSourceChunlao3161DiscardEntryChecks
{
    private const string Hero = StandardShanheSourceJiangdongTrioPackage.ChengPuHardId;
    private const string Skill = StandardShanheSourceJiangdongTrioPackage.ChunlaoSkillId;
    private const string Driver = "fixture:source-chunlao-discard-driver", Claim = "fixture:source-chunlao-earlier-claim";
    private const string Mode = "identity:source-chunlao-3161-discard-entry", Deck = "fixture:source-chunlao-3161-deck";
    private const string Drop = "skill-program." + Driver + ".SelectAndMoveOwnedCard";
    private const string Take = "skill-program." + Claim + ".ClaimMovedCards";
    private const string Deliver = "skill-program." + Claim + ".SelectAndMoveOwnedCard";
    private const string Store = "shanhe.source-chunlao-3161.store";
    private static readonly string[] Generals = ["fixture:source-chunlao-claimant", Hero, "fixture:source-chunlao-peer", "fixture:source-chunlao-mounted-discarder"];

    internal static void SourceChunlao3161OffersOnlyTheDiscardersOriginalDistanceOneEntryCold()
    {
        var (game, content) = Start();
        Require(game.GetSeatDistance(3, 1) == 2 && game.GetCombatDistance(3, 1) == 1 && game.GetCombatDistance(1, 3) == 2,
            "The original discarder sits across the table and Mashu makes only the source-to-holder combat distance one.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [], [3], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 3, SkillPrompt.SkillId: Driver });
        var slash = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([slash]));
        ReachClaim(game, 0, choice => Action(choice) == "activate");

        var original = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Drop);
        var origin = Facts<ProgramAdjacentDiscardOriginEvent>(game).Single(fact => fact.MovementSequence == original.Sequence);
        var batch = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId);
        Require(original.From == CardLocation.Hand(3) && original.To == CardLocation.DiscardPile && origin.CardId == slash &&
            origin is { SourceSeat: 3, PreviousLivingSeat: 2, NextLivingSeat: 0, EffectiveKind: CardKind.Slash } &&
            batch.Candidates.Skip(batch.CandidateIndex + 1).Any(candidate => candidate.SkillId == Skill && candidate.OwnerSeat == 1),
            "The real original Slash queues variant 3161 for a combat-distance-one holder outside the frozen adjacent seats.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => prompt.SkillPrompt?.SkillId == Skill);
        var choice = P(control)!;
        var storedBy = control.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
        Require(choice is { PlayerSeat: 3, IsPrivate: false } && choice.ValidCardIds.SequenceEqual([slash]) &&
            storedBy is { OwnerSeat: 1, ShanheSourceChunlaoStorage: { SourceSeat: 3, Paid: false } receipt } &&
            receipt.CardId == slash && receipt.DiscardSequence == original.Sequence,
            "The original discarder owns the public choice for the original entity, while the skill holder owns the pile.");
        var store = choice.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault("branch") == "store");
        var wrongSeat = control.Submit(new AnswerPromptCommand(1, choice.PromptId, store.Id, control.Revision));
        Require(!wrongSeat.Accepted && P(control)?.PromptId == choice.PromptId && !Facts<ShanheSourceChunlaoStoredEvent>(control).Any(),
            "The skill holder cannot decide to store the discarding player's Slash.");
        var accepted = Cold(control, content);
        AnswerStorage(control, "decline");
        Settle(control);
        Require(!Facts<ShanheSourceChunlaoStoredEvent>(control).Any() &&
            control.CreateCardZoneDiagnostics().Single(card => card.CardId == slash).Location == CardLocation.DiscardPile,
            "The actual discarder can decline and leave the original Slash untouched.");
        AnswerStorage(accepted, "store");
        Settle(accepted);
        var stored = Facts<ShanheSourceChunlaoStoredEvent>(accepted).Single();
        var deposit = accepted.CardMovements.Single(move => move.Reason.Value == Store);
        Require(stored is { OwnerSeat: 1, SourceSeat: 3 } && stored.CardId == slash && stored.DiscardSequence == original.Sequence &&
            stored.StoreSequence == deposit.Sequence && deposit.CardId == slash && deposit.From == CardLocation.DiscardPile &&
            deposit.To == new CardLocation(CardZoneKind.Chunlao, 1),
            "The original discarder can store the untouched entry exactly once in the other seat's actual Chunlao pile.");

        Activate(game);
        ReachClaim(game, 0, candidate => candidate.Targets.SequenceEqual([3]));
        Answer(game, candidate => candidate.Targets.SequenceEqual([3]));
        ReachClaim(game, 0, candidate => candidate.Cards.SequenceEqual([slash]));
        Answer(game, candidate => candidate.Cards.SequenceEqual([slash]));
        ReachClaim(game, 3, candidate => Action(candidate) == "request-slash" && candidate.Cards.SequenceEqual([slash]));
        var taken = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Take);
        var transfer = game.CardMovements.Where(move => move.CardId == slash && move.Reason.Value == Deliver).ToArray();
        Require(taken.From == CardLocation.DiscardPile && taken.To == CardLocation.Hand(0) && taken.Sequence > original.Sequence &&
            transfer is [var departed, var delivered] && departed.From == taken.To && departed.To == CardLocation.Processing &&
            delivered.From == departed.To && delivered.To == CardLocation.Hand(3) && departed.Sequence > taken.Sequence &&
            delivered.Sequence > departed.Sequence,
            "A prior claimant takes and returns the physical Slash to the original discarder during the old window.");

        game = Cold(game, content);
        Answer(game, candidate => Action(candidate) == "request-slash" && candidate.Cards.SequenceEqual([slash]));
        var next = FinishSlash(game);
        var offered = next.SkillPrompt?.SkillId == Skill;
        if (offered)
        {
            Require(next.PlayerSeat == 3, "The stale storage offer still points to the old discarder.");
            AnswerStorage(game, "store");
            next = FinishSlash(game);
        }
        var declared = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == slash);
        var cleanup = game.CardMovements.Single(move => move.CardId == slash && move.Reason == CardMoveReasons.UseFinished);
        Require(declared is { SourceSeat: 3, CardKind: CardKind.Slash } && cleanup.From == CardLocation.Processing &&
            cleanup.To == CardLocation.DiscardPile && cleanup.BatchId != original.BatchId && cleanup.Sequence > transfer[^1].Sequence &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == declared.ResolutionId && fact.CardId == slash) == 1 &&
            Facts<ProgramAdjacentDiscardOriginEvent>(game).Where(fact => fact.CardId == slash).SequenceEqual([origin]),
            "The real native Slash's finish movement has its own entry and no new actual discard provenance.");
        Require(next is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && !offered && !Facts<ShanheSourceChunlaoStoredEvent>(game).Any() &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == slash).Location == CardLocation.DiscardPile,
            $"Source 3161 cannot offer or store native Slash cleanup entry {cleanup.Sequence} through original discard entry {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void AnswerStorage(GameEngine game, string branch) => Answer(game, choice =>
        Action(choice) == "shanhe-source-chunlao-store" && choice.Parameters.GetValueOrDefault("branch") == branch);

    private static void ReachClaim(GameEngine game, int seat, Func<PromptChoice, bool> predicate) =>
        Reach(game, prompt => prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == Claim && prompt.Choices.Any(predicate));

    private static PendingDecision FinishSlash(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || prompt.SkillPrompt?.SkillId == Skill)
                return prompt;
            Require(prompt.Kind == DecisionKind.RespondDodge, "Only a native Slash defence may interrupt this discard window.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not reach the original discard window's next decision.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves all seat views, the discarder-owned choice, and exact physical movements.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
            new StandardClassicGeneralPackage(), new StandardShanheSourceJiangdongTrioPackage(), new Fixture());
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
        public PackageManifest Manifest { get; } = new("fixture-source-chunlao-3161-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                        "cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-source-chunlao","window":"discardPileReceived","subject":"owner",
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
            builder.AddGeneral(new(Generals[2], "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddGeneral(new(Generals[3], "持有马术的弃牌者", "supporter", "classic:mashu", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "山河3161醇醪原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

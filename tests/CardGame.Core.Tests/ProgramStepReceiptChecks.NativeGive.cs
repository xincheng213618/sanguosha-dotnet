using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ProgramStepReceiptChecks
{
    private const string GiveDriver = "fixture:step-native-give", GiveOwner = "fixture:step-native-giver",
        GiveWatch = "fixture:step-native-give-watch", GivePeer = "fixture:step-native-give-peer",
        GiveQuiet = "fixture:step-native-give-quiet", Tuicheng = "mou:tuicheng", Jiyuan = "boundary:jiyuan-mobile";

    private static void NativeGiveFeedsTuichengAndJiyuanFromOneOriginalBatchCold()
    {
        foreach (var withJiyuan in new[] { false, true }) NativeGiveConsumersCold(withJiyuan);
        DeclaredNativeGiveUsesLogicalOccurrencesAndOriginalMovementWindowsCold();
    }

    private static void NativeGiveConsumersCold(bool withJiyuan)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new NativeGiveFixture(withJiyuan));
        var setup = new ShanheBattleSetup(7, 1, "shared-native-give", GiveOwner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Spade, 8)], [],
            [new(GivePeer, Hp: 8) { StartingHand = [] }, new(GivePeer, Hp: 8) { StartingHand = [] }],
            [new(GivePeer, Hp: 8) { StartingHand = [] }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(game, new StartGameCommand()); Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var originals = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        Accept(game, new UseProgramSkillCommand(0, GiveDriver, "give", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        Answer(game, choice => choice.Cards.SequenceEqual([originals[0]]));
        Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "give-bound-card") == true);
        var producerId = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == GiveDriver).Id;
        game = NativeGiveCold(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "give-bound-card" && choice.Targets.SequenceEqual([1]));
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == (withJiyuan ? Jiyuan : GiveWatch));
        var give = E<NativeCardsGivenEvent>(game).Single();
        Need(give is { ProducerId: var producer, Giver: 0, Recipient: 1, Count: 1 } && producer == producerId &&
            give.Receipt is { Movements: [var delivered] } receipt && delivered.CardId == originals[0] &&
            delivered.From == CardLocation.Hand(0) && delivered.To == CardLocation.Hand(1) &&
            receipt.CompletionSequence == delivered.Sequence &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window =>
                window.Batch.Movements.Any(move => move.Sequence == receipt.CompletionSequence) &&
                window.Candidates.Any(candidate => candidate.SkillId == Tuicheng) &&
                window.Candidates.Any(candidate => candidate.SkillId == Jiyuan) == withJiyuan),
            "The real Give publishes its original entity and batch occurrence for every actual Tuicheng and Jiyuan consumer.");
        if (withJiyuan)
        {
            Need(P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate") &&
                !E<NativeCardsGivenEvent>(game).Any(fact => fact.Giver == 1 && fact.Recipient == 0) &&
                !E<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == Tuicheng),
                "At equal priority the real boundary Jiyuan candidate precedes Mou Tuicheng; neither consumer is silently skipped.");
            game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == Jiyuan &&
                P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target"));
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target" && choice.Targets.SequenceEqual([1]));
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == Tuicheng);
            var reward = game.CardMovements.Single(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                move.Reason.Value == "skill-program." + Jiyuan + ".Draw");
            var replies = P(game)!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card")
                .SelectMany(choice => choice.Cards).ToArray();
            Need(P(game) is { PlayerSeat: 1 } && replies.SequenceEqual([reward.CardId]) && !replies.Contains(originals[0]) &&
                game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == originals[0]) &&
                E<ProgramDrawCompletedEvent>(game).Count(fact => fact.RecipientSeat == 1 && fact.RequestedCount == 1 && fact.ActualDrawCount == 1) == 1,
                "Tuicheng cannot return the original gift even while it is held: after Jiyuan's actual Draw, only that new entity is eligible for the equal-count reply.");
            var replyFrameId = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Tuicheng).Id;
            game = NativeGiveCold(game, registry);
            Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            var returned = E<NativeCardsGivenEvent>(game).Single(fact => fact.Giver == 1 && fact.Recipient == 0);
            Need(returned.ProducerId == replyFrameId && returned.Count == 1 && returned.Id != give.Id &&
                returned.Receipt is { Movements: [var reply] } && reply.CardId == reward.CardId &&
                reply.From == CardLocation.Hand(1) && reply.To == CardLocation.Hand(0) &&
                returned.CompletionSequence == reply.Sequence &&
                game.CardMovements.Count(move => move.CardId == reward.CardId && move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0)) == 1 &&
                game.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).SequenceEqual([originals[0]]) &&
                E<ProgramExpressionNumberCapturedEvent>(game).Single(fact => fact.FrameId == replyFrameId && fact.Name == "count").Value == 1 &&
                !E<ProgramDrawCompletedEvent>(game).Any(fact => fact.FrameId == replyFrameId),
                "The reply is a distinct actual Give from recipient to original giver, using only the Jiyuan draw; a paid reply does not also award Tuicheng's fallback Draw.");
        }
        else
        {
            var fallbackFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Tuicheng);
            Need(!E<NativeCardsGivenEvent>(game).Any(fact => fact.Giver == 1 && fact.Recipient == 0) &&
                fallbackFrame.CardSetBindings.Single(binding => binding.Name == "reply").CardIds.Count == 0 &&
                E<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == fallbackFrame.Id) is { RequestedCount: 1, ActualDrawCount: 1, RecipientSeat: 0 } &&
                E<NativeDrawAttemptStartedEvent>(game).Count(fact => fact.Context.Reason.Value == "skill-program.mou:tuicheng.Draw") == 1 &&
                !E<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == Jiyuan),
                "An otherwise empty recipient cannot return the very card just given this turn, so Tuicheng issues one real equal-count Draw.");
            game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var tuichengFrameId = E<ProgramBindingStartedEvent>(game).Single(fact => fact.SkillId == Tuicheng).FrameId;
        Need(E<NativeCardsGivenEvent>(game).Count(fact => fact.Giver == 0 && fact.Recipient == 1) == 1 &&
            E<NativeCardsGivenEvent>(game).Count() == (withJiyuan ? 2 : 1) &&
            E<ProgramExpressionNumberCapturedEvent>(game).Count(fact => fact.FrameId == tuichengFrameId && fact.Name == "count" && fact.Value == 1) == 1 &&
            E<ProgramDrawCompletedEvent>(game).Count(fact => fact.FrameId == tuichengFrameId && fact.RequestedCount == 1 && fact.ActualDrawCount == 1) == (withJiyuan ? 0 : 1) &&
            E<ProgramBindingResolvedEvent>(game).Count(fact => fact.SkillId == Tuicheng && fact.Activated && fact.Completed) == 1 &&
            E<ProgramBindingResolvedEvent>(game).Count(fact => fact.SkillId == Jiyuan && fact.Activated && fact.Completed) == (withJiyuan ? 1 : 0) &&
            game.CardMovements.Count(move => move.CardId == originals[0] && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(1)) == 1,
            "Every actual consumer returns once from the original native Give; its fallback or recipient reward remains separate from the one physical original gift.");
        var draws = E<NativeDrawAttemptStartedEvent>(game).Count();
        Accept(game, new UseProgramSkillCommand(0, GiveDriver, "move", [originals[1]], [2], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(game.CardMovements.Any(move => move.CardId == originals[1] && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(2)) &&
            E<NativeCardsGivenEvent>(game).Count() == (withJiyuan ? 2 : 1) &&
            E<ProgramBindingStartedEvent>(game).Count(fact => fact.SkillId == Tuicheng) == 1 &&
            E<NativeDrawAttemptStartedEvent>(game).Count() == draws,
            "An ordinary real Hand-to-Hand movement is not a declared Give and cannot trigger either Give consumer or fabricate another reward.");
        _ = NativeGiveCold(game, registry);
    }

    private static GameEngine NativeGiveCold(GameEngine game, ContentRegistry registry)
    {
        static string State(GameEngine match) => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(match.CreateSnapshot(seat))),
            match.CardMovements,
            Facts = E<NativeCardsGivenEvent>(match).Select(fact => fact.Receipt),
            Events = match.Events.Select(envelope => JsonSerializer.Serialize(envelope.Payload, envelope.Payload.GetType()))
        });
        var restored = Cold(game, registry);
        Need(State(game) == State(restored), "Actual command cold replay reconstructs the same shared Give facts, private choices, physical movements and public scalar events.");
        return restored;
    }

    private sealed class NativeGiveFixture(bool withJiyuan) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-give-consumers", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var (resource, id) in new[] { ("mou-huang-quan", Tuicheng), ("boundary-yi-ji", Jiyuan) })
            {
                var bundle = CharacterFixtureContent.EmbeddedProgramBundle(resource);
                builder.AddSkill(new(id, bundle.Presentations[id].Name, bundle.Presentations[id].Description)
                    { Program = bundle.Programs[id], ProgramPresentation = bundle.Presentations[id] });
            }
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{GiveDriver}}","revision":1,"activations":[
                    {"id":"give","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":1,"maximumCards":1,"resultBind":"gift"},{"op":"giveBoundCard","target":"owner","sourceBind":"gift","targetKind":"otherLiving","awaitMovementTriggers":true}]},
                    {"id":"move","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"captureSelectedCards","target":"owner","resultBind":"moved"},{"op":"moveBoundCards","target":"owner","sourceBind":"moved","destination":"selectedTargetHand"}]}]},
                  {"id":"{{GiveWatch}}","revision":1,"triggers":[
                    {"id":"tuicheng-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mou:tuicheng.Draw"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-draw","options":[{"id":"continue"}]}]},
                    {"id":"native-request","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
                  {"id":"{{GiveQuiet}}","revision":1,"triggers":[{"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [GiveDriver] = new { name = "Native Give driver", description = "Declared Give and ordinary move stay separate" },
                        [GiveWatch] = new { name = "Native Draw child", description = "Real paid child", optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" } },
                        [GiveQuiet] = new { name = "Quiet", description = "Fixed opening cards" }
                    } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(GiveOwner, "Native giver", "", GiveDriver, "wu", 4,
                withJiyuan ? [Tuicheng, Jiyuan, GiveWatch, GiveQuiet] : [Tuicheng, GiveWatch, GiveQuiet]));
            builder.AddGeneral(new(GivePeer, "Fixed peer", "", GiveQuiet, "qun", 8));
            const string deck = "fixture:native-give-deck";
            builder.AddDeck(new(deck, "Fixed Dodge deck", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 9)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Native Give consumers", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [GiveOwner, GivePeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

// The shared Record unit uses an unstarted composition fixture: production
// output correctly drops facts before Start, so this narrow port targets only
// its existing journal. The real consumer fixture above keeps native output.
internal sealed class NativeGiveJournalEvents(ProgramEventHistoryIndex journal) : IProgramRuntimeEvents
{
    public void AdvanceFact(IGameEvent fact) => journal.Append(fact);
}

using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ProgramStepReceiptChecks
{
    private const string MovementSkill = "fixture:accepted-movement", MovementDriver = "fixture:movement-driver",
        MovementBefore = "fixture:movement-before", MovementGate = "fixture:movement-native-gate",
        MovementOwner = "fixture:movement-owner";
    private const string MovementDrawReason = "skill-program." + MovementSkill + ".Draw";

    private static void AcceptedMovementStaysWithItsOriginalCandidate()
    {
        var movement = new CardMovementRecord(1, 0, 11, CardKind.Dodge, CardLocation.Hand(0), CardLocation.Hand(1), new("fixture.movement"));
        var batch = new CardMovementBatchContext(500, 400, null, 0, [movement], []);
        var candidate = new ProgramTriggerCandidate(0, MovementSkill, "accepted", "original-instance", "original-hash", 0);
        var future = new ProgramTriggerCandidate(1, "fixture:future-gain", "future", "future-instance", "future-hash", 0);
        var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.CardsMoved, 500, 0,
            SourceSeat: 0, TargetSeat: 1, Amount: 1, MovementBatch: batch, MovementIndex: 0);
        var futureContext = new ProgramSkillWindowContext(SkillProgramTriggerWindow.CardsGained, 500, 1,
            SourceSeat: 0, TargetSeat: 1, Amount: 1, MovementBatch: batch, MovementIndex: 0);
        var early = new ProgramInvocationAuthorization(500, 400, SkillProgramTriggerWindow.CardsGained, future, RetainOwnerDeath: true);
        var parent = new CardsMovedTriggerWindowFrame(500, batch, [candidate, candidate, future],
            Contexts: [context, context, futureContext]) { InvocationAuthorizations = [early] };
        var ticket = ProgramInvocations.AcceptMovement(parent, candidate, context, 900, retainOwnerDeath: false);
        var accepted = ProgramInvocations.AttachAcceptedMovement(parent, ticket);
        var child = new ProgramSkillFrame(900, 0, MovementSkill, "accepted", "original-hash", 0, [], [])
        {
            TriggerId = "accepted", SkillInstanceId = "original-instance", WindowContext = context, InvocationAuthorization = ticket
        };
        Need(ProgramInvocations.Allows(child, accepted) && !ticket.RetainOwnerDeath &&
            ProgramInvocations.Find(accepted, candidate, context) is null &&
            accepted.InvocationAuthorizations!.Contains(early) && accepted.InvocationAuthorizations!.Count == 2,
            "Beginning one movement child appends its exact late ticket without granting pre-acceptance permission or replacing another binding's early authorization.");
        Need(!ProgramInvocations.Allows(child, accepted with { CandidateIndex = 1 }) &&
            !ProgramInvocations.Allows(child with { Id = 901, InvocationAuthorization = ticket with { ProgramFrameId = 901 } }, accepted) &&
            !ProgramInvocations.Allows(child, accepted with { Batch = batch with { ParentFrameId = 401 } }) &&
            !ProgramInvocations.Allows(child, accepted with { Contexts = [context with { TargetSeat = 2 }, context, futureContext] }),
            "Identical candidate scalars at another cursor, a copied child, another native batch parent or a replaced frozen context cannot borrow the accepted movement ticket.");
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        frames.Push(accepted);
        game.GetRuntimeModule<ProgramWindowCursorRuntime>().AdvanceCardsMovedProgramCursor(accepted);
        var advanced = (CardsMovedTriggerWindowFrame)frames.Top!;
        Need(advanced.CandidateIndex == 1 && advanced.InvocationAuthorizations!.SequenceEqual([early]) &&
            ProgramInvocations.Find(advanced with { CandidateIndex = 2 }, future, futureContext) == early,
            "Advancing the real movement cursor retires only its current late child ticket and preserves the future early gain/equipment authorization.");
        frames.Pop(advanced.Id, ResolutionFrameKind.CardsMovedTriggerWindow);
    }

    private static void AcceptedMovementContinuesOnlyItsOriginalOrdinaryProgramCold()
    {
        foreach (var scenario in new[] { "source-loss", "before-acceptance", "owner-death" })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new MovementInvocationFixture(scenario));
            var setup = new ShanheBattleSetup(7, 1, "accepted-movement", MovementOwner, 4, 0, 0, 0, 0, [], [], [],
                [new(GivePeer, Hp: 8) { StartingHand = [] }, new(GivePeer, Hp: 8) { StartingHand = [] }],
                [new(GivePeer, Hp: 8) { StartingHand = [] }], new string('d', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            Accept(game, new StartGameCommand()); Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Accept(game, new UseProgramSkillCommand(0, MovementDriver, "draw", [], [], game.Revision, P(game)!.PromptId));
            if (scenario == "before-acceptance")
            {
                Reach(game, () => MovementOption(game, MovementBefore, "before"));
                var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last();
                Need(window.Candidates.Any(candidate => candidate.SkillId == MovementSkill) &&
                    window.InvocationAuthorizations?.Any(authorization => authorization.Candidate.SkillId == MovementSkill) != true &&
                    !game.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == MovementSkill) &&
                    !E<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == MovementSkill),
                    "The original movement candidate has no pre-signed permission: a real earlier skill-loss effect runs before it may Begin.");
                game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "before");
                Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
                Need(!E<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == MovementSkill) &&
                    !E<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == MovementDrawReason),
                    "Losing an unaccepted movement source cannot create its program or issue its unpaid Draw after cold replay.");
                continue;
            }
            Reach(game, () => MovementOption(game, MovementSkill, "accepted"));
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == MovementSkill);
            var parent = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(item => item.Id == frame.WindowContext!.ParentFrameId);
            Need(frame.WindowContext is { Window: SkillProgramTriggerWindow.CardsGained, NativeGive: null, MovementBatch: { } } &&
                frame.InvocationAuthorization is { RetainOwnerDeath: false } ticket && ticket.ProgramFrameId == frame.Id &&
                ticket.CandidateIndex == parent.CandidateIndex && ticket.NativeFrameId == (parent.Batch.ParentFrameId ?? 0) &&
                ProgramInvocations.Allows(frame, parent) && !E<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == MovementDrawReason),
                "A plain CardsGained JSON consumer acquires its child-specific movement permission only after Begin, with owner-death permission still stopped.");
            var frameId = frame.Id;
            game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "accepted");
            if (scenario == "source-loss")
            {
                Reach(game, () => MovementOption(game, MovementSkill, "lost"));
                Need(!game.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == MovementSkill) &&
                    !E<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == MovementDrawReason),
                    "The accepted program really loses its own source through a replayable state instruction before issuing its suffix.");
                game = NativeGiveCold(game, registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "lost");
                Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
                Need(E<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == frameId) is { RecipientSeat: 0, RequestedCount: 1, ActualDrawCount: 1 } &&
                    E<ProgramBindingResolvedEvent>(game).Count(fact => fact.FrameId == frameId && fact.Completed) == 1 &&
                    E<NativeDrawAttemptStartedEvent>(game).Count(fact => fact.Context.Reason.Value == MovementDrawReason) == 1,
                    "The accepted original program completes one ordinary Draw and returns once after real source loss and command cold recovery.");
                _ = NativeGiveCold(game, registry);
            }
            else
            {
                for (var step = 0; step < 240 && !E<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId); step++)
                {
                    if (P(game) is { PlayerSeat: 0, Kind: DecisionKind.RescueDying })
                        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
                    else Accept(game, new AdvanceOneStepCommand(game.Revision));
                }
                Need(!game.CreateSnapshot(0).Players[0].IsAlive && E<PlayerDiedEvent>(game).Any(fact => fact.VictimSeat == 0) &&
                    !E<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == MovementDrawReason) &&
                    E<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == frameId).Completed == false,
                    "Accepted source-loss permission does not authorize an unpaid Draw suffix after its owner actually dies.");
                _ = NativeGiveCold(game, registry);
            }
        }
    }

    private static bool MovementOption(GameEngine game, string skill, string option) => P(game)?.SkillPrompt?.SkillId == skill &&
        P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == option);

    private sealed class MovementInvocationFixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-movement-invocation", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var suffix = scenario == "owner-death" ? """{"op":"loseHp","target":"owner","amount":20},""" : $$$"""
                {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{MovementSkill}}}"]}},
                {"op":"chooseOption","target":"owner","resultBind":"lost","options":[{"id":"lost"}]},
                """;
            var labels = new Dictionary<string, string> { ["accepted"] = "Accepted" };
            if (scenario != "owner-death") labels["lost"] = "Source lost";
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{MovementDriver}}}","revision":1,"activations":[{"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}]}]},
                  {"id":"{{{MovementSkill}}}","revision":1,"triggers":[{"id":"accepted","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.{{{MovementDriver}}}.Draw"],"movementOccurrence":"perBatch","optional":false,"priority":100,"invocation":{"source":"movement"},"effects":[
                    {"op":"chooseOption","target":"owner","resultBind":"accepted","options":[{"id":"accepted"}]},{{{suffix}}}{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{{MovementBefore}}}","revision":1,"triggers":[{"id":"before","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.{{{MovementDriver}}}.Draw"],"movementOccurrence":"perBatch","optional":false,"priority":200,"effects":[
                    {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{MovementSkill}}}"]}},{"op":"chooseOption","target":"owner","resultBind":"before","options":[{"id":"before"}]}]}]},
                  {"id":"{{{MovementGate}}}","revision":1,"triggers":[{"id":"native","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
                  {"id":"{{{GiveQuiet}}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                {
                    [MovementDriver] = new { name = "Movement producer", description = "One real gain" },
                    [MovementSkill] = new { name = "Accepted movement", description = "Ordinary issued program", optionLabels = labels },
                    [MovementBefore] = new { name = "Earlier actual loss", description = "Before candidate acceptance", optionLabels = new Dictionary<string, string> { ["before"] = "Before acceptance" } },
                    [MovementGate] = new { name = "Native producer", description = "Real Draw request" },
                    [GiveQuiet] = new { name = "Quiet", description = "Fixed empty opening" }
                } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(MovementOwner, "Movement owner", "", MovementDriver, "wu", 4,
                scenario == "before-acceptance" ? [MovementSkill, MovementBefore, MovementGate, GiveQuiet] : [MovementSkill, MovementGate, GiveQuiet]));
            builder.AddGeneral(new(GivePeer, "Fixed peer", "", GiveQuiet, "qun", 8));
            const string deck = "fixture:movement-invocation-deck";
            builder.AddDeck(new(deck, "Fixed native deck", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 9)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Accepted movement", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [MovementOwner, GivePeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

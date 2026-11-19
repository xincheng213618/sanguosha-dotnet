using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlMoDiaoChanQingshiChecks
{
    private const string Qingshi = "ol:mo-qingshi", Recapture = "skill-program.ol:mo-qingshi.recapture";
    private const string Claim = "fixture:mo-dc-claim", Drop = "fixture:mo-dc-drop";
    private const string DiscardReason = "skill-program.fixture:mo-dc-drop.DiscardSelected";
    private const string ClaimReason = "skill-program.fixture:mo-dc-claim.ClaimMovedCards";
    private const string TransferReason = "skill-program.fixture:mo-dc-claim.SelectAndMoveOwnedCard";
    private const string Mode = "identity:mo-dc-qingshi", Deck = "fixture:mo-dc-deck";
    private static readonly string[] Generals = ["fixture:mo-dc-claimant", "fixture:mo-dc-owner", "fixture:mo-dc-discarder", "fixture:mo-dc-peer"];

    internal static void RecaptureUsesTheOriginalNonUseDiscardEntry()
    {
        var (game, content) = Start();
        EndPlay(game, 0);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Qingshi } && prompt.Choices.Any(c => Branch(c) == "enter"));
        Answer(game, choice => Branch(choice) == "enter");
        Reach(game, prompt => IsPlay(prompt, 1));
        var grant = game.Events.Select(e => e.Payload).OfType<MoDiaoChanCardGrantedEvent>().Single(e => e.RecipientSeat == 2);
        Require(grant is { Source.OwnerSeat: 1, Kind: CardKind.Slash } && game.CardMovements.Any(move =>
                move.Sequence == grant.Sequence && move.CardId == grant.CardId && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(2)),
            "Real Qingshi grants the original marked Slash to the later discarder.");
        EndPlay(game, 1);
        Reach(game, prompt => IsPlay(prompt, 2));
        Accept(game, new UseProgramSkillCommand(2, Drop, "drop-marked", [grant.CardId], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(c => Action(c) == "activate"));
        var original = game.CardMovements.Single(move => move.CardId == grant.CardId && move.Reason.Value == DiscardReason);
        Require(original.From == CardLocation.Hand(2) && original.To == CardLocation.DiscardPile &&
                game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.Batch.Id == original.BatchId &&
                    window.Candidates.Any(candidate => candidate.SkillId == Qingshi)),
            "The original non-use discard queues Qingshi behind the earlier claimant on the same movement window.");

        var positive = Cold(game, content);
        Answer(positive, choice => Action(choice) == "skip");
        Reach(positive, prompt => IsPlay(prompt, 2));
        var recaptured = positive.CardMovements.Single(move => move.Reason.Value == Recapture);
        Require(recaptured.CardId == grant.CardId && recaptured.From == CardLocation.DiscardPile && recaptured.To == CardLocation.Hand(1) &&
                positive.Events.Select(e => e.Payload).OfType<MoDiaoChanDiscardClaimedEvent>().Single() is { Sequence: var claimed } &&
                claimed == original.Sequence,
            "The marked entity still at the original non-use entry is recaptured by Qingshi.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(c => c.Targets.SequenceEqual([1])));
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(c => c.Cards.SequenceEqual([grant.CardId])));
        Answer(game, choice => choice.Cards.SequenceEqual([grant.CardId]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(c => Action(c) == "request-slash"));
        var taken = game.CardMovements.Single(move => move.CardId == grant.CardId && move.Reason.Value == ClaimReason);
        var transfer = game.CardMovements.Where(move => move.CardId == grant.CardId && move.Reason.Value == TransferReason)
            .OrderBy(move => move.Sequence).ToArray();
        Require(taken.Sequence > original.Sequence && taken.From == CardLocation.DiscardPile && taken.To == CardLocation.Hand(0) &&
                transfer is [var departed, var delivered] && departed.Sequence > taken.Sequence && departed.From == taken.To &&
                departed.To == CardLocation.Processing && delivered.From == departed.To && delivered.To == CardLocation.Hand(1) &&
                game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.Batch.Id == original.BatchId &&
                    window.Candidates.Skip(window.CandidateIndex + 1).Any(candidate => candidate.SkillId == Qingshi)),
            "The earlier claimant takes and passes the same physical card while the original Qingshi candidate is still queued.");

        game = Cold(game, content);
        Answer(game, choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([grant.CardId]));
        FinishSlash(game);
        var native = game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>().Single(e => e.CardId == grant.CardId);
        var cleanup = game.CardMovements.Single(move => move.CardId == grant.CardId && move.Reason == CardMoveReasons.UseFinished);
        Require(native is { SourceSeat: 1, CardKind: CardKind.Slash } &&
                game.Events.Select(e => e.Payload).OfType<CardUseFinishedEvent>().Any(e => e.ResolutionId == native.ResolutionId) &&
                game.Events.Select(e => e.Payload).OfType<MoDiaoChanUseMaterialEvent>().Any(e => e.CardId == grant.CardId) &&
                cleanup.From == CardLocation.Processing && cleanup.To == CardLocation.DiscardPile &&
                cleanup.Sequence > transfer[^1].Sequence && cleanup.BatchId != original.BatchId,
            "The earlier child uses the same Slash normally and produces a later native use cleanup entry.");
        Require(game.CardMovements.All(move => move.Reason.Value != Recapture) &&
                game.Events.All(e => e.Payload is not MoDiaoChanDiscardClaimedEvent) &&
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == grant.CardId).Location == CardLocation.DiscardPile,
            "The original non-use Qingshi opportunity must leave the later use cleanup entry in the discard pile.");
    }

    private static bool IsPlay(PendingDecision prompt, int seat) => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == seat;
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");

    private static void EndPlay(GameEngine game, int seat) => Accept(game, new EndPlayPhaseCommand(seat, game.Revision, P(game)!.PromptId));

    private static void FinishSlash(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is { } prompt)
            {
                if (IsPlay(prompt, 2)) return;
                Require(prompt.Kind == DecisionKind.RespondDodge, "Only the native Slash defence may interrupt the queued discard window.");
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The original discarder did not return to Play after the native Slash.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, 4).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay retains the original Qingshi window, its movement entries and the current decision for each viewer.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        ReachSetupBoundary();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachSetupBoundary();
        }
        return (game, content);

        void ReachSetupBoundary() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || IsPlay(prompt, 0));
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mo-dc-qingshi-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:mo-diao-chan", "ol:mo-sun-quan");
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Drop}}}","revision":1,"activations":[{"id":"drop-marked","minCards":1,"maxCards":1,
                    "minTargets":0,"maxTargets":0,"targetKind":"anyLiving","sourceZones":["hand"],"usesPerTurn":1,
                    "effects":[{"op":"discardSelected","target":"owner","amount":1}]}]},
                  {"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-qingshi","window":"discardPileReceived",
                    "subject":"owner","discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{DiscardReason}}}"],
                    "optional":true,"effects":[
                      {"op":"claimMovedCards","target":"owner"},
                      {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true},
                      {"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"slash"}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Drop] = new { name = "弃牌", description = "弃置实际手牌。" },
                    [Claim] = new { name = "先行领取", description = "先领取原弃牌，然后交给另一角色请求使用。" }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "先行领取者", "supporter", Claim, "qun", 8));
            builder.AddGeneral(new(Generals[1], "倾世持有者", "ol-mo-diao-chan", Qingshi, "qun", 8, Gender: GeneralGender.Female));
            builder.AddGeneral(new(Generals[2], "原弃置者", "supporter", Drop, "qun", 8));
            builder.AddGeneral(new(Generals[3], "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "倾世原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlGuoSiTanbeiDyingPeachChecks
{
    private const string GuoSi = "ol:guo-si", Tanbei = "ol:tanbei", GainReason = "skill-program.tanbei.gain";
    private const string Victim = "fixture:tanbei-dying-victim", Watch = "fixture:tanbei-dying-payment";
    private const string Helper = "fixture:tanbei-dying-helper", Reserve = "fixture:tanbei-dying-reserve";
    private const string Mode = "identity:classic-tanbei-dying-peach", Deck = "fixture:tanbei-dying-deck";
    private const int Actor = 0, Target = 1, OtherHelper = 2, LastHelper = 3;
    private static readonly string[] Generals = [GuoSi, Victim, Helper, Reserve];

    internal static void TanbeiTargetProhibitionBlocksOnlyGuoSiDyingPeachCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(Actor, Tanbei, "choose-target-pact", [], [Target], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt.PlayerSeat == Target && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "guo-si"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "0");
        Reach(game, prompt => prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: Target });

        var paid = Facts<GuoSiPactPaidEvent>(game).Single();
        var policy = Facts<DirectedTurnCardPolicyGrantedEvent>(game).Single(fact => fact.Policy.ParentFrameId == paid.Pact.FrameId).Policy;
        var dying = game.ResolutionStack.OfType<DyingFrame>().Single();
        var movement = game.CardMovements.Single(move => move.Reason.Value == GainReason);
        Require(paid.Pact is { Actor: Actor, Target: Target, Branch: 0 } && paid.ActualCount == 1 &&
            movement.From == CardLocation.Hand(Target) && movement.To == CardLocation.Hand(Actor) && movement.Sequence > paid.Before && movement.Sequence <= paid.After &&
            policy is { ActorSeat: Actor, TargetSeat: Target, Effects: DirectedTurnCardPolicyEffect.ForbidTarget, CardKinds.Count: 0 } &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Batch.ParentFrameId == paid.Pact.FrameId && frame.Batch.Movements.Contains(movement)) &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch).Id == dying.ParentFrameId &&
            dying.VictimSeat == Target && game.CreateSnapshot(Target).Players[Target].Hp == 0,
            "The real received-card batch issues the directed restriction before its paid movement child puts the original target into Dying.");
        var guoPeach = game.CreateSnapshot(Actor).Players[Actor].Hand.First(card => card.Kind == CardKind.Peach).Id;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
        Reach(game, prompt => prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: OtherHelper });

        var control = Cold(game, content);
        var otherPeach = P(control)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "peach");
        Answer(control, choice => choice.Id == otherPeach.Id);
        Reach(control, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: Actor });
        Require(Facts<DyingResponseEvent>(control).Single(fact => fact.UsedPeach) is { ResponderSeat: OtherHelper } response &&
            response.ResolutionId == dying.Id && response.PeachCardId == otherPeach.Cards.Single() &&
            Facts<DyingResolvedEvent>(control).Single(fact => fact.ResolutionId == dying.Id).Survived &&
            control.CardMovements.Count(move => move.CardId == response.PeachCardId && move.From == CardLocation.Hand(OtherHelper) && move.To == CardLocation.Processing) == 1,
            "The same live pact leaves an uninvolved player's real Peach legal and saves the original target.");

        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
        Reach(game, prompt => prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: LastHelper });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
        Reach(game, prompt => prompt.PlayerSeat == Actor && prompt.Kind is DecisionKind.RescueDying or DecisionKind.PlayCard);
        var decision = P(game)!;
        var before = State(game);
        var reply = game.Submit(new AnswerPromptCommand(Actor, decision.PromptId, new($"dying.peach.card-{guoPeach}.source-0"), game.Revision));
        Require(!reply.Accepted && decision.Kind != DecisionKind.RescueDying && State(game) == before,
            $"The original paid Tanbei target prohibition must block Guo Si's physical Peach. Published {decision.Kind}, accepted {reply.Accepted}.");
        Require(Facts<DyingResolvedEvent>(game).Single(fact => fact.ResolutionId == dying.Id) is { Survived: false } &&
            Facts<DyingResponseEvent>(game).All(fact => !fact.UsedPeach) &&
            game.CreateSnapshot(Actor).Players[Actor].Hand.Any(card => card.Id == guoPeach),
            "Guo Si's original Peach stays in hand when the other responders all decline the original Dying.");
        _ = Cold(game, content);
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, game.PlayerCount).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))),
        Frames = JsonSerializer.Serialize(game.ResolutionStack), game.CardMovements,
        Facts = game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())),
        Commands = CommandJson.Serialize(game.AcceptedCommands)
    });

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(State(restored) == State(game), "Cold commands preserve the actual paid pact, original Dying and each seat's rescue rights.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, ModeId = Mode, HumanSeat = Actor,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
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
        public PackageManifest Manifest { get; } = new("fixture-ol-tanbei-dying-peach", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, GuoSi);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"paid-card-lost","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["{{{GainReason}}}"],"optional":false,
                    "effects":[{"op":"loseHp","target":"owner","amount":1}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "实际离手", description = "贪狈实际取得手牌后失去一点体力。" } } }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Victim, "真实被选者", "supporter", Watch, "wei", 4) { InitialHp = 1 });
            builder.AddGeneral(new(Helper, "旁座持桃者", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Reserve, "后位持桃者", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定实体桃", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 36)
                .Select(index => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "贪狈定向限制濒死桃", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

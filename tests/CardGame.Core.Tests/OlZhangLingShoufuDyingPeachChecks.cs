using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlZhangLingShoufuDyingPeachChecks
{
    private const string ZhangLing = "ol:zhang-ling", Shoufu = "ol:shoufu";
    private const string Victim = "fixture:shoufu-dying-victim", Driver = "fixture:shoufu-dying-driver";
    private const string Helper = "fixture:shoufu-dying-helper", Reserve = "fixture:shoufu-dying-reserve";
    private const string Mode = "identity:classic-shoufu-dying-peach", Deck = "fixture:shoufu-dying-deck";
    private const int Caster = 0, DyingSeat = 1, HelperSeat = 2, ReserveSeat = 3;
    private static readonly string[] Generals = [ZhangLing, Victim, Helper, Reserve];

    internal static void BasicShoufuBlocksOnlyItsHoldersNativeDyingPeachCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(Caster, Shoufu, "draw-and-place-talisman", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Shoufu && prompt.Choices.Any(choice => choice.Cards.Count == 1));
        var talismanId = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([talismanId]));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Shoufu && prompt.ValidTargetSeats.Contains(HelperSeat) && prompt.ValidTargetSeats.Contains(ReserveSeat));

        var control = Cold(game, content);
        var controlDying = PlaceThenEnterRescue(control, talismanId, holder: ReserveSeat);
        Require(P(control) is { PlayerSeat: HelperSeat }, "A basic talisman on the other player leaves the original helper's physical Peach legal.");
        var originalChoice = P(control)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "peach");
        var peachId = originalChoice.Cards.Single();
        control = Cold(control, content);
        Answer(control, choice => choice.Id == originalChoice.Id);
        CheckRescue(control, controlDying, HelperSeat, peachId);

        var blockedDying = PlaceThenEnterRescue(game, talismanId, holder: HelperSeat);
        game = Cold(game, content);
        var before = State(game);
        var decision = P(game)!;
        Require(game.CreateSnapshot(HelperSeat).Players[HelperSeat].Hand.Any(card => card.Id == peachId && card.Kind == CardKind.Peach),
            "The restricted helper still holds the very same real Peach from the positive control.");
        var rejected = game.Submit(new AnswerPromptCommand(HelperSeat, decision.PromptId, originalChoice.Id, game.Revision));
        Require(!rejected.Accepted && State(game) == before && P(game) is { PlayerSeat: ReserveSeat } &&
            game.CreateSnapshot(HelperSeat).PendingDecision is null,
            $"The basic talisman must block the holder's physical Peach in the original Dying. Published seat {decision.PlayerSeat}, accepted {rejected.Accepted}.");
        var reservePeachId = P(game)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "peach").Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([reservePeachId]));
        CheckRescue(game, blockedDying, ReserveSeat, reservePeachId);
        Require(game.CreateSnapshot(HelperSeat).Players[HelperSeat].Hand.Any(card => card.Id == peachId) &&
            Facts<DyingResponseEvent>(game).All(fact => !fact.UsedPeach || fact.ResponderSeat != HelperSeat),
            "Blocking one holder keeps their Peach unspent while the other player rescues normally.");
        _ = Cold(game, content);
    }

    private static long PlaceThenEnterRescue(GameEngine game, int talismanId, int holder)
    {
        Answer(game, choice => choice.Targets.SequenceEqual([holder]));
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: Caster });
        var placed = Facts<CategoryTalismanPlacedEvent>(game).Single();
        Require(placed.Source.SkillId == Shoufu && placed.HolderSeat == holder && placed.Category == SkillProgramCardCategory.Basic &&
            placed.CardId == talismanId && game.CardMovements.Count(move => move.CardId == talismanId && move.From == CardLocation.Hand(Caster) &&
                move.To == placed.Pile.Location && move.Sequence > placed.Before && move.Sequence <= placed.After) == 1,
            "The real Shoufu use places its selected physical basic card in the original recipient's public talisman pile.");
        Accept(game, new EndPlayPhaseCommand(Caster, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: DyingSeat });
        Accept(game, new UseProgramSkillCommand(DyingSeat, Driver, "lose-last-hp", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: DyingSeat });
        var dying = game.ResolutionStack.OfType<DyingFrame>().Single();
        Require(dying.VictimSeat == DyingSeat && game.CreateSnapshot(DyingSeat).Players[DyingSeat].Hp == 0 &&
            Facts<PlayerDyingEvent>(game).Single().ResolutionId == dying.Id &&
            Facts<CategoryTalismanReleasedEvent>(game).Length == 0 && game.CardMovements.Last(move => move.CardId == talismanId).To == placed.Pile.Location,
            "The original Dying opens without releasing the other player's basic talisman.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
        Reach(game, prompt => prompt.Kind == DecisionKind.RescueDying && prompt.PlayerSeat is HelperSeat or ReserveSeat);
        return dying.Id;
    }

    private static void CheckRescue(GameEngine game, long dyingId, int helper, int cardId)
    {
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: DyingSeat });
        Require(Facts<DyingResponseEvent>(game).Single(fact => fact.UsedPeach) is { } response &&
            response.ResolutionId == dyingId && response.ResponderSeat == helper && response.PeachCardId == cardId &&
            Facts<DyingResolvedEvent>(game).Single(fact => fact.ResolutionId == dyingId).Survived &&
            Facts<RecoveryAppliedEvent>(game).Single() == new RecoveryAppliedEvent(helper, DyingSeat, 1, 1) &&
            game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Hand(helper) && move.To == CardLocation.Processing) == 1 &&
            game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
            "One real physical Peach from the eligible helper saves the original victim and finishes exactly once.");
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
        Require(State(restored) == State(game), "Cold replay preserves the original public talisman and each seat's private rescue rights.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, ModeId = Mode, HumanSeat = Caster,
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
        public PackageManifest Manifest { get; } = new("fixture-ol-shoufu-dying-peach", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ZhangLing);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"lose-last-hp","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "实际濒死", description = "失去一点体力。" } } }));
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Victim, "真实濒死者", "supporter", Driver, "wei", 4) { InitialHp = 1 });
            builder.AddGeneral(new(Helper, "指定持桃者", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Reserve, "后位持桃者", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定实体桃", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 36)
                .Select(index => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "授符限制濒死桃", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

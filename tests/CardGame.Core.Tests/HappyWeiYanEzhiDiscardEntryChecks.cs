using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class HappyWeiYanEzhiDiscardEntryChecks
{
    private const string Ezhi = "happy:ezhi", Gujing = "happy:gujing";
    private const string Watch = "fixture:ezhi-reentry", Mode = "identity:ezhi-entry", Deck = "fixture:ezhi-entry-deck";
    private const string DiscardBone = "skill-program.happy:gujing.discard";
    private const string Rediscard = "skill-program.fixture:ezhi-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:ezhi-owner", "fixture:ezhi-holder", "fixture:ezhi-peer-a", "fixture:ezhi-peer-b"];

    internal static void EzhiUsesOnlyTheOriginalDiscardedBoneCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var play = Next(game);
        var bone = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The actual Gujing owner has an original physical card to place and the four opening hands exhaust the draw pile.");
        Accept(game, new UseProgramSkillCommand(0, Gujing, "bone-path", [], [1], game.Revision, play.PromptId));
        GujingChoice(game, "card:" + bone);
        GujingChoice(game, "slot:Weapon");
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Gujing } &&
            game.CreateSnapshot(0).Players[1].Equipment.Any(card => card.Id == bone) && game.GetAttackRange(1) == 1,
            "The Qinggang Sword occupies a weapon slot as a bone without granting its printed range of two.");
        GujingChoice(game, "abolish:" + bone);
        GujingChoice(game, "finish");

        var placed = game.CardMovements.Single(move => move.CardId == bone && move.From == CardLocation.Hand(0) && move.To == CardLocation.Equipment(1));
        var discarded = game.CardMovements.Single(move => move.CardId == bone && move.Reason.Value == DiscardBone);
        play = Next(game);
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && placed.CardKind == CardKind.QinggangSword &&
            discarded.From == placed.To && discarded.To == CardLocation.DiscardPile && discarded.Sequence > placed.Sequence &&
            Zone(game, bone) == CardLocation.DiscardPile &&
            Facts<HappyWeiYanBonePlacedEvent>(game).Single() is { Holder: 1, Slot: EquipmentSlot.Weapon } placement && placement.After == placed.Sequence &&
            Facts<HappyWeiYanGujingAbolishedEvent>(game).Single() is { Holder: 1, Slot: EquipmentSlot.Weapon } abolition && abolition.After == discarded.Sequence,
            "The real limited skill first places the weapon as a bone and physically discards it before abolishing the other player's slot.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        var earlier = Next(game);
        var ended = game.ResolutionStack.OfType<DeferredTurnEndFrame>().Single();
        Require(earlier is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && ended.TurnNumber == discarded.TurnNumber &&
            Facts<TurnEndedEvent>(game).Any(fact => fact.ActorSeat == 0 && fact.TurnNumber == ended.TurnNumber),
            "The earlier public choice is in the actual end of the same turn that discarded the bone.");

        var control = Cold(game, content);
        Skip(control);
        var offered = Next(control);
        Require(offered is { PlayerSeat: 0, SkillPrompt.SkillId: Ezhi } && offered.Choices.Any(choice => Uses(choice, bone)) &&
            control.ResolutionStack.OfType<DeferredTurnEndFrame>().Single().Id == ended.Id,
            "The untouched original discarded bone is offered for a genuine equipment use at the original actual turn end.");
        Answer(control, choice => Uses(choice, bone));
        Require(Next(control) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } &&
            Facts<HappyWeiYanBoneUseIssuedEvent>(control).Single() is { Ordinal: 0 } issue && issue.CardId == bone && issue.DiscardSequence == discarded.Sequence &&
            Facts<HappyWeiYanBoneUseReturnedEvent>(control).Single().UseId == issue.UseId &&
            Facts<CardUseFinishedEvent>(control).Single(fact => fact.ResolutionId == issue.UseId) is { CardKind: CardKind.QinggangSword } use && use.CardId == bone &&
            Zone(control, bone) == CardLocation.Equipment(0) && control.GetAttackRange(0) == 2,
            "Ezhi uses and equips the same sword once, now granting its printed range of two.");

        Activate(game);
        var rethrow = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == bone && move.Sequence > discarded.Sequence).ToArray();
        Require(rethrow is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && rethrow.Choices.Any(choice => choice.Cards.SequenceEqual([bone])) &&
            game.ResolutionStack.OfType<DeferredTurnEndFrame>().Single().Id == ended.Id &&
            pickup is [var reshuffled, var drawn] && reshuffled.Reason == CardMoveReasons.Reshuffle &&
            reshuffled.From == CardLocation.DiscardPile && reshuffled.To == CardLocation.DrawPile && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The earlier actual turn-end effect physically shuffles and draws the bone before asking for a real hand discard.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([bone]));
        var fresh = game.CardMovements.Last(move => move.CardId == bone && move.To == CardLocation.DiscardPile);
        Require(fresh.From == CardLocation.Hand(0) && fresh.Sequence > pickup[^1].Sequence && fresh.TurnNumber == discarded.TurnNumber &&
            fresh.BatchId != discarded.BatchId && fresh.Reason.Value == Rediscard && Zone(game, bone) == CardLocation.DiscardPile,
            "The same physical weapon now has a separate, current ordinary hand discard entry in the original turn.");

        var next = Next(game);
        if (next is { PlayerSeat: 0, SkillPrompt.SkillId: Ezhi } && next.Choices.Any(choice => Uses(choice, bone)))
            Answer(game, choice => Uses(choice, bone));
        Require(Next(game) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } && next.SkillPrompt?.SkillId != Ezhi &&
            !Facts<HappyWeiYanBoneUseIssuedEvent>(game).Any() && Zone(game, bone) == CardLocation.DiscardPile,
            "The original discarded-bone permission cannot use the same weapon from its later ordinary hand discard.");
    }

    private static bool Uses(PromptChoice choice, int bone) =>
        choice.Parameters.GetValueOrDefault("option")?.StartsWith("use:", StringComparison.Ordinal) == true && choice.Cards.SequenceEqual([bone]);

    private static void GujingChoice(GameEngine game, string option)
    {
        var prompt = Next(game);
        Require(prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Gujing } && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == option),
            $"The actual Gujing choice is offered for {option}.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == option);
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return game;
    }

    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(fact => fact.CardId == id).Location;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restoration preserves the original actual turn end, movement entries and every seat's prompt.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-happy-wei-yan-ezhi-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "happy:shen-wei-yan");
            var rules = $$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[{"id":"draw-and-discard","window":"afterTurnEnded",
                  "subject":"owner","priority":1000,"optional":true,"effects":[
                  {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                  {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                   "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "摸牌后弃置", description = "实际回合结束时，可以摸一张牌并弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Generals[0], "恶峙与骨径", "supporter", Ezhi, "shu", 6, [Gujing, Watch]));
            foreach (var general in Generals.Skip(1))
                builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "四张真实青釭剑", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 4).Select(rank => new ContentDeckPhysicalCard("standard:qinggang_sword", Suit.Club, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "欢乐恶峙原弃骨", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class HappySimaYiRenjieDiscardExecutorChecks
{
    private const string Renjie = "happy:renjie", Driver = "fixture:renjie-phase-discard";
    private const string Mode = "identity:renjie-discard-executor", Deck = "fixture:renjie-discard-deck";
    private const string DiscardReason = "skill-program.fixture:renjie-phase-discard.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:renjie-owner", "fixture:renjie-chooser", "fixture:renjie-peer-2", "fixture:renjie-peer-3"];

    internal static void RenjieCountsOnlyTheActualSelfDiscardExecutorCold()
    {
        Check(chooserSeat: 0);
        Check(chooserSeat: 1);
    }

    private static void Check(int chooserSeat)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        var play = Next(game);
        while (play.Kind == DecisionKind.SelectGeneral)
        {
            Accept(game, new SelectGeneralCommand(play.PlayerSeat, Generals[play.PlayerSeat], game.Revision, play.PromptId));
            play = Next(game);
        }

        var cardId = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && RenChanges(game) is [{ Delta: 1, Count: 1 }],
            "Renjie has only the initial one Ren when the first actual Play phase starts.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Driver } && game.State.Phase == TurnPhase.Discard && game.State.CurrentSeat == 0,
            "The authorized selector opens in the original owner's actual discard phase.");
        Answer(game, choice => choice.Targets.SequenceEqual([chooserSeat]));
        var payment = Next(game);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Driver);
        Require(payment.PlayerSeat == chooserSeat && payment.SkillPrompt?.SkillId == Driver && payment.Choices is [var offered] &&
            offered.Parameters.GetValueOrDefault("source-zone") == "Hand" && offered.Parameters.GetValueOrDefault("card-owner-seat") == "0" &&
            offered.Cards.SequenceEqual(chooserSeat == 0 ? [cardId] : Array.Empty<int>()),
            "The physical hand card belongs to the phase owner while the selected executor makes the real payment choice.");
        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(cold.CardMovements.SequenceEqual(game.CardMovements) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(cold.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restoration preserves the actual payer, physical source and hidden hand choice.");
        game = cold;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("source-zone") == "Hand");
        var next = Next(game);
        var movement = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == DiscardReason);
        var invoice = Facts<NativeLogicalDiscardInvocationRecordedEvent>(game).Single(fact => fact.ParentFrameId == frame.Id);
        var material = Facts<NativeLogicalDiscardMaterialCompletedEvent>(game).Single(fact => fact.InvocationId == invoice.InvocationId);
        Require(next is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } && movement.From == CardLocation.Hand(0) &&
            movement.To == CardLocation.DiscardPile && movement.MovementTiming is { ActualTurnOwnerSeat: 0, PhaseActorSeat: 0, Phase: TurnPhase.Discard } &&
            invoice is { InitiatorSeat: 0, SubjectSeat: 0, ActualTurnOwnerSeat: 0, ActualCount: 1, HandCount: 1 } &&
            invoice.ExecutorSeat == chooserSeat && invoice.TurnNumber == movement.TurnNumber &&
            material.CardId == cardId && material.CompletionSequence == movement.Sequence && material.From == movement.From && material.To == movement.To,
            "The completed native discard records the same physical hand card and phase in both cases, with the two different actual executors.");

        var changes = RenChanges(game);
        var expected = chooserSeat == 0 ? 2 : 1;
        Require(changes.Length == expected && changes.All(change => change.Delta == 1) && changes[^1].Count == expected,
            $"Renjie awards one Ren for the participant's own actual discard. Executor {chooserSeat}; expected {expected} Ren, actual {changes[^1].Count}.");
    }

    private static PlayerMarkerChangedEvent[] RenChanges(GameEngine game) => Facts<PlayerMarkerChangedEvent>(game)
        .Where(fact => fact is { PlayerSeat: 0, Marker: PlayerMarkerKind.Ren }).ToArray();

    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-happy-renjie-discard-executor", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "happy:shen-sima-yi");
            var rules = $$"""
                {"skills":[{"id":"{{Driver}}","revision":1,"triggers":[{"id":"choose-actual-discard-executor",
                  "window":"discardPhaseStarting","subject":"owner","optional":false,"effects":[
                    {"op":"selectTarget","target":"owner","targetKind":"anyLiving"},
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                      "cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "选择弃置者", description = "令一名角色弃置你的一张手牌。" }
                }
            });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var shown = catalog.Presentations[Driver];
            builder.AddSkill(new(Driver, shown.Name, shown.Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = shown });
            builder.AddGeneral(new(Generals[0], "忍戒角色", "supporter", Renjie, "wei", 3, [Driver]));
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "shu", 4));
            builder.AddDeck(new(Deck, "四张实体闪", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 4).Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "忍戒实际弃牌执行者", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

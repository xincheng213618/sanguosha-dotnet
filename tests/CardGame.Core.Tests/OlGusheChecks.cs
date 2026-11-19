using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlGusheChecks
{
    private const string Gushe = "ol:gushe", WangLang = "ol:wang-lang";
    private const string Mode = "identity:ol-gushe-cleanup", Deck = "fixture:ol-gushe-deck";
    private static readonly string[] Generals = [WangLang, "fixture:gushe-peer-one", "fixture:gushe-peer-two", "fixture:gushe-peer-three"];

    public static void ThreeComparisonsCleanOriginalMaterialsInOrderAndReplay()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        ReachSetupOrPlay();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } setup)
        {
            Accept(game, new SelectGeneralCommand(setup.PlayerSeat, Generals[setup.PlayerSeat], game.Revision, setup.PromptId));
            ReachSetupOrPlay();
        }

        var source = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        int[] opponents = [3, 1, 2];
        Accept(game, new UseProgramSkillCommand(0, Gushe, "simultaneous-pindian", [source], opponents, game.Revision, P(game)!.PromptId));
        var start = Events<SimultaneousPindianStartedEvent>(game).Single();
        Require(start.Source.SkillId == Gushe && start.Opponents.SequenceEqual(opponents),
            "The real OL Gushe activation owns the original ordered group.");
        foreach (var seat in opponents)
        {
            Reach(game, p => p.PlayerSeat == seat && p.Choices.Any(c => Branch(c) == "commit"));
            Require(P(game) is { IsPrivate: true } && !game.CardMovements.Any(m => m.Reason == CardMoveReasons.PindianReveal),
                "All real opponents must commit privately before the common reveal.");
            Answer(game, c => Branch(c) == "commit");
        }

        Reach(game, p => p.PlayerSeat == 0 && p.Choices.Any(c => Branch(c) == "draw"));
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == start.ProgramFrameId);
        var group = frame.SimultaneousPindian!;
        var reveal = Events<PindianCardsRevealedEvent>(game).Single();
        var reveals = game.CardMovements.Where(m => m.Reason == CardMoveReasons.PindianReveal).ToArray();
        Require(reveal is { Simultaneous: true } && reveal.ParentFrameId == frame.Id &&
                reveal.Cards.Select(c => c.Seat).SequenceEqual(new[] { 0 }.Concat(opponents)) && reveal.Cards[0].CardId == source &&
                group.ProcessingEntries!.Select(entry => (entry.CardId, entry.EntrySequence))
                    .SequenceEqual(reveals.Select(move => (move.CardId, (long)move.Sequence))) &&
                reveals.Select(move => move.CardId).SequenceEqual(reveal.Cards.Select(c => c.CardId)),
            "The native group must retain the exact ordered physical Processing entries for all four revealed cards.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
            game.CardMovements.SequenceEqual(restored.CardMovements),
            "Cold command replay must retain the same published consequence, original reveal entries, and card movements.");
        game = restored;

        foreach (var opponent in opponents)
        {
            Reach(game, p => p.PlayerSeat == 0 && p.Choices.Any(c => Branch(c) == "draw"));
            Answer(game, c => Branch(c) == "draw");
            Reach(game, p => p.PlayerSeat == opponent && p.Choices.Any(c => Branch(c) == "discard"));
            Answer(game, c => Branch(c) == "discard");
        }
        Settle(game);

        var results = Events<PindianResultDeterminedEvent>(game);
        var payments = Events<SimultaneousPindianConsequencePaidEvent>(game);
        Require(results.Select(e => e.Result.OpponentSeat).SequenceEqual(opponents) &&
                results.All(e => e.FrameId == frame.Id && e.Result.SourceCardId == source && e.Result.SourceRank == e.Result.OpponentRank) &&
                payments.Select(p => (p.ParticipantSeat, p.Branch)).SequenceEqual(opponents.SelectMany(seat =>
                    new[] { (0, "draw"), (seat, "discard") })) &&
                Events<SimultaneousPindianCompletedEvent>(game).Single() is { CompletedComparisons: 3 },
            "All three ordered comparisons reuse the one common source entity and each nonwinner pays once.");
        var finished = game.CardMovements.Where(m => m.Reason == CardMoveReasons.PindianFinish).ToArray();
        Require(finished.Select(m => m.CardId).SequenceEqual(reveals.Select(m => m.CardId)) &&
                finished.Zip(reveals).All(pair => pair.First.From == CardLocation.Processing &&
                    pair.First.To == CardLocation.DiscardPile && pair.First.Sequence > pair.Second.Sequence),
            "The original group discards each unclaimed physical card once in its reveal order.");
        ReplayFourViews(game, content);

        void ReachSetupOrPlay() => Reach(game, p => p.Kind == DecisionKind.SelectGeneral ||
            p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");

    private static T[] Events<T>(GameEngine game) where T : IGameEvent =>
        game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-gushe-cleanup", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, WangLang);
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "拼点参与者", "supporter", "standard:none", "wei"));
            builder.AddDeck(new(Deck, "固定平局牌面", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 96)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "鼓舌原拼点实体", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 1 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

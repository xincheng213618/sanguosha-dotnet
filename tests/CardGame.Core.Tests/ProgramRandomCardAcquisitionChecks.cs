using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ProgramRandomCardAcquisitionChecks
{
    public static void SharedRandomOwnerAndOrderedPhysicalAcquisitions()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 8 },
            StandardContentRegistry.Create());
        var matchRandom = (DeterministicRandom)typeof(GameEngine)
            .GetField("_random", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var beforeBinding = matchRandom.State;
        var match = (MatchRuntime)typeof(GameEngine)
            .GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var capabilities = match.Capabilities;
        var production = game.GetRuntimeModule<ProgramRandomCardAcquisitionRuntime>();
        var moduleRandom = typeof(ProgramRandomCardAcquisitionRuntime)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(field => field.FieldType == typeof(IMatchRandomSource)).GetValue(production);
        Require(ReferenceEquals(capabilities.Get<IMatchRandomSource>(), matchRandom) &&
                ReferenceEquals(moduleRandom, matchRandom) && matchRandom.State == beforeBinding,
            "Capability binding and module construction retain the match's exact generator without consuming a random step.");

        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var participants = capabilities.Get<IMatchParticipantStateAccess>();
        IReadOnlyList<CharacterState> players = capabilities.Get<IProgramRandomCardAcquisitionFacts>()
            .Players.Take(6).ToArray();
        var zones = new CardZoneStore(players.Count);
        zones.LoadInitialDeck([new(90, CardKind.Crossbow, Suit.Club, 1), new(50, CardKind.Dodge, Suit.Heart, 2),
            new(10, CardKind.Indulgence, Suit.Spade, 3), new(70, CardKind.Dodge, Suit.Club, 4),
            new(30, CardKind.Slash, Suit.Spade, 5), new(20, CardKind.Crossbow, Suit.Spade, 6),
            new(60, CardKind.Slash, Suit.Heart, 7), new(80, CardKind.Dodge, Suit.Heart, 8),
            new(40, CardKind.Slash, Suit.Club, 9)]);
        foreach (var (id, destination) in new (int, CardLocation)[]
                 { (90, CardLocation.Equipment(5)), (50, CardLocation.Hand(5)), (10, CardLocation.Judgment(5)),
                     (70, CardLocation.Hand(3)), (30, CardLocation.Hand(3)), (20, CardLocation.Equipment(1)),
                     (60, CardLocation.Hand(2)), (80, CardLocation.Hand(0)) })
            zones.Move(id, CardLocation.DrawPile, destination);

        var random = new DeterministicRandom(1);
        var ports = new AcquisitionPorts(zones, players, frames, random);
        var runtime = new ProgramRandomCardAcquisitionRuntime(frames, zones, participants,
            capabilities.Get<IProgramContentQueries>(), capabilities.Get<IProgramBoundInstructionFacts>(),
            capabilities.Get<IProgramBoundCardMovementOperations>(), ports, ports, ports, random);
        Require(random.State == 1 && ReferenceEquals(participants.At(3), players[3]),
            "The isolated shared operation starts with the known generator state and the original participant objects.");
        var originalTop = frames.Top;
        var wasAlive = players[2].IsAlive;
        var frame = new ProgramSkillFrame(frames.NextId(), 0, "fixture:random-acquisition", "run", "hash", 0, [], []);
        var reason = new CardMoveReason("fixture.random-acquisition");
        CardZoneKind[] declaredZones = [CardZoneKind.Equipment, CardZoneKind.Hand, CardZoneKind.Judgment];
        players[2].IsAlive = false;
        frames.Push(frame);
        try
        {
            runtime.TakeProgramRandomCardFromEveryOtherCharacter(frame.Id, 0, declaredZones, reason);
            Require(ports.Transfers.Select(call => (call.Card.Id, call.Source)).SequenceEqual(
                    [(10, CardLocation.Judgment(5)), (40, CardLocation.Hand(3)), (20, CardLocation.Equipment(1))]) &&
                    ports.Transfers.Select(call => call.RandomState).SequenceEqual([270369u, 67634689u, 2647435461u]) &&
                    random.State == 2647435461u,
                "The original table order visits 5, 3, 1, skipping the dead and empty seats, sorts physical candidates by ID and consumes exactly one shared random step per nonempty pool.");
            Require(ports.Transfers.All(call => call.RecipientSeat == 0 && call.FrameId == frame.Id && call.Reason == reason) &&
                    ports.Transfers.Select(call => call.Disclosure).SequenceEqual(
                        [CardMovementDisclosure.FaceUp, CardMovementDisclosure.FaceDown, CardMovementDisclosure.FaceUp]) &&
                    zones.CardsAt(CardLocation.Hand(0)).Select(card => card.Id).SequenceEqual([80, 10, 40]) &&
                    zones.GetLocation(20) == CardLocation.OutsideGame && zones.GetLocation(60) == CardLocation.Hand(2) &&
                    zones.GetLocation(70) == CardLocation.DiscardPile && zones.Count(CardLocation.Processing) == 0,
                "Native entries retain source, recipient and frame identity, hide hand cards, reveal public areas, read the later participant's changed physical area and preserve the destruction result.");
            Require(ports.Trace.SequenceEqual(["move:10", "producer:10", "gain:10", "refresh:3",
                    "move:40", "producer:40", "gain:40", "move:20", "producer:20", "destroy:20", "fact", "log"]) &&
                    ports.Facts is [ProgramRandomCardsTakenFromCharactersEvent fact] &&
                    fact.FrameId == frame.Id && fact.SkillId == frame.SkillId && fact.BindingId == frame.ActivationId &&
                    fact.OwnerSeat == 0 && fact.CardCount == 2 && fact.SourceSeats.SequenceEqual([5, 3]) &&
                    fact.Zones.SequenceEqual(declaredZones) &&
                    ports.Log == ("SkillEffect", $"{players[0].Name} 从 {players[5].Name}、{players[3].Name} 各随机获得一张牌。", 0),
                "Every original departure immediately reaches its producer hook before the gain; destroyed cards do not count, and the public completion fact precedes its unchanged log.");
            Require(ReferenceEquals(frames.Top, frame) && matchRandom.State == beforeBinding,
                "A synchronous acquisition retains its original frame and never advances a different match's generator.");
        }
        finally
        {
            frames.Pop(frame.Id, frame.Kind);
            players[2].IsAlive = wasAlive;
        }
        Require(ReferenceEquals(frames.Top, originalTop), "The shared mechanism check restores the original frame owner.");
    }

    private sealed record TransferCall(Card Card, CardLocation Source, int RecipientSeat, long FrameId,
        CardMoveReason Reason, CardMovementDisclosure Disclosure, uint RandomState);

    private sealed class AcquisitionPorts(CardZoneStore zones, IReadOnlyList<CharacterState> players,
        IProgramRuntimeFrames frames, DeterministicRandom random) : IProgramRandomCardAcquisitionFacts,
        IProgramRandomCardAcquisitionNativeEntries, IProgramRuntimeEvents
    {
        private readonly int[] _tableOrder = [0, 5, 2, 4, 3, 1];
        public IReadOnlyList<CharacterState> Players => players;
        internal List<TransferCall> Transfers { get; } = [];
        internal List<string> Trace { get; } = [];
        internal List<IGameEvent> Facts { get; } = [];
        internal (string Category, string Message, int OwnerSeat)? Log { get; private set; }

        public int RelativeTableOrder(int from, int target)
        {
            Require(from == 0, "The table query retains its original acquisition owner.");
            return Array.IndexOf(_tableOrder, target);
        }

        public void MoveToProcessingWithOwnedTransferReceipt(Card card, CardLocation source, CardMoveReason reason,
            int recipientSeat, long frameId, CardMovementDisclosure disclosure)
        {
            Require(frames.Top?.Id == frameId && ReferenceEquals(card, zones.CardsAt(source).Single(c => c.Id == card.Id)),
                "A native departure must receive the original physical card on its active program frame.");
            zones.Move(card.Id, source, CardLocation.Processing);
            Trace.Add($"move:{card.Id}");
            Transfers.Add(new(card, source, recipientSeat, frameId, reason, disclosure, random.State));
            Trace.Add($"producer:{card.Id}");
        }

        public bool MoveProcessingUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason)
        {
            Require(destination == CardLocation.Hand(0) && reason == Transfers[^1].Reason &&
                    ReferenceEquals(card, Transfers[^1].Card),
                "The second movement retains the paid card, recipient and reason from its immediately preceding native departure.");
            var destroyed = card.Id == 20;
            zones.Move(card.Id, CardLocation.Processing, destroyed ? CardLocation.OutsideGame : destination);
            Trace.Add($"{(destroyed ? "destroy" : "gain")}:{card.Id}");
            if (card.Id == 10)
            {
                zones.Move(70, CardLocation.Hand(3), CardLocation.DiscardPile);
                zones.Move(40, CardLocation.DrawPile, CardLocation.Hand(3));
                Trace.Add("refresh:3");
            }
            return !destroyed;
        }

        public void AdvanceFact(IGameEvent fact) { Facts.Add(fact); Trace.Add("fact"); }
        public void AddLog(string category, string message, int ownerSeat)
        { Log = (category, message, ownerSeat); Trace.Add("log"); }
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}

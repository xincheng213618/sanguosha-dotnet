using CardGame.Content.Standard;
using CardGame.Core;
using static GuanduCardChecks;

internal static class OlSpMaChaoZhuijiGuanduChariotChecks
{
    private const string MaChao = "ol:sp-ma-chao", Zhuiji = "ol:zhuiji", LiuYe = "guandu:liu-ye";
    private const string Mode = "fixture:guandu-zhuiji-chariot", Deck = "fixture:guandu-zhuiji-chariot-deck";

    internal static void ZhuijiDiscardsGuanduChariotAsItsRealDestroyedPaymentCold() => PayChariot(recast: false);

    internal static void ZhuijiRecastsGuanduChariotAsItsRealDestroyedPaymentCold() => PayChariot(recast: true);

    private static void PayChariot(bool recast)
    {
        var (game, content) = Start();
        var created = E<GuanduChariotCreatedEvent>(game).Single(fact => fact.OwnerSeat == 0);
        var chariot = V(game, 0).Equipment.Single(card => card.Id == created.CardId);
        Need(chariot is { Kind: CardKind.ThunderChariot, Suit: Suit.Diamond, Rank: 9 } &&
             game.CardMovements.Count(move => move.CardId == chariot.Id && move.From == CardLocation.OutsideGame && move.To == CardLocation.Equipment(0)) == 1,
            "The actual Guandu Liu Ye's opening Polu creates and installs its one native Thunder Chariot.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Until(game, () => P(game) is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Zhuiji });
        var offered = E<SlashTargetPenaltyOfferedEvent>(game).Single(fact => fact.Identity.TargetSeat == 0).Identity;
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == offered.CardUseFrameId);
        Need(offered is { ActorSeat: 1, TargetSeat: 0, FrozenDistance: 1, ActualTurnOwnerSeat: 1 } &&
             use is { SourceSeat: 1, CardKind: CardKind.Slash, CardId: > 0, Action: { Type: CardActionType.Use, ActorSeat: 1 } } &&
             use.TargetSeats.Contains(0) && game.CardMovements.Any(move => move.CardId == use.CardId && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) &&
             E<GuanduChariotDestroyedEvent>(game).All(fact => fact.CardId != chariot.Id),
            "The adjacent actual SP Ma Chao uses a paid native Slash on the real equipped owner, with the original Zhuiji choice still unpaid.");

        var active = P(game)!;
        Need(active.Choices.Any(choice => choice.Parameters.GetValueOrDefault("penalty-option") == "recast") &&
             active.Choices.Count(choice => choice.Parameters.GetValueOrDefault("penalty-option") == "discard" && choice.Cards.SequenceEqual([chariot.Id])) == 1,
            "The actual target can choose its generated chariot as a direct discard or use the independent all-equipment recast.");
        var checkpoint = GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint()));
        var restored = GameReplay.Restore(checkpoint, content);
        Need(restored.CardMovements.SequenceEqual(game.CardMovements) &&
             SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
            "Cold replay preserves the exact generated card, original attacker and private target payment choice.");

        Answer(restored, choice => recast ? choice.Parameters.GetValueOrDefault("penalty-option") == "recast" :
            choice.Parameters.GetValueOrDefault("penalty-option") == "discard" && choice.Cards.SequenceEqual([chariot.Id]));
        Until(restored, () => E<CardUseFinishedEvent>(restored).Any(fact => fact.ResolutionId == use.Id));

        var paid = E<SlashTargetPenaltyPaidEvent>(restored).Single(fact => fact.CardUseFrameId == use.Id && fact.TargetSeat == 0);
        var departure = restored.CardMovements.Single(move => move.CardId == chariot.Id && move.From == CardLocation.Equipment(0));
        Need(paid.Recast == recast && paid.CardIds.SequenceEqual([chariot.Id]) && paid.ActorSeat == 1 &&
             departure.To == CardLocation.OutsideGame && departure.Sequence > paid.SequenceBefore && departure.Sequence <= paid.SequenceAfter &&
             E<GuanduChariotDestroyedEvent>(restored).Count(fact => fact.CardId == chariot.Id && fact.MovementSequence == departure.Sequence) == 1 &&
             restored.CardMovements.All(move => move.CardId != chariot.Id || move.To != CardLocation.DiscardPile && move.To != CardLocation.Processing),
            "The accepted payment keeps the native one-time Equipment-to-Outside destruction, without a fabricated discard entry.");
        var finish = E<SlashTargetPenaltyFinishedEvent>(restored).Single(fact => fact.CardUseFrameId == use.Id && fact.TargetSeat == 0);
        var draw = E<SlashTargetPenaltyDrawIssuedEvent>(restored).Where(fact => fact.ProgramFrameId == paid.ProgramFrameId).ToArray();
        Need(finish.Completed && finish.Recast == recast && finish.ActualDrawCount == (recast ? 1 : 0) &&
             (recast ? departure.Reason == CardMoveReasons.RecastDiscard && draw is [{ RequestedCount: 1, ActualCount: 1, TargetSeat: 0 }]
                 : departure.Reason.Value == "skill-program.ol:zhuiji.RequireTargetDiscardOrEquipmentRecast" && draw.Length == 0) &&
             E<CardUseFinishedEvent>(restored).Count(fact => fact.ResolutionId == use.Id) == 1,
            "The recast draws once when chosen, the direct discard draws nothing, and each returns the same original Slash once.");
        Cold(restored, content);
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardGuanduActivityPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 8, HumanSeat = 0, HumanRole = null, HumanTeamId = "team:cao", HumanGuanduRole = GuanduSeatRole.Commander,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 10
        }, content);
        Accept(game, new StartGameCommand());
        Until(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, LiuYe, game.Revision, P(game)!.PromptId));
        Play(game);
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-guandu-zhuiji-chariot", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var commanders = new Dictionary<string, string> { ["team:cao"] = LiuYe, ["team:yuan"] = MaChao };
            var pools = new Dictionary<string, IReadOnlyList<string>>();
            foreach (var (team, commander) in commanders)
            {
                var faction = team == "team:cao" ? "wei" : "qun";
                var peers = Enumerable.Range(1, 9).Select(index => $"fixture:guandu-zhuiji-{faction}-{index}").ToArray();
                foreach (var peer in peers)
                    builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", faction, 8));
                pools[team] = [commander, ..peers];
            }

            builder.AddDeck(new(Deck, "Fixed physical Slashes", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 100).Select(index => new ContentDeckPhysicalCard("standard:slash", Suit.Club, index % 13 + 1)).ToArray()
            });
            IReadOnlyList<string> layout = ["team:cao", "team:yuan", "team:yuan", "team:yuan", "team:yuan", "team:cao", "team:cao", "team:cao"];
            builder.AddMode(new(Mode, "Guandu Zhuiji destroyed payment", 8, 8, new Dictionary<string, int>(), Deck, 6,
                pools.Values.SelectMany(pool => pool).ToArray(), ContentModeKind.Team,
                new Dictionary<string, int> { ["team:cao"] = 4, ["team:yuan"] = 4 })
            {
                GuanduActivity = new(pools, commanders, [GuanduBattleEvent.Underdog],
                    Enumerable.Repeat(layout, 5).ToArray(), PlayerTier: 1)
            });
        }
    }
}

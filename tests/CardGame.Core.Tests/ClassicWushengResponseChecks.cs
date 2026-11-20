using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicWushengResponseChecks
{
    private const string Hero = "classic:guan-yu", Skill = "classic:wusheng", Binding = "red-owned-as-slash";
    private const string Mode = "identity:classic-wusheng-response", Deck = "fixture:classic-wusheng-deck";
    private static readonly string[] Generals = [Hero, "fixture:classic-wusheng-peer-1", "fixture:classic-wusheng-peer-2", "fixture:classic-wusheng-peer-3"];

    internal static void RedEquipmentKeepsItsClassicSourceInPlayAndDuelCold()
    {
        var (game, content) = Start();
        var owner = game.CreateSnapshot(0).Players[0];
        Require(owner.GeneralId == Hero && owner.Hand.Count == 3 &&
            owner.Hand.Any(card => card is { Kind: CardKind.Crossbow, Suit: Suit.Heart }) &&
            owner.Hand.Any(card => card is { Kind: CardKind.BaguaFormation, Suit: Suit.Spade }),
            "The fixed opening deal gives classic Guan Yu one red weapon and one black armor.");
        var weapon = owner.Hand.Single(card => card.Kind == CardKind.Crossbow).Id;
        var armor = owner.Hand.Single(card => card.Kind == CardKind.BaguaFormation).Id;
        Equip(game, weapon);
        Equip(game, armor);

        var play = Cold(game, content);
        var action = play.GetHumanLegalActions().Single(item => item.Kind == LegalActionKind.Slash &&
            item.CardId == weapon && item.TargetSeats.SequenceEqual([1]));
        Require(action.PlayedCardKind == CardKind.Slash && action.ConversionSource is
                { SkillId: Skill, BindingId: Binding, OwnerSeat: 0 } &&
            !play.GetHumanLegalActions().Any(item => item.Kind == LegalActionKind.Slash && item.CardId == armor),
            "The formal red-equipment declaration publishes its exact Wusheng source and excludes the black armor.");
        Accept(play, new PlayCardCommand(0, weapon, action.TargetSeats, play.Revision, P(play)!.PromptId, action.PlayedCardKind)
            { ConversionSource = action.ConversionSource });
        ReachPlay(play, 0);
        AssertPayment(play, weapon, CardMoveReasons.Use, CardMoveReasons.UseFinished);
        Require(Facts<CardUsedEvent>(play).Count(entry => entry.CardId == weapon && entry is
            { CardKind: CardKind.Slash, SourceSeat: 0, TargetSeat: 1 }) == 1,
            "The declared Slash uses the one physical equipped weapon through its native Use path.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        ReachPlay(game, 1);
        var duelCard = game.CreateSnapshot(1).Players[1].Hand.First(card => card.Kind == CardKind.Duel).Id;
        Accept(game, new PlayCardCommand(1, duelCard, [0], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.RespondSlash, PlayerSeat: 0, IncomingCard: CardKind.Duel });
        var duel = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == duelCard);
        game = Cold(game, content);
        var response = P(game)!;
        var choice = response.Choices.Single(item => item.Cards.SequenceEqual([weapon]));
        Require(game.CreateSnapshot(1).PendingDecision is null && !response.ValidCardIds.Contains(armor) &&
            choice.Parameters.GetValueOrDefault("response-card-kind") == nameof(CardKind.Slash) &&
            choice.Parameters.GetValueOrDefault("conversion-skill-id") == Skill &&
            choice.Parameters.GetValueOrDefault("conversion-binding-id") == Binding &&
            choice.Parameters.GetValueOrDefault("conversion-owner-seat") == "0" &&
            choice.Parameters.GetValueOrDefault("conversion-instance-id") == action.ConversionSource!.SkillInstanceId,
            "The private real Duel response publishes the same original red equipment and exact source, with black armor excluded.");
        Accept(game, new AnswerPromptCommand(0, response.PromptId, choice.Id, game.Revision));
        ReachPlay(game, 1);
        AssertPayment(game, weapon, CardMoveReasons.Respond, CardMoveReasons.ResponseFinished);
        Require(Facts<CardRespondedEvent>(game).Count(entry => entry.CardId == weapon && entry.ResponderSeat == 0 &&
                entry.EffectiveCardKind == CardKind.Slash) == 1 &&
            Facts<DuelResponseEvent>(game).Count(entry => entry.ResolutionId == duel.Id && entry.ResponderSeat == 0 &&
                entry.UsedSlash && entry.SlashCardId == weapon) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(entry => entry.ResolutionId == duel.Id) == 1,
            "The exact Wusheng response pays once and returns to its original native Duel.");
        _ = Cold(game, content);
    }

    private static void Equip(GameEngine game, int card)
    {
        Accept(game, new PlayCardCommand(0, card, [], game.Revision, P(game)!.PromptId));
        ReachPlay(game, 0);
    }

    private static void AssertPayment(GameEngine game, int weapon, CardMoveReason use, CardMoveReason finish)
    {
        var installed = game.CardMovements.Single(move => move.CardId == weapon && move.To == CardLocation.Equipment(0));
        var moves = game.CardMovements.Where(move => move.CardId == weapon && move.Sequence > installed.Sequence).ToArray();
        Require(moves is [var payment, var cleanup] && payment.CardKind == CardKind.Crossbow &&
            payment.From == CardLocation.Equipment(0) && payment.To == CardLocation.Processing && payment.Reason == use &&
            cleanup.From == CardLocation.Processing && cleanup.To == CardLocation.DiscardPile && cleanup.Reason == finish,
            "Wusheng pays and clears the equipped entity once with the native movement reasons of the accepted action.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static void ReachPlay(GameEngine game, int seat) => Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == seat);

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay keeps the exact physical equipment and each seat's published choices.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachOpening();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachOpening();
        }
        return (game, content);

        void ReachOpening() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral ||
            prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-wusheng-response", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var general in Generals.Skip(1))
                builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "武圣与决斗固定牌堆", 3, 0, [])
            {
                PhysicalCards = Enumerable.Range(1, 12).Select(rank => rank switch
                {
                    6 => new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, rank),
                    11 => new ContentDeckPhysicalCard("standard:bagua", Suit.Spade, rank),
                    _ => new ContentDeckPhysicalCard("standard:duel", Suit.Club, rank)
                }).ToArray()
            });
            builder.AddMode(new(Mode, "经典武圣的装备使用与响应", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

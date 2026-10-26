using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheMiaoshouChecks
{
    private const string Owner = "fixture:miaoshou-owner", Deck = "fixture:miaoshou-deck";
    private static string Peer(int seat) => "fixture:miaoshou-peer-" + seat;

    public static void RealSnatchReachesDistantHandWithoutExtendingSlash()
    {
        foreach (var enabled in new[] { false, true })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMiaoshouPackage(), new Fixture());
            var setup = new ShanheBattleSetup(23, 1, "fixture:miaoshou", Owner, 20, 0, 0, 0, 0, [],
                [new(CardKind.Snatch, Suit.Spade, 3), new(CardKind.Slash, Suit.Spade, 4)], [],
                Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition(Peer(seat))
                    { StartingHand = seat == 2 ? [new(CardKind.Peach, Suit.Heart, 5)] : [] }).ToArray(), [], new string('c', 64))
                { TacticSkillIds = enabled ? [StandardShanheMiaoshouPackage.SkillId] : [] };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            Accept(game, new StartGameCommand()); Play(game);
            var actions = game.GetHumanLegalActions();
            var snatch = actions.FirstOrDefault(a => a.CardId is { } id &&
                game.CreateSnapshot(0).Players[0].Hand.Any(c => c.Id == id && c.Kind == CardKind.Snatch) &&
                a.TargetSeats.SequenceEqual([2]));
            Need((snatch is not null) == enabled && !actions.Any(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.Contains(2)),
                "At actual four-seat distance two, Miaoshou permits Snatch's real hand target while leaving Slash's native range unchanged.");
            if (snatch is null) continue;
            var original = snatch.CardId!.Value;
            var takenId = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
            Accept(game, new PlayCardCommand(0, original, snatch.TargetSeats, game.Revision,
                game.PendingDecision!.PromptId, snatch.PlayedCardKind, snatch.TargetCardId)
                { ConversionSource = snatch.ConversionSource, AdditionalConversionSources = snatch.AdditionalConversionSources });
            Play(game);
            var takenMoves = game.CardMovements.Where(m => m.CardId == takenId &&
                (m.Reason == CardMoveReasons.Snatch || m.Reason == CardMoveReasons.SnatchFinished)).ToArray();
            Need(game.CreateSnapshot(0).Players[0].Hand.Count(c => c.Id == takenId && c.Kind == CardKind.Peach) == 1 &&
                game.CreateSnapshot(2).Players[2].Hand.Count == 0 &&
                takenMoves is [var removed, var received] &&
                removed.From == CardLocation.Hand(2) && removed.To == CardLocation.Processing && removed.Reason == CardMoveReasons.Snatch &&
                received.From == CardLocation.Processing && received.To == CardLocation.Hand(0) && received.Reason == CardMoveReasons.SnatchFinished &&
                received.Sequence == removed.Sequence + 1 &&
                game.Events.Count(e => e.Payload is CardUseDeclaredEvent use && use.CardId == original) == 1 &&
                game.CardMovements.Count(m => m.CardId == original && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                game.CardMovements.Count(m => m.CardId == original && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
                "The selected distant hand card moves once into the real owner's hand, with one ordinary Snatch use and one physical-card cleanup. Actual: " +
                JsonSerializer.Serialize(new
                {
                    Original = original,
                    Taken = takenId, TakenMoves = takenMoves,
                    OwnerHand = game.CreateSnapshot(0).Players[0].Hand,
                    TargetHand = game.CreateSnapshot(2).Players[2].Hand,
                    Movements = game.CardMovements.Where(m => m.CardId == original || m.From == CardLocation.Hand(2) || m.To == CardLocation.Hand(0)).ToArray(),
                    Declarations = game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>().ToArray(),
                    AcceptedActions = game.Events.Select(e => e.Payload).OfType<CardActionAcceptedEvent>().Select(e => e.Action).ToArray(),
                    Appearances = game.Events.Select(e => e.Payload).OfType<CardUseAppearanceCapturedEvent>().Select(e => e.Action).ToArray(),
                    Finished = game.Events.Select(e => e.Payload).OfType<CardUseFinishedEvent>().ToArray()
                }));
        }
    }

    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Miaoshou native command failed."); }
    private static void Play(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            if (game.PendingDecision is { PlayerSeat: 0 } prompt)
            {
                var choice = prompt.Kind == DecisionKind.SelectTargetCard ? prompt.Choices.FirstOrDefault()
                    : prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                Need(choice is not null, "Unexpected distant Snatch decision: " + JsonSerializer.Serialize(prompt));
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, game.Revision));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Distant Snatch did not return to its original Play frame.");
    }
    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-miaoshou", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "妙手使用者", "supporter", "standard:none", "wu", 20));
            foreach (var seat in Enumerable.Range(1, 3))
                builder.AddGeneral(new(Peer(seat), "守军", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "固定无普通摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "妙手距离小局", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}

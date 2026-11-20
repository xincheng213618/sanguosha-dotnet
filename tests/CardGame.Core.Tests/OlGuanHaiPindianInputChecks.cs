using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlGuanHaiPindianInputChecks
{
    private const string Hero = "ol:guan-hai", Xiewei = "ol:xiewei", Youque = "ol:youque";
    private const string Contest = "minimum-bait-pindian", Deck = "fixture:youque-input-deck";
    private const string Enemy = "fixture:youque-input-enemy", Ally = "fixture:youque-input-ally", Empty = "fixture:youque-input-empty";

    internal static void RealBaitNeedsPindianInputAndKeepsItsOriginalEntityCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "youque-input", Hero, 2, 0, 0, 0, 0, [],
            [new(CardKind.Crossbow, Suit.Spade, 5), new(CardKind.Crossbow, Suit.Club, 9)], [],
            [new(Ally, Hp: 10) { StartingHand = [] }, new(Empty, Hp: 10) { StartingHand = [] }],
            [new(Enemy, Hp: 10) { StartingHand = [new(CardKind.Crossbow, Suit.Club, 5)] }],
            new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        GameEngine Restore(GameEngine source) =>
            GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(source.CreateCheckpoint())), registry);
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The published Guan Hai command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 80 && !ready(); step++)
            {
                Need(Prompt() is null && game.State.Winner == Winner.None, "The fixed scene reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The fixed scene did not reach its actual choice.");
        }
        void Play() => Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0);

        Submit(new StartGameCommand());
        Play();
        var low = Owner().Hand.Single(card => card.Kind == CardKind.Crossbow && card.Rank == 5).Id;
        var high = Owner().Hand.Single(card => card.Kind == CardKind.Crossbow && card.Rank == 9).Id;
        var xiewei = game.GetHumanLegalActions().First(action => action.SkillId == Xiewei &&
            action.PlayedCardKind == CardKind.Slash && action.SelectableCardIds.Order().SequenceEqual(new[] { low, high }.Order()));
        Submit(new UseProgramSkillCommand(0, Xiewei, xiewei.ProgramActivationId!, [low, high], [1], game.Revision, Prompt()!.PromptId));
        Play();
        var baits = Facts<XieweiBaitStoredEvent>();
        Need(baits.Select(fact => fact.CardId).Order().SequenceEqual(new[] { low, high }.Order()) &&
            baits.Select(fact => fact.BatchId).Distinct().Count() == 1 &&
            Owner().Hand is [{ Kind: CardKind.Peach }], "Real Xiewei stores two physical baits and leaves one ordinary hand input.");

        var withHand = game;
        game = Restore(withHand);
        var peach = game.GetHumanLegalActions().First(action => action.CardId == Owner().Hand.Single().Id);
        Submit(new PlayCardCommand(0, peach.CardId!.Value, peach.TargetSeats, game.Revision, Prompt()!.PromptId,
            peach.PlayedCardKind, peach.TargetCardId));
        Play();
        var revision = game.Revision;
        var rejected = game.Submit(new UseProgramSkillCommand(0, Youque, Contest, [], [1], revision, Prompt()!.PromptId));
        Need(Owner().HandCount == 0 && baits.All(bait => game.CreateCardZoneDiagnostics().Any(card =>
                card.CardId == bait.CardId && card.Location == bait.Pile)) &&
            !game.GetHumanLegalActions().Any(action => action.SkillId == Youque) && !rejected.Accepted && game.Revision == revision,
            "The exact baits remain available in the pile. An empty hand still fails both Youque advertisement and submission.");

        game = withHand;
        Submit(new UseProgramSkillCommand(0, Youque, Contest, [], [1], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { PlayerSeat: 0, SkillPrompt.SkillId: Youque } &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.GuanHai?.Stage == GuanHaiStage.ChooseBait));
        var choice = Prompt()!.Choices.Single(item => item.Cards.SequenceEqual([low]));
        Need(Prompt()!.Choices.All(item => !item.Cards.Contains(high)), "The original higher bait is ineligible.");
        var beforeChoice = game;
        game = Restore(beforeChoice);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(beforeChoice.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(beforeChoice.CardMovements), "Cold replay retains all views and the actual bait pair.");
        Submit(new AnswerPromptCommand(0, Prompt()!.PromptId, choice.Id, game.Revision));
        Reach(() => Facts<PindianResultDeterminedEvent>().Length == 1);
        var issued = Facts<YouquePindianIssuedEvent>().Single();
        var result = Facts<PindianResultDeterminedEvent>().Single();
        Need(issued.CardId == low && issued.OpponentSeat == 1 &&
            result.FrameId == issued.PindianId && result.Result is { SourceRank: 5, OpponentRank: 5 } &&
            result.Result.SourceCardId == low && game.CardMovements.Count(move => move.Sequence > issued.Before && move.CardId == low &&
                move.From == issued.Pile.Location && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.PindianReveal) == 1 &&
            !game.CardMovements.Any(move => move.Sequence > issued.Before && move.CardId == low && move.To == CardLocation.Hand(0)),
            "The same lowest bait enters the native tied contest directly from its original pile.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-youque-input", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var id in new[] { Enemy, Ally, Empty })
                builder.AddGeneral(new(id, "固定同桌", "supporter", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "诱阙固定牌堆", 0, 1, [new("standard:peach", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "诱阙原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy, Ally, Empty], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

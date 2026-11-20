using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class PangJiBiguoOpponentChecks
{
    private const string Hero = "ol:pang-ji", Biguo = "ol:biguo", Deck = "fixture:biguo-opponent-deck";
    private const string Peer = "fixture:biguo-opponent-peer";

    internal static void ActualBiguoSelectsNativeOpponentAndCardCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "biguo-opponent", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.IronChain, Suit.Club, 7), new(CardKind.Slash, Suit.Spade, 13)], [],
            [new(Peer, Hp: 10) { StartingHand = [new(CardKind.Crossbow, Suit.Club, 5)] },
             new(Peer, Hp: 10) { StartingHand = [] }],
            [new(Peer, Hp: 10) { StartingHand = [new(CardKind.Crossbow, Suit.Club, 9)] }],
            new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T Fact<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().Single();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Biguo fixture command was rejected.");
        }
        void Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80 && Prompt() is null && game.State.Winner == Winner.None; step++)
                Submit(new AdvanceOneStepCommand(game.Revision));
            Need(Prompt() is { } prompt && ready(prompt), "The fixed scene did not reach the expected choice.");
        }
        void Answer(Func<PromptChoice, bool> choose)
        {
            var prompt = Prompt()!;
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(choose).Id, game.Revision));
        }
        void ActivateBiguo()
        {
            Reach(prompt => prompt.SkillPrompt?.SkillId == Biguo &&
                prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        }

        Submit(new StartGameCommand());
        ActivateBiguo();
        Reach(prompt => prompt.SkillPrompt?.SkillId == Biguo &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("pang-ji") == "cost-card"));
        var cost = Owner().Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        Answer(choice => choice.Parameters.GetValueOrDefault("pang-ji") == "cost-card" && choice.Cards.SequenceEqual([cost]));
        Reach(prompt => prompt.Kind == DecisionKind.PlayCard);
        var paid = Fact<BiguoCostPaidEvent>();
        var arm = Fact<BiguoArmedEvent>();
        Need(paid.FrameId == arm.FrameId && paid.CardId == cost && paid.From == CardLocation.Hand(0) &&
            paid.To == CardLocation.DiscardPile && arm is { TurnOwner: 0 } && arm.ActualTurn == game.State.TurnNumber,
            "The actual Draw-end discard arms Biguo on the original turn.");

        var chain = game.GetHumanLegalActions().Single(action =>
            action.Kind == LegalActionKind.IronChain && action.TargetSeats.SequenceEqual([0]));
        var chainId = chain.CardId!.Value;
        Submit(new PlayCardCommand(0, chainId, chain.TargetSeats, game.Revision, Prompt()!.PromptId));
        ActivateBiguo();
        Reach(prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-start"));

        var issued = Fact<BiguoPindianIssuedEvent>();
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == issued.UseId);
        var sourceCard = Owner().Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var menu = Prompt()!;
        Need(issued.ArmFrameId == arm.FrameId && use.Action?.TargetSeats.SequenceEqual([0]) == true &&
            pair is { PindianStep: PindianStep.ChooseParticipants, SourceSeat: 0, OpponentSeat: null, SourceCardId: null } &&
            pair.Id == issued.PindianId && pair.ParentFrameId == issued.FrameId &&
            pair.ParentProcessingCardIds?.Contains(chainId) == true &&
            menu is { PlayerSeat: 0, IsPrivate: true } && menu.ValidTargetSeats.SequenceEqual([1, 2]) &&
            game.CreateSnapshot(0).Players[3].HandCount == 0,
            "The armed original use opens the private native pair menu with only the two peers who have a card.");
        var choice = menu.Choices.Single(item => item.Parameters.GetValueOrDefault("action") == "pindian-start" &&
            item.Cards.SequenceEqual([sourceCard]) && item.Targets.SequenceEqual([2]));
        Need(menu.Choices.All(item => !item.Targets.Contains(3)), "No source card may be paired with the empty peer.");

        var beforeRestore = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(beforeRestore.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(beforeRestore.CardMovements) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<PindianFrame>().Single()) == JsonSerializer.Serialize(pair),
            "Cold replay preserves the ordered private candidate menu, original use payment and uncommitted pair.");

        Answer(item => item.Id == choice.Id);
        var committed = game.ResolutionStack.OfType<PindianFrame>().Single();
        var peer = game.CreateSnapshot(2);
        Need(committed.Id == pair.Id && committed.ParentFrameId == issued.FrameId &&
            committed is { PindianStep: PindianStep.ChooseOpponentCard, OpponentSeat: 2 } &&
            committed.SourceCardId == sourceCard && peer.PendingDecision is { PlayerSeat: 2, IsPrivate: true, Choices: [var offered] } &&
            offered.Parameters.GetValueOrDefault("action") == "pindian-card" &&
            offered.Cards.SequenceEqual([peer.Players[2].Hand.Single().Id]),
            "The two published choices commit the exact source card and selected peer on the original native pair.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-biguo-opponent", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "愎果固定牌堆", 0, 1, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "愎果原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

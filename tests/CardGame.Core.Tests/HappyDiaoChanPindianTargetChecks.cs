using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class HappyDiaoChanPindianTargetChecks
{
    private const string Hero = "happy:shen-diao-chan", Huoxin = "happy:huoxin", LuKang = "classic:lu-kang";
    private const string Peer = "fixture:huoxin-peer", Deck = "fixture:huoxin-pindian-deck";

    internal static void PaidHuoxinKeepsOrderedPrivatePairCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(37, 1, "happy-huoxin-pindian", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Club, 2)], [],
            [new(LuKang) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 5)] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 5)] }], new string('c', 64))
            { DefaultGodFactionId = "qun" };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, content);

        Accept(game, new StartGameCommand());
        Settle(game);
        var cost = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        var sourceCard = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
        var opponentCard = game.CreateSnapshot(1).Players[1].Hand.Single().Id;
        Accept(game, new UseProgramSkillCommand(0, Huoxin, "charm-contest", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, _ => AtStage(HappyDiaoChanStage.Cost));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("mode") == "cost" && choice.Cards.SequenceEqual([cost]));
        Reach(game, _ => AtStage(HappyDiaoChanStage.Pair));
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.HappyDiaoChan is not null);
        var paid = Facts<HappyDiaoChanCostPaidEvent>().Single();
        var discard = game.CardMovements.Single(move => move.Sequence == paid.Sequence);
        Require(paid.FrameId == root.Id && paid.CardId == cost && discard.CardId == cost &&
            discard.From == CardLocation.Hand(0) && discard.To == CardLocation.DiscardPile &&
            game.CreateSnapshot(0).Players[0].HandCount == 0 && P(game)!.Choices.Single().Targets.SequenceEqual([2, 1]),
            "Huoxin pays its only hand card and offers the single ordered pair with Qianjie only as the source.");

        Answer(game, choice => choice.Targets.SequenceEqual([2, 1]));
        var issued = Facts<HappyDiaoChanPindianIssuedEvent>().Single();
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var sourcePrompt = game.CreateSnapshot(2).PendingDecision;
        Require(issued == new HappyDiaoChanPindianIssuedEvent(root.Id, pair.Id, 2, 1) &&
            pair is { SourceSeat: 2, OpponentSeat: 1, PindianStep: PindianStep.ChooseSourceCard } && pair.ParentFrameId == root.Id &&
            sourcePrompt is { PlayerSeat: 2, IsPrivate: true } && sourcePrompt.Choices.Single().Cards.SequenceEqual([sourceCard]) &&
            game.CreateSnapshot(0).PendingDecision is null && game.CreateSnapshot(1).PendingDecision is null &&
            !game.CardMovements.Any(move => move.Reason == CardMoveReasons.PindianReveal),
            "The real ordered pair opens a private, unrevealed native choice for its first participant.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(game.ResolutionStack.OfType<PindianFrame>().Single().Id == pair.Id &&
            game.CreateSnapshot(2).PendingDecision?.PromptId == sourcePrompt!.PromptId && game.CreateSnapshot(0).PendingDecision is null,
            "Cold restore keeps the original pair and private source prompt.");
        Reach(game, _ => AtStage(HappyDiaoChanStage.HuoxinSuit));
        var result = Facts<PindianResultDeterminedEvent>().Single();
        Require(result.FrameId == pair.Id && result.Result is { SourceSeat: 2, OpponentSeat: 1, SourceRank: 5, OpponentRank: 5 } &&
            result.Result.SourceCardId == sourceCard && result.Result.OpponentCardId == opponentCard &&
            Facts<HappyDiaoChanPindianReturnedEvent>().Single() == new HappyDiaoChanPindianReturnedEvent(root.Id, pair.Id, 2, 1, 5, 5),
            "Both original physical cards yield a real tie and return to the same Huoxin activation.");

        bool AtStage(HappyDiaoChanStage stage) => game.ResolutionStack.LastOrDefault() is ProgramSkillFrame
            { HappyDiaoChan: { Stage: var current } } && current == stage;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huoxin-pindian-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, LuKang);
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "惑心固定牌堆", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "惑心原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, LuKang, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

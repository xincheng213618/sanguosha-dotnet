using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class AnniversaryLiuLiFuliTurnChecks
{
    private const string Hero = "anniversary:liu-li", Fuli = "anniversary:fuli", Binding = "reveal-discard-draw";
    private const string Owner = "fixture:fuli-owner", Peer = "fixture:fuli-peer", Deck = "fixture:fuli-deck";

    internal static void PaidRangeSurvivesRecipientTurnAndExpiresAtSourceTurnCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(43, 1, "fuli-range-turn", Owner, 3, 0, 0, 0, 0, [],
            [new(CardKind.Dismantlement, Suit.Spade, 3)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer)
            {
                StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Diamond, 3)],
                StartingEquipment = [new(CardKind.QinggangSword, Suit.Spade, 6)]
            }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, content);

        Accept(game, new StartGameCommand());
        Settle(game);
        const int recipient = 1;
        var card = game.CreateSnapshot(0).Players[0].Hand.Single();
        var ordinaryRange = game.GetAttackRange(recipient);
        Require(card.Kind == CardKind.Dismantlement && ordinaryRange == 2,
            "The real Fuli source has one nondamage trick and the original recipient has its weapon range.");
        Accept(game, new UseProgramSkillCommand(0, Fuli, Binding, [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Fuli &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "category-1"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "category-1" && choice.Cards.SequenceEqual([card.Id]));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Fuli && prompt.Choices.Any(choice => choice.Targets.Contains(recipient)));
        var started = Facts<AnniversaryLiuLiStartedEvent>().Single();
        var paid = Facts<AnniversaryLiuLiDiscardPaidEvent>().Single();
        var draw = Facts<AnniversaryLiuLiDrawIssuedEvent>().Single();
        var discard = game.CardMovements.Single(move => move.CardId == card.Id && move.Reason.Value == "skill-program.anniversary:fuli.discard");
        Require(started.Source is { OwnerSeat: 0, SkillId: Fuli, BindingId: Binding } &&
            paid is { Category: 1, Count: 1, DamageDiscarded: false } && paid.FrameId == started.FrameId &&
            discard.From == CardLocation.Hand(0) && discard.To == CardLocation.DiscardPile &&
            draw.FrameId == started.FrameId && draw.Count == 2 && game.CreateSnapshot(0).Players[0].HandCount == draw.Count,
            "Fuli pays the complete physical trick category and returns the draw before issuing an attack-range effect.");
        Answer(game, choice => choice.Targets.SequenceEqual([recipient]));
        Settle(game);
        var issued = Facts<AnniversaryLiuLiRangeIssuedEvent>().Single();
        Require(issued is { TargetSeat: recipient, OwnerTurnCount: 1, SetZero: false } && issued.FrameId == paid.FrameId &&
            issued.Source == started.Source && issued.Hash == started.Hash && game.GetAttackRange(recipient) == ordinaryRange - 1,
            "The original paid source issues minus one range against the selected recipient on its first actual turn.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Facts<AnniversaryLiuLiRangeIssuedEvent>().Single() == issued && game.GetAttackRange(recipient) == ordinaryRange - 1,
            "Cold restore retains the issued effect and its original source turn count.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        ReachTurnStart(recipient, 1);
        Require(Facts<TurnStartedEvent>().Last().ActorSeat == recipient &&
            Facts<TurnStartedEvent>().Count(turn => turn.ActorSeat == 0) == 1 && game.GetAttackRange(recipient) == ordinaryRange - 1,
            "The recipient's first actual TurnStarted still leaves the original range reduction active.");
        ReachTurnStart(0, 2);
        Require(Facts<TurnStartedEvent>().Last().ActorSeat == 0 && game.GetAttackRange(recipient) == ordinaryRange &&
            Facts<AnniversaryLiuLiRangeIssuedEvent>().Single() == issued && Facts<AnniversaryLiuLiDiscardPaidEvent>().Single() == paid,
            "The source's next actual TurnStarted restores the original attack range without issuing or paying Fuli again.");

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

        void ReachTurnStart(int seat, int count)
        {
            for (var step = 0; step < 100 && Facts<TurnStartedEvent>().Count(turn => turn.ActorSeat == seat) < count; step++)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            Require(Facts<TurnStartedEvent>().Count(turn => turn.ActorSeat == seat) == count,
                "The fixed scene must reach the expected actual turn start.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-fuli-turn", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Owner, "抚黎验证", "", Fuli, "shu", 3));
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "抚黎固定牌堆", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "抚黎原回合期限", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;
using static BoundaryLiDianChecks;

internal static class WuLuXunRuxianTurnStartChecks
{
    private const string Hero = "wumiao:lu-xun", Ruxian = "wumiao:ruxian", Zhangcai = "wumiao:zhangcai";
    private const string Peer = "fixture:ruxian-quiet-peer", Deck = "fixture:ruxian-turn-start-deck";
    private const string ZhangcaiDraw = "skill-program.wu-lu-xun.zhangcai.draw";

    internal static void IssuedPermissionExpiresAtNextOwnActualStart()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(17, 1, "ruxian-turn-start", Hero, 12, 9, 0, 0, 0, [],
            [new(CardKind.IronChain, Suit.Club, 7), new(CardKind.IronChain, Suit.Spade, 7)], [],
            [new(Peer, Hp: 12) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, content);

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        bool AtPlay() => Prompt()?.Kind == DecisionKind.PlayCard;
        void Answer(PendingDecision prompt, string action) => Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == action).Id, game.Revision));
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120 && !ready() && game.State.Winner == Winner.None; step++)
            {
                if (Prompt() is not { } prompt)
                    Accept(game, new AdvanceOneStepCommand(game.Revision));
                else if (prompt.SkillPrompt?.SkillId == "wumiao:xiongmu")
                    Answer(prompt, "skip");
                else
                    throw new InvalidOperationException($"Unexpected Ruxian choice {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
            }
            Require(ready(), $"Missing Ruxian boundary at turn {game.State.TurnNumber}, phase {game.State.Phase}.");
        }
        void UseIronChain(int cardId)
        {
            var use = game.GetHumanLegalActions().Single(action => action is { Kind: LegalActionKind.IronChain, ConversionSource: null } &&
                action.CardId == cardId && action.TargetSeats.SequenceEqual([0]));
            Accept(game, new PlayCardCommand(0, cardId, use.TargetSeats, game.Revision, Prompt()!.PromptId, use.PlayedCardKind, use.TargetCardId));
        }

        Accept(game, new StartGameCommand());
        Reach(AtPlay);
        var cards = game.CreateSnapshot(0).Players[0].Hand.Where(card => card is { Kind: CardKind.IronChain, Rank: 7 }).ToArray();
        Require(cards.Length == 2, "Both original rank-seven Iron Chains are available before paying either Use.");
        Accept(game, new UseProgramSkillCommand(0, Ruxian, "all-ranks-until-next-turn", [], [], game.Revision, Prompt()!.PromptId));
        Reach(AtPlay);
        var issued = Facts<WuLuXunRuxianActivatedEvent>().Single();
        var firstStart = Facts<TurnStartedEvent>().Single();
        Require(firstStart.ActorSeat == 0 && issued.ActualTurn == firstStart.TurnNumber && issued.Source.OwnerSeat == 0,
            "The real Ruxian activation is issued on the owner's original actual turn.");

        UseIronChain(cards[0].Id);
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zhangcai);
        Answer(Prompt()!, "activate");
        Reach(AtPlay);
        var drawn = Facts<WuLuXunZhangcaiStartedEvent>().Single();
        var firstUse = Facts<CardUseDeclaredEvent>().Single(fact => fact.CardId == cards[0].Id);
        Require(drawn is { EffectiveRank: 7, EffectiveKind: CardKind.IronChain, Source.OwnerSeat: 0 } &&
            drawn.RuxianFrameId == issued.FrameId && drawn.NativeParentId == firstUse.ResolutionId &&
            Facts<WuLuXunZhangcaiDrawReturnedEvent>().Single() is { RequestedCount: 1, ActualCount: 1 } draw && draw.FrameId == drawn.FrameId &&
            game.CardMovements.Count(move => move.Reason.Value == ZhangcaiDraw && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) == 1,
            "The original Ruxian right qualifies the paid rank-seven Use and returns one physical Zhangcai draw.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Reach(() => Facts<TurnStartedEvent>().Any(fact => fact.ActorSeat == 1));
        var runtime = game.GetRuntimeModule<CharacterLuXunWumiaoRuntime>();
        Require(Facts<TurnStartedEvent>().Last().ActorSeat == 1 && runtime.WuLuXunActiveRuxian(0) == issued,
            "The first foreign actual start preserves the original Ruxian right.");
        Reach(() => Facts<TurnStartedEvent>().Count(fact => fact.ActorSeat == 0) == 2);
        Require(Facts<TurnStartedEvent>().Last().TurnNumber > issued.ActualTurn && runtime.WuLuXunActiveRuxian(0) is null,
            "The next real own actual start expires the right before the next Play.");
        Reach(AtPlay);
        UseIronChain(cards[1].Id);
        Reach(AtPlay);
        var secondUse = Facts<CardUseDeclaredEvent>().Single(fact => fact.CardId == cards[1].Id);
        Require(Facts<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == secondUse.ResolutionId && fact.CardId == cards[1].Id) &&
            Facts<WuLuXunZhangcaiQualifiedEvent>().Single().Qualification.ActionId == drawn.ActionId &&
            Facts<WuLuXunZhangcaiStartedEvent>().SequenceEqual([drawn]),
            "The second real rank-seven Use finishes after expiry without another Zhangcai qualification or draw.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ruxian-turn-start", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "儒贤固定牌堆", 0, 1, [])
            {
                PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "儒贤实际回合", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

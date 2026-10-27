using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheResponseStockTacticChecks
{
    private const string Absolute = StandardShanheResponseTacticsPackage.JueduiWuxieSkillId;
    private const string Owner = "fixture:response-stock-owner", Enemy = "fixture:response-stock-enemy";
    private const string Deck = "fixture:response-stock-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheResponseTacticsPackage(), new Fixture()));

    public static void PaidAbsoluteNullificationEndsItsNativeResponseChain()
    {
        foreach (var owned in new[] { false, true })
        {
            var s = Scene.Start(owned ? [Absolute] : [],
                [new(CardKind.DrawTwo, Suit.Heart, 3), new(CardKind.Nullification, Suit.Spade, 11)],
                [new(CardKind.Nullification, Suit.Club, 12), new(CardKind.Nullification, Suit.Spade, 13)]);
            var draw = s.View().Hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
            var counter = s.View().Hand.Single(card => card.Kind == CardKind.Nullification).Id;
            var enemyCounters = s.View(1).Hand.Select(card => card.Id).ToArray();
            var action = s.Game.GetHumanLegalActions().First(candidate => candidate.CardId == draw && candidate.ConversionSource is null);
            s.Accept(new PlayCardCommand(0, draw, action.TargetSeats, s.Game.Revision, s.Prompt!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            s.Until(() => s.Prompt is { Kind: DecisionKind.Nullification, PlayerSeat: 0 });
            var window = s.Game.ResolutionStack.OfType<NullificationWindowFrame>().Single();
            Need(window.ChainDepth == 1 && s.Events<NullificationRespondedEvent>() is [{ ResponderSeat: 1, ChainDepth: 1 }],
                "The native enemy first pays a real Nullification against the player's Draw Two, so the holder's own response is a genuine second-layer counterspell.");
            var choice = s.Prompt!.Choices.Single(choice => choice.Cards.SequenceEqual([counter]) &&
                !choice.Parameters.ContainsKey("conversion-skill-id"));
            s.Accept(new AnswerPromptCommand(0, s.Prompt.PromptId, choice.Id, s.Game.Revision));
            s.Play();
            var resolved = s.Events<NullificationResolvedEvent>().Single();
            Need(s.Game.CardMovements.Count(move => move.CardId == counter && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Nullification) == 1 &&
                s.Game.CardMovements.Count(move => move.CardId == counter && move.From == CardLocation.Processing &&
                    move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.NullificationFinished) == 1 &&
                s.Events<CardUseFinishedEvent>().Count(fact => fact.CardId == draw) == 1,
                "The original physical counterspell is paid and cleaned exactly once before its original Draw Two use finishes.");
            if (owned)
                Need(s.Events<UnrespondableCounterspellIssuedEvent>() is [{ ResponderSeat: 0, ChainDepth: 2, Source.SkillId: Absolute }] &&
                    s.Events<NullificationRespondedEvent>().Select(fact => fact.ResponderSeat).SequenceEqual([1, 0]) &&
                    s.Events<NullificationRequestedEvent>().All(fact => fact.ChainDepth < 2) &&
                    resolved is { ChainDepth: 2, EffectNullified: false } &&
                    s.Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 2 &&
                    s.View(1).Hand.Count(card => enemyCounters.Contains(card.Id)) == 1,
                    "The owned production policy ends the actual paid depth-two node before any further response, leaves the enemy's spare Nullification untouched, and restores the original two-card draw.");
            else
                Need(s.Events<UnrespondableCounterspellIssuedEvent>().Length == 0 &&
                    s.Events<NullificationRespondedEvent>().Select(fact => fact.ResponderSeat).SequenceEqual([1, 0, 1]) &&
                    s.Events<NullificationRequestedEvent>().Any(fact => fact.ChainDepth == 2 && fact.ResponderSeat == 1) &&
                    resolved is { ChainDepth: 3, EffectNullified: true } &&
                    !s.Game.CardMovements.Any(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw),
                    "Without ownership the same paid physical response permits the enemy's real counter-counterspell, which cancels Draw Two through the ordinary chain.");
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native response-stock command."); }
        public void Play() => Until(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> condition)
        {
            for (var step = 0; step < 120; step++)
            {
                if (condition()) return;
                if (Prompt is { Kind: DecisionKind.Nullification } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId,
                        prompt.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "pass").Id, Game.Revision));
                else
                {
                    Need(Prompt?.PlayerSeat != 0, "The bounded response-stock scene reached an unexpected human prompt.");
                    Accept(new AdvanceOneStepCommand(Game.Revision));
                }
            }
            throw new InvalidOperationException("Missing bounded response-stock boundary.");
        }
        public static Scene Start(string[] tactics, ShanheStartingCard[] playerCards, ShanheStartingCard[] enemyCards)
        {
            var setup = new ShanheBattleSetup(113, 1, "fixture:response-stock", Owner, 12, 0, 0, 0, 0,
                [], playerCards, [], [new(Enemy) { StartingHand = enemyCards }], [], new string('f', 64))
                { TacticSkillIds = tactics };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-response-stock-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:taishi-ci");
            builder.AddGeneral(new(Owner, "响应战法持有者", "supporter", "classic:tianyi", "shu", 12));
            builder.AddGeneral(new(Enemy, "固定响应守军", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "零自然摸牌实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(index =>
                new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "响应与拼点战法实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

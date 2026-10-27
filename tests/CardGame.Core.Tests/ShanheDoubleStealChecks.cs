using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDoubleStealChecks
{
    private const string Owner = "fixture:double-steal-owner", Enemy = "fixture:double-steal-enemy";
    private const string Deck = "fixture:double-steal-deck", Fengzi = "ol:fengzi";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheDoubleStealPackage(), new Fixture()));

    public static void RealCounterspellAndPaidFengziKeepIndependentEffectRounds()
    {
        var cancelled = Scene.Start(enemyCards: 2); cancelled.Buy();
        var root = cancelled.Use(3);
        cancelled.Until(() => cancelled.G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        var counter = cancelled.Player(0).Hand.Single(card => card.Kind == CardKind.Nullification).Id;
        cancelled.Answer(choice => choice.Cards.SequenceEqual([counter]) && !choice.Parameters.ContainsKey("conversion-skill-id"));
        cancelled.Play();
        cancelled.AssertRoot(root, expectedTransfers: 1, owned: true, extraRounds: 1);
        Need(cancelled.Events<NullificationResolvedEvent>().Count(e => e.EffectNullified) == 1 &&
            cancelled.Player(1).HandCount == 1 &&
            cancelled.G.CardMovements.Count(move => move.CardId == counter && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Nullification) == 1 &&
            cancelled.G.CardMovements.Count(move => move.CardId == counter && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.NullificationFinished) == 1,
            "A real paid counterspell cancels one original effect; the extra effect has its own native counterspell window and takes only one target card.");

        var repeated = Scene.Start(enemyCards: 3, fengzi: true); repeated.Buy();
        var cost = repeated.Player(0).Hand.Single(card => card.Kind == CardKind.Dismantlement).Id;
        var card = repeated.Use(3); repeated.Play(payFengzi: true);
        repeated.AssertRoot(card, expectedTransfers: 3, owned: true, extraRounds: 2);
        var use = repeated.Events<CardUseDeclaredEvent>().Single(e => e.CardId == card);
        Need(repeated.Events<ExtraCardEffectIssuedEvent>().Count(e => e.CardUseFrameId == use.ResolutionId && e.PaidCardId == cost) == 1 &&
            repeated.G.CardMovements.Count(move => move.CardId == cost && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == "skill-program.extra-effect.cost") == 1 &&
            repeated.Player(1).HandCount == 0,
            "Production Fengzi pays one separate Trick and owns its extra round alongside Double Steal's independent native round, for three effects on one paid Snatch.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PlayerSnapshot Player(int seat) => G.CreateSnapshot(seat).Players[seat];
        public ShanheCampaignSnapshot Market() => G.CreateShanheMarketSnapshot(0)!;
        public T[] Events<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Double Steal command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = G.PendingDecision!;
            var choice = prompt.Choices.FirstOrDefault(predicate) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Until(Func<bool> done, bool payFengzi = false)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                if (G.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    if (payFengzi && prompt.SkillPrompt?.SkillId == Fengzi &&
                        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"))
                        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
                    else if (payFengzi && prompt.SkillPrompt?.SkillId == Fengzi &&
                        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "cost"))
                        Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "cost" &&
                            choice.Cards.Any(id => Player(0).Hand.Any(card => card.Id == id && card.Kind == CardKind.Dismantlement)));
                    else if (prompt.Kind == DecisionKind.SelectTargetCard)
                        Answer(_ => true);
                    else if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"))
                        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
                    else if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") is "no-nullification" or "pass"))
                        Answer(choice => choice.Parameters.GetValueOrDefault("response") is "no-nullification" or "pass");
                    else throw new InvalidOperationException("Unexpected Double Steal prompt: " + JsonSerializer.Serialize(prompt));
                }
                else
                {
                    Need(G.State.Winner == Winner.None, "The bounded Snatch scene must retain its surviving target.");
                    Submit(new AdvanceOneStepCommand(G.Revision));
                }
            }
            throw new InvalidOperationException("The real Double Steal effect did not return to its bounded native parent.");
        }
        public void Play(bool payFengzi = false) => Until(() => G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, payFengzi);
        public void Buy()
        {
            var item = Market().ShopItems.Single(item => item.Reward?.Id == StandardShanheCampaignCatalog.CurrentDoubleStealRewardId);
            Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
        }
        public int Use(int rank)
        {
            var material = Player(0).Hand.Single(card => card.Kind == CardKind.Snatch && card.Rank == rank);
            var action = G.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Snatch &&
                action.CardId == material.Id && action.TargetSeats.SequenceEqual([1]));
            Submit(new PlayCardCommand(0, material.Id, action.TargetSeats, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            return material.Id;
        }
        public void Snatch(int rank, int expectedTransfers, bool owned)
        {
            var before = G.CardMovements.Count(move => move.From == CardLocation.Hand(1) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Snatch);
            var material = Use(rank); Play();
            AssertRoot(material, expectedTransfers, owned, owned ? 1 : 0, before);
        }
        public void AssertRoot(int material, int expectedTransfers, bool owned, int extraRounds, int transferOffset = 0)
        {
            var use = Events<CardUseDeclaredEvent>().Single(item => item.CardId == material);
            var moves = G.CardMovements.Where(move => move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Snatch)
                .Skip(transferOffset).ToArray();
            Need(moves.Length == expectedTransfers && moves.Select(move => move.CardId).Distinct().Count() == expectedTransfers &&
                moves.All(move => G.CardMovements.Count(finish => finish.CardId == move.CardId &&
                    finish.From == CardLocation.Processing && finish.To == CardLocation.Hand(0) &&
                    finish.Reason == CardMoveReasons.SnatchFinished) == 1) &&
                Events<ShanheSchoolTrickIssuedEvent>().Count(item => item.UseFrameId == use.ResolutionId &&
                    item.Source.SkillId == StandardShanheDoubleStealPackage.SkillId) == (owned ? 1 : 0) &&
                Events<ShanheSchoolTrickStartedEvent>().Count(item => item.UseFrameId == use.ResolutionId) == (owned ? 1 : 0) &&
                Events<ShanheSchoolTrickCompletedEvent>().Count(item => item.UseFrameId == use.ResolutionId) == (owned ? 1 : 0) &&
                Events<ExtraCardEffectStartedEvent>().Count(item => item.CardUseFrameId == use.ResolutionId) == extraRounds &&
                Events<CardUseFinishedEvent>().Count(item => item.ResolutionId == use.ResolutionId) == 1 &&
                G.CardMovements.Count(move => move.CardId == material && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                G.CardMovements.Count(move => move.CardId == material && move.From == CardLocation.Processing &&
                    move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished) == 1 &&
                !G.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == use.ResolutionId),
                "Each actual effect takes at most one exact entity and returns before one root use, material settlement and finish: " +
                JsonSerializer.Serialize(new { material, expectedTransfers, moves, extraRounds,
                    issues = Events<ShanheSchoolTrickIssuedEvent>(), starts = Events<ExtraCardEffectStartedEvent>() }));
        }
        public static Scene Start(int enemyCards, bool fengzi = false)
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentDoubleStealRewardId, "双掠")
                    { TacticSkillId = StandardShanheDoubleStealPackage.SkillId, Rarity = 2 },
                new("supplies", "原生顺手试牌", StartingCards: [new(CardKind.Snatch, Suit.Diamond, 3),
                    new(CardKind.Snatch, Suit.Diamond, 4), new(CardKind.Snatch, Suit.Diamond, 5),
                    new(CardKind.Dismantlement, Suit.Spade, 6), new(CardKind.Nullification, Suit.Spade, 11)])
            ];
            var definition = new ShanheCampaignDefinition("fixture:double-steal", "真实顺手额外效果", [Owner], ["supplies"],
                [new("supplies", "实体补给", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take", "领取实体补给", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"],
                    Choices: [new("buy", "双掠", 300, RewardId: rewards[0].Id, LeavesNode: false)]),
                 new("battle", "固定守军", ShanheNodeKind.Battle, [],
                    [new("guard", "守军", [new(Enemy) { StartingHand = Enumerable.Range(0, enemyCards)
                        .Select(_ => new ShanheStartingCard(CardKind.Dodge, Suit.Club, 7)).ToArray() }])])],
                rewards, StartingGold: 1000);
            var campaign = ShanheCampaignSession.Create(definition, new(23, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { revision => new MoveShanheNodeCommand("supplies", revision),
                  revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                  revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = campaign.Submit(command(campaign.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter the real Snatch scene."); }
            var setup = campaign.PendingBattleSetup! with { ExtraSkillIds = fengzi ? [Fengzi] : [] };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-double-steal", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:wang-rong");
            builder.AddGeneral(new(Owner, "顺手使用者", "supporter", "standard:none", "wu", 20));
            builder.AddGeneral(new(Enemy, "固定守军", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "零自然摸牌实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "双掠实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

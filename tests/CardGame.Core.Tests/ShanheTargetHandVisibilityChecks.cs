using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheTargetHandVisibilityChecks
{
    private const string Owner = "fixture:target-hand-owner", Peer = "fixture:target-hand-peer";
    private const string Deck = "fixture:target-hand-deck", Identity = "fixture:target-hand-identity";
    private const string IdentityOwner = "fixture:target-hand-identity-owner";
    private const string VisibleCardId = "visible-target-hand-card-id";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheTargetHandVisibilityPackage(), new Fixture()));
    private static readonly (CardKind Kind, string Reward, int Price)[] Cases =
    [
        (CardKind.Dismantlement, StandardShanheCampaignCatalog.CurrentYanxianRewardId, 300),
        (CardKind.Snatch, StandardShanheCampaignCatalog.CurrentQiangquHaoduoRewardId, 500)
    ];

    public static void PaidVisibilityRefreshesIssuedOpaqueSlotsAndSelectsTheExactPhysicalCard()
    {
        foreach (var (kind, reward, price) in Cases)
        {
            var s = Scene.Start();
            var targetHand = s.G.CreateSnapshot(1).Players[1].Hand.ToArray();
            var chosen = targetHand.Single(card => card.Kind == CardKind.Dodge);
            var trick = s.Begin(kind);
            var old = s.P!;
            var selection = s.G.ResolutionStack.OfType<TargetCardSelectionFrame>().Single();
            Need(old is { Kind: DecisionKind.SelectTargetCard, IsPrivate: true } &&
                old.Choices.All(choice => choice.Cards.Count == 0 && !choice.Parameters.ContainsKey(VisibleCardId)) &&
                (s.G.CreateSnapshot(0).PrivateRevealedCards?.Count ?? 0) == 0,
                "The registered but unowned tactic leaves the real issued hand picker opaque.");
            var actionKind = kind == CardKind.Dismantlement ? LegalActionKind.Dismantlement : LegalActionKind.Snatch;
            Need(new SimpleAiBrain(0, 31, AiDecisionPolicy.CurrentVersion).ChooseTargetCardSlot(
                    s.G.CreateSnapshot(0), 1, actionKind, old.Choices, 1).Choice == old.Choices[0].Id,
                "Without the tactic the AI has only equal opaque slots, so it cannot use the target's unknown card faces.");

            s.Buy(reward, price);
            var view = s.G.CreateSnapshot(0);
            var current = view.PendingDecision!;
            Need(current.PromptId != old.PromptId && current.IsPrivate && current.PlayerSeat == 0 &&
                s.G.ResolutionStack.OfType<TargetCardSelectionFrame>().Single() == selection &&
                current.Choices.Select(choice => choice.Id).SequenceEqual(old.Choices.Select(choice => choice.Id)) &&
                current.Choices.All(choice => choice.Cards.Count == 0) &&
                (view.PrivateRevealedCards ?? []).Select(card => card.Id).Order()
                    .SequenceEqual(targetHand.Select(card => card.Id).Order()) &&
                current.Choices.All(choice => int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot) &&
                    slot >= 0 && slot < targetHand.Length &&
                    choice.Parameters.GetValueOrDefault(VisibleCardId) == targetHand[slot].Id.ToString() &&
                    choice.Description.Contains(targetHand[slot].DisplayName, StringComparison.Ordinal)),
                "The paid grant refreshes the suspended native picker, preserving its original frame, choice IDs and slot answers while privately mapping each visible entity.");
            Need(Enumerable.Range(2, 2).All(viewer =>
                s.G.CreateSnapshot(viewer) is var other && other.PendingDecision is null &&
                other.Players[1].Hand.Count == 0 &&
                !(other.PrivateRevealedCards ?? []).Any(card => targetHand.Any(target => target.Id == card.Id))) &&
                !view.PublicRevealedCards.Any(card => targetHand.Any(target => target.Id == card.Id)) &&
                !s.Events<CardsRevealedEvent>().Any(fact => fact.Cards.Any(card => targetHand.Any(target => target.Id == card.Id))),
                "Seeing the target hand creates no public reveal and gives neither bystander its cards or the chooser's private mapping.");

            var answer = current.Choices.Single(choice => choice.Parameters.GetValueOrDefault(VisibleCardId) == chosen.Id.ToString());
            var reorderedPrivateView = view with { PrivateRevealedCards = (view.PrivateRevealedCards ?? []).Reverse().ToArray() };
            Need(answer.Id != current.Choices[0].Id &&
                new SimpleAiBrain(0, 31, AiDecisionPolicy.CurrentVersion).ChooseTargetCardSlot(
                    reorderedPrivateView, 1, actionKind, current.Choices, 2).Choice == answer.Id,
                "With authorized visibility the AI selects the higher-value Dodge in the second actual slot by its entity-ID mapping, independently of private-card list order.");
            if (kind == CardKind.Dismantlement)
            {
                var revision = s.G.Revision;
                Need(!s.G.Submit(new AnswerPromptCommand(0, old.PromptId, answer.Id, revision)).Accepted && s.G.Revision == revision &&
                    !s.G.Submit(new AnswerPromptCommand(0, current.PromptId, new ChoiceId("target-card.id-" + chosen.Id), revision)).Accepted &&
                    s.G.Revision == revision,
                    "Display metadata creates no entity-ID answer path, and the replaced pre-purchase prompt cannot answer the native selection.");
            }
            s.Accept(new AnswerPromptCommand(0, current.PromptId, answer.Id, s.G.Revision));
            s.Play();
            var destination = kind == CardKind.Dismantlement ? CardLocation.DiscardPile : CardLocation.Hand(0);
            Need(s.G.CardMovements.Count(move => move.CardId == chosen.Id && move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(move => move.CardId == chosen.Id && move.From == CardLocation.Processing &&
                    move.To == destination) == 1 &&
                !s.G.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == chosen.Id) &&
                (s.G.CreateSnapshot(0).PrivateRevealedCards?.Count ?? 0) == 0,
                "The chosen visible physical card is discarded or acquired once by the original trick, and temporary hand visibility ends with its picker.");
            s.AssertPaid(trick);

            var otherKind = kind == CardKind.Dismantlement ? CardKind.Snatch : CardKind.Dismantlement;
            var otherTrick = s.Begin(otherKind);
            Need((s.G.CreateSnapshot(0).PrivateRevealedCards?.Count ?? 0) == 0 &&
                s.P!.Choices.All(choice => choice.Cards.Count == 0 && !choice.Parameters.ContainsKey(VisibleCardId)),
                "Each tactic is limited to its one corresponding trick; the other native hand picker stays opaque.");
            s.Accept(new AnswerPromptCommand(0, s.P.PromptId, s.P.Choices[0].Id, s.G.Revision));
            s.Play(); s.AssertPaid(otherTrick);
        }
    }

    public static void RegisteredVisibilitySkillsRemainInactiveInAnOrdinaryIdentityTable()
    {
        var s = Scene.StartIdentity();
        var trick = s.Begin(CardKind.Dismantlement);
        Need(s.P is { Kind: DecisionKind.SelectTargetCard } &&
            s.P.Choices.All(choice => choice.Cards.Count == 0 && !choice.Parameters.ContainsKey(VisibleCardId)) &&
            (s.G.CreateSnapshot(0).PrivateRevealedCards?.Count ?? 0) == 0 &&
            s.G.CreateSnapshot(0).Players[1].Hand.Count == 0,
            "Even a general owning both registered tactic programs gains no target-hand visibility in ordinary identity mode.");
        s.Accept(new AnswerPromptCommand(0, s.P!.PromptId, s.P.Choices[0].Id, s.G.Revision));
        s.Play(); s.AssertPaid(trick);
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public T[] Events<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native target-hand command rejected."); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(P is not { PlayerSeat: 0, Kind: DecisionKind.PlayCard or DecisionKind.SelectTargetCard },
                    "Unexpected earlier native target-hand prompt.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native target-hand flow did not reach its prompt.");
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard } && G.ResolutionStack.Count == 0);
        public int Begin(CardKind kind)
        {
            var card = G.CreateSnapshot(0).Players[0].Hand.First(item => item.Kind == kind);
            var action = G.GetHumanLegalActions().Single(candidate => candidate.CardId == card.Id &&
                candidate.ConversionSource is null && candidate.TargetSeats.SequenceEqual([1]) && candidate.TargetCardId is null);
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId));
            Until(() => P is { Kind: DecisionKind.SelectTargetCard });
            return card.Id;
        }
        public void Buy(string reward, int price)
        {
            var market = G.CreateShanheMarketSnapshot(0)!;
            var item = market.ShopItems.Single(stock => stock.Reward?.Id == reward);
            Need(item.GoldCost == price, "The visible tactic uses its source rarity and actual original price.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
            Need(G.CreateShanheMarketSnapshot(0)!.Gold == market.Gold - price &&
                Events<ShanheMarketPurchaseCommittedEvent>().Single().Price == price &&
                Events<ShanheMarketPurchaseAppliedEvent>().Single().RewardId == reward,
                "The live purchase pays once and applies its original native acquisition before the suspended picker resumes.");
        }
        public void AssertPaid(int card)
        {
            var use = Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == card).ResolutionId;
            Need(G.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Processing &&
                    move.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use) == 1,
                "The original trick keeps its one real physical payment and finishes once.");
        }
        public static Scene Start()
        {
            ShanheRewardDefinition[] rewards =
            [new(StandardShanheCampaignCatalog.CurrentYanxianRewardId, "眼线")
                { TacticSkillId = StandardShanheTargetHandVisibilityPackage.YanxianSkillId, Rarity = 2 },
             new(StandardShanheCampaignCatalog.CurrentQiangquHaoduoRewardId, "强取豪夺")
                { TacticSkillId = StandardShanheTargetHandVisibilityPackage.QiangquHaoduoSkillId, Rarity = 3 },
             new("cards", "真实锦囊", StartingCards:
                [new(CardKind.Dismantlement, Suit.Spade, 3), new(CardKind.Snatch, Suit.Spade, 4)])];
            var definition = new ShanheCampaignDefinition("fixture:target-hand", "目标手牌可见", [Owner], ["cards"],
                [new("cards", "真实补给", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take", "领取", RewardId: "cards")]),
                 new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("yanxian", "眼线", 300, RewardId: rewards[0].Id, LeavesNode: false),
                     new("qiangqu", "强取豪夺", 500, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "真实目标手牌", ShanheNodeKind.Battle, [],
                    [new("peers", "目标与旁观者", Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer)
                        { StartingHand = [new(CardKind.Peach, Suit.Heart, 12), new(CardKind.Dodge, Suit.Club, 2)] }).ToArray())])],
                rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true,
                    UnlockedRewardIds = [rewards[0].Id, rewards[1].Id] }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { revision => new MoveShanheNodeCommand("cards", revision),
                  revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                  revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter visibility fixture."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, Registry.Value);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
        public static Scene StartIdentity()
        {
            var game = GameEngine.CreateStandard(new()
            {
                Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Identity,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, Registry.Value);
            var scene = new Scene(game); scene.Accept(new StartGameCommand());
            scene.Until(() => scene.P is { Kind: DecisionKind.SelectGeneral });
            scene.Accept(new SelectGeneralCommand(0, IdentityOwner, game.Revision, scene.P!.PromptId));
            scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:target-hand", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "锦囊使用者", "supporter", "standard:none", "shu", 8));
            builder.AddGeneral(new(Peer, "目标与旁观者", "supporter", "standard:none", "wei", 8));
            builder.AddGeneral(new(IdentityOwner, "普通场持有者", "supporter",
                StandardShanheTargetHandVisibilityPackage.YanxianSkillId, "shu", 8,
                [StandardShanheTargetHandVisibilityPackage.QiangquHaoduoSkillId]));
            foreach (var seat in Enumerable.Range(1, 3))
                builder.AddGeneral(new(Peer + seat, "普通场角色", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "无自然摸牌", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dismantlement", Suit.Spade, 3)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "目标手牌实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
            builder.AddMode(new(Identity, "普通身份场", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [IdentityOwner, Peer + "1", Peer + "2", Peer + "3"]));
        }
    }
}

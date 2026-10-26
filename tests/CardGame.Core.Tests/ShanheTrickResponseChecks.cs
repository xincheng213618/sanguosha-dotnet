using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheTrickResponseChecks
{
    private const string Owner = "fixture:trick-response-owner", Peer = "fixture:trick-response-peer", Deck = "fixture:trick-response-deck";
    private const int Ally = 1, Enemy = 2, Bystander = 3;
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheTrickResponsePackage(), new Fixture()));

    public static void PaidYinyangKeepsAlliesAndCounterspellChainOutsideEnemyScope()
    {
        var s = Scene.Start();
        var before = s.Events<DuelResponseEvent>().Length;
        s.Use(CardKind.Duel, 1, Enemy);
        var baseline = s.Events<DuelResponseEvent>().Skip(before).Where(e => e.ResponderSeat == Enemy && e.UsedSlash).ToArray();
        Need(baseline is [{ SlashCardId: { } paid }] &&
            s.G.CardMovements.Count(m => m.CardId == paid && m.From == CardLocation.Hand(Enemy) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == paid && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "Registered future stock does not prohibit the real unowned Duel response or its actual Slash payment. " + s.DuelDiagnostic());
        s.Buy(StandardShanheCampaignCatalog.CurrentYinyangRewardId);
        var slashBefore = s.Hand(Enemy).Count(c => c.Kind == CardKind.Slash);
        var hits = s.Events<DamageAppliedEvent>().Length;
        s.Use(CardKind.Duel, 2, Enemy);
        Need(s.Hand(Enemy).Count(c => c.Kind == CardKind.Slash) == slashBefore &&
            s.Events<DamageAppliedEvent>().Skip(hits).Any(e => e.SourceSeat == 0 && e.TargetSeat == Enemy && e.Amount == 1),
            "The actual 700-coin purchase immediately prevents the enemy Duel response without consuming its Slash.");
        foreach (var (kind, rank) in new[] { (CardKind.BarbarianAssault, 3), (CardKind.ArrowBarrage, 4) })
        {
            var responses = s.Events<GroupResponseEvent>().Length;
            s.Use(kind, rank, Enemy);
            var actual = s.Events<GroupResponseEvent>().Skip(responses).Where(e => e.IncomingCard == kind).ToArray();
            Need(actual.Single(e => e.ResponderSeat == Ally).UsedResponse &&
                actual.Where(e => e.ResponderSeat is Enemy or Bystander).All(e => !e.UsedResponse),
                "Enemy-only restrictions preserve the teammate's genuine Slash/Dodge payment for each native group trick: " + JsonSerializer.Serialize(actual));
        }

        var chain = Scene.Start(ownCounter: true, bystanderCounter: true);
        chain.Buy(StandardShanheCampaignCatalog.CurrentYinyangRewardId);
        var card = chain.Begin(CardKind.Duel, 1, Ally);
        chain.Until(() => chain.P is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        Need(chain.Events<NullificationRespondedEvent>().Length == 0 && chain.Hand(Bystander).Any(c => c.Kind == CardKind.Nullification),
            "An enemy bystander cannot answer the original damage trick, while the source retains its own initial counterspell choice.");
        chain.Counter(); chain.Play(); chain.AssertPaid(card);
        Need(chain.Events<NullificationRespondedEvent>().Select(e => (e.ResponderSeat, e.ChainDepth)).SequenceEqual([(0, 1), (Bystander, 2)]) &&
            chain.Events<CardRespondedEvent>().Any(e => e.ResponderSeat == Ally && e.EffectiveCardKind == CardKind.Slash),
            "The enemy may respond to the actual subsequent counterspell, and the friendly original target still pays its own Duel response.");
    }

    public static void PaidSuanwuyiceKeepsBystandersAndResumesItsOwnBlockedInitialCounterspell()
    {
        var chain = Scene.Start(ownCounter: true, targetCounter: true, bystanderCounter: true);
        chain.Buy(StandardShanheCampaignCatalog.CurrentSuanwuyiceRewardId);
        var card = chain.Begin(CardKind.Duel, 1, Enemy);
        chain.Until(() => chain.P is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        Need(chain.Events<NullificationRespondedEvent>().Select(e => (e.ResponderSeat, e.ChainDepth)).SequenceEqual([(Bystander, 1)]) &&
            chain.Hand(Enemy).Any(c => c.Kind == CardKind.Nullification),
            "Only the actual original target is prohibited: an enemy bystander pays the initial counterspell and the target keeps its card.");
        chain.Counter(); chain.Play(); chain.AssertPaid(card);
        Need(chain.Events<NullificationRespondedEvent>().Select(e => (e.ResponderSeat, e.ChainDepth)).SequenceEqual([(Bystander, 1), (0, 2), (Enemy, 3)]) &&
            chain.Events<NullificationResolvedEvent>().Last().EffectNullified,
            "A target can counter the real counterspell at a later chain node; the original trick's prohibition is not inherited.");

        var target = Scene.Start(); target.Buy(StandardShanheCampaignCatalog.CurrentSuanwuyiceRewardId);
        var allySlashes = target.Hand(Ally).Count(c => c.Kind == CardKind.Slash);
        target.Use(CardKind.Duel, 1, Ally);
        Need(target.Hand(Ally).Count(c => c.Kind == CardKind.Slash) == allySlashes &&
            target.Events<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == Ally),
            "The actual friendly target is also prohibited, rather than silently replacing target scope with enemy scope.");

        var own = Scene.Start(ownCounter: true);
        var draw = own.Begin(CardKind.DrawTwo, 5, 0);
        own.Until(() => own.P is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        var oldPrompt = own.P!.PromptId;
        own.Buy(StandardShanheCampaignCatalog.CurrentSuanwuyiceRewardId);
        own.Play(); own.AssertPaid(draw);
        Need(own.Events<NullificationRespondedEvent>().Length == 0 && own.Hand(0).Any(c => c.Kind == CardKind.Nullification) &&
            own.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Draw) == 2 &&
            own.P!.PromptId != oldPrompt,
            "Buying during one's own DrawTwo initial counterspell resumes that original native window, blocks the now-target owner's response, and resolves two real draws without paying its retained counterspell.");
    }

    public static void DamageTrickScopeIncludesQiZhengFloodAndUnexpectedAssault()
    {
        foreach (var (kind, rank) in new[] { (CardKind.QizhengXiangsheng, 7), (CardKind.WumiaoFlood, 8), (CardKind.UnexpectedAssault, 9) })
        {
            var s = Scene.Start(bystanderCounter: true);
            s.Buy(StandardShanheCampaignCatalog.CurrentYinyangRewardId);
            var card = s.Use(kind, rank, Enemy);
            var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == card);
            Need(!s.Events<NullificationRespondedEvent>().Any(e => e.ResolutionId == use.ResolutionId) &&
                s.Hand(Bystander).Any(c => c.Kind == CardKind.Nullification),
                "The existing damage-trick classification includes the extended real native effect and prohibits an enemy's initial counterspell: " + kind);
            if (kind == CardKind.QizhengXiangsheng)
                Need(s.Events<QizhengResolvedEvent>().Single(e => e.UseId == use.ResolutionId) is { ResponseKind: null, Ambush: true } &&
                    !s.Events<QizhengResponsePaidEvent>().Any(e => e.UseId == use.ResolutionId),
                    "QiZheng's typed response owner offers no physical or converted Slash/Dodge when its actual enemy target cannot respond.");
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.PendingDecision;
        public IReadOnlyList<CardSnapshot> Hand(int seat) => G.CreateSnapshot(seat).Players[seat].Hand;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public string DuelDiagnostic() => JsonSerializer.Serialize(new
        {
            duel = Events<DuelResponseEvent>(), responded = Events<CardRespondedEvent>(),
            players = Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).Players[seat]).Select(player =>
                new { player.Seat, player.TeamId, player.Hp, player.MaxHp, hand = player.Hand.Select(card => new { card.Id, card.Kind }) }),
            thoughts = G.AiThoughts.TakeLast(5)
        });
        private ShanheCampaignSnapshot Market => G.CreateShanheMarketSnapshot(0)!;
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native trick-response command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P!;
            var choice = prompt.Choices.FirstOrDefault(predicate) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded response fixture ended before its actual native use returned.");
                if (P is { PlayerSeat: 0, Choices.Count: > 0 })
                {
                    var choice = P.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "qizheng" &&
                        c.Parameters.GetValueOrDefault("value") == "ambush") ??
                        P.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "no-nullification") ??
                        P.Choices[0];
                    Answer(candidate => candidate.Id == choice.Id);
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native trick-response flow did not return: " + JsonSerializer.Serialize(P));
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Buy(string reward)
        {
            var item = Market.ShopItems.Single(i => i.Reward?.Id == reward);
            Need(item.GoldCost == 700, "The first real tactic uses its exact source price.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
            Need(Market.Gold == 500 && Events<ShanheMarketPurchaseCommittedEvent>() is [{ Price: 700 }] &&
                Events<ShanheMarketPurchaseAppliedEvent>().Length == 1,
                "Each real purchase pays 700 once and applies the owned native policy immediately.");
        }
        public int Begin(CardKind kind, int rank, int target)
        {
            var card = Hand(0).Single(c => c.Kind == kind && c.Rank == rank);
            var actions = G.GetHumanLegalActions().Where(a => a.CardId == card.Id && a.ConversionSource is null).ToArray();
            var action = actions.FirstOrDefault(a => a.TargetSeats.Contains(target) ||
                a.TargetSeats.Count == 0 && (kind == CardKind.DrawTwo && target == 0 ||
                    kind is CardKind.BarbarianAssault or CardKind.ArrowBarrage && target != 0))
                ?? throw new InvalidOperationException($"Missing native {kind}/{rank} toward seat {target}: " + JsonSerializer.Serialize(actions));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId)); return card.Id;
        }
        public int Use(CardKind kind, int rank, int target)
        { var card = Begin(kind, rank, target); Play(); AssertPaid(card); return card; }
        public void Counter()
        {
            var id = Hand(0).Single(c => c.Kind == CardKind.Nullification).Id;
            Answer(choice => choice.Cards.SequenceEqual([id]) && !choice.Parameters.ContainsKey("conversion-skill-id"));
        }
        public void AssertPaid(int id)
        {
            var use = Events<CardUseDeclaredEvent>().Single(e => e.CardId == id);
            Need(G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1,
                "The original real trick pays, cleans up and finishes exactly once.");
        }
        public static Scene Start(bool ownCounter = false, bool targetCounter = false, bool bystanderCounter = false)
        {
            var starting = new List<ShanheStartingCard>
            {
                new(CardKind.Duel, Suit.Heart, 1), new(CardKind.Duel, Suit.Spade, 2),
                new(CardKind.BarbarianAssault, Suit.Spade, 3), new(CardKind.ArrowBarrage, Suit.Heart, 4),
                new(CardKind.DrawTwo, Suit.Heart, 5), new(CardKind.QizhengXiangsheng, Suit.Club, 7),
                new(CardKind.WumiaoFlood, Suit.Spade, 8), new(CardKind.UnexpectedAssault, Suit.Club, 9),
                new(CardKind.Slash, Suit.Heart, 10)
            };
            if (ownCounter) starting.Add(new(CardKind.Nullification, Suit.Club, 6));
            ShanheStartingCard[] PeerCards(bool counter) =>
                [new(CardKind.Slash, Suit.Spade, 11), new(CardKind.Slash, Suit.Club, 12),
                 new(CardKind.Slash, Suit.Heart, 13), new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Diamond, 8),
                 .. (counter ? new[] { new ShanheStartingCard(CardKind.Nullification, Suit.Spade, 6) } : [])];
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentYinyangRewardId, "阴阳术法")
                    { TacticSkillId = StandardShanheTrickResponsePackage.YinyangSkillId, Rarity = 4 },
                new(StandardShanheCampaignCatalog.CurrentSuanwuyiceRewardId, "算无遗策")
                    { TacticSkillId = StandardShanheTrickResponsePackage.SuanwuyiceSkillId, Rarity = 4 },
                new("supplies", "真实锦囊试牌", StartingCards: starting.ToArray())
            ];
            var definition = new ShanheCampaignDefinition("fixture:trick-response", "真实锦囊响应", [Owner], ["supplies"],
                [new("supplies", "实体补给", ShanheNodeKind.Event, ["shop", "battle"], Choices: [new("take", "领取", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-yin", "阴阳术法", 700, RewardId: rewards[0].Id, LeavesNode: false),
                     new("buy-suan", "算无遗策", 700, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "固定敌友", ShanheNodeKind.Battle, [],
                    [new("peers", "敌友原生响应", [new(Peer, Hp: 28) { StartingHand = PeerCards(targetCounter) },
                        new(Peer, Hp: 28) { StartingHand = PeerCards(bystanderCounter) }],
                        Allies: [new(Peer, Hp: 28) { StartingHand = PeerCards(false) }])])], rewards, StartingGold: 1200);
            var campaign = ShanheCampaignSession.Create(definition, new(37, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true,
                    UnlockedRewardIds = [rewards[0].Id, rewards[1].Id] }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                     { revision => new MoveShanheNodeCommand("supplies", revision),
                       revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                       revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = campaign.Submit(command(campaign.Revision)); Need(result.Accepted, result.Error ?? "Cannot start the actual trick-response battle."); }
            var setup = campaign.PendingBattleSetup!;
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shanhe-trick-response", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "锦囊使用者", "supporter", "standard:none", "shu", 30));
            builder.AddGeneral(new(Peer, "固定友军与敌军", "supporter", "standard:none", "wei", 30));
            builder.AddDeck(new(Deck, "零普通摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 36).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "锦囊响应实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

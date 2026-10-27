using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHandLimitStockChecks
{
    private const string Owner = "fixture:hand-limit-stock-owner", Enemy = "fixture:hand-limit-stock-enemy";
    private const string Driver = "fixture:hand-limit-stock-driver", Deck = "fixture:hand-limit-stock-deck";
    private const string DoubleQuota = "fixture:hand-limit-stock-double-quota";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheMarketPackage(), new StandardShanheHandLimitStockPackage(), new Fixture()));

    public static void PaidExemptionsRefreshTheSamePartlyPaidDiscard()
    {
        var s = Scene.Start();
        Need(s.V.HandCount == 10 && s.V.HandLimit == 2,
            "Registering future exemption stock grants no exemption before the real purchase.");
        s.EndPlay();
        Need(s.P is { Kind: DecisionKind.DiscardCards, RequiredCardCount: 8 } && s.P.ValidCardIds.Count == 10,
            "The unowned native discard counts every actual hand entity.");
        var original = s.G.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        var earlyPayment = s.PaySlashes(2);
        Need(s.P is { Kind: DecisionKind.DiscardCards, RequiredCardCount: 6 },
            "A real partial payment leaves exactly the original unpaid six-card obligation.");

        var firstPrice = s.Buy(StandardShanheCampaignCatalog.CurrentHaoshenfaRewardId);
        s.AssertDiscard(original.Id, 3, [CardKind.Slash, CardKind.Peach]);
        var secondPrice = s.Buy(StandardShanheCampaignCatalog.CurrentCangtaohuRewardId);
        s.AssertDiscard(original.Id, 1, [CardKind.Slash]);
        Need(firstPrice == 500 && secondPrice == 300 && s.Market.Gold == 1200 &&
            s.Events<ShanheMarketPurchaseCommittedEvent>().Select(e => e.Price).SequenceEqual([500, 300]) &&
            s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 2,
            "Both actual acquisitions pay the quoted escalating tactic price once and immediately refresh the original discard.");
        var rejected = s.G.Submit(new DiscardCardsCommand(0, [s.V.Hand.First(c => c.Kind == CardKind.Dodge).Id],
            s.P!.PromptId, s.G.Revision));
        Need(!rejected.Accepted && s.P.RequiredCardCount == 1,
            "An exempt Dodge cannot be used as payment for the remaining native hand-limit discard.");
        var finalPayment = s.PaySlashes(1);
        var paid = earlyPayment.Concat(finalPayment).ToArray();
        Need(s.V.HandCount == 7 && s.V.Hand.Count(c => c.Kind == CardKind.Dodge) == 3 &&
            s.V.Hand.Count(c => c.Kind == CardKind.Peach) == 2 && s.V.Hand.Count(c => c.Kind == CardKind.Slash) == 2 &&
            s.V.Hp == 2 && !s.G.ResolutionStack.OfType<ManualDiscardFrame>().Any(),
            "The native discard keeps all five exempt physical cards and only two counted cards without healing or adding hand capacity. " + s.Diagnostic());
        Need(paid.All(id => s.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard) == 1) &&
            s.G.CardMovements.Count(move => move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard) == 3,
            "Earlier payments remain paid, and only the one remaining entity is discarded after the two purchases.");
        s.ReachPlay();
        Need(s.V.HandLimit == 2 && s.V.HandCount == 7 && s.V.Hp == 2,
            "The next real owner play boundary exposes the same two-card limit and retains the seven actual cards.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public ShanheCampaignSnapshot Market => G.CreateShanheMarketSnapshot(0)!;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public string Diagnostic() => JsonSerializer.Serialize(new
        {
            V.Hp, V.MaxHp, V.HandCount, V.HandLimit,
            hand = V.Hand.Select(card => new { card.Id, card.Kind }),
            discard = G.ResolutionStack.OfType<ManualDiscardFrame>().ToArray(),
            prompt = P is null ? null : new { P.Kind, P.RequiredCardCount, P.ValidCardIds },
            moves = G.CardMovements.Where(move => move.From == CardLocation.Hand(0)).TakeLast(14)
        });
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native hand-limit stock command."); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                Need(G.State.Status != EngineStatus.Completed && P?.PlayerSeat != 0,
                    "Unexpected hand-limit stock boundary: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native hand-limit stock boundary: " + JsonSerializer.Serialize(P));
        }
        public void EndPlay()
        {
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards });
        }
        public void ReachPlay() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string activation)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], G.Revision, P!.PromptId));
            Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public int Buy(string rewardId)
        {
            var item = Market.ShopItems.Single(i => i.Reward?.Id == rewardId);
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
            return item.GoldCost;
        }
        public int[] PaySlashes(int count)
        {
            var prompt = P!;
            var ids = V.Hand.Where(card => card.Kind == CardKind.Slash).Take(count).Select(card => card.Id).ToArray();
            Need(ids.Length == count, "The actual hand must contain every requested payment.");
            Accept(new DiscardCardsCommand(0, ids, prompt.PromptId, G.Revision));
            return ids;
        }
        public void AssertDiscard(long frameId, int remaining, CardKind[] kinds, int initial = 8, int multiplier = 1)
        {
            var frame = G.ResolutionStack.OfType<ManualDiscardFrame>().SingleOrDefault();
            var legal = V.Hand.Where(card => kinds.Contains(card.Kind)).Select(card => card.Id).Order().ToArray();
            Need(frame is not null && frame.Id == frameId && frame.InitialRequiredCount == initial &&
                frame.QuotaMultiplier == multiplier && frame.PaidCount == 2 && frame.RemainingCount == remaining &&
                P is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards } && P.RequiredCardCount == remaining &&
                P.ValidCardIds.Order().SequenceEqual(legal),
                "The acquired policy refreshes the same owning discard frame, preserves paid entities and publishes only still-counted kinds. " + Diagnostic());
        }
        public static Scene Start(bool doubleQuota = false, bool uncapped = false)
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentHaoshenfaRewardId, "好身法")
                    { TacticSkillId = StandardShanheHandLimitStockPackage.HaoshenfaSkillId, Rarity = 3 },
                new(StandardShanheCampaignCatalog.CurrentCangtaohuRewardId, "藏桃户")
                    { TacticSkillId = StandardShanheHandLimitStockPackage.CangtaohuSkillId, Rarity = 1 },
                new(StandardShanheCampaignCatalog.CurrentStableCapacityRewardId, "稳定承载")
                    { TacticSkillId = StandardShanheHandLimitStockPackage.StableCapacitySkillId, Rarity = 1 },
                new("supplies", "实体试牌", StartingCards: [new(CardKind.Dodge), new(CardKind.Dodge), new(CardKind.Dodge),
                    new(CardKind.Peach), new(CardKind.Peach), new(CardKind.Slash), new(CardKind.Slash),
                    new(CardKind.Slash), new(CardKind.Slash), new(CardKind.Slash)])
            ];
            var definition = new ShanheCampaignDefinition("fixture:hand-limit-stock", "实际手牌容量", [Owner], ["supplies"],
                [new("supplies", "领取试牌", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take", "领取实体试牌", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-dodge", "好身法", 500, RewardId: rewards[0].Id, LeavesNode: false),
                     new("buy-peach", "藏桃户", 200, RewardId: rewards[1].Id, LeavesNode: false),
                     new("buy-stable", "稳定承载", 200, RewardId: rewards[2].Id, LeavesNode: false)]),
                 new("battle", "固定守军", ShanheNodeKind.Battle, [],
                    [new("guard", "守军", [new(Enemy) { StartingHand = [] }])])], rewards, StartingGold: 2000);
            var session = ShanheCampaignSession.Create(definition, new(23, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { revision => new MoveShanheNodeCommand("supplies", revision),
                  revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                  revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter the actual hand-limit scene."); }
            var setup = session.PendingBattleSetup! with
            { Hp = uncapped ? 5 : 2, MaxHpBonus = uncapped ? 1 : 0, ExtraSkillIds = doubleQuota ? [DoubleQuota] : [] };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Until(() => scene.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-hand-limit-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                  {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"loseHp","target":"owner","amount":1}]},
                  {"id":"shrink","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"changeMaximumHp","target":"owner","amount":-1}]}]},
                  {"id":"{{DoubleQuota}}","revision":1,"modifiers":[
                    {"id":"double-frozen-discard","query":"discardQuotaMultiplier","operation":"set","value":2,"priority":0}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    { [Driver] = new { name = "真实体力变化", description = "容量实战试牌。" },
                      [DoubleQuota] = new { name = "双倍弃牌", description = "原生双倍弃牌容量。" } } }));
            builder.AddSkill(new(Driver, "真实体力变化", "容量实战试牌。")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddSkill(new(DoubleQuota, "双倍弃牌", "原生双倍弃牌容量。")
                { Program = catalog.Programs[DoubleQuota], ProgramPresentation = catalog.Presentations[DoubleQuota], Tags = SkillTag.Locked });
            builder.AddGeneral(new(Owner, "容量使用者", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Enemy, "固定守军", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "零常规摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "手牌容量实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

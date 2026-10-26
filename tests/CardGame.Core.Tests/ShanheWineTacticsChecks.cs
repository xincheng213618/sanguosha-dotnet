using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWineTacticsChecks
{
    private const string Owner = "fixture:wine-owner", Peer = "fixture:wine-peer";
    private const string Driver = "fixture:wine-driver", Quiet = "fixture:wine-quiet", Deck = "fixture:wine-deck";

    public static void PaidWineTiersAddRealQuotaAndRenewOnTheNextActualTurn()
    {
        foreach (var (rewards, quota, paid) in new (string[] Rewards, int Quota, int Paid)[]
        {
            ([StandardShanheCampaignCatalog.CurrentHugujiuRewardId], 2, 300),
            ([StandardShanheCampaignCatalog.CurrentHugujiu2RewardId], 3, 700),
            ([StandardShanheCampaignCatalog.CurrentHugujiuRewardId, StandardShanheCampaignCatalog.CurrentHugujiu2RewardId], 4, 1100)
        })
        {
            var s = Scene.Start();
            var turn = s.ActualTurn;
            s.WineSlash();
            Need(s.OrdinaryPaid(turn) == 1 && !s.HasWineAction &&
                !s.Player().Skills!.Any(skill => skill.ContentId is StandardShanheCampaignCatalog.CurrentHugujiuSkillId or
                    StandardShanheCampaignCatalog.CurrentHugujiu2SkillId),
                "Merely registering future wine stock records the first real paid wine without granting its unowned extra quota.");
            s.RejectWine();
            foreach (var reward in rewards) s.Buy(reward);
            Need(s.Market().Gold == 3000 - paid && s.Events<ShanheMarketPurchaseCommittedEvent>().Length == rewards.Length &&
                s.Events<ShanheMarketPurchaseAppliedEvent>().Length == rewards.Length && s.HasWineAction,
                "A real current-price purchase, including the 100-coin second-tactic surcharge, immediately opens only the remaining native wine quota.");
            for (var use = 1; use < quota; use++) s.WineSlash();
            Need(s.OrdinaryPaid(turn) == quota && !s.HasWineAction &&
                s.Events<AlcoholAppliedEvent>().Length == quota &&
                s.Events<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => e.Amount).SequenceEqual(Enumerable.Repeat(2, quota)),
                "The real 2108, 2109 and combined sources allow exactly 1+1, 1+2 and 1+1+2 ordinary wine uses, each feeding one native two-damage Slash.");
            s.RejectWine();
            s.NextOwnPlay();
            Need(s.ActualTurn > turn && s.OrdinaryPaid(s.ActualTurn) == 0 && s.HasWineAction,
                "The original owner reaches a genuinely later native turn and starts with a fresh bounded wine budget.");
            for (var use = 0; use < quota; use++) s.WineSlash();
            Need(s.OrdinaryPaid(s.ActualTurn) == quota && !s.HasWineAction && s.Market().Gold == 3000 - paid &&
                s.Events<WushuangCombatUseFactEvent>().Length == 0,
                "The purchased per-turn quota repeats without another charge or Wushuang ownership/ledger.");
            s.RejectWine();
        }
    }

    public static void DyingPhysicalWineDoesNotConsumeTheOrdinaryWineBudget()
    {
        var s = Scene.Start();
        s.Submit(new UseProgramSkillCommand(0, Driver, "hurt-four", [], [], s.G.Revision, s.G.PendingDecision!.PromptId));
        s.ReachDying();
        var wine = s.Player().Hand.First(c => c.Kind == CardKind.Alcohol).Id;
        var prompt = s.G.PendingDecision!;
        var choice = prompt.Choices.First(c => c.Cards.Contains(wine));
        s.Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, s.G.Revision)); s.Play();
        Need(s.Player().Hp == 1 && !s.Player().HasAlcoholEffect && s.OrdinaryPaid(s.ActualTurn) == 0 &&
            s.Events<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 1, RemainingHp: 1 }] &&
            s.Events<AlcoholAppliedEvent>().Length == 0 &&
            s.G.CardMovements.Count(m => m.CardId == wine && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == wine && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "An actual physical Alcohol rescues its original dying owner once; it neither arms Slash damage nor debits the ordinary wine-use invoice.");
        s.Buy(StandardShanheCampaignCatalog.CurrentHugujiuRewardId);
        for (var use = 0; use < 2; use++) s.WineSlash();
        Need(s.OrdinaryPaid(s.ActualTurn) == 2 && !s.HasWineAction && s.Player().Hp == 1,
            "After real rescue and a paid live purchase, the owner still has both ordinary uses of base plus 2108.");
        s.RejectWine();
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PlayerSnapshot Player() => G.CreateSnapshot(0).Players[0];
        public ShanheCampaignSnapshot Market() => G.CreateShanheMarketSnapshot(0)!;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int ActualTurn => Events<TurnStartedEvent>().Last(e => e.ActorSeat == 0).TurnNumber;
        public int OrdinaryPaid(int turn) => Events<JianjiOrdinaryAlcoholPaidEvent>().Count(e => e.ActorSeat == 0 && e.ActualTurn == turn && e.CountsQuota);
        public bool HasWineAction => G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Alcohol);

        public void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected wine-tactic command."); }

        public void Play()
        {
            for (var step = 0; step < 160; step++)
            {
                if (G.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(G.State.Winner == Winner.None, "Wine fixture ended before its native Play boundary.");
                if (G.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        ?? prompt.Choices[0];
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Wine fixture lost its bounded native Play boundary.");
        }

        public void ReachDying()
        {
            for (var step = 0; step < 80; step++)
            {
                if (G.PendingDecision is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 }) return;
                Need(G.PendingDecision is null, "Wine self-damage reached an unrelated decision.");
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Wine self-damage failed to reach its native dying rescue.");
        }

        private int Card(CardKind kind)
        {
            var card = Player().Hand.First(c => c.Kind == kind);
            var targets = kind == CardKind.Slash ? new[] { 1 } : [];
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Submit(new PlayCardCommand(0, card.Id, targets, G.Revision, G.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play(); return card.Id;
        }

        public void WineSlash()
        {
            var wine = Card(CardKind.Alcohol);
            Need(Player().HasAlcoholEffect && !HasWineAction,
                "Extra uses preserve the native pending-wine gate: a second wine cannot stack while the first boost is still armed.");
            var hits = Events<DamageAppliedEvent>().Length;
            var slash = Card(CardKind.Slash);
            Need(!Player().HasAlcoholEffect && Events<DamageAppliedEvent>().Skip(hits).ToArray() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 2 }] &&
                new[] { wine, slash }.All(id =>
                    G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                    G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1),
                "Each physical wine/Slash pair pays each hand entity once, deals its native two damage and finishes once in the discard pile.");
        }

        public void RejectWine()
        {
            var card = Player().Hand.First(c => c.Kind == CardKind.Alcohol);
            var revision = G.Revision; var gold = Market().Gold;
            var paid = Events<JianjiOrdinaryAlcoholPaidEvent>().Length; var moves = G.CardMovements.Count;
            Need(!G.Submit(new PlayCardCommand(0, card.Id, [], revision, G.PendingDecision!.PromptId, CardKind.Alcohol)).Accepted &&
                G.Revision == revision && Market().Gold == gold && Player().Hand.Any(c => c.Id == card.Id) &&
                G.CardMovements.Count == moves && Events<JianjiOrdinaryAlcoholPaidEvent>().Length == paid,
                "At the genuine ordinary-use limit, another physical wine command cannot spend a card, invoice, revision or market coins.");
        }

        public void Buy(string reward)
        {
            var item = Market().ShopItems.Single(i => i.Reward?.Id == reward);
            Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision)); Play();
        }

        public void NextOwnPlay()
        {
            var turn = ActualTurn;
            Submit(new EndPlayPhaseCommand(0, G.Revision, G.PendingDecision!.PromptId)); Play();
            Need(ActualTurn > turn, "The wine quota check must cross a real native turn boundary.");
        }

        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMarketPackage(),
                new StandardShanheWineTacticsPackage(), new Fixture());
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentHugujiuRewardId, "虎骨酒")
                    { TacticSkillId = StandardShanheCampaignCatalog.CurrentHugujiuSkillId, Rarity = 2 },
                new(StandardShanheCampaignCatalog.CurrentHugujiu2RewardId, "虎骨酒Ⅱ")
                    { TacticSkillId = StandardShanheCampaignCatalog.CurrentHugujiu2SkillId, Rarity = 4 },
                new("supplies", "实体酒杀补给", StartingCards: Enumerable.Range(0, 10)
                    .SelectMany(i => new[] { new ShanheStartingCard(CardKind.Alcohol, Suit.Spade, i + 1),
                        new ShanheStartingCard(CardKind.Slash, Suit.Club, i + 1) }).ToArray())
            ];
            var definition = new ShanheCampaignDefinition("fixture:paid-wine", "真实酒次数小局", [Owner], ["supply"],
                [new("supply", "实体补给", ShanheNodeKind.Event, ["shop", "battle"],
                     Choices: [new("take-supplies", "领取实体补给", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-base", "虎骨酒", 300, RewardId: rewards[0].Id, LeavesNode: false),
                     new("buy-two", "虎骨酒Ⅱ", 700, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "固定无手牌守军", ShanheNodeKind.Battle, [],
                    [new("guard", "固定守军", [new(Peer) { StartingHand = [] }])])], rewards, StartingGold: 3000);
            var session = ShanheCampaignSession.Create(definition, new(83, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => registry);
            var supplied = session.Submit(new MoveShanheNodeCommand("supply", session.Revision));
            Need(supplied.Accepted, supplied.Error ?? "Wine fixture failed to enter supplies.");
            var acquired = session.Submit(new ChooseShanheNodeOptionCommand("take-supplies", null, session.Revision));
            Need(acquired.Accepted, acquired.Error ?? "Wine fixture failed to acquire its actual supply reward.");
            var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
            Need(entered.Accepted, entered.Error ?? "Wine fixture failed to enter its genuine battle.");
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-paid-wine", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"quota","query":"slashLimit","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "activations":[{"id":"hurt-four","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":4}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "实体酒次数小局", description = "真实酒、杀、濒死与实际回合" })
                }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "酒杀使用者", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "无普通摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图酒次数", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

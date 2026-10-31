using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheQianlongChecks
{
    private const string Owner = "fixture:qianlong-owner", Peer = "fixture:qianlong-peer";
    private const string Driver = "fixture:qianlong-driver", Quiet = "fixture:qianlong-quiet";
    private const string LearnedA = "fixture:qianlong-learned-a", LearnedB = "fixture:qianlong-learned-b", LearnedC = "fixture:qianlong-learned-c";
    private const string UnlockOne = "fixture:qianlong-unlock-one", UnlockTwo = "fixture:qianlong-unlock-two";
    private const string Deck = "fixture:qianlong-deck", Ordinary = "team:qianlong-ordinary";
    private const string Supplies = "qianlong-supplies", Filler = "qianlong-filler";

    public static void LivePurchasesReplacementAndUnlockedSlotsRecomputeWithoutResettingPaidSlashes()
    {
        var s = Scene.Start(persistent: true, carried: false, initialSkill: false, rank: 1, slots: 2);
        Need(Limit(s.Game, 0) == 1 && s.Player().Skills!.All(skill => skill.ContentId != StandardShanheQianlongPackage.SkillId),
            "Registering unowned potential stock does not confer a Slash bonus.");
        s.Slash(); s.RejectSlash();
        s.Buy(StandardShanheCampaignCatalog.CurrentQianlongRewardId);
        Need(Limit(s.Game, 0) == 5 && s.HasSlash && s.PaidSlashes == 1 && s.Market().ExtraSkillSlots == 2,
            "Buying the actual 200-coin tactic immediately uses the two genuinely empty slots without resetting the already-paid first Slash.");
        s.Slash(); s.Slash();
        s.Buy("learn-a");
        Need(Limit(s.Game, 0) == 3 && s.PaidSlashes == 3 && !s.HasSlash && s.Market().ExtraSkillIds.SequenceEqual([LearnedA]),
            "Buying the first actual skill consumes one slot immediately; three prior Slashes already exhaust the reduced limit.");
        s.RejectSlash();
        s.Buy("learn-b");
        Need(Limit(s.Game, 0) == 1 && !s.HasSlash && s.Market().ExtraSkillIds.SequenceEqual([LearnedA, LearnedB]),
            "A second purchase fills both slots and removes the complete potential bonus.");
        s.Buy("learn-c", LearnedA);
        Need(Limit(s.Game, 0) == 1 && s.PaidSlashes == 3 && !s.HasSlash &&
             s.Market().ExtraSkillIds.SequenceEqual([LearnedB, LearnedC]) &&
             s.Player().Skills!.All(skill => skill.ContentId != LearnedA) &&
             s.Player().Skills!.Any(skill => skill.ContentId == LearnedC),
            "A full-slot native replacement removes its real previous skill while leaving occupancy and paid Slash count unchanged.");
        s.Buy("unlock-one");
        Need(s.Market().ExtraSkillSlots == 3 && Limit(s.Game, 0) == 3 && !s.HasSlash && s.PaidSlashes == 3,
            "One newly unlocked empty slot adds two uses but cannot erase the three uses already paid in this phase.");
        s.RejectSlash();
        s.Buy("unlock-two");
        Need(s.Market().ExtraSkillSlots == 4 && Limit(s.Game, 0) == 5 && s.HasSlash && s.PaidSlashes == 3,
            "A second new empty slot restores only the two genuinely remaining uses in the original phase.");
        s.Slash(); s.Slash(); s.RejectSlash();
        Need(s.PaidSlashes == 5 && !s.HasSlash && s.Market().ExtraSkillIds.Count == 2 &&
             s.Events<ShanheMarketPurchaseCommittedEvent>().Length == 6 &&
             s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 6,
            "Six real paid purchases finish on the original market frames; the same native Play phase still ends at five paid Slashes.");
    }

    private static int Limit(GameEngine game, int seat)
    {
        var players = (IReadOnlyList<CharacterState>)global::FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(game)!;
        var evaluation = (RuleQueryEvaluation)global::FixtureRuntimeAccess.Engine
            .GetMethod("CalculateSlashUseLimit", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(game, [players[seat], null, null])!;
        Need(evaluation.Value is FiniteRuleQueryValue, "Potential fixture must retain a finite native Slash limit.");
        return ((FiniteRuleQueryValue)evaluation.Value).Value;
    }

    private static ContentRegistry Registry(bool ordinary = false) => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheRankPrivilegePackage(),
        new StandardShanheQianlongPackage(), new Fixture(ordinary));
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ShanheBattleSetup setup)
    {
        public GameEngine Game { get; } = game;
        public ShanheBattleSetup Setup { get; } = setup;
        public PlayerSnapshot Player() => Game.CreateSnapshot(0).Players[0];
        public ShanheCampaignSnapshot Market() => Game.CreateShanheMarketSnapshot(0)!;
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public int PaidSlashes => Events<CardUseDeclaredEvent>().Count(fact => fact.SourceSeat == 0 && fact.CardKind == CardKind.Slash);
        public bool HasSlash => Game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash);
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A native Qianlong command was rejected."); }
        public void Play()
        {
            for (var step = 0; step < 180; step++)
            {
                if (Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(Game.State.Winner == Winner.None, "The potential fixture ended before its native Play boundary.");
                if (Game.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "no-nullification" or "let-die");
                    Need(choice is not null, "Unexpected potential decision: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The potential fixture lost its bounded native Play boundary.");
        }
        public void Slash()
        {
            var card = Player().Hand.First(item => item.Kind == CardKind.Slash);
            var action = Game.GetHumanLegalActions().First(item => item.CardId == card.Id &&
                item.ConversionSource is null && item.Kind == LegalActionKind.Slash && item.TargetSeats.SequenceEqual([1]));
            var before = Events<DamageAppliedEvent>().Length;
            Accept(new PlayCardCommand(0, card.Id, [1], Game.Revision, Game.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
            Need(Events<DamageAppliedEvent>().Skip(before).ToArray() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 1 }] &&
                 Game.CardMovements.Count(move => move.CardId == card.Id && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                 Game.CardMovements.Count(move => move.CardId == card.Id && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
                "Each allowed Slash pays one original hand entity, deals its genuine native damage and finishes that same entity exactly once.");
        }
        public void RejectSlash()
        {
            var card = Player().Hand.First(item => item.Kind == CardKind.Slash);
            var revision = Game.Revision; var moves = Game.CardMovements.Count; var paid = PaidSlashes;
            Need(!Game.Submit(new PlayCardCommand(0, card.Id, [1], revision, Game.PendingDecision!.PromptId, CardKind.Slash)).Accepted &&
                 Game.Revision == revision && Game.CardMovements.Count == moves && PaidSlashes == paid &&
                 Player().Hand.Any(item => item.Id == card.Id),
                "At the native limit an additional physical Slash cannot spend its material, revision or use count.");
        }
        public void Buy(string rewardId, string? replaceSkill = null)
        {
            var item = Market().ShopItems.Single(choice => choice.Reward?.Id == rewardId);
            var gold = Market().Gold; var paid = PaidSlashes;
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, replaceSkill, Game.Revision)); Play();
            Need(Market().Gold == gold - item.GoldCost && PaidSlashes == paid,
                "The real current-price purchase pays its quote once without resetting the original phase's Slash ledger.");
        }

        public static Scene Start(bool persistent, bool carried, bool initialSkill, int rank, int slots)
        {
            var registry = Registry();
            ShanheRewardDefinition[] rewards =
            [
                new(Supplies, "实际起手配给", SkillId: initialSkill ? LearnedA : null,
                    StartingCards: Enumerable.Range(1, 12).Select(n => new ShanheStartingCard(CardKind.Slash, Suit.Club, n)).ToArray()),
                new(StandardShanheCampaignCatalog.CurrentQianlongRewardId, "潜龙") { Rarity = 1, TacticSkillId = StandardShanheQianlongPackage.SkillId },
                new("learn-a", "实际技能甲", SkillId: LearnedA) { Rarity = 1 },
                new("learn-b", "实际技能乙", SkillId: LearnedB) { Rarity = 1 },
                new("learn-c", "实际技能丙", SkillId: LearnedC) { Rarity = 1 },
                new("unlock-one", "解锁空技能槽", AddSkillSlots: 1) { Rarity = 1, TacticSkillId = UnlockOne },
                new("unlock-two", "再解锁空技能槽", AddSkillSlots: 1) { Rarity = 1, TacticSkillId = UnlockTwo },
                new(Filler, "已携带经济战法") { Rarity = 1, CampaignEconomy = new(ReplacedEquipmentGold: 1) }
            ];
            var definition = new ShanheCampaignDefinition("fixture:qianlong", "潜龙真实空槽", [Owner], ["prepare"],
                [new("prepare", "征程配给", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take-supplies", "领取实际配给", RewardId: Supplies)]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-qianlong", "潜龙", 200, RewardId: StandardShanheCampaignCatalog.CurrentQianlongRewardId, LeavesNode: false),
                     new("buy-a", "技能甲", 100, RewardId: "learn-a", LeavesNode: false),
                     new("buy-b", "技能乙", 100, RewardId: "learn-b", LeavesNode: false),
                     new("buy-c", "替换技能", 100, RewardId: "learn-c", LeavesNode: false),
                     new("buy-slot", "空槽", 100, RewardId: "unlock-one", LeavesNode: false),
                     new("buy-slot-two", "空槽二", 100, RewardId: "unlock-two", LeavesNode: false)]),
                 new("battle", "无手牌守军", ShanheNodeKind.Battle, [], [new("guard", "实际双人阵", [new(Peer) { StartingHand = [] }])])],
                rewards, StartingGold: 5000, ExtraSkillSlots: slots);
            var campaign = ShanheCampaignSession.Create(definition, new(83, Owner)
            {
                UseCodexRewardPool = true, PersistentMarket = persistent, PlayerRank = rank,
                EquippedTacticIds = carried ? [StandardShanheCampaignCatalog.CurrentQianlongRewardId, Filler] : []
            }, _ => registry);
            var prepared = campaign.Submit(new MoveShanheNodeCommand("prepare", campaign.Revision)); Need(prepared.Accepted, prepared.Error ?? "Preparation failed.");
            var supplied = campaign.Submit(new ChooseShanheNodeOptionCommand("take-supplies", null, campaign.Revision)); Need(supplied.Accepted, supplied.Error ?? "Actual supplies failed.");
            var entered = campaign.Submit(new MoveShanheNodeCommand("battle", campaign.Revision)); Need(entered.Accepted, entered.Error ?? "Actual battle entry failed.");
            var setup = campaign.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game, setup); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool ordinary) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:qianlong-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "activations":[{"id":"grant-off-ledger","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{LearnedB}}}"]}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "潜龙原生流程", description = "实际槽位与实体杀" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            foreach (var id in new[] { LearnedA, LearnedB, LearnedC, UnlockOne, UnlockTwo })
            {
                var extra = SkillProgramCatalog.Load($$$"""
                    {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{id}}}","revision":1,"modifiers":[
                      {"id":"actual-printing","query":"attackRange","operation":"add","value":1,"priority":0}]}]}
                    """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [id] = new { name = "实际技能或槽位来源", description = "实际注册的独立来源" } } }));
                builder.AddSkill(new(id, id, id) { Program = extra.Programs[id], ProgramPresentation = extra.Presentations[id],
                    CampaignTacticAcquisition = id == UnlockOne || id == UnlockTwo ? new(id, Repeatable: false) : null });
            }
            builder.AddGeneral(new(Owner, "潜龙持有者", "supporter", Driver, "shu", 6)
                { AdditionalSkillIds = ordinary ? [StandardShanheQianlongPackage.SkillId] : [] });
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30)
                { AdditionalSkillIds = [StandardShanheQianlongPackage.SkillId] });
            builder.AddDeck(new(Deck, "原生实体牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "原生潜龙双人阵", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
            foreach (var id in new[] { Peer + "-2", Peer + "-3" })
                builder.AddGeneral(new(id, "普通模式潜龙持有者", "supporter", Quiet, "wei", 30)
                    { AdditionalSkillIds = [StandardShanheQianlongPackage.SkillId] });
            builder.AddMode(new(Ordinary, "普通四人隔离阵", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer, Peer + "-2", Peer + "-3"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

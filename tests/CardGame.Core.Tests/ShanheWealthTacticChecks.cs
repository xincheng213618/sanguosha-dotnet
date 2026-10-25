using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWealthTacticChecks
{
    private const string Owner = "fixture:wealth-owner", Driver = "fixture:wealth-driver";
    private const string Quiet = "fixture:wealth-quiet", Deck = "fixture:wealth-deck";
    private static readonly (int Source, int Threshold, string Skill)[] Wealth =
    [
        (2178, 300, StandardShanheWealthPackage.WealthHandLimitSkillId),
        (2179, 500, StandardShanheWealthPackage.WealthInitialHandSkillId),
        (2180, 700, StandardShanheWealthPackage.WealthDrawSkillId),
        (2181, 1000, StandardShanheWealthPackage.WealthDamageSkillId)
    ];
    private static readonly Lazy<ContentRegistry> BattleRegistry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheWealthPackage(),
        new StandardShanheLegendaryTacticsPackage(), new Fixture()));

    public static void StrictBalancesAndActualOwnershipQualifyNativeOpeningEffects()
    {
        var unowned = Create(2000, []);
        Enter(unowned, "first");
        Need(unowned.PendingBattleSetup!.TacticSkillIds.Count == 0,
            "A high campaign balance cannot grant a wealth effect that the player does not actually own.");
        foreach (var (source, threshold, skill) in Wealth)
        {
            var reward = "tactic-economy-" + source;
            var exact = Create(threshold, [reward]); Enter(exact, "first");
            Need(exact.PendingBattleSetup!.TacticSkillIds.Count == 0 && exact.Snapshot.TacticIds.SequenceEqual([reward]),
                "Owning the actual source at its exact balance threshold keeps inventory but grants no battle program.");
            var above = Create(threshold + 1, [reward]); Enter(above, "first");
            Need(above.PendingBattleSetup!.TacticSkillIds.SequenceEqual([skill]) &&
                above.Snapshot.Bonuses == new ShanheTacticBonuses() && above.Snapshot.ExtraSkillIds.Count == 0,
                "One coin above the strict threshold grants only that source's battle program, without permanent bonuses or an extra skill slot.");
            var battle = Scene.Start(above.PendingBattleSetup!);
            battle.AssertOpening(initial: source == 2179 ? 4 : 2, draw: source == 2180 ? 4 : 2,
                handLimit: source == 2178 ? 14 : 12);
        }
    }

    public static void QualifiedDamageIncludesCardsDirectSkillsAndRealReflection()
    {
        var session = Create(1001, ["tactic-economy-2181"], withKit: true);
        Enter(session, "kit");
        Accept(session, new ChooseShanheNodeOptionCommand("take", null, session.Revision));
        Enter(session, "first");
        var s = Scene.Start(session.PendingBattleSetup!);
        s.Card(CardKind.Slash, [1]);
        s.Use("hit", [1]);
        s.Use("incoming", [2]);
        Need(s.Facts<DamageAppliedEvent>().Select(hit => (hit.SourceSeat, hit.TargetSeat, hit.Amount))
                .SequenceEqual([(0, 1, 2), (0, 1, 2), (2, 0, 1), (0, 2, 2)]),
            "The held, qualified source raises genuine physical-card, direct skill and production armor-reflection damage once; incoming enemy damage stays one.");
        s.Card(CardKind.IronChain, [1, 2]);
        s.Card(CardKind.FireSlash, [1]);
        var wealthModifiers = s.Facts<ProgramCardDamageModifiedEvent>()
            .Where(fact => fact.Source.SkillId == StandardShanheWealthPackage.WealthDamageSkillId).ToArray();
        Need(s.Facts<DamageAppliedEvent>().TakeLast(2).Select(hit => (hit.TargetSeat, hit.Amount, hit.Nature))
                .SequenceEqual([(1, 2, DamageNature.Fire), (2, 3, DamageNature.Fire)]) &&
            wealthModifiers.Length == 5 && wealthModifiers.All(fact => fact.ModifiedAmount == fact.BaseAmount + 1) &&
            wealthModifiers.Select(fact => (fact.TargetSeat, fact.BaseAmount, fact.ModifiedAmount))
                .SequenceEqual([(1, 1, 2), (1, 1, 2), (2, 1, 2), (1, 1, 2), (2, 2, 3)]) &&
            s.Facts<ChainedDamagePropagatedEvent>() is [{ SourceSeat: 0, FromSeat: 1, TargetSeat: 2, Amount: 2, Nature: DamageNature.Fire }],
            "The shared damage-source additional-damage layer adds exactly one at every genuine damage occurrence: the physical Fire Slash deals two, propagates two, then adds one for the next recipient. This follows all-source damage semantics rather than a card-only bonus. Actual facts: " +
            JsonSerializer.Serialize(new
            {
                damage = s.Facts<DamageAppliedEvent>().Select(hit => new { hit.SourceSeat, hit.TargetSeat, hit.Amount, hit.Nature, hit.SourceCardUseResolutionId }),
                modifiers = s.Facts<ProgramCardDamageModifiedEvent>().Where(fact => fact.Source.SkillId == StandardShanheWealthPackage.WealthDamageSkillId)
                    .Select(fact => new { fact.TargetSeat, fact.BaseAmount, fact.ModifiedAmount }),
                chain = s.Facts<ChainedDamagePropagatedEvent>()
            }));
    }

    public static void SpendingRequalifiesAllHeldSourcesForTheNextEncounter()
    {
        var session = Create(1201, Wealth.Select(item => "tactic-economy-" + item.Source).ToArray());
        Enter(session, "first");
        Need(session.PendingBattleSetup!.TacticSkillIds.ToHashSet().SetEquals(Wealth.Select(item => item.Skill)),
            "The first actual encounter receives all four separately held wealth programs while every threshold is met.");
        var first = Scene.Start(session.PendingBattleSetup!);
        first.AssertOpening(4, 4, 14);
        first.Finish();
        Need(session.CompleteBattle(first.Game, session.Revision).Accepted,
            "A real completed encounter must advance the campaign to its actual shop.");
        Enter(session, "shop");
        Accept(session, new ChooseShanheNodeOptionCommand("spend", null, session.Revision));
        Need(session.Snapshot.Gold == 300 && session.Snapshot.TacticIds.Count == 4,
            "A real nine-hundred-and-one-coin purchase lowers the balance to the lowest exact threshold while preserving all source ownership.");
        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Enter(session, "second");
        Need(session.PendingBattleSetup!.TacticSkillIds.Count == 0 && session.Snapshot.Bonuses == new ShanheTacticBonuses(),
            "The next encounter recomputes its own eligibility and cannot retain bonuses from the previous richer encounter.");
        Scene.Start(session.PendingBattleSetup!).AssertOpening(2, 2, 12, extraCards: 2);
    }

    private static ShanheCampaignSession Create(int gold, string[] held, bool withKit = false)
    {
        var rewards = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => Wealth.Any(item => item.Source == source.SourceId))
            .Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
                { CampaignEconomy = source.Effect, Description = source.Description }).ToArray();
        Need(rewards.Length == Wealth.Length, "All four real wealth sources must be in the current source catalog.");
        var kit = new ShanheRewardDefinition("kit", "固定实体军需", StartingCards:
            [new(CardKind.FireSlash, Suit.Spade, 7), new(CardKind.IronChain, Suit.Club, 10)]);
        ShanheNodeDefinition Battle(string id, string[] next) => new(id, "真实战斗", ShanheNodeKind.Battle, next,
            [new("guard", "固定守军", Enumerable.Range(1, 3).Select(seat =>
                new ShanheCombatantDefinition(Peer(seat)) { StartingHand = [] }).ToArray())]);
        var nodes = new List<ShanheNodeDefinition>
        {
            Battle("first", ["shop"]),
            new("shop", "真实消费", ShanheNodeKind.Shop, ["second"], Choices:
                [new("spend", "购买军需", 901, RewardId: kit.Id, LeavesNode: false)]),
            Battle("second", [])
        };
        if (withKit) nodes.Insert(0, new("kit", "领用军需", ShanheNodeKind.Event, ["first"],
            Choices: [new("take", "领用", RewardId: kit.Id)]));
        var definition = new ShanheCampaignDefinition("fixture:wealth", "守财奴真实阈值", [Owner],
            [withKit ? "kit" : "first"], nodes, [.. rewards, kit], StartingGold: gold);
        var probed = false;
        var session = ShanheCampaignSession.Create(definition, new(83, Owner) { EquippedTacticIds = held }, setup =>
        {
            if (!probed)
            {
                Need(setup.TacticSkillIds.ToHashSet().SetEquals(Wealth.Select(item => item.Skill)),
                    "The registry-only constructor probe discovers every potentially qualified program even when the current balance cannot activate it.");
                probed = true;
            }
            return BattleRegistry.Value;
        });
        return session;
    }

    private static void Enter(ShanheCampaignSession session, string node) =>
        Accept(session, new MoveShanheNodeCommand(node, session.Revision));
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real wealth campaign command."); }
    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
    private static string Peer(int seat) => "fixture:wealth-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public T[] Facts<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private PlayerSnapshot OwnerView => Game.CreateSnapshot(0).Players[0];
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real wealth battle command."); }
        private void Play()
        {
            for (var step = 0; step < 250; step++)
            {
                if (Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || Game.State.Status == EngineStatus.Completed) return;
                if (Game.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, Game.Revision));
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded wealth fixture play boundary.");
        }
        public void AssertOpening(int initial, int draw, int handLimit, int extraCards = 0) => Need(
            Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal) == initial + extraCards &&
            Game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal) == initial &&
            Game.CardMovements.Count(move => move.From == CardLocation.OutsideGame && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal) == extraCards &&
            Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == draw &&
            OwnerView.HandLimit == handLimit && OwnerView.HandCount == initial + draw + extraCards,
            "Actual initial entities, normal draw-phase entities and the current native hand limit use exactly the active wealth programs. Actual facts: " +
            JsonSerializer.Serialize(new
            {
                OwnerView.Hp, OwnerView.MaxHp, OwnerView.HandCount, OwnerView.HandLimit,
                initial = Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal),
                initialFromDrawPile = Game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal),
                initialFromOutside = Game.CardMovements.Count(move => move.From == CardLocation.OutsideGame && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.InitialDeal),
                normalDraw = Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw)
            }));
        public void Use(string activation, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, Game.Revision, Game.PendingDecision!.PromptId)); Play(); }
        public void Card(CardKind kind, int[] targets)
        {
            var card = OwnerView.Hand.First(card => card.Kind == kind);
            var action = Game.GetHumanLegalActions().First(action => action.CardId == card.Id && action.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, targets, Game.Revision, Game.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Finish()
        {
            foreach (var seat in Enumerable.Range(1, 3)) Use("defeat", [seat]);
            Need(Game.State.Status == EngineStatus.Completed, "Real HP-loss instructions must complete all enemy deaths before campaign acceptance.");
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, BattleRegistry.Value);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wealth", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"slash-limit","query":"slashLimit","operation":"add","value":10,"priority":0}],"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                    {"id":"defeat","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":20}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "守财奴实战驱动", description = "原生卡牌、直接伤害与战斗结算" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "守财者", "supporter", Driver, "qun", 12,
                [StandardShanheCampaignCatalog.CurrentJingjijiaSkillId]));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定守军", "supporter", Quiet, "wei", 16));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 2, [])
                { PhysicalCards = Enumerable.Range(0, 128).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "守财奴真实战斗", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBuildTacticChecks
{
    private const string Owner = "fixture:build-tactic-owner", Driver = "fixture:build-tactic-driver";
    private const string Quiet = "fixture:build-tactic-quiet", Deck = "fixture:build-tactic-deck";
    private const string Cuixue = StandardShanheCampaignCatalog.CurrentCuixue2SkillId;
    private const string Sougua = StandardShanheCampaignCatalog.CurrentSougua3SkillId;
    private const string Fenjin = StandardShanheCampaignCatalog.CurrentFenjinSkillId;
    private const string Yuanzhu = StandardShanheCampaignCatalog.CurrentYuanzhu3SkillId;

    public static void FirstRoundSlashDrawAndReflectionKillsPayNativeRecovery()
    {
        var s = Scene.Start(logistics: false, hp: 4,
            [Cuixue, Sougua, StandardShanheCampaignCatalog.CurrentTouxiSkillId,
                StandardShanheCampaignCatalog.CurrentJingjijiaSkillId]);
        var slash = s.V().Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var fire = s.V().Hand.Single(card => card.Kind == CardKind.FireSlash).Id;
        var thunder = s.V().Hand.Single(card => card.Kind == CardKind.ThunderSlash).Id;
        var initialHand = s.V().HandCount;
        s.Slash(slash, 1);
        Need(s.DrawCount(Cuixue) == 2 && s.V().HandCount == initialHand + 1,
            "The first genuinely damaging Slash pays one native Draw2, after its real entity leaves hand.");
        s.Slash(fire, 1);
        Need(!s.G.CreateSnapshot(0).Players[1].IsAlive && s.V().Hp == 8 && s.DrawCount(Cuixue) == 2 &&
            s.E<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 4) == 1,
            "A second elemental Slash shares the same round draw quota; its real kill instead pays Recover4.");

        s.Use("lose-six", []);
        Need(s.V().Hp == 2, "A real six-HP loss prepares the reflection sustain boundary without manufacturing damage.");
        s.Use("incoming-one", [2]);
        Need(s.V().Hp == 5 && !s.G.CreateSnapshot(0).Players[2].IsAlive &&
            s.E<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 0 && fact.TargetSeat == 2 && fact.Amount == 1 && fact.SourceCardUseResolutionId is null) &&
            s.E<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 4) == 2 && s.DrawCount(Cuixue) == 2,
            "Native received damage reflects once, that skill damage really kills its source, and the killer recovers four without pretending the reflection was a Slash.");
        s.NextOwnPlay();
        s.Slash(thunder, 3);
        Need(s.DrawCount(Cuixue) == 4 && s.E<SkillUsageConsumedEvent>().Count(fact => fact.SkillId == Cuixue && fact.SkillOwnerSeat == 0) == 2,
            "A later genuine round restores the shared Slash damage reward once.");
    }

    public static void LogisticsKeepStartHpThresholdDrawPhaseAndEndingPaymentsSeparate()
    {
        var s = Scene.Start(logistics: true, hp: 3,
            [Fenjin, Yuanzhu, StandardShanheCampaignCatalog.CurrentMuniuLiumaSkillId,
                StandardShanheCampaignCatalog.CurrentJingjijiaSkillId]);
        Need(s.V().Hp == 2 && s.V().HandLimit == 1 && s.V().HandCount == 6 &&
            s.DrawCount(Fenjin) == 2 && s.NormalDrawCount() == 4 && !s.E<DamageAppliedEvent>().Any(),
            "At three HP, Fenjin really loses one HP and draws two; Muniu only raises the normal phase draw from two to four and reduces capacity by one, without triggering reflection.");
        s.NextOwnPlay();
        Need(s.V().Hp == 2 && s.V().HandLimit == 1 && s.V().HandCount == 8 &&
            s.DrawCount(Fenjin) == 2 && s.DrawCount(Yuanzhu) == 3 && s.NormalDrawCount() == 8 &&
            s.E<ProgramSkillHpLostEvent>().Count(fact => fact.SkillId == Fenjin && fact.Amount == 1) == 1,
            "The real discard leaves one card, own ending draws three, enemy endings pay nothing, and the next four-card draw occurs with Fenjin disabled at exactly two HP.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private static string Peer(int seat) => "fixture:build-tactic-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(fact => fact.Payload).OfType<T>().ToArray();
        public int DrawCount(string skillId) => G.CardMovements.Count(move => move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program." + skillId + ".Draw");
        public int NormalDrawCount() => G.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw);
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native build-tactic command."); }
        private void Until(Func<bool> condition)
        {
            for (var step = 0; step < 300; step++)
            {
                if (condition()) return;
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, G.Revision));
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native build-tactic boundary.");
        }
        private void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Slash(int cardId, int target)
        {
            var action = G.GetHumanLegalActions().Single(candidate => candidate.CardId == cardId &&
                candidate.TargetSeats.SequenceEqual([target]) && candidate.Kind == LegalActionKind.Slash);
            Accept(new PlayCardCommand(0, cardId, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Use(string activation, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, G.Revision, P!.PromptId)); Play(); }
        public void NextOwnPlay()
        {
            var turn = G.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Until(() => G.State.TurnNumber > turn && P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public static Scene Start(bool logistics, int hp, string[] tactics)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLegendaryTacticsPackage(),
                new StandardShanheBuildTacticsPackage(), new Fixture(logistics));
            var setup = new ShanheBattleSetup(83, 1, "fixture:build-tactics", Owner, hp, 0, 0, 0, 0, [],
                logistics ? [] : [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.FireSlash, Suit.Spade, 8), new(CardKind.ThunderSlash, Suit.Spade, 9)], [],
                [new(Peer(1), Hp: 2) { StartingHand = [] }, new(Peer(2), Hp: 1) { StartingHand = [] }, new(Peer(3)) { StartingHand = [] }],
                [], new string('b', 64)) { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool logistics) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-build-tactics", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"lose-six","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":6}]},
                    {"id":"incoming-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "原生构筑驱动", description = "真实伤害、真实失去体力与真实阶段" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "构筑使用者", "supporter", Driver, "qun", 10));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定敌军", "supporter", Quiet, "wei", 16));
            builder.AddDeck(new(Deck, "固定实体闪", 0, logistics ? 2 : 0, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "战法构筑实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}

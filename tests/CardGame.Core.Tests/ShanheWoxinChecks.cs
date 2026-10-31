using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheWoxinChecks
{
    private const string Owner = "fixture:woxin-owner", Driver = "fixture:woxin-driver", Pulse = "fixture:woxin-pulse";
    private const string Quiet = "fixture:woxin-quiet", Shield = "fixture:woxin-shield", Deck = "fixture:woxin-deck";
    private static string Peer(int seat) => "fixture:woxin-peer-" + seat;

    public static void ReflectionsConsumeNewCreditsWithinTheOriginalOutsideTurn()
    {
        var s = Scene.Start(true); s.NextOwnPlay();
        Need(s.E<ShanheWoxinCreditEarnedEvent>().Length == 2 &&
            s.E<ShanheWoxinReservationConsumedEvent>().Select(e => e.AppliedAmount).SequenceEqual([2, 2]) &&
            s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0 && e.TargetSeat == 1).Select(e => e.Amount).SequenceEqual([2, 2]) &&
            s.E<ShanheWoxinCreditReservedEvent>().Select(e => e.CreditOrdinal).SequenceEqual([1, 2]),
            "Each real outside-turn hit earns one credit before its native reflection: both nested skill hits receive and consume exactly one distinct credit.");
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Woxin native command rejected."); }
        private void Play()
        {
            for (var step = 0; step < 200; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded Woxin Play boundary.");
        }
        public void Use(string id, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId)); Play(); }
        public void NextOwnPlay()
        { Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId)); Play(); }
        public static Scene Start(bool reflection)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheWoxinPackage(),
                new StandardShanheLegendaryTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:woxin", Owner, 30, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Spade, 7)], [],
                [new(Peer(1)) { StartingHand = [] }, new(Peer(2)) { StartingHand = [] }, new(Peer(3)) { StartingHand = [] }],
                [], new string('b', 64)) { TacticSkillIds = reflection
                    ? [StandardShanheWoxinPackage.SkillId, StandardShanheCampaignCatalog.CurrentJingjijiaSkillId]
                    : [StandardShanheWoxinPackage.SkillId] };
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, PlayerCount = 4,
                HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-woxin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"add","value":10,"priority":0}],"activations":[
                    {"id":"strike","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                    {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{Pulse}}","revision":1,"triggers":[{"id":"two-hits","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"selectRandomPublicTeamTarget","target":"owner","targetKind":"publicTeamEnemies","allowEmpty":true},{"op":"damageParticipants","target":"selectedTargets","amount":1},{"op":"damageParticipants","target":"selectedTargets","amount":2}]}]},
                  {"id":"{{Shield}}","revision":1,"triggers":[{"id":"prevent","window":"beforeDamageApplied","subject":"damageTarget","optional":false,"effects":[{"op":"preventCurrentDamage","target":"owner"}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, Pulse, Shield }.ToDictionary(id => id, _ => new { name = "卧薪原生流程", description = "真实回合、伤害、预防与反伤" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "卧薪持有者", "supporter", Driver, "qun", 30));
            builder.AddGeneral(new(Peer(1), "双次攻击者", "supporter", Quiet, "wei", 30, [Pulse]));
            builder.AddGeneral(new(Peer(2), "普通目标", "supporter", Quiet, "wei", 30));
            builder.AddGeneral(new(Peer(3), "防止目标", "supporter", Quiet, "wei", 30, [Shield]));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "卧薪实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}

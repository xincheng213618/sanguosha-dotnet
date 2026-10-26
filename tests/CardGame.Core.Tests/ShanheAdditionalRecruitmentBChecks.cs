using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheAdditionalRecruitmentBChecks
{
    private const string Driver = "fixture:recruitment-b-driver";
    private const string EquipmentLoss = "fixture:recruitment-b-equipment-loss";
    private const string General = "fixture:recruitment-b-owner";
    private const string Npc = "fixture:recruitment-b-npc";
    private const string Deck = "fixture:recruitment-b-deck";

    public static void ShunxueRefillsAtPlayAndEndingWithActualHandLimit()
    {
        var scene = new Scene(StandardShanheAdditionalRecruitmentBPackage.ShunxueSkillId);
        scene.Play();
        Need(scene.G.CreateSnapshot(0).Players[0].HandCount == 3 && scene.Refills().Length == 1,
            "An empty opening followed by the ordinary two-card draw is refilled to the actual three-card hand limit at Play start.");
        var discard = scene.G.CreateSnapshot(0).Players[0].Hand.Take(2).Select(card => card.Id).ToArray();
        scene.Use("pay", discard);
        scene.Play();
        Need(scene.G.CreateSnapshot(0).Players[0].HandCount == 1, "The active skill pays two actual hand entities before the ending refill.");
        scene.Cold();
        scene.Accept(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId));
        scene.Reach(() => scene.Refills().Length == 2);
        Need(scene.G.CreateSnapshot(0).Players[0].HandCount == 3 &&
            scene.G.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value ==
                "skill-program." + StandardShanheAdditionalRecruitmentBPackage.ShunxueSkillId + ".Draw") == 3,
            "Ending refills only the missing two cards; both skill windows use real draws without replacing the normal Draw phase.");
        scene.Cold();
    }

    public static void DuanziAbolishesAllEnemySlotsAndKeepsAlliedEquipmentCold()
    {
        var scene = new Scene(StandardShanheAdditionalRecruitmentBPackage.DuanziSkillId);
        scene.Play();
        for (var seat = 1; seat < 4; seat++) { scene.Use("equip-" + seat, targets: [seat]); scene.Play(); }
        var before = scene.G.CreateSnapshot(0);
        Need(before.Players.Skip(1).All(player => player.Equipment.Count == 1),
            "Both enemies and the ally hold actual installed weapons before the death trigger.");
        var enemyWeapons = before.Players.Where(player => player.TeamId == ShanheBattleSetup.EnemyTeamId)
            .SelectMany(player => player.Equipment).Select(card => card.Id).ToHashSet();
        var enemyHands = before.Players.Where(player => player.TeamId == ShanheBattleSetup.EnemyTeamId)
            .ToDictionary(player => player.Seat, player => player.HandCount);
        scene.Cold();
        scene.Use("die");
        scene.Reach(() => scene.G.Events.Any(item => item.Payload is ProgramBindingResolvedEvent
            { SkillId: StandardShanheAdditionalRecruitmentBPackage.DuanziSkillId, Completed: true }));
        var after = scene.G.CreateSnapshot(0);
        var enemies = after.Players.Where(player => player.TeamId == ShanheBattleSetup.EnemyTeamId).ToArray();
        Need(enemies.Length == 2 && enemies.All(player => player.Equipment.Count == 0 &&
                Enum.GetValues<EquipmentSlot>().All(slot => player.EquipmentSlotCapacities!.GetValueOrDefault(slot, 1) == 0)) &&
            after.Players.Single(player => player.Seat != 0 && player.TeamId == ShanheBattleSetup.PlayerTeamId).Equipment.Count == 1 &&
            scene.G.CardMovements.Count(move => enemyWeapons.Contains(move.CardId) && move.To == CardLocation.DiscardPile) == 2 &&
            enemies.All(player => player.HandCount == enemyHands[player.Seat] + 1) &&
            scene.G.Events.Count(item => item.Payload is ProgramBindingResolvedEvent { SkillId: EquipmentLoss, Completed: true }) == 2,
            "The original dead owner's mandatory Duanzi disables every enemy equipment slot, discards each installed entity once, drains both native loss-trigger children, and preserves the living ally.");
        scene.Cold();
    }

    private sealed class Scene
    {
        private readonly ContentRegistry _registry;
        public GameEngine G { get; private set; }
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;

        public Scene(string skill)
        {
            _registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
                new StandardShanheAdditionalRecruitmentBPackage(), new Fixture());
            var setup = new ShanheBattleSetup(71, 1, "recruitment-b", General, 3, 0, 0, 0, 0,
                [skill], [], [], [new(Npc), new(Npc)], [new(Npc)], new string('b', 64));
            G = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, _registry);
            Accept(new StartGameCommand());
        }

        public ProgramBindingResolvedEvent[] Refills() => G.Events.Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>()
            .Where(item => item.SkillId == StandardShanheAdditionalRecruitmentBPackage.ShunxueSkillId && item.Completed).ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard });
        public void Use(string activation, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) =>
            Accept(new UseProgramSkillCommand(0, Driver, activation, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The companion check command was rejected."); }
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human choice in companion check: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The companion check did not reach the expected boundary.");
        }
        public void Cold()
        {
            var snapshot = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), _registry);
            Need(snapshot == SnapshotJson.Serialize(G.CreateSnapshot(0)), "Cold recovery preserves the actual companion result and state.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-recruitment-b", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                 "modifiers":[{"id":"empty-opening","query":"initialHandSize","operation":"add","value":-4,"priority":0}],"activations":[
                  {"id":"pay","minCards":2,"maxCards":2,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"discardSelected","target":"owner","amount":2}]},
                  {"id":"equip-1","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"placeNamedWeapon","target":"selectedTarget","outputKind":"qinggangSword"}]},
                  {"id":"equip-2","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"placeNamedWeapon","target":"selectedTarget","outputKind":"stoneAxe"}]},
                  {"id":"equip-3","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"placeNamedWeapon","target":"selectedTarget","outputKind":"crossbow"}]},
                  {"id":"die","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"loseHp","target":"owner","amount":3}]}
                ]},{"id":"{{EquipmentLoss}}","revision":1,"triggers":[
                  {"id":"actual-equipment-loss","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],
                   "movementOccurrence":"perBatch","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}
                ]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "队友检查驱动", description = "真实弃牌、装备与死亡" },
                    [EquipmentLoss] = new { name = "失装摸牌", description = "实际失去装备后摸一张牌。" }
                } }));
            builder.AddSkill(new(Driver, "队友检查驱动", "真实弃牌、装备与死亡")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddSkill(new(EquipmentLoss, "失装摸牌", "实际失去装备后摸一张牌。")
            { Program = catalog.Programs[EquipmentLoss], ProgramPresentation = catalog.Presentations[EquipmentLoss] });
            builder.AddGeneral(new(General, "队友检查角色", "", Driver, "qun", 3));
            builder.AddGeneral(new(Npc, "队友检查其他角色", "", EquipmentLoss, "qun", 4));
            builder.AddDeck(new(Deck, "固定闪", 4, 2, [new("standard:dodge", 60)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "队友检查", 4, 4,
                new Dictionary<string, int>(), Deck, GeneralPoolIds: [General, Npc], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}

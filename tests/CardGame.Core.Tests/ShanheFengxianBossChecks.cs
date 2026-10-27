using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheFengxianBossChecks
{
    private const string Owner = "fixture:fengxian-owner", Enemy = "fixture:fengxian-enemy";
    private const string Driver = "fixture:fengxian-driver", Watch = "fixture:fengxian-cost-watch";

    public static void ObservedFirstFormTransitionsWithNativeEquipmentAndRecoversCold()
    {
        var (scene, setup) = Start(xiuluo: false);
        var transition = setup.BossTransition!;
        Need(scene.V(1) is { GeneralId: StandardShanheFengxianBossPackage.FirstId, Hp: 6, MaxHp: 6, HandCount: 7 } &&
            setup.EnemyRoster is [{ DrawBonus: 3, SlashBonus: 1 }] && transition is { Hp: 6, MaxHp: 6, DrawCards: 0 },
            "The actual first-chapter recipe starts as 6/6 Lv Bu with seven opening cards, Draw5 and Slash2.");
        Need(scene.V(1).Skills!.Select(skill => skill.ContentId).Contains("classic:wushuang") &&
            scene.V(1).Skills!.Select(skill => skill.ContentId).Contains("boundary:liyu-current"),
            "The first form owns ordinary two-response Wushuang and the matching current Liyu.");

        // The new collection is immutable before it enters content hashing or replay.
        var callerCards = transition.StartingEquipment!.ToList();
        var frozen = transition with { StartingEquipment = callerCards };
        callerCards.Clear();
        Need(frozen.StartingEquipment!.Count == 2, "A transition detaches its caller-owned equipment list.");
        var mutationRejected = false;
        try { ((IList<ShanheStartingCard>)frozen.StartingEquipment)[0] = new(CardKind.Crossbow); }
        catch (NotSupportedException) { mutationRejected = true; }
        Need(mutationRejected, "The published transition equipment list cannot be edited.");
        RejectEquipment([new(CardKind.Dodge, Suit.Heart, 2)]);
        RejectEquipment([new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.FangtianHalberd, Suit.Heart, 12)]);
        Need(!JsonSerializer.Serialize(new ShanheBossTransition(0, StandardShanheFengxianBossPackage.RageId, 6, 6))
            .Contains(nameof(ShanheBossTransition.StartingEquipment), StringComparison.Ordinal),
            "An absent equipment recipe retains the historical serialized transition shape.");

        scene.Play();
        var player = scene.V();
        var playerCards = player.Hand.Concat(player.Equipment).Select(card => card.Id).Order().ToArray();
        var bossCards = scene.V(1).Hand.Select(card => card.Id).Order().ToArray();
        scene.Accept(new UseProgramSkillCommand(0, Driver, "lethal", [], [1], scene.G.Revision, scene.P!.PromptId));
        scene.Until(() => scene.E<ShanheBossStageStartedEvent>().Length == 1);
        var rage = scene.V(1);
        Need(rage is { GeneralId: StandardShanheFengxianBossPackage.RageId, IsAlive: true, Hp: 6, MaxHp: 6 } &&
            scene.G.CreateSnapshot(0).Winner == Winner.None &&
            scene.E<DamageAppliedEvent>().Single() is { TargetSeat: 1, Amount: 6, RemainingHp: 0 },
            "Real lethal damage replaces the first form with the living 6/6 rage form without settling victory.");
        var skills = rage.Skills!.Select(skill => skill.ContentId).ToHashSet(StringComparer.Ordinal);
        Need(skills.Contains("shanhe:shengui-wushuang") && skills.Contains(StandardShanheFengxianBossPackage.XiuluoId) &&
            skills.Contains(StandardShanheFengxianBossPackage.MashuId) && !skills.Contains("classic:wushuang") &&
            !skills.Contains("boundary:liyu-current"), "The replacement uses exactly the observed rage skill family instead of retaining first-form skills.");
        Need(rage.Equipment.OrderBy(card => card.Rank).Select(card => (card.Kind, card.Suit, card.Rank))
                .SequenceEqual([(CardKind.PurpleGoldCrown, Suit.Diamond, 1), (CardKind.WushuangHalberd, Suit.Diamond, 12)]) &&
            scene.G.CardMovements.Count(move => rage.Equipment.Any(card => card.Id == move.CardId) &&
                move.From == CardLocation.OutsideGame && move.To == CardLocation.Equipment(1)) == 2,
            "The stage installs the real Diamond-A crown and Diamond-Q halberd through two physical equipment movements.");
        Need(scene.V().Hp == player.Hp && scene.V().Hand.Concat(scene.V().Equipment).Select(card => card.Id).Order().SequenceEqual(playerCards) &&
            rage.Hand.Select(card => card.Id).Order().SequenceEqual(bossCards),
            "The stage grant preserves the player's live cards and HP and the boss's existing hand, with no inferred refill.");
        var equipmentIds = rage.Equipment.Select(card => card.Id).Order().ToArray();
        scene.Cold();
        Need(scene.E<ShanheBossStageStartedEvent>().Length == 1 && scene.V(1).Equipment.Select(card => card.Id).Order().SequenceEqual(equipmentIds) &&
            scene.G.CardMovements.Count(move => equipmentIds.Contains(move.CardId) && move.To == CardLocation.Equipment(1)) == 2,
            "Cold command replay retains the single stage grant and the same physical equipment identities.");

        void RejectEquipment(IReadOnlyList<ShanheStartingCard> cards)
        {
            var rejected = false;
            try { _ = ShanheBattleSetup.Freeze(setup with { BossTransition = transition with { StartingEquipment = cards } }); }
            catch (ArgumentException) { rejected = true; }
            Need(rejected, "Stage equipment rejects non-equipment cards and colliding equipment slots.");
        }
    }

    private static (Scene, ShanheBattleSetup) Start(bool xiuluo)
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var node = definition.Nodes.Single(node => node.Id == "huaxiong");
        var recipe = node.Encounters!.Single(encounter => encounter.Id == "huaxiong:wushuang-fengxian");
        Need(node.Kind == ShanheNodeKind.BossBattle && node.ChapterNumber == 1, "The complete two-form recipe belongs to the first-chapter boss pool.");
        var sourceSetup = new ShanheBattleSetup(71, 1, node.Id, "classic:guan-yu", 4, 0, 0, 0, 0,
            [], [], [], recipe.Enemies, [], new string('a', 64)) { BossTransition = recipe.BossTransition };
        var production = StandardShanheCampaignCatalog.CreateBattleRegistry(sourceSetup);
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShanheBossPackage(),
            new StandardShanheFengxianBossPackage(), new Fixture(production));
        var setup = sourceSetup with
        {
            PlayerGeneralId = xiuluo ? StandardShanheFengxianBossPackage.RageId : Owner, Hp = xiuluo ? 6 : 20,
            ExtraSkillIds = xiuluo ? [Watch] : [],
            StartingEquipment = xiuluo ? [] : [new(CardKind.Crossbow, Suit.Club, 1)],
            EnemyRoster = xiuluo ? [new(Enemy) { StartingHand = [new(CardKind.Indulgence, Suit.Heart, 6), new(CardKind.SupplyShortage, Suit.Club, 4)] }] : recipe.Enemies,
            BossTransition = xiuluo ? null : recipe.BossTransition
        };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
        var scene = new Scene(game, registry);
        scene.Accept(new StartGameCommand());
        return (scene, setup);
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() where T : IGameEvent => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Fengxian command rejected."); }
        public void Answer(Func<PromptChoice, bool> match)
        {
            var prompt = P!;
            var choice = prompt.Choices.FirstOrDefault(match);
            Need(choice is not null, "Missing native Fengxian choice: " + JsonSerializer.Serialize(prompt));
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, G.Revision));
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> ready)
        {
            for (var step = 0; step < 180; step++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards } discard)
                    Accept(new DiscardCardsCommand(0, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision));
                else if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } play)
                    Accept(new EndPlayPhaseCommand(0, G.Revision, play.PromptId));
                else if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0 &&
                        choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                    Need(pass is not null, "Unexpected Fengxian boundary: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Fengxian fixture did not reach its boundary: " + JsonSerializer.Serialize(P));
        }
        public void Cold()
        {
            var checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint &&
                Enumerable.Range(0, G.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(G.CreateSnapshot(seat))) &&
                JsonSerializer.Serialize(restored.CardMovements) == JsonSerializer.Serialize(G.CardMovements),
                "Cold replay preserves the real forms, paused cost, equipment entities and player views.");
            G = restored;
        }
    }

    private sealed class Fixture(ContentRegistry production) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:fengxian-panels", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var kind in new[] { CardKind.WushuangHalberd, CardKind.PurpleGoldCrown })
                builder.AddCard(production.Cards.Values.First(card => card.LegacyKind == kind));
            foreach (var id in new[] { "hulao:wushuang-halberd", "hulao:purple-gold-crown" }) builder.AddSkill(production.GetSkill(id));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                 {"id":"{{Driver}}","revision":1,"activations":[{"id":"lethal","minCards":0,"maxCards":0,
                  "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"damage","target":"selectedTarget","amount":6}]}]},
                 {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-cost","window":"cardsMoved","subject":"owner",
                  "sourceZones":["hand","equipment"],"movementReasons":["skill-program.shanhe:fengxian-xiuluo.SelectAndMoveOwnedCard"],"movementOccurrence":"perOwnerBatch",
                  "optional":false,"isSkillInvocation":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Watch }.ToDictionary(id => id, id => new { name = id, description = "Native Fengxian fixture",
                        optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : new() }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, "Native Fengxian fixture") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "行者", "supporter", Driver, "shu", 20));
            builder.AddGeneral(new(Enemy, "施计敌军", "supporter", null, "wei", 20));
            builder.AddDeck(new("fixture:fengxian-deck", "固定实体闪", 4, 2, [])
            { PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 9), 80).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "无双奉先原生规则", 2, 2, new Dictionary<string, int>(), "fixture:fengxian-deck",
                GeneralPoolIds: [Owner, Enemy, StandardShanheFengxianBossPackage.FirstId, StandardShanheFengxianBossPackage.RageId],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

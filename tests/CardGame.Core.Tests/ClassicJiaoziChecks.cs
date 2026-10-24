using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ClassicJiaoziChecks
{
    private const string Hero = "classic:qu-yi", Skill = "classic:jiaozi", Driver = "fixture:classic-jiaozi-driver",
        Peer = "fixture:classic-jiaozi-peer", Mode = ShanheBattleSetup.BattleModeId, Deck = "fixture:classic-jiaozi-deck";

    public static void SelfFireAttackPaysBothTimingsAndColdReplays()
    {
        var (g, registry) = Start();
        var p = P(g)!;
        var action = g.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.FireAttack && a.TargetSeats.SequenceEqual([0]));
        var hp = V(g).Hp;
        Need(g.PredictPublicActionDamage(0, action, 0).DamageAmount == 3,
            "A public self FireAttack estimate includes both Jiaozi timings after paying its real material.");
        Accept(g, new PlayCardCommand(0, action.CardId!.Value, [0], g.Revision, p.PromptId));
        Reach(g, () => P(g)?.Kind == DecisionKind.FireAttackReveal);
        var cost = V(g).Hand.First(c => c.Suit == Suit.Diamond).Id;
        Answer(g, c => c.Cards.SequenceEqual([cost]));
        Reach(g, () => P(g)?.Kind == DecisionKind.FireAttackDiscard);
        g = Cold(g, registry);
        Answer(g, c => c.Cards.SequenceEqual([cost])); Play(g);
        Need(V(g).Hp == hp - 3 && E<DamageAppliedEvent>(g).Single() is
            { SourceSeat: 0, TargetSeat: 0, Amount: 3, Nature: DamageNature.Fire },
            "The paid native self FireAttack causes three damage, not two, while hand count remains uniquely greatest.");
        Need(E<ProgramCardDamageModifiedEvent>(g).Where(e => e.Source.SkillId == Skill)
            .Select(e => (e.BaseAmount, e.ModifiedAmount)).SequenceEqual([(1, 2), (2, 3)]) &&
            g.CardMovements.Count(m => m.CardId == cost && m.Reason == CardMoveReasons.FireAttackDiscard) == 1,
            "Causing and receiving each add one to the same attempt; the original suit payment occurs once.");
        _ = Cold(g, registry);
    }

    public static void ParticipantRolesDoNotDuplicateSourcelessDamageOrHpLoss()
    {
        var (g, registry) = Start();
        foreach (var (id, target, amount) in new[] { ("outgoing", 1, 2), ("incoming", 1, 2),
            ("self", 0, 3), ("source-less", 0, 2) })
        {
            var before = E<DamageAppliedEvent>(g).Length;
            Accept(g, new UseProgramSkillCommand(0, Driver, id, [], id is "outgoing" or "incoming" ? [target] : [], g.Revision, P(g)!.PromptId));
            Play(g);
            Need(E<DamageAppliedEvent>(g).Length == before + 1 && E<DamageAppliedEvent>(g).Last().Amount == amount,
                "Real participant roles retain the correct classic Jiaozi amount for " + id);
        }
        var hp = V(g).Hp; var damageCount = E<DamageAppliedEvent>(g).Length;
        Accept(g, new UseProgramSkillCommand(0, Driver, "lose", [], [], g.Revision, P(g)!.PromptId)); Play(g);
        Need(V(g).Hp == hp - 1 && E<DamageAppliedEvent>(g).Length == damageCount,
            "Direct HP loss is not damage and does not receive either Jiaozi bonus.");
        _ = Cold(g, registry);
    }

    private static PlayerSnapshot V(GameEngine g) => g.CreateSnapshot(0).Players[0];
    private static PendingDecision? P(GameEngine g) => g.CreateSnapshot(0).PendingDecision;
    private static T[] E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine g, GameCommand c) { var r = g.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Jiaozi command rejected."); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> matches)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(0, p.PromptId, p.Choices.First(matches).Id, g.Revision)); }
    private static void Play(GameEngine g) => Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Reach(GameEngine g, Func<bool> ready)
    {
        for (var i = 0; i < 80; i++)
        {
            if (ready()) return;
            if (P(g) is { PlayerSeat: 0, Kind: DecisionKind.Nullification }) Answer(g, c => c.Cards.Count == 0);
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Jiaozi missed its real command boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static GameEngine Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Need(Enumerable.Range(-1, 3).All(seat => SnapshotJson.Serialize(g.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(restored.CardMovements), "Cold replay preserves every safe view and exact paid movement.");
        return restored;
    }
    private static (GameEngine, ContentRegistry) Start()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var hand = new[] { new ShanheStartingCard(CardKind.FireAttack, Suit.Diamond, 11) }
            .Concat(Enumerable.Range(2, 5).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Diamond, rank))).ToArray();
        var setup = new ShanheBattleSetup(37, 1, "classic-jiaozi", Hero, 20, 16, 0, 0, 0, [Driver], hand, [],
            [new(Peer, Hp: 20) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] }], [], new string('a', 64));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, ModeId = Mode, PlayerCount = 2,
            HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ShanheBattle = setup,
            UseInteractiveSetup = false, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
        Accept(g, new StartGameCommand()); Play(g); return (g, registry);
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:classic-jiaozi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var definitions = new[] { ("outgoing", "selectedTarget", 1, "owner", "damage"), ("incoming", "owner", 1, "selectedTarget", "damage"),
                ("self", "owner", 0, "owner", "damage"), ("source-less", "owner", 0, "owner", "receiveOwnerDamage"), ("lose", "owner", 0, "owner", "loseHp") };
            var activations = definitions.Select(d => new { id = d.Item1, minCards = 0, maxCards = 0, minTargets = d.Item3,
                maxTargets = d.Item3, targetKind = "anyLiving", usesPerTurn = 1, effects = new[] {
                    new { op = d.Item5, target = d.Item2, amount = 1, sourceRef = d.Item5 == "damage" ? new { kind = d.Item4 } : null } } });
            var options = new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Driver, revision = 1, activations } } }, options), JsonSerializer.Serialize(new {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> {
                        [Driver] = new { name = "真实伤害角色", description = "原生伤害与体力流失。" } } }));
            b.AddSkill(new(Driver, "真实伤害角色", "原生伤害与体力流失。") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            b.AddGeneral(new(Peer, "安静对手", "", null, "wei", 20));
            b.AddDeck(new(Deck, "固定实体牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            b.AddMode(new(Mode, "真实骄恣自伤", 2, 2, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> {
                    [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}

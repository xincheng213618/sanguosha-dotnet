using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryDianWeiMobileChecks
{
    private const string Hero = "boundary:dian-wei-mobile", Skill = "boundary:qiangxi-mobile", Peer = "fixture:mobile-dian-wei-peer",
        Extra = "fixture:mobile-dian-wei-extra", Hp = "fixture:mobile-dian-wei-hp", Cost = "fixture:mobile-dian-wei-cost";
    private const string CostReason = "skill-program.boundary:qiangxi-mobile.MoveBoundCards";

    public static void SharedPerPhaseTargetsPayHpAndThirdWeaponOnceCold()
    {
        var s = Scene.Start(); s.Play(); var turn = s.G.State.TurnNumber;
        var weapon = s.V(0).Hand.Single(c => c.Kind == CardKind.Crossbow).Id;
        s.Use("lose-hp-and-damage", [], 1); s.Hold(Hp); var first = s.Root!.Id;
        s.Need(s.V(0).Hp == 3 && s.V(1).Hp == 8 && !s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0),
            "The HP branch pays actual HP loss before its target damage and never fabricates self damage.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(1).Hp == 7 && s.G.ResolutionStack.All(f => f.Id != first) && !s.HasTarget(1),
            "The original paid HP child returns once; both payment branches reject this already-used target.");
        s.Use("lose-hp-and-damage", [], 3); s.Hold(Hp); s.Continue(); s.Play();
        s.Need(s.V(0).Hp == 2 && s.V(3).Hp == 7 && s.HasTarget(2) && s.G.GetAttackRange(0) == 2,
            "A second distinct target spends one more HP, while an in-range third target remains available beyond the old two-use limit.");
        s.Use("discard-weapon-and-damage", [weapon], 2); s.Hold(Cost); var third = s.Root!.Id;
        s.Need(s.Zone(weapon) == CardLocation.DiscardPile && s.V(2).Hp == 8 && s.Root.PendingMovementContinuation is not null &&
            s.G.CardMovements.Count(m => m.CardId == weapon && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == CostReason) == 1,
            "The third use pays its actual hand weapon and suspends on that original movement child before damage.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(0).Hp == 2 && s.V(2).Hp == 7 && !Enumerable.Range(1, 3).Any(s.HasTarget) &&
            s.G.ResolutionStack.All(f => f.Id != third) && s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == third && e.Completed) == 0 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == third && e.Completed) == 1,
            "All three distinct targets receive one actual damage; the weapon return does not pay HP or repeat the discard, and the active frame completes once.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.G.State.TurnNumber == turn && s.HasTarget(1),
            "A genuine second Play phase in the same actual turn resets the per-target phase ledger.");
        s.Use("lose-hp-and-damage", [], 1); s.Hold(Hp); s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(0).Hp == 1 && s.V(1).Hp == 6 && !s.HasTarget(1) &&
            !s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0) &&
            s.G.CardMovements.Count(m => m.CardId == weapon && m.Reason.Value == CostReason) == 1,
            "The new phase permits the same target once more with one new HP payment; cold restores never repeat either earlier cost or target damage.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool HasTarget(int seat) => G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill && a.SelectableTargetSeats.Contains(seat));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void Use(string binding, IReadOnlyList<int> cards, int target) => Accept(new UseProgramSkillCommand(0, Skill, binding, cards, [target], G.Revision, P!.PromptId));
        public void Continue() { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++) { if (ready()) return; Need(P is not { PlayerSeat: 0 }, "Unexpected native Dian Wei boundary."); Accept(new AdvanceOneStepCommand(G.Revision)); }
            throw new InvalidOperationException("Bounded mobile Dian Wei flow was not reached.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original HP or weapon cost child and shared actual-phase target quota.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Hp = Enumerable.Range(0, 4).Select(i => V(i).Hp) })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Dian Wei command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-dian-wei-native-cost", Hero, 4, 0, 0, 0, 0, [Extra, Hp, Cost],
                [new(CardKind.Crossbow, Suit.Club, 1)], [new(CardKind.QinggangSword, Suit.Spade, 6)], [peer, peer], [peer], new string('d', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var s = new Scene(engine, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-dian-wei-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } };
            var skills = new object[] {
                new { id = Extra, revision = 1, triggers = new[] { new { id = "extra", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "real-hp-payment", window = "afterHpLost", subject = "owner", optional = false, effects = new[] { Pause() } } } },
                new { id = Cost, revision = 1, triggers = new[] { new { id = "real-weapon-payment", window = "cardsMoved", subject = "owner", optional = false,
                    sourceZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { CostReason }, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Extra] = new { name = "真实额外出牌阶段", description = "同实际回合中两段真正出牌阶段" } };
            foreach (var id in new[] { Hp, Cost }) labels[id] = new { name = "真实强袭付款子窗", description = "真实失体力或弃武器后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-dian-wei-deck";
            b.AddDeck(new(deck, "固定防御牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动强袭真实付款与阶段账本", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

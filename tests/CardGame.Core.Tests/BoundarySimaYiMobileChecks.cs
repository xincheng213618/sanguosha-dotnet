using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundarySimaYiMobileChecks
{
    private const string Hero = "boundary:sima-yi-mobile", Feedback = "boundary:feedback",
        Quiet = "fixture:mobile-smy-quiet", Driver = "fixture:mobile-smy-driver", Paid = "fixture:mobile-smy-paid",
        Peer = "fixture:mobile-smy-peer", ObtainReason = "skill-program." + Feedback + ".MoveBoundCards";

    public static void SelfSourceFeedbackObtainsOwnEquipmentAndReturnsPaidGainCold()
    {
        var s = Scene.Start(); s.Play();
        var equipment = s.G.CreateCardZoneDiagnostics().Single(c => c.Location == CardLocation.Equipment(0)).CardId;
        s.Need(s.V.GeneralId == Hero && s.V.Hp == 3 && s.V.HandCount == 0,
            "The actual mobile Sima Yi identity starts with its own equipment and the real shared Feedback program.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "self-damage", [], [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Feedback && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        var damageWindow = s.G.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single();
        var damage = s.G.ResolutionStack.OfType<DamageFrame>().Single();
        s.Need(damage is { SourceSeat: 0, TargetSeat: 0, Amount: 1 } && s.V.Hp == 2 &&
            s.Facts<DamageAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 0, Amount: 1, SourceLess: false } &&
            damageWindow.Candidates.Count(c => c.ProgramId == Feedback) == 1,
            "A real source-zero target-zero damage applies once and offers Feedback for that exact applied point.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Feedback && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-source-card"));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Feedback);
        var frameId = root.Id;
        s.Need(root.OwnerSeat == 0 && root.WindowContext is { SourceSeat: 0, TargetSeat: 0 } context &&
            context.ParentFrameId == damageWindow.Id && context.DamageFrameId == damage.Id &&
            s.P!.Choices.Any(c => c.Cards.SequenceEqual([equipment]) && c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Equipment)),
            "Feedback retains the native self-source damage window and publishes its own real equipment as an obtainable source.");
        s.Answer(c => c.Cards.SequenceEqual([equipment]) && c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Equipment));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
        root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frameId);
        var child = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last();
        var bound = root.CardSetBindings.Single(b => b.Name == "selected");
        s.Need(bound.CardIds.SequenceEqual([equipment]) && bound.SourceLocations.SequenceEqual([CardLocation.Equipment(0)]) &&
            child.Batch.ParentFrameId == frameId && child.Batch.OriginSkillId == Feedback && child.Batch.OriginSkillInstanceId == root.SkillInstanceId &&
            child.Batch.Movements is [var gained] && gained.CardId == equipment && gained.From == CardLocation.Equipment(0) &&
            gained.To == CardLocation.Hand(0) && gained.Reason.Value == ObtainReason &&
            s.V.Hand.Any(c => c.Id == equipment) && s.G.CardMovements.Count(m => m.CardId == equipment && m.Reason.Value == ObtainReason) == 1 &&
            !s.Facts<ProgramBindingResolvedEvent>().Any(e => e.FrameId == frameId),
            "One actual Equipment-to-Hand acquisition is already paid while its gain child suspends the original Feedback frame and frozen source binding. " +
            JsonSerializer.Serialize(new { Bound = bound, frameId, child.Batch.ParentFrameId, child.Batch.AwaitingProgramFrameId,
                Moves = child.Batch.Movements.Select(m => new { m.CardId, m.From, m.To, Reason = m.Reason.Value }).ToArray(),
                HasEquipmentInHand = s.V.Hand.Any(c => c.Id == equipment), Count = s.G.CardMovements.Count(m => m.CardId == equipment && m.Reason.Value == ObtainReason),
                Resolved = s.Facts<ProgramBindingResolvedEvent>().Any(e => e.FrameId == frameId) }));
        s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V.Hp == 2 && s.V.Hand.Count(c => c.Id == equipment) == 1 &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == equipment).Location == CardLocation.Hand(0) &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Equipment(0) && m.To == CardLocation.Hand(0) && m.Reason.Value == ObtainReason) == 1 &&
            s.Facts<DamageAppliedEvent>().Length == 1 &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frameId && e.SkillId == Feedback && e.Activated && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != frameId) && !s.G.ResolutionStack.OfType<DamageFrame>().Any(),
            "The paid gain child returns through the same Feedback and native damage parents once after cold, without reacquisition, repeated damage or a second Feedback completion.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players.Single(p => p.Seat == 0);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected self-source Feedback command."); }
        public void Answer(Func<PromptChoice, bool> choose) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 80; step++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human boundary interrupted self-source Feedback.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Self-source Feedback exceeded its native step bound.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the already acquired entity, original Feedback identity, self-source damage parent and actual paid gain child.");
            G = restored;
        }
        public void Need(bool yes, string why)
        {
            if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase, V.Hp,
                Pending = P?.Kind, Seat = P?.PlayerSeat, Skill = P?.SkillPrompt?.SkillId, Stack = G.ResolutionStack.Select(f => f.Kind) }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-sima-yi", Hero, 3, 0, 0, 0, 0, [Quiet, Driver, Paid], [],
                [new(CardKind.Crossbow, Suit.Club, 5)], [new(Peer, Hp: 4) { StartingHand = [] }, new(Peer, Hp: 4) { StartingHand = [] }],
                [new(Peer, Hp: 4) { StartingHand = [] }], new string('d', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-sima-yi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:sima-yi");
            var quiet = new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } };
            var driver = new { id = Driver, revision = 1, activations = new[] { new { id = "self-damage", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "damage", target = "owner", amount = 1 } } } } };
            var paid = new { id = Paid, revision = 1, triggers = new[] { new { id = "paid", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                movementOccurrence = "perBatch", movementReasons = new[] { ObtainReason }, optional = false, isSkillInvocation = false,
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid-return", options = new[] { new { id = "continue" } } } } } } };
            var names = new Dictionary<string, object> { [Quiet] = new { name = "安静摸牌", description = "保留固定反馈来源。" },
                [Driver] = new { name = "真实自源伤害", description = "原生一点自身伤害。" },
                [Paid] = new { name = "反馈获得牌子窗", description = "实际装备获取已支付。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] { quiet, driver, paid } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静同桌", "", Quiet, "wei", 4));
            const string deck = "fixture:mobile-smy-deck";
            b.AddDeck(new(deck, "反馈固定库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界司马懿反馈", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

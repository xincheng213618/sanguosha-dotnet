using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryZhouTaiMobileChecks
{
    private const string Hero = "boundary:zhou-tai-mobile", Fenji = "boundary:fenji-mobile", Buqu = "boundary:buqu-current",
        Peer = "fixture:mobile-zt-peer", Quiet = "fixture:mobile-zt-quiet", Skip = "fixture:mobile-zt-skip", Gain = "fixture:mobile-zt-gain", Driver = "fixture:mobile-zt-driver";

    public static void ActualOwnAndForeignEmptyHandTurnEndDrawBeforeHpLossAndBuquCold()
    {
        foreach (var own in new[] { true, false })
        {
            var s = Scene.Start(own); s.Play();
            if (!own)
            {
                s.Accept(new UseProgramSkillCommand(0, Driver, "strip", [], [2], s.G.Revision, s.P!.PromptId));
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Driver);
                s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"); s.Play();
                s.Need(s.V(2).HandCount == 0 && s.V(0).Hp == 1 && !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Fenji),
                    "An actual foreign last-hand discard in Play does not invoke the OL immediate-loss Fenji or charge mobile Zhou Tai HP.");
            }
            s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Fenji && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            var ended = own ? 0 : 1;
            var parent = s.G.ResolutionStack.OfType<DeferredTurnEndFrame>().Single();
            s.Need(parent.OwnerSeat == ended && parent.Prelude is { Completed: true } && s.G.State.Phase == TurnPhase.Finished &&
                s.E<TurnEndedEvent>().Any(e => e.ActorSeat == ended && e.TurnNumber == parent.TurnNumber) &&
                s.V(ended).HandCount == 0 && s.V(0).Hp == 1 &&
                !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Fenji),
                "The optional offer belongs to the actual empty-handed turn owner after TurnEnded and the completed return prelude, including a foreign actual turn; a nonempty Zhou Tai turn did not offer or pay.");
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain || s.P?.SkillPrompt?.SkillId == Fenji && s.P.Choices.Any(c => c.Targets.Count != 0));
            if (s.P?.SkillPrompt?.SkillId == Fenji)
            {
                s.Need(s.P.Choices.All(c => c.Targets.SequenceEqual([ended])), "The native fixed participant is the actual ended player, never another empty-handed role.");
                s.Answer(c => c.Targets.SequenceEqual([ended]));
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain);
            }
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Fenji);
            var identity = root.Id;
            s.Need(root.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } context && context.ParentFrameId == parent.Id &&
                context.SourceSeat == ended && context.TargetSeat == ended && s.P!.PlayerSeat == ended &&
                s.V(ended).HandCount == 2 && s.V(0).Hp == 1 &&
                s.E<CardsDrawnEvent>().Count(e => e.PlayerSeat == ended && e.Count == 2) == 1 &&
                s.E<ProgramSkillHpLostEvent>().All(e => e.SkillId != Fenji) && s.E<ProgramUniqueRankDyingResolvedEvent>().Length == 0,
                "The same original ended-turn frame actually draws two for its exact recipient before any HP payment or dying; real gain children still suspend the source.");
            s.Cold(); s.ReturnGains(root, ended);
            s.Reach(() => s.G.ResolutionStack.All(f => f.Id != identity));
            var paid = s.E<ProgramSkillHpLostEvent>().Single(e => e.SkillId == Fenji);
            var rescued = s.E<ProgramUniqueRankDyingResolvedEvent>().Single();
            s.Need(paid is { TargetSeat: 0, Amount: 1, RemainingHp: 0 } && paid.FrameId == identity &&
                s.V(0).IsAlive && s.V(0).Hp == 1 && s.V(0).BuquWounds is { Count: 1 } &&
                rescued is { SkillId: Buqu, OwnerSeat: 0, RankWasUnique: true } && s.G.CreateCardZoneDiagnostics().Single(z => z.CardId == rescued.CardId).Location == CardLocation.BuquWound(0) &&
                s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == identity && e.SkillId == Fenji && e.Window == SkillProgramTriggerWindow.AfterTurnEnded && e.Activated && e.Completed) == 1,
                "Only after real Draw2 gain returns does the original Fenji pay one native HP loss; its true dying child invokes the existing locked Buqu once and returns alive to complete the original source once.");
            var events = s.G.Events.Select(e => e.Payload).ToArray();
            var drawIndex = Array.FindIndex(events, e => e is CardsDrawnEvent drawn && drawn.PlayerSeat == ended && drawn.Count == 2);
            var lossIndex = Array.FindIndex(events, e => e is ProgramSkillHpLostEvent lost && lost.SkillId == Fenji);
            s.Need(drawIndex >= 0 && lossIndex > drawIndex && s.E<CardsDrawnEvent>().Count(e => e.PlayerSeat == ended && e.Count == 2) == 1 &&
                s.G.CardMovements.Count(m => m.Reason.Value == "skill-program." + Fenji + ".Draw" && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(ended)) == 2,
                "Cold does not draw again or pay HP before the recipient: the physical two-card draw precedes one actual original-frame HP debit.");
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Fenji command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 180; step++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human boundary interrupted the native mobile Fenji scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The native Fenji fixture exceeded its step bound.");
        }
        public void ReturnGains(ProgramSkillFrame frame, int recipient)
        {
            for (var count = 0; count < 3; count++)
            {
                Reach(() => P?.SkillPrompt?.SkillId == Gain || G.ResolutionStack.All(f => f.Id != frame.Id));
                if (G.ResolutionStack.All(f => f.Id != frame.Id)) return;
                var child = G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last();
                Need(P!.PlayerSeat == recipient && V(0).Hp == 1 && child.Batch.ParentFrameId == frame.Id &&
                    child.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason.Value == "skill-program." + Fenji + ".Draw"),
                    "Each actual gain child retains the same original Draw2 and leaves the HP payment pending, including a nonhuman recipient.");
                if (P.PlayerSeat == 0) Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The one Fenji Draw2 issued too many gain children.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint()); var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the exact actual-turn-ended parent, original Fenji frame and already paid Draw2 child before its HP cost."); G = restored;
        }
        public void Need(bool yes, string why)
        {
            if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase,
                Pending = P?.Kind, Seat = P?.PlayerSeat, Skill = P?.SkillPrompt?.SkillId, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(bool own)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-zhou-tai", Hero, 1, 0, 0, 0, 0, [Quiet, Driver, Gain],
                own ? [] : [new(CardKind.Dodge, Suit.Heart, 2)], [],
                [new(Peer, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Club, 3)] }, new(Peer, Hp: 8) { StartingHand = [] }],
                [new(Peer, Hp: 8) { StartingHand = [] }], new string('d', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-zhou-tai", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:zhou-tai");
            object[] Pause() => [new { op = "chooseOption", target = "owner", resultBind = "gained", options = new[] { new { id = "continue" } } }];
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Skip, revision = 1, triggers = new[] { new { id = "skip", window = "afterNormalDraw", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = Driver, revision = 1, activations = new[] { new { id = "strip", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "selectedTarget" }, zones = new[] { "hand" }, count = 1, destination = "discardPile" } } } } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { "skill-program." + Fenji + ".Draw" }, optional = false, isSkillInvocation = false, effects = Pause() } } }
            } });
            var names = new Dictionary<string, object> { [Quiet] = new { name = "静默摸牌", description = "保留固定手牌条件。" }, [Skip] = new { name = "同桌跳过出牌", description = "只简化无关的同桌出牌。" },
                [Driver] = new { name = "真实失去最后手牌", description = "用原生弃置区分旧版即时奋激。" }, [Gain] = new { name = "真实Draw2子窗", description = "摸牌已支付，失体力尚未支付。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静实际回合同桌", "", Quiet, "wei", 8, [Skip, Gain]));
            const string deck = "fixture:mobile-zt-deck";
            b.AddDeck(new(deck, "真实摸牌与原生创", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界周泰实际回合结束", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

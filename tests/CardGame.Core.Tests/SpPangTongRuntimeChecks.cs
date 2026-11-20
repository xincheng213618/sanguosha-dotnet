using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class SpPangTongRuntimeChecks
{
    private const string Hero = "ol:pang-tong-sp", Driver = "fixture:sp-pt-driver", Quiet = "fixture:sp-pt-quiet",
        Watch = "fixture:sp-pt-watch", GrantWatch = "fixture:sp-pt-grant-watch", RecoveryWatch = "fixture:sp-pt-recovery-watch",
        Peer = "fixture:sp-pt-peer", Deck = "fixture:sp-pt-deck";

    public static void GuolunExchangeReturnsOriginalPaidChildrenCold()
    {
        var s = Scene.Start(); s.Play();
        var originalTarget = s.V(2).Hand.Select(c => c.Id).ToArray();
        s.Use("ol:guolun", "reveal-exchange", [2]);
        s.Need(s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "reveal") == true,
            "Guolun starts its real selected other hand reveal.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "reveal");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "exchange") == true);
        var revealed = s.E<GuolunRevealedEvent>().Single();
        var foreign = s.V(2).Hand.Single(c => c.Id == revealed.CardId);
        var offered = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "exchange" &&
            s.V(0).Hand.Single(card => card.Id == c.Cards.Single()).Rank != foreign.Rank);
        var own = s.V(0).Hand.Single(c => c.Id == offered.Cards.Single());
        var reward = own.Rank < foreign.Rank ? 0 : foreign.Rank < own.Rank ? 2 : (int?)null;
        s.Answer(c => c.Id == offered.Id);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SpPangTong is { Stage: "exchange-children" }));
        var paid = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SpPangTong is not null);
        s.Need(originalTarget.Contains(revealed.CardId) && paid.SpPangTong!.OwnCard == own.Id &&
            paid.SpPangTong.RevealedCard == foreign.Id && paid.PendingMovementContinuation is not null &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.guolun.exchange") == 2,
            "Both real exchange movements are already paid before the native gain child, on the original Guolun frame.");
        s.Cold(); s.Play();
        s.Need(s.V(0).Hand.Any(c => c.Id == foreign.Id) && s.V(2).Hand.Any(c => c.Id == own.Id) &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.guolun.exchange") == 2 &&
            s.E<SpPangTongSegmentPaidEvent>().Count(e => e.FrameId == paid.Id && e.Stage == "exchange-children") == 1 &&
            s.E<SpPangTongSegmentPaidEvent>().Count(e => e.FrameId == paid.Id && e.Stage == "draw-children") == (reward is null ? 0 : 1) &&
            (reward is null || s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.sp-pang-tong.draw" && m.To == CardLocation.Hand(reward.Value)) == 1),
            "Cold return retains the original paid exchange, then draws once for the frozen smaller rank, without repeating either card movement.");
    }

    public static void SongsangHealthBranchesGrantZhanjiAndExcludeItsOwnDraw()
    {
        foreach (var wounded in new[] { false, true })
        {
            var s = Scene.Start(wounded ? RecoveryWatch : GrantWatch); s.Play();
            if (wounded) { s.Use(Driver, "wound", []); s.Play(); }
            var hp = s.V(0).Hp; var max = s.V(0).MaxHp;
            s.Use(Driver, "kill", [2]);
            s.Reach(() => s.P?.SkillPrompt?.SkillId == "ol:songsang");
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            if (wounded)
            {
                s.Reach(() => s.P is { PlayerSeat: 0, SkillPrompt.SkillId: RecoveryWatch });
                var health = s.E<SongsangHealthIssuedEvent>().Single();
                var healthParent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == health.FrameId);
                var healthChild = s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(f => f.Change.ParentFrameId == healthParent.Id);
                var healthWatcher = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == RecoveryWatch);
                s.Need(healthParent.SpPangTong is { Stage: "health-children", Wounded: true } &&
                    healthChild.ResumeFrameId == healthParent.Id && healthChild.Continuation == PostEventContinuation.AwaitedProgramMovement &&
                    healthChild.Change is { SourceSeat: 0, TargetSeat: 0, Kind: HpChangeKind.Recovery, Amount: 1 } &&
                    healthChild.Change.HpBefore == hp && healthChild.Change.HpAfter == hp + 1 &&
                    healthWatcher.WindowContext is { Window: SkillProgramTriggerWindow.AfterHpRecovered } context &&
                    context.ParentFrameId == healthChild.Id && context.HpChange == healthChild.Change &&
                    s.V(0).Hp == hp + 1 && s.V(0).MaxHp == max && ZhanjiGrants(s) == 0,
                    "The real recovery suspends its original Songsang frame before Zhanji is granted.");
                s.Cold();
                s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                s.Play();
            }
            else
            {
                s.Reach(() => s.P is { PlayerSeat: 0, SkillPrompt.SkillId: GrantWatch });
                var health = s.E<SongsangHealthIssuedEvent>().Single();
                var stack = s.G.ResolutionStack.ToArray();
                var parent = stack.OfType<ProgramSkillFrame>().Single(f => f.Id == health.FrameId);
                var watcher = stack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == GrantWatch);
                var window = stack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(f => f.Id == watcher.WindowContext?.ParentFrameId);
                var childIndex = Array.IndexOf(stack, window);
                s.Need(parent is { SkillId: "ol:songsang", OwnerSeat: 0, SpPangTong.Stage: "grant-children" } &&
                    childIndex > 0 && childIndex + 1 < stack.Length && stack[childIndex - 1].Id == parent.Id &&
                    stack[childIndex + 1].Id == watcher.Id && window.OwnerSeat == 0 && ZhanjiGrants(s) == 1 &&
                    s.V(0).Skills!.Count(skill => skill.ContentId == "ol:zhanji") == 1,
                    "The original Songsang frame has granted Zhanji once and directly owns the native skill-change window.");
                s.Cold();
                var restored = s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(f => f.Id == window.Id);
                s.Need(ProgramChildWindowRuntime.MatchesSkillsChangedChild(parent.Id, restored) &&
                    !ProgramChildWindowRuntime.MatchesSkillsChangedChild(parent.Id, restored with { ResumeProgramFrameId = watcher.Id }) &&
                    !ProgramChildWindowRuntime.MatchesSkillsChangedChild(parent.Id, restored with { Continuation = ProgramLifecycleContinuation.ResumeCharacterStateChange }) &&
                    !ProgramChildWindowRuntime.MatchesSkillsChangedChild(parent.Id, restored with { Window = SkillProgramTriggerWindow.CharacterDied }),
                    "The restored child matches the original parent, SkillsChanged window and parent-program continuation.");
                s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                s.Play();
                var resolved = new ProgramBindingResolvedEvent(parent.Id, parent.SkillId,
                    parent.TriggerId!, parent.SkillInstanceId, parent.OwnerSeat, SkillProgramTriggerWindow.CharacterDied,
                    Activated: true, Completed: true);
                s.Need(s.G.ResolutionStack.All(f => f.Id != parent.Id && f.Id != window.Id) &&
                    s.E<ProgramBindingResolvedEvent>().Count(fact => fact == resolved) == 1,
                    "The native window returns to the original Songsang frame and completes it once.");
            }
            var recoveries = s.E<RecoveryAppliedEvent>().Count(e => e == new RecoveryAppliedEvent(0, 0, 1, hp + 1));
            var growths = s.E<MaximumHpChangedEvent>().Count(e => e is { PlayerSeat: 0, Delta: 1, SkillId: "ol:songsang" } && e.MaximumHp == max + 1);
            s.Need(!s.V(2).IsAlive && s.V(0).Hp == hp + (wounded ? 1 : 0) &&
                s.V(0).MaxHp == max + (wounded ? 0 : 1) && recoveries == (wounded ? 1 : 0) && growths == (wounded ? 0 : 1) &&
                s.E<SongsangHealthIssuedEvent>() is [{ Wounded: var frozen }] && frozen == wounded &&
                ZhanjiGrants(s) == 1 && s.V(0).Skills!.Count(skill => skill.ContentId == "ol:zhanji") == 1,
                "Songsang resolves its actual other death, preserves the original wounded/maximum-HP branch, and really grants Zhanji.");
            var before = s.V(0).Hand.Count;
            s.Use(Driver, "draw-two", []); s.Play();
            var bonus = s.E<SpPangTongSegmentPaidEvent>().Where(e => e.Source.SkillId == "ol:zhanji").ToArray();
            s.Need(s.V(0).Hand.Count == before + 3 && bonus is [{ Stage: "draw-children", Count: 1, DrawInvocation: not null }] &&
                s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                    m.Reason.Value == "skill-program.sp-pang-tong.draw") == 1,
                "One actual own Play draw invocation of two cards earns exactly one Zhanji card; that skill's own physical draw cannot recurse.");
        }
    }

    private static int ZhanjiGrants(Scene scene) => scene.E<SkillsAcquiredEvent>().Count(fact =>
        fact.PlayerSeat == 0 && fact.SourceSkillId == "ol:songsang" && fact.SkillIds.SequenceEqual(["ol:zhanji"]));

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sp-pang-tong-runtime", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"draw-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":2}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","isSkillInvocation":false,"window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"exchange-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.guolun.exchange"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
              {"id":"{{GrantWatch}}","revision":1,"triggers":[{"id":"grant-child","window":"skillsChanged","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
              {"id":"{{RecoveryWatch}}","revision":1,"triggers":[{"id":"recovery-child","window":"afterHpRecovered","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = Driver, description = "真实原生伤害、失去体力和摸牌" },
                    [Quiet] = new { name = Quiet, description = "固定同桌跳过出牌阶段" },
                    [Watch] = new { name = Watch, description = "已交换后的真实获得牌子窗", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [GrantWatch] = new { name = GrantWatch, description = "展骥授予后的原生技能变化窗口", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [RecoveryWatch] = new { name = RecoveryWatch, description = "送丧的原生回复窗口", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, id, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "", Quiet, "wei", 20, [Watch]));
            b.AddDeck(new(Deck, "固定真实闪牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 72)
                .Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "SP庞统原生返回", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(v => G.CreateSnapshot(v).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string skill, string activation, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, skill, activation, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The native fixture must reach its real boundary before game end.");
                if (P?.SkillPrompt?.SkillId == Watch) Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                else if (P is not { PlayerSeat: 0 }) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (P.Kind == DecisionKind.PlayCard) Accept(new EndPlayPhaseCommand(0, G.Revision, P.PromptId));
                else if (P.Kind == DecisionKind.RescueDying) Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die");
                else if (P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected SP Pang Tong boundary " + JsonSerializer.Serialize(P));
            }
            throw new InvalidOperationException("SP Pang Tong did not reach the bounded native boundary " + JsonSerializer.Serialize(P));
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(v => SnapshotJson.Serialize(G.CreateSnapshot(v))).ToArray(),
            Frames = G.ResolutionStack, Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Moves = G.CardMovements });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold restore preserves every original paid frame, movement invoice and published child choice."); }
        public void Accept(GameCommand command) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected command."); }
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, P })); }
        public static Scene Start(string? observer = null)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(37, 1, "sp-pang-tong-runtime", Hero, 3, 0, 0, 0, 0, observer is null ? [Driver] : [Driver, observer], [], [],
                [new(Peer), new(Peer)], [new(Peer)], new string('a', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
}

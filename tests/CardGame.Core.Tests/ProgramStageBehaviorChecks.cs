using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ProgramStageBehaviorChecks
{
    private const string Hero = "mou:cao-hong", Driver = "fixture:program-stage-driver", Watch = "fixture:program-stage-watch",
        Peer = "fixture:program-stage-peer", Deck = "fixture:program-stage-deck";
    private const string Take = "skill-program.mou-cao-hong.obtain", Give = "skill-program.mou:yingjia.return";

    public static void YingjiaReturnsActualGainAndKeepsBothPaidChildrenCold()
    {
        var s = Scene.Start();
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var slash = s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1]));
        s.Accept(new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P is { PlayerSeat: 0 } && s.Parent?.MouCaoHong?.Stage == MouCaoHongStage.ExchangeTarget);
        var original = s.G.CreateSnapshot(2).Players.Single(p => p.Seat == 2).Hand.Select(c => c.Id).ToArray();
        s.Need(original.Length == 3, "The real completed Slash leaves the separate exchange target's three original hand entities untouched.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.SequenceEqual([2]));
        s.Pause();
        var frameId = s.Parent!.Id;
        s.Need(s.Parent.MouCaoHong is { Stage: MouCaoHongStage.ExchangeTakeChildren, Target: 2, Requested: 3, Actual: 3 } &&
            s.Paid(frameId).Length == 3 && s.Paid(frameId).All(e => original.Contains(e.CardId) && e.From == CardLocation.Hand(2) && e.To == CardLocation.Hand(0)) &&
            !s.E<MouCaoHongSegmentCompletedEvent>().Any(e => e.FrameId == frameId),
            "The original parent pays all three acquisitions once and retains the unfinished take segment while its actual gain observer is suspended.");
        var handAfterTake = s.G.CreateSnapshot(0).Players[0].HandCount;
        s.Cold(); s.Resume();
        s.Reach(() => s.P is { PlayerSeat: 0 } && s.Parent?.MouCaoHong?.Stage == MouCaoHongStage.ReturnCards);
        s.Need(s.Parent!.Id == frameId && s.Parent.MouCaoHong is { Requested: 3, Selected.Count: 0 } &&
            s.G.CreateSnapshot(0).Players[0].HandCount == handAfterTake + 1 &&
            s.E<MouCaoHongSegmentCompletedEvent>().Single(e => e.FrameId == frameId) is { Stage: MouCaoHongStage.ExchangeTakeChildren, Requested: 3, Actual: 3 } &&
            s.Paid(frameId).Length == 3,
            "After cold return, the gain child's extra real draw changes current hand size without changing the original segment's actual three-card return debt or paying the take again.");
        for (var i = 0; i < 3; i++) s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "add");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "done"); s.Pause();
        s.Need(s.Parent!.Id == frameId && s.Parent.MouCaoHong is { Stage: MouCaoHongStage.ReturnChildren, Requested: 3, Actual: 3 } &&
            s.Paid(frameId).Count(e => e.From == CardLocation.Hand(0) && e.To == CardLocation.Hand(2)) == 3 &&
            s.E<MouCaoHongCompletedEvent>().All(e => e.FrameId != frameId),
            "The exact three selected return entities pay on the same original frame before the return observer suspends completion.");
        s.Cold(); s.Resume();
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.Need(s.Paid(frameId).Length == 6 && s.G.CardMovements.Count(m => m.Reason.Value == Take) == 3 &&
            s.G.CardMovements.Count(m => m.Reason.Value == Give) == 3 &&
            s.E<MouCaoHongExchangeIssuedEvent>().Count(e => e.FrameId == frameId) == 1 &&
            s.E<MouCaoHongCompletedEvent>().Count(e => e.FrameId == frameId) == 1 &&
            s.E<MouCaoHongSegmentCompletedEvent>().Where(e => e.FrameId == frameId).Select(e => (e.Stage, e.Actual)).SequenceEqual(
                [(MouCaoHongStage.ExchangeTakeChildren, 3), (MouCaoHongStage.ReturnChildren, 3)]) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().All(f => f.Id != frameId),
            "Cold recovery after each paid child preserves one exchange, one take, one equal actual return and one completion without replaying either physical payment.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Parent => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.MouCaoHong is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public MouCaoHongMovementPaidEvent[] Paid(long frame) => E<MouCaoHongMovementPaidEvent>().Where(e => e.FrameId == frame).ToArray();
        public void Pause() => Reach(() => P?.SkillPrompt?.SkillId == Watch);
        public void Resume() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "resume");
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P; var c = p?.Choices.FirstOrDefault(predicate);
            Need(c is not null, "The current native stage publishes its required choice.");
            Accept(new AnswerPromptCommand(p!.PlayerSeat, p.PromptId, c!.Id, G.Revision));
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 100; i++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0 } p)
                    throw new InvalidOperationException("Unexpected human boundary before the native stage: " + JsonSerializer.Serialize(p));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The real native stage reaches its bounded child or own Play boundary.");
        }
        public void Cold()
        {
            var before = State();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "The checkpoint cold-restores the exact original stage, paid receipt, observer prompt, physical movements and accepted commands.");
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Need(bool yes, string message)
        { if (!yes) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { P, Parent })); }
        public void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Real stage command rejected.");
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(39, 1, "program-stage-paid-children", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3)], [],
                Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Peer, Hp: 8)
                { StartingHand = [new(CardKind.Dodge, Suit.Heart, 6), new(CardKind.Dodge, Suit.Club, 7), new(CardKind.Dodge, Suit.Spade, 8)] }).ToArray(), [], new string('d', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
            var scene = new Scene(g, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:program-stage-behavior", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[
                {"id":"outgoing","query":"outgoingDistance","operation":"add","value":-1,"priority":0},
                {"id":"incoming","query":"incomingDistance","operation":"add","value":-1,"priority":0},
                {"id":"hold","query":"handLimit","operation":"add","value":30,"priority":0}]},
              {"id":"{{Watch}}","revision":1,"triggers":[
                {"id":"take-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{Take}}"],"movementOccurrence":"perBatch","optional":false,"isSkillInvocation":false,"effects":[
                  {"op":"chooseOption","target":"owner","resultBind":"take-resume","options":[{"id":"resume"}]},{"op":"draw","target":"owner","amount":1}]},
                {"id":"return-paid","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["{{Give}}"],"movementOccurrence":"perBatch","optional":false,"isSkillInvocation":false,"effects":[
                  {"op":"chooseOption","target":"owner","resultBind":"return-resume","options":[{"id":"resume"}]}]}
              ]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Driver] = new { name = "固定真实距离", description = "稳定原生迎驾资格与手牌。" },
                [Watch] = new { name = "原段已支付子窗", description = "真实实体移动后暂停并改变当前手牌。", optionLabels = new Dictionary<string, string> { ["resume"] = "继续" } }
            } }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "公共阶段行为检查") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定真实同桌", "", "standard:none", "wei", 8));
            b.AddDeck(new(Deck, "固定闪实体牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 9)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "公共阶段原帧恢复", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}

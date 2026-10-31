using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class DingShangWanRuntimeChecks
{
    private const string Owner = "fixture:ding-shang-wan-owner", Peer = "fixture:ding-shang-wan-peer";
    private const string Driver = "fixture:ding-shang-wan-driver", Watch = "fixture:ding-shang-wan-watch";
    private const string NoDraw = "fixture:ding-shang-wan-no-draw", Deck = "fixture:ding-shang-wan-deck";

    public static void FengyanDamageBranchesKeepOriginalPaidDrawAndGiftCold()
    {
        foreach (var branch in new[] { "owner-draw", "other-draw" })
        {
            var s = Scene.Create(); s.Play();
            var beforeOwner = s.V(0).HandCount; var beforeOther = s.V(1).HandCount;
            s.Accept(new UseProgramSkillCommand(0, Driver, "incoming", [], [1], s.G.Revision, s.P!.PromptId));
            s.Reach(() => s.P is { PlayerSeat: 0 } && s.Root()?.DingShangWan is { Stage: DingShangWanStage.FengChoice });
            var frame = s.Root()!; var receipt = frame.DingShangWan!;
            s.Need(receipt is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Counterpart: 1 } &&
                receipt.Source.SkillId == "ol:fengyan" && receipt.Source.OwnerSeat == 0 &&
                s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 1 && e.TargetSeat == 0 && e.Amount == 1) == 1,
                "The genuine foreign-source damage owns Fengyan's exact counterpart and mandatory branch.");
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == branch);
            s.Reach(() => s.P is { PlayerSeat: 0 } && s.P.SkillPrompt?.SkillId == Watch &&
                s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var paid = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frame.Id);
            var recipient = branch == "owner-draw" ? 0 : 1;
            s.Need(paid.DingShangWan is { ActualCount: 1 } p && p.Source == receipt.Source && p.Counterpart == 1 &&
                p.Branch == branch && p.Stage == (recipient == 0 ? DingShangWanStage.FengOwnerDraw : DingShangWanStage.FengTargetDraw) &&
                paid.PendingMovementContinuation is { SubjectSeat: var subject } && subject == recipient &&
                s.E<DingShangWanMovementIssuedEvent>().Count(e => e.FrameId == frame.Id && e.Payer == recipient && e.Count == 1) == 1,
                "The real one-card draw is already paid on the original frame while its CardsGained child awaits a command.");
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            int? gifted = null;
            if (recipient == 0)
            {
                s.Reach(() => s.P is { PlayerSeat: 0 } && s.Root()?.DingShangWan is { Stage: DingShangWanStage.FengGiftChoice });
                gifted = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "pick").Cards.Single();
                s.Answer(c => c.Cards.SequenceEqual([gifted.Value]) && c.Parameters.GetValueOrDefault("branch") == "pick");
                s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "finish");
            }
            s.Play();
            s.Need(s.E<DingShangWanStartedEvent>().Count(e => e.FrameId == frame.Id) == 1 &&
                s.E<DingShangWanBranchEvent>().Count(e => e.FrameId == frame.Id && e.Branch == branch) == 1 &&
                s.E<DingShangWanCompletedEvent>().Count(e => e.FrameId == frame.Id) == 1 &&
                s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) &&
                    m.Reason.Value == "skill-program.ding-shang-wan.draw") == 1 &&
                s.V(0).HandCount == beforeOwner && s.V(1).HandCount == beforeOther + (recipient == 0 ? 1 : -1),
                "Both options resume the original paid draw once, then give one actual card or discard two actual cards.");
            if (gifted is { } id)
                s.Need(s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Hand(1) && m.Reason.Value == "skill-program.ding-shang-wan.fengyan.give") == 1,
                    "The selected physical gift reaches its original foreign counterpart exactly once after cold selection.");
            else
                s.Need(s.G.CardMovements.Count(m => m.From == CardLocation.Hand(1) && m.To == CardLocation.DiscardPile &&
                    m.Reason.Value == "skill-program.ding-shang-wan.fengyan.discard") == 2,
                    "The native counterpart chooses and pays its two physical discards through the existing AI command path.");
            s.Cold();
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramSkillFrame? Root() => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.DingShangWan is not null);
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
        public void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Rejected genuine Fengyan command.");
        }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; Need(p.PlayerSeat == 0, "Only actual human choices use AnswerPromptCommand."); Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "Fengyan fixture ended before its real continuation.");
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => c.Targets.SequenceEqual([0])))
                    { Answer(c => c.Targets.SequenceEqual([0])); continue; }
                    var skip = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                    Need(skip is not null, "Unexpected Fengyan boundary: " + JsonSerializer.Serialize(p));
                    Answer(c => c.Id == skip!.Id);
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded Fengyan continuation: " + JsonSerializer.Serialize(P));
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = State();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold replay preserves every view, branch, physical movement and original frame child.");
        }
        public static Scene Create()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var hand = new[] { new ShanheStartingCard(CardKind.Dodge, Suit.Club, 4), new(CardKind.Dodge, Suit.Heart, 6) };
            var peers = new[] { new ShanheStartingCard(CardKind.Dodge, Suit.Spade, 3), new(CardKind.Dodge, Suit.Diamond, 8), new(CardKind.Dodge, Suit.Club, 10) };
            var setup = new ShanheBattleSetup(43, 1, "ding-shang-wan-native", Owner, 8, 4, 0, 0, 0, [], hand, [],
                [new(Peer, Hp: 10) { StartingHand = peers }, new(Peer, Hp: 10) { StartingHand = peers }],
                [new(Peer, Hp: 10) { StartingHand = peers }], new string('d', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry);
            var result = new Scene(game, registry); result.Accept(new StartGameCommand()); return result;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ding-shang-wan-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:ding-shang-wan");
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{NoDraw}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.ding-shang-wan.draw"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"chooseOption","target":"selectedTarget","resultBind":"paid","options":[{"id":"continue"}]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [NoDraw] = new { name = "固定真实手牌", description = "保留物理起手用于结算。" },
                [Driver] = new { name = "真实外来源伤害", description = "由指定存活角色对自己造成伤害。" },
                [Watch] = new { name = "讽言已付摸牌子窗口", description = "在原生获得牌窗口暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "实际讽言持有人", "ol-ding-shang-wan", Driver, "wei", 8, ["ol:fengyan", NoDraw, Watch]));
            builder.AddGeneral(new(Peer, "实际外来源", "supporter", NoDraw, "qun", 10, [Watch]));
            builder.AddDeck(new(Deck, "讽言固定原生实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, i % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "讽言原生结算", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

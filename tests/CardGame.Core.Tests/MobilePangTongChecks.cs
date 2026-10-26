using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobilePangTongChecks
{
    private const string Hero = "mobile:pang-tong", Chain = "mobile:lianhuan", Rebirth = "mobile:niepan",
        Driver = "fixture:ordinary-mobile-pang-tong-driver", Watch = "fixture:ordinary-mobile-pang-tong-draw",
        Peer = "fixture:ordinary-mobile-pang-tong-peer", DrawReason = "skill-program.mobile:niepan.Draw",
        DiscardReason = "skill-program.mobile:niepan.DiscardOwnedZoneCards";

    public static void OrderedDyingRebirthReturnsAfterNativeDrawCold()
    {
        var s = Scene.Start(); s.Play();
        var lightning = s.G.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.Lightning);
        s.Accept(new PlayCardCommand(0, lightning.CardId!.Value, lightning.TargetSeats, s.G.Revision, s.P!.PromptId,
            lightning.PlayedCardKind, lightning.TargetCardId) { ConversionSource = lightning.ConversionSource });
        s.Play();
        var original = s.V().Hand.Select(c => c.Id).Concat(s.V().Equipment.Select(c => c.Id))
            .Concat(s.V().Judgment.Select(c => c.Id)).ToArray();
        var club = s.V().Hand.Single(c => c.Suit == Suit.Club).Id;
        var equipment = s.V().Equipment.Single().Id;
        s.Need(s.V() is { Hp: 3, MaxHp: 3, HandCount: 2, Equipment.Count: 1, Judgment.Count: 1 } &&
            s.G.GetHumanLegalActions().All(a => a.ProgramSkillId != Rebirth) &&
            s.G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.IronChain && a.CardId == club && a.ConversionSource?.SkillId == Chain) &&
            s.G.GetHumanLegalActions().All(a => a.CardId != equipment || a.Kind is not (LegalActionKind.IronChain or LegalActionKind.Recast)),
            "The actual ordinary Pang Tong has HEJ originals, Hand-only Lianhuan and no proactive Niepan entry.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "enter-dying", [], [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "program-trigger" &&
            c.Parameters.GetValueOrDefault("skill-id") == Rebirth) == true);
        var dying = s.G.ResolutionStack.OfType<DyingFrame>().Single(f => f.VictimSeat == 0);
        var producer = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver).Id;
        s.Need(s.V() is { Hp: 0, MaxHp: 2, IsChained: true, IsFaceDown: true },
            "Native maximum-HP change, chain, turn-over and HP loss establish a real dying owner with a two-HP recovery ceiling.");
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "program-trigger" &&
            c.Parameters.GetValueOrDefault("skill-id") == Rebirth);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var rebirth = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Rebirth);
        var rebirthId = rebirth.Id;
        s.Need(rebirth.TriggerId == "dying-rebirth" && rebirth.InstructionIndex == 4 &&
            rebirth.WindowContext is { Window: SkillProgramTriggerWindow.SelfDyingResponse, TargetSeat: 0, ParentFrameId: var parent } && parent == dying.Id &&
            s.V() is { Hp: 0, MaxHp: 2, HandCount: 3, Equipment.Count: 0, Judgment.Count: 0, IsChained: false, IsFaceDown: false } &&
            original.All(id => s.Zone(id) == CardLocation.DiscardPile &&
                s.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.DiscardPile && m.Reason.Value == DiscardReason) == 1) &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == DrawReason) == 3 &&
            !s.E<RecoveryAppliedEvent>().Any() && s.G.ResolutionStack.Any(f => f.Id == producer && f is ProgramSkillFrame),
            "Before the real draw child returns, all original HEJ cards are discarded, both states are restored and three cards are drawn, while the original dying owner is still at zero HP.");
        s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Play();
        s.Need(s.V() is { IsAlive: true, Hp: 2, MaxHp: 2, HandCount: 3, Equipment.Count: 0, Judgment.Count: 0, IsChained: false, IsFaceDown: false } &&
            s.E<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 2, RemainingHp: 2 }] &&
            s.E<DyingResolvedEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == 0 && e.Survived) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == rebirthId && e.SkillId == Rebirth && e.Activated && e.Completed) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == producer && e.SkillId == Driver && e.Completed) == 1 &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Rebirth && e.Scope == SkillUsageScope.Game && e.Count == 1) == 1 &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == DrawReason) == 3 &&
            original.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.DiscardPile && m.Reason.Value == DiscardReason) == 1) &&
            s.G.ResolutionStack.All(f => f.Id != producer && f.Id != dying.Id && f.Id != rebirthId),
            "Cold finishes one native recovery to min(three, current maximum HP), one limited rebirth and the original lethal producer, without repeated discard, draw or rescue.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected ordinary Pang Tong command."); }
        public void Answer(Func<PromptChoice, bool> select)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(select).Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140; i++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.Nullification })
                { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                Need(P is null, "Unexpected ordinary Pang Tong boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Fixed ordinary Pang Tong flow did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => string.Join("|", Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i)))) +
                JsonSerializer.Serialize(G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))) +
                GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original zero-HP dying owner and the paid rebirth's native draw child.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, Skill = p.SkillPrompt?.SkillId } : null, V().Hp, V().MaxHp, V().HandCount, Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(97, 1, "ordinary-mobile-pang-tong-rebirth", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
                [new(CardKind.Lightning, Suit.Spade, 1), new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Dodge, Suit.Club, 8)],
                [new(CardKind.QinggangSword, Suit.Club, 6)],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('c', 64));
            var scene = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry), registry);
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-pang-tong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "enter-dying", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                    targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new object[] {
                        new { op = "changeMaximumHp", target = "owner", amount = -1 },
                        new { op = "setChainedState", target = "owner", chained = true },
                        new { op = "setFaceState", target = "owner", faceDown = true },
                        new { op = "loseHp", target = "owner", amount = 2 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "real-rebirth-draw", window = "cardsGained", subject = "owner",
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { DrawReason }, optional = false,
                    usageScope = "game", usageLimit = 1, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause",
                        options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> {
                [Driver] = new { name = "真实濒死与复原前置", description = "原生减上限、横置、翻面和失去体力。" },
                [Watch] = new { name = "真实涅槃摸牌子窗", description = "原生摸三张后，回复前暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原涅槃" } }
            };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "真实其他角色", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:ordinary-mobile-pang-tong-deck";
            b.AddDeck(new(deck, "固定涅槃摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "普通移动涅槃顺序", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}

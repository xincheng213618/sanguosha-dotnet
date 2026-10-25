using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryYuanShaoMobileChecks
{
    private const string Hero = "boundary:yuan-shao-mobile", Skill = "boundary:luanji-mobile", Binding = "unused-suit-pair-as-arrow-barrage",
        Gain = "fixture:mobile-yuan-shao-gain", Peer = "fixture:mobile-yuan-shao-peer", ResponderDraw = "skill-program.boundary:luanji-mobile.responder-draw", NoDamageDraw = "skill-program.boundary:luanji-mobile.undamaged-draw";

    public static void UnusedSuitsAndNativeArrowRewardsReturnPaidCold()
    {
        var s = Scene.Start(); s.Play(); var hand = s.V(0).Hand; var initial = hand.Count;
        var first = hand.Where(c => c.Rank is 2 or 3).Select(c => c.Id).ToArray();
        var usedHeart = hand.Single(c => c.Rank == 4).Id;
        var second = hand.Where(c => c.Rank is 5 or 6).Select(c => c.Id).ToArray();
        s.Accept(new UseProgramSkillCommand(0, Skill, Binding, first, [], s.G.Revision, s.P!.PromptId)); s.Hold(Gain);
        var root = s.Root!.Id;
        var context = s.Root.WindowContext!; var native = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == context.CardUse!.ParentCardUseFrameId);
        var outer = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill && f.TriggerId is null && f.ActivationId == Binding && f.SelectedCardIds.Order().SequenceEqual(first.Order())).Id;
        s.Need(s.Root.TriggerId == "undamaged-arrow-draw" && context.Window == SkillProgramTriggerWindow.CardUseCompleted &&
            native is { SourceSeat: 0, CardKind: CardKind.ArrowBarrage, Action.Type: CardActionType.Use } && native.Action.ActionId == context.CardUse!.CardActionId &&
            native.Action.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(first.Order()) &&
            s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Id == context.ParentFrameId && w.ParentFrameId == native.Id && w.Action.ActionId == native.Action.ActionId) &&
            s.Root.PendingMovementContinuation is not null && s.V(0).HandCount == initial - 2 + 3 &&
            Enumerable.Range(1, 3).All(i => s.V(i).Hp == 8 && s.RewardMoves(ResponderDraw, i) == 1) &&
            s.RewardMoves(NoDamageDraw, 0) == 3 && s.E<CardRespondedEvent>().Count(e => e.EffectiveCardKind == CardKind.Dodge) == 3 && !s.E<DamageAppliedEvent>().Any() &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Skill && e.Scope == SkillUsageScope.Turn && e.UsageId.StartsWith("mobile-luanji:suit:", StringComparison.Ordinal)) == 2,
            "The true different-suit hand pair pays both turn-scoped suits; each actual Dodge provider draws once, and a wholly undamaged three-target Arrow pays exactly three owner cards before its final native gain child.");
        s.Cold(); s.Continue(); s.Play();
        var actions = s.G.GetHumanLegalActions().Where(a => a.ProgramSkillId == Skill).ToArray();
        s.Need(actions.Length > 0 && actions.All(a => !a.SelectableCardIds.Contains(usedHeart)) && second.All(id => actions.Any(a => a.SelectableCardIds.Contains(id))) &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == root && e.Completed) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == outer && e.Completed) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == native.Id && e.CardKind == CardKind.ArrowBarrage) == 1 && s.G.ResolutionStack.All(f => f.Id != root && f.Id != outer && f.Id != native.Id),
            "The original paid tail returns once; cards of either spent suit are excluded while the two still-unused suits remain legal in the same actual turn.");
        var revision = s.G.Revision;
        var rejected = s.G.Submit(new UseProgramSkillCommand(0, Skill, Binding, [usedHeart, second[0]], [], revision, s.P!.PromptId));
        s.Need(!rejected.Accepted && s.G.Revision == revision && s.RewardMoves(NoDamageDraw, 0) == 3,
            "A later pair containing a previously spent suit is rejected before new material payment or reward.");
        s.Accept(new UseProgramSkillCommand(0, Skill, Binding, second, [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.V(0).HandCount == initial - 4 + 6 && s.RewardMoves(NoDamageDraw, 0) == 6 &&
            Enumerable.Range(1, 3).All(i => s.V(i).Hp == 8 && s.RewardMoves(ResponderDraw, i) == 2) &&
            s.E<CardRespondedEvent>().Count(e => e.EffectiveCardKind == CardKind.Dodge) == 6 && s.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.ArrowBarrage) == 2 &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Skill && e.Scope == SkillUsageScope.Turn && e.UsageId.StartsWith("mobile-luanji:suit:", StringComparison.Ordinal)) == 4 &&
            !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill) && !s.E<DamageAppliedEvent>().Any() &&
            first.Concat(second).All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1),
            "A second genuine Arrow consumes the other two suits with no old once-per-phase cap; six real responses and six owner tail cards are paid once, after which all four suits are unavailable for this turn.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill && f.OwnerSeat == 0 && f.TriggerId == "undamaged-arrow-draw" && f.WindowContext?.Window == SkillProgramTriggerWindow.CardUseCompleted);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int RewardMoves(string reason, int target) => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(target) && m.Reason.Value == reason);
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void Continue() { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        { for (var i = 0; i < 240; i++) { if (ready()) return; Need(P is not { PlayerSeat: 0 }, "Unexpected native mobile Yuan Shao boundary."); Accept(new AdvanceOneStepCommand(G.Revision)); } throw new InvalidOperationException("Bounded Luanji flow was not reached."); }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original Arrow, actual suit usage and final paid native draw child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Yuan Shao command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 9)] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-yuan-shao-native-arrow", Hero, 4, 0, 0, 0, 0, [Gain],
                [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Diamond, 3), new(CardKind.Dodge, Suit.Heart, 4), new(CardKind.Dodge, Suit.Club, 5), new(CardKind.Dodge, Suit.Spade, 6)], [], [peer, peer], [peer], new string('e', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(engine, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-yuan-shao-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "classic:yuan-shao", Hero);
            var skills = new[] { new { id = Gain, revision = 1, triggers = new[] { new { id = "first-whole-arrow-tail", window = "cardsGained", subject = "owner", optional = false,
                destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { NoDamageDraw }, condition = new { kind = "nativeDrawActualTurnOrdinalIs", value = 1 },
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } } } } };
            var labels = new Dictionary<string, object> { [Gain] = new { name = "乱击已付尾奖", description = "整次原生摸牌末张实体后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-yuan-shao-deck";
            b.AddDeck(new(deck, "固定万箭响应牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 60).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动乱击真实万箭", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

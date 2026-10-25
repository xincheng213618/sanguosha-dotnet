using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryXuHuangMobileChecks
{
    public static void HandComparedSupplyAndOtherSkippedDrawReturnCold()
    {
        var s = BoundaryMobileSupplyAndDuelScene.Start(xuHuang: true); s.Play();
        s.Need(s.E<DrawPhaseSkippedEvent>().Any(e => e.OwnerSeat == 0) &&
            !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == "boundary:jiezi-mobile"),
            "Actual own skipped Draw does not trigger mobile Jiezi.");
        bool Supply(int target) => s.G.GetLegalActions(0).Any(a => a.PlayedCardKind == CardKind.SupplyShortage && a.TargetSeats.SequenceEqual([target]));
        s.Need(s.G.GetCombatDistance(0, 2) == 2 && s.V(0).HandCount == s.V(2).HandCount && Supply(2),
            "Equal hand counts admit a genuine distance-two black basic SupplyShortage conversion.");
        s.Draw(0); s.Play();
        s.Need(s.V(0).HandCount == s.V(2).HandCount + 1 && !Supply(2),
            "A smaller target hand does not get a free distance increase.");
        s.Draw(2); s.Play();
        s.Need(Supply(2) && !Supply(3), "The exact equality boundary is target-local and does not grant distance to the smaller other hand.");
        var use = s.G.GetLegalActions(0).First(a => a.PlayedCardKind == CardKind.SupplyShortage && a.TargetSeats.SequenceEqual([2]));
        var material = use.CardId!.Value;
        s.PlayAction(use); s.Play();
        s.Need(s.V(2).Judgment.Any(c => c.Id == material) && s.G.CardMovements.Count(m => m.CardId == material &&
            m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1,
            "One real material is paid once and reaches the ordinary native delayed-card zone.");
        var draws = s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count);
        s.End(0); s.Reach(() => s.P?.SkillPrompt?.SkillId == BoundaryMobileSupplyAndDuelScene.Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == "boundary:jiezi-mobile");
        s.Need(s.E<DrawPhaseSkippedEvent>().Any(e => e.OwnerSeat == 2) &&
            root.WindowContext is { Window: SkillProgramTriggerWindow.DrawPhaseSkipped, TargetSeat: 2 } &&
            s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == draws + 1,
            "An actual failed SupplyShortage judgment skips the other role's Draw and issues exactly one native owner draw before its gain child.");
        s.Cold(); s.Continue(); s.Play(2);
        s.Need(s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == "boundary:jiezi-mobile" && e.Completed && e.Activated) == 1 &&
            s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == draws + 1,
            "The cold native gain child returns to the original locked trigger without repeating its draw.");
    }
}

internal static class BoundaryYanLiangWenChouMobileChecks
{
    public static void RevealChoiceAndExactOpponentResponsesReturnCold()
    {
        var s = BoundaryMobileSupplyAndDuelScene.Start(xuHuang: false);
        s.Reach(() => s.Offered());
        var draws = s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count);
        s.Activate(); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-subset") == true);
        var reveal = s.E<ProgramCardsRevealedEvent>().Single(e => e.SkillId == BoundaryMobileSupplyAndDuelScene.Shuangxiong);
        static bool Red(Suit suit) => suit is Suit.Heart or Suit.Diamond;
        var chosen = reveal.Cards.First(c => s.V(0).Hand.Any(h => Red(h.Suit) != Red(c.Suit)));
        s.Need(reveal.Cards.Count == 2 && reveal.Cards.All(c => s.P!.ValidCardIds.Contains(c.Id)),
            "The original replacement publicly reveals exactly two real top entities and offers their one-card subset.");
        s.Answer(c => c.Cards.SequenceEqual([chosen.Id])); s.Play();
        var other = reveal.Cards.Single(c => c.Id != chosen.Id);
        s.Need(s.V(0).Hand.Any(c => c.Id == chosen.Id) && s.G.CreateCardZoneDiagnostics().Any(c => c.CardId == other.Id && c.Location == CardLocation.DiscardPile) &&
            s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == draws,
            "The selected entity is gained, the other is discarded, and the normal two-card draw is replaced.");
        var duel = s.G.GetLegalActions(0).First(a => a.PlayedCardKind == CardKind.Duel && a.TargetSeats.SequenceEqual([1]) &&
            a.ConversionSource?.SkillId == BoundaryMobileSupplyAndDuelScene.Shuangxiong);
        s.Need(s.V(0).Hand.Any(c => c.Id == duel.CardId && Red(c.Suit) != Red(chosen.Suit)),
            "The granted native Duel uses one real opposite-color hand card.");
        var hp = s.V(0).Hp; s.PlayAction(duel);
        s.Reach(() => s.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 1 });
        var response = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("response") == "slash");
        var slash = response.Cards.Single(); s.Answer(c => c.Id == response.Id);
        s.Reach(() => s.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 0 });
        var useId = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel).Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("response") is "take-damage" or "pass");
        s.Reach(() => s.Offered());
        s.Need(s.V(0).Hp == hp - 1 && s.E<DuelResponseEvent>().Any(e => e.ResolutionId == useId && e.ResponderSeat == 1 && e.UsedSlash && e.SlashCardId == slash),
            "The real opponent's Slash makes the original Duel user take actual native damage before the optional claim.");
        s.Activate(); s.Reach(() => s.P?.SkillPrompt?.SkillId == BoundaryMobileSupplyAndDuelScene.Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.BoundaryYanLiangWenChouClaim is not null);
        s.Need(root.BoundaryYanLiangWenChouClaim is { } claim && claim.UseId == useId && claim.CardIds.SequenceEqual([slash]) &&
            !claim.CardIds.Contains(duel.CardId!.Value) && s.V(0).Hand.Any(c => c.Id == slash) &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == root.Id && w.Batch.AwaitingProgramFrameId == root.Id),
            "Only this Duel's other actual response entity is already gained in the original awaited movement batch; its original use cost is excluded.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.E<BoundaryYanLiangWenChouResponseClaimedEvent>().Count(e => e.UseId == useId && e.CardId == slash) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.Reason.Value == "skill.boundary-yan-liang-wen-chou.shuangxiong.claim") == 1 &&
            s.G.CardMovements.Count(m => m.CardId == duel.CardId && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == BoundaryMobileSupplyAndDuelScene.Shuangxiong && e.BindingId == "obtain-opponent-duel-slashes" && e.Completed) == 1,
            "Cold returns complete the original claim and original Duel once without repaying or collecting the Duel material.");
    }
}

internal sealed class BoundaryMobileSupplyAndDuelScene(ContentRegistry registry, GameEngine game, bool xuHuang)
{
    internal const string Shuangxiong = "boundary:shuangxiong-mobile", Watch = "fixture:boundary-mobile-supply-duel-watch";
    private const string Owner = "fixture:boundary-mobile-supply-duel-owner", Driver = "fixture:boundary-mobile-supply-duel-driver", Skip = "fixture:boundary-mobile-supply-duel-skip", Mode = "identity:classic-boundary-mobile-supply-duel";
    public GameEngine G = game;
    public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
    public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
    public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
    public bool Offered() => P?.SkillPrompt?.SkillId == Shuangxiong && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
    public void Draw(int target) => Accept(new UseProgramSkillCommand(0, Driver, "draw-one", [], [target], G.Revision, P!.PromptId));
    public void Play(int seat = 0) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
    public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
    public void PlayAction(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
        { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
    public void Answer(Func<PromptChoice, bool> test) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
    public void Accept(GameCommand c) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected fixed native command."); }
    public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { XuHuang = xuHuang, Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
    public void Reach(Func<bool> goal)
    {
        for (var n = 0; n < 220 && !goal(); n++)
        {
            if (P is { Kind: DecisionKind.SelectGeneral } pick) { Accept(new SelectGeneralCommand(pick.PlayerSeat, pick.PlayerSeat == 0 ? Owner : "fixture:boundary-mobile-supply-duel-peer-" + pick.PlayerSeat, G.Revision, pick.PromptId)); continue; }
            if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { End(foreign.PlayerSeat); continue; }
            if (P is { Kind: DecisionKind.ProgramTrigger or DecisionKind.RespondSlash or DecisionKind.RespondDodge }) throw new InvalidOperationException("Unexpected fixed mobile native prompt: " + JsonSerializer.Serialize(new { P.Kind, P.PlayerSeat, P.SkillPrompt, P.Choices }));
            Accept(new AdvanceOneStepCommand(G.Revision));
        }
        Need(goal(), "Missing bounded real native boundary.");
    }
    public void Cold()
    {
        string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements, Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
        var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
        Need(before == Dump(), "Five player views, native entities, original frame receipts and paid movement child survive real checkpoint JSON.");
    }
    public static BoundaryMobileSupplyAndDuelScene Start(bool xuHuang)
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(xuHuang));
        var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r);
        var s = new BoundaryMobileSupplyAndDuelScene(r, g, xuHuang); s.Accept(new StartGameCommand()); return s;
    }
    private sealed class Fixture(bool xuHuang) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-mobile-supply-duel", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, xuHuang ? "boundary:xu-huang-mobile" : "boundary:yan-liang-wen-chou-mobile");
            var reason = xuHuang ? "skill-program.boundary:jiezi-mobile.Draw" : "skill.boundary-yan-liang-wen-chou.shuangxiong.claim";
            object Label(string name) => new { name, description = "实际固定原生场景" };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "draw-one", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                    targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "draw", target = "selectedTarget", amount = 1 } } } } },
                new { id = Skip, revision = 1, triggers = new[] { new { id = "skip-own-draw", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "draw" } } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-gain-child", window = "cardsGained", subject = "owner", optional = false,
                    destinationZones = new[] { "hand" }, movementReasons = new[] { reason }, movementOccurrence = "perBatch",
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "seen", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = Label("原生摸一"), [Skip] = Label("本人真实跳摸"),
                [Watch] = new { name = "实际获得牌孩子", description = "实际固定原生场景", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动界真实固定人物", "supporter", xuHuang ? "boundary:duanliang-mobile" : Shuangxiong, xuHuang ? "wei" : "qun", 4,
                xuHuang ? ["boundary:jiezi-mobile", Driver, Skip, Watch] : [Driver, Watch]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:boundary-mobile-supply-duel-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "真实固定对手", "supporter", "standard:none", "wu", 6));
            const string deck = "fixture:boundary-mobile-supply-duel-deck";
            b.AddDeck(new(deck, "固定真实单实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(i => new ContentDeckPhysicalCard(
                xuHuang ? "standard:dodge" : "standard:slash", xuHuang || i % 2 == 0 ? Suit.Spade : Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "移动界真实原生验证", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}

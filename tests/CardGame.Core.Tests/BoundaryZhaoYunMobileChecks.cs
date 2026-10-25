using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryZhaoYunMobileChecks
{
    private const string Hero = "boundary:zhao-yun-mobile", Skill = "boundary:yajiao-mobile", Peer = "fixture:mobile-zhao-yun-peer",
        Caster = "fixture:mobile-zhao-yun-caster", NoDraw = "fixture:mobile-zhao-yun-no-draw", Quiet = "fixture:mobile-zhao-yun-quiet",
        Arrows = "fixture:mobile-zhao-yun-arrows", Gain = "fixture:mobile-zhao-yun-gain", Cost = "fixture:mobile-zhao-yun-cost";
    private const string GiftReason = "skill-program.boundary:yajiao-mobile.GiveMobileYajiaoRevealedCard",
        CostReason = "skill-program.boundary:yajiao-mobile.SelectAndMoveOwnedCard";

    public static void ForeignHandResponseGivesMismatchedTopBeforeOwnDiscardAndCold()
    {
        var s = Scene.Start(); s.Play();
        var material = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        var weapon = s.V().Equipment.Single(c => c.Kind == CardKind.Crossbow).Id;
        var rescue = s.V().Hand.Where(c => c.Kind is CardKind.Peach or CardKind.Alcohol).Select(c => c.Id).ToHashSet();
        s.Need(rescue.Count == 2 && !s.G.GetHumanLegalActions().Any(a => a.CardId is { } id && rescue.Contains(id) &&
                a.ConversionSource?.SkillId == "classic:longdan"),
            "The mobile general has the original two-way Longdan, with no OL Peach/Alcohol conversion.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge } && s.G.State.CurrentSeat == 3);
        var incoming = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.SourceSeat == 3 && f.CardKind == CardKind.ArrowBarrage).Id;
        var hp = s.V().Hp;
        s.Need(!s.E<ProgramCardTriggerResolvedEvent>().Any(e => e.SkillId == Skill),
            "Own-turn progression and ordinary hand retention do not invent an off-turn hand action.");
        s.Answer(c => c.Cards.SequenceEqual([material]) && c.Parameters.GetValueOrDefault("conversion-skill-id") == "classic:longdan");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => Action(c) == "activate"));
        s.Answer(c => Action(c) == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => Action(c) == "select-target"));
        var frameId = s.Root!.Id;
        var windowId = s.Root.WindowContext!.ParentFrameId;
        var revealed = s.E<ProgramCardsRevealedEvent>().Single(e => e.FrameId == frameId).Cards.Single();
        s.Need(s.Root.TriggerId == "reveal-on-hand-card-response" && revealed.Kind == CardKind.BaguaFormation &&
            s.Zone(revealed.Id) == CardLocation.Processing && s.P!.Choices.All(c => Action(c) == "select-target") &&
            s.P.Choices.Any(c => c.Targets.SequenceEqual([0])),
            "A genuine foreign-turn played Dodge reveals the fixed equipment top card and must name a recipient, including self, despite the category mismatch.");
        s.Cold(); s.Answer(c => c.Targets.SequenceEqual([0])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain);
        s.Need(s.Root?.Id == frameId && s.Root.PendingMovementContinuation is not null &&
            s.Zone(revealed.Id) == CardLocation.Hand(0) && s.Zone(weapon) == CardLocation.Equipment(0) &&
            s.G.CardMovements.Count(m => m.CardId == revealed.Id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) && m.Reason.Value == GiftReason) == 1 &&
            !s.G.CardMovements.Any(m => m.Reason.Value == CostReason),
            "The actual gift is already paid on its original frame and its movement child pauses before any mismatch discard.");
        s.Cold(); s.Continue();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => Action(c) == "select-and-move-owned-card"));
        s.Need(s.P!.Choices.Any(c => c.Cards.SequenceEqual([weapon])) && s.P.Choices.Any(c => c.Cards.SequenceEqual([revealed.Id])),
            "After the gift child, mismatched Yajiao discards from Zhao Yun's current HE, including the newly obtained card or an equipped weapon.");
        s.Answer(c => c.Cards.SequenceEqual([weapon])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Cost);
        s.Need(s.Root?.Id == frameId && s.Root.PendingMovementContinuation is not null && s.Zone(weapon) == CardLocation.DiscardPile,
            "The original frame also owns the actual equipped-card discard while its movement child is suspended.");
        s.Cold(); s.Continue(); s.Reach(() => s.E<CardUseFinishedEvent>().Any(e => e.ResolutionId == incoming));
        s.Need(s.V().Hp == hp && s.Zone(revealed.Id) == CardLocation.Hand(0) && s.Zone(material) == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == revealed.Id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) && m.Reason.Value == GiftReason) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == weapon && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == CostReason) == 1 &&
            s.E<ProgramCardTriggerResolvedEvent>().Count(e => e.FrameId == windowId && e.SkillId == Skill && e.Activated) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frameId && e.SkillId == Skill && e.Completed) == 1 &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 0 && e.CardId == material && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == incoming) == 1 &&
            s.E<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Skill) == 1 && s.G.ResolutionStack.All(f => f.Id != frameId),
            "Cold returns finish the original played Dodge and Arrow Barrage without damage, duplicate reveal, duplicate gift or repeated own discard.");
        s.Cold();
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> match)
        { var p = P!; var c = p.Choices.FirstOrDefault(match); Need(c is not null, "The exact current native Zhao Yun choice exists."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0, Kind: DecisionKind.Nullification }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                Need(P is not { PlayerSeat: 0 }, "Unexpected native mobile Zhao Yun boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native Zhao Yun scene missed its boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the exact foreign hand action, public top entity, paid gift and native own discard child.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why)
        { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, Root, G.State.CurrentSeat, G.State.Phase })); }
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Zhao Yun command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] };
            var caster = new ShanheCombatantDefinition(Caster, Hp: 8) { StartingHand = [] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-zhao-yun-native-response", Hero, 4, 0, 0, 0, 0, [NoDraw, Gain, Cost],
                [new(CardKind.Slash, Suit.Spade, 8), new(CardKind.Peach, Suit.Heart, 7), new(CardKind.Alcohol, Suit.Diamond, 9)],
                [new(CardKind.Crossbow, Suit.Club, 1)], [peer, caster], [peer], new string('a', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var s = new Scene(engine, registry); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhao-yun-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "classic:zhao-yun", Hero);
            object Pause(string id, string window, string reason, string[]? sourceZones = null, string[]? destinationZones = null) => new {
                id, window, subject = "owner", optional = false, sourceZones, destinationZones, movementOccurrence = "perBatch", movementReasons = new[] { reason },
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } } } };
            var skills = new object[] {
                new { id = NoDraw, revision = 1, triggers = new[] { new { id = "no-draw", window = "drawPhaseStarting", subject = "owner", optional = false,
                    isSkillInvocation = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "afterNormalDraw", subject = "owner", optional = false,
                    isSkillInvocation = false, effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = Arrows, revision = 1, triggers = new[] { new { id = "arrows", window = "playPhaseStarting", subject = "owner", optional = false,
                    isSkillInvocation = false, effects = new[] { new { op = "useVirtualOrdinaryTrick", target = "owner", actorRef = new { kind = "owner" }, targetRef = new { kind = "owner" }, outputKind = "arrowBarrage" } } } } },
                new { id = Gain, revision = 1, triggers = new[] { Pause("paid-gift", "cardsGained", GiftReason, destinationZones: ["hand"]) } },
                new { id = Cost, revision = 1, triggers = new[] { Pause("paid-cost", "cardsMoved", CostReason, sourceZones: ["equipment"]) } }
            };
            var labels = new Dictionary<string, object>();
            foreach (var id in new[] { NoDraw, Quiet, Arrows }) labels[id] = new { name = "固定原生回合", description = "真实外国回合万箭响应" };
            foreach (var id in new[] { Gain, Cost }) labels[id] = new { name = "真实涯角移动子窗", description = "真实赠牌或自己弃牌后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var options = new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }, options),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定安静同桌", "supporter", NoDraw, "wei", 8, [Quiet]));
            b.AddGeneral(new(Caster, "真实回合万箭", "supporter", NoDraw, "wei", 8, [Arrows]));
            const string deck = "fixture:mobile-zhao-yun-deck";
            b.AddDeck(new(deck, "固定异类装备顶牌", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 2)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界赵云原生回合外响应", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer, Caster], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileBuLianShiChecks
{
    private const string Hero = "mobile:bu-lian-shi", Skill = "mobile:anxu", Watch = "fixture:mobile-bu-lian-shi-gift", Peer = "fixture:mobile-bu-lian-shi-peer", Deck = "fixture:mobile-bu-lian-shi-deck",
        Owner = "fixture:mobile-bu-lian-shi-owner", Driver = "fixture:mobile-bu-lian-shi-prepare", Mode = "identity:mobile-bu-lian-shi-native";
    public static void DonorChoosesActualGiftAndPostChildEqualityCold()
    {
        foreach (var unequal in new[] { false, true })
        {
            var s = Scene.Start(unequal); s.Play(); var ownerHand = s.V(0).HandCount; var ownerHp = s.V(0).Hp;
            s.Accept(new UseProgramSkillCommand(0, Skill, "donor-gives-to-lower-hand", [], [], s.G.Revision, s.P!.PromptId));
            s.Reach(() => s.P?.Choices.Any(c => c.Targets.SequenceEqual(new[] { 1, 2 })) == true); s.Answer(c => c.Targets.SequenceEqual([1, 2]));
            s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            var donorCard = s.V(2).Hand.First().Id;
            s.Need(s.P!.PlayerSeat == 2 && s.P.Choices.Any(c => c.Cards.SequenceEqual(new[] { donorCard })) && s.V(2).Hand.First().Suit == Suit.Spade,
                "The higher-hand donor owns the real private choice and chooses their actual spade card; neither the lower-hand recipient nor Bu Lian Shi draws a hidden slot.");
            s.Answer(c => c.Cards.SequenceEqual([donorCard])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var rootId = root.Id;
            s.Need(s.V(1).Hand.Any(c => c.Id == donorCard) && s.V(0).Hp == ownerHp && s.V(0).HandCount == ownerHand &&
                !s.E<MobileBuLianShiAnxuComparedEvent>().Any(e => e.FrameId == rootId) && !s.E<ProgramCardsRevealedEvent>().Any(e => e.FrameId == rootId),
                "The real chosen hand entity is delivered before the native gain child, while no old non-spade reward or public reveal is manufactured.");
            if (!unequal) s.Cold();
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            if (!unequal)
            {
                s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "mobile-anxu-benefit") == true);
                s.Need(s.E<MobileBuLianShiAnxuComparedEvent>().Single(e => e.FrameId == rootId) is { RecipientHand: 2, DonorHand: 2, Equal: true },
                    "Only after the paid gift child returns does the original frame compare the actual equal hands and offer the owner's reward.");
                s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "recover");
            }
            s.Play();
            s.Need(s.V(0).Hp == ownerHp + (unequal ? 0 : 1) && s.V(0).HandCount == ownerHand && s.V(1).HandCount == 2 && s.V(2).HandCount == (unequal ? 3 : 2) &&
                s.E<MobileBuLianShiAnxuComparedEvent>().Single(e => e.FrameId == rootId).Equal == !unequal &&
                s.G.CardMovements.Count(m => m.CardId == donorCard && m.From == CardLocation.Hand(2) && m.Reason.Value == "skill-program.mobile:anxu.SelectAndMoveOwnedCard") == 1 &&
                s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == rootId && e.Completed) == 1 && s.G.GetLegalActions(0).All(a => a.ProgramSkillId != Skill),
                "Equal hands permit exactly one recovery despite the spade gift; unequal hands give neither reward. Both use the same actual per-Play activation without repeating the gift.");
        }
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, int? target = null) => Accept(new UseProgramSkillCommand(0, Driver, id, [], target is { } seat ? [seat] : [], G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Bu Lian Shi command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 0 ? Owner : Peer + "-" + setup.PlayerSeat, G.Revision, setup.PromptId)); continue; }
                Need(P is null, "Unexpected mobile Bu Lian Shi prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Missing bounded mobile Bu Lian Shi boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original donor's paid gift and native gain child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, V(0).Hp, Hand = V(0).HandCount })); }
        public static Scene Start(bool unequal)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(unequal));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r));
            s.Accept(new StartGameCommand()); s.Play(); s.Use("hurt"); s.Play();
            s.Need(s.V(0).Hp == s.V(0).MaxHp - 1, "The supported actual identity owner becomes wounded through a real native HP loss, including the mode's Lord HP bonus.");
            foreach (var seat in new[] { 1, 2 })
                while (s.V(seat).HandCount > 0)
                {
                    s.Use("trim-peer", seat); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
                    s.Answer(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand"); s.Play();
                }
            s.Use("low-hand", 1); s.Play(); s.Use("high-hand", 2); s.Play(); return s;
        }
    }
    private sealed class Fixture(bool unequal) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-bu-lian-shi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:bu-lian-shi");
            var skills = new object[] { new { id = Watch, revision = 1, triggers = new[] { new { id = "original-donor-gift", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                movementReasons = new[] { "skill-program.mobile:anxu.SelectAndMoveOwnedCard" }, optional = false, usageScope = "game", usageLimit = 1,
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } } } } },
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "hurt", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "loseHp", target = "owner", amount = 1 } } },
                    new { id = "low-hand", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "draw", target = "selectedTarget", amount = 1 } } },
                    new { id = "high-hand", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "draw", target = "selectedTarget", amount = unequal ? 4 : 3 } } },
                    new { id = "trim-peer", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "selectedTarget" }, zones = new[] { "hand" }, count = 1, destination = "discardPile", resultBind = "trim", awaitMovementTriggers = true } } }
                } } };
            var labels = new Dictionary<string, object> { [Watch] = new { name = "真实安恤交牌孩子", description = "先实际交牌，再比较双方手牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                [Driver] = new { name = "真实身份准备", description = "用原生失去体力、弃牌和摸牌构造固定手牌数。" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "真实移动安恤身份", "supporter", Skill, "wu", 3, AdditionalSkillIds: ["classic:zhuiyi", Driver], Gender: GeneralGender.Female));
            foreach (var seat in new[] { 1, 2, 3 }) b.AddGeneral(new(Peer + "-" + seat, "真实同行", "supporter", seat == 1 ? Watch : "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定补牌", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(Mode, "移动步练师真实身份交牌", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 1 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer + "-1", Peer + "-2", Peer + "-3"]));
        }
    }
}

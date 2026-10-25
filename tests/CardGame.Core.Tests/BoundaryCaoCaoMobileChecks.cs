using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryCaoCaoMobileChecks
{
    private const string Hero = "boundary:cao-cao-mobile", Skill = "boundary:jianxiong-mobile", Owner = "fixture:mobile-cao-cao-owner",
        Driver = "fixture:mobile-cao-cao-driver", Watch = "fixture:mobile-cao-cao-claim-child", Mode = "identity:classic-mobile-cao-cao",
        ClaimReason = "skill-program.boundary:jianxiong-mobile.ClaimDamageCards", DrawReason = "skill-program.boundary:jianxiong-mobile.Draw";

    public static void NativeClaimChildrenPrecedeDrawAndCardlessDamageStillDrawsCold()
    {
        var s = Scene.Start(); s.Play();
        var a = s.G.GetLegalActions(0).First(a => a.PlayedCardKind == CardKind.Duel && a.TargetSeats.SequenceEqual([1]));
        var material = a.CardId!.Value;
        s.Accept(new PlayCardCommand(0, material, a.TargetSeats, s.G.Revision, s.P!.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        s.Reach(() => s.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 1 }); s.Answer(c => c.Cards.Count == 1);
        s.Reach(() => s.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 0 }); s.Answer(c => c.Cards.Count == 0);
        s.Reach(s.Offered); var hand = s.V(0).HandCount; var hp = s.V(0).Hp;
        s.Activate(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var frameId = frame.Id;
        s.Need(frame.InstructionIndex == 1 && frame.PendingMovementContinuation is { SubjectSeat: 0 } &&
            s.V(0).HandCount == hand + 1 && s.V(0).Hand.Any(c => c.Id == material) && s.Moves(DrawReason) == 0 &&
            s.E<ProgramDamageCardsClaimedEvent>().Count(e => e.FrameId == frameId && e.OwnerSeat == 0 && e.CardIds.SequenceEqual([material])) == 1,
            "The actual Duel material is obtained on the original damage frame before the mobile draw; its genuine gain child interrupts the still-paid claim instruction.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"); s.Play();
        s.Need(s.V(0).HandCount == hand + 2 && s.V(0).Hp == hp && s.V(0).Hand.Any(c => c.Id == material) &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) && m.Reason.Value == ClaimReason) == 1 &&
            s.Moves(DrawReason) == 1 && s.V(0).Skills!.All(k => k.ContentId != Skill) &&
            s.E<ProgramDamageCardsClaimedEvent>().Count(e => e.FrameId == frameId) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frameId) == 1,
            "Cold return completes the original native claim once, then draws once despite genuine source loss in its gain child, without repaying the physical Duel or repeating damage.");

        s.Accept(new UseProgramSkillCommand(0, Driver, "restore-skill", [], [], s.G.Revision, s.P!.PromptId)); s.Play();
        var before = s.V(0).HandCount; var claimCount = s.E<ProgramDamageCardsClaimedEvent>().Count();
        s.Accept(new UseProgramSkillCommand(0, Driver, "cardless", [], [], s.G.Revision, s.P!.PromptId)); s.Reach(s.Offered);
        s.Need(s.V(0).Hp == hp - 1, "A genuine standalone native damage instance has no attack material to claim.");
        s.Activate(); s.Play();
        s.Need(s.V(0).HandCount == before + 1 && s.E<ProgramDamageCardsClaimedEvent>().Count() == claimCount && s.Moves(DrawReason) == 2,
            "The independent mobile draw remains available after cardless damage instead of inheriting the old claim-only physical-card prerequisite.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int Moves(string reason) => G.CardMovements.Count(m => m.Reason.Value == reason && m.To == CardLocation.Hand(0));
        public bool Offered() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Cao Cao command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 160 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-cao-cao-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger or DecisionKind.RespondSlash or DecisionKind.RespondDodge }) throw new InvalidOperationException("Unexpected mobile Cao Cao boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Bounded mobile Cao Cao scene missed the expected native boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original damage claim and its native movement child before drawing."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + Short()); }
        public static Scene Start()
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, r)); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-cao-cao-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:cao-cao");
            var skills = new object[] {
                new { id = Driver, revision = 1, viewAs = new[] { new { id = "actual-duel", inputKinds = new[] { "slash" }, inputSuits = Array.Empty<string>(), outputKind = "duel", forPlay = true, forResponse = false, singleCardTrickUse = true } },
                    activations = new object[] { new { id = "cardless", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "damage", target = "owner", amount = 1 } } },
                        new { id = "restore-skill", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                            effects = new[] { new { op = "grantSkills", target = "owner", skillIds = new[] { Skill } } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-claim-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { ClaimReason }, optional = false,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "claim-child", options = new[] { new { id = "continue" } } },
                        new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实伤害驱动", description = "原生实体决斗与无实体伤害" }, [Watch] = new { name = "真实奸雄获得子窗", description = "先获得后摸牌", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            var extra = Enumerable.Range(0, 6).Select(i => "fixture:mobile-cao-cao-dummy-" + i).ToArray();
            foreach (var id in extra) b.AddSkill(new(id, "真实技能替换资格", "供原生获得子窗选择一次失去技能。"));
            b.AddGeneral(new(Owner, "移动界曹操原生场景", "supporter", Skill, "wei", 4, AdditionalSkillIds: [Driver, Watch, .. extra]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-cao-cao-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "原生角色", "supporter", "standard:none", "wu", 5));
            const string deck = "fixture:mobile-cao-cao-deck";
            b.AddDeck(new(deck, "固定真实实体杀", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "移动界曹操原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}

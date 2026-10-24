using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryWangYiMobileChecks
{
    private const string Hero = "boundary:wang-yi-mobile", Zhenlie = "boundary:zhenlie-mobile", Miji = "boundary:miji-mobile", Owner = "fixture:mobile-wang-yi-owner",
        Driver = "fixture:mobile-wang-yi-driver", Hp = "fixture:mobile-wang-yi-hp", Mode = "identity:classic-mobile-wang-yi";
    private const string DiscardReason = "skill-program." + Zhenlie + ".SelectAndMoveOwnedCard", GiftReason = "skill-program." + Miji + ".DistributeOwnedCards", DrawReason = "skill-program." + Miji + ".Draw";

    public static void PaidNullificationThenForcedSourceDiscardNativeChildrenCold()
    {
        var s = Scene.Start(); s.Play(); s.Use("equip", 1); s.Play(); s.Use("hurt", 1); s.Play();
        var lion = s.V(1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id; var before = s.V(0).Hp;
        s.Use("enemy-duel", 1); s.Reach(() => s.Offered(Zhenlie)); s.Activate(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Zhenlie); var frameId = root.Id; var use = root.PaidOwnTarget!.Use;
        s.Need(use.ActorSeat == 1 && use.TargetSeat == 0 && use.EffectiveKind == CardKind.Duel && root.PaidOwnTarget is { ActualLost: 1, Applied: false } &&
            s.V(0).Hp == before - 1 && s.E<PaidOwnTargetAppliedEvent>().All(e => e.ProgramFrameId != frameId),
            "The original opponent's actual ordinary Duel freezes its use and pays one real HP; nullification waits for the HP child.");
        s.Cold(); s.Answer(_ => true); s.Reach(() => s.P?.SkillPrompt?.SkillId == Zhenlie && s.Action("select-and-move-owned-card"));
        s.Need(s.E<PaidOwnTargetAppliedEvent>().Single(e => e.ProgramFrameId == frameId).CardUseFrameId == use.CardUseFrameId &&
            s.P!.Choices.All(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") &&
            s.P.Choices.Where(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand").All(c => c.Cards.Count == 0),
            "Paid nullification applies once to that exact use before a mandatory HE discard; mobile Zhenlie offers no OL obtain/Ending/skip benefit.");
        s.Answer(c => c.Cards.SequenceEqual([lion])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp && s.P.PlayerSeat == 1);
        s.Need(s.V(1).Hp == s.V(1).MaxHp && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Zhenlie).Id == frameId &&
            s.G.CardMovements.Count(m => m.CardId == lion && m.From == CardLocation.Equipment(1) && m.To == CardLocation.DiscardPile && m.Reason.Value == DiscardReason) == 1,
            "The forced source discard removes the actual armor and awaits native recovery on the same paid program frame.");
        s.Cold(); s.Answer(_ => true); s.Play();
        s.Need(s.V(0).Hp == before - 1 && s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frameId) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Zhenlie && e.Completed && e.Activated) == 1 &&
            s.E<EarnedActualEndingBenefitIssuedEvent>().All(e => e.Benefit.ProducerProgramFrameId != frameId) &&
            s.G.CardMovements.Count(m => m.CardId == lion && m.Reason.Value == DiscardReason) == 1,
            "The native Duel completes ineffective for Wang Yi after one HP payment and one discard, without repeating either child or issuing OL earned Ending.");
    }

    public static void MijiChoosesFewerOrZeroThenOptionallyGivesEqualHandCountCold()
    {
        var s = Scene.Start(); s.Play(); s.Use("equip", 0); s.Play(); s.Use("hurt-two", 0); s.Play(); s.EndPlay();
        s.Reach(() => s.Offered(Miji)); s.Activate(); s.Reach(() => s.Action("mobile-miji-count"));
        var before = s.V(0).HandCount; var old = s.V(0).Hand.Select(c => c.Id).ToArray(); var recipient = s.V(2).HandCount;
        s.Need(s.P!.Choices.Select(c => c.Parameters["count"]).SequenceEqual(new[] { "0", "1", "2" }) && s.V(0).MaxHp - s.V(0).Hp == 2,
            "The actual Ending offers the complete 0..lost-HP draw domain instead of the OL fixed maximum draw.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("count") == "1"); s.Reach(() => s.Action("distribute-owned-card"));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Miji); var frame = root.Id;
        var drawn = root.CardSetBindings.Single(b => b.Name == "drawn").CardIds.Single();
        s.Need(root.BoundaryWangYiMijiCount is { Maximum: 2, Count: 1 } && root.OwnedCardDistribution is { RequiredCount: 1, Zones: [CardZoneKind.Hand], AllowDeclineBeforeFirst: true } &&
            s.V(0).HandCount == before + 1 && !old.Contains(drawn) && s.P!.Choices.Where(c => c.Parameters.GetValueOrDefault("program-action") == "distribute-owned-card").All(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand") &&
            s.Action("decline-owned-card-distribution"),
            "Choosing below lost HP draws exactly that count, freezes an optional equal-count Hand distribution, and excludes equipment rather than using OL HE up-to-X gifts.");
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([old[0]]) && c.Targets.SequenceEqual([2]));
        s.Reach(() => s.E<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Miji && e.Completed));
        s.Need(s.V(2).HandCount == recipient + 1 && s.V(2).Hand.Any(c => c.Id == old[0]) && s.V(0).Hand.Any(c => c.Id == drawn) && s.V(0).HandCount == before &&
            s.E<ProgramOwnedCardDistributedEvent>().Count(e => e.FrameId == frame && e.SkillId == Miji) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == old[0] && m.To == CardLocation.Hand(2) && m.Reason.Value == GiftReason) == 1 &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == DrawReason) == 1,
            "The selected old hand card may pay the native equal gift; the drawn card can stay, with exactly one counted draw and one physical delivery after cold restore.");

        var zero = Scene.Start(); zero.Play(); zero.Use("hurt-two", 0); zero.Play(); zero.EndPlay(); zero.Reach(() => zero.Offered(Miji)); zero.Activate(); zero.Reach(() => zero.Action("mobile-miji-count"));
        zero.Answer(c => c.Parameters.GetValueOrDefault("count") == "0"); zero.Reach(() => zero.E<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Miji && e.Completed));
        zero.Need(zero.E<BoundaryWangYiMijiCountChosenEvent>().Single().Count == 0 && zero.G.CardMovements.All(m => m.Reason.Value is not (DrawReason or GiftReason)),
            "Choosing zero is legal and completes without manufactured draws, gifts or a second count prompt.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Action(string value) => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == value) == true;
        public bool Offered(string skill) => P?.SkillPrompt?.SkillId == skill && Action("activate");
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Wang Yi command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-wang-yi-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId == Hp) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Wang Yi boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Bounded mobile Wang Yi scene missed the expected native boundary.");
        }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original paid actual-use target and scalar count choice with native draw/gift continuations."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + Short()); }
        public static Scene Start()
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r)); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-wang-yi-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Activation(string id, object[] effects, int? perPhase = null) => new { id, minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = id == "enemy-duel" ? "otherLiving" : "anyLiving", usesPerTurn = (int?)null, usesPerPhase = perPhase, effects };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] {
                    Activation("equip", [new { op = "useRandomDeckEquipment", target = "owner", resultBind = "equipped" }]),
                    Activation("hurt", [new { op = "loseHp", target = "selectedTarget", amount = 1 }]),
                    Activation("hurt-two", [new { op = "loseHp", target = "selectedTarget", amount = 2 }]),
                    Activation("enemy-duel", [new { op = "useSelectedActorDuel", target = "owner" }], 2) } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "hp-child", window = "afterHealthChanged", subject = "owner", optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "原生对手用牌与体力", description = "真实Du​​el装备与失血" }, [Hp] = new { name = "原生体力孩子", description = "实际HP变化后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddCard(new("classic:silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            b.AddGeneral(new(Owner, "移动界王异真实场景", "supporter", Zhenlie, "wei", 3, AdditionalSkillIds: [Miji, Driver, Hp], Gender: GeneralGender.Female));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-wang-yi-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定真实角色", "supporter", "standard:none", "wu", 4, AdditionalSkillIds: [Hp]));
            const string deck = "fixture:mobile-wang-yi-deck";
            b.AddDeck(new(deck, "真实白银狮子实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(i => new ContentDeckPhysicalCard("classic:silver-lion", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界王异原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}

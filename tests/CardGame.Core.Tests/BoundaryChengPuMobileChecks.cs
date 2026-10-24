using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryChengPuMobileChecks
{
    private const string Hero = "boundary:cheng-pu-mobile", Lihuo = "boundary:lihuo-mobile", Chunlao = "boundary:chunlao-mobile", Owner = "fixture:mobile-cheng-pu-owner",
        Driver = "fixture:mobile-cheng-pu-driver", Hp = "fixture:mobile-cheng-pu-hp", Entry = "fixture:mobile-cheng-pu-entry", Wine = "fixture:mobile-cheng-pu-wine", Mode = "identity:classic-mobile-cheng-pu";

    public static void ConvertedChainAndNaturalFireActualUseTotalsCold()
    {
        foreach (var natural in new[] { false, true })
        {
            var s = Scene.Start(natural: natural); s.Play();
            foreach (var seat in new[] { 1, 2, 3 }) { s.Use("chain", seat); s.Play(); }
            var hp = s.V(0).Hp; var before = s.G.Events.Count;
            var action = s.Slash(natural);
            s.Need(s.G.PredictPublicActionDamage(0, action, 1).ActualHpLost == (natural ? 1 : 2), "Public action prediction includes chained conversion damage but does not add it to a natural FireSlash.");
            s.PlayAction(action); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Lihuo);
            var receipt = root.BoundaryChengPuLihuo!; var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.Id == receipt.UseId);
            var damage = s.G.Events.Skip(before).Select(e => e.Payload).OfType<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == use.Id).ToArray();
            var perTarget = natural ? 1 : 2; var loss = natural ? 1 : 3;
            s.Need(damage.Length == 3 && damage.Select(e => e.TargetSeat).Order().SequenceEqual([1, 2, 3]) && damage.All(e => e.Amount == perTarget) &&
                receipt is { Paid: true } && receipt.TotalDamage == perTarget * 3 && receipt.RequestedLoss == loss && s.V(0).Hp == hp - loss &&
                use.CardKind == CardKind.FireSlash && (use.Action!.ConversionChain.Any(c => c.SkillId == Lihuo) == !natural),
                "All actual direct and chain damage belongs to one real FireSlash; converted damage adds once before propagation and the complete total pays floor(total/2) once, including natural FireSlash.");
            var frame = root.Id; s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
            s.Need(s.E<BoundaryChengPuLihuoPaidEvent>().Count(e => e.FrameId == frame) == 1 && s.E<CardUseFinishedEvent>().Any(e => e.CardKind == CardKind.FireSlash) && s.V(0).Hp == hp - loss,
                "The paid original frame returns from its genuine HP observer after cold restore without another loss or another chain application.");
        }
        var one = Scene.Start(); one.Play(); var hpBefore = one.V(0).Hp; one.PlayAction(one.Slash(false)); one.Play();
        one.Need(one.E<DamageAppliedEvent>().Single().Amount == 1 && one.V(0).Hp == hpBefore && one.E<BoundaryChengPuLihuoPaidEvent>().Length == 0,
            "A converted unchained one-damage FireSlash has no two-point unit to pay; damage remains one and HP is unchanged.");
    }

    public static void StoredSlashesPayOnceThenNativeSelfAlcoholAfterLihuoDyingCold()
    {
        var s = Scene.Start(initialHp: 2); s.Play(); var hand = s.V(0).Hand.Select(c => c.Id).ToArray();
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Reach(() => s.P?.SkillPrompt?.SkillId == Chunlao);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        var stored = s.P!.Choices.Where(c => c.Cards.Count == 1).Take(2).Select(c => c.Cards[0]).ToArray();
        s.Need(stored.Length == 2 && stored.All(hand.Contains), "Only current hand Slashes are privately selected for the initially empty Chun pile.");
        s.Answer(c => c.Cards.SequenceEqual([stored[0]])); s.Cold();
        s.Answer(c => c.Cards.SequenceEqual([stored[1]]));
        if (s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") == true) s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards");
        s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        s.Need(s.V(0).ChunlaoCards!.Select(c => c.Id).Order().SequenceEqual(stored.Order()) && stored.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Chunlao(0)) == 1),
            "Ending stores the selected real hand entities once and retains the public pile across the next complete native turn cycle.");
        foreach (var target in new[] { 1, 2 }) { s.Use("chain", target); s.Play(); }
        s.PlayAction(s.Slash(false)); s.Reach(() => s.P?.SkillPrompt?.SkillId == Entry);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Lihuo); var rootId = root.Id;
        s.Need(root.BoundaryChengPuLihuo is { TotalDamage: 4, RequestedLoss: 2, Paid: true, HpBefore: 2, HpAfter: 0 } && s.G.ResolutionStack.OfType<DyingFrame>().Single().ParentFrameId == rootId,
            "The completed original FireSlash pays its genuine two-HP total before its own native Dying entry child; the cost is already final.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Reach(() => s.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "program-trigger" && c.Parameters.GetValueOrDefault("skill-id") == Chunlao);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-source-card") == true);
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([stored[0]])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Wine);
        s.Need(s.V(0).Hp == 1 && s.V(0).ChunlaoCards!.Count == 1 && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == rootId && f.BoundaryChengPuLihuo!.Paid) &&
            s.E<CardUseDeclaredEvent>().Any(e => e.CardId == stored[0] && e.CardKind == CardKind.Alcohol && e.SourceSeat == 0),
            "Spending one Chun issues an actual victim-owned Alcohol use and native recovery while the original completed-FireSlash payment remains paid below its children.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(0).Hp == 1 && s.V(0).ChunlaoCards!.Count == 1 && s.E<BoundaryChengPuLihuoPaidEvent>().Count(e => e.FrameId == rootId) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == stored[0] && m.From == CardLocation.Chunlao(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Alcohol && e.CardId == stored[0]) == 1,
            "Returning the cold native Alcohol completion consumes one real pile entity once, finishes the original FireSlash once and never repeats its HP cost.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public LegalAction Slash(bool natural) => G.GetLegalActions(0).First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1]) &&
            (natural ? a.ConversionSource is null : a.ConversionSource?.SkillId == Lihuo) && (natural || a.PlayedCardKind == CardKind.FireSlash));
        public void PlayAction(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected native mobile Cheng Pu command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 250 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-cheng-pu-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                if (P is { Kind: DecisionKind.RescueDying }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                if (P?.SkillPrompt?.SkillId is Hp or Entry or Wine) { Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); continue; }
                if (P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip") == true) { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Cheng Pu prompt: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The bounded mobile Cheng Pu scene missed its native boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves exact native Use, applied damage totals, paid HP, pile materials and typed child returns."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string reason) { if (!ok) throw new InvalidOperationException(reason + " " + JsonSerializer.Serialize(new { P, Views = Enumerable.Range(0, 5).Select(V).Select(p => new { p.Seat, p.Hp, p.HandCount, p.IsChained }) })); }
        public static Scene Start(bool natural = false, int initialHp = 4)
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(natural, initialHp)); var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 15 }, r); var s = new Scene(r, g); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture(bool natural, int initialHp) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-cheng-pu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Driver}}","revision":1,"activations":[{"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]}]},
             {"id":"{{Hp}}","revision":1,"triggers":[{"id":"native-loss","window":"afterHpLost","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{Entry}}","revision":1,"triggers":[{"id":"native-entry","window":"dyingEntering","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"entered","options":[{"id":"continue"}]}]}]},
             {"id":"{{Wine}}","revision":1,"triggers":[{"id":"native-alcohol","window":"cardUseCompleted","ownerRelation":"actor","cardKinds":["alcohol"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"wine-used","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Driver] = new { name = "真实连环", description = "原生连环状态" }, [Hp] = Pause("原生失血孩子"), [Entry] = Pause("原生濒死进入"), [Wine] = Pause("真实酒完成") } });
            var c = SkillProgramCatalog.Load(rules, presentation); foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            // Identity mode adds the Lord opening HP point after this base value.
            b.AddGeneral(new(Owner, "移动界程普实际原帧", "supporter", Lihuo, "wu", 4, AdditionalSkillIds: [Chunlao, Driver, Hp, Entry, Wine], Gender: GeneralGender.Male) { InitialHp = initialHp - 1 });
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-cheng-pu-peer-" + i).ToArray(); foreach (var id in peers) b.AddGeneral(new(id, "连环实际角色", "supporter", "standard:none", "wu", 6, Gender: GeneralGender.Male));
            const string deck = "fixture:mobile-cheng-pu-deck"; b.AddDeck(new(deck, "固定真实杀", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 120).Select(i => new ContentDeckPhysicalCard(natural ? "standard:fire_slash" : "standard:slash", Suit.Heart, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界程普真实身份", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
        private static object Pause(string name) => new { name, description = "真实原帧子结算暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
    }
}

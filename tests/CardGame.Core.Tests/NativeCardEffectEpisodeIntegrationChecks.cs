using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativeCardEffectEpisodeIntegrationChecks
{
    private const string Hero = "fixture:episode-native-owner", Peer = "fixture:episode-native-peer";
    private const string Driver = "fixture:episode-native-driver", Quiet = "fixture:episode-native-quiet";
    private const string Late = "fixture:episode-native-late", Fengzi = "ol:fengzi", Zixi = "le:zixi";

    public static void RealPaidWholeAndLocalEpisodesKeepLateTargetsAndIssuedSourceRightsCold()
    {
        var s = Scene.Start();
        var delayed = s.Hand().Single(c => c.Kind == CardKind.Indulgence).Id;
        s.Card(delayed, [1]); s.Play();
        s.Need(s.V(1).Judgment.Any(c => c.Id == delayed) && !s.E<NativeCardEffectEpisodeIssuedEvent>().Any(),
            "A real delayed placement establishes one Judgment card without spending Fengzi or fabricating a Zixi episode.");
        var material = s.Hand().Single(c => c.Kind == CardKind.Slash).Id;
        var cost = s.Hand().Single(c => c.Kind == CardKind.Dodge).Id;
        var hp1 = s.V(1).Hp; var hp3 = s.V(3).Hp;
        s.Card(material, [1]);
        s.Reach(() => s.Optional(Fengzi));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Fengzi && s.P.Choices.Any(c => c.Cards.SequenceEqual([cost])));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "cost" && c.Cards.SequenceEqual([cost]));
        s.Reach(() => s.Optional(Zixi));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Late && s.P.Choices.Any(c => c.Targets.SequenceEqual([3])));
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        s.Need(use.TargetSeats.SequenceEqual([1]) && use.EffectEpisodes is { Episodes.Count: 2 },
            "Both real writers issue their independent paid whole-use and accepted local episodes before a later target is chosen.");
        s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-target" && c.Targets.SequenceEqual([3]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Late && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var changed = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == use.Id);
        s.Need(changed.TargetSeats.SequenceEqual([1, 3]) && s.V(0).Skills?.All(k => k.ContentId is not (Fengzi or Zixi)) == true &&
            s.E<NativeCardEffectEpisodeStartedEvent>().All(e => e.UseFrameId != use.Id),
            "The native target addition and actual loss of both issuing skills happen before any original or repeated damage.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        var started = s.E<NativeCardEffectEpisodeStartedEvent>().Where(e => e.UseFrameId == use.Id).ToArray();
        s.Need(started.Select(e => e.Episode.Issue.Source.SkillId).SequenceEqual([Fengzi, Zixi]) &&
            started[0].Cursor.TargetSeats.SequenceEqual([1, 3]) && started[1].Cursor.TargetSeats.SequenceEqual([1]) &&
            s.V(1).Hp == hp1 - 3 && s.V(3).Hp == hp3 - 2 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 3 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 3 && e.Amount == 1) == 2 &&
            s.E<NativeCardEffectEpisodeCompletedEvent>().Count(e => e.UseFrameId == use.Id && e.Executed) == 2 &&
            s.E<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.Reason.Value == "skill-program.extra-effect.cost") == 1,
            "One real Slash and one Fengzi payment resolve the original and whole-use passes against both targets, then the one local pass, after both sources disappear.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0, true).Players[seat];
        public IReadOnlyList<CardSnapshot> Hand() => V(0).Hand;
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Optional(string skill) => P?.SkillPrompt?.SkillId == skill &&
            P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected episode fixture command."); }
        public void Answer(Func<PromptChoice, bool> select)
        { var p = P!; var choice = p.Choices.Single(select); Accept(new AnswerPromptCommand(0, p.PromptId, choice.Id, G.Revision)); }
        public void Card(int id, IReadOnlyList<int> targets) => Accept(new PlayCardCommand(0, id, targets, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "Unexpected terminal before the next episode boundary.");
                if (P is not { PlayerSeat: 0 } p) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                    Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.Kind == DecisionKind.Nullification)
                    Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                else throw new InvalidOperationException("Unexpected original-use episode boundary " + Diagnostic());
            }
            throw new InvalidOperationException("Missing bounded native episode boundary " + Diagnostic());
        }
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == State(), "Cold replay retains original episode order, source loss, actual targets and physical payment.");
        }
        private string State() => JsonSerializer.Serialize(new { G.State, Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
        private string Diagnostic() => JsonSerializer.Serialize(new { P, Frames = G.ResolutionStack, Hp = Enumerable.Range(0, 4).Select(i => V(i).Hp), Facts = G.Events.TakeLast(8).Select(e => e.Payload.ToString()) });
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + Diagnostic()); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(31, 1, "native-effect-episodes", Hero, 12, 0, 0, 0, 0, [],
                [new(CardKind.Indulgence, Suit.Diamond, 6), new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Dodge, Suit.Club, 2)], [],
                [new(Peer, Hp: 12) { StartingHand = [] }, new(Peer, Hp: 12) { StartingHand = [] }],
                [new(Peer, Hp: 12) { StartingHand = [] }], new string('f', 64)) { StartingResourcesExpireOnUse = false };
            var g = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-effect-episodes", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "le:xiao-qiao", "le:da-qiao", "ol:wang-rong");
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":20,"priority":0}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"fixed-opening","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Late}}}","revision":1,"triggers":[{"id":"later-target-and-source-loss","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],"priority":-1000,"optional":false,
                    "condition":{"kind":"compare","left":{"kind":"cardUseDesignatedTargetCount"},"operator":"equal","right":{"kind":"integerConstant","value":1}},
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},{"op":"addCurrentCardUseTarget","target":"selectedTarget"},
                      {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["ol:fengzi","le:zixi"]}},
                      {"op":"chooseOption","target":"owner","resultBind":"held","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, Late }.ToDictionary(id => id, id => new { name = id, description = "Native episode fixed boundary",
                        optionLabels = id == Late ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Native episode owner", "", Driver, "wu", 12, [Quiet, Late, Fengzi, Zixi]));
            b.AddGeneral(new(Peer, "Native episode peer", "", Quiet, "shu", 12));
            foreach (var kind in new[] { CardKind.Indulgence, CardKind.Slash, CardKind.Dodge })
            { var card = CardCatalog.Get(kind); b.AddCard(new("fixture:episode-card-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            b.AddDeck(new("fixture:episode-native-deck", "Fixed native episode deck", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 36).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Native episode fixture", 4, 4, new Dictionary<string, int>(), "fixture:episode-native-deck",
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativeCardEffectEpisodePlacementChecks
{
    private const string Skill = "mou:qiaodui", Hero = "fixture:episode-placement-owner", Peer = "fixture:episode-placement-peer";
    private const string Quiet = "fixture:episode-placement-quiet", Hold = "fixture:episode-placement-hold";

    public static void RealQiaoduiGiftConsumesDelayedEpisodeWithoutPlacingTheEntityAgainCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "native-episode-placement", Hero, 12, 0, 0, 0, 0, [],
            [new(CardKind.Indulgence, Suit.Diamond, 6), new(CardKind.Dodge, Suit.Club, 2)], [],
            [new(Peer, Hp: 12) { StartingHand = [] }, new(Peer, Hp: 12) { StartingHand = [] }],
            [new(Peer, Hp: 12) { StartingHand = [] }], new string('f', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
        var s = new Scene(game, registry); s.Accept(new StartGameCommand()); s.Play();
        var material = s.V(0).Hand.Single(c => c.Kind == CardKind.Indulgence).Id;
        var gift = s.V(0).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        s.Accept(new PlayCardCommand(0, material, [1], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "card"));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([gift]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "confirm");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "recipient" && c.Targets.SequenceEqual([2]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        var issued = use.EffectEpisodes!.Episodes.Single();
        s.Need(issued.Issue.Source.SkillId == Skill && issued.Issue.Spec.Replay == NativeCardEffectEpisodeReplay.ConsumePhysicalPlacement &&
            issued.Started == 0 && s.V(1).Judgment.Count == 0 && s.V(2).Hand.Any(c => c.Id == gift) &&
            s.G.CardMovements.Count(m => m.CardId == gift && m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(2) &&
                m.Reason.Value == "skill-program.mou:qiaodui.give") == 1 &&
            s.E<MouHqCommittedEvent>().Count(e => e.OwnerSeat == 0 && e.Recipient == 2 && e.Count == 1) == 1,
            "The real finalized delayed-use writer issues only after its original one-card gift actually arrives and all Give children return.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(1).Judgment.Count(c => c.Id == material) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.Judgment(1)) == 1 &&
            s.E<NativeCardEffectEpisodeStartedEvent>().Count(e => e.UseFrameId == use.Id && e.Episode.Id == issued.Id) == 1 &&
            s.E<NativeCardEffectEpisodeCompletedEvent>().Count(e => e.UseFrameId == use.Id && e.Episode.Id == issued.Id && !e.Executed) == 1 &&
            s.E<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.To.Zone == CardZoneKind.Judgment) == 1,
            "The accepted extra delayed-placement opportunity is consumed once; one original physical entity enters Judgment once and its native use finishes once.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0, true).Players[seat];
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected delayed episode command."); }
        public void Answer(Func<PromptChoice, bool> match)
        { var p = P!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(match).Id, G.Revision)); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 120; i++)
            {
                if (ready()) return;
                if (P is not { PlayerSeat: 0 } p) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.Kind == DecisionKind.Nullification) Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                else throw new InvalidOperationException("Unexpected delayed-placement boundary " + Diagnostic());
            }
            throw new InvalidOperationException("Missing delayed-placement boundary " + Diagnostic());
        }
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == State(), "Cold replay retains the actual gift, one paid delayed material and original episode identity.");
        }
        private string State() => JsonSerializer.Serialize(new { G.State, Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
        private string Diagnostic() => JsonSerializer.Serialize(new { P, Frames = G.ResolutionStack, Facts = G.Events.TakeLast(8).Select(e => e.Payload.ToString()) });
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + Diagnostic()); }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-episode-placement", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mou:huang-quan");
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"fixed-opening","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Hold}}}","revision":1,"triggers":[{"id":"after-paid-grant","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["indulgence"],"priority":-1000,"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"issued","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Quiet, Hold }.ToDictionary(id => id, id => new { name = id, description = "Real delayed episode return",
                        optionLabels = id == Hold ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Delayed episode payer", "", Skill, "shu", 12, [Quiet, Hold]));
            b.AddGeneral(new(Peer, "Delayed episode peer", "", Quiet, "shu", 12));
            var card = CardCatalog.Get(CardKind.Indulgence); b.AddCard(new("fixture:episode-placement-card", card.DisplayName, card.CategoryName, card.Description, CardKind.Indulgence));
            b.AddDeck(new("fixture:episode-placement-deck", "Fixed delayed episode cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Delayed episode fixture", 4, 4, new Dictionary<string, int>(), "fixture:episode-placement-deck",
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

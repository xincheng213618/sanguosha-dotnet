using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileTaishiCiChecks
{
    private const string Hero = "mobile:taishi-ci", Skill = "mobile:taishi-ci-tianyi", Owner = "fixture:mobile-tsc-owner", Extra = "fixture:mobile-tsc-extra",
        Rank = "fixture:mobile-tsc-rank", Mode = "identity:classic-mobile-taishi-ci";
    private const string Won = "mobile-tianyi-won-phase", Failed = "mobile-tianyi-failed-phase";

    public static void NativePindianWinAndLossExpireAtActualPlayBoundaryCold()
    {
        var s = Scene.Start(); s.Play(); var turn = s.G.State.TurnNumber;
        s.Need(s.V(0).Hp == 5 && s.V(0).HandCount == 6 && s.G.GetAttackRange(0) == 1 && !s.Slashes().Any(a => a.TargetSeats.Contains(2)),
            "The real four-player identity Lord starts at native five HP with one extra fixed two-card opening modifier; a remote seat is initially outside ordinary Slash range.");
        var firstMaterial = s.Contest(1); var firstChild = s.G.ResolutionStack.OfType<PindianFrame>().Single(); var firstPindian = firstChild.Id;
        s.Need(firstChild.SourceCardId == firstMaterial && firstChild.OpponentSeat == 1 && s.Zone(firstMaterial) == CardLocation.Hand(0) && s.MaterialPayments(firstMaterial) == 0 && !s.State(Won) && !s.State(Failed),
            "The original Pindian child freezes the exact source material while awaiting the opponent; neither a material payment nor a future winning state is fabricated before the native comparison.");
        s.Cold(); s.Answer(c => c.Cards.Count == 1); s.Play();
        var first = s.E<PindianResultDeterminedEvent>().Single(e => e.SkillId == Skill);
        s.Need(first.FrameId == firstPindian && first.Result.SourceRank == 13 && first.Result.OpponentRank == 7 && first.Result.SourceWon &&
            s.MaterialPayments(firstMaterial) == 1 && s.State(Won) && !s.State(Failed) && !s.CanContest() && s.Slashes().Any(a => a.TargetSeats.SequenceEqual([1, 2])),
            "The genuine winning contest enables this actual phase's unlimited range and second target, while its once-per-phase Pindian quota is consumed.");
        var hp1 = s.V(1).Hp; var hp2 = s.V(2).Hp;
        s.PlaySlash([1, 2]); s.Play();
        s.Need(s.V(1).Hp == hp1 - 1 && s.V(2).Hp == hp2 - 1 && s.Slashes().Any(),
            "One physical Slash actually damages both targets including the remote seat and still leaves the winning phase's additional Slash available.");
        s.PlaySlash([3]); s.Play();
        s.Need(!s.Slashes().Any() && s.MaterialPayments(firstMaterial) == 1,
            "Exactly two actual Slashes consume the winning phase's two-use allowance without replaying the Pindian payment.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == turn && !s.State(Won) && !s.State(Failed) && s.CanContest() && s.Slashes().Any() &&
            s.Slashes().All(a => a.TargetSeats.Count == 1 && !a.TargetSeats.Contains(2)),
            "A second real inserted Play in the same turn clears all three winning benefits and restores both the ordinary Slash quota and this phase's Pindian allowance.");
        var secondMaterial = s.Contest(2); var secondChild = s.G.ResolutionStack.OfType<PindianFrame>().Single(); var secondPindian = secondChild.Id;
        s.Need(secondPindian != firstPindian && secondChild.SourceCardId == secondMaterial && secondChild.OpponentSeat == 2 && s.Zone(secondMaterial) == CardLocation.Hand(0) && s.MaterialPayments(secondMaterial) == 0,
            "The next actual phase freezes a distinct exact source material on its own native Pindian child.");
        s.Cold(); s.Answer(c => c.Cards.Count == 1); s.Play();
        var tied = s.E<PindianResultDeterminedEvent>().Single(e => e.FrameId == secondPindian).Result;
        s.Need(tied.SourceRank == 13 && tied.OpponentRank == 13 && !tied.SourceWon && s.V(0).HandCount > 0 &&
            s.MaterialPayments(secondMaterial) == 1 && s.V(0).Hand.All(c => c.Kind == CardKind.Slash) && s.State(Failed) && !s.State(Won) && !s.Slashes().Any() && !s.CanContest(),
            "A real tied contest is not a win: actual remaining Slashes are forbidden for this phase, and the phase's Pindian quota is still spent.");
        s.Cold(); s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == turn && !s.State(Failed) && !s.State(Won) && s.CanContest() && s.Slashes().Any() &&
            s.Slashes().All(a => a.TargetSeats.Count == 1 && !a.TargetSeats.Contains(2)),
            "The same turn's normal Play starts after both real inserted Plays; the previous failed phase's prohibition expires without leaving the old winning modifiers behind.");
        s.PlaySlash([1]); s.Play();
        s.Need(!s.Slashes().Any() && s.E<PindianResultDeterminedEvent>().Count(e => e.SkillId == Skill) == 2 &&
            s.MaterialPayments(firstMaterial) == 1 && s.MaterialPayments(secondMaterial) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Slash) == 3,
            "Only the restored ordinary one-Slash quota applies in the third actual Play; both native contests and all three original Slash uses finish once across cold recovery.");
        s.Cold();
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool State(string id) => V(0).SkillRuntimeStates!.Single(s => s.SkillId == Skill).BooleanStates!.Single(s => s.StateId == id).Value;
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public int MaterialPayments(int id) => G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.PindianReveal);
        public IEnumerable<LegalAction> Slashes() => G.GetLegalActions(0).Where(a => a.Kind == LegalActionKind.Slash);
        public bool CanContest() => G.GetLegalActions(0).Any(a => a.ProgramSkillId == Skill && a.ProgramActivationId == "contest");
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public int Contest(int target)
        {
            var material = V(0).Hand.First().Id;
            Accept(new UseProgramSkillCommand(0, Skill, "contest", [material], [target], G.Revision, P!.PromptId));
            Reach(() => P?.PlayerSeat == target && P.Choices.Any(c => c.Cards.Count == 1));
            return material;
        }
        public void PlaySlash(IReadOnlyList<int> targets)
        {
            var action = Slashes().First(a => a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> choose) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } general)
                { var id = general.PlayerSeat == 0 ? Owner : "fixture:mobile-tsc-peer-" + general.PlayerSeat; Accept(new SelectGeneralCommand(general.PlayerSeat, id, G.Revision, general.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage"); continue; }
                Need(P is null, "Unexpected native mobile Tianyi boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Bounded native mobile Tianyi flow was not reached.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original native Pindian child, exact committed source material, public phase states and actual phase quotas."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.TurnNumber, G.State.Phase, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Tianyi command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(registry, game); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-taishi-ci", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var skills = new object[] {
                new { id = Extra, revision = 1, modifiers = new[] { new { id = "two-fixed-opening-cards", query = "initialHandSize", operation = "add", value = 2, priority = 0 } },
                    triggers = new[] { new { id = "two-actual-extra-plays", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false, usageScope = "game", usageLimit = 1,
                        effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" }, new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } },
                new { id = Rank, revision = 1, cardPolicies = new[] { new { id = "fixed-native-heart-rank", kind = "pindianRankBySuit", inputSuit = "heart", value = 13 } } }
            };
            var labels = new Dictionary<string, object> {
                [Extra] = new { name = "实际额外出牌阶段", description = "两段真实插入出牌阶段" },
                [Rank] = new { name = "固定原生拼点点数", description = "红桃实际拼点13" }
            };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "移动太史慈固定身份", "supporter", Skill, "wu", 4, AdditionalSkillIds: [Extra, Rank], Gender: GeneralGender.Male));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new("fixture:mobile-tsc-peer-" + seat, "固定真实拼点角色", "supporter", "standard:none", "wei", 4,
                AdditionalSkillIds: seat == 2 ? [Rank] : [], Gender: GeneralGender.Male));
            const string deck = "fixture:mobile-tsc-deck";
            b.AddDeck(new(deck, "固定真实杀牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "移动天义实际阶段", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, "fixture:mobile-tsc-peer-1", "fixture:mobile-tsc-peer-2", "fixture:mobile-tsc-peer-3"]));
        }
    }
}

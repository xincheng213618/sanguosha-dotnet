using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class PangJiChecks
{
    private const string DySkill = "ol:douyu", DyDriver = "fixture:dy-driver", DyWatch = "fixture:dy-watch";
    private const string DyOwner = "fixture:dy-owner", DyMode = "identity:classic-douyu", DyDraw = "skill-program.pang-ji.douyu.draw";

    private static void DyRequire(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private static string DyRead(string bundle, string part)
    { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + bundle + "." + part + ".json") ?? throw new InvalidOperationException("Missing embedded " + bundle); using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
    private sealed class DyScenario(string scene, GameEngine game, ContentRegistry registry)
    {
        public GameEngine DyG = game;
        public PendingDecision? DyP => Enumerable.Range(0, 4).Select(s => DyG.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot DyV(int seat = 0) => DyG.CreateSnapshot(seat).Players[seat];
        public T[] DyE<T>() where T : IGameEvent => DyG.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation DyZone(int id) => DyG.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool DyOptional(string skill) => DyP is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == skill && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public bool DyHold() => DyP is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == DyWatch && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void DyUse(string binding, IReadOnlyList<int>? cards = null) => DyAccept(new UseProgramSkillCommand(0, DyDriver, binding, cards ?? [], [1], DyG.Revision, DyP!.PromptId));
        public void DyStep() => DyAccept(new AdvanceOneStepCommand(DyG.Revision));
        public void DyAnswer(Func<PromptChoice, bool> pick)
        { var p = DyP; var c = p?.Choices.FirstOrDefault(pick); DyNeed(p is { PlayerSeat: 0 } && c is not null, "Only the real human chooses an actually published input."); DyAccept(new AnswerPromptCommand(0, p!.PromptId, c!.Id, DyG.Revision)); }
        public void DyPlay() => DyReach(() => DyP is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void DyNeed(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + DyDiagnostic()); }
        public void DyAccept(GameCommand command)
        { try { var result = DyG.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); DyNeed(result.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(result)); } catch (Exception e) { throw new InvalidOperationException("Douyu " + scene + " " + command.GetType().Name + " cause=" + e.Message + " inner=" + e + " " + DyDiagnostic(), e); } }
        public void DyReject()
        { var p = DyP!; var before = DyState(); DyNeed(!DyG.Submit(new AnswerPromptCommand(2, p.PromptId, p.Choices[0].Id, DyG.Revision)).Accepted && before == DyState() && !DyG.Submit(new AnswerPromptCommand(0, p.PromptId, new ChoiceId("unpublished-douyu-target"), DyG.Revision)).Accepted && before == DyState(), "Foreign or forged private input cannot change the native selections or reveal either entity."); }
        public void DyRejectOld(PendingDecision old)
        { var before = DyState(); DyNeed(!DyG.Submit(new AnswerPromptCommand(0, old.PromptId, old.Choices[0].Id, DyG.Revision)).Accepted && before == DyState(), "Replaying the old private choice cannot exchange or draw twice."); }
        public void DyReach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 190; i++)
            {
                if (done()) return; DyNeed(DyG.State.Winner == Winner.None, "The fixed native fixture ended before boundary " + line); var p = DyP;
                if (p is null || p.PlayerSeat != 0) DyStep();
                else if (DyHold()) { DyCold(); DyAnswer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); }
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) DyAnswer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    DyAnswer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                else throw new InvalidOperationException("Unexpected real Douyu frontier " + line + " " + DyDiagnostic());
            }
            throw new InvalidOperationException("Missing bounded Douyu frontier " + line + " " + DyDiagnostic());
        }
        public void DyCold() { var before = DyState(); DyG = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(DyG.CreateCheckpoint())), registry); DyNeed(before == DyState(), "Cold replay retains every private view, original native payment zone, unique logical assignment and exact draw child cursor."); }
        private string DyState() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(DyG.CreateSnapshot(s))), Frames = DyG.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Events = DyG.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), DyG.CardMovements, Zones = DyG.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(DyG.AcceptedCommands) });
        private string DyDiagnostic() => JsonSerializer.Serialize(new { Scene = scene, DyG.State.TurnNumber, DyG.State.Phase, Prompt = DyP, Players = Enumerable.Range(0, 4).Select(s => new { Seat = s, DyV(s).Hp, DyV(s).HandCount }), Frames = DyG.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = DyG.Events.TakeLast(10).Select(e => e.Payload) });
        public static DyScenario DyStart(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new DyFixture(scene));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, HumanSeat = 0, HumanRole = scene == "paid-death" ? Role.Rebel : Role.Lord, PlayerCount = 4, ModeId = DyMode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var s = new DyScenario(scene, g, registry); s.DyAccept(new StartGameCommand()); s.DyReach(() => s.DyP is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral });
            s.DyAccept(new SelectGeneralCommand(0, DyOwner, g.Revision, s.DyP!.PromptId)); s.DyPlay(); return s;
        }
    }
    private sealed class DyFixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-pang-ji-douyu", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            void Add(SkillProgramCatalog c, params string[] only)
            { foreach (var (id, p) in c.Programs.Where(p => only.Length == 0 || only.Contains(p.Key))) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] }); }
            Add(SkillProgramCatalog.Load(DyRead("ol-pang-ji", "rules"), DyRead("ol-pang-ji", "presentation")), DySkill);
            if (scene == "jici") Add(SkillProgramCatalog.Load(DyRead("ol-wang-lang", "rules"), DyRead("ol-wang-lang", "presentation")));
            if (scene == "pile") Add(SkillProgramCatalog.Load(DyRead("ol-guan-hai", "rules"), DyRead("ol-guan-hai", "presentation")));
            static JsonNode N(string text) => JsonNode.Parse(text)!;
            static JsonObject A(string id, string effects, int cards) => new() { ["id"] = id, ["minCards"] = cards, ["maxCards"] = cards, ["sourceZones"] = new JsonArray("hand"), ["minTargets"] = 1, ["maxTargets"] = 1, ["targetKind"] = "otherLivingWithHand", ["usesPerTurn"] = null, ["effects"] = N("[" + effects + "]") };
            var driver = new JsonObject { ["id"] = DyDriver, ["revision"] = 1, ["activations"] = new JsonArray(
                A("pair", "{\"op\":\"pindian\",\"target\":\"selectedTarget\",\"amount\":1}", 1),
                A("top", "{\"op\":\"startPindian\",\"target\":\"owner\",\"opponentRef\":{\"kind\":\"selectedTarget\"},\"resultBind\":\"contest\",\"visibility\":\"public\"}", 0)) };
            if (scene == "jici") driver["triggers"] = N("[{\"id\":\"native-mark\",\"window\":\"playPhaseStarting\",\"subject\":\"owner\",\"optional\":false,\"usageScope\":\"game\",\"usageLimit\":1,\"effects\":[{\"op\":\"changeParticipantMarker\",\"target\":\"owner\",\"marker\":\"raoShe\",\"amount\":7}]}]");
            var policy = new JsonArray();
            if (scene is "win" or "both" or "claim" or "paid-loss" or "paid-death" or "top" or "pile") policy.Add(N("{\"id\":\"native-final-rank\",\"kind\":\"pindianRankBySuit\",\"inputSuit\":\"spade\",\"value\":13}"));
            if (scene == "loss") policy.Add(N("{\"id\":\"native-final-rank\",\"kind\":\"pindianRankBySuit\",\"inputSuit\":\"spade\",\"value\":1}"));
            if (scene == "claim") policy.Add(N("{\"id\":\"native-claim\",\"kind\":\"pindianClaimAllWhenSourceWins\"}"));
            if (scene == "alcohol") policy.Add(N("{\"id\":\"native-alcohol\",\"kind\":\"alcoholKingIdentityRank\",\"cardKinds\":[\"alcohol\"]}"));
            if (scene == "top") policy.Add(N("{\"id\":\"native-top\",\"kind\":\"pindianTopCardChoice\"}"));
            var defs = new JsonArray(driver,
                N("{\"id\":\"" + DyWatch + "\",\"revision\":1,\"triggers\":[{\"id\":\"actual-reward-child\",\"window\":\"cardsGained\",\"subject\":\"owner\",\"optional\":false,\"destinationZones\":[\"hand\"],\"movementOccurrence\":\"perBatch\",\"movementReasons\":[\"" + DyDraw + "\"],\"effects\":[{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"actual-child\",\"options\":[{\"id\":\"continue\"}]}" + (scene is "paid-loss" or "paid-death" ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + (scene == "paid-death" ? 20 : 1) + "}" : "") + "]}]}"));
            if (policy.Count > 0) defs.Add(new JsonObject { ["id"] = "fixture:dy-policy", ["revision"] = 1, ["cardPolicies"] = policy });
            if (scene == "paid-loss") defs.Add(N("{\"id\":\"fixture:dy-conditional\",\"revision\":1,\"hpSkillGrants\":[{\"id\":\"native-health-source\",\"skillId\":\"" + DySkill + "\",\"minimumHp\":13}]}"));
            if (scene == "top") defs.Add(N("{\"id\":\"fixture:dy-peer-top\",\"revision\":1,\"cardPolicies\":[{\"id\":\"native-peer-top\",\"kind\":\"pindianTopCardChoice\"}]}"));
            if (scene == "paid-death") defs.Add(N("{\"id\":\"fixture:dy-peer-claim\",\"revision\":1,\"cardPolicies\":[{\"id\":\"native-living-claim\",\"kind\":\"pindianClaim\"}]}"));
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "Actual native blind Pindian fixture", optionLabels = n["id"]!.GetValue<string>() == DyWatch ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() });
            Add(SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels })));
            const string pick = "fixture:dy-pick"; b.AddSkill(new(pick, "Human", "Fixed public draft") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => -100000000d) });
            var skills = new List<string> { DyDriver, DyWatch, scene == "paid-loss" ? "fixture:dy-conditional" : DySkill };
            if (policy.Count > 0) skills.Add("fixture:dy-policy");
            if (scene == "jici") skills.AddRange(["ol:gushe", "ol:jici"]); if (scene == "pile") skills.AddRange(["ol:xiewei", "ol:youque"]);
            b.AddGeneral(new(DyOwner, "斗誉原生检查", "supporter", pick, "qun", 12, skills, GeneralGender.Male));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:dy-peer-" + i).ToArray();
            foreach (var i in Enumerable.Range(1, 3))
            {
                var peerPick = "fixture:dy-peer-pick-" + i; b.AddSkill(new(peerPick, "Peer", "Fixed public draft") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => 1000000d * (4 - i)) });
                b.AddGeneral(new(peers[i - 1], "原生拼点对手", "supporter", peerPick, "qun", 12, i == 1 && scene == "both" ? [DySkill] : scene == "top" ? ["fixture:dy-peer-top"] : scene == "paid-death" ? ["fixture:dy-peer-claim"] : [], GeneralGender.Male));
            }
            b.AddDeck(new("fixture:dy-deck", "Actual fixed rank-seven originals", 6, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard(scene == "alcohol" ? "standard:alcohol" : "standard:slash", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(DyMode, "真实私密拼点交换", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, "fixture:dy-deck", GeneralCandidateCount: 4, GeneralPoolIds: [DyOwner, .. peers]));
        }
    }
}

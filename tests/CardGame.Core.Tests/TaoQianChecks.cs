using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class TaoQianChecks
{
    private const string TqZongluan = "ol:zongluan", TqZhaohuo = "ol:zhaohuo", TqWenren = "ol:wenren";
    private const string TqDriver = "fixture:tq-driver", TqWatch = "fixture:tq-watch", TqQuiet = "fixture:tq-quiet", TqCounter = "fixture:tq-counter", TqConditional = "fixture:tq-source";
    private const string TqOwner = "fixture:tq-owner", TqMode = "identity:classic-tao-qian", TqGift = "skill-program.tao-qian.zhaohuo.give", TqDiscard = "skill-program.tao-qian.zongluan.discard";

    private sealed class TqScenario(string scene, GameEngine game, ContentRegistry registry)
    {
        public GameEngine TqG { get; private set; } = game;
        public PendingDecision? TqP => Enumerable.Range(0, 4).Select(i => TqG.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot TqV(int seat = 0) => TqG.CreateSnapshot(seat).Players[seat];
        public T[] TqE<T>() => TqG.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool TqHas(string id) => TqV().Skills?.Any(s => s.ContentId == id) == true;
        public ProgramSkillFrame TqOwn(long? id = null) => TqG.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.TaoQian is not null && (id is null || f.Id == id));
        public bool TqBranch(string b) => TqP?.Choices.Any(c => c.Parameters.GetValueOrDefault("tao-qian") == b) == true;
        public bool TqZhaoChoice() => TqP?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "zhaohuo") == true;
        public bool TqHold(string id) => TqP?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == id) == true;
        public bool TqOptional(string id) => TqP?.SkillPrompt?.SkillId == id && TqP.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void TqActivate() => TqAnswer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void TqBranchAnswer(string b) => TqAnswer(c => c.Parameters.GetValueOrDefault("tao-qian") == b);
        public void TqToggle(string b, int seat) { var current = TqOwn().TaoQian!.Targets; TqAnswer(c => c.Parameters.GetValueOrDefault("tao-qian") == b && c.Targets.SequenceEqual(current.Append(seat))); }
        public void TqContinue() { if (TqP!.PlayerSeat == 0) TqAnswer(c => c.Parameters.ContainsKey("option-id")); else TqAdvance(); }
        public void TqAdvance() => TqAccept(new AdvanceOneStepCommand(TqG.Revision));
        public void TqUse(string id, int[]? targets = null, int[]? cards = null) => TqAccept(new UseProgramSkillCommand(0, TqDriver, id, cards ?? [], targets ?? [], TqG.Revision, TqP!.PromptId));
        public void TqEnd() => TqAccept(new EndPlayPhaseCommand(0, TqG.Revision, TqP!.PromptId));
        public void TqPlay() => TqReach(() => TqP is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void TqNextOwnPlay() { var turn = TqG.State.TurnNumber; TqEnd(); TqReach(() => TqG.State.TurnNumber > turn && TqP is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }); }
        public void TqTakeAll(int seat)
        {
            while (TqV(seat).HandCount > 0) { TqUse("take", targets: [seat]); TqReach(() => TqP?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true); TqAnswer(c => c.Cards.Count == 0); TqPlay(); }
        }
        public void TqPrivate(int seat) => TqNeed(TqP is { IsPrivate: true } p && p.PlayerSeat == seat && Enumerable.Range(0, 4).Where(i => i != seat).All(i => TqG.CreateSnapshot(i).PendingDecision is null), "Only the actual native chooser receives the original private menu.");
        public void TqReject()
        {
            var p = TqP!; var state = TqState(); TqNeed(!TqG.Submit(new AnswerPromptCommand((p.PlayerSeat + 1) % 4, p.PromptId, p.Choices[0].Id, TqG.Revision)).Accepted && TqState() == state &&
                !TqG.Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, new ChoiceId("unpublished-tao-qian"), TqG.Revision)).Accepted && TqState() == state, "Foreign or unpublished input cannot alter the real source, entities, quota or current private decision.");
        }
        public void TqRejectOld(PendingDecision old) { var state = TqState(); TqNeed(!TqG.Submit(new AnswerPromptCommand(old.PlayerSeat, old.PromptId, old.Choices[0].Id, TqG.Revision)).Accepted && TqState() == state, "A genuinely stale consecutive prompt cannot answer the current target or cost decision."); }
        public void TqAnswer(Func<PromptChoice, bool> pick)
        { var p = TqP; var c = p?.Choices.FirstOrDefault(pick); TqNeed(p is { PlayerSeat: 0 } && c is not null, "The human must choose one genuinely published canonical input."); TqAccept(new AnswerPromptCommand(0, p!.PromptId, c!.Id, TqG.Revision)); }
        public void TqReach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 260; i++)
            {
                if (done()) return; TqNeed(TqG.State.Winner == Winner.None, "Fixture ended before boundary at " + line); var p = TqP;
                if (p is null || p.PlayerSeat != 0) TqAdvance();
                else if (p.Kind == DecisionKind.PlayCard) TqEnd();
                else if (p.Choices.Any(c => c.Parameters.ContainsKey("option-id"))) TqContinue();
                else if (TqZhaoChoice()) TqAnswer(c => c.Cards.Count == 1);
                else if (TqBranch("discard-card")) TqAnswer(c => c.Parameters.GetValueOrDefault("tao-qian") == "discard-card");
                else if (TqBranch("discard-confirm")) TqBranchAnswer("discard-confirm");
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) TqAnswer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification) TqAnswer(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("response") is not ("faction-slash-request" or "faction-defense-request"));
                else if (p.Kind == DecisionKind.RescueDying) TqAnswer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
                else throw new InvalidOperationException("Unexpected Tao Qian boundary at " + line + " " + TqDiagnostic());
            }
            throw new InvalidOperationException("Missing bounded Tao Qian boundary at " + line + " " + TqDiagnostic());
        }
        public void TqAccept(GameCommand command)
        {
            try { var result = TqG.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); TqNeed(result.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(result)); }
            catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " command=" + command.GetType().Name + " cause=" + e.Message + " " + TqDiagnostic(), e); }
        }
        public void TqNeed(bool value, string why) { if (!value) throw new InvalidOperationException(why + " " + TqDiagnostic()); }
        public void TqFrozen<T>(IReadOnlyList<T> list) { var rejected = false; try { ((IList)list).Add(default(T)); } catch (NotSupportedException) { rejected = true; } TqNeed(rejected, "Owning selections expose detached frozen collections."); }
        public void TqCold() { var state = TqState(); TqG = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(TqG.CreateCheckpoint())), registry); TqNeed(state == TqState(), "Cold replay preserves every viewer, real native parent, private choice, whole-Use return and exact paid entity."); }
        private string TqState() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(TqG.CreateSnapshot(i))), Frames = TqG.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Events = TqG.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), TqG.CardMovements, Zones = TqG.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(TqG.AcceptedCommands) });
        private string TqDiagnostic() => JsonSerializer.Serialize(new { Scene = scene, TqG.State.TurnNumber, TqG.State.CurrentSeat, TqG.State.Phase, TqG.State.Winner, Prompt = TqP, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, TqV(i).Hp, TqV(i).MaxHp, TqV(i).IsAlive, TqV(i).HandCount, Skills = TqV(i).Skills?.Select(s => s.ContentId) }), Frames = TqG.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = TqG.Events.TakeLast(12).Select(e => e.Payload), Moves = TqG.CardMovements.TakeLast(8) });
        public static TqScenario Start(string scene, bool preparation = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new TqFixture(scene));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = scene == "gift-death" ? Role.Renegade : Role.Lord, ModeId = TqMode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var s = new TqScenario(scene, g, r); s.TqAccept(new StartGameCommand()); s.TqReach(() => s.TqP is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }); s.TqAccept(new SelectGeneralCommand(0, TqOwner, g.Revision, s.TqP!.PromptId));
            if (scene == "zong-lion")
            {
                s.TqReach(() => s.TqP?.SkillPrompt?.SkillId == TqDriver && s.TqP.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
                s.TqAnswer(c => c.Targets.SequenceEqual([0]));
            }
            if (preparation) s.TqReach(() => s.TqOptional(TqZongluan)); else s.TqPlay(); return s;
        }
    }

    private sealed class TqFixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-tao-qian-core", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            static JsonNode N(string value) => JsonNode.Parse(value)!;
            static string C(string id) => "{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"" + id + "\",\"options\":[{\"id\":\"" + id + "\"}]}";
            static JsonNode A(string id, string effects, int targets = 0) => N("{\"id\":\"" + id + "\",\"minCards\":0,\"maxCards\":0,\"minTargets\":" + targets + ",\"maxTargets\":" + targets + ",\"targetKind\":\"anyLiving\",\"usesPerTurn\":null,\"effects\":[" + effects + "]}");
            static JsonNode T(string id, string window, string subject, string effects, string more = "") => N("{\"id\":\"" + id + "\",\"window\":\"" + window + "\",\"subject\":\"" + subject + "\",\"optional\":false," + more + "\"effects\":[" + effects + "]}");
            TqProduction(b);
            var driver = new JsonObject { ["id"] = TqDriver, ["revision"] = 1, ["activations"] = new JsonArray(
                A("hit", "{\"op\":\"damage\",\"target\":\"selectedTarget\",\"amount\":1}", 1), A("hit-two", "{\"op\":\"damage\",\"target\":\"selectedTarget\",\"amount\":2}", 1),
                A("lose", "{\"op\":\"loseHp\",\"target\":\"selectedTarget\",\"amount\":1}", 1), A("draw-peer", "{\"op\":\"draw\",\"target\":\"selectedTarget\",\"amount\":1}", 1),
                A("take", "{\"op\":\"selectAndMoveOwnedCard\",\"target\":\"owner\",\"chooserRef\":{\"kind\":\"owner\"},\"cardOwnerRef\":{\"kind\":\"selectedTarget\"},\"zones\":[\"hand\"],\"count\":1,\"destination\":\"ownerHand\",\"awaitMovementTriggers\":true}", 1),
                A("grant-zhaohuo", "{\"op\":\"grantSkills\",\"target\":\"owner\",\"skillIds\":[\"" + TqZhaohuo + "\"]}")) };
            if (scene == "zong-lion") driver["triggers"] = new JsonArray(T("real-start-equipment", "gameStarting", "owner", "{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"anyLiving\"},{\"op\":\"useRandomDeckEquipment\",\"target\":\"owner\",\"resultBind\":\"lion\"},{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":1}"));
            var watches = new JsonArray(T("real-health", "afterHpRecovered", "owner", C("health")));
            if (scene.StartsWith("zong", StringComparison.Ordinal))
            {
                var committed = T("real-committed", "cardUseCommitted", "owner", C("committed"), "\"ownerRelation\":\"actor\",\"cardKinds\":[\"slash\"],").AsObject();
                var completed = T("real-completed", "cardUseCompleted", "owner", C("completed"), "\"ownerRelation\":\"actor\",\"cardKinds\":[\"slash\"],").AsObject();
                committed.Remove("subject"); completed.Remove("subject");
                watches.Add(committed); watches.Add(completed);
            }
            var afterGift = scene is "zong-loss" or "gift-loss" ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":1}" : scene == "gift-death" ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":20}" : "";
            watches.Add(T("real-original-gift", "cardsGained", "owner", C("gift") + afterGift, "\"destinationZones\":[\"hand\"],\"movementOccurrence\":\"perBatch\",\"movementReasons\":[\"" + TqGift + "\"],"));
            var defs = new JsonArray(driver, new JsonObject { ["id"] = TqWatch, ["revision"] = 1, ["triggers"] = watches },
                new JsonObject { ["id"] = TqQuiet, ["revision"] = 1, ["triggers"] = new JsonArray(T("real-skip-play", "afterNormalDraw", "owner", "{\"op\":\"skipTurnPhases\",\"target\":\"owner\",\"phases\":[\"play\"]}")) });
            if (scene is "zong-loss" or "gift-loss") defs.Add(N("{\"id\":\"" + TqConditional + "\",\"revision\":1,\"hpSkillGrants\":[{\"id\":\"healthy-source\",\"skillId\":\"" + (scene == "zong-loss" ? TqZongluan : TqZhaohuo) + "\",\"minimumHp\":13}]}"));
            if (scene == "zong-counter") defs.Add(new JsonObject { ["id"] = TqCounter, ["revision"] = 1, ["triggers"] = new JsonArray(T("real-counter", "afterDamageApplied", "owner", C("counter") + ",{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"eventSource\"},{\"op\":\"damage\",\"target\":\"selectedTarget\",\"amount\":1}", "\"damageOccurrence\":\"perDamage\",\"usageScope\":\"game\",\"usageLimit\":1,")) });
            if (scene == "gift-prevent") defs.Add(N("{\"id\":\"fixture:tq-prevent\",\"revision\":1,\"triggers\":[{\"id\":\"real-first-prevent\",\"window\":\"beforeDamageApplied\",\"subject\":\"damageTarget\",\"optional\":true,\"usageScope\":\"game\",\"usageLimit\":1,\"effects\":[{\"op\":\"preventCurrentDamage\",\"target\":\"owner\"}]}]}"));
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "Small actual native commands", optionLabels = new[] { "activations", "triggers" }.SelectMany(k => n[k]?.AsArray() ?? new JsonArray()).SelectMany(a => a!["effects"]!.AsArray()).SelectMany(e => e!["options"]?.AsArray() ?? new JsonArray()).Select(o => o!["id"]!.GetValue<string>()).Distinct().ToDictionary(id => id, id => id) });
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "Genuine small fixture") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            const string pick = "fixture:tq-pick"; b.AddSkill(new(pick, "Human", "Public general selection") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => -100000000d) });
            var own = new List<string> { TqDriver, TqWatch };
            if (scene.StartsWith("zong", StringComparison.Ordinal)) own.Add(scene == "zong-loss" ? TqConditional : TqZongluan);
            if (scene is "zong" or "zong-counter" or "zong-loss") own.Add(TqZhaohuo);
            if (scene.StartsWith("gift", StringComparison.Ordinal) && scene != "gift-late") own.Add(scene == "gift-loss" ? TqConditional : TqZhaohuo);
            b.AddGeneral(new(TqOwner, "陶谦真实技能夹具", "ol-tao-qian", pick, "qun", 12, own, GeneralGender.Male));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:tq-peer-" + i).ToArray();
            foreach (var i in Enumerable.Range(1, 3))
            {
                var peerPick = "fixture:tq-pick-" + i; b.AddSkill(new(peerPick, "Peer", "Public selection") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => 1000000d * (4 - i)) });
                var skills = new List<string> { TqQuiet, TqWatch }; if (scene == "zong-counter") skills.Add(TqCounter); if (scene == "gift-prevent") skills.Add("fixture:tq-prevent");
                b.AddGeneral(new(peers[i - 1], "原生其他角色", "supporter", peerPick, "qun", 12, skills, GeneralGender.Male));
            }
            const string entity = "fixture:tq-entity"; var kind = scene == "zong-lion" ? CardKind.SilverLion : CardKind.Slash;
            b.AddCard(new(entity, "原实体" + kind, CardCatalog.Get(kind).CategoryName, "Real physical entity", kind));
            b.AddDeck(new("fixture:tq-deck", "80 fixed actual entities", scene == "zong-short" ? 1 : 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard(entity, new[] { Suit.Spade, Suit.Heart, Suit.Club, Suit.Diamond }[i % 4], i % 13 + 1)).ToArray() });
            var roles = scene == "gift-death" ? new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 } : new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 };
            b.AddMode(new(TqMode, "陶谦真实原生边界", 4, 4, roles, "fixture:tq-deck", GeneralCandidateCount: 4, GeneralPoolIds: [TqOwner, .. peers]));
        }
        private static void TqProduction(IContentRegistryBuilder b)
        {
            string Read(string part) { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.ol-tao-qian." + part + ".json") ?? throw new InvalidOperationException("Missing actual Tao Qian embedded " + part); using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
            var c = SkillProgramCatalog.Load(Read("rules"), Read("presentation")); foreach (var id in new[] { TqZongluan, TqZhaohuo, TqWenren }) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = c.Programs[id], ProgramPresentation = c.Presentations[id] });
        }
    }
}

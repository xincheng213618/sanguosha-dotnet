using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class LeMiHengChecks
{
    private const string JmSkill = "le:jigu", JmOwner = "fixture:lmj-owner", JmMode = "identity:classic-le-mi-heng-jigu";
    private const string JmDriver = "fixture:lmj-driver", JmQuiet = "fixture:lmj-quiet", JmWatch = "fixture:lmj-watch";
    private const string JmConditional = "fixture:lmj-conditional", JmZenrun = "ol:zenrun";
    private const string JmDraw = "skill-program.mi-heng.jigu.draw", JmObtain = "skill-program.sun-hong.zenrun.obtain", JmCompensation = "skill-program.sun-hong.zenrun.compensation";

    private sealed class JmScenario(string scene, GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Hold(string id) => P?.SkillPrompt?.SkillId == JmWatch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public bool Optional(string skill) => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public bool Zen(string branch) => P?.SkillPrompt?.SkillId == JmZenrun && P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == branch);
        public void Use(string id, int[]? cards = null, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, JmDriver, id, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public void Native(int id, int[] targets) { var a = G.GetHumanLegalActions().First(a => a.CardId == id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets)); Accept(new PlayCardCommand(0, id, targets, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Advance() => Accept(new AdvanceOneStepCommand(G.Revision));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Continue() => Answer(c => c.Parameters.ContainsKey("option-id"));
        public void Private(int seat) => Need(P is { IsPrivate: true } p && p.PlayerSeat == seat && Enumerable.Range(0, 4).Where(i => i != seat).All(i => G.CreateSnapshot(i).PendingDecision is null), "Only the actual native chooser sees the pending private child.");
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; var c = p.Choices.FirstOrDefault(match); Need(p.PlayerSeat == 0 && c is not null, "The human chooses a genuine published choice."); Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 180; i++) { if (done()) return; Need(G.State.Winner == Winner.None, "Native fixture ended before boundary " + line); var p = P;
                if (p is null || p.PlayerSeat != 0) Advance(); else if (p.Kind == DecisionKind.PlayCard) End();
                else if (p.SkillPrompt?.SkillId == JmWatch) Continue(); else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.Kind == DecisionKind.RescueDying) Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die"); else throw new InvalidOperationException("Unexpected native boundary " + line + " " + Diagnostic()); }
            throw new InvalidOperationException("Missing bounded native boundary " + line + " " + Diagnostic());
        }
        public void Accept(GameCommand command) { try { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(r)); } catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " command=" + command.GetType().Name + " cause=" + e.Message + " " + Diagnostic(), e); } }
        public void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message + " " + Diagnostic()); }
        public string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold replay preserves exact initial entity membership, historical starts, round latch, native draw outcomes and pending children."); }
        public void Reject() { var p = P!; var before = State(); Need(!G.Submit(new AnswerPromptCommand(1, p.PromptId, p.Choices[0].Id, G.Revision)).Accepted && State() == before && !G.Submit(new AnswerPromptCommand(0, p.PromptId, new ChoiceId("unpublished-jigu-child"), G.Revision)).Accepted && State() == before, "Foreign and unpublished inputs cannot mutate the prepared damage/draw child."); }
        private string Diagnostic() => JsonSerializer.Serialize(new { Scene = scene, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner, Prompt = P, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).IsAlive, V(i).HandCount, V(i).HandLimit }), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Paid = E<JiguActivationPaidEvent>(), Returned = E<JiguDrawReturnedEvent>() });
        public static JmScenario Start(string scene)
        {
            ContentRegistry r; try { r = ContentRegistry.Build(new StandardContentPackage(), new JmFixture(scene)); } catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " fixture=" + e.Message, e); }
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = scene == "paid-death" ? Role.Renegade : Role.Lord, ModeId = JmMode, UseInteractiveSetup = true, ConfirmOpeningHand = scene == "opening", UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 10, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var s = new JmScenario(scene, g, r); s.Accept(new StartGameCommand()); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }); s.Accept(new SelectGeneralCommand(0, JmOwner, g.Revision, s.P!.PromptId)); if (scene == "opening") s.Reach(() => s.P?.Kind == DecisionKind.OpeningHand); else s.Play(); return s;
        }
    }
    private static void LmProduction(IContentRegistryBuilder b, string bundle, params string[] ids)
    {
        string Read(string suffix) { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + bundle + "." + suffix + ".json") ?? throw new InvalidOperationException("Missing real production bundle " + bundle); using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
        var c = SkillProgramCatalog.Load(Read("rules"), Read("presentation")); foreach (var id in ids) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = c.Programs[id], ProgramPresentation = c.Presentations[id] });
    }
    private sealed class JmFixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-le-mi-heng-jigu", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            LmProduction(b, "le-mi-heng", JmSkill); if (scene == "replacement") LmProduction(b, "ol-sun-hong", JmZenrun);
            static JsonNode N(string text) => JsonNode.Parse(text)!;
            static JsonNode A(string id, string effects, int cards = 0, int targets = 0) => N("{\"id\":\"" + id + "\",\"minCards\":" + cards + ",\"maxCards\":" + cards + ",\"minTargets\":" + targets + ",\"maxTargets\":" + targets + ",\"targetKind\":\"anyLiving\",\"usesPerTurn\":null," + (cards == 0 ? "" : "\"sourceZones\":[\"hand\"],") + "\"effects\":[" + effects + "]}");
            static string Choice(string id) => "{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"seen\",\"options\":[{\"id\":\"" + id + "\"}]}";
            var acts = new JsonArray(A("stock-four", "{\"op\":\"draw\",\"target\":\"owner\",\"amount\":4}"), A("stock-six", "{\"op\":\"draw\",\"target\":\"owner\",\"amount\":6}"),
                A("give", "{\"op\":\"giveSelected\",\"target\":\"selectedTarget\"}", 1, 1), A("take", "{\"op\":\"selectAndMoveOwnedCard\",\"target\":\"owner\",\"chooserRef\":{\"kind\":\"owner\"},\"cardOwnerRef\":{\"kind\":\"selectedTarget\"},\"zones\":[\"hand\"],\"count\":1,\"destination\":\"ownerHand\",\"awaitMovementTriggers\":true}", targets: 1),
                A("self-hit", "{\"op\":\"damage\",\"target\":\"owner\",\"amount\":1}"), A("self-hit-two", "{\"op\":\"damage\",\"target\":\"owner\",\"amount\":2}"), A("kill", "{\"op\":\"loseHp\",\"target\":\"selectedTarget\",\"amount\":20}", targets: 1),
                A("abolish-armor", "{\"op\":\"abolishEquipmentSlotGroup\",\"target\":\"owner\",\"equipmentSlots\":[\"armor\"]}"), A("abolish-all", "{\"op\":\"abolishEquipmentSlotGroup\",\"target\":\"owner\",\"equipmentSlots\":[\"weapon\",\"armor\",\"offensiveHorse\",\"defensiveHorse\",\"treasure\"]}"), A("extra-turn", "{\"op\":\"pendExtraTurn\",\"target\":\"owner\"}"));
            var defs = new JsonArray(new JsonObject { ["id"] = JmDriver, ["revision"] = 1, ["activations"] = acts },
                N($$"""{"id":"{{JmQuiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}"""));
            var own = new List<string> { JmDriver, scene == "paid-loss" ? JmConditional : JmSkill };
            if (scene == "paid-loss") defs.Add(N($$"""{"id":"{{JmConditional}}","revision":1,"hpSkillGrants":[{"id":"healthy","skillId":"{{JmSkill}}","minimumHp":12}]}"""));
            if (scene == "skipped") ((JsonObject)defs[0]!)["triggers"] = N("[{\"id\":\"real-first-skip\",\"window\":\"gameStarting\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"setFaceState\",\"target\":\"owner\",\"faceDown\":true}]}]");
            if (scene == "opening") ((JsonObject)defs[0]!)["triggers"] = N("[{\"id\":\"earlier-real-opening-move\",\"window\":\"gameStarting\",\"subject\":\"owner\",\"priority\":100,\"optional\":false,\"effects\":[{\"op\":\"selectAndMoveOwnedCard\",\"target\":\"owner\",\"chooserRef\":{\"kind\":\"owner\"},\"cardOwnerRef\":{\"kind\":\"owner\"},\"zones\":[\"hand\"],\"count\":1,\"destination\":\"discardPile\",\"awaitMovementTriggers\":true}]}]");
            var watch = new JsonArray();
            void Gain(string id, string reason, string after = "") => watch.Add(N("{\"id\":\"actual-" + id + "\",\"window\":\"cardsGained\",\"subject\":\"owner\",\"destinationZones\":[\"hand\"],\"movementOccurrence\":\"perBatch\",\"movementReasons\":[\"" + reason + "\"],\"optional\":false,\"effects\":[" + Choice(id) + (after.Length == 0 ? "" : "," + after) + "]}"));
            if (scene is "paid-loss" or "paid-death") Gain("paid", JmDraw, "{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + (scene == "paid-loss" ? 1 : 20) + "}");
            if (scene == "replacement") { own.Add(JmZenrun); Gain("obtained", JmObtain); Gain("compensated", JmCompensation); }
            if (scene == "round") watch.Add(N("{\"id\":\"other-real-start\",\"window\":\"turnStartBeforeNormalFlow\",\"subject\":\"owner\",\"turnOwnerScope\":\"otherLiving\",\"optional\":false,\"effects\":[" + Choice("turn-hit") + ",{\"op\":\"damage\",\"target\":\"owner\",\"amount\":1}]}"));
            if (watch.Count > 0) { defs.Add(new JsonObject { ["id"] = JmWatch, ["revision"] = 1, ["triggers"] = watch }); own.Add(JmWatch); }
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "真实激鼓实体与原生伤害摸牌夹具", optionLabels = new[] { "activations", "triggers" }.SelectMany(k => n[k]?.AsArray() ?? new JsonArray()).SelectMany(t => t!["effects"]!.AsArray()).SelectMany(e => e!["options"]?.AsArray() ?? new JsonArray()).Select(o => o!["id"]!.GetValue<string>()).Distinct().ToDictionary(x => x, x => x) });
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, "真实激鼓夹具") { Program = p, ProgramPresentation = c.Presentations[id] });
            const string pick = "fixture:lmj-pick"; b.AddSkill(new(pick, "本人", "固定真人候选") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => -100000000d) });
            var baseHp = scene is "marks" or "recipient" or "inherited" ? 3 : 12; b.AddGeneral(new(JmOwner, "真实乐祢衡激鼓者", "supporter", pick, "qun", baseHp, own));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:lmj-peer-" + i).ToArray();
            for (var i = 1; i <= 3; i++) { var pp = "fixture:lmj-peer-pick-" + i; b.AddSkill(new(pp, "其他角色", "固定公开候选") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => 1000000d * (4 - i)) }); b.AddGeneral(new(peers[i - 1], "真实对局角色", "supporter", pp, "shu", baseHp, scene == "replacement" ? [JmQuiet, JmWatch] : scene == "inherited" && i == 1 ? [JmQuiet, JmSkill] : [JmQuiet])); }
            var openingNames = new[] { "standard:slash", "standard:dodge", "standard:peach", "standard:duel", "standard:crossbow" };
            b.AddDeck(new("fixture:lmj-deck", "真实开局实体与装备", scene is "marks" or "equipment" or "recipient" or "inherited" or "opening" ? 4 : 0, 0, []) { PhysicalCards = scene == "opening" ? Enumerable.Range(0, 40).Select(i => new ContentDeckPhysicalCard(openingNames[i % 5], Suit.Club, 7)).ToArray() : Enumerable.Range(0, 36).Select(i => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 7)).ToArray() });
            var roles = scene == "paid-death" ? new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 } : new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 };
            b.AddMode(new(JmMode, "真实激鼓边界", 4, 4, roles, "fixture:lmj-deck", GeneralCandidateCount: 4, GeneralPoolIds: [JmOwner, .. peers]));
        }
    }
}

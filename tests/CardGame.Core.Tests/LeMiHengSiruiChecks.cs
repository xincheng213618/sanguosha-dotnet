using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class LeMiHengChecks
{
    private const string SmSkill = "le:sirui", SmOwner = "fixture:lms-owner", SmMode = "identity:classic-le-mi-heng-sirui";
    private const string SmDriver = "fixture:lms-driver", SmQuiet = "fixture:lms-quiet", SmWatch = "fixture:lms-watch", SmConditional = "fixture:lms-conditional";
    private const string SmSetup = "fixture:lms-setup", SmInsert = "fixture:lms-insert", SmRegrant = "fixture:lms-regrant", SmForced = "fixture:lms-forced", SmPeerDodge = "fixture:lms-peer-dodge";

    private sealed class SmScenario(string scene, GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardSnapshot Card(int id) => Enumerable.Range(0, 4).SelectMany(i => V(i).Hand.Concat(V(i).Equipment)).First(c => c.Id == id);
        public LegalAction[] Conversions() => G.GetHumanLegalActions().Where(a => a.ConversionSource?.SkillId == SmSkill).ToArray();
        public bool Hold(string id) => P?.SkillPrompt?.SkillId == SmWatch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void Stock() { Use("stock-all"); Play(); Use("stock-all"); Play(); }
        public void StockFour() { Use("stock-four"); Play(); }
        public void Use(string id, int[]? cards = null, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, SmDriver, id, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public PlayCardCommand Command(LegalAction a, CardConversionSource? source = null) => new(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = source ?? a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources };
        public void Native(LegalAction a) => Accept(Command(a));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void PlayUnlessDead() => Reach(() => !V().IsAlive || P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Advance() => Accept(new AdvanceOneStepCommand(G.Revision));
        public void Continue() => Answer(c => c.Parameters.ContainsKey("option-id"));
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; var c = p.Choices.FirstOrDefault(match); Need(p.PlayerSeat == 0 && c is not null, "The human selects a genuine canonical choice."); Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision)); }
        public void Private(int seat) => Need(P is { } p && p.PlayerSeat == seat && Enumerable.Range(0, 4).Where(i => i != seat).All(i => G.CreateSnapshot(i).PendingDecision is null), "Only the actual native response or child chooser receives its exact choices and private material identities.");
        public void RejectSource(LegalAction action) { var before = State(); Need(!G.Submit(Command(action, action.ConversionSource! with { SkillInstanceId = "forged-sirui-source" })).Accepted && before == State() &&
            !G.Submit(Command(action, action.ConversionSource! with { BindingId = "unpublished-equal-name-output" })).Accepted && before == State(), "A forged instance or binding cannot substitute for the published exact conversion source or consume its phase allowance."); }
        public void RejectOld(PendingDecision old) { var before = State(); Need(!G.Submit(new AnswerPromptCommand(0, old.PromptId, old.Choices[0].Id, G.Revision)).Accepted && before == State(), "A previous native request token cannot replay the paid output."); }
        public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 190; i++) { if (done()) return; Need(G.State.Winner == Winner.None, "Native fixture ended before boundary " + line); var p = P;
                if (p is null || p.PlayerSeat != 0) Advance(); else if (p.Kind == DecisionKind.PlayCard) End(); else if (p.SkillPrompt?.SkillId == SmWatch) Continue();
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.SkillPrompt?.SkillId == SmInsert && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target")) Answer(c => c.Targets.SequenceEqual([0]));
                else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying) Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "target-card-slot")) Answer(c => c.Parameters.GetValueOrDefault("action") == "target-card-slot");
                else throw new InvalidOperationException("Unexpected Sirui boundary " + line + " " + Diagnostic()); }
            throw new InvalidOperationException("Missing bounded Sirui boundary " + line + " " + Diagnostic());
        }
        public void Accept(GameCommand command) { try { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(r)); } catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " command=" + command.GetType().Name + " cause=" + e.Message + " " + Diagnostic(), e); } }
        public void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message + " " + Diagnostic()); }
        public string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold replay preserves native true-use identity, exact HE payment, source-wide actual-phase quota and the original child return."); }
        private string Diagnostic() => JsonSerializer.Serialize(new { Scene = scene, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
            Prompt = P is not { } p ? null : new { p.Kind, p.PlayerSeat, p.PromptId, p.SourceSeat, p.TargetSeat, p.RequiredCardKind, ChoiceCount = p.Choices.Count,
                Choices = p.Choices.Take(6).Select(c => new { c.Id, c.Cards, c.Targets, c.Parameters }) },
            Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).IsAlive, V(i).HandCount }),
            Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind, f.Step, f.Sirui }), Issued = E<SiruiPhaseUseIssuedEvent>(), Paid = E<SiruiMaterialPaidEvent>(), Bound = E<SiruiNativeActionBoundEvent>(),
            Finished = E<CardUseFinishedEvent>().Where(e => E<SiruiPhaseUseIssuedEvent>().Any(paid => paid.OwnerFrameId == e.ResolutionId)),
            TargetProcessing = G.CardMovements.Where(m => m.From == CardLocation.Hand(2) && m.To == CardLocation.Processing).TakeLast(8),
            ObtainedFromProcessing = G.CardMovements.Where(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(0)).TakeLast(8) });
        public static SmScenario Start(string scene)
        {
            ContentRegistry r; try { r = ContentRegistry.Build(new StandardContentPackage(), new SmFixture(scene)); } catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " fixture=" + e.Message, e); }
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = scene is "paid-death" or "provider" ? Role.Renegade : Role.Lord, ModeId = SmMode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var s = new SmScenario(scene, g, r); s.Accept(new StartGameCommand()); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }); s.Accept(new SelectGeneralCommand(0, SmOwner, g.Revision, s.P!.PromptId));
            s.Play(); return s;
        }
    }
    private sealed class SmFixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-le-mi-heng-sirui", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            LmProduction(b, "le-mi-heng", SmSkill);
            if (scene == "provider") CharacterFixtureContent.Register(b, "classic:cao-cao");
            if (scene == "dianjun") LmProduction(b, "ol-zhang-yi", "ol:dianjun");
            if (scene == "distance-loss") LmProduction(b, "ordinary-sun-shao", "ol:yidian");
            if (scene is "played-arrows" or "played-barbarian") LmProduction(b, "ol-hu-ban", "ol:huiyun");
            static JsonNode N(string text) => JsonNode.Parse(text)!;
            static JsonObject A(string id, string effects, int cards = 0, int targets = 0) => N("{\"id\":\"" + id + "\",\"minCards\":" + cards + ",\"maxCards\":" + cards + ",\"minTargets\":" + targets + ",\"maxTargets\":" + targets + ",\"targetKind\":\"anyLiving\",\"usesPerTurn\":null," + (cards == 0 ? "" : "\"sourceZones\":[\"hand\"],") + "\"effects\":[" + effects + "]}").AsObject();
            static string Choice(string id) => "{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"seen\",\"options\":[{\"id\":\"" + id + "\"}]}";
            var duel = A("duel", "{\"op\":\"useSelectedActorDuel\",\"target\":\"owner\"}", targets: 1); duel["targetKind"] = "otherLiving"; duel["usesPerPhase"] = 2;
            var incoming = A("incoming", "{\"op\":\"offerVirtualSlashOrDraw\",\"target\":\"selectedTarget\"}", targets: 1); incoming["targetKind"] = "otherLiving";
            var acts = new JsonArray(A("stock-all", "{\"op\":\"draw\",\"target\":\"owner\",\"amount\":20}"), A("stock-four", "{\"op\":\"draw\",\"target\":\"owner\",\"amount\":4}"),
                A("give", "{\"op\":\"giveSelected\",\"target\":\"selectedTarget\"}", 1, 1), A("wound", "{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":1}"),
                A("self-dying", "{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":13}"), A("peer-dying", "{\"op\":\"loseHp\",\"target\":\"selectedTarget\",\"amount\":12}", targets: 1),
                A("ban-slash", "{\"op\":\"grantTurnCardActionProhibition\",\"target\":\"owner\",\"cardKinds\":[\"slash\",\"fireSlash\",\"thunderSlash\"],\"actionTypes\":[\"use\"]}"),
                A("lose-sirui", "{\"op\":\"loseOwnerSkillsAndGrant\",\"target\":\"owner\",\"skillIds\":[\"le:sirui\"],\"sourceBind\":\"" + SmRegrant + "\"}"), A("grant-sirui", "{\"op\":\"grantSkills\",\"target\":\"owner\",\"skillIds\":[\"le:sirui\"]}"), duel, incoming);
            if (scene == "general-weapon") acts.Add(A("weapon", "{\"op\":\"equipSampledGenerals\",\"target\":\"owner\",\"amount\":1}"));
            var driver = new JsonObject { ["id"] = SmDriver, ["revision"] = 1, ["activations"] = acts, ["modifiers"] = N("[{\"id\":\"keep\",\"query\":\"handLimit\",\"operation\":\"add\",\"value\":40,\"priority\":0}]") };
            if (scene == "provider") driver["modifiers"]!.AsArray().Add(N("{\"id\":\"real-provider-test-attack\",\"query\":\"slashDistanceLimit\",\"operation\":\"unlimited\",\"value\":0,\"priority\":0}"));
            if (scene is "alcohol" or "dodge" or "played-duel" or "played-arrows" or "played-barbarian" or "provider") driver["viewAs"] = N("[{\"id\":\"ordinary-fallback\",\"inputKinds\":[],\"inputSuits\":[],\"outputKind\":\"" + (scene is "dodge" or "played-arrows" or "provider" ? "dodge" : "slash") + "\",\"forPlay\":" + (scene == "alcohol" ? "true" : "false") + ",\"forResponse\":" + (scene == "alcohol" ? "false" : "true") + ",\"allowSameKind\":true}]");
            var defs = new JsonArray(driver, N($$"""{"id":"{{SmQuiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}"""),
                N($$"""{"id":"{{SmRegrant}}","revision":1,"modifiers":[{"id":"real-granted-neutral","query":"handLimit","operation":"add","value":1,"priority":0}]}"""));
            var own = new List<string> { SmDriver, scene is "paid-loss" or "distance-loss" ? SmConditional : SmSkill };
            if (scene is "paid-loss" or "distance-loss") defs.Add(N($$"""{"id":"{{SmConditional}}","revision":1,"hpSkillGrants":[{"id":"healthy-method","skillId":"{{SmSkill}}","minimumHp":12}]}"""));
            if (scene == "dianjun") own.Add("ol:dianjun");
            if (scene == "distance-loss") own.Add("ol:yidian");
            if (scene == "qinglong") own.Add(SmSetup);
            if (scene is "played-arrows" or "played-barbarian") own.Add("ol:huiyun");
            if (scene == "forced") defs.Add(N($$"""{"id":"{{SmForced}}","revision":1,"triggers":[{"id":"real-foreign-request-in-owner-play","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"requestSlashAgainstChosenTarget","target":"selectedTarget","chooserRef":{"kind":"selectedTarget"},"resultBind":"forced-use"}]}]}"""));
            if (scene == "regain") { defs.Add(N($$"""{"id":"{{SmInsert}}","revision":1,"triggers":[{"id":"new-real-play","window":"playEnding","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"insertPhase","target":"selectedTarget","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}""")); own.Add(SmInsert); }
            var mixed = scene is "pool" or "snatch" or "banned" or "regain" or "null";
            if (mixed || scene == "qinglong") defs.Add(N($$"""{"id":"{{SmSetup}}","revision":1,"triggers":[{"id":"actual-native-weapon","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"useRandomDeckEquipment","target":"owner","targetRef":{"kind":"owner"},"resultBind":"actual-equipment"}]}]}"""));
            if (scene == "qinglong") defs.Add(N($$"""{"id":"{{SmPeerDodge}}","revision":1,"viewAs":[{"id":"actual-two-character-dodge","inputKinds":["duel"],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true}]}"""));
            var watch = new JsonArray();
            if (scene is "dodge" or "null" or "rescue-peach" or "rescue-alcohol" or "forced" or "paid-loss" or "paid-death" or "distance-loss" or "general-weapon" or "qinglong")
                foreach (var zone in new[] { "hand", "equipment" })
                    watch.Add(N("{\"id\":\"real-method-cost-" + zone + "\",\"window\":\"cardsMoved\",\"subject\":\"owner\",\"sourceZones\":[\"" + zone + "\"],\"movementOccurrence\":\"perBatch\",\"movementReasons\":[\"" + CardMoveReasons.Use.Value + "\",\"" + CardMoveReasons.Respond.Value + "\",\"" + CardMoveReasons.Nullification.Value + "\"],\"optional\":false,\"effects\":[" + Choice("cost") + (scene is "paid-loss" or "paid-death" or "distance-loss" ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + (scene == "paid-death" ? 20 : 2) + "}" : "") + "]}"));
            if (scene is "paid-loss" or "paid-death") watch.Add(N("{\"id\":\"real-lion-recovery\",\"window\":\"afterHpRecovered\",\"subject\":\"owner\",\"optional\":false,\"effects\":[" + Choice("recovered") + "]}"));
            if (watch.Count > 0) { defs.Add(new JsonObject { ["id"] = SmWatch, ["revision"] = 1, ["triggers"] = watch }); own.Add(SmWatch); }
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "真实思锐原生使用与付款夹具", optionLabels = new[] { "activations", "triggers" }.SelectMany(k => n[k]?.AsArray() ?? new JsonArray()).SelectMany(t => t!["effects"]!.AsArray()).SelectMany(e => e!["options"]?.AsArray() ?? new JsonArray()).Select(o => o!["id"]!.GetValue<string>()).Distinct().ToDictionary(x => x, x => x) });
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels })); foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, "真实思锐夹具") { Program = p, ProgramPresentation = c.Presentations[id] });
            const string pick = "fixture:lms-pick"; b.AddSkill(new(pick, "本人", "固定真人候选") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => -100000000d) }); b.AddGeneral(new(SmOwner, "真实乐祢衡思锐者", "supporter", pick, scene == "provider" ? "wei" : "qun", 12, own));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:lms-peer-" + i).ToArray();
            for (var i = 1; i <= 3; i++) { var pp = "fixture:lms-peer-pick-" + i; b.AddSkill(new(pp, "其他角色", "固定公开候选") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => 1000000d * (4 - i)) }); var skills = new List<string> { SmQuiet }; if (mixed) skills.Add(SmSetup); if (i == 1 && scene == "forced") skills.Add(SmForced); if (i == 1 && scene == "qinglong") skills.Add(SmPeerDodge); if (scene == "provider") skills.Add("classic:hujia"); b.AddGeneral(new(peers[i - 1], "真实对局角色", "supporter", pp, scene == "provider" ? "wei" : "shu", 12, skills)); }
            if (scene == "general-weapon") { const string reservePick = "fixture:lms-reserve-pick"; b.AddSkill(new(reservePick, "真实未选备用", "只提供原生魂牌来源") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, _ => -200000000d) }); b.AddGeneral(new("fixture:lms-reserve", "真实未选武将", "supporter", reservePick, "shu", 4, [])); }
            var singleKind = scene == "grain" ? CardKind.WoodenOx : scene == "alcohol" ? CardKind.Alcohol : scene is "paid-loss" or "paid-death" ? CardKind.SilverLion : scene == "general-weapon" ? CardKind.Duel : CardKind.Slash;
            const string single = "fixture:lms-physical-card"; var definition = CardCatalog.Get(singleKind);
            b.AddCard(new(single, definition.DisplayName, definition.CategoryName, "真实同名原生牌堆实体", singleKind));
            if (scene is "played-arrows" or "played-barbarian") { var group = CardCatalog.Get(scene == "played-arrows" ? CardKind.ArrowBarrage : CardKind.BarbarianAssault); b.AddCard(new("fixture:lms-natural-group", group.DisplayName, group.CategoryName, "真实晖云展示并使用的原AOE", group.Kind)); }
            if (scene == "qinglong") b.AddCard(new("fixture:lms-real-qinglong", "青龙偃月刀", "装备牌", "真实青龙后续杀", CardKind.QinglongCrescentBlade));
            var cards = scene is "played-arrows" or "played-barbarian" ? Enumerable.Range(0, 25).Select(i => new ContentDeckPhysicalCard(i == 0 ? "fixture:lms-natural-group" : "standard:slash", Suit.Heart, 7)).ToArray() :
                scene == "qinglong" ? Enumerable.Range(0, 25).Select(i => new ContentDeckPhysicalCard(i == 0 ? "fixture:lms-real-qinglong" : i <= 12 ? "standard:slash" : "standard:duel", Suit.Heart, 7)).ToArray() :
                mixed ? Enumerable.Range(0, scene == "pool" ? 44 : 36).Select(i => new ContentDeckPhysicalCard(i < 12 ? "standard:slash" : i < 24 ? "standard:duel" : "standard:crossbow", Suit.Heart, 7)).ToArray() : Enumerable.Range(0, 24).Select(i => new ContentDeckPhysicalCard(single, Suit.Heart, 7)).ToArray();
            b.AddDeck(new("fixture:lms-deck", "真实一二四字牌名", scene == "provider" ? 4 : 0, 0, []) { PhysicalCards = cards });
            var roles = scene is "paid-death" or "provider" ? new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 } : new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 };
            b.AddMode(new(SmMode, "真实思锐原生边界", 4, 4, roles, "fixture:lms-deck", GeneralCandidateCount: scene == "general-weapon" ? 5 : 4, GeneralPoolIds: scene == "general-weapon" ? [SmOwner, .. peers, "fixture:lms-reserve"] : [SmOwner, .. peers]));
        }
    }
}

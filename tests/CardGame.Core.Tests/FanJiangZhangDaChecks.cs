using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class FanJiangZhangDaChecks
{
    private const string Skill = "ol:juesheng", Armor = "ol:yuanchou", Owner = "fixture:fjzd-owner", Driver = "fixture:fjzd-driver", Quiet = "fixture:fjzd-quiet",
        Watch = "fixture:fjzd-watch", Health = "fixture:fjzd-health", Mode = "identity:classic-fan-jiang-zhang-da-fixture";
    private static PlayerSnapshot V(GameEngine g, int seat) => g.CreateSnapshot(seat).Players[seat];
    private static bool HasSkill(GameEngine g, int seat, string skill) { var skills = V(g, seat).Skills; Need(skills is not null, "A character's own public skill snapshot must exist."); return skills!.Any(s => s.Id == skill); }
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static T[] E<T>(GameEngine g) where T : IGameEvent => g.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static bool Action(GameEngine g, string action) => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == action) == true;
    private static bool Optional(GameEngine g, string skill) => P(g)?.SkillPrompt?.SkillId == skill && Action(g, "activate");
    private static bool BingTargets(GameEngine g) => P(g)?.SkillPrompt?.SkillId == "ol:bingzhong" && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "bingzhong-target");
    private static void Need(bool yes, string why) { if (!yes) throw new InvalidOperationException(why); }
    private static void Accept(GameEngine g, GameCommand command) { var result = g.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected genuine Fan Jiang Zhang Da command."); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> predicate) { var p = P(g)!; var choice = p.Choices.FirstOrDefault(predicate); Need(p.PlayerSeat == 0 && choice is not null, "Missing actual human choice: " + Diagnostic(g)); Accept(g, new AnswerPromptCommand(0, p.PromptId, choice!.Id, g.Revision)); }
    private static void Continue(GameEngine g) => Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
    private static void Use(GameEngine g, string id, int[]? targets = null) => Accept(g, new UseProgramSkillCommand(0, Driver, id, [], targets ?? [], g.Revision, P(g)!.PromptId));
    private static void Juesheng(GameEngine g, int target) => Accept(g, new UseProgramSkillCommand(0, Skill, "limited-duel", [], [target], g.Revision, P(g)!.PromptId));
    private static void End(GameEngine g) => Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
    private static void Draw(GameEngine g) { Use(g, "draw"); Play(g); }
    private static void Incoming(GameEngine g, int actor) { Use(g, "incoming", [actor]); Reach(g, () => P(g) is { PlayerSeat: 0 } && Action(g, "virtual-slash-offer") && P(g)!.Choices.Any(c => c.Targets.SequenceEqual([0]))); Answer(g, c => c.Targets.SequenceEqual([0])); }
    private static void Native(GameEngine g, Func<LegalAction, bool> predicate) { var a = g.GetHumanLegalActions().FirstOrDefault(predicate); Need(a is not null, "Missing native card action: " + Diagnostic(g)); Accept(g, new PlayCardCommand(0, a!.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources ?? [] }); }
    private static void Play(GameEngine g) => Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Reach(GameEngine g, Func<bool> done, [CallerMemberName] string caller = "", [CallerLineNumber] int line = 0)
    {
        for (var i = 0; i < 320; i++)
        {
            if (done()) return; Need(g.State.Winner == Winner.None, $"Ended before {caller}:{line}: " + Diagnostic(g)); var p = P(g);
            if (p is null || p.PlayerSeat != 0) Accept(g, new AdvanceOneStepCommand(g.Revision));
            else if (p.Kind == DecisionKind.PlayCard) End(g);
            else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash) Answer(g, c => c.Cards.Count == 0);
            else if (p.Kind == DecisionKind.Nullification) Answer(g, c => c.Parameters.GetValueOrDefault("response") == "pass");
            else if (p.Kind == DecisionKind.RescueDying) Answer(g, c => c.Parameters.GetValueOrDefault("response") is "let-die" or "pass");
            else if (p.SkillPrompt?.SkillId is Watch or Health) Continue(g);
            else if (Action(g, "skip")) Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
            else if (P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "decline")) Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "decline");
            else throw new InvalidOperationException($"Unexpected {caller}:{line}: " + Diagnostic(g));
        }
        throw new InvalidOperationException($"Missing bounded {caller}:{line}: " + Diagnostic(g));
    }
    private static string Diagnostic(GameEngine g) => JsonSerializer.Serialize(new { g.State.TurnNumber, g.State.CurrentSeat, g.State.Phase, Players = Enumerable.Range(0, 4).Select(s => new { Seat = s, V(g, s).Hp, V(g, s).HandCount, V(g, s).IsAlive }), Prompt = P(g) is { } p ? new { p.PlayerSeat, p.Kind, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Take(6).Select(c => new { c.Cards, c.Targets, c.Parameters }) } : null, Frames = g.ResolutionStack.TakeLast(5).Select(f => new { f.Id, Type = f.GetType().Name }), Facts = g.Events.TakeLast(5).Select(e => e.Payload.ToString()) });
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))), Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), g.CardMovements, Events = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(g.AcceptedCommands) });
    private static GameEngine Cold(GameEngine g, ContentRegistry r) { var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), r); Need(State(g) == State(copy), "Cold recovery preserves every permitted private view, permanent spend, lifetime facts, whole Duel and independently owned temporary grant."); return copy; }
    private static (GameEngine, ContentRegistry) Start(string scene)
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene)); var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = scene == "lion-death" ? Role.Rebel : Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r);
        Accept(g, new StartGameCommand()); Reach(g, () => P(g) is { Kind: DecisionKind.SelectGeneral }); Accept(g, new SelectGeneralCommand(0, Owner, g.Revision, P(g)!.PromptId)); Play(g); return (g, r);
    }
    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-fan-jiang-zhang-da", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "ol:fan-jiang-zhang-da");
            if (scene == "committed-zero-rank") CharacterFixtureContent.Register(b, "wumiao:huangfu-song");
            if (scene == "multi") { CharacterFixtureContent.Register(b, "ol:yang-fu"); CharacterFixtureContent.Register(b, "ol:wang-rong"); }
            if (scene == "effective-red") CharacterFixtureContent.Register(b, "classic:xiao-qiao");
            var armor = scene is "black" or "red" or "effective-red" or "actor-black"; var material = scene.StartsWith("lion", StringComparison.Ordinal) ? "silverLion" : scene == "ox" ? "woodenOx" : scene == "multi" ? "duel" : "crossbow";
            var defs = new List<string> { $$"""
            {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":50,"priority":0},{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0},{"id":"quota","query":"slashLimit","operation":"add","value":4,"priority":0}],"viewAs":[
             {"id":"slash","inputKinds":[],"inputSuits":[],"sourceZones":["hand","equipment"],"outputKind":"slash","forPlay":true,"forResponse":{{(scene.StartsWith("lion",StringComparison.Ordinal)||scene is "ox" or "reverse"?"true":"false")}},"allowSameKind":true},
             {"id":"dodge","inputKinds":[],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true,"allowSameKind":true},
             {"id":"borrowed","inputKinds":[],"inputSuits":[],"outputKind":"borrowedSword","forPlay":true,"forResponse":false,"singleCardTrickUse":true,"allowSameKind":true}],"activations":[
             {"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":20}]},
             {"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
             {"id":"wound-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":2}]},
             {"id":"regrant","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["ol:juesheng"]}]},
             {"id":"flip-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"turnOver","target":"selectedTarget"}]},
             {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"offerVirtualSlashOrDraw","target":"selectedTarget"}]},
             {"id":"equip-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment","awaitMovementTriggers":true}]}]}
            """, $$"""{"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}""",
                $$"""{"id":"fixture:fjzd-response","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0}],"viewAs":[{"id":"actual-slash","inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":{{(scene is "reverse" or "lion-loss" or "lion-death" or "ox" ? "true" : "false")}},"allowSameKind":true}]}""" };
            if (scene is "actor" or "actor-black") defs.Add("""{"id":"fixture:fjzd-actor","revision":1,"triggers":[{"id":"actual-actor","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},{"op":"replaceCurrentCardUseActor","target":"selectedTarget"}]}]}""");
            if (scene == "actor-black") defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-rewritten-black","window":"cardUseBeforeTargetEffects","ownerRelation":"observer","cardKinds":["slash"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"rewritten","options":[{"id":"continue"}]}]}]}""");
            if (scene == "multi") defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-native-damage","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"damage","options":[{"id":"continue"}]}]}]}""");
            if (scene.StartsWith("committed-", StringComparison.Ordinal)) defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-committed-duel","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["duel"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"committed","options":[{"id":"continue"}]}]}]}""");
            if (scene == "nullified") defs.Add("""{"id":"fixture:fjzd-nullification","revision":1,"viewAs":[{"id":"real-nullification","inputKinds":["crossbow"],"inputSuits":[],"outputKind":"nullification","forPlay":false,"forResponse":true}]}""");
            if (scene == "lion-loss") defs.Add("""{"id":"fixture:fjzd-conditional","revision":1,"hpSkillGrants":[{"id":"healthy-source","skillId":"ol:juesheng","minimumHp":12}]}""");
            if (scene.StartsWith("lion", StringComparison.Ordinal)) defs.Add($$"""{"id":"{{Health}}","revision":1,"triggers":[{"id":"real-lion-health","window":"afterHpRecovered","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]},{"op":"loseHp","target":"owner","amount":{{(scene=="lion-death"?20:2)}}}]}]}""");
            var names = defs.Select(d => JsonNode.Parse(d)!).ToDictionary(d => d["id"]!.GetValue<string>(), d => new { name = d["id"]!.GetValue<string>(), description = "真实决生原生夹具", optionLabels = new[] { "activations", "triggers" }.SelectMany(k => d[k]?.AsArray() ?? new JsonArray()).SelectMany(t => t!["effects"]!.AsArray()).SelectMany(e => e!["options"]?.AsArray() ?? new JsonArray()).Select(o => o!["id"]!.GetValue<string>()).Distinct().ToDictionary(id => id, id => id) });
            var catalog = SkillProgramCatalog.Load($$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{{string.Join(',',defs)}}]}""", JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "实际原生指令") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddSkill(new("fixture:fjzd-pick", "实际本人", "固定公开选将权重") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => -100000d) });
            var own = new List<string> { Driver, Armor, scene == "lion-loss" ? "fixture:fjzd-conditional" : Skill };
            if (scene == "effective-red") own.Add("classic:hongyan"); if (scene is "actor" or "actor-black") own.Add("fixture:fjzd-actor"); if (scene == "actor-black") own.Add(Watch);
            if (scene == "nullified") own.Add("fixture:fjzd-nullification");
            if (scene.StartsWith("committed-", StringComparison.Ordinal)) own.Add(Watch);
            if (scene == "multi") own.AddRange(["ol:bingzhong", "ol:fengzi", Watch]); if (scene.StartsWith("lion", StringComparison.Ordinal)) own.Add(Health);
            b.AddGeneral(new(Owner, "实际范疆张达", "ol-fan-jiang-zhang-da", "fixture:fjzd-pick", "wu", 12, own, GeneralGender.Male));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:fjzd-peer-" + i).ToArray();
            foreach (var peer in peers) { var skills = new List<string> { "fixture:fjzd-response" }; if (scene == "actor-black") skills.Add(Armor); if (scene == "multi") skills.Add(Skill); b.AddGeneral(new(peer, "实际其他角色", "supporter", Quiet, "qun", 12, skills)); }
            var kind = Enum.Parse<CardKind>(material, true); var kinds = armor ? new[] { CardKind.Crossbow, CardKind.Tengjia } : [kind];
            foreach (var actual in kinds) b.AddCard(new("fixture:fjzd-card-" + actual, "实际" + actual, CardCatalog.Get(actual).CategoryName, "原生实体", actual));
            b.AddDeck(new("fixture:fjzd-deck", "固定有界原生牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard("fixture:fjzd-card-" + kinds[i % kinds.Length], scene == "red" ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "范疆张达实际原生小局面", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, "fixture:fjzd-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}

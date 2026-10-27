using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WuJingChecks
{
    private const string Heji = "ol:heji", Liubing = "ol:liubing", Owner = "fixture:wj-owner", Driver = "fixture:wj-driver", Quiet = "fixture:wj-quiet", Convert = "fixture:wj-convert", Response = "fixture:wj-response", Watch = "fixture:wj-watch", Health = "fixture:wj-health", Conditional = "fixture:wj-conditional", Prevent = "fixture:wj-prevent", Zero = "fixture:wj-zero", Fire = "fixture:wj-fire", Mode = "identity:classic-wu-jing-fixture";
    private static PlayerSnapshot V(GameEngine g, int s = 0) => g.CreateSnapshot(s).Players.Single(p => p.Seat == s);
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static WuJingReceipt? Current(GameEngine g) => g.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.WuJing is not null)?.WuJing;
    private static T[] E<T>(GameEngine g) where T : IGameEvent => g.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static void Need(bool yes, string why) { if (!yes) throw new InvalidOperationException(why); }
    private static void Accept(GameEngine g, GameCommand c) { var result = g.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Wu Jing actual command."); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> pick) { var p = P(g) ?? throw new InvalidOperationException("A Wu Jing answer requires a real current prompt: " + Diagnostic(g)); Need(V(g, p.PlayerSeat).IsHuman, "Only the real human may answer; AI uses AdvanceOneStep."); var selected = p.Choices.FirstOrDefault(pick); if (selected is null) throw new InvalidOperationException("No published Wu Jing choice matches the actual requested action: " + Diagnostic(g)); Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, selected.Id, g.Revision)); }
    private static bool Optional(GameEngine g, string skill) => P(g)?.SkillPrompt?.SkillId == skill && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    private static void Activate(GameEngine g) => Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    private static void Continue(GameEngine g) => Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
    private static void Follow(GameEngine g, CardKind kind) => Answer(g, c => c.Cards.Count > 0 && c.Parameters.GetValueOrDefault("kind") == kind.ToString());
    private static void Use(GameEngine g, string id, int[]? cards = null, int[]? targets = null) => Accept(g, new UseProgramSkillCommand(0, Driver, id, cards ?? [], targets ?? [], g.Revision, P(g)!.PromptId));
    private static void Native(GameEngine g, CardKind kind, int target, int? id = null)
    {
        var a = g.GetHumanLegalActions().First(a => a.CardId is not null && (id is null || a.CardId == id) && a.TargetSeats.SequenceEqual([target]) && (a.PlayedCardKind ?? V(g).Hand.Single(c => c.Id == a.CardId).Kind) == kind);
        Accept(g, new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
    }
    private static void End(GameEngine g) => Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
    private static void Play(GameEngine g) => Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Reach(GameEngine g, Func<bool> done)
    {
        for (var i = 0; i < 500; i++)
        {
            if (done()) return; if (g.State.Status == EngineStatus.Completed) throw new InvalidOperationException("Wu Jing completed before expected boundary: " + Diagnostic(g)); var p = P(g);
            if (p is not null && !V(g, p.PlayerSeat).IsHuman) Accept(g, new AdvanceOneStepCommand(g.Revision));
            else if (Optional(g, Heji) || Optional(g, "ol:fengzi")) Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
            else if (Current(g) is { Stage: WuJingStage.UseChoice } && p?.SkillPrompt?.SkillId == Heji && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "wu-jing")) Answer(g, c => c.Parameters.GetValueOrDefault("kind") == "decline");
            else if (p is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification }) Answer(g, c => c.Cards.Count == 0);
            else if (p is { PlayerSeat: 0, Kind: DecisionKind.RescueDying }) Answer(g, c => c.Parameters.GetValueOrDefault("response") == "let-die");
            else if (p?.SkillPrompt?.SkillId is Watch or Health) Continue(g);
            else if (p?.SkillPrompt?.SkillId == "ol:kenshang" && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "keep")) Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "keep");
            else if (p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) End(g);
            else if (p is not null) throw new InvalidOperationException("Unexpected Wu Jing prompt: " + Diagnostic(g));
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Wu Jing missing actual boundary: " + Diagnostic(g));
    }
    private static string Diagnostic(GameEngine g) => JsonSerializer.Serialize(new { g.State.TurnNumber, g.State.CurrentSeat, g.State.Phase, g.State.Winner, Prompt = P(g), Players = Enumerable.Range(0, 4).Select(s => { var p = V(g, s); return new { p.Seat, p.Role, p.Hp, p.HandCount, Skills = p.Skills?.Select(x => x.ContentId) }; }), Frames = g.ResolutionStack.TakeLast(6).Select(f => new { f.Id, Kind = f.GetType().Name }), Uses = E<CardUseDeclaredEvent>(g).TakeLast(6), ActualPhases = E<ActualDiscardRecoveryPhaseStartedEvent>(g).TakeLast(4), Facts = g.Events.TakeLast(10).Select(e => e.Payload.ToString()) });
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))), Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), g.CardMovements, Commands = CommandJson.Serialize(g.AcceptedCommands), Zones = g.CreateCardZoneDiagnostics() });
    private static GameEngine Cold(GameEngine g, ContentRegistry r) { var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), r); Need(State(g) == State(restored), "Cold replay preserves all privacy views, actual-use appearance, original materials, genuine child returns and seeded red rewards exactly."); return restored; }
    private static (GameEngine, ContentRegistry) Start(string scene)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene)); var g = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = scene == "reward-death" ? Role.Renegade : Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(g, new StartGameCommand()); Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }); Accept(g, new SelectGeneralCommand(0, Owner, g.Revision, P(g)!.PromptId)); Play(g); return (g, registry);
    }
    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wu-jing", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "ol:wu-jing"); if (scene == "extra") CharacterFixtureContent.Register(b, "ol:wang-rong"); if (scene == "kenshang") CharacterFixtureContent.Register(b, "ol:ma-xiu-ma-tie", "standard:demo-mashu");
            var rules = JsonNode.Parse($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"slash-room","query":"slashLimit","operation":"add","value":12,"priority":0},{"id":"keep-hand","query":"handLimit","operation":"add","value":20,"priority":0}],"viewAs":[{"id":"pair","inputKinds":[],"inputSuits":[],"inputCount":2,"sourceZones":["hand"],"outputKind":"slash","forPlay":true,"forResponse":false,"allowSameKind":true}],"activations":[
                {"id":"pair","minCards":2,"maxCards":2,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useSelectedCardsAs","target":"selectedTarget","sourceBind":"pair","outputKind":"slash"}]},
                {"id":"incoming-duel","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,"effects":[{"op":"useSelectedActorDuel","target":"owner"}]},
                {"id":"trim","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]},
                {"id":"lose-liubing","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{Liubing}}"],"sourceBind":"standard:none"}]},
                {"id":"gain-liubing","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{Liubing}}"]}]},
                {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"gear"}]},
                {"id":"lose-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                {"id":"chain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"owner","chained":true}]}]},
              {"id":"{{Zero}}","revision":1,"viewAs":[{"id":"round-slash","inputCount":0,"sourceZones":[],"inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false,"roundOnceCardlessBasic":{"usageGroup":"wj-round","usesPerRound":1} },{"id":"funded-slash","inputCount":0,"sourceZones":[],"inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false,"useOnly":true,"drawFundedDistinctBasic":{"methodLedgerId":"wj-funded"} },{"id":"release-slash","inputCount":0,"sourceZones":[],"inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false,"useOnly":true,"chainedStateCost":false}]},
              {"id":"{{Convert}}","revision":1,"viewAs":[{"id":"duel","inputKinds":[],"inputSuits":[],"sourceZones":["hand"],"outputKind":"duel","forPlay":true,"forResponse":false,"singleCardTrickUse":true},{"id":"he-slash","inputKinds":["silverLion"],"inputSuits":[],"sourceZones":["hand","equipment"],"outputKind":"slash","forPlay":true,"forResponse":false}]},
              {"id":"{{Response}}","revision":1,"viewAs":[{"id":"real-dodge","inputKinds":["slash"],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Conditional}}","revision":1,"hpSkillGrants":[{"id":"healthy-heji","skillId":"{{Heji}}","minimumHp":{{(scene=="draw-zero-loss"?5:8)}}}]},
              {"id":"{{Health}}","revision":1,"triggers":[{"id":"actual-hp-child","window":"{{(scene=="lion"?"afterHpRecovered":"afterDamageApplied")}}","subject":"owner",{{(scene=="lion"?"":"\"damageOccurrence\":\"perDamage\",")}}"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-paid-child","window":"{{(scene=="lion"?"cardsMoved":"cardsGained")}}","subject":"owner",{{(scene=="lion"?"\"sourceZones\":[\"equipment\"],":"\"destinationZones\":[\"hand\"],")}}"movementOccurrence":"perBatch","movementReasons":["{{(scene=="lion"?CardMoveReasons.Use.Value:scene=="draw-zero-loss"?"skill-program.draw-funded-distinct-basic.draw":"skill-program.wu-jing.heji.red")}}"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]},{{(scene=="lion"?"{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":2}":"{\"op\":\"damage\",\"target\":\"owner\",\"amount\":"+(scene=="reward-death"?20:1)+"}, {\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":1}")}}]}]},
              {"id":"{{Prevent}}","revision":1,"triggers":[{"id":"prevent-first","window":"beforeDamageApplied","subject":"damageTarget","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"preventCurrentDamage","target":"owner"}]}]},
              {"id":"{{Fire}}","revision":1,"triggers":[{"id":"actual-fire","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":true,"effects":[{"op":"offerCurrentSlashFireAndExtraTarget","target":"owner"}]}]}
            ]}
            """)!;
            if (scene == "chain-zero") rules["skills"]!.AsArray().Single(n => n!["id"]!.GetValue<string>() == Watch)!["triggers"] = JsonNode.Parse("""[{"id":"real-paid-zero-use","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]""");
            var ids = new[] { Driver, Convert, Response, Quiet, Conditional, Watch, Health, Prevent, Zero, Fire }; var catalog = SkillProgramCatalog.Load(rules.ToJsonString(), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = ids.ToDictionary(id => id, id => new { name = id, description = "吴景真实用牌与支付边界", optionLabels = id is Watch or Health ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "真实边界") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            var human = scene == "reward-death" ? Role.Renegade : Role.Lord; b.AddSkill(new("fixture:wj-pick-owner", "本人评分", "真实选将") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, role => role == human ? 100000d : -100000d) });
            var own = new List<string> { Driver, Liubing }; if (scene is "lion" or "reward-loss" or "reward-death" or "draw-zero-loss") own.AddRange([Conditional, Watch, Health]); else if (scene is not ("foreign" or "multi" or "extra")) own.Add(Heji);
            if (scene.Contains("zero", StringComparison.Ordinal)) own.Add(Zero); if (scene == "chain-zero") own.Add(Watch); if (scene == "kenshang") own.Add("ol:kenshang");
            if (scene.EndsWith("-fire", StringComparison.Ordinal)) own.Add(Fire);
            if (scene is "conversion" or "lion") own.Add(Convert); if (scene is "claim" or "black" or "damaged") own.Add(Response); if (scene == "extra") own.Add("ol:fengzi");
            b.AddGeneral(new(Owner, "吴景真实本人", "ol-wu-jing", "fixture:wj-pick-owner", "wu", scene is "draw-zero" or "draw-zero-loss" ? 4 : 8, own, GeneralGender.Male));
            var peers = Enumerable.Range(1, 3).Select(i => $"fixture:wj-peer-{i}").ToArray();
            foreach (var (id, index) in peers.Select((id, index) => (id, index)))
            {
                var pick = "fixture:wj-pick-" + index; b.AddSkill(new(pick, "其他评分", "固定原生顺序") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, role => role == human ? -100000d : 100003d - index) }); var skills = new List<string>();
                if (scene is not ("claim" or "black" or "damaged")) skills.Add(Quiet);
                if (scene == "foreign" && index == 0) skills.Add(Heji); if (scene is "multi" or "extra" && index == 0) skills.Add(Liubing); if (scene == "multi") skills.Add(Response); if (scene == "extra") skills.Add(Prevent);
                b.AddGeneral(new(id, "其他真实角色", "supporter", pick, "wu", 8, skills, GeneralGender.Male));
            }
            b.AddCard(new("fixture:wj-lion", "白银狮子", "装备牌", "真实支付及回复", CardKind.SilverLion));
            b.AddDeck(new("fixture:wj-deck", "固定实体牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard(scene == "lion" ? "fixture:wj-lion" : scene is "recursive" or "foreign" or "reward-loss" or "reward-death" or "kenshang" || scene.Contains("zero", StringComparison.Ordinal) ? "standard:duel" : "standard:slash", scene is "recursive" or "reward-loss" or "reward-death" or "claim" or "damaged" or "extra" or "kenshang" or "physical-fire" || scene.Contains("zero", StringComparison.Ordinal) ? Suit.Heart : scene == "multi" && i % 2 == 0 ? Suit.Heart : Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "吴景原生完整规则", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [(scene is "claim" or "black" or "damaged" ? nameof(Role.Rebel) : nameof(Role.Renegade))] = 3 }, "fixture:wj-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}

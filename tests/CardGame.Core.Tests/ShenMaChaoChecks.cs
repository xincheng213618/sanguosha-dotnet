using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ShenMaChaoChecks
{
    private const string Owner = "fixture:shen-ma-chao-owner", Driver = "fixture:shen-ma-chao-driver", Quiet = "fixture:shen-ma-chao-quiet", Probe = "fixture:shen-ma-chao-probe";
    public static (GameEngine Game, ContentRegistry Registry) CreateForeignMountUiScenario()
    { var scene = new Scene(false); scene.Play(); return (scene.G, scene.Registry); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class Scene
    {
        public GameEngine G; private readonly ContentRegistry _registry;
        public ContentRegistry Registry => _registry;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.ShenMaChao is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public int HandLimit(int seat)
        {
            var players = (System.Collections.IList)typeof(GameEngine).GetField("_players", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(G)!;
            return (int)typeof(GameEngine).GetMethod("GetHandLimit", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(G, [players[seat]])!;
        }
        public IEnumerable<T> E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>();
        public Scene(bool defense, bool withWine = false)
        {
            _registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture(defense, withWine));
            G = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
                ModeId = "identity:classic-shen-ma-chao-fixture", UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, _registry);
            Accept(new StartGameCommand()); Reach(() => P is { Kind: DecisionKind.SelectGeneral });
            Accept(new SelectGeneralCommand(0, Owner, G.Revision, P!.PromptId));
        }
        private void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Native command rejected."); }
        public void Answer(Func<PromptChoice, bool> choose) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void Use(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void EmptyHand() { if (V(0).HandCount == 0) return; Use("empty", []); Play(); }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void PlayCard(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 320; step++)
            {
                if (done()) return;
                if (P?.SkillPrompt?.SkillId == "anniversary:hengwu") throw new InvalidOperationException("Hengwu unexpectedly offered another optional trigger for the same actual action.");
                if (P is { Kind: DecisionKind.ProgramTrigger } && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (P is { PlayerSeat: 0 } && P.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "select-god-faction")) Answer(c => c.Parameters.GetValueOrDefault("faction-id") == "wei");
                else if (P is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying }) Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Shen Ma Chao boundary was not reached: " + JsonSerializer.Serialize(new { Prompt = P, Frames = G.ResolutionStack }));
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Moves = G.CardMovements });
        public void Cold() { var state = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), _registry); Need(state == State(), "Cold restore must preserve the original setup/use parent and native accepted costs."); }
        public void Replay() { var state = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, _registry); Need(state == State(), "Accepted-command replay preserves Shouli and Hengwu's genuine entity movements."); }
    }
    private sealed class Fixture(bool defense, bool withWine = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shen-ma-chao", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            var json = $$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Quiet}}","revision":1,"triggers":[{"id":"setup-pause","window":"cardUseCommitted","ownerRelation":"actor","cardCategories":["equipment"],"optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]},{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},{"id":"{{Probe}}","revision":1,"modifiers":[{"id":"real-hand-limit","query":"handLimit","operation":"add","value":3,"priority":0},{"id":"counter-distance","query":"slashDistanceLimit","operation":"unlimited","value":0,"priority":0}],"triggers":[{"id":"real-counter-slash","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"useVirtualSlash","target":"selectedTarget"}]}]},{"id":"{{Driver}}","revision":1,"viewAs":[{"id":"zero-field-trick","inputKinds":["defensiveHorse"],"inputSuits":[],"outputKind":"drawTwo","forPlay":true,"forResponse":false,"singleCardTrickUse":true}],"activations":[{"id":"empty","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]},{"id":"poke","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},{"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},{"id":"strip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"discardParticipantCards","target":"owner","zones":["equipment"],"amount":1}]},{"id":"one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}""";
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Quiet] = new { name = "安静同桌", description = "开局原生装备使用可暂停；正常摸牌后跳过出牌。" }, [Probe] = new { name = "真实失效与回击", description = "增加手牌上限，首次受伤后用原生虚拟杀回击。" }, [Driver] = new { name = "真实检查驱动", description = "使用原生伤害、弃牌与摸牌。" } } });
            var catalog = SkillProgramCatalog.Load(json, presentation);
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id], Tags = id == Quiet ? SkillTag.Locked : SkillTag.None });
            b.AddGeneral(new(Owner, "神马超夹具", "supporter", "anniversary:shouli", "god", 4, ["anniversary:hengwu", Driver], GeneralGender.Male));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:shen-ma-chao-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "安静同桌", "supporter", Quiet, "wei", 20, [Probe]));
            b.AddDeck(new("fixture:shen-ma-chao-deck", "真实坐骑实体牌堆", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 96).Select(index => new ContentDeckPhysicalCard(withWine && index % 3 != 0 ? "standard:alcohol" : defense ? "standard:defensive_horse" : "standard:offensive_horse", Suit.Spade, 5)).ToArray() });
            b.AddMode(new("identity:classic-shen-ma-chao-fixture", "神马超原生检查", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, "fixture:shen-ma-chao-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}

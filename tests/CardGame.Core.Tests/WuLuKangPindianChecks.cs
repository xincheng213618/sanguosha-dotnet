using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal sealed class WuLuKangScene(WuLuKangSpec spec, GameEngine game, ContentRegistry registry)
{
    public const string Shenduan = "wumiao:shenduan", Kegou = "wumiao:kegou", Dixian = "wumiao:dixian", Gushe = "ol:gushe";
    public const string Driver = "fixture:wlk-driver", Quiet = "fixture:wlk-quiet", Watch = "fixture:wlk-watch", Conditional = "fixture:wlk-sources";
    private const string Owner = "fixture:wlk-owner", Ally = "fixture:wlk-ally", EnemyA = "fixture:wlk-enemy-a", EnemyB = "fixture:wlk-enemy-b", Deck = "fixture:wlk-deck";
    public static ShanheStartingCard C(CardKind kind, int rank = 7, Suit suit = Suit.Spade) => new(kind, suit, rank);
    public GameEngine G { get; private set; } = game;
    public WuLuKangSpec Spec => spec;
    public PendingDecision? P => Enumerable.Range(0, 4).Select(s => G.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
    public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
    public bool Has(string skill, int seat = 0) => V(seat).Skills?.Any(s => s.ContentId == skill) == true;
    public bool Action(string action) => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == action) == true;
    public bool Optional(string skill) => P?.SkillPrompt?.SkillId == skill && Action("activate");
    public bool Hold(string option = "continue") => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == option);
    public CardZoneDiagnostic[] Stock() => G.CreateCardZoneDiagnostics().Where(c => c.Location == CardLocation.DrawPile).OrderByDescending(c => c.ZoneIndex).ToArray();
    public int StockRank(CardZoneDiagnostic card)
    {
        var ranks = spec.Stock.Where(c => c.Kind == card.CardKind).Select(c => c.Rank).Distinct().ToArray();
        Need(ranks.Length == 1, "The explicit stock fixture assigns one printed rank to each stock kind."); return ranks.Single();
    }
    public CardLocation Location(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
    public void Use(string id, int[]? targets = null, int[]? cards = null) => Accept(new UseProgramSkillCommand(0, Driver, id, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
    public void BeginPair(int target) => Use("pair", [target]);
    public void BeginGroup(int[] targets, int? card) => Accept(new UseProgramSkillCommand(0, Gushe, "simultaneous-pindian", card is { } id ? [id] : [], targets, G.Revision, P!.PromptId));
    public void BeginDixian()
    {
        Accept(new UseProgramSkillCommand(0, Dixian, "declare-rank", [], [], G.Revision, P!.PromptId)); Reach(() => Action("wu-lu-kang-dixian"));
    }
    public void ChooseRank(int rank) => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "wu-lu-kang-dixian" && c.Parameters.GetValueOrDefault("rank") == rank.ToString(System.Globalization.CultureInfo.InvariantCulture));
    public LegalAction[] Actions() => G.GetHumanLegalActions().ToArray();
    public void Native(CardKind kind, int[] targets, int? rank = null)
    {
        var a = Actions().FirstOrDefault(a => a.CardId is { } id && V().Hand.Any(c => c.Id == id && c.Kind == kind && (rank is null || c.Rank == rank)) &&
            a.Kind != LegalActionKind.Recast && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
        Need(a is not null, "Expected a genuine legal physical " + kind + " at native targets."); Native(a!);
    }
    public void Native(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
        { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
    public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
    public void Answer(Func<PromptChoice, bool> predicate)
    {
        var p = P; var choice = p?.Choices.FirstOrDefault(predicate); Need(p is { PlayerSeat: 0 } && choice is not null, "Only a currently published human choice may be answered.");
        Accept(new AnswerPromptCommand(0, p!.PromptId, choice!.Id, G.Revision));
    }
    public void Continue() => Answer(c => c.Parameters.ContainsKey("option-id"));
    public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    public void Decline() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip" || c.Parameters.GetValueOrDefault("branch") is "decline" or "repeat-decline" || c.Parameters.GetValueOrDefault("choice") == "decline");
    public void PindianCard(int? rank = null)
    {
        Answer(c => c.Parameters.GetValueOrDefault("action") is "pindian-source-card" or "pindian-card" && c.Cards.Count == 1 &&
            (rank is null || V().Hand.Any(h => h.Id == c.Cards[0] && h.Rank == rank)));
    }
    public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
    {
        for (var n = 0; n < 300; n++)
        {
            if (done()) return; Need(G.State.Winner == Winner.None, "Native terminal cutoff before expected boundary " + line);
            var p = P;
            if (p is null || p.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
            else if (p.Kind == DecisionKind.PlayCard) End();
            else if (p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => c.Parameters.ContainsKey("option-id"))) Continue();
            else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
            else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "simultaneous-pindian" && c.Parameters.GetValueOrDefault("branch") == "draw")) Answer(c => c.Parameters.GetValueOrDefault("branch") == "draw");
            else if (Action("skip") || p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") is "decline" or "repeat-decline" || c.Parameters.GetValueOrDefault("choice") == "decline")) Decline();
            else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") is "pindian-source-card" or "pindian-card")) PindianCard();
            else throw new InvalidOperationException("Unexpected actual boundary " + line + " " + Diagnostic());
        }
        throw new InvalidOperationException("Bounded actual scene did not reach boundary " + line + " " + Diagnostic());
    }
    public void Private()
    {
        var p = P!; Need(p.IsPrivate && Enumerable.Range(0, 4).Where(v => v != p.PlayerSeat).All(v => G.CreateSnapshot(v).PendingDecision is null) &&
            Enumerable.Range(0, 4).All(v => G.CreateSnapshot(v).Players.Where(s => s.Seat != v).All(s => s.Hand.Count == 0)), "Only the actual chooser sees its prompt; all other Hands stay opaque.");
        Frozen(p.Choices); foreach (var choice in p.Choices) { Frozen(choice.Cards); Frozen(choice.Targets); }
    }
    public void Frozen<T>(IReadOnlyList<T> values)
    {
        if (values is IList list && list.Count != 0) { var before = JsonSerializer.Serialize(values); var rejected = false; try { list[0] = list[0]; } catch (NotSupportedException) { rejected = true; } Need(rejected && before == JsonSerializer.Serialize(values), "Prepared nested collections are immutable."); }
    }
    public string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(v => SnapshotJson.Serialize(G.CreateSnapshot(v))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
        Zones = G.CreateCardZoneDiagnostics(), G.CardMovements, Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands) });
    public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold replay retains all safe views, exact owning frames, private drafts, RNG, materials, movements, events and accepted commands."); }
    public void RejectPrompt()
    {
        var p = P!; Reject(new AnswerPromptCommand(1, p.PromptId, p.Choices[0].Id, G.Revision));
        Reject(new AnswerPromptCommand(0, p.PromptId, new("forged-wu-lu-kang-choice"), G.Revision));
        Reject(new AnswerPromptCommand(0, p.PromptId, p.Choices[0].Id, G.Revision - 1));
    }
    public void Reject(GameCommand command) { var before = State(); Need(!G.Submit(command).Accepted && before == State(), "Invalid actor, stale or unpublished input changes no state, history, stock or RNG."); }
    public void Accept(GameCommand command)
    { try { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Code + "/" + result.Error?.Message); } catch (Exception e) { throw new InvalidOperationException(spec.Name + ": " + e.Message + " " + Diagnostic(), e); } }
    public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Diagnostic()); }
    private string Diagnostic() => JsonSerializer.Serialize(new { spec.Name, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner, Prompt = P,
        Players = Enumerable.Range(0, 4).Select(i => new { i, V(i).Hp, V(i).IsAlive, V(i).HandCount, V(i).Equipment, V(i).Judgment }),
        StockTopFirst = Stock().Select(c => new { c.CardId, Kind = c.CardKind, c.ZoneIndex,
            PrintedRank = c.CardId > 0 && c.CardId <= spec.Stock.Length ? (int?)spec.Stock[c.CardId - 1].Rank : null }),
        Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = G.Events.Where(e => e.Payload.GetType().Name.Contains("WuLuKang", StringComparison.Ordinal)).TakeLast(24).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Moves = G.CardMovements.TakeLast(14) });
    public static SkillProgramCatalog Bundle(string bundle)
    {
        string Read(string suffix) { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + bundle + "." + suffix + ".json") ?? throw new InvalidOperationException("Missing complete bundle " + bundle); using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
        return SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
    }
    public static WuLuKangScene Start(WuLuKangSpec spec, bool stopBeforePlay = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(spec));
        var setup = new ShanheBattleSetup(7, 1, "wlk-" + spec.Name, Owner, spec.OwnerHp, 0, 0, 0, 0, [], spec.Hand, spec.Equipment,
            [new(EnemyA, Hp: 12) { StartingHand = spec.Peers[1] }, new(EnemyB, Hp: 12) { StartingHand = spec.Peers[2] }], [new(Ally, Hp: 12) { StartingHand = spec.Peers[0] }], new string('a', 64));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry);
        var scene = new WuLuKangScene(spec, g, registry); scene.Accept(new StartGameCommand()); if (!stopBeforePlay) scene.Play(); return scene;
    }
    private sealed class Fixture(WuLuKangSpec spec) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wu-lu-kang-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            void Add(SkillProgramCatalog catalog, params string[] ids)
            { foreach (var id in ids) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] }); }
            var real = Bundle("wumiao-lu-kang"); Add(real, Shenduan, Kegou, Dixian); Add(Bundle("ol-wang-lang"), Gushe);
            if (spec.Zenrun) Add(Bundle("ol-sun-hong"), "ol:zenrun");
            if (spec.Tianbian) Add(Bundle("classic-qin-mi"), "classic:tianbian");
            var kinds = spec.Stock.Concat(spec.Hand).Concat(spec.Equipment).Concat(spec.Peers.SelectMany(x => x)).Select(c => c.Kind)
                .Concat(new[] { CardKind.Slash, CardKind.Dodge, CardKind.Peach, CardKind.Alcohol, CardKind.Indulgence, CardKind.IronChain, CardKind.SilverLion, CardKind.StoneAxe }).Distinct();
            foreach (var kind in kinds) { var card = CardCatalog.Get(kind); b.AddCard(new("fixture:wlk-card-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            static JsonNode N(string text) => JsonNode.Parse(text)!;
            static string Pause() => "{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"held\",\"options\":[{\"id\":\"continue\"}]}";
            static JsonNode A(string id, string effects, int targets = 0, int cards = 0) => N("{\"id\":\"" + id + "\",\"minCards\":" + cards + ",\"maxCards\":" + cards + ",\"minTargets\":" + targets + ",\"maxTargets\":" + targets + ",\"targetKind\":\"anyLiving\",\"usesPerTurn\":null,\"effects\":[" + effects + "]}");
            var acts = new JsonArray(A("pair", "{\"op\":\"startPindian\",\"target\":\"owner\",\"opponentRef\":{\"kind\":\"selectedTarget\"},\"resultBind\":\"contest\",\"visibility\":\"public\"}", 1),
                A("lose-one", "{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":1}"), A("recover-one", "{\"op\":\"recover\",\"target\":\"owner\",\"amount\":1}"),
                A("draw20", "{\"op\":\"draw\",\"target\":\"owner\",\"amount\":20}"), A("discard", "{\"op\":\"discardSelected\",\"target\":\"owner\",\"amount\":1}", cards: 1),
                A("damage", "{\"op\":\"damage\",\"target\":\"selectedTarget\",\"amount\":1}", 1));
            var selectedPair = (JsonObject)A("legacy-pair", "{\"op\":\"pindian\",\"target\":\"selectedTarget\",\"amount\":1}", 1, 1);
            selectedPair["targetKind"] = "otherLivingWithHand"; selectedPair["sourceZones"] = N("[\"hand\"]"); acts.Add(selectedPair);
            var defs = new JsonArray(new JsonObject { ["id"] = Driver, ["revision"] = 1, ["activations"] = acts, ["modifiers"] = N("[{\"id\":\"keep\",\"query\":\"handLimit\",\"operation\":\"add\",\"value\":20,\"priority\":0}]") },
                N("{\"id\":\"" + Quiet + "\",\"revision\":1,\"triggers\":[{\"id\":\"quiet\",\"window\":\"afterNormalDraw\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"skipTurnPhases\",\"target\":\"owner\",\"phases\":[\"play\"]}]}]}"));
            var soleDuelTarget = spec.ForeignDuel && spec.Peers[1].Any(c => c.Kind == CardKind.Duel);
            if (soleDuelTarget) defs.Add(N("{\"id\":\"fixture:wlk-duel-shield\",\"revision\":1,\"cardPolicies\":[{\"id\":\"native-only-human-Duel-target\",\"kind\":\"prohibitTarget\",\"cardKinds\":[\"duel\"]}]}"));
            var watches = new JsonArray();
            if (spec.WatchReasons.Length != 0)
                foreach (var zone in new[] { "hand", "equipment", "judgment" })
                    watches.Add(new JsonObject { ["id"] = "paid-movement-" + zone, ["window"] = "cardsMoved", ["subject"] = "owner", ["sourceZones"] = JsonSerializer.SerializeToNode(new[] { zone }), ["movementReasons"] = JsonSerializer.SerializeToNode(spec.WatchReasons),
                        ["movementOccurrence"] = "perBatch", ["usageScope"] = "game", ["usageLimit"] = 1, ["namedUsageGroup"] = "paid-movement", ["optional"] = false, ["priority"] = 1000, ["effects"] = N("[" + Pause() + (spec.ChildLoss > 0 ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + spec.ChildLoss + "}" : "") + "]") });
            if (spec.WatchDraw) watches.Add(N("{\"id\":\"actual-draw\",\"window\":\"cardsGained\",\"subject\":\"owner\",\"destinationZones\":[\"hand\"],\"movementReasons\":[\"skill-program.wumiao:dixian.draw\"],\"movementOccurrence\":\"perBatch\",\"priority\":1000,\"optional\":false,\"effects\":[" + Pause() + (spec.DrawChildLoss > 0 ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + spec.DrawChildLoss + "}" : "") + "]}"));
            if (spec.WatchGainReason is { } gainReason) watches.Add(new JsonObject { ["id"] = "paid-gain", ["window"] = "cardsGained", ["subject"] = "owner", ["destinationZones"] = N("[\"hand\"]"), ["movementReasons"] = JsonSerializer.SerializeToNode(new[] { gainReason }),
                ["movementOccurrence"] = "perBatch", ["usageScope"] = "game", ["usageLimit"] = 1, ["optional"] = false, ["priority"] = 1000, ["effects"] = N("[" + Pause() + (spec.ChildLoss > 0 ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + spec.ChildLoss + "}" : "") + "]") });
            if (spec.PauseUse) watches.Add(N("{\"id\":\"actual-use\",\"window\":\"cardUseCompleted\",\"ownerRelation\":\"actor\",\"singleActionInstance\":true,\"priority\":1000,\"optional\":false,\"effects\":[" + Pause() + "]}"));
            if (watches.Count > 0) defs.Add(new JsonObject { ["id"] = Watch, ["revision"] = 1, ["triggers"] = watches });
            if (spec.ConditionalSources) defs.Add(new JsonObject { ["id"] = Conditional, ["revision"] = 1, ["hpSkillGrants"] = new JsonArray(new[] { Shenduan, Kegou, Dixian }.Where(id => id == Shenduan && spec.Shenduan || id == Kegou && spec.Kegou || id == Dixian && spec.Dixian)
                .Select(id => (JsonNode)new JsonObject { ["id"] = "healthy-" + id, ["skillId"] = id, ["minimumHp"] = 4 }).ToArray()) });
            if (spec.InsertPlay) defs.Add(N("{\"id\":\"fixture:wlk-insert\",\"revision\":1,\"triggers\":[{\"id\":\"insert\",\"window\":\"turnStartBeforeNormalFlow\",\"subject\":\"owner\",\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"effects\":[{\"op\":\"insertPhase\",\"target\":\"owner\",\"phase\":\"play\",\"phaseContinuation\":\"beforeNormalPreparation\"}]}]}"));
            if (spec.Rescue) defs.Add(N("{\"id\":\"fixture:wlk-rescue\",\"revision\":1,\"triggers\":[{\"id\":\"recover\",\"window\":\"selfDyingResponse\",\"subject\":\"owner\",\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"effects\":[{\"op\":\"recoverTo\",\"target\":\"owner\",\"numberExpression\":\"integerConstant\",\"minimumValue\":2,\"clampToMaxHp\":true}]}]}"));
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "武陆抗真实原生边界检查", optionLabels = n!["id"]!.GetValue<string>() == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() });
            var local = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in local.Programs) b.AddSkill(new(id, labels[id].name, labels[id].description) { Program = program, ProgramPresentation = local.Presentations[id] });
            string[] Skills(int seat)
            {
                var skills = new List<string>(); if (watches.Count > 0) skills.Add(Watch);
                if (seat == 0)
                {
                    skills.Add(Driver); skills.Add(Gushe); if (spec.ConditionalSources) skills.Add(Conditional);
                    else { if (spec.Shenduan) skills.Add(Shenduan); if (spec.Kegou) skills.Add(Kegou); if (spec.Dixian) skills.Add(Dixian); }
                    if (spec.Tianbian) skills.Add("classic:tianbian"); if (spec.InsertPlay) skills.Add("fixture:wlk-insert"); if (spec.Zenrun) skills.Add("ol:zenrun"); if (spec.Rescue) skills.Add("fixture:wlk-rescue");
                }
                else { if (spec.OtherShenduanSeats.Contains(seat)) skills.Add(Shenduan); if (!(spec.ForeignDuel && seat == 2)) skills.Add(Quiet);
                    if (soleDuelTarget && seat is 1 or 3) skills.Add("fixture:wlk-duel-shield"); }
                return skills.ToArray();
            }
            b.AddGeneral(new(Owner, "真实武陆抗检查人", "", "standard:none", "wu", 4, Skills(0)));
            b.AddGeneral(new(Ally, "真实同行", "", "standard:none", "wei", 12, Skills(1))); b.AddGeneral(new(EnemyA, "真实对手甲", "", "standard:none", "wei", 12, Skills(2))); b.AddGeneral(new(EnemyB, "真实对手乙", "", "standard:none", "wei", 12, Skills(3)));
            b.AddDeck(new(Deck, "固定点数实体", 0, 0, []) { PhysicalCards = spec.Stock.Select(c => new ContentDeckPhysicalCard("fixture:wlk-card-" + c.Kind, c.Suit, c.Rank)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "武陆抗原生小局", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Owner, Ally, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}


internal sealed record WuLuKangSpec(string Name)
{
    public bool Shenduan { get; init; }
    public bool Kegou { get; init; }
    public bool Dixian { get; init; }
    public bool ConditionalSources { get; init; }
    public bool InsertPlay { get; init; }
    public bool ForeignDuel { get; init; }
    public bool Zenrun { get; init; }
    public bool Tianbian { get; init; }
    public int[] OtherShenduanSeats { get; init; } = [];
    public int OwnerHp { get; init; } = 4;
    public ShanheStartingCard[] Hand { get; init; } = [WuLuKangScene.C(CardKind.Slash, 9), WuLuKangScene.C(CardKind.Dodge, 2), WuLuKangScene.C(CardKind.Dodge, 4)];
    public ShanheStartingCard[] Equipment { get; init; } = [];
    public ShanheStartingCard[][] Peers { get; init; } = [[WuLuKangScene.C(CardKind.Dodge, 3)], [WuLuKangScene.C(CardKind.Dodge, 7)], [WuLuKangScene.C(CardKind.Dodge, 9)]];
    public ShanheStartingCard[] Stock { get; init; } = Enumerable.Range(0, 4).SelectMany(_ => new[] { WuLuKangScene.C(CardKind.Dodge, 1), WuLuKangScene.C(CardKind.Slash, 3), WuLuKangScene.C(CardKind.Peach, 5), WuLuKangScene.C(CardKind.Indulgence, 13) }).ToArray();
    public string[] WatchReasons { get; init; } = [];
    public int ChildLoss { get; init; }
    public bool WatchDraw { get; init; }
    public int DrawChildLoss { get; init; }
    public string? WatchGainReason { get; init; }
    public bool PauseUse { get; init; }

    public bool Rescue { get; init; }
}

using System.Globalization;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class MobileShenJiangWeiChecks
{
    private const string Hero = "mobile:shen-jiang-wei", Driver = "fixture:mobile-jw-driver", Watch = "fixture:mobile-jw-watch", Rescue = "fixture:mobile-jw-rescue", Quiet = "fixture:mobile-jw-quiet", Mode = "identity:classic-mobile-jw-fixture";
    private static GameOptions Options(bool nestedDying = false)
    {
        var setup = new ShanheBattleSetup(7, 1, "mobile-jiang-wei", Hero, 4, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Slash, Suit.Club, 4), new(CardKind.Dodge, Suit.Diamond, 6)], [],
            [new("fixture:mobile-jw-peer-2") { StartingHand = [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 9)] }, new("fixture:mobile-jw-peer-3") { StartingHand = [new(CardKind.Slash, Suit.Spade, 8)] }],
            [new("fixture:mobile-jw-peer-1") { StartingHand = [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 9)] }], new string('f', 64)) { DefaultGodFactionId = "shu" };
        if (nestedDying) setup = setup with
        {
            StartingCards = [.. setup.StartingCards, new(CardKind.Peach, Suit.Heart, 3)],
            AllyRoster = setup.AllyRoster.Select(p => p with { Hp = 2 }).ToArray()
        };
        return new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 };
    }
    private static (GameEngine, ContentRegistry) Start(bool nestedDying = false)
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var g = GameEngine.CreateStandard(Options(nestedDying), r);
        Accept(g, new StartGameCommand()); return (g, r);
    }
    private static PlayerSnapshot V(GameEngine g, int seat = 0) => g.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static T[] E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static ProgramSkillFrame? Root(GameEngine g) => g.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.MobileJiangWei is not null);
    private static CardLocation Zone(GameEngine g, int id) => g.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
    private static string? Branch(PromptChoice c) => c.Parameters.GetValueOrDefault("branch");
    private static int Value(PromptChoice c) => int.TryParse(c.Parameters.GetValueOrDefault("value"), CultureInfo.InvariantCulture, out var n) ? n : -1;
    private static void Need(bool valid, string why) { if (!valid) throw new InvalidOperationException(why); }
    private static void Accept(GameEngine g, GameCommand c) { var r = g.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Jiang Wei real command."); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> pick)
    { var p = P(g)!; var c = p.Choices.FirstOrDefault(pick); Need(c is not null, "Missing Jiang Wei published choice: " + JsonSerializer.Serialize(p)); Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, g.Revision)); }
    private static void Use(GameEngine g, string id, int[]? targets = null) => Accept(g, new UseProgramSkillCommand(0, Driver, id, [], targets ?? [], g.Revision, P(g)!.PromptId));
    private static void Play(GameEngine g) => Reach(g, p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Menu(GameEngine g, MobileJiangWeiStage stage) => Reach(g, p => Root(g)?.MobileJiangWei?.Stage == stage && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "mobile-jiang-wei"));
    private static void Offer(GameEngine g, string skill) => Reach(g, p => p.SkillPrompt?.SkillId == skill && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
    private static void Give(GameEngine g, int card, int target)
    { Use(g, "give", [target]); Reach(g, p => p.Choices.Any(c => c.Cards.SequenceEqual([card]))); Answer(g, c => c.Cards.SequenceEqual([card])); Play(g); }
    private static void Reach(GameEngine g, Func<PendingDecision, bool> ready)
    {
        for (var n = 0; n < 260; n++)
        {
            if (P(g) is { } p)
            {
                if (ready(p)) return;
                if (p.Kind == DecisionKind.SelectFaction) { Answer(g, _ => true); continue; }
                if (p.PlayerSeat != 0) { Accept(g, new AdvanceOneStepCommand(g.Revision)); continue; }
                throw new InvalidOperationException("Unexpected mobile Jiang Wei boundary: " + JsonSerializer.Serialize(new { Prompt = p, Root = Root(g)?.MobileJiangWei }));
            }
            Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Mobile Jiang Wei missed its bounded native boundary.");
    }
    private static GameEngine Cold(GameEngine g, ContentRegistry r)
    { var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), r); Need(State(g) == State(restored), "Cold restore preserves the original private receipt, native parent, paid movements and all four views."); return restored; }
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))).ToArray(), Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Facts = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), g.CardMovements, Zones = g.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(g.AcceptedCommands) });
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-jw", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(new OwnerBuilder(b), Hero);
            var json = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"draw","targetKind":"anyLiving","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":12}]},
                {"id":"hurt","targetKind":"anyLiving","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                {"id":"give","targetKind":"otherLiving","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"selectedTargetHand","awaitMovementTriggers":true}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"exchange","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["mobile.xinghun.exchange"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{Rescue}}","revision":1,"triggers":[{"id":"first-rescue","window":"dyingEntering","subject":"owner","optional":false,"priority":-100,"usageScope":"game","usageLimit":1,"effects":[{"op":"recover","target":"owner","amount":1}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
            """;
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实姜维驱动", description = "原生摸牌、给牌与体力流失。" }, [Watch] = new { name = "真实交换子窗", description = "暂停实际等量交换支付。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }, [Rescue] = new { name = "一次真实救援", description = "第一次神霈放弃后进行实际回复。" }, [Quiet] = new { name = "安静角色", description = "跳过其他角色出牌。" } };
            var catalog = SkillProgramCatalog.Load(json, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "Native mobile Jiang Wei fixture") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            for (var n = 1; n < 4; n++) b.AddGeneral(new($"fixture:mobile-jw-peer-{n}", "真实目标", "supporter", Quiet, "wei", 12));
            b.AddDeck(new("fixture:mobile-jw-deck", "小型固定实体牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard(i % 2 == 0 ? "standard:slash" : "standard:dodge", (Suit)(i % 4), i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动神姜维真实结算", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, "fixture:mobile-jw-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Hero, "fixture:mobile-jw-peer-1", "fixture:mobile-jw-peer-2", "fixture:mobile-jw-peer-3"]));
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "固定配牌姜维真实结算", 4, 4, new Dictionary<string, int>(), "fixture:mobile-jw-deck", GeneralPoolIds: [Hero, "fixture:mobile-jw-peer-1", "fixture:mobile-jw-peer-2", "fixture:mobile-jw-peer-3"], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    private sealed class OwnerBuilder(IContentRegistryBuilder b) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition d) => b.AddGeneral(d.Id == Hero ? d with { AdditionalSkillIds = [.. d.AdditionalSkillIds!, Driver, Watch, Rescue] } : d);
        public void AddSkill(ContentSkillDefinition d) => b.AddSkill(d);
        public void AddCard(ContentCardDefinition d) => b.AddCard(d);
        public void AddDeck(ContentDeckRecipe d) => b.AddDeck(d);
        public void AddMode(ContentModeDefinition d) => b.AddMode(d);
    }
}

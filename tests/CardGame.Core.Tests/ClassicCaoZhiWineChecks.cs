using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ClassicCaoZhiWineChecks
{
    private const string Skill = "classic:jiushi", Watch = "fixture:classic-cao-zhi-face-child",
        Owner = "fixture:classic-cao-zhi-owner", Mode = "identity:classic-cao-zhi-wine", Deck = "fixture:classic-cao-zhi-deck";

    public static void ActiveJiushiPaysFaceChildThenUsesNativeZeroMaterialWineCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
        Accept(game, new StartGameCommand()); Play(game);
        var hand = View(game).Hand.Select(c => c.Id).ToArray();
        Need(game.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill),
            "The complete classic Jiushi skill reused by mobile Cao Zhi offers its real Play activation while face up.");
        Accept(game, new UseProgramSkillCommand(0, Skill, "virtual-alcohol", [], [], game.Revision, Prompt(game)!.PromptId));
        Reach(game, () => Prompt(game)?.SkillPrompt?.SkillId == Watch);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        Need(root.ProvenanceAlcohol is { Stage: ProgramProvenanceAlcoholStage.FaceCost, ChildFrameId: null } &&
            View(game).IsFaceDown && !Facts<AlcoholAppliedEvent>(game).Any(),
            "The real face-down cost belongs to the original frame and its native turnover child returns before Wine is issued.");
        var state = State(game);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == state, "The paid face cost and unissued zero-material Wine cold-restore on their original frame.");
        var p = Prompt(game)!;
        Accept(game, new AnswerPromptCommand(p.PlayerSeat, p.PromptId,
            p.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Play(game);
        Need(View(game).IsFaceDown && View(game).Hand.Select(c => c.Id).SequenceEqual(hand) &&
            Facts<CardUseDeclaredEvent>(game).Where(e => e.CardKind == CardKind.Alcohol).ToArray() is [{ CardId: 0 }] &&
            Facts<AlcoholAppliedEvent>(game).Count() == 1 && !game.CardMovements.Any(m => m.Reason == CardMoveReasons.Use) &&
            !game.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill),
            "The original paid face child completes one native zero-entity Wine, keeps all real hand entities and offers no second unpaid face-down activation.");
        state = State(game);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == state, "The completed Wine and exact original events replay once after the paid child return.");
    }

    private static string Peer(int seat) => "fixture:classic-cao-zhi-peer-" + seat;
    private static PlayerSnapshot View(GameEngine g) => g.CreateSnapshot(0).Players.Single(p => p.Seat == 0);
    private static IEnumerable<T> Facts<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? Prompt(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected classic Cao Zhi native Wine command."); }
    private static void Play(GameEngine game) => Reach(game, () => Prompt(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Reach(GameEngine game, Func<bool> done)
    {
        for (var i = 0; i < 150 && !done(); i++)
        {
            if (Prompt(game) is { Kind: DecisionKind.SelectGeneral } p)
            { Accept(game, new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), game.Revision, p.PromptId)); continue; }
            Need(Prompt(game) is null, "Unexpected native Cao Zhi boundary: " + JsonSerializer.Serialize(Prompt(game)));
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        Need(done(), "Missing bounded native Cao Zhi Wine boundary.");
    }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new {
        Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(game.CreateSnapshot(i))).ToArray(),
        Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
        Events = game.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), game.CardMovements });

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-cao-zhi-wine", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "classic:cao-zhi", "classic:cao-zhi");
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
              {"id":"native-paid-face-child","window":"characterTurnedOver","subject":"owner","optional":false,
               "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-face","options":[{"id":"continue"}]}]}
            ]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "真实翻面子窗", description = "翻面付款后暂停原生子结算。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            b.AddSkill(new(Watch, "真实翻面子窗", "翻面付款后暂停原生子结算。") { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            b.AddGeneral(new(Owner, "完整酒诗持有者", "supporter", Skill, "wei", 4, [Watch]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "实际同桌", "supporter", "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定真实闪牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "经典及移动曹植酒诗原生入口", 4, 4, new Dictionary<string, int> {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}

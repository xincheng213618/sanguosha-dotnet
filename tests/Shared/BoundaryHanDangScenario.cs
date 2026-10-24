using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryHanDangScenario
{
    internal const string Gongqi = "boundary:gongqi-mobile", Jiefan = "boundary:jiefan-mobile", Driver = "fixture:boundary-hd-mobile-driver", Watch = "fixture:boundary-hd-mobile-watch";
    internal const string CostReason = "skill-program.boundary:gongqi-mobile.DiscardSelected", AidReason = "skill-program.boundary:jiefan-mobile.RequestAttackRangeAid";
    private const string Hero = "boundary:han-dang-mobile", Owner = "fixture:boundary-hd-mobile-owner", Quiet = "fixture:boundary-hd-mobile-quiet", Mode = "identity:classic-boundary-hd-mobile", Deck = "fixture:boundary-hd-mobile-deck";

    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool singleHuman = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = singleHuman ? null : [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; step < 120; step++)
        {
            var prompt = Decision(game); if (prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return (registry, game);
            if (prompt is { Kind: DecisionKind.SelectGeneral }) Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? Owner : prompt.ValidContentIds.First(id => id != Owner), game.Revision, prompt.PromptId));
            else if (prompt is not null) throw new InvalidOperationException("Unexpected Han Dang setup decision " + prompt.Kind);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Han Dang public identity setup did not reach its first Play.");
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    internal static PlayerSnapshot Player(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat];
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Han Dang public command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static void Continue(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 200; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Han Dang scene decision " + prompt.Kind + " seat " + prompt.PlayerSeat + " skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Han Dang scene did not reach its next boundary.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void Use(GameEngine game, string skill, string activation, int[]? cards = null, int[]? targets = null) =>
        Accept(game, new UseProgramSkillCommand(0, skill, activation, cards ?? [], targets ?? [], game.Revision, Decision(game)!.PromptId));
    internal static int EnsureCard(GameEngine game, CardKind kind)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            if (Player(game, 0).Hand.FirstOrDefault(card => card.Kind == kind) is { } card) return card.Id;
            Use(game, Driver, "refill"); Play(game);
        }
        throw new InvalidOperationException("The fixed native Han Dang deck did not provide its required physical input.");
    }
    internal static void Equip(GameEngine game, CardKind kind)
    {
        var id = EnsureCard(game, kind); var action = game.GetLegalActions(0).First(candidate => candidate.CardId == id);
        Accept(game, new PlayCardCommand(0, id, action.TargetSeats, game.Revision, Decision(game)!.PromptId, action.PlayedCardKind)); Play(game);
    }
    internal static int EquipPeerWeapon(GameEngine game, int peer)
    {
        var id = EnsureCard(game, CardKind.Crossbow); Use(game, Driver, "equip-peer", targets: [peer]);
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        Answer(game, choice => choice.Cards.SequenceEqual([id])); Play(game); return id;
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-hd-mobile", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            object Activation(string id, object[] effects, int targetCount = 0) => new Dictionary<string, object?> {
                ["id"] = id, ["minCards"] = 0, ["maxCards"] = 0, ["minTargets"] = targetCount, ["maxTargets"] = targetCount,
                ["targetKind"] = targetCount == 0 ? "anyLiving" : "otherLiving", ["usesPerTurn"] = null, ["effects"] = effects };
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, modifiers = new[] { new { id = "keep-real-material", query = "handLimit", operation = "add", value = 40, priority = 0 } },
                    triggers = new[] { new { id = "no-normal-draw", window = "drawPhaseStarting", subject = "owner", optional = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Driver, revision = 1, activations = new[] {
                    Activation("refill", [new { op = "draw", target = "owner", amount = 8 }]),
                    Activation("equip-peer", [new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, targetRef = new { kind = "selectedTarget" },
                        zones = new[] { "hand" }, cardCategories = new[] { "equipment" }, count = 1, destination = "selectedTargetEquipment" }], 1) } },
                new { id = Watch, revision = 1, triggers = new object[] {
                    new { id = "real-paid-discard", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand", "equipment" }, movementOccurrence = "perOwnerBatch", movementReasons = new[] { CostReason, AidReason }, optional = false,
                        effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "original-discard", options = new[] { new { id = "continue" } } } } },
                    new { id = "real-native-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { AidReason }, optional = false,
                        effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "original-native-draw", options = new[] { new { id = "continue" } } } } }
                } }
            } });
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "保留真实手牌", description = "安静原生回合" }, [Driver] = new { name = "实际装备和牌堆", description = "只给玩家的公开API驱动" },
                [Watch] = new { name = "原生付款子窗", description = "真实HE弃牌或解烦摸牌后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "移动界韩当实际来源", "supporter", Gongqi, "wu", 4, [Jiefan, Quiet, Driver, Watch]));
            var pool = new List<string> { Owner };
            for (var index = 1; index < 4; index++) { var id = "fixture:boundary-hd-mobile-peer-" + index; pool.Add(id); builder.AddGeneral(new(id, "可控制的其他角色", "supporter", Quiet, "wei", 4, [Watch])); }
            string[] physical = ["standard:slash", "standard:crossbow", "standard:offensive_horse", "standard:defensive_horse", "standard:indulgence", "standard:dodge"];
            builder.AddDeck(new(Deck, "固定真实基本锦囊坐骑武器", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 192).Select(index => new ContentDeckPhysicalCard(physical[index % physical.Length], index % 2 == 0 ? Suit.Club : Suit.Heart, index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "真实四席身份回合", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: pool));
        }
    }
}

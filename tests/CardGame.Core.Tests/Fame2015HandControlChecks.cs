using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class Fame2015HandControlChecks
{
    private const string Mode = "identity:classic-hand-control-check", RevealHold = "fixture:hand-control-public-reveal-child";

    public static void RevealColorRealCostOpaqueGainAndHpLossReplay()
    {
        CompleteTwoColorFlow("classic:huaiyi");
        var monoRegistry = Registry(["classic:huaiyi"], monochrome: true);
        var mono = Start(monoRegistry, 13); ReachPlay(mono);
        var monoHand = mono.CreateSnapshot(0, true).Players[0].Hand.ToArray();
        var monoHp = mono.CreateSnapshot(0, true).Players[0].Hp;
        Activate(mono, "classic:huaiyi", "reveal-color-take");
        Require(Pending(mono)!.Choices.Count == 1, "A monochrome actual hand must allow payment of its one existing color in the current edition.");
        Require(Enumerable.Range(0, 5).All(seat => mono.CreateSnapshot(seat, revealAll: false).PublicRevealedCards.Count == monoHand.Length),
            "A monochrome reveal must also publish every actual face to all ordinary viewers.");
        AssertReplay(mono, monoRegistry);
        AnswerAction(mono, "red"); ReachHandChoice(mono, "take-targets"); AnswerAction(mono, "finish"); Finish(mono); ReachPlay(mono);
        Require(mono.CreateSnapshot(0, true).Players[0].Hand.Count == 0 && mono.CreateSnapshot(0, true).Players[0].Hp == monoHp &&
            monoHand.All(card => mono.CardMovements.Any(move => move.CardId == card.Id && move.To == CardLocation.DiscardPile)) &&
            Enumerable.Range(0, 5).All(seat => mono.CreateSnapshot(seat, revealAll: false).PublicRevealedCards.Count == 0),
            "A zero-target choice must still pay all actual monochrome cards and must not lose HP.");
        AssertReplay(mono, monoRegistry);
    }

    public static void MobileRequiresBothColorsWithPaidCold()
    {
        const string mobile = "mobile:huaiyi";
        foreach (var withRevealChild in new[] { false, true })
        {
            var registry = Registry(withRevealChild ? [mobile, "mou-ol:jueyan", RevealHold] : [mobile], monochrome: true);
            var game = Start(registry, 13); ReachPlay(game);
            var hand = game.CreateSnapshot(0, true).Players[0].Hand.ToArray();
            var hp = game.CreateSnapshot(0, true).Players[0].Hp;
            var targets = Enumerable.Range(1, 4).Select(seat => game.CreateSnapshot(seat, true).Players[seat].Hand.Select(c => c.Id).ToArray()).ToArray();
            Activate(game, mobile, "reveal-color-take");
            if (withRevealChild)
            {
                for (var step = 0; step < 100 && Pending(game)?.SkillPrompt?.SkillId != RevealHold; step++) Advance(game);
                Require(Pending(game)?.SkillPrompt?.SkillId == RevealHold && game.ResolutionStack.OfType<PublicHandRevealWindowFrame>()
                    .Any(frame => frame.ReturnKey == "hand-color-whole-hand"),
                    "The actual whole-hand reveal must wait for its native first-suit draw child before returning to the two-color guard.");
                Require(Enumerable.Range(0, 5).All(seat => game.CreateSnapshot(seat, false).PublicRevealedCards.Count == hand.Length),
                    "The original monochrome hand remains publicly shown while the real reveal child is pending.");
                AssertReplay(game, registry);
                Answer(game, Pending(game)!.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            }
            ReachPlay(game);
            var drawn = game.CardMovements.Count(m => m.Reason.Value == "skill-program.mou-ol:jueyan.draw" && m.To == CardLocation.Hand(0));
            Require(game.Events.Count(e => e.Payload is ProgramCardsRevealedEvent reveal && reveal.SkillId == mobile &&
                    reveal.Cards.Select(c => c.Id).SequenceEqual(hand.Select(c => c.Id))) == 1 &&
                game.CreateSnapshot(0, true).Players[0].Hand.Count == hand.Length + drawn && drawn == (withRevealChild ? 1 : 0) &&
                hand.All(card => game.CreateSnapshot(0, true).Players[0].Hand.Any(c => c.Id == card.Id)) &&
                Enumerable.Range(1, 4).All(seat => game.CreateSnapshot(seat, true).Players[seat].Hand.Select(c => c.Id).SequenceEqual(targets[seat - 1])) &&
                !game.CardMovements.Any(m => m.Reason.Value is "program.hand-color.discard" or "program.hand-color.obtain") &&
                game.CreateSnapshot(0, true).Players[0].Hp == hp &&
                Enumerable.Range(0, 5).All(seat => game.CreateSnapshot(seat, false).PublicRevealedCards.Count == 0) &&
                !game.GetHumanLegalActions().Any(action => action.ProgramSkillId == mobile),
                "Mobile monochrome Huaiyi shows every original entity once and then completes, with no color payment, target gain or HP loss; its Play use stays spent after either immediate or native-child return.");
            AssertReplay(game, registry);
        }
        CompleteTwoColorFlow(mobile);
    }

    private static void CompleteTwoColorFlow(string skill)
    {
        var registry = Registry(skill == "classic:huaiyi" ? null : [skill]);
        var game = Start(registry, 11);
        ReachPlay(game);
        var hand = game.CreateSnapshot(0, true).Players[0].Hand.ToArray();
        var countBefore = hand.Length;
        var hpBefore = game.CreateSnapshot(0, true).Players[0].Hp;
        Activate(game, skill, "reveal-color-take");
        var prompt = Pending(game)!;
        Require(game.Events.Any(item => item.Payload is ProgramCardsRevealedEvent revealed && revealed.OwnerSeat == 0 && revealed.SkillId == skill &&
                revealed.Cards.Select(card => card.Id).SequenceEqual(hand.Select(card => card.Id))),
            "All physical hand cards must be publicly revealed before selecting color.");
        Require(Enumerable.Range(0, 5).All(seat =>
                JsonSerializer.Serialize(game.CreateSnapshot(seat, revealAll: false).PublicRevealedCards.OrderBy(card => card.Id)) ==
                JsonSerializer.Serialize(hand.OrderBy(card => card.Id)) &&
                (seat == 0 || game.CreateSnapshot(seat, revealAll: false).PendingDecision is null)),
            "Every ordinary viewer must see the complete actual revealed faces while only the owner sees its private color choices.");
        AssertReplay(game, registry);
        var red = hand.Count(card => card.Suit is Suit.Heart or Suit.Diamond) >= 2;
        var paid = hand.Where(card => (card.Suit is Suit.Heart or Suit.Diamond) == red).ToArray();
        Require(paid.Length >= 2, "The fixture needs a real two-card color cost.");
        AnswerAction(game, red ? "red" : "black");
        ReachHandChoice(game, "take-targets");
        Require(paid.All(card => game.CardMovements.Any(move => move.CardId == card.Id && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile)),
            "Color payment must discard every physical card of that color.");
        Require(Enumerable.Range(0, 5).All(seat => game.CreateSnapshot(seat, revealAll: false).PublicRevealedCards.Count == 0),
            "The displayed hand must leave every public reveal panel after real color payment, before taking any target card.");
        AssertReplay(game, registry);
        for (var index = 0; index < 2; index++)
        {
            var target = Pending(game)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("hand-control-action") == "take-target");
            var seat = target.Targets.Single();
            Answer(game, target);
            Require(Pending(game)!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)).All(choice => choice.Cards.Count == 0),
                "Another participant's hand must use opaque positions without card IDs.");
            AssertReplay(game, registry);
            Answer(game, Pending(game)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)));
            if (index == 0) ReachHandChoice(game, "take-targets");
            Require(game.CardMovements.Any(move => move.From == CardLocation.Hand(seat) && move.To == CardLocation.Hand(0)), "The gain must move a real target-owned card.");
        }
        Finish(game);
        if (Pending(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == "take-targets") == true) AnswerAction(game, "finish");
        Finish(game); ReachPlay(game);
        Require(game.CreateSnapshot(0, true).Players[0].Hp == hpBefore - 1 && game.CreateSnapshot(0, true).Players[0].Hand.Count == countBefore - paid.Length + 2,
            "Obtaining two cards must lose exactly one HP after real color payment and gains.");
        Require(!game.GetHumanLegalActions().Any(action => action.ProgramSkillId == skill), "The phase usage ledger must retain Huaiyi's paid activation.");
        AssertReplay(game, registry);
    }

    private static int ReadHandLimit(GameEngine game, int seat)
    {
        var players = (System.Collections.IList)typeof(GameEngine).GetField("_players", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(game)!;
        return (int)typeof(GameEngine).GetMethod("GetHandLimit", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(game, [players[seat]])!;
    }
    private static bool ReadWithinAttackRange(GameEngine game, int actor, int target) => (bool)typeof(GameEngine).GetMethod("IsWithinAttackRange",
        BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(game, [actor, target])!;

    private static ContentRegistry Registry(string[]? skills = null, bool equipmentDeck = false, bool slashOnly = false, int ownerHp = 8, bool monochrome = false,
        bool arrowOnly = false, bool factionDefense = false, bool publicCosts = false) => ContentRegistry.Build(new StandardContentPackage(),
        new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
        new StandardClassicGeneralPackage(), new Scenario(skills, equipmentDeck, slashOnly, ownerHp, monochrome, arrowOnly, factionDefense, publicCosts));
    private static GameEngine Start(ContentRegistry registry, int seed) => TryStart(registry, seed, Role.Lord) ??
        throw new InvalidOperationException("The human fixture general is unavailable.");
    private static GameEngine? TryStart(ContentRegistry registry, int seed, Role humanRole)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = seed, PlayerCount = 5, HumanSeat = 0, HumanRole = humanRole,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(game.Submit(new StartGameCommand()));
        if (!Pending(game)!.ValidContentIds.Contains("fixture:hand-control-owner")) return null;
        Accept(game.Submit(new SelectGeneralCommand(0, "fixture:hand-control-owner", game.Revision, Pending(game)!.PromptId)));
        return game;
    }
    private static void Activate(GameEngine game, string skill, string activation) => Accept(game.Submit(new UseProgramSkillCommand(0,
        skill, activation, [], [], game.Revision, Pending(game)!.PromptId)));
    private static PendingDecision? Pending(GameEngine game) => game.PendingDecision ?? Enumerable.Range(0, 5).Select(seat => game.CreateSnapshot(seat, true).PendingDecision).FirstOrDefault(item => item is not null);
    private static void Answer(GameEngine game, PromptChoice choice) => Accept(game.Submit(new AnswerPromptCommand(Pending(game)!.PlayerSeat, Pending(game)!.PromptId, choice.Id, game.Revision)));
    private static void AnswerAction(GameEngine game, string action) => Answer(game, Pending(game)!.Choices.Single(choice => choice.Parameters.GetValueOrDefault("hand-control-action") == action));
    private static void Advance(GameEngine game) => Accept(game.Submit(new AdvanceOneStepCommand(game.Revision)));
    private static void ReachPlay(GameEngine game)
    {
        for (var count = 0; count < 100 && Pending(game)?.Kind != DecisionKind.PlayCard; count++)
            if (Pending(game) is { Kind: DecisionKind.ProgramTrigger } prompt) Answer(game, prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") ?? prompt.Choices.First());
            else Advance(game);
        Require(Pending(game)?.Kind == DecisionKind.PlayCard, "Fixture did not reach Play.");
    }
    private static void ReachHandChoice(GameEngine game, string stage)
    {
        for (var count = 0; count < 100 && Pending(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == stage) != true; count++) Advance(game);
        Require(Pending(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == stage) == true, $"Missing hand-control stage {stage}.");
    }
    private static void Finish(GameEngine game)
    {
        for (var count = 0; count < 150 && game.ResolutionStack.Count > 0; count++)
            if (Pending(game) is { Kind: DecisionKind.ProgramTrigger } prompt)
                Answer(game, prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("hand-control-action") == "finish") ??
                    prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") ?? prompt.Choices.First());
            else Advance(game);
    }
    private static void AssertReplay(GameEngine game, ContentRegistry registry)
    {
        var replay = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 5).All(seat => JsonSerializer.Serialize(game.CreateSnapshot(seat, true)) == JsonSerializer.Serialize(replay.CreateSnapshot(seat, true))) &&
            Enumerable.Range(0, 5).All(seat => JsonSerializer.Serialize(game.CreateSnapshot(seat, revealAll: false)) ==
                JsonSerializer.Serialize(replay.CreateSnapshot(seat, revealAll: false))) &&
            JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(replay.ResolutionStack) &&
            JsonSerializer.Serialize(game.CardMovements) == JsonSerializer.Serialize(replay.CardMovements) &&
            game.Events.Select(EventJson).SequenceEqual(replay.Events.Select(EventJson)) &&
            Enumerable.Range(0, 5).All(seat => ReadHandLimit(game, seat) == ReadHandLimit(replay, seat)) &&
            Enumerable.Range(0, 5).All(actor => Enumerable.Range(0, 5).All(target =>
                ReadWithinAttackRange(game, actor, target) == ReadWithinAttackRange(replay, actor, target))),
            "Checkpoint command JSON must replay every ordinary private/public viewer, paused and final state, typed events, movements and effective hand-limit/range queries exactly.");
    }
    private static string EventJson(EventEnvelope item) => $"{item.Sequence}|{item.Payload.GetType().Name}|{JsonSerializer.Serialize(item.Payload, item.Payload.GetType())}";
    private static void Accept(CommandResult result) => Require(result.Accepted, result.Error?.Message ?? "Command rejected.");
    private static void Require(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private sealed class Scenario(string[]? skills, bool equipmentDeck, bool slashOnly, int ownerHp, bool monochrome, bool arrowOnly, bool factionDefense, bool publicCosts) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("hand-control-check", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var driver = SkillProgramCatalog.Load("""
                {"schemaVersion":62,"skills":[{"id":"fixture:hand-control-driver","revision":1,"minimumRulesVersion":190,"activations":[
                {"id":"damage-other","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"damage-self","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                {"id":"draw-owner","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":2}]},
                {"id":"equip-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment"}]}]}]}
                """, """{"schemaVersion":3,"skills":{"fixture:hand-control-driver":{"name":"规则测试","description":"真实伤害测试"}}}""");
            builder.AddSkill(new ContentSkillDefinition("fixture:hand-control-driver", "规则测试", "测试") { Program = driver.Programs["fixture:hand-control-driver"] });
            if (skills?.Contains(RevealHold, StringComparer.Ordinal) == true)
            {
                var hold = SkillProgramCatalog.Load($$"""
                    {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{RevealHold}}","revision":1,"triggers":[
                    {"id":"first-real-reveal-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                    "movementReasons":["skill-program.mou-ol:jueyan.draw"],"usageScope":"game","usageLimit":1,"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"held","options":[{"id":"continue"}]}]}]}]}
                    """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [RevealHold] = new { name = "实际展示子窗", description = "原生展示收益摸牌已完成，尚未返回原展示。",
                            optionLabels = new Dictionary<string, string> { ["continue"] = "继续展示返回" } } } }));
                builder.AddSkill(new(RevealHold, "实际展示子窗", "原生展示收益摸牌已完成，尚未返回原展示。")
                    { Program = hold.Programs[RevealHold], ProgramPresentation = hold.Presentations[RevealHold] });
            }
            if (factionDefense)
            {
                var defense = SkillProgramCatalog.Load("""
                    {"schemaVersion":62,"skills":[{"id":"fixture:faction-defense","revision":1,"cardPolicies":[
                    {"id":"allied-dodge","kind":"factionResponseRequest","requiredCardKinds":["dodge"],"factionId":"wu","ownerRole":"lord"}]}]}
                    """, """{"schemaVersion":3,"skills":{"fixture:faction-defense":{"name":"阵营响应","description":"验证虚拟闪实际提供给主公"}}}""");
                builder.AddSkill(new ContentSkillDefinition("fixture:faction-defense", "阵营响应", "验证") { Program = defense.Programs["fixture:faction-defense"], Tags = SkillTag.Lord,
                    SelectionWeights = new Dictionary<Role, double> { [Role.Lord] = 100 } });
            }
            builder.AddGeneral(new ContentGeneralDefinition("fixture:hand-control-owner", "规则测试角色", "supporter", "fixture:hand-control-driver", "wu", BaseHp: ownerHp,
                AdditionalSkillIds: skills ?? ["classic:huaiyi", "classic:xingxue", "classic:yaoming", "classic:yanzhu"]));
            var targets = Enumerable.Range(1, 4).Select(index => $"fixture:hand-control-target-{index}").ToArray();
            foreach (var target in targets) builder.AddGeneral(new ContentGeneralDefinition(target, "规则目标", "supporter", factionDefense ? "fixture:faction-defense" : "standard:none", "wu", BaseHp: 8));
            builder.AddDeck(new ContentDeckRecipe("fixture:hand-control-deck", "控制测试", 6, 2, []) {
                PhysicalCards = Enumerable.Range(0, 300).Select(index => new ContentDeckPhysicalCard(equipmentDeck || publicCosts && index % 6 == 0 ? "standard:crossbow" : publicCosts && index % 6 == 1 ? "standard:lightning" : arrowOnly ? "standard:arrow_barrage" : !slashOnly && index % 3 == 0 ? "standard:dodge" : "standard:slash", monochrome ? Suit.Heart : (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new ContentModeDefinition(Mode, "控制测试", 5, 5, new Dictionary<string, int> {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                "fixture:hand-control-deck", GeneralCandidateCount: 5, GeneralPoolIds: ["fixture:hand-control-owner", .. targets]));
        }
    }
}

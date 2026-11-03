using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ProgramAmountExpressionChecks
{
    private const string Daili = "ol:daili", DailiDriver = "fixture:daili-driver", DailiWatch = "fixture:daili-watch", DailiOrdinary = "fixture:daili-ordinary";
    private const string DailiOwner = "fixture:daili-owner", DailiPeer = "fixture:daili-peer";
    private const string DailiDrawReason = "skill-program." + Daili + ".Draw";

    private static void DailiUsesSharedHistoryAndOriginalDrawBindingsCold()
    {
        foreach (var scenario in new[] { "zero", "odd", "two", "reentered", "changed", "empty", "death-flip", "death-draw", "unpermitted" })
        {
            var (game, registry) = CreateDaili(scenario);
            var activeSkill = scenario == "unpermitted" ? DailiOrdinary : Daili;
            var drawReason = "skill-program." + activeSkill + ".Draw";
            var original = View(game, 0).Hand.Select(card => card.Id).ToArray();
            var shownCount = scenario == "two" ? 2 : scenario is "odd" or "reentered" ? 1 : 0;
            foreach (var id in original.Take(shownCount)) ShowDailiCard(game, id);
            if (scenario == "reentered")
            {
                Use(game, "give", [1], DailiDriver, [original[0]]); Play(game);
                Use(game, "take", [1], DailiDriver);
                ReachAccepted(game, () => game.PendingDecision?.Choices.Any(choice => choice.Parameters.ContainsKey("slot-index")) == true);
                var prompt = game.PendingDecision!;
                Need(prompt.Choices.All(choice => choice.Cards.Count == 0), "The real foreign Hand return retains opaque slot selection.");
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
                    prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("slot-index") == "0").Id, game.Revision));
                Play(game);
                var show = Events<PublicCardRevealHistoryRecordedEvent>(game).SelectMany(fact => fact.Facts).Single(fact => fact.CardId == original[0]);
                Need(View(game, 0).Hand.Any(card => card.Id == original[0]) &&
                    game.CardMovements.Last(move => move.CardId == original[0] && move.To == CardLocation.Hand(0)).Sequence != show.HandEntryMovementSequence,
                    "The displayed entity really left and reentered its owner's Hand with a new physical entry before qualification.");
            }
            var endedTurn = game.State.TurnNumber;
            Accept(game, new EndPlayPhaseCommand(0, game.Revision, game.PendingDecision!.PromptId));
            if (scenario == "odd")
            {
                ReachAccepted(game, () => Events<TurnStartedEvent>(game).Any(fact => fact.TurnNumber > endedTurn));
                Need(!Events<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == Daili) &&
                    !Events<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == DailiDrawReason) && !View(game, 0).IsFaceDown,
                    "One currently displayed Hand residency is odd: the actual completed turn offers no flip or Draw3.");
                continue;
            }
            ReachAccepted(game, () => game.PendingDecision?.SkillPrompt?.SkillId == activeSkill &&
                game.PendingDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            var offer = game.PendingDecision!;
            Need(Events<TurnEndedEvent>(game).Any(fact => fact.TurnNumber == endedTurn && fact.ActorSeat == 0) &&
                !Events<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == activeSkill),
                "Zero and even current residencies qualify only at the real AfterTurnEnded offer, before any effect is issued.");
            game = Cold(game, registry);
            offer = game.PendingDecision!;
            Accept(game, new AnswerPromptCommand(0, offer.PromptId, offer.Choices.Single(choice =>
                choice.Parameters.GetValueOrDefault("program-action") == "activate").Id, game.Revision));
            ReachAccepted(game, () => DailiOption(game, "flip"));
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == activeSkill);
            var frameId = frame.Id;
            Need(View(game, 0).IsFaceDown && frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } &&
                (scenario == "unpermitted" ? frame.InvocationAuthorization is null : frame.InvocationAuthorization is { RetainOwnerDeath: false }) &&
                Events<CharacterStateChangedEvent>(game).Count(fact => fact.Change.ParentFrameId == frameId &&
                    fact.Change.Window == SkillProgramTriggerWindow.CharacterTurnedOver) == 1 &&
                !Events<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == drawReason),
                "The accepted original frame flips once and waits for its genuine character-state child before requesting Draw3.");
            game = Cold(game, registry); AnswerAccepted(game, "flip");
            if (scenario == "death-flip")
            {
                ReachAccepted(game, () => Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId));
                AssertDailiDeath(game, frameId, expectedDraws: 0);
                continue;
            }
            if (scenario is "changed" or "empty" or "unpermitted")
            {
                ReachAccepted(game, () => DailiOption(game, "flip-return"));
                Need(View(game, 0).IsAlive && View(game, 0).Skills?.Any(skill => skill.Id == activeSkill) != true,
                    scenario + ": the real flip observer removes the original source before any Draw3 request.");
                game = Cold(game, registry); AnswerAccepted(game, "flip-return");
                if (scenario == "unpermitted")
                {
                    ReachAccepted(game, () => Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId));
                    Need(!Events<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == frameId).Completed &&
                        !Events<NativeDrawAttemptStartedEvent>(game).Any(fact => fact.Context.Reason.Value == drawReason) &&
                        !Events<ProgramCardsRevealedEvent>(game).Any(fact => fact.FrameId == frameId),
                        "An ordinary AfterTurnEnded binding without declared invocation permission cannot start its unpaid Draw after source loss.");
                    continue;
                }
            }
            ReachAccepted(game, () => DailiOption(game, "draw"));
            var paid = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frameId);
            var drawn = paid.CardSetBindings.Single(binding => binding.Name == "daili-drawn");
            var ids = drawn.CardIds.ToArray();
            Need(ids.Length == 3 && drawn.SourceLocations.All(location => location == CardLocation.Hand(0)) &&
                drawn.SourceEntrySequences is { Count: 3 } entries && entries.All(entry => entry is not null) &&
                ids.Select((id, index) => game.CardMovements.Any(move => move.CardId == id && move.Sequence == entries[index] &&
                    move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason.Value == DailiDrawReason)).All(value => value) &&
                !Events<ProgramCardsRevealedEvent>(game).Any(fact => fact.FrameId == frameId),
                "The original native Draw3 binds its exact three physical entry sequences before paid children can change the Hand.");
            game = Cold(game, registry); AnswerAccepted(game, "draw");
            if (scenario == "death-draw")
            {
                ReachAccepted(game, () => Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId));
                AssertDailiDeath(game, frameId, expectedDraws: 1);
                continue;
            }
            if (scenario == "changed")
            {
                ReachAccepted(game, () => game.PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
                AnswerAcceptedCard(game, ids[0]);
                ReachAccepted(game, () => game.PendingDecision?.Choices.Any(choice => choice.Cards.SequenceEqual([ids[1]])) == true);
                AnswerAcceptedCard(game, ids[1]);
            }
            else if (scenario == "empty")
            {
                foreach (var id in ids)
                {
                    ReachAccepted(game, () => game.PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
                    AnswerAcceptedCard(game, id);
                }
            }
            ReachAccepted(game, () => DailiOption(game, "draw-return"));
            if (scenario is "changed" or "empty")
            {
                var extra = game.CardMovements.Where(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                    move.Reason.Value == "skill-program." + DailiWatch + ".Draw" && !ids.Contains(move.CardId)).ToArray();
                Need(extra.Length == 1 && View(game, 0).Skills?.Any(skill => skill.Id == Daili) != true &&
                    (scenario == "empty" ? ids.All(id => !View(game, 0).Hand.Any(card => card.Id == id)) :
                        View(game, 0).Hand.Any(card => card.Id == ids[0]) && !View(game, 0).Hand.Any(card => card.Id == ids[1]) &&
                        game.CardMovements.Last(move => move.CardId == ids[0] && move.To == CardLocation.Hand(0)).Sequence != drawn.SourceEntrySequences![0]),
                    scenario + ": actual draw children alter the original residencies and draw one unrelated new entity after source loss.");
                game = Cold(game, registry);
            }
            AnswerAccepted(game, "draw-return");
            if (scenario == "empty")
            {
                ReachAccepted(game, () => Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId));
                Need(Events<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == frameId).Completed &&
                    Events<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == frameId) is { RequestedCount: 3, ActualDrawCount: 3 } &&
                    !Events<ProgramCardsRevealedEvent>(game).Any(fact => fact.FrameId == frameId) &&
                    !Events<PublicHandRevealWindowStartedEvent>(game).Any(fact => fact.ParentFrameId == frameId),
                    "When every original Draw3 entity leaves, the empty observed binding emits no empty public-show event and does not expose a later child draw.");
                continue;
            }
            ReachAccepted(game, () => DailiOption(game, "reward"));
            var reveal = Events<ProgramCardsRevealedEvent>(game).Single(fact => fact.FrameId == frameId);
            var expectedShown = scenario == "changed" ? ids.Skip(2).ToArray() : ids;
            Need(reveal.Bind == "daili-shown" && reveal.Cards.Select(card => card.Id).SequenceEqual(expectedShown) &&
                game.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Any(window => window.ParentFrameId == frameId) &&
                Events<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == frameId) is { RequestedCount: 3, ActualDrawCount: 3 } &&
                !Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId),
                "Only the original Draw3 entities are publicly shown, and their actual first-suit reward suspends the same unfinished program frame.");
            game = Cold(game, registry); AnswerAccepted(game, "reward");
            ReachAccepted(game, () => Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId));
            Need(Events<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == frameId).Completed &&
                Events<NativeDrawAttemptStartedEvent>(game).Count(fact => fact.Context.Reason.Value == DailiDrawReason) == 1 &&
                Events<ProgramCardsRevealedEvent>(game).Count(fact => fact.FrameId == frameId) == 1 &&
                Events<PublicHandRevealWindowReturnedEvent>(game).Count(fact => fact.ParentFrameId == frameId) == 1,
                "All paid flip, draw and public-reveal children return once to the actual completed-turn program after cold recovery.");
        }
    }

    private static void AssertDailiDeath(GameEngine game, long frameId, int expectedDraws)
    {
        Need(!View(game, 0).IsAlive && game.State.Winner == Winner.None &&
            Events<PlayerDyingEvent>(game).Count(fact => fact.VictimSeat == 0) == 1 &&
            Events<PlayerDiedEvent>(game).Count(fact => fact.VictimSeat == 0) == 1 &&
            Events<ProgramBindingResolvedEvent>(game).Count(fact => fact.FrameId == frameId) == 1 &&
            Events<NativeDrawAttemptStartedEvent>(game).Count(fact => fact.Context.Reason.Value == DailiDrawReason) == expectedDraws &&
            Events<ProgramDrawCompletedEvent>(game).Count(fact => fact.FrameId == frameId) == expectedDraws &&
            !Events<ProgramCardsRevealedEvent>(game).Any(fact => fact.FrameId == frameId) &&
            !Events<ProgramBindingStartedEvent>(game).Any(fact => fact.SkillId == "mou-ol:jueyan"),
            "Death drains the paid original instruction in a live Team game without issuing an unpaid Draw, show or reward suffix.");
        if (expectedDraws == 1)
            Need(Events<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == frameId) is { RequestedCount: 3, ActualDrawCount: 3 },
                "Death inside the real gain child does not undo or repeat the already delivered original Draw3.");
    }

    private static bool DailiOption(GameEngine game, string id) => game.PendingDecision?.SkillPrompt?.SkillId == DailiWatch &&
        game.PendingDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == id);

    private static void ShowDailiCard(GameEngine game, int id)
    {
        Use(game, "show", skill: DailiDriver);
        ReachAccepted(game, () => game.PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        AnswerAcceptedCard(game, id); Play(game);
    }

    private static (GameEngine, ContentRegistry) CreateDaili(string scenario)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new DailiFixture(scenario));
        var setup = new ShanheBattleSetup(7, 1, "shared-turn-end-recipe", DailiOwner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(DailiPeer, Hp: 8) { StartingHand = [] }, new(DailiPeer, Hp: 8) { StartingHand = [] }],
            [new(DailiPeer, Hp: 8) { StartingHand = [] }], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(game, new StartGameCommand()); Play(game); return (game, registry);
    }

    private sealed class DailiFixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shared-turn-end", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var jueman = scenario.StartsWith("jueman-", StringComparison.Ordinal);
            var sources = new List<(string Resource, string Skill)> { ("ol-luo-xian", Daili), ("mou-xiao-qiao-ol", "mou-ol:jueyan") };
            if (jueman) sources.Add(("ol-a-hui-nan", Jueman));
            foreach (var (resource, id) in sources)
            {
                var bundle = CharacterFixtureContent.EmbeddedProgramBundle(resource);
                builder.AddSkill(new(id, bundle.Presentations[id].Name, bundle.Presentations[id].Description)
                    { Program = bundle.Programs[id], ProgramPresentation = bundle.Presentations[id] });
            }
            static string Pause(string id) => "{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"" + id + "\",\"options\":[{\"id\":\"" + id + "\"}]}";
            var activeSkill = jueman ? Jueman : scenario == "unpermitted" ? DailiOrdinary : Daili;
            var flipEffects = Pause("flip");
            if (scenario is "changed" or "empty" or "unpermitted")
                flipEffects += "," + JsonSerializer.Serialize(new { op = "skillState", target = "owner", action = "lose", match = new { skillIds = new[] { activeSkill } } }) + "," + Pause("flip-return");
            if (scenario == "death-flip") flipEffects += ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":10}";
            var drawEffects = Pause("draw");
            if (scenario == "changed") drawEffects += """
                ,{"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":1,"maximumCards":1,"resultBind":"away"},
                 {"op":"moveBoundCards","target":"owner","sourceBind":"away","destination":"drawPileTop","awaitMovementTriggers":true},
                 {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                 {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true},
                 {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}
                """;
            if (scenario == "empty") drawEffects += """
                ,{"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":3,"maximumCards":3,"resultBind":"all-away"},
                 {"op":"moveBoundCards","target":"owner","sourceBind":"all-away","destination":"discardPile","awaitMovementTriggers":true},
                 {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}
                """;
            if (scenario == "death-draw") drawEffects += ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":10}";
            else drawEffects += ",{\"op\":\"grantSkills\",\"target\":\"owner\",\"skillIds\":[\"mou-ol:jueyan\"]}," + Pause("draw-return");
            var optionLabels = new Dictionary<string, string>
                { ["flip"] = "翻面返回", ["draw"] = "摸牌已交付", ["reward"] = "奖励返回" };
            if (scenario is "changed" or "empty" or "unpermitted") optionLabels["flip-return"] = "返回原指令";
            if (scenario != "death-draw") optionLabels["draw-return"] = "返回展示";
            var juemanWatch = "";
            if (jueman)
            {
                var effects = Pause("jueman-draw") + "," + JsonSerializer.Serialize(new { op = "skillState", target = "owner", action = "lose", match = new { skillIds = new[] { Jueman } } }) + "," + Pause("jueman-return");
                juemanWatch = "," + $$$"""{"id":"jueman-paid-draw","window":"cardsGained","subject":"owner","movementOccurrence":"perBatch","destinationZones":["hand"],"movementReasons":["{{{JuemanDrawReason}}}"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{{{effects}}}]}""";
                optionLabels["jueman-draw"] = "基本牌奖励已交付";
                optionLabels["jueman-return"] = "失源后返回";
            }
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{DailiDriver}}}","revision":1,"activations":[
                    {"id":"show","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":1,"maximumCards":1,"resultBind":"shown"},
                      {"op":"revealBoundCards","target":"owner","sourceBind":"shown"}]},
                    {"id":"give","minCards":1,"maxCards":1,"minTargets":1,"maxTargets":1,"sourceZones":["hand"],"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"captureSelectedCards","target":"owner","resultBind":"gift"},{"op":"moveBoundCards","target":"owner","sourceBind":"gift","destination":"selectedTargetHand"}]},
                    {"id":"take","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"ownerHand","awaitMovementTriggers":true}]}]},
                  {"id":"{{{DailiWatch}}}","revision":1,"triggers":[
                    {"id":"flip","window":"characterTurnedOver","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{{{flipEffects}}}]},
                    {"id":"draw","window":"cardsGained","subject":"owner","movementOccurrence":"perBatch","destinationZones":["hand"],"movementReasons":["{{{DailiDrawReason}}}"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[
                      {{{drawEffects}}}]},
                    {"id":"reward","window":"cardsGained","subject":"owner","movementOccurrence":"perBatch","destinationZones":["hand"],"movementReasons":["skill-program.mou-ol:jueyan.Draw"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{{{Pause("reward")}}}]}{{{juemanWatch}}}]},
                  {"id":"{{{DailiOrdinary}}}","revision":1,"triggers":[{"id":"ordinary-after-end","window":"afterTurnEnded","subject":"owner","turnOwnerScope":"own","optional":true,
                    "effects":[{"op":"turnOver","target":"owner"},{"op":"draw","target":"owner","amount":3,"awaitMovementTriggers":true}]}]},
                  {"id":"{{{AcceptedQuiet}}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{NativeGate}}}","revision":1,"triggers":[{"id":"native-query","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [DailiDriver] = new { name = "共享流程命令", description = "真实展示与实体重入" },
                        [DailiWatch] = new { name = "原帧子窗口", description = "真实翻面、摸牌与展示奖励", optionLabels },
                        [DailiOrdinary] = new { name = "普通回合末流程", description = "未声明失源许可的共享边界反例" },
                        [AcceptedQuiet] = new { name = "固定正常摸牌", description = "不混入普通摸牌" },
                        [NativeGate] = new { name = "原生请求", description = "真实原生摸牌订阅" }
                    } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(DailiOwner, "共享回合末流程", "", DailiDriver, "wu", 4, [activeSkill, DailiWatch, AcceptedQuiet, NativeGate]));
            builder.AddGeneral(new(DailiPeer, "固定同行", "", AcceptedQuiet, "qun", 8));
            builder.AddDeck(new("fixture:daili-deck", "固定真实原生牌", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 80)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 9)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "共享回合末真实流程", 4, 4, new Dictionary<string, int>(), "fixture:daili-deck",
                GeneralPoolIds: [DailiOwner, DailiPeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

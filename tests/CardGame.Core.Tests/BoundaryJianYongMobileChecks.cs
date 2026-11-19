using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryJianYongMobileChecks
{
    private const string Zongshi = "boundary:zongshih-mobile";
    private const string Obtain = "skill-program.boundary:zongshih-mobile.obtain";
    private const string Driver = "fixture:zongshi-comparison", Watch = "fixture:zongshi-obtain", Rank = "fixture:zongshi-rank";
    private const string Mode = "identity:zongshi-processing-entry", Deck = "fixture:zongshi-deck";
    private static readonly string[] Generals = ["fixture:zongshi-owner", "fixture:zongshi-peer-one",
        "fixture:zongshi-peer-two", "fixture:zongshi-peer-three"];

    public static void FirstComparisonObtainsCommonCardAndSecondUsesOriginalEntries()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The fixed match must reach the source's actual Play phase.");
        var material = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        Submit(game, new UseProgramSkillCommand(0, Driver, "group", [material], [1, 2], game.Revision, play.PromptId));

        var firstChoice = ReachZongshi(game);
        var firstResult = Events<PindianResultDeterminedEvent>(game).Single();
        var reveal = game.CardMovements.Single(move => move.CardId == material && move.Reason == CardMoveReasons.PindianReveal);
        var firstGroup = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == firstResult.FrameId);
        Require(firstResult.Result is { SourceSeat: 0, OpponentSeat: 1, SourceRank: 7, OpponentRank: 13 } &&
                firstResult.Result.SourceCardId == material && reveal.From == CardLocation.Hand(0) &&
                reveal.To == CardLocation.Processing && firstGroup.SimultaneousPindian!.ProcessingEntries!
                    .Single(entry => entry.CardId == material).EntrySequence == reveal.Sequence &&
                firstChoice.Choices.Where(choice => Branch(choice) == "contest")
                    .SelectMany(choice => choice.Cards).SequenceEqual([material]),
            "The first comparison must offer the common source entity as its only lower original card.");
        game = Cold(game, registry);
        Answer(game, choice => Branch(choice) == "contest" && choice.Cards.SequenceEqual([material]));
        var paid = Events<BoundaryJianYongZongshiPaidEvent>(game).Single();
        Require(Next(game).SkillPrompt?.SkillId == Watch && paid.PindianId == firstResult.FrameId &&
                paid.ComparisonIndex == 0 && paid.FromZone == CardZoneKind.Processing &&
                game.CardMovements.Count(move => move.CardId == material && move.From == CardLocation.Processing &&
                    move.To == CardLocation.Hand(0) && move.Reason.Value == Obtain && move.Sequence > reveal.Sequence) == 1,
            "Zongshi must obtain the original material once before its real gain observer returns.");
        game = Cold(game, registry);
        Answer(game, _ => true);

        var secondChoice = ReachZongshi(game);
        var secondResult = Events<PindianResultDeterminedEvent>(game).Last();
        var viewed = secondChoice.Choices.Single(choice => Branch(choice) == "top").Cards.Single();
        Require(secondResult.FrameId == firstResult.FrameId &&
                secondResult.Result is { SourceSeat: 0, OpponentSeat: 2, SourceRank: 7, OpponentRank: 13 } &&
                secondResult.Result.SourceCardId == material && !secondChoice.Choices.Any(choice => Branch(choice) == "contest") &&
                game.CreateSnapshot(1).PendingDecision is null && game.CreateSnapshot(2).PendingDecision is null &&
                game.CreateSnapshot(2).PublicRevealedCards.All(card => card.Id != viewed),
            "The second comparison can still view the deck top privately after the only lower original entity was obtained.");
        game = Cold(game, registry);
        Answer(game, choice => Branch(choice) == "skip");
        ReachPlay(game);
        Require(Events<SimultaneousPindianCompletedEvent>(game).Single().CompletedComparisons == 2 &&
                Events<BoundaryJianYongZongshiPaidEvent>(game).SequenceEqual([paid]) &&
                game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == material) &&
                !game.CardMovements.Any(move => move.CardId == material && move.Reason == CardMoveReasons.PindianFinish),
            "The same group must resolve twice while leaving the once-obtained common entity with its recipient.");
        _ = Cold(game, registry);
    }

    private static PendingDecision ReachZongshi(GameEngine game) =>
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.SkillPrompt?.SkillId == Zongshi);

    private static void ReachPlay(GameEngine game) =>
        _ = Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });

    private static PendingDecision Reach(GameEngine game, Func<PendingDecision, bool> ready)
    {
        for (var step = 0; step < 30; step++)
        {
            var prompt = Next(game);
            if (ready(prompt)) return prompt;
            Require(prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "simultaneous-pindian"),
                $"Unexpected decision while waiting for the original comparison. {prompt.Kind} / {prompt.SkillPrompt?.SkillId}");
            Answer(game, choice => Branch(choice) is "commit" or "draw" or "claim-decline");
        }
        throw new InvalidOperationException("The simultaneous Pindian did not reach its expected public command boundary.");
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 60; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt) return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The fixed match did not publish its next decision.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> select)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.First(select);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat =>
                    SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
                JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
                game.CardMovements.SequenceEqual(restored.CardMovements) &&
                Events<BoundaryJianYongZongshiPaidEvent>(game).SequenceEqual(Events<BoundaryJianYongZongshiPaidEvent>(restored)),
            "Cold replay must reproduce private choices, the original comparison, and its physical payment.");
        return restored;
    }

    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The real comparison command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-zongshi-processing-entry", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:jian-yong-mobile");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"group","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":3,"targetKind":"otherLivingWithHand","usesPerTurn":null,"usesPerPhase":1,"effects":[{"op":"startSimultaneousPindian","target":"owner"}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"obtain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{Obtain}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{Rank}}","revision":1,"cardPolicies":[{"id":"heart-thirteen","kind":"pindianRankBySuit","inputSuit":"heart","value":13}]}
            ]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "原生同时拼点", description = "与两名角色拼点" },
                    [Watch] = new { name = "实际领牌", description = "暂停领取后的移动子窗", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [Rank] = new { name = "固定有效点数", description = "红桃拼点牌的点数为K" } } });
            var programs = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                    { Program = program, ProgramPresentation = programs.Presentations[id] });

            builder.AddGeneral(new(Generals[0], "界简雍验证", "supporter", Driver, "shu", AdditionalSkillIds: [Zongshi, Watch]));
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "拼点参与者", "supporter", Rank, "wu", 6));
            builder.AddDeck(new(Deck, "固定真实牌面", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 96)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "纵适原拼点牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 1 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

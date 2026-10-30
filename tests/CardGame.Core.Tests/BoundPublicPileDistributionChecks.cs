using System.Text.Json;
using CardGame.Core;

internal static class BoundPublicPileDistributionChecks
{
    public static void SelectedSourceFrozenSuitsAndPaidChildrenReplayExactly()
    {
        Reject("""
            {"op":"draw","target":"owner","amount":4,"resultBind":"pile"},
            {"op":"distributeBoundCards","target":"owner","sourceBind":"pile","targetKind":"otherLiving"}
            """,
            "requires an original selected public-pile binding");
        Reject("""
            {"op":"selectPublicPile","target":"owner","amount":4,"skillIds":["classic:bizhuan"],"resultBind":"pile"},
            {"op":"distributeBoundCards","target":"owner","sourceBind":"pile","targetKind":"otherLiving"},
            {"op":"distributeBoundCards","target":"owner","sourceBind":"pile","targetKind":"otherLiving"}
            """,
            "cards may be moved more than once");
        foreach (var mode in new[] { "", "none-suits" })
        {
            var (game, registry) = Fame2017PublicPileChecks.CreateDistributionTableFixture(mode);
            var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
            var frameId = parent.Id;
            var binding = parent.CardSetBindings.Single(binding => binding.Name == "selected-books");
            var original = binding.CardIds.ToArray();
            Need(parent.PublicPileDraft is { Stage: "distribute" } draft && draft.RemainingIds.SequenceEqual(original) &&
                binding.Visibility == SkillProgramCardSetVisibility.Public && binding.FrozenSelectedSuits is { Count: 4 } suits &&
                suits.Distinct().Count() == 4 && (mode == "none-suits" ? suits.Contains(Suit.None) : !suits.Contains(Suit.None)),
                "The exact selected source and its printed suit metadata are frozen before the first gift, including the legacy None suit value.");
            var issued = game.Events.Select(item => item.Payload).OfType<ProgramPublicPileSelectedEvent>()
                .Single(item => item.FrameId == frameId && item.Bind == binding.Name);
            var committed = JsonSerializer.Serialize(issued);
            AssertReadOnly(issued.CardIds, -issued.CardIds[0]);
            AssertReadOnly(issued.PrintedSuits, issued.PrintedSuits[0] == Suit.None ? Suit.Spade : Suit.None);
            Need(JsonSerializer.Serialize(game.Events.Select(item => item.Payload).OfType<ProgramPublicPileSelectedEvent>()
                    .Single(item => item.FrameId == frameId && item.Bind == binding.Name)) == committed,
                "Attempting to alter committed source entities or printed suits cannot change the historical selection invoice.");
            var restored = Cold(game, registry);
            var restoredBinding = restored.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == frameId)
                .CardSetBindings.Single(item => item.Name == binding.Name);
            Need(restoredBinding.CardIds.SequenceEqual(binding.CardIds) &&
                restoredBinding.SourceLocations.SequenceEqual(binding.SourceLocations) &&
                restoredBinding.FrozenSelectedSuits!.SequenceEqual(binding.FrozenSelectedSuits!),
                "Cold replay retains the selected source's exact frozen entities, original locations and printed suit metadata.");
            foreach (var branch in new[] { game, restored })
            {
                var movementStart = branch.CardMovements.Count;
                var choice = Decision(branch)!.Choices.First(choice => choice.Cards.Count == 1 && choice.Targets.SequenceEqual([1]));
                Accept(branch, new AnswerPromptCommand(0, Decision(branch)!.PromptId, choice.Id, branch.Revision));
                Reach(branch, () => Decision(branch)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "choose-option") == true);
                Need(branch.CardMovements.Skip(movementStart).Count(move => move.Reason.Value == "skill-program.public-pile.distribute") == 1 &&
                    branch.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == frameId).PublicPileDraft!.RemainingIds.Count == 3,
                    "The first real gift stops at its gained-card child without moving the other bound entities.");
                _ = Cold(branch, registry);
                for (var step = 0; branch.ResolutionStack.Any(frame => frame.Id == frameId); step++)
                {
                    Need(step < 100, "The bounded public distribution must finish its original cursor.");
                    if (Decision(branch) is { } prompt)
                    {
                        var answer = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("stage") == "distribute" && choice.Targets.SequenceEqual([1])) ??
                            prompt.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "choose-option") ??
                            (prompt.SkillPrompt?.SkillId == "fixture:book-observer" ? prompt.Choices.SingleOrDefault(choice =>
                                choice.Parameters.GetValueOrDefault("program-action") == "activate") : null);
                        if (answer is not null) Accept(branch, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, answer.Id, branch.Revision));
                        else
                        {
                            Need(prompt.PlayerSeat != 0, "Unexpected human choice during native distribution.");
                            Accept(branch, new AdvanceOneStepCommand(branch.Revision));
                        }
                    }
                    else Accept(branch, new AdvanceOneStepCommand(branch.Revision));
                }
                var given = branch.CardMovements.Skip(movementStart).Where(move => move.Reason.Value == "skill-program.public-pile.distribute").ToArray();
                Need(given.Select(move => move.CardId).Order().SequenceEqual(original.Order()) &&
                    given.All(move => move.From.Zone == CardZoneKind.PublicPersistentPile && move.To == CardLocation.Hand(1)) &&
                    branch.CreateSnapshot(0).Players[0].PublicPersistentPileCount == 0,
                    "Every original bound entity moves directly to a real recipient exactly once, preserving the source, reason and printed None semantics.");
            }
            Need(State(game) == State(restored), "Cold command replay preserves distribution choices, paid child returns, all views and committed facts.");
        }
    }

    private static void AssertReadOnly<T>(IReadOnlyList<T> values, T replacement)
    {
        Need(values is IList<T> { IsReadOnly: true }, "Committed public-pile collection payloads must be frozen.");
        var before = values.ToArray();
        try
        {
            ((IList<T>)values)[0] = replacement;
            throw new InvalidOperationException("A committed public-pile collection accepted an observer write.");
        }
        catch (NotSupportedException) { }
        Need(values.SequenceEqual(before), "A rejected observer write must leave every committed collection element unchanged.");
    }

    private static void Reject(string effects, string fragment)
    {
        var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:pile-binding","revision":1,"activations":[
              {"id":"go","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{{effects}}]}]}]}
            """;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:pile-binding"] = new { name = "公开堆绑定", description = "来源与单次消费" } } });
        try { _ = SkillProgramCatalog.Load(rules, presentation); }
        catch (InvalidOperationException error) when (error.Message.Contains(fragment, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException("Expected public-pile binding rejection: " + fragment);
    }

    private static void Reach(GameEngine game, Func<bool> done)
    {
        for (var step = 0; !done(); step++)
        {
            Need(step < 50, "The next expected gained-card child must be reached within its bounded native steps.");
            if (Decision(game) is { PlayerSeat: 0 } pending)
            {
                var activate = pending.SkillPrompt?.SkillId == "fixture:book-observer"
                    ? pending.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate") : null;
                Need(activate is not null, "Unexpected public distribution choice: " + JsonSerializer.Serialize(new
                { pending.Kind, pending.PlayerSeat, Skill = pending.SkillPrompt?.SkillId,
                    Choices = pending.Choices.Take(1).Select(choice => new { choice.Id, choice.Parameters }).ToArray(),
                    Frames = game.ResolutionStack.Select(frame => new { frame.Id, Type = frame.GetType().Name,
                        Skill = (frame as ProgramSkillFrame)?.SkillId }).ToArray(),
                    Events = game.Events.TakeLast(18).Select(item => new { Type = item.Payload.GetType().Name,
                        Json = JsonSerializer.Serialize(item.Payload, item.Payload.GetType()) }).ToArray() }));
                Accept(game, new AnswerPromptCommand(0, pending.PromptId, activate!.Id, game.Revision));
                continue;
            }
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
    }
    private static PendingDecision? Decision(GameEngine game) => Enumerable.Range(0, 4)
        .Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(decision => decision is not null);
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Public distribution command rejected."); }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    { game.Revision, Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(),
        game.CardMovements, Events = game.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())).ToArray() });
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == State(restored), "A suspended public distribution must restore its exact command-driven state.");
        return restored;
    }
    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
}

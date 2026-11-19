using CardGame.Content.Standard;
using CardGame.Core;

internal static class XingbuRevealChecks
{
    private const string Xingbu = "ol:xingbu", Mode = "identity:xingbu-reveal-test", Deck = "fixture:xingbu-reveal-deck";
    private const string Reveal = "skill-program.ol:xingbu.forecast.reveal";
    private const string Cleanup = "skill-program.ol:xingbu.forecast.discard";
    private static readonly string[] Generals = ["fixture:xingbu-owner", "fixture:xingbu-peer-1",
        "fixture:xingbu-peer-2", "fixture:xingbu-peer-3"];

    public static void FrozenPublicRevealAndOwnedCleanupReplay()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        var offer = Next(game);
        Need(offer is { PlayerSeat: 0, SkillPrompt.SkillId: Xingbu }, "Xingbu opens on its owner's actual turn ending.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var assignment = Next(game);
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.QiaoZhouForecast is not null).QiaoZhouForecast!;
        var revealed = game.Events.Select(e => e.Payload).OfType<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Xingbu);
        var entries = game.CardMovements.Where(move => move.Reason.Value == Reveal).ToArray();
        Need(assignment is { PlayerSeat: 0, SkillPrompt.SkillId: Xingbu } &&
            assignment.Choices.Where(choice => choice.Targets.Count != 0).All(choice => !choice.Targets.Contains(0)) &&
            original is { Stage: QiaoZhouForecastStage.Recipient, RedCount: 3 } && entries.Length == 3 &&
            original.Cards.Select(card => card.MovementSequence).SequenceEqual(entries.Select(move => move.Sequence)) &&
            revealed.Cards.Select(card => card.Id).SequenceEqual(entries.Select(move => move.CardId)) &&
            entries.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Processing),
            "The frozen three-card reveal must keep its original Processing movements and red count until the later assignment.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements), "The reveal, movement identities and choice must cold-replay identically.");

        foreach (var (run, grant) in new[] { (game, true), (restored, false) })
        {
            Answer(run, choice => grant ? choice.Targets.SequenceEqual([2]) : choice.Parameters.GetValueOrDefault("option") == "finish");
            Need(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 }, "The same turn ending must close after forecast cleanup.");
            var moves = run.CardMovements.Where(move => move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile &&
                entries.Any(entry => entry.CardId == move.CardId)).ToArray();
            var marks = run.Events.Select(e => e.Payload).OfType<QiaoZhouForecastGrantedEvent>().ToArray();
            Need(moves.Select(move => move.CardId).SequenceEqual(entries.Select(entry => entry.CardId)) &&
                moves.All(move => move.Reason.Value == Cleanup) && (grant
                    ? marks is [{ RecipientSeat: 2, RedCount: 3 }]
                    : marks.Length == 0),
                "All three original entries are retired once by Xingbu with or without a mark.");
        }
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> select)
    {
        var prompt = Next(game);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Xingbu did not reach the next public command boundary.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Xingbu command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-xingbu-reveal", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var production = CharacterFixtureContent.EmbeddedProgramBundle("ol-qiao-zhou");
            var presentation = production.Presentations[Xingbu];
            builder.AddSkill(new(Xingbu, presentation.Name, presentation.Description)
                { Program = production.Programs[Xingbu], ProgramPresentation = presentation });
            builder.AddGeneral(new(Generals[0], "星卜", "supporter", Xingbu, "shu", 8));
            foreach (var id in Generals.Skip(1))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "红色实体牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "星卜原始亮牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

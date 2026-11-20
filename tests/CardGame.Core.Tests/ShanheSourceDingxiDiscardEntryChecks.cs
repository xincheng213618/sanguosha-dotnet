using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceDingxiDiscardEntryChecks
{
    private const string Dingxi = StandardShanheSourceBeigongDuoPackage.DingxiSkillId;
    private const string Driver = "fixture:source-dingxi-completed-use", Deck = "fixture:source-dingxi-discard-entry-deck";
    private const string LaterDiscard = "skill-program.fixture:source-dingxi-completed-use.SelectAndMoveOwnedCard";

    internal static void DingxiClaimsOnlyItsOriginalUseDiscardEntry()
    {
        foreach (var rediscard in new[] { false, true })
        {
            var (game, content) = Start(rediscard);
            var play = Next(game);
            Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.CreateSnapshot(0).DrawPileCount == 0,
                "The original Play phase begins with no cards left in the draw pile.");
            var duel = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Duel).Id;
            var action = game.GetHumanLegalActions().Single(option => option.CardId == duel && option.ConversionSource is null && option.TargetSeats.SequenceEqual([1]));
            Submit(game, new PlayCardCommand(0, duel, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind, action.TargetCardId));
            var pending = Next(game);
            var original = game.CardMovements.Single(move => move.CardId == duel && move.Reason == CardMoveReasons.UseFinished);
            var qualified = Facts<SourceDingxiDiscardQualifiedEvent>(game).Single(e => e.CardId == duel);
            Need(original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile && original.Sequence == qualified.DiscardSequence &&
                 original.BatchId == qualified.BatchId && qualified.OwnerSeat == 0 &&
                 Facts<CardUseFinishedEvent>(game).Single(e => e.CardId == duel).ResolutionId == qualified.OriginalUseId,
                "A genuine finished Duel qualifies on its one original Processing-to-Discard entry.");

            if (!rediscard)
            {
                Need(pending is { SkillPrompt.SkillId: Dingxi }, "The original still-current use discard offers Dingxi.");
                var material = pending;
                if (pending.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate") is { } activate)
                {
                    Answer(game, pending, activate);
                    material = Next(game);
                }
                Answer(game, material, material.Choices.Single(choice => choice.Cards.SequenceEqual([duel]) && choice.Targets.SequenceEqual([2])));
                Need(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                     Facts<SourceDingxiEnteredEvent>(game).Single() is { CardId: var entered, OriginalDiscardSequence: var sequence } && entered == duel && sequence == original.Sequence &&
                     Facts<SourceDingxiStoredEvent>(game).Single().CardId == duel,
                    "Dingxi reuses that physical Duel on the fixed left target and stores it after the whole native use.");
                continue;
            }

            var draw = game.CardMovements.Where(move => move.CardId == duel && move.Sequence > original.Sequence).ToArray();
            Need(pending.Choices.Count(choice => choice.Cards.SequenceEqual([duel])) == 1 &&
                 game.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == qualified.OriginalUseId && frame.Step == ResolutionFrameStep.Completed) &&
                 draw is [var shuffled, var gained] && shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
                 shuffled.Reason == CardMoveReasons.Reshuffle && gained.From == CardLocation.DrawPile && gained.To == CardLocation.Hand(0),
                "The public completed-use child runs under the original native Use and actually draws its exact discarded Duel after reshuffling.");
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
            Need(restored.CardMovements.SequenceEqual(game.CardMovements) &&
                 SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "Cold replay preserves the completed-use child and its successfully drawn Duel.");
            foreach (var run in new[] { game, restored })
            {
                var discard = Next(run);
                Answer(run, discard, discard.Choices.Single(choice => choice.Cards.SequenceEqual([duel])));
                var next = Next(run);
                var later = run.CardMovements.Single(move => move.CardId == duel && move.Reason.Value == LaterDiscard && move.To == CardLocation.DiscardPile);
                Need(later.Sequence > draw[^1].Sequence && later.BatchId != original.BatchId &&
                     Facts<NativeLogicalDiscardMaterialCompletedEvent>(run).Any(receipt => receipt.CardId == duel && receipt.From == CardLocation.Hand(0) &&
                         receipt.To == CardLocation.DiscardPile && receipt.CompletionSequence == later.Sequence) &&
                     Facts<SourceDingxiDiscardQualifiedEvent>(run).Select(receipt => receipt.DiscardSequence).SequenceEqual([original.Sequence]),
                    "The completed-use child genuinely discards the same Duel into a different entry with no qualifying native use receipt.");
                Need(next is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                     run.CreateCardZoneDiagnostics().Single(card => card.CardId == duel).Location == CardLocation.DiscardPile &&
                     !Facts<SourceDingxiEnteredEvent>(run).Any(),
                    "The old Dingxi window cannot offer or obtain a Duel that has returned through a different entry.");
            }
        }
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool rediscard)
    {
        const string enemy = "classic:guan-yu";
        var setup = new ShanheBattleSetup(211, 1, "source-dingxi-discard-entry", StandardShanheSourceBeigongDuoPackage.BeigongBaseId,
            4, 0, 0, 0, 0, rediscard ? [Driver] : [], [new(CardKind.Duel, Suit.Club, 7)], [],
            [new(enemy) { StartingHand = [] }, new(enemy) { StartingHand = [] }], [], new string('d', 64));
        var content = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new Fixture(), mode =>
            mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        return (game, content);
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                if (pass is null)
                    return prompt;
                Answer(game, prompt, pass);
            }
            else Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Dingxi did not reach the next public decision.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, PromptChoice choice) =>
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Dingxi command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-dingxi-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"triggers":[
                  {"id":"draw-and-discard-completed-duel","window":"cardUseCompleted","ownerRelation":"actor","cardKinds":["duel"],
                   "optional":false,"usageScope":"game","usageLimit":1,"effects":[
                     {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"cardKinds":["duel"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "收回后弃置", description = "决斗结算后摸一张牌，然后弃置一张决斗。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var skill = catalog.Presentations[Driver];
            builder.AddSkill(new(Driver, skill.Name, skill.Description) { Program = catalog.Programs[Driver], ProgramPresentation = skill });
            builder.AddDeck(new(Deck, "两张开局摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Spade, 5), new("standard:dodge", Suit.Club, 6)] });
        }
    }
}

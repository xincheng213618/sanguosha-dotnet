using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class XingbuRevealChecks
{
    private const string Xingbu = "ol:xingbu", Mode = "identity:xingbu-reveal-test", Deck = "fixture:xingbu-reveal-deck";
    private const string Reveal = "skill-program.ol:xingbu.forecast.reveal";
    private const string Cleanup = "skill-program.ol:xingbu.forecast.discard";
    private const string PhaseDemand = "fixture:xingbu-phase-demand";
    private const int Seed = 47, BlackForecastCard = 24, Recipient = 1;
    private static readonly string[] Generals = ["fixture:xingbu-owner", "fixture:xingbu-peer-1",
        "fixture:xingbu-peer-2", "fixture:xingbu-peer-3"];

    public static void FrozenPublicRevealAndOwnedCleanupReplay()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = StartForecast(content);
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

        var restored = Cold(game, content);

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

    public static void TwoRedForecastSkipsTheNextDiscardWithoutAnActualPhaseEndCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(twoRedForecast: true));
        var game = StartForecast(content);
        var assignment = Next(game);
        var forecast = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.QiaoZhouForecast is not null).QiaoZhouForecast!;
        Need(assignment is { PlayerSeat: 0, SkillPrompt.SkillId: Xingbu } && forecast is { Stage: QiaoZhouForecastStage.Recipient, RedCount: 2 } &&
            forecast.Cards.Count == 3 && forecast.Cards.Any(card => card.CardId == BlackForecastCard && card.Suit == Suit.Club),
            $"The fixed deck must put two red cards and club card {BlackForecastCard} in the actual Xingbu reveal. Revealed: {string.Join(", ", forecast.Cards)}.");

        var control = Cold(game, content);
        Answer(control, choice => choice.Parameters.GetValueOrDefault("option") == "finish");
        CheckRecipientTurn(control, content, null);

        var issuedTurn = game.CreateSnapshot(0).TurnNumber;
        Answer(game, choice => choice.Targets.SequenceEqual([Recipient]));
        var grant = Facts<QiaoZhouForecastGrantedEvent>(game).Single();
        Need(grant.RecipientSeat == Recipient && grant.RedCount == 2 && grant.IssuedTurn == issuedTurn,
            "The two-red result is issued to the next living seat from the original turn ending.");
        CheckRecipientTurn(game, content, grant);
    }

    private static void CheckRecipientTurn(GameEngine game, ContentRegistry content, QiaoZhouForecastGrantedEvent? grant)
    {
        var play = Next(game);
        var snapshot = game.CreateSnapshot(Recipient);
        var turn = snapshot.TurnNumber;
        var recipient = snapshot.Players[Recipient];
        var excess = recipient.HandCount - (recipient.HandLimit ?? recipient.HandCount);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: Recipient } && excess > 0 &&
            Facts<TurnStartedEvent>(game).Count(fact => fact.ActorSeat == Recipient && fact.TurnNumber == turn) == 1 &&
            (grant is null ? !Facts<QiaoZhouForecastActivatedEvent>(game).Any() :
                turn == grant.IssuedTurn + 1 && Facts<QiaoZhouForecastActivatedEvent>(game).Single() == new QiaoZhouForecastActivatedEvent(grant.GrantId, Recipient, turn)),
            "The next actual turn reaches Play with real excess cards and only the issued forecast becomes active.");
        game = Cold(game, content);
        Submit(game, new EndPlayPhaseCommand(Recipient, game.Revision, play.PromptId));
        Need(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 2 }, "The recipient's turn returns to the next seat's ordinary Play.");

        bool OwnPhase(ActualDiscardRecoveryPhaseKey phase, ActualDiscardRecoveryPhaseKind kind) =>
            phase.Kind == kind && phase.TurnNumber == turn && phase.ActualTurnOwnerSeat == Recipient && phase.ActorSeat == Recipient;
        bool OwnDiscard(ActualDiscardRecoveryPhaseKey phase) => OwnPhase(phase, ActualDiscardRecoveryPhaseKind.Discard);
        var discardStarts = Facts<ActualDiscardRecoveryPhaseStartedEvent>(game).Where(fact => OwnDiscard(fact.Phase)).ToArray();
        var confirmations = Facts<ActualDiscardPhaseExecutionConfirmedEvent>(game).Where(fact => OwnDiscard(fact.Phase)).ToArray();
        var discardEnds = Facts<ActualPhaseEndedEvent>(game).Where(fact => OwnDiscard(fact.Phase)).ToArray();
        var skips = Facts<ActualPhaseSkippedEvent>(game).Where(fact => fact.TurnNumber == turn && fact.Kind == ActualDiscardRecoveryPhaseKind.Discard).ToArray();
        var payments = game.CardMovements.Count(move => move.TurnNumber == turn && move.From == CardLocation.Hand(Recipient) &&
            move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard);
        var phaseChanges = Facts<PhaseChangedEvent>(game).Count(fact => fact.ActorSeat == Recipient && fact.Phase == TurnPhase.Discard);
        var ending = Facts<ActualPhaseEndedEvent>(game).Single(fact => OwnPhase(fact.Phase, ActualDiscardRecoveryPhaseKind.Ending));
        Need(Facts<TurnEndedEvent>(game).Count(fact => fact.ActorSeat == Recipient && fact.TurnNumber == turn) == 1 &&
            Facts<ActualTurnCursorReturnedEvent>(game).Count(fact => fact.OwnerSeat == Recipient && fact.ActualTurn == turn) == 1 &&
            Facts<ActualPhaseEndingEvent>(game).Single(fact => OwnPhase(fact.Phase, ActualDiscardRecoveryPhaseKind.Ending)).BoundaryFrameId == ending.BoundaryFrameId,
            "The recipient's actual Ending opens, completes and returns once in either branch.");

        if (grant is null)
        {
            Need(discardStarts is [var started] && confirmations is [var confirmed] && discardEnds is [var ended] &&
                started.Phase == confirmed.Phase && started.Phase == ended.Phase && skips.Length == 0 && payments == excess && phaseChanges == 1 &&
                !Facts<QiaoZhouDiscardPhaseSkippedEvent>(game).Any(),
                "Declining the forecast pays the real hand-limit discard and completes its original entered phase once.");
            return;
        }

        Need(discardStarts.Length == 0 && confirmations.Length == 0 && discardEnds.Length == 0 && payments == 0 && phaseChanges == 0 &&
            skips is [var skipped] && skipped is { ActualTurnOwnerSeat: Recipient, ActorSeat: Recipient, NativeFrameId: null, IsExtra: false } &&
            Facts<QiaoZhouDiscardPhaseSkippedEvent>(game).Single() == new QiaoZhouDiscardPhaseSkippedEvent(grant.GrantId, Recipient, turn) &&
            game.CreateSnapshot(Recipient).Players[Recipient].HandCount == recipient.HandCount,
            "The accepted two-red forecast skips the natural discard once before entry and preserves all excess cards.");
    }

    private static GameEngine StartForecast(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = Seed, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Need(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Xingbu }, "Xingbu opens on its owner's actual turn ending.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        return game;
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements), "The movement identities and current choice must cold-replay identically.");
        return restored;
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

    private sealed class Fixture(bool twoRedForecast = false) : IGameContentPackage
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
            if (twoRedForecast)
            {
                // Register a phase-end consumer without granting it to any seat so both branches expose their actual facts.
                var demand = SkillProgramCatalog.LoadCurrent($$"""
                    {"skills":[{"id":"{{PhaseDemand}}","revision":1,"triggers":[
                      {"id":"actual-end","window":"actualPhaseEnded","subject":"owner","optional":false,
                       "effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
                    """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [PhaseDemand] = new { name = "实际阶段观测", description = "阶段结束后摸一张牌。" } } }));
                builder.AddSkill(new(PhaseDemand, "实际阶段观测", "阶段结束后摸一张牌。")
                    { Program = demand.Programs[PhaseDemand], ProgramPresentation = demand.Presentations[PhaseDemand] });
            }
            // Seed 47 reveals physical cards 24, 30 and 3 after the opening hands and first Draw.
            builder.AddDeck(new(Deck, twoRedForecast ? "两红星卜实体牌" : "红色实体牌", 4, 2, []) { PhysicalCards = Enumerable.Range(1, 48)
                .Select(id => new ContentDeckPhysicalCard("standard:bagua", twoRedForecast && id == BlackForecastCard ? Suit.Club : Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "星卜原始亮牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}

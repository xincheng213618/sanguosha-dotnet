using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class HappyShuangshuNativeDrawChecks
{
    private const string Sisters = "happy:shen-da-qiao-xiao-qiao", Shuangshu = "happy:shuangshu";
    private const string Peer = "fixture:shuangshu-peer", Watch = "fixture:shuangshu-native-draw", Deck = "fixture:shuangshu-draw-deck";
    private const string PhaseWatch = "fixture:shuangshu-draw-phase-starting";

    internal static void InsertedAndNaturalDrawEachOpenTheirNativeAttemptCold()
    {
        const int owner = 0;
        var (registry, game) = CreateScene(Watch);

        PendingDecision? Prompt() => game.CreateSnapshot(owner).PendingDecision;
        bool Watching() => Prompt() is { PlayerSeat: owner, SkillPrompt.SkillId: Watch };
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        ProgramPhaseScheduledEvent[] Schedule() => Facts<ProgramPhaseScheduledEvent>().Where(e => e.SkillId == Shuangshu).ToArray();
        CardMovementRecord[] Draws(int turn) => game.CardMovements.Where(move => move.TurnNumber == turn &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner)).ToArray();
        void Submit(GameCommand command) => SubmitCommand(game, command);
        void Reach(Func<bool> ready) => ReachExpected(game, ready);
        void ContinueDraw() => ContinueObserver(game);
        NativeDrawAttemptFrame CurrentAttempt(long? scheduleFrameId = null)
        {
            var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
            var producer = game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single(frame => frame.Id == attempt.Context.ParentFrameId);
            Need(attempt.Return.Kind == NativeDrawAttemptReturnKind.DrawPhase &&
                attempt.Context is { RecipientSeat: owner, RequestedCount: 2, ActualPhase.Kind: ActualDiscardRecoveryPhaseKind.Draw } &&
                producer.NativeDraw?.ActiveAttemptId == attempt.Id && attempt.Context.Reason == CardMoveReasons.Draw &&
                producer.IsExtra == scheduleFrameId.HasValue && producer.PhaseOccurrenceFrameId == scheduleFrameId,
                "The visible choice is inside an original actual Draw phase's paid two-card request.");
            return attempt;
        }

        Need(registry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.NativeDrawStarting),
            "The fixture registers the native draw attempt observer.");
        Submit(new StartGameCommand());
        Reach(Watching);
        Need(Facts<ProgramBindingStartedEvent>().All(entry => entry.Window != SkillProgramTriggerWindow.DrawPhaseStarting),
            "The living fixture participants enter the draw request without executing a DrawPhaseStarting binding.");
        var turn = Facts<TurnStartedEvent>().Single().TurnNumber;
        var initialSchedule = Schedule();
        Need(initialSchedule.Length == 1 && initialSchedule[0] is { Started: true, OwnerSeat: owner, Phase: TurnPhase.Draw } && Draws(turn).Length == 0,
            $"The first native draw watcher must open inside the still-running Shuangshu insertion before any cards move. " +
            $"Schedule started flags: [{string.Join(", ", initialSchedule.Select(entry => entry.Started))}]. Physical draws already paid: {Draws(turn).Length}.");
        var inserted = CurrentAttempt(initialSchedule[0].FrameId);
        var promptId = Prompt()!.PromptId;
        game = Cold(game, registry);
        Need(Watching() && Prompt()?.PromptId == promptId && CurrentAttempt(initialSchedule[0].FrameId) == inserted && Schedule().SequenceEqual(initialSchedule),
            "Cold replay preserves the original native request while the Shuangshu phase is still scheduled.");
        ContinueDraw();
        Reach(Watching);

        var natural = CurrentAttempt();
        var firstCards = Draws(turn);
        var schedule = Schedule();
        Need(schedule is [var started, { Started: false } ended] && started == initialSchedule[0] && ended.FrameId == started.FrameId &&
            natural.Id != inserted.Id && natural.Context.ParentFrameId != inserted.Context.ParentFrameId &&
            natural.Context.ActualPhase!.Token != inserted.Context.ActualPhase!.Token &&
            natural.Context.ActualTurnNumber == turn && inserted.Context.ActualTurnNumber == turn &&
            firstCards.Length == 2 && firstCards.All(move => move.Sequence > inserted.Context.SequenceBefore && move.Sequence <= natural.Context.SequenceBefore),
            "The two original Shuangshu cards are paid before the independent natural Draw request begins in the same actual turn.");
        ContinueDraw();
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner });

        var draws = Draws(turn);
        var history = game.Events.Select(entry => entry.Payload).ToArray();
        var preparation = Facts<ActualDiscardRecoveryPhaseStartedEvent>().Single(e => e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation);
        var resumed = Facts<ActualDiscardRecoveryPhaseRestoredEvent>().Single(e => e.ProducerFrameId == initialSchedule[0].FrameId);
        var attempts = Facts<NativeDrawAttemptStartedEvent>();
        Need(attempts.Select(e => e.Context).SequenceEqual([inserted.Context, natural.Context]) &&
            Facts<NativeDrawAttemptDecidedEvent>().SequenceEqual([
                new(inserted.Id, inserted.Context.ParentFrameId, owner, 2, false),
                new(natural.Id, natural.Context.ParentFrameId, owner, 2, false)]) &&
            Facts<ActualDrawPhaseCompletedEvent>().SequenceEqual([
                new(inserted.Context.ParentFrameId, owner, turn, IsExtra: true, Skipped: false, DrawCount: 2),
                new(natural.Context.ParentFrameId, owner, turn, IsExtra: false, Skipped: false, DrawCount: 2)]) &&
            draws.Take(2).SequenceEqual(firstCards) && draws.Length == 4 && draws.Skip(2).All(move => move.Sequence > natural.Context.SequenceBefore) &&
            draws.Select(move => move.CardId).Distinct().Count() == 4 && game.CreateSnapshot(owner).Players[owner].HandCount == 4,
            "Each native request resolves once and each actual phase delivers exactly two distinct physical cards.");
        Need(resumed.Phase == preparation.Phase && Array.IndexOf(history, schedule[1]) < Array.IndexOf(history, resumed) &&
            Array.IndexOf(history, resumed) < Array.IndexOf(history, attempts[1]) && game.State.TurnNumber == turn,
            "Shuangshu returns to the original Preparation before the natural Draw and original Play proceed.");
    }

    internal static void DrawObserversPreserveInsertedAndNaturalPhysicalCardsCold()
    {
        var direct = Run(observer: null);
        var unheld = Run(Watch, holdObserver: false);
        (bool IsExtra, bool Skipped, int DrawCount)[] expected = [(true, false, 2), (false, false, 2)];
        Need(direct.Completions.SequenceEqual(expected) && unheld.Completions.SequenceEqual(expected),
            $"Shuangshu must complete one extra Draw then one natural Draw even without a native listener. " +
            $"No listener: {string.Join(", ", direct.Completions)}. Unheld listener: {string.Join(", ", unheld.Completions)}.");
        foreach (var observed in new[] { unheld, Run(PhaseWatch) })
        {
            Need(direct.Phases.SequenceEqual(observed.Phases) && direct.Cards.SequenceEqual(observed.Cards) &&
                direct.Hand.SequenceEqual(observed.Hand) && direct.Completions.SequenceEqual(observed.Completions),
                "The same setup delivers the same physical cards in both actual Draw phases with either observer or neither.");
        }

        static (ActualDiscardRecoveryPhaseStartedEvent[] Phases,
            (int CardId, CardKind CardKind, CardLocation From, CardLocation To, CardMoveReason Reason)[] Cards,
            int[] Hand, (bool IsExtra, bool Skipped, int DrawCount)[] Completions) Run(string? observer, bool holdObserver = true)
        {
            var (registry, game) = CreateScene(observer, trackActualPhases: true, holdObserver: holdObserver);
            var native = observer == Watch;
            var phaseWatcher = observer == PhaseWatch;
            Need(registry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.NativeDrawStarting) == native &&
                registry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.ActualPhaseEnded),
                "All fixtures observe actual completions and only one registers the unheld native listener.");
            SubmitCommand(game, new StartGameCommand());
            if (phaseWatcher)
            {
                for (var index = 0; index < 2; index++)
                {
                    ReachExpected(game, () => game.CreateSnapshot(0).PendingDecision?.SkillPrompt?.SkillId == observer);
                    var before = game.CreateSnapshot(0);
                    Need(before.PendingDecision?.PlayerSeat == 0 && game.State.Phase == TurnPhase.Draw && Draws(game, game.State.TurnNumber).Length == index * 2 &&
                        !game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Any(),
                        "The real observer sees the original inserted or ordinary Draw before that phase moves cards.");
                    if (index == 0)
                    {
                        game = Cold(game, registry);
                        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(before),
                            "Cold replay retains the same original draw observer and pending choice.");
                    }
                    ContinueObserver(game);
                }
            }
            ReachExpected(game, () => game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            if (!phaseWatcher)
                game = Cold(game, registry);

            var turn = game.State.TurnNumber;
            var history = game.Events.Select(entry => entry.Payload).ToArray();
            var phases = history.OfType<ActualDiscardRecoveryPhaseStartedEvent>().Where(entry =>
                entry.Phase.TurnNumber == turn && entry.Phase.ActorSeat == 0).ToArray();
            var preparation = phases.Single(entry => entry.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation);
            var judgment = phases.Single(entry => entry.Phase.Kind == ActualDiscardRecoveryPhaseKind.Judgment);
            var play = phases.Single(entry => entry.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play);
            var actualDraws = phases.Where(entry => entry.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).ToArray();
            var schedule = history.OfType<ProgramPhaseScheduledEvent>().Where(entry => entry.SkillId == Shuangshu).ToArray();
            var restored = history.OfType<ActualDiscardRecoveryPhaseRestoredEvent>().Single(entry => entry.ProducerFrameId == schedule[0].FrameId);
            var draws = Draws(game, turn);
            var hand = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
            Need(schedule is [{ OwnerSeat: 0, Phase: TurnPhase.Draw, Started: true } starts, { Started: false } ends] && starts.FrameId == ends.FrameId &&
                actualDraws is [var inserted, var natural] && inserted.Phase.Token != natural.Phase.Token && restored.Phase == preparation.Phase &&
                Array.IndexOf(history, starts) < Array.IndexOf(history, inserted) && Array.IndexOf(history, inserted) < Array.IndexOf(history, ends) &&
                Array.IndexOf(history, ends) < Array.IndexOf(history, restored) && Array.IndexOf(history, restored) < Array.IndexOf(history, judgment) &&
                Array.IndexOf(history, judgment) < Array.IndexOf(history, natural) && Array.IndexOf(history, natural) < Array.IndexOf(history, play) &&
                draws.Length == 4 && draws.Select(move => move.CardId).Distinct().Count() == 4 &&
                draws.All(move => move.Reason == CardMoveReasons.Draw) &&
                draws.Take(2).All(move => move.Sequence > inserted.MovementSequenceBefore && move.Sequence <= natural.MovementSequenceBefore) &&
                draws.Skip(2).All(move => move.Sequence > natural.MovementSequenceBefore) && hand.Order().SequenceEqual(draws.Select(move => move.CardId).Order()),
                "Each path returns the original schedule to Preparation and deals two distinct entities in each actual Draw before ordinary Play.");
            Need(history.OfType<ProgramBindingStartedEvent>().Count(entry => entry.Window == SkillProgramTriggerWindow.DrawPhaseStarting) == (phaseWatcher ? 2 : 0) &&
                !history.OfType<NativeDrawAttemptStartedEvent>().Any() && !history.OfType<NativeDrawAttemptDecidedEvent>().Any() &&
                !game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Any(),
                "None of the three paths opens a native attempt, and only the DrawPhaseStarting observer visits both phases.");
            var completions = history.OfType<ActualDrawPhaseCompletedEvent>().Where(entry => entry.OwnerSeat == 0 && entry.ActualTurnNumber == turn)
                .Select(entry => (entry.IsExtra, entry.Skipped, entry.DrawCount)).ToArray();
            return (phases, draws.Select(move => (move.CardId, move.CardKind, move.From, move.To, move.Reason)).ToArray(), hand, completions);
        }
    }

    private static (ContentRegistry Registry, GameEngine Game) CreateScene(string? observer, bool trackActualPhases = false, bool holdObserver = true)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(observer, trackActualPhases));
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        string[] extraSkills = holdObserver && observer is not null ? [observer] : [];
        var setup = new ShanheBattleSetup(47, 1, "shuangshu-native-draw", Sisters, 3, 0, 0, 0, 0, extraSkills,
            [], [], [peer, peer], [peer], new string('a', 64)) { DefaultGodFactionId = "wu", ExtraSkillSlots = extraSkills.Length };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        return (registry, game);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry registry) =>
        GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);

    private static CardMovementRecord[] Draws(GameEngine game, int turn) => game.CardMovements.Where(move => move.TurnNumber == turn &&
        move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)).ToArray();

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void SubmitCommand(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The Shuangshu fixture command was rejected.");
    }

    private static void ReachExpected(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 60 && !ready(); step++)
        {
            var prompt = game.CreateSnapshot(0).PendingDecision;
            Need(prompt is null && game.State.Winner == Winner.None,
                $"Unexpected Shuangshu boundary {game.State.TurnNumber}/{game.State.Phase}: {prompt?.Kind}/{prompt?.SkillPrompt?.SkillId}.");
            SubmitCommand(game, new AdvanceOneStepCommand(game.Revision));
        }
        Need(ready(), "The actual Shuangshu turn did not reach its expected native draw or Play.");
    }

    private static void ContinueObserver(GameEngine game)
    {
        var prompt = game.CreateSnapshot(0).PendingDecision!;
        var proceed = prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        SubmitCommand(game, new AnswerPromptCommand(0, prompt.PromptId, proceed.Id, game.Revision));
    }

    private sealed class Fixture(string? observer, bool trackActualPhases) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-happy-shuangshu-native-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Sisters);
            if (trackActualPhases)
                CharacterFixtureContent.Register(builder, "ol:qu-huang");
            if (observer is not null)
            {
                var window = observer == Watch ? "nativeDrawStarting" : "drawPhaseStarting";
                var catalog = SkillProgramCatalog.Load($$"""
                    {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{observer}}","revision":1,"triggers":[
                      {"id":"actual-native-draw","window":"{{window}}","subject":"owner","optional":false,"isSkillInvocation":false,
                       "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                    """, $$"""
                    {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{observer}}":{"name":"观察双姝摸牌",
                     "description":"在实际摸牌前暂停。","optionLabels":{"continue":"继续"} } } }
                    """);
                builder.AddSkill(new(observer, "观察双姝摸牌", "在实际摸牌前暂停。")
                    { Program = catalog.Programs[observer], ProgramPresentation = catalog.Presentations[observer] });
            }
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "双姝固定闪牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "双姝实际摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Sisters, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class HappyShuangshuNativeDrawChecks
{
    private const string Sisters = "happy:shen-da-qiao-xiao-qiao", Shuangshu = "happy:shuangshu";
    private const string Peer = "fixture:shuangshu-peer", Watch = "fixture:shuangshu-native-draw", Deck = "fixture:shuangshu-draw-deck";

    internal static void InsertedAndNaturalDrawEachOpenTheirNativeAttemptCold()
    {
        const int owner = 0;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(47, 1, "shuangshu-native-draw", Sisters, 3, 0, 0, 0, 0, [Watch],
            [], [], [peer, peer], [peer], new string('a', 64)) { DefaultGodFactionId = "wu", ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(owner).PendingDecision;
        bool Watching() => Prompt() is { PlayerSeat: owner, SkillPrompt.SkillId: Watch };
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        ProgramPhaseScheduledEvent[] Schedule() => Facts<ProgramPhaseScheduledEvent>().Where(e => e.SkillId == Shuangshu).ToArray();
        CardMovementRecord[] Draws(int turn) => game.CardMovements.Where(move => move.TurnNumber == turn &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner)).ToArray();
        static void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Shuangshu fixture command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 60 && !ready(); step++)
            {
                Need(Prompt() is null && game.State.Winner == Winner.None,
                    $"Unexpected Shuangshu boundary {game.State.TurnNumber}/{game.State.Phase}: {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The actual Shuangshu turn did not reach its expected native draw or Play.");
        }
        void ContinueDraw()
        {
            var prompt = Prompt()!;
            var proceed = prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, proceed.Id, game.Revision));
        }
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
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
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

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-happy-shuangshu-native-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Sisters);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"actual-native-draw","window":"nativeDrawStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{"name":"观察双姝摸牌",
                 "description":"在实际摸牌前暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, "观察双姝摸牌", "在实际摸牌前暂停。")
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "双姝固定闪牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "双姝实际摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Sisters, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

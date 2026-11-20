using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static class OlZhangShiPingHongjiNativeDrawChecks
{
    private const string Issuer = "fixture:hongji-issuer", Beneficiary = "fixture:hongji-recipient", Peer = "fixture:hongji-peer";
    private const string Hongji = "ol:hongji", Watch = "fixture:hongji-native-draw", Deck = "fixture:hongji-draw-deck";

    internal static void ForeignPromiseReturnsThroughSecondNativeDrawCold()
    {
        const int issuer = 0, recipient = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(47, 1, "hongji-foreign-native-draw", Issuer, 4, 0, 0, 0, 0,
            [], [], [], [peer, peer], [new(Beneficiary) { StartingHand = [] }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = issuer, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt(int seat = issuer) => game.CreateSnapshot(seat).PendingDecision;
        bool Watching() => Prompt(recipient) is { PlayerSeat: recipient, SkillPrompt.SkillId: Watch };
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        ProgramPhaseScheduledEvent[] Schedules() => Facts<ProgramPhaseScheduledEvent>().Where(e => e.SkillId == Hongji).ToArray();
        ProgramPhaseSchedule ActiveSchedule() => ((IProgramPhaseScheduleState)game.GetRuntimeModule<CoreProgramPhaseSchedulingRuntimePortsRuntime>()).Current ??
            throw new InvalidOperationException("Hongji lost its original detached phase schedule.");
        CardMovementRecord[] Draws(int turn) => game.CardMovements.Where(move => move.TurnNumber == turn &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(recipient) && move.Reason == CardMoveReasons.Draw).ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, actor {game.State.CurrentSeat}, " +
                    $"issuer prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}, recipient prompt {Prompt(recipient)?.Kind}/{Prompt(recipient)?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual Hongji fixture command was rejected.");
        }
        bool Has(string key, string value) => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault(key) == value) == true;
        void Choose(string key, string value)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(option => option.Parameters.GetValueOrDefault(key) == value);
            Submit(new AnswerPromptCommand(issuer, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready, bool skipOwnOffer = false)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                if (skipOwnOffer && game.State.CurrentSeat == issuer && Prompt()?.SkillPrompt?.SkillId == Hongji && Has("program-action", "skip"))
                    Choose("program-action", "skip");
                else
                {
                    Need(Prompt() is null && game.State.Winner == Winner.None, "The game reached an unexpected public boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Need(ready(), "The actual Hongji fixture did not reach the expected preparation or Draw boundary.");
        }
        NativeDrawAttemptFrame CurrentAttempt(long? extraFrameId = null)
        {
            var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
            var parent = game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single(frame => frame.Id == attempt.Context.ParentFrameId);
            Need(attempt.Return.Kind == NativeDrawAttemptReturnKind.DrawPhase && attempt.Context.Reason == CardMoveReasons.Draw &&
                attempt.Context is { RecipientSeat: recipient, RequestedCount: 2, ActualPhase.Kind: ActualDiscardRecoveryPhaseKind.Draw } &&
                parent.OwnerSeat == recipient && parent.NativeDraw?.ActiveAttemptId == attempt.Id &&
                parent.IsExtra == extraFrameId.HasValue && parent.PhaseOccurrenceFrameId == extraFrameId,
                "The native request belongs to the foreign recipient's original actual Draw producer.");
            return attempt;
        }

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: issuer }, skipOwnOffer: true);
        Need(game.CreateSnapshot(recipient).Players[recipient] is { GeneralId: Beneficiary, HandCount: 0 },
            "The watched foreign recipient has the minimum hand before its first actual preparation.");
        Submit(new EndPlayPhaseCommand(issuer, game.Revision, Prompt()!.PromptId));
        Reach(() => game.State.CurrentSeat == recipient && Prompt()?.SkillPrompt?.SkillId == Hongji && Has("program-action", "activate"));
        Choose("program-action", "activate");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Hongji && Has("branch", "draw"));
        Need(Prompt()!.Choices.Single(choice => choice.Parameters.GetValueOrDefault("branch") == "draw").Targets.SequenceEqual([recipient]),
            "The human issuer explicitly commits to the original foreign recipient's next Draw end.");
        Choose("branch", "draw");
        Reach(Watching);
        var promise = Facts<ZhangShiPingHongjiGrantedEvent>().Single();
        var natural = CurrentAttempt();
        var preparation = Facts<ProgramBindingStartedEvent>().Single(e => e.FrameId == promise.FrameId);
        Need(promise is { Source: { OwnerSeat: issuer, SkillId: Hongji, BindingId: "lifecycle" }, RecipientSeat: recipient, Phase: TurnPhase.Draw } &&
            preparation is { OwnerSeat: issuer, SkillId: Hongji, BindingId: "foreign-preparation-promise", Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } &&
            preparation.SkillInstanceId == promise.Source.SkillInstanceId,
            $"The original foreign preparation binding issues the recipient's Draw promise. Promise {promise}, binding {preparation}.");
        var drawnBeforeReturn = Draws(promise.ActualTurn).Length;
        var consumedBeforeReturn = Facts<ZhangShiPingHongjiConsumedEvent>().Length;
        var scheduledBeforeReturn = Schedules().Length;
        Need(promise.ActualTurn == natural.Context.ActualTurnNumber && drawnBeforeReturn == 0 && consumedBeforeReturn == 0 && scheduledBeforeReturn == 0,
            $"The promise waits through the foreign normal Draw's original request. Promise turn {promise.ActualTurn}, " +
            $"request turn {natural.Context.ActualTurnNumber}, cards paid {drawnBeforeReturn}, consumed promises {consumedBeforeReturn}, schedules {scheduledBeforeReturn}.");
        Submit(new AdvanceOneStepCommand(game.Revision));
        Reach(() => Watching() || Facts<ZhangShiPingHongjiReturnedEvent>().Length > 0);

        var consumed = Facts<ZhangShiPingHongjiConsumedEvent>().Single();
        var schedule = Schedules();
        var naturalCards = Draws(promise.ActualTurn);
        Need(Watching() && schedule is [{ Started: true }] && naturalCards.Length == 2 &&
            Facts<NativeDrawAttemptStartedEvent>().Count(e => e.Context.RecipientSeat == recipient) == 2,
            $"Hongji must open the second native draw watcher before paying the inserted Draw. Schedule started flags " +
            $"[{string.Join(", ", schedule.Select(entry => entry.Started))}], beneficiary native requests " +
            $"{Facts<NativeDrawAttemptStartedEvent>().Count(e => e.Context.RecipientSeat == recipient)}, physical draws paid {naturalCards.Length}.");
        var inserted = CurrentAttempt(consumed.FrameId);
        var active = ActiveSchedule();
        Need(consumed.PromiseFrameId == promise.FrameId && consumed.Source == promise.Source && consumed.RecipientSeat == recipient &&
            consumed.ActualTurn == promise.ActualTurn && schedule[0].FrameId == consumed.FrameId && schedule[0].OwnerSeat == issuer &&
            active.ParentFrame is { OwnerSeat: recipient, Window: SkillProgramTriggerWindow.DrawPhaseEnded } ending &&
            ending.Id == consumed.ParentWindowId && ending.ResumeDrawPhaseObligationFrameId == natural.Context.ParentFrameId &&
            inserted.Context.ActualTurnNumber == natural.Context.ActualTurnNumber && inserted.Context.ParentFrameId != natural.Context.ParentFrameId &&
            inserted.Context.ActualPhase!.Token != natural.Context.ActualPhase!.Token && Facts<ZhangShiPingHongjiReturnedEvent>().Length == 0 &&
            Facts<ProgramBindingStartedEvent>().All(e => e.Window != SkillProgramTriggerWindow.DrawPhaseStarting),
            "There are no DrawPhaseStarting bindings. The consumed foreign grant owns a new phase for the recipient while the issuer's original return is pending.");

        var promptId = Prompt(recipient)!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Prompt(recipient)?.PromptId == promptId && CurrentAttempt(consumed.FrameId) == inserted &&
            Facts<ZhangShiPingHongjiConsumedEvent>().Single() == consumed && Schedules().SequenceEqual(schedule) &&
            JsonSerializer.Serialize(ActiveSchedule()) == JsonSerializer.Serialize(active),
            "Cold restore retains the exact inserted request and the single consumed foreign promise.");
        Submit(new AdvanceOneStepCommand(game.Revision));
        Reach(() => game.State is { CurrentSeat: recipient, Phase: TurnPhase.Play });

        var returned = Facts<ZhangShiPingHongjiReturnedEvent>().Single();
        var restoration = Facts<ActualDiscardRecoveryPhaseRestoredEvent>().Single(e => e.ProducerFrameId == consumed.FrameId);
        var binding = Facts<ProgramBindingResolvedEvent>().Single(e => e.FrameId == consumed.FrameId && e.SkillId == Hongji && e.BindingId == "issued-draw-phase");
        var draws = Draws(promise.ActualTurn);
        Need(returned == new ZhangShiPingHongjiReturnedEvent(promise.FrameId, consumed.FrameId, promise.ActualTurn, recipient, TurnPhase.Draw) &&
            restoration.Phase == natural.Context.ActualPhase && binding is { OwnerSeat: issuer, Window: SkillProgramTriggerWindow.DrawPhaseEnded, Completed: true } &&
            Facts<ActualDrawPhaseCompletedEvent>().Any(e => e.FrameId == natural.Context.ParentFrameId && e is { Skipped: false, DrawCount: 2 }) &&
            Schedules() is [var opened, { Started: false } closed] && opened == schedule[0] && closed.FrameId == opened.FrameId,
            "The inserted Draw returns to the exact original foreign DrawEnded window and completes the consumed grant once.");
        Need(draws.Length == 4 && draws.Take(2).SequenceEqual(naturalCards) && draws.Select(move => move.CardId).Distinct().Count() == 4 &&
            draws.Skip(2).All(move => move.Sequence > inserted.Context.SequenceBefore) &&
            Facts<NativeDrawAttemptDecidedEvent>().Where(e => e.RecipientSeat == recipient).SequenceEqual([
                new(natural.Id, natural.Context.ParentFrameId, recipient, 2, false),
                new(inserted.Id, inserted.Context.ParentFrameId, recipient, 2, false)]),
            "The original and inserted Draw each deliver two separate real entities before the recipient enters Play.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-hongji-native-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:zhang-shi-ping");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"actual-native-draw","window":"nativeDrawStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "观察鸿济摸牌", description = "在真实摸牌前暂停。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
                }));
            builder.AddSkill(new(Watch, "观察鸿济摸牌", "在真实摸牌前暂停。")
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Issuer, "鸿济拥有者", "supporter", Hongji, "shu", 4));
            builder.AddGeneral(new(Beneficiary, "鸿济受领者", "supporter", Watch, "wei", 4));
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "鸿济固定闪牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "异席鸿济实际摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Issuer, Beneficiary, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

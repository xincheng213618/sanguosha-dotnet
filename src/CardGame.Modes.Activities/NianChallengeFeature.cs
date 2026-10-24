using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Round forms, elapsed-time commands and shared-revival qualification use the native event ledger.</summary>
internal sealed class NianChallengeFeature : INianChallengeFeature, IMatchModeCommandHandler,
    IMatchModeRuntimeHookProvider, IMatchModeRuleObserverProvider
{
    private const int TimeLimitMilliseconds = 30 * 60 * 1000;
    private const string ReviveSkill = "nian:revive";
    private readonly ContentModeDefinition _mode;
    private readonly INianChallengeHost _native;
    private readonly IModeSetupRandom _random;
    private IModeEncounterHost Host => _native.Encounter;

    internal NianChallengeFeature(ContentModeDefinition mode, INianChallengeHost native, IModeSetupRandom random)
    {
        _mode = mode;
        _native = native;
        _random = random;
        CommandTypes = Array.AsReadOnly<Type>([typeof(TickNianClockCommand)]);
        RuntimeHooks = Array.AsReadOnly<ModeRuntimeHookRegistration>(
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 27930, TryAdvanceRevivalAi)]);
        RuleObservers = Array.AsReadOnly<ModeRuleObserverRegistration>(
            [ModeRuleObserverRegistration.ForEvent<RoundStartedEvent>(ProgramRulePhase.AfterEvent, 27900, ChangeFormForRound)]);
    }

    public IReadOnlyList<Type> CommandTypes { get; }
    public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }
    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    private static int BossSeat(ModeEncounterFacts facts) => (Math.Max(0, facts.HumanSeat) + 3) % facts.PlayerCount;
    public bool ClockRunning => _native.Started && Host.CaptureFacts() is { SetupComplete: true, HasWinner: false };
    public int RemainingMilliseconds => Math.Max(0, TimeLimitMilliseconds - Host.History<NianClockAdvancedEvent>()
        .Select(entry => entry.TotalMilliseconds).DefaultIfEmpty(0).Last());
    public int UsedRevives => Host.History<NianRevivalPaidEvent>().Count;
    public int SharedRevives => _mode.NianActivity!.SharedRevives;

    public bool IsAlliedSeat(int seat)
    {
        var facts = Host.CaptureFacts();
        return IsAlliedSeat(facts, seat);
    }

    private bool IsAlliedSeat(ModeEncounterFacts facts, int seat) =>
        seat >= 0 && seat < facts.PlayerCount && seat != BossSeat(facts) &&
            facts.Players[seat].TeamId == _mode.BossActivity!.PlayerTeamId;

    public IReadOnlyList<int> DeadAllies(int ownerSeat)
    {
        var facts = Host.CaptureFacts();
        if (!IsAlliedSeat(facts, ownerSeat) || !facts.Players[ownerSeat].IsAlive || UsedRevives >= SharedRevives) return [];
        var died = Host.History<PlayerDiedEvent>().Select(e => e.VictimSeat).ToHashSet();
        var participants = _native.Participants;
        return Array.AsReadOnly(facts.Players.Where(player => player.Seat != ownerSeat && IsAlliedSeat(facts, player.Seat) &&
                !player.IsAlive && participants[player.Seat].GeneralSelected && player.MaxHp > 0 &&
                participants[player.Seat].HasHpValue && died.Contains(player.Seat))
            .Select(player => player.Seat).Order().ToArray());
    }

    public CommandResult Submit(GameCommand command)
    {
        if (command is not TickNianClockCommand clock) throw new InvalidOperationException("Unsupported Nian command.");
        if (!ClockRunning)
            return _native.Reject(CommandErrorCode.IllegalAction, "The Nian clock runs only during a started, fully selected challenge.");
        if (clock.ElapsedMilliseconds is <= 0 or > TimeLimitMilliseconds)
            return _native.Reject(CommandErrorCode.IllegalAction, "Elapsed Nian time must be positive and no greater than the entire time limit.");
        return _native.Accept(() =>
        {
            var remaining = RemainingMilliseconds;
            var elapsed = Math.Min(clock.ElapsedMilliseconds, remaining);
            var total = TimeLimitMilliseconds - remaining + elapsed;
            Host.PublishFact(new NianClockAdvancedEvent(elapsed, total));
            if (total == TimeLimitMilliseconds) ExpireChallenge();
            Host.PublishState();
        });
    }

    private void ExpireChallenge()
    {
        // Paid state stays on its original frame until the native stack is retired.
        var judgments = _native.CaptureRevealedJudgments();
        Host.ClearDecision();
        Host.CancelResolutionTree(frame => new NianResolutionCancelledEvent(frame.FrameId, frame.Kind));
        Host.ClearTransientBatches();
        _native.CancelDeferredPhases();
        _native.SetWinnerForCleanup(_mode.BossActivity!.EnemyTeamId);
        Host.DiscardProcessingCards("activity.nian.timeout");
        foreach (var judgment in judgments)
            _native.RetireRevealedJudgment(judgment, "activity.nian.timeout-judgment");
        _native.ClearPendingCardMovements();
        Host.PublishFact(new NianClockExpiredEvent(TimeLimitMilliseconds));
        _native.PublishCurrentWinner();
        _native.CompleteGame();
    }

    private void ChangeFormForRound(RoundStartedEvent started)
    {
        var facts = Host.CaptureFacts();
        var boss = facts.Players[BossSeat(facts)];
        if (!facts.SetupComplete || started.RoundNumber < 2 || !boss.IsAlive) return;
        var position = (started.RoundNumber - 2) % 4;
        var cycleNumber = (started.RoundNumber - 2) / 4 + 1;
        if (position == 0)
        {
            var forms = _mode.NianActivity!.FormGeneralIds.Skip(1).ToList();
            _random.Shuffle(forms);
            Host.PublishFact(new NianFormCycleStartedEvent(cycleNumber, forms.ToArray()));
        }
        var cycle = Host.History<NianFormCycleStartedEvent>().Single(entry => entry.CycleNumber == cycleNumber);
        boss = Host.CaptureFacts().Players[BossSeat(facts)];
        var id = cycle.GeneralIds[position];
        Host.ReplaceCombatant(new(boss.Seat, boss.GeneralId, id, PreserveCurrentState: true,
            MaximumHpOverride: boss.MaxHp, HpOverride: boss.Hp));
        Host.PublishFact(new NianFormChangedEvent(started.RoundNumber, boss.Seat, boss.GeneralId, id, cycleNumber, position));
        Host.Log("NianFormChanged", $"第 {started.RoundNumber} 轮，年兽变为{Host.General(id).Name}，保留当前体力和区域状态。", boss.Seat);
    }

    private bool TryAdvanceRevivalAi()
    {
        var facts = Host.CaptureFacts();
        if (!facts.SetupComplete || facts.HasWinner || facts.PendingDecisionKind is not null ||
            facts.ResolutionCount != 0 || facts.Phase != TurnPhase.Play || !IsAlliedSeat(facts, facts.CurrentSeat) ||
            _native.Participants[facts.CurrentSeat].IsHuman || !facts.Players[facts.CurrentSeat].IsAlive) return false;
        if (!_native.TryExecuteProgramAction(facts.CurrentSeat, ReviveSkill, "revive", () =>
        {
            var participants = _native.Participants;
            return DeadAllies(facts.CurrentSeat).OrderByDescending(seat => participants[seat].IsHuman).ThenBy(seat => seat).First();
        })) return false;
        Host.PublishState();
        return true;
    }
}

using CardGame.Core;

namespace CardGame.Modes.Wushuang;

internal sealed partial class WushuangModeFeature : IMatchModeCommandHandler, IMatchModeOpeningBoundary,
    IMatchModeTurnIntermission, IMatchModeRuleObserverProvider, IMatchModeNumericRules,
    IMatchModeLifecycle, IMatchModeSnapshotProjector, IMatchModeNativeFrameProvider, IMatchModePromptAnswerProvider
{
    public int Order => 0;
    public IReadOnlyList<Type> CommandTypes { get; } = Array.AsReadOnly<Type>(
    [typeof(TickWushuangClockCommand), typeof(BuyWushuangOfferCommand), typeof(RerollWushuangShopCommand),
        typeof(UpgradeWushuangShopCommand), typeof(FinishWushuangShopCommand), typeof(ToggleWushuangShopLockCommand)]);

    public CommandResult Submit(GameCommand command) => command switch
    {
        TickWushuangClockCommand clock => SubmitClock(clock),
        BuyWushuangOfferCommand buy => SubmitBuyWushuangOffer(buy),
        RerollWushuangShopCommand reroll => SubmitRerollWushuangShop(reroll),
        UpgradeWushuangShopCommand upgrade => SubmitUpgradeWushuangShop(upgrade),
        FinishWushuangShopCommand finish => SubmitFinishWushuangShop(finish),
        ToggleWushuangShopLockCommand toggle => SubmitToggleWushuangShopLock(toggle),
        _ => throw new InvalidOperationException("An unregistered command reached the Wushuang feature.")
    };

    bool IMatchModeOpeningBoundary.TryBeginOpening() => TryBeginWushuangOpening();
    bool IMatchModeTurnIntermission.TryBeginIntermission(int endedSeat, bool skipped) => TryBeginWushuangShopping(endedSeat, skipped);
    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers => Array.AsReadOnly<ModeRuleObserverRegistration>(
    [ModeRuleObserverRegistration.ForEvent<GameStartedEvent>(ProgramRulePhase.AfterEvent, 61001, InitializeWushuangShops),
        ModeRuleObserverRegistration.ForEvent<TurnStartedEvent>(ProgramRulePhase.AfterEvent, 61002, ProgressWushuangEconomy),
        ModeRuleObserverRegistration.ForEvent<RoundStartedEvent>(ProgramRulePhase.AfterEvent, 61003, ProgressWushuangRound)]);

    public IReadOnlyList<ModeNativeFrameRoute> FrameRoutes => Array.AsReadOnly<ModeNativeFrameRoute>(
    [ModeNativeFrameRoute.For<WushuangIntermissionFrame>(0, ContinueWushuangIntermission,
        automatic: new(NativeFrameAutomaticPhase.BeforeOpening, 0))]);

    public IReadOnlyList<ModePromptAnswerRegistration> PromptAnswers => Array.AsReadOnly<ModePromptAnswerRegistration>(
        [new("wushuang-choice", AnswerWushuangChoice)]);

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query) =>
        WushuangNumericRuleContributions(seat, query);
    public RuleQueryEvaluation AdjustNumericEvaluation(int seat, RuleQueryEvaluation ordinary, SkillRuleQuery query) =>
        ApplyWushuangEnemyQuota(seat, ordinary, query);

    public PlayerSnapshot ProjectPlayer(PlayerSnapshot player, int viewerSeat, bool revealAll) =>
        player with { WushuangTacticIds = TacticIds(player.Seat) };
    public GameSnapshot ProjectGame(GameSnapshot game, int viewerSeat, bool revealAll) => game with
    {
        Wushuang = CreateWushuangSnapshot(viewerSeat) is { } shop
            ? shop with { RemainingTurns = Math.Max(0, _host.MaximumTurns - game.TurnNumber) } : null
    };

    public bool TryFinishAtTurnLimit(int maximumTurns)
    { CompleteAtLimit($"达到最大回合数 {maximumTurns}", false); return true; }

    private CommandResult SubmitClock(TickWushuangClockCommand command)
    {
        if (command.ActorSeat != -1)
            return _host.Reject(CommandErrorCode.InvalidActor, "无双计时只能由主机（座位-1）推进。");
        if (!IsWushuangClockRunning)
            return _host.Reject(CommandErrorCode.IllegalAction, "无双计时仅在选将完成后的未结束对局中推进。");
        if (command.ElapsedMilliseconds is <= 0 or > WushuangMode.TimeLimitMilliseconds)
            return _host.Reject(CommandErrorCode.IllegalAction, "计时增量必须为正数且不超过15分钟。");
        return _host.Accept(() =>
        {
            var elapsed = Math.Min(command.ElapsedMilliseconds, RemainingMilliseconds);
            ElapsedMilliseconds += elapsed;
            _host.PublishFact(new WushuangClockAdvancedEvent(elapsed, ElapsedMilliseconds));
            if (ElapsedMilliseconds == WushuangMode.TimeLimitMilliseconds) CompleteAtLimit("15分钟时限已到", true);
            _host.AdvanceState();
        });
    }

    private void CompleteAtLimit(string reason, bool clockExpired)
    {
        var survivors = _host.Players.Where(player => player.IsAlive).OrderByDescending(player => player.Hp).ToArray();
        if (survivors.Any(player => player.Hp is null))
            throw new InvalidOperationException("Wushuang survival adjudication requires one defined current HP value per living player.");
        var team = survivors.Length > 0 && (survivors.Length == 1 || survivors[0].Hp > survivors[1].Hp)
            ? survivors[0].TeamId : null;
        _host.RetirePendingMatch(team, (id, kind) => new WushuangResolutionCancelledEvent(id, kind),
            "mode.wushuang.timeout", "mode.wushuang.timeout-judgment");
        var description = reason + "，" + (team is null ? "双方当前体力相同，和局。" : "当前体力较高的一方获胜。");
        _host.Log("WushuangSurvivalLimit", description);
        if (clockExpired) _host.PublishFact(new WushuangClockExpiredEvent(ElapsedMilliseconds, null, description));
        _host.PublishCurrentWinner();
        _host.CompleteGame();
    }
}

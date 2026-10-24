using CardGame.Core;

namespace CardGame.Modes.Activities;

internal sealed class WenheBattleRulesFeature : IWenheBattleRulesFeature, IMatchModeRuleObserverProvider,
    IMatchModeRuntimeHookProvider, IMatchModeNumericRules, IMatchModeLifecycle, IMatchModeDeathRanking,
    IMatchModeSnapshotProjector, IMatchModeSkillVisibility, IMatchModeDamageRuleProvider
{
    public IReadOnlyList<CardKind> OrdinaryTrickKinds { get; } = Array.AsReadOnly<CardKind>(
        [CardKind.StarShift, CardKind.BeamSwap, CardKind.HandRedistribution, CardKind.WenheChaos]);
    private readonly ContentModeDefinition _mode;
    private readonly GameOptions _options;
    private readonly IModeBattleRulesHost _host;
    private readonly IWenheRuleFlowHost _flow;
    private bool Active => _mode.WenheActivity is not null;
    private bool Survival => _mode.ModeKind == ContentModeKind.Survival;

    internal WenheBattleRulesFeature(ContentModeDefinition mode, GameOptions options,
        IModeBattleRulesHost host, IWenheRuleFlowHost flow)
    {
        (_mode, _options, _host, _flow) = (mode, options, host, flow);
        RuleObservers = Active ? Array.AsReadOnly<ModeRuleObserverRegistration>(
            [ModeRuleObserverRegistration.ForEvent<RoundStartedEvent>(ProgramRulePhase.AfterEvent, 41000, ObserveRound)]) : [];
        RuntimeHooks = Survival ? Array.AsReadOnly<ModeRuntimeHookRegistration>(
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 41000, TryComplete)]) : [];
        DamageRules = Active ? Array.AsReadOnly<ModeDamageRuleRegistration>(
            [ModeDamageRuleRegistration.Additive(270, damage => SweepDamageBonus(damage.IsChainPropagation))]) : [];
    }

    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }
    public IReadOnlyList<ModeDamageRuleRegistration> DamageRules { get; }
    int IMatchModeNumericRules.Order => 30;
    int IMatchModeLifecycle.Order => 10;
    int IMatchModeSnapshotProjector.Order => 10;
    public WenheRuleKind? CurrentRule => Active ? _host.History<WenheRoundRuleChangedEvent>().LastOrDefault()?.Rule : null;
    public WenheRoundRuleSnapshot? RoundRuleSnapshot => Active && _host.History<WenheRoundRuleChangedEvent>().LastOrDefault() is { } fact
        ? new(fact.RoundNumber, fact.Rule) : null;
    public int? SurvivalRank(int seat) => Survival ? _host.History<WenhePlayerRankedEvent>().LastOrDefault(fact => fact.Seat == seat)?.Rank : null;
    public bool ChampionMarkUsed(int seat) => _host.History<WenheChampionMarkUsedEvent>().Any(fact => fact.Seat == seat);
    public int GoalRank(int seat) => seat == _options.HumanSeat ? _options.WenheAspirationRank ?? 1 : 1;
    public WenheAspirationSnapshot? Aspiration(int viewerSeat) => _mode.WenheActivity is { EnableAspirations: true } &&
        viewerSeat >= 0 && viewerSeat < _options.PlayerCount ? new(GoalRank(viewerSeat),
            _host.CaptureFacts().Players[viewerSeat].IsAlive && GoalRank(viewerSeat) == 1 && !ChampionMarkUsed(viewerSeat),
            ChampionMarkUsed(viewerSeat)) : null;

    public bool IsSkillVisible(int seat, string? skillId, int viewerSeat, bool revealAll) => !Active ||
        skillId != "wenhe:champion-mark" || revealAll || ChampionMarkUsed(seat) ||
        viewerSeat == seat && GoalRank(seat) == 1 || _host.HasIndependentSkillGrant(seat, skillId);

    public GameSnapshot ProjectGame(GameSnapshot snapshot, int viewerSeat, bool revealAll) => snapshot with
        { WenheRule = RoundRuleSnapshot, WenheAspiration = Aspiration(viewerSeat) };
    public PlayerSnapshot ProjectPlayer(PlayerSnapshot snapshot, int viewerSeat, bool revealAll) => !Survival ? snapshot : snapshot with
        { SurvivalRank = SurvivalRank(snapshot.Seat), WenheChampionMarkUsed = ChampionMarkUsed(snapshot.Seat) ? true : null };

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        if (_mode.WenheActivity is not { } activity) yield break;
        var amount = query switch { SkillRuleQuery.InitialHandSize => activity.PlayerTier >= 3 ? 1 : 0,
            SkillRuleQuery.DrawCount => activity.PlayerTier >= 4 ? 1 : 0, SkillRuleQuery.SlashLimit => activity.PlayerTier >= 3 ? 1 : 0, _ => 0 };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"wenhe:tier:{query}", SkillRuleOperation.Add, amount);
    }

    public void RecordDeathRank(long deathFrameId, int seat)
    {
        if (!Survival) return;
        if (SurvivalRank(seat) is not null) throw new InvalidOperationException("A Wenhe death cannot award its rank twice.");
        _host.PublishFact(new WenhePlayerRankedEvent(deathFrameId, seat, _host.CaptureFacts().Players.Count(player => player.IsAlive) + 1));
    }

    public bool TryCompleteMatch(ModeCompletionBoundary boundary) =>
        boundary is ModeCompletionBoundary.AfterCommand or ModeCompletionBoundary.AfterTurnEnded && TryComplete();

    private bool TryComplete()
    {
        if (!Survival) return false;
        var facts = _host.CaptureFacts();
        if (!facts.SetupComplete || !facts.StartingProgramsResolved || facts.HasWinner || facts.ResolutionCount != 0 ||
            facts.PendingDecisionKind is { } decision && decision != DecisionKind.PlayCard) return false;
        var living = facts.Players.Where(player => player.IsAlive).ToArray();
        if (living.Length > 1) return false;
        if (facts.PendingMovementBatches != 0 || facts.PendingHpChanges != 0)
        {
            _host.ClearDecision();
            _host.PublishState();
            facts = _host.CaptureFacts();
            if (facts.ResolutionCount != 0 || facts.PendingMovementBatches != 0 || facts.PendingHpChanges != 0) return true;
            living = facts.Players.Where(player => player.IsAlive).ToArray();
            if (living.Length > 1) return true;
        }
        var winnerSeat = living.SingleOrDefault()?.Seat;
        var winner = winnerSeat is null ? Winner.Draw : Winner.SoleSurvivor;
        _host.SetWinner(winner, seat: winnerSeat);
        if (winnerSeat is { } seat && SurvivalRank(seat) is null) _host.PublishFact(new WenhePlayerRankedEvent(0, seat, 1));
        _host.PublishFact(new WinnerDeterminedEvent(winner) { WinnerSeat = winnerSeat });
        _host.CompleteGame();
        return true;
    }

    private void ObserveRound(RoundStartedEvent round)
    {
        var rule = round.RoundNumber == 1 ? WenheRuleKind.Chaos : (WenheRuleKind)_host.NextRandom(7);
        _host.PublishFact(new WenheRoundRuleChangedEvent(round.RoundNumber, rule));
        _host.Log("WenheRule", $"第{round.RoundNumber}轮规则：{RuleName(rule)}。", round.ActorSeat);
    }

    private static string RuleName(WenheRuleKind rule) => rule switch { WenheRuleKind.Chaos => "乱武", WenheRuleKind.Bounty => "重赏",
        WenheRuleKind.LastStand => "破釜沉舟", WenheRuleKind.Equipment => "横刀跃马", WenheRuleKind.Sweep => "横扫千军",
        WenheRuleKind.Famine => "饿莩载道", WenheRuleKind.Poison => "宴安鸩毒", _ => throw new ArgumentOutOfRangeException(nameof(rule)) };
    public int SweepDamageBonus(bool isChainPropagation) => CurrentRule == WenheRuleKind.Sweep && !isChainPropagation ? 1 : 0;
    public int? PoisonRound => Active && _host.CaptureFacts() is { SetupComplete: true, HasWinner: false } facts &&
        CurrentRule == WenheRuleKind.Poison ? facts.RoundNumber : null;
    public bool IsPhysicalPoison(CardKind kind, int cardId, bool alive, bool inOwnerHand) => Active &&
        CurrentRule == WenheRuleKind.Poison && kind == CardKind.Peach && cardId > 0 && alive && inOwnerHand;
    public bool AllowsActivation(string skillId, int ownerSeat) => skillId switch
    {
        "wenhe:chaos-resolution" => false,
        "wenhe:champion-mark" => _mode.WenheActivity is { EnableAspirations: true } && GoalRank(ownerSeat) == 1 && !ChampionMarkUsed(ownerSeat),
        _ => true
    };
    public bool HasDeadOwnerEnding(int actualOwner) => Active && CurrentRule == WenheRuleKind.Equipment &&
        !_host.CaptureFacts().Players[actualOwner].IsAlive && _host.CaptureFacts().Players.Any(player => player.IsAlive);
    public bool IsDeadOwnerEndingCandidate(string skillId, string bindingId, int candidateOwner, int actualOwner) =>
        HasDeadOwnerEnding(actualOwner) && skillId == "wenhe:rules" && bindingId == "turn-end-rule" &&
        candidateOwner == _host.CaptureFacts().Players.Where(player => player.IsAlive).Min(player => player.Seat);

    public bool AllowsTrigger(ModeWenheTriggerFacts trigger)
    {
        if (trigger.SkillId != "wenhe:rules") return true;
        if (!Active) return false;
        if (trigger.Window is null) return true;
        var facts = _host.CaptureFacts();
        return trigger.Window switch
        {
            SkillProgramTriggerWindow.RoundStarting => trigger.OwnerSeat == facts.CurrentSeat && CurrentRule == WenheRuleKind.Chaos &&
                !_host.History<WenheRuleFlowIssuedEvent>().Any(fact => fact.RoundNumber == facts.RoundNumber && fact.Operation == SkillProgramEffectOp.ResolveWenheRoundStart),
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow => trigger.OwnerSeat == facts.CurrentSeat && CurrentRule == WenheRuleKind.LastStand,
            SkillProgramTriggerWindow.TurnEnding => (trigger.OwnerSeat == facts.CurrentSeat || IsDeadOwnerEndingCandidate(
                trigger.SkillId, trigger.BindingId, trigger.OwnerSeat, facts.CurrentSeat)) && CurrentRule == WenheRuleKind.Equipment &&
                !_host.History<WenheRuleFlowIssuedEvent>().Any(fact => fact.TurnNumber == facts.TurnNumber && fact.Operation == SkillProgramEffectOp.ResolveWenheTurnEnd),
            SkillProgramTriggerWindow.RoundEnding => trigger.OwnerSeat == facts.Players.Where(player => player.IsAlive)
                .Select(player => player.Seat).DefaultIfEmpty(-1).Min() && CurrentRule == WenheRuleKind.Famine &&
                !_host.History<WenheRuleFlowIssuedEvent>().Any(fact => fact.RoundNumber == facts.RoundNumber && fact.Operation == SkillProgramEffectOp.ResolveWenheRoundEnd),
            SkillProgramTriggerWindow.CharacterDied => trigger.SourceSeat == trigger.OwnerSeat && trigger.TargetSeat != trigger.OwnerSeat,
            _ => false
        };
    }

    public ModeWenheFlowPlan IssueFlow(long frameId, SkillProgramEffectOp operation, int ownerSeat)
    {
        if (!Active) throw new InvalidOperationException("Wenhe requires its installed activity rules.");
        var facts = _host.CaptureFacts();
        var living = facts.Players.Where(player => player.IsAlive).ToArray();
        var amount = operation == SkillProgramEffectOp.ResolveWenheRoundEnd ? facts.RoundNumber :
            operation == SkillProgramEffectOp.RewardWenheKiller ? CurrentRule == WenheRuleKind.Bounty ? 2 : 1 : 1;
        var seats = operation switch
        {
            SkillProgramEffectOp.ResolveWenheTurnEnd => living.Where(player => player.EquipmentCount == living.Min(other => other.EquipmentCount)).Select(player => player.Seat).ToArray(),
            SkillProgramEffectOp.ResolveWenheRoundEnd => living.Where(player => player.HandCount == living.Min(other => other.HandCount)).Select(player => player.Seat).ToArray(),
            _ => new[] { ownerSeat }
        };
        if (operation == SkillProgramEffectOp.UseWenheChampionMark)
        {
            if (GoalRank(ownerSeat) != 1 || ChampionMarkUsed(ownerSeat)) throw new InvalidOperationException("The private champion mark is unavailable.");
            _host.PublishFact(new WenheChampionMarkUsedEvent(frameId, ownerSeat));
        }
        return new(amount, seats, operation == SkillProgramEffectOp.ResolveWenheRoundStart);
    }

    public int DrawCount(SkillProgramEffectOp operation, int amount) => operation switch
    { SkillProgramEffectOp.RewardWenheKiller => amount * 3, SkillProgramEffectOp.ResolveWenheTurnStart => 3, SkillProgramEffectOp.UseWenheChampionMark => 2, _ => 0 };

    public bool ResumeFlow(long frameId)
    {
        if (_flow.BeginContinuation(frameId) is not { } cursor) return false;
        if (cursor.Receipt.Stage == 3) throw new InvalidOperationException("Round chaos cannot resume before its issued child returns.");
        if (_flow.DrainChildren(frameId)) return true;
        cursor = _flow.Current(frameId);
        while (cursor.Receipt.Index < cursor.Receipt.Seats.Count)
        {
            var receipt = cursor.Receipt;
            var seat = receipt.Seats[receipt.Index];
            if (receipt.Stage == 0)
            {
                _flow.AdvanceCursor(frameId, receipt.Index, 0, receipt.Index, 1);
                if (_host.CaptureFacts().Players[seat].IsAlive)
                {
                    if (receipt.Operation == SkillProgramEffectOp.RewardWenheKiller) _flow.IncreaseMaximumHp(frameId, receipt.Amount);
                    else if (receipt.Operation != SkillProgramEffectOp.UseWenheChampionMark && _flow.LoseHp(frameId, seat, receipt.Amount)) return true;
                    if (_flow.DrainHpChildren(frameId)) return true;
                }
                cursor = _flow.Current(frameId); receipt = cursor.Receipt;
            }
            if (receipt.Stage == 1)
            {
                _flow.AdvanceCursor(frameId, receipt.Index, 1, receipt.Index, 2);
                if (_host.CaptureFacts().Players[seat].IsAlive)
                {
                    var draw = DrawCount(receipt.Operation, receipt.Amount);
                    if (draw > 0) { _flow.BeginDraw(frameId, seat, draw); return true; }
                    if (receipt.Operation == SkillProgramEffectOp.ResolveWenheTurnEnd) _flow.EquipRandomAvailable(frameId, seat);
                }
            }
            if (_flow.DrainChildren(frameId)) return true;
            cursor = _flow.Current(frameId); receipt = cursor.Receipt;
            _flow.AdvanceCursor(frameId, receipt.Index, receipt.Stage, receipt.Index + 1, 0);
            cursor = _flow.Current(frameId);
        }
        _flow.ClearFlow(frameId);
        return false;
    }
}

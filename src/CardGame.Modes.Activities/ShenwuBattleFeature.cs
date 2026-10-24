using CardGame.Core;

namespace CardGame.Modes.Activities;

internal sealed class ShenwuBattleFeature : IShenwuBattleFeature, IMatchModeNumericRules, IMatchModeLifecycle,
    IMatchModeRuleObserverProvider, IMatchModeRuntimeHookProvider
{
    private readonly GameOptions _options;
    private readonly ShenwuActivityDefinition _activity;
    private readonly ContentRegistry _registry;
    private readonly IStandardBattleHost _host;
    private readonly IModeEncounterHost _encounter;

    internal ShenwuBattleFeature(GameOptions options, ShenwuActivityDefinition activity, ContentRegistry registry,
        IStandardBattleHost host, IModeEncounterHost encounter)
    {
        _options = options; _activity = activity; _registry = registry; _host = host; _encounter = encounter;
        RuleObservers = Array.AsReadOnly<ModeRuleObserverRegistration>(
            [ModeRuleObserverRegistration.ForEvent<IGameEvent>(ProgramRulePhase.AfterEvent, 27700, GrantHalfHpSkill)]);
        RuntimeHooks = Array.AsReadOnly<ModeRuntimeHookRegistration>(
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 32, TryCompleteEncounter)]);
    }

    int IMatchModeNumericRules.Order => 50;
    int IMatchModeLifecycle.Order => 30;
    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }
    private int Seat(int offset) => (Math.Max(0, _options.HumanSeat) + offset) % _options.PlayerCount;
    private int LeaderSeat => Seat(_activity.LeaderSeatOffset);
    public bool IsPlayerSlot(int seat) => _activity.PlayerSeatOffsets.Contains(
        (seat - Math.Max(0, _options.HumanSeat) + _options.PlayerCount) % _options.PlayerCount);

    public int ReplacementDrawBonus(int seat) => _activity is { Edition: 2020, EnemyTier: >= 4 } &&
        !IsPlayerSlot(seat) ? 1 : 0;

    public bool AllowsTierTrigger(int seat, string skillId) => _activity.Edition > 0 && (skillId switch
    {
        "shenwu:enemy-tier" => !IsPlayerSlot(seat) && _activity.EnemyTier >= 2,
        "shenwu:player-tier-2020" => IsPlayerSlot(seat) && _activity.PlayerTier >= 2,
        _ => false
    });

    public void OnMatchStart(ModeMatchStartPhase phase)
    {
        if (phase != ModeMatchStartPhase.AfterRunning || _host.ShenwuDraft() is not { } draft) return;
        _encounter.PublishFact(new ShenwuSharedGodCandidatesEvent(draft.SharedGods));
        _encounter.PublishFact(new ShenwuLeaderChosenEvent(LeaderSeat, draft.LeaderId));
    }

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        var allied = IsPlayerSlot(seat);
        var tier = allied ? _activity.PlayerTier : _activity.EnemyTier;
        if (_activity.Edition != 0)
        {
            var extra = query switch
            {
                SkillRuleQuery.InitialHandSize => (tier >= 4 ? 2 : tier >= 2 ? 1 : 0) + (!allied && seat == LeaderSeat ? 8 - _host.InitialHandSize : 0),
                SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
                SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
                _ => 0
            };
            if (extra != 0) yield return new FiniteRuleQueryContribution($"activity:shenwu:{_activity.Edition}:tier-{query}", SkillRuleOperation.Add, extra);
            yield break;
        }
        var amount = allied ? query switch
        {
            SkillRuleQuery.InitialHandSize => tier >= 3 ? 1 : 0,
            SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
            SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
            _ => 0
        } : query == SkillRuleQuery.InitialHandSize && seat == LeaderSeat ? 8 - _host.InitialHandSize : 0;
        if (amount != 0) yield return new FiniteRuleQueryContribution($"activity:shenwu:tier-{query}", SkillRuleOperation.Add, amount);
    }

    private void GrantHalfHpSkill(IGameEvent fact)
    {
        if (fact is not (DamageAppliedEvent or ProgramSkillHpLostEvent or MaximumHpChangedEvent or DamageReplacedWithHpLossEvent) ||
            !_encounter.CaptureFacts().SetupComplete) return;
        var leader = _host.Player(LeaderSeat);
        if (!leader.IsAlive || !leader.GeneralSelected || leader.Hp * 2 > leader.MaxHp ||
            _encounter.History<ShenwuLeaderEnragedEvent>().Any()) return;
        var skill = _activity.HalfHpSkillIds[leader.GeneralId];
        var grant = $"activity:shenwu:half-health:{leader.Seat}:{skill}";
        _host.GrantSkill(leader.Seat, new(grant, skill, grant, grant));
        _encounter.PublishFact(new ShenwuLeaderEnragedEvent(leader.Seat, skill, leader.Hp, leader.MaxHp));
        _encounter.Log("ShenwuEnraged", $"{leader.GeneralName}首次降至半血，获得【{_registry.GetSkill(skill).Name}】。", leader.Seat);
    }

    private bool TryCompleteEncounter()
    {
        var facts = _encounter.CaptureFacts();
        if (!facts.SetupComplete || facts.HasWinner || facts.Players[LeaderSeat].IsAlive || facts.ResolutionCount != 0 ||
            facts.PendingDecisionKind is { } kind && kind != DecisionKind.PlayCard) return false;
        _encounter.ClearDecision();
        _encounter.DeclareTeamWinner(facts.Players.Any(player => IsPlayerSlot(player.Seat) && player.IsAlive)
            ? _activity.PlayerTeamId : _activity.EnemyTeamId);
        _encounter.PublishState();
        return true;
    }
}

using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Four encounters and NPC appearances, with their replayed history as the only ledger.</summary>
internal sealed class HellEncounterFeature : IHellEncounterFeature, IMatchModeRuntimeHookProvider, IMatchModeNumericRules,
    IMatchModeDamageRuleProvider
{
    private readonly ContentModeDefinition _mode;
    private readonly IHellEncounterHost _native;
    private IModeEncounterHost Host => _native.Encounter;

    internal HellEncounterFeature(ContentModeDefinition mode, IHellEncounterHost native)
    {
        _mode = mode;
        _native = native;
        RuntimeHooks = Array.AsReadOnly<ModeRuntimeHookRegistration>(
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 29, TryAdvance)]);
        DamageRules = Array.AsReadOnly<ModeDamageRuleRegistration>([ModeDamageRuleRegistration.Additive(260, ArrivalDamageBonus)]);
    }

    public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }
    public IReadOnlyList<ModeDamageRuleRegistration> DamageRules { get; }
    public int Order => 60;

    public bool IsNpcTeam(string? teamId) => teamId == _mode.BossActivity!.EnemyTeamId;

    public bool AllowsTierTrigger(string skillId, string bindingId, string? teamId)
    {
        if (skillId == "hell:player-tier") return !IsNpcTeam(teamId);
        if (skillId != "hell:boss-tier" || !IsNpcTeam(teamId)) return false;
        return bindingId switch
        {
            "arrival-equipment" => _mode.HellActivity!.BossTier >= 2,
            "arrival-assault" => _mode.HellActivity!.BossTier == 5,
            "turn-slash" => _mode.HellActivity!.BossTier >= 3,
            _ => false
        };
    }
    private int ArrivalDamageBonus(ModeDamageFacts damage)
    {
        if (_mode.HellActivity!.BossTier != 5 || damage.IsChainPropagation || damage.IsSourceLess ||
            damage.EffectiveCardKind != CardKind.BarbarianAssault || _native.ArrivalSource(damage.ResolutionId) is not { SkillId: "hell:boss-tier" } source)
            return 0;
        return source.IsInitialGameStarting || source.AppearanceId is { } id && IsPendingAppearance(id, source.CandidateOwnerSeats) ? 1 : 0;
    }
    private int StageIndex => Host.History<BossActivityStageStartedEvent>().Select(e => e.StageIndex).DefaultIfEmpty(0).Last();
    private static int BossSeat(ModeEncounterFacts facts) => (Math.Max(0, facts.HumanSeat) + 3) % facts.PlayerCount;
    private static int AuxiliarySeat(ModeEncounterFacts facts) => (Math.Max(0, facts.HumanSeat) + 4) % facts.PlayerCount;
    private int NpcBonus => _mode.HellActivity!.BossTier >= 5 ? 2 : _mode.HellActivity.BossTier >= 3 ? 1 : 0;
    private int HealthBonus(ModeEncounterPlayerFacts player) => player.TeamId == _mode.BossActivity!.EnemyTeamId ? NpcBonus : 2;

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        var player = Host.CaptureFacts().Players[seat];
        var npc = player.TeamId == _mode.BossActivity!.EnemyTeamId;
        var tier = npc ? _mode.HellActivity!.BossTier : 5;
        var amount = !player.IsAlive ? query == SkillRuleQuery.InitialHandSize ? -_native.InitialHandSize : 0 : query switch
        {
            SkillRuleQuery.InitialHandSize => tier >= 5 || !npc && tier >= 4 ? 2 : tier >= (npc ? 3 : 2) ? 1 : 0,
            SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
            SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
            SkillRuleQuery.HandLimit => npc && tier >= 4 ? 1 : 0,
            _ => 0
        };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"activity:{_mode.Id}:tier-{query}", SkillRuleOperation.Add, amount);
    }

    private bool TryAdvance()
    {
        var facts = Host.CaptureFacts();
        if (!facts.SetupComplete || facts.HasWinner || facts.ResolutionCount != 0 ||
            facts.PendingDecisionKind is { } decision && decision != DecisionKind.PlayCard) return false;
        var bossSeat = BossSeat(facts);
        if (Host.History<HellAuxiliaryScheduledEvent>().Count == 0)
        {
            var scheduled = Host.NextRandom(3);
            Host.PublishFact(new HellAuxiliaryScheduledEvent(scheduled, AuxiliarySeat(facts)));
            RecordAppearance(Host.CaptureFacts().Players[bossSeat], 0, completed: true);
            if (scheduled == 0) ActivateAuxiliary(0, initial: true);
        }
        if (_native.GameStartingProgramsResolved && TryBeginAppearance()) return true;
        facts = Host.CaptureFacts();
        var boss = facts.Players[bossSeat];
        if (boss.IsAlive) return false;
        var activity = _mode.BossActivity!;
        if (StageIndex == 3)
        {
            Host.ClearDecision();
            Host.DeclareTeamWinner(activity.PlayerTeamId);
            Host.PublishState();
            return true;
        }
        if (_native.ActualTurnOwnerSeat == bossSeat && facts.Phase is not (TurnPhase.NotStarted or TurnPhase.Finished))
        { Host.EndCurrentTurn(); return true; }
        var nextIndex = StageIndex + 1;
        var pool = activity.Stages[nextIndex].GeneralPoolIds;
        var hiddenId = _mode.HellActivity!.HiddenGeneralId;
        var hidden = nextIndex == 3 && facts.TurnNumber <= 50 &&
            Host.History<PlayerDiedEvent>().Any(e => e.VictimSeat == AuxiliarySeat(facts));
        var id = nextIndex == 3 ? hidden ? hiddenId : pool.Single(g => g != hiddenId) : pool[Host.NextRandom(pool.Count)];
        var general = Host.General(id);
        var healthBonus = HealthBonus(boss);
        Host.ReplaceCombatant(new(bossSeat, boss.GeneralId, id, PreserveCurrentState: false,
            MaximumHpOverride: general.BaseHp + healthBonus, HpOverride: general.InitialHp + healthBonus));
        Host.ClearDecision();
        Host.PublishFact(new BossActivityStageStartedEvent(nextIndex, bossSeat, id));
        Host.DrawInitialCards(bossSeat, 4 + NpcBonus);
        RecordAppearance(Host.CaptureFacts().Players[bossSeat], nextIndex);
        if (Host.History<HellAuxiliaryScheduledEvent>().Single().StageIndex == nextIndex) ActivateAuxiliary(nextIndex, initial: false);
        Host.Log("ActivityStageStarted", $"第 {nextIndex + 1}/4 关：{general.Name}登场。", bossSeat);
        TryBeginAppearance();
        Host.PublishState();
        return true;
    }

    private void RecordAppearance(ModeEncounterPlayerFacts npc, int stage, bool completed = false)
    {
        var id = _native.NextResolutionId();
        Host.PublishFact(new HellNpcAppearedEvent(id, stage, npc.Seat, npc.GeneralId));
        if (completed) Host.PublishFact(new HellAppearanceCompletedEvent(id));
    }

    private void ActivateAuxiliary(int stage, bool initial)
    {
        var facts = Host.CaptureFacts();
        var seat = AuxiliarySeat(facts);
        var auxiliary = facts.Players[seat];
        var general = Host.General(auxiliary.GeneralId);
        var bonus = HealthBonus(auxiliary);
        _native.RestoreDormantCombatant(seat, general.BaseHp + bonus, general.InitialHp + bonus);
        Host.DrawInitialCards(seat, 4 + NpcBonus);
        RecordAppearance(Host.CaptureFacts().Players[seat], stage, completed: initial);
    }

    private bool TryBeginAppearance()
    {
        var completed = Host.History<HellAppearanceCompletedEvent>().Select(e => e.AppearanceId).ToHashSet();
        foreach (var appearance in Host.History<HellNpcAppearedEvent>().Where(e => !completed.Contains(e.AppearanceId)))
        {
            if (_native.TryOpenNpcGameStartingWindow(appearance)) return true;
            Host.PublishFact(new HellAppearanceCompletedEvent(appearance.AppearanceId));
        }
        return false;
    }

    public bool IsPendingAppearance(long appearanceId, IReadOnlyList<int> candidateOwnerSeats)
    {
        if (Host.History<HellAppearanceCompletedEvent>().Any(e => e.AppearanceId == appearanceId) ||
            Host.History<HellNpcAppearedEvent>().SingleOrDefault(e => e.AppearanceId == appearanceId) is not { } appeared) return false;
        return Host.CaptureFacts().Players[appeared.Seat].GeneralId == appeared.GeneralId &&
            candidateOwnerSeats.All(seat => seat == appeared.Seat);
    }
}

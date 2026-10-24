namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Constructor draft data is deterministic from the seed. Queue its public
    // facts only after Start, when the command session can retain them.
    private sealed record ShenwuDraftSetup(IReadOnlyList<string> SharedGods, string LeaderId);
    private ShenwuDraftSetup? _shenwuDraft;
    private int ShenwuSeat(int offset) => (Math.Max(0, _options.HumanSeat) + offset) % _playerCount;
    private int ShenwuSeatOffset(int seat) => (seat - Math.Max(0, _options.HumanSeat) + _playerCount) % _playerCount;
    private IReadOnlyList<int> ShenwuPlayerSlots => _modeDefinition.ShenwuActivity!.PlayerSeatOffsets.Select(ShenwuSeat).ToArray();
    private int ShenwuLeaderSeat => ShenwuSeat(_modeDefinition.ShenwuActivity!.LeaderSeatOffset);
    private bool IsShenwuPlayerSlot(int seat) => _modeDefinition.ShenwuActivity is { } activity &&
        activity.PlayerSeatOffsets.Contains(ShenwuSeatOffset(seat));

    private void RecordShenwuDraftFacts()
    {
        if (_shenwuDraft is not { } draft) return;
        AdvanceEventRulesAndQueueFact(new ShenwuSharedGodCandidatesEvent(draft.SharedGods));
        AdvanceEventRulesAndQueueFact(new ShenwuLeaderChosenEvent(ShenwuLeaderSeat, draft.LeaderId));
    }

    private PromptChoice WithShenwuGeneralChoice(PromptChoice choice, GeneralDefinition general, string candidateId)
    {
        if (_modeDefinition.ShenwuActivity is not { } activity) return choice;
        var god = activity.GodPoolIds.Contains(general.Id, StringComparer.Ordinal);
        var healthBonus = ShenwuPlayerHealthBonus(activity);
        return choice with
        {
            Parameters = new Dictionary<string, string>(choice.Parameters, StringComparer.Ordinal)
            {
                ["candidate-general-id"] = candidateId,
                ["shenwu-shared-god"] = god ? "true" : "false",
                ["health-preview"] = $"本地{(activity.PlayerTier == 5 ? "五" : activity.PlayerTier.ToString(System.Globalization.CultureInfo.InvariantCulture))}阶：初始体力 {(general.InitialHp ?? general.BaseHp) + healthBonus}/{general.BaseHp + healthBonus}。" +
                    (god ? "此神将来自三人共享候选；选择后其他队友不能再选。" : "普通候选仅对你可见。")
            }
        };
    }

    private static int ShenwuPlayerHealthBonus(ShenwuActivityDefinition activity) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ShenwuActivities.ShenwuPlayerHealthBonus(activity);

    private IEnumerable<RuleQueryContribution> ShenwuActivityRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        if (_modeDefinition.ShenwuActivity is not { } activity) yield break;
        var allied = IsShenwuPlayerSlot(player.Seat);
        var tier = allied ? activity.PlayerTier : activity.EnemyTier;
        if (activity.Edition != 0)
        {
            var extra = query switch
            {
                SkillRuleQuery.InitialHandSize => (tier >= 4 ? 2 : tier >= 2 ? 1 : 0) + (!allied && player.Seat == ShenwuLeaderSeat ? 8 - _initialHandSize : 0),
                SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
                SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
                _ => 0
            };
            if (extra != 0) yield return new FiniteRuleQueryContribution($"activity:shenwu:{activity.Edition}:tier-{query}", SkillRuleOperation.Add, extra);
            yield break;
        }
        var amount = IsShenwuPlayerSlot(player.Seat) ? query switch
        {
            SkillRuleQuery.InitialHandSize => tier >= 3 ? 1 : 0,
            SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
            SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
            _ => 0
        } : query == SkillRuleQuery.InitialHandSize && player.Seat == ShenwuLeaderSeat ? 8 - _initialHandSize : 0;
        if (amount != 0) yield return new FiniteRuleQueryContribution($"activity:shenwu:tier-{query}", SkillRuleOperation.Add, amount);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27700)]
    private void GrantShenwuHalfHpSkill(IGameEvent fact)
    {
        if (_modeDefinition.ShenwuActivity is not { } activity || !_setupComplete ||
            fact is not (DamageAppliedEvent or ProgramSkillHpLostEvent or MaximumHpChangedEvent or DamageReplacedWithHpLossEvent)) return;
        var leader = _players[ShenwuLeaderSeat];
        if (!leader.IsAlive || !leader.GeneralSelected || leader.Hp * 2 > leader.MaxHp ||
            CompleteProgramEventHistory().OfType<ShenwuLeaderEnragedEvent>().Any()) return;
        var skill = activity.HalfHpSkillIds[leader.General.Id];
        var grant = $"activity:shenwu:half-health:{leader.Seat}:{skill}";
        leader.SkillGrants.Grant(new(grant, skill, grant, grant));
        _skillBindingIndex.Invalidate(leader.Seat);
        AdvanceEventRulesAndQueueFact(new ShenwuLeaderEnragedEvent(leader.Seat, skill, leader.Hp, leader.MaxHp));
        AddLog("ShenwuEnraged", $"{leader.General.Name}首次降至半血，获得【{_contentRegistry.GetSkill(skill).Name}】。", leader.Seat);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 32)]
    private bool TryCompleteShenwuEncounter()
    {
        if (_modeDefinition.ShenwuActivity is not { } activity || !_setupComplete || _winner != Winner.None ||
            _players[ShenwuLeaderSeat].IsAlive || _resolutionStack.Count != 0 ||
            _pendingDecision is { Kind: not DecisionKind.PlayCard }) return false;
        _pendingDecision = null;
        _winnerTeamId = _players.Any(player => IsShenwuPlayerSlot(player.Seat) && player.IsAlive)
            ? activity.PlayerTeamId : activity.EnemyTeamId;
        _winner = GetWinnerForTeam(_winnerTeamId);
        AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(_winner, _winnerTeamId, null));
        AdvanceRulesAndPublishState();
        return true;
    }
}

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int BossActivitySeat => (Math.Max(0, _options.HumanSeat) + 3) % _playerCount;
    private int BossActivityStageIndex => CompleteProgramEventHistory().OfType<BossActivityStageStartedEvent>()
        .Select(e => e.StageIndex).DefaultIfEmpty(0).Last();
    private IEnumerable<RuleQueryContribution> BossActivityRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        if (_modeDefinition.BossActivity is not { } a) yield break;
        if (_modeDefinition.HellActivity is not null)
        { foreach (var contribution in HellActivityRuleContributions(player, query)) yield return contribution; yield break; }
        var tier = a.PlayerTier;
        var amount = player.Seat == BossActivitySeat
            ? query == SkillRuleQuery.InitialHandSize ? a.Stages[BossActivityStageIndex].InitialHandSize - _initialHandSize : 0
            : query switch
            {
                SkillRuleQuery.InitialHandSize => (tier >= 3 ? 1 : 0) +
                    (_modeDefinition.HulaoActivity is not null && BossActivityStageIndex == 0 ?
                        3 + (player.Seat - Math.Max(0, _options.HumanSeat) + _playerCount) % _playerCount - _initialHandSize : 0),
                SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
                SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
                _ => 0
            };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"activity:{_modeDefinition.Id}:tier-{query}", SkillRuleOperation.Add, amount);
    }

    // Change encounters only after the complete killing action, death skills,
    // and every child resolution return. A pending play prompt is refreshed.
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 30)]
    private bool TryAdvanceBossActivityStage()
    {
        if (_modeDefinition.HellActivity is not null) return false;
        if (_modeDefinition.BossActivity is not { } activity || !_setupComplete || _winner != Winner.None ||
            _players[BossActivitySeat].IsAlive || _resolutionStack.Count != 0 ||
            _pendingDecision is { Kind: not DecisionKind.PlayCard }) return false;
        if (_modeDefinition.HulaoActivity is not null && BossActivityStageIndex == 0 && _players[BossActivitySeat].MaxHp > 0) return false;
        if (BossActivityStageIndex + 1 >= activity.Stages.Count || _modeDefinition.HulaoActivity is not null && _players[BossActivitySeat].MaxHp == 0)
        {
            _pendingDecision = null;
            _winnerTeamId = activity.PlayerTeamId;
            _winner = GetWinnerForTeam(_winnerTeamId);
            AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(_winner, _winnerTeamId, null));
            AdvanceRulesAndPublishState();
            return true;
        }
        if (_currentSeat == BossActivitySeat && _phase is not (TurnPhase.NotStarted or TurnPhase.Finished))
        {
            // A boss killed by a counterattack must finish its actual dead
            // turn before another character can occupy this seat.
            EndTurn();
            return true;
        }
        var stageIndex = BossActivityStageIndex + 1;
        var stage = activity.Stages[stageIndex];
        var generalId = stage.GeneralPoolIds[_random.Next(stage.GeneralPoolIds.Count)];
        var general = _generalPool.Single(g => g.Id == generalId);
        var previous = _players[BossActivitySeat];
        var boss = new CharacterState
        {
            Seat = previous.Seat, Name = previous.Name, IsHuman = false, Role = previous.Role,
            TeamId = previous.TeamId, TeamRevealed = true, RoleRevealed = true,
            General = general, GeneralSelected = true, GeneralRevealed = true,
            Hp = general.InitialHp ?? general.BaseHp, MaxHp = general.BaseHp
        };
        if (HasPrivateGeneralLibraryCapability) boss.GeneralLibraryGenderQuery = GetPrivateGeneralLibraryGender;
        _players[boss.Seat] = boss;
        _skillBindingIndex.Invalidate(boss.Seat);
        _skillRuntimeState.ResetOwner(boss.Seat);
        foreach (var key in _programUses.Keys.Where(key => key.Seat == boss.Seat).ToArray()) _programUses.Remove(key);
        foreach (var key in _programPhaseUses.Keys.Where(key => key.Seat == boss.Seat).ToArray()) _programPhaseUses.Remove(key);
        foreach (var key in _programContributionUses.Keys.Where(key => key.SkillOwnerSeat == boss.Seat).ToArray()) _programContributionUses.Remove(key);
        foreach (var key in _programBooleanStates.Keys.Where(key => key.OwnerSeat == boss.Seat).ToArray()) _programBooleanStates.Remove(key);
        _observedSkillGrantRevisions[boss.Seat] = boss.SkillGrants.Revision;
        foreach (var id in EnabledContentSkillIds(boss)) RegisterTaggedConversionSkill(boss, id);
        _pendingDecision = null;
        AdvanceEventRulesAndQueueFact(new BossActivityStageStartedEvent(stageIndex, boss.Seat, general.Id));
        DrawCards(boss, stage.InitialHandSize, log: true, CardMoveReasons.InitialDeal);
        AddLog("ActivityStageStarted", $"第 {stageIndex + 1}/{activity.Stages.Count} 关：{stage.Name}，{general.Name}登场。", boss.Seat);
        AdvanceRulesAndPublishState();
        return true;
    }
}

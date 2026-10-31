#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouRuntime
{
    internal const string HengyeSkillId = "mou:hengye";
    internal const string HengyeGrowthUsageId = "growth";
    internal int GetHengyeGrowth(CharacterState player) => _coreMouLuMengOperations.Value.UsesFormalMouLuMeng && _coreNationalSkillsOperations.HasRuntimeSkill(player, HengyeSkillId) ? _skillRuntimeStateOperations._skillRuntimeState.GetUsage(player.Seat, HengyeSkillId, HengyeGrowthUsageId, SkillUsageScope.Game) : 0;
    internal void ApplyHengyeGrowth(long damageFrameId, CharacterState source, CharacterState target, int amount)
    {
        if (!_coreMouLuMengOperations.Value.UsesFormalMouLuMeng || amount <= 0 || !source.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(source, HengyeSkillId))
            return;
        var previous = GetHengyeGrowth(source);
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(source.Seat, HengyeSkillId, HengyeGrowthUsageId, SkillUsageScope.Game, limit: 3))
            return;
        var current = previous + 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(source.Seat, HengyeSkillId, HengyeGrowthUsageId, SkillUsageScope.Game, current));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HengyeGrowthChangedEvent(damageFrameId, source.Seat, previous, current));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(source.Name)} 的【横野】成长至 {(current)}/3（摸牌数、出杀次数、攻击范围、手牌上限同步增加）。", source.Seat, target.Seat);
    }

    internal void ResolveHengyeTurnStart(CharacterState player)
    {
        if (GetHengyeGrowth(player) < 3 || player.Hp >= player.MaxHp)
            return;
        var recoveryFrameId = _coreNativeRecoveryFramesOperations.BeginRecovery(0, player.Seat, player.Seat, 1);
        try
        {
            player.Hp++;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(player.Seat, player.Seat, 1, player.Hp));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(player.Name)} 的【横野】已成长至 3，本回合开始时回复 1 点体力。", player.Seat);
        }
        finally
        {
            _coreRuntimeOperations.PopResolutionFrame(recoveryFrameId, ResolutionFrameKind.Recovery);
        }
    }

    internal void ResetHengyeAfterKill(CharacterState? killer)
    {
        if (!_coreMouLuMengOperations.Value.UsesFormalMouLuMeng || killer is not { IsAlive: true } || !_coreNationalSkillsOperations.HasRuntimeSkill(killer, HengyeSkillId))
            return;
        var previous = GetHengyeGrowth(killer);
        _skillRuntimeStateOperations._skillRuntimeState.ResetSkill(killer.Seat, HengyeSkillId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillResetEvent(killer.Seat, HengyeSkillId, previous));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(killer.Name)} 杀死一名角色，重置【横野】至游戏开始时状态。", killer.Seat);
    }

    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeContributions, 50)]
    internal void AddGetHengyeGrowthAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => CoreGameEngineRuleQueriesRuntime.AddFiniteContribution(contributions, $"state:{(player.Seat)}:hengye:growth", GetHengyeGrowth(player));
}

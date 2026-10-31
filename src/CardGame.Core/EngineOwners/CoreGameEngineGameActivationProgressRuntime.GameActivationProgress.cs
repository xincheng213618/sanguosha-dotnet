#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGameActivationProgressRuntime
{
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot>? GetSkillPublicRuleStates(CharacterState owner, string skillId)
    {
        var existing = _coreHandComparisonOperations.GetHandComparisonPublicRuleStates(owner, skillId);
        List<ProgramPublicRuleStateSnapshot>? states = null;
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
        {
            if (instance.SkillId == skillId)
                foreach (var policy in instance.Program.ViewAs.Where(rule => rule.RoundOnceCardlessBasic is not null).Select(rule => rule.RoundOnceCardlessBasic!).DistinctBy(policy => policy.UsageGroup))
                    (states ??= []).Add(new(instance.SkillInstanceId, policy.UsageGroup, ProgramPublicRuleStateKind.ActivationLimit, policy.UsesPerRound, _coreRoundOnceCardlessBasicOperations.RoundOnceCardlessBasicUsed(owner.Seat, skillId, policy.UsageGroup) ? 1 : 0, SkillUsageScope.Round));
            if (instance.SkillId == skillId && MouLiuChengComposition.IsProgram(instance.Program) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } liuChengPhase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(liuChengPhase))
                (states ??= []).Add(new(instance.SkillInstanceId, "after-designation", ProgramPublicRuleStateKind.ActivationLimit, 2, _characterLiuChengMouMobileOperations.MouLiuChengPhaseUses(owner.Seat, skillId, liuChengPhase), SkillUsageScope.Phase));
            if (instance.SkillId == skillId)
                foreach (var activation in instance.Program.Activations)
                {
                    var plan = ProgramInstructionResolver.Default.Resolve(instance.Program, ProgramInstructionSourceKind.Activation, activation.Id);
                    var phaseLimit = _coreProgramSelectionModulesOperations.ProgramSelections.PhaseLimit(new(owner.Seat, instance.Program, plan));
                    AddLimit(phaseLimit, SkillUsageScope.Phase, _coreProgramPhaseUsedProjectionsOperations.PhaseUsedProjections.Apply(owner.Seat, skillId, _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault((owner.Seat, skillId, activation.UsageGroup))));
                    AddLimit(activation.UsesPerTurn, SkillUsageScope.Turn, _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault((owner.Seat, skillId, activation.UsageGroup)));
                    AddLimit(activation.UsesPerGame, SkillUsageScope.Game, _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, activation.UsageGroup, SkillUsageScope.Game));
                    void AddLimit(int? limit, SkillUsageScope scope, int used)
                    {
                        if (limit is not > 0 || (existing ?? []).Any(state => state.Kind == ProgramPublicRuleStateKind.ActivationLimit && state.SkillInstanceId == instance.SkillInstanceId && state.StateId == activation.Id && state.Scope == scope))
                            return;
                        (states ??= []).Add(new(instance.SkillInstanceId, activation.Id, ProgramPublicRuleStateKind.ActivationLimit, limit.Value, used, scope));
                    }
                }

            foreach (var effect in instance.Program.Triggers.SelectMany(trigger => trigger.Effects))
                if (effect.Op == SkillProgramEffectOp.AwakenAfterGameActivations && effect.SourceBind == skillId)
                    (states ??= []).Add(new(instance.SkillInstanceId, effect.StateId!, ProgramPublicRuleStateKind.GameActivationProgress, effect.MinimumValue, _coreGameActivationAwakeningOperations.GameProgramActivationCount(owner.Seat, skillId, effect.StateId!), SkillUsageScope.Game));
        }

        var weiYanStates = _characterWeiYanMobileShiOperations.MobileShiWeiYanPublicStates(owner, skillId);
        if (weiYanStates.Count > 0)
            (states ??= []).AddRange(weiYanStates);
        var chenQunStates = _characterChenQunMobileShiOperations.MobileShiChenQunPublicStates(owner, skillId);
        if (chenQunStates.Count > 0)
            (states ??= []).AddRange(chenQunStates);
        var wangChangStates = _characterWangChangMobileShiOperations.MobileShiWangChangPublicStates(owner, skillId);
        if (wangChangStates.Count > 0)
            (states ??= []).AddRange(wangChangStates);
        var tianFengStates = _characterTianFengMobileShiOperations.MobileShiTianFengPublicStates(owner, skillId);
        if (tianFengStates.Count > 0)
            (states ??= []).AddRange(tianFengStates);
        var chenZhiStates = _characterChenZhiMobileShiOperations.MobileShiChenZhiPublicStates(owner, skillId);
        if (chenZhiStates.Count > 0)
            (states ??= []).AddRange(chenZhiStates);
        var dongZhaoStates = _characterDongZhaoMobileShiOperations.MobileShiDongZhaoPublicStates(owner, skillId);
        if (dongZhaoStates.Count > 0)
            (states ??= []).AddRange(dongZhaoStates);
        var chenDaoStates = _characterChenDaoMobileShiOperations.MobileShiChenDaoPublicStates(owner, skillId);
        if (chenDaoStates.Count > 0)
            (states ??= []).AddRange(chenDaoStates);
        var huangZuStates = _characterHuangZuMobileShiOperations.MobileShiHuangZuPublicStates(owner, skillId);
        if (huangZuStates.Count > 0)
            (states ??= []).AddRange(huangZuStates);
        var guoYuanStates = _characterGuoYuanMobileShiOperations.MobileShiGuoYuanPublicStates(owner, skillId);
        if (guoYuanStates.Count > 0)
            (states ??= []).AddRange(guoYuanStates);
        var xinXianYingStates = _characterXinXianYingMobileShiOperations.ShiXinXianYingPublicStates(owner, skillId);
        if (xinXianYingStates.Count > 0)
            (states ??= []).AddRange(xinXianYingStates);
        var heQiStates = _characterHeQiMobileShiOperations.MobileShiHeQiPublicStates(owner, skillId);
        if (heQiStates.Count > 0)
            (states ??= []).AddRange(heQiStates);
        var luSuStates = _characterLuSuMobileShiOperations.MobileShiLuSuPublicStates(owner, skillId);
        if (luSuStates.Count > 0)
            (states ??= []).AddRange(luSuStates);
        var luYuShengStates = _characterLuYuShengMobileShiOperations.MobileShiLuYuShengPublicStates(owner, skillId);
        if (luYuShengStates.Count > 0)
            (states ??= []).AddRange(luYuShengStates);
        var sunChenStates = _characterSunChenMobileShiOperations.MobileShiSunChenPublicStates(owner, skillId);
        if (sunChenStates.Count > 0)
            (states ??= []).AddRange(sunChenStates);
        var sunJunStates = _characterSunJunMobileShiOperations.MobileShiSunJunPublicStates(owner, skillId);
        if (sunJunStates.Count > 0)
        {
            states?.RemoveAll(state => sunJunStates.Any(actual => actual.SkillInstanceId == state.SkillInstanceId && actual.StateId == state.StateId && actual.Kind == state.Kind && actual.Scope == state.Scope));
            (states ??= []).AddRange(sunJunStates);
        }

        var xiaoQiaoStates = _characterXiaoQiaoMobileShiOperations.MobileShiXiaoQiaoPublicStates(owner, skillId);
        if (xiaoQiaoStates.Count > 0)
            (states ??= []).AddRange(xiaoQiaoStates);
        var zhangYanStates = _characterZhangYanMobileShiOperations.MobileShiZhangYanPublicStates(owner, skillId);
        if (zhangYanStates.Count > 0)
            (states ??= []).AddRange(zhangYanStates);
        var dengAiStates = _characterDengAiMobileShiOperations.MobileShiDengAiPublicStates(owner, skillId);
        if (dengAiStates.Count > 0)
            (states ??= []).AddRange(dengAiStates);
        var taiShiCiStates = _characterTaiShiCiMobileShiOperations.ShiTaiShiCiPublicStates(owner, skillId);
        if (taiShiCiStates.Count > 0)
            (states ??= []).AddRange(taiShiCiStates);
        var quanCongStates = _characterQuanCongBoundaryMobileOperations.BoundaryQuanCongPublicStates(owner, skillId);
        if (quanCongStates.Count > 0)
            (states ??= []).AddRange(quanCongStates);
        var zhuZhiStates = _characterZhuZhiBoundaryMobileOperations.BoundaryZhuZhiPublicStates(owner, skillId);
        if (zhuZhiStates.Count > 0)
            (states ??= []).AddRange(zhuZhiStates);
        var yanYanStates = _characterYanYanBoundaryMobileOperations.BoundaryYanYanPublicStates(owner, skillId);
        if (yanYanStates.Count > 0)
            (states ??= []).AddRange(yanYanStates);
        return states is null ? existing : Array.AsReadOnly((existing ?? []).Concat(states).ToArray());
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseUsedProjections PhaseUsedProjections { get; }
}

internal interface ICoreGameActivationProgressCoreRoundOnceCardlessBasicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundOnceCardlessBasicUsed(int ownerSeat, string skillId, string group);
}

internal interface ICoreGameActivationProgressCharacterWangChangMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiWangChangPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiLuSuPublicStates(global::CardGame.Core.CharacterState owner, string id);
}

internal interface ICoreGameActivationProgressCharacterHeQiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiHeQiPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterLuYuShengMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiLuYuShengPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreGameActivationProgressCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiGuoYuanPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterChenDaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenDaoPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterXiaoQiaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiXiaoQiaoPublicStates(global::CardGame.Core.CharacterState owner, string id);
}

internal interface ICoreGameActivationProgressCharacterChenQunMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenQunPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterQuanCongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryQuanCongPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterHuangZuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiHuangZuPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get; }
}

internal interface ICoreGameActivationProgressCharacterYanYanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryYanYanPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterDengAiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiDengAiPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterZhangYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiZhangYanPublicStates(global::CardGame.Core.CharacterState owner, string id);
}

internal interface ICoreGameActivationProgressCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGameActivationProgressCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int MouLiuChengPhaseUses(int owner, string skill, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreGameActivationProgressCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiWeiYanPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCoreGameActivationAwakeningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GameProgramActivationCount(int ownerSeat, string skillId, string activationId);
}

internal interface ICoreGameActivationProgressCharacterChenZhiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenZhiPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterSunJunMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiSunJunPublicStates(global::CardGame.Core.CharacterState owner, string id);
}

internal interface ICoreGameActivationProgressCharacterTianFengMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiTianFengPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot>? GetHandComparisonPublicRuleStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterXinXianYingMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> ShiXinXianYingPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterDongZhaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiDongZhaoPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterSunChenMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiSunChenPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterTaiShiCiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> ShiTaiShiCiPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCharacterZhuZhiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryZhuZhiPublicStates(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreGameActivationProgressCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
    bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

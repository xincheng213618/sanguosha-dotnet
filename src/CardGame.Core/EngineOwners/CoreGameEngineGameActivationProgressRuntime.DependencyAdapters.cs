#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsRuntime))]
internal sealed class CoreGameActivationProgressCoreProgramPhaseUsedProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseUsedProjections PhaseUsedProjections { get => this._targetFactory().PhaseUsedProjections; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime))]
internal sealed class CoreGameActivationProgressCoreRoundOnceCardlessBasicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreRoundOnceCardlessBasicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> _targetFactory = targetFactory;
    public bool RoundOnceCardlessBasicUsed(int ownerSeat, string skillId, string group) => this._targetFactory().RoundOnceCardlessBasicUsed(ownerSeat, skillId, group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterWangChangMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterWangChangMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiWangChangPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiWangChangPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiLuSuPublicStates(global::CardGame.Core.CharacterState owner, string id) => this._targetFactory().MobileShiLuSuPublicStates(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterHeQiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterHeQiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiHeQiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiHeQiPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterLuYuShengMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterLuYuShengMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiLuYuShengPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiLuYuShengPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterGuoYuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterGuoYuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiGuoYuanPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiGuoYuanPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterChenDaoMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterChenDaoMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenDaoPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiChenDaoPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterXiaoQiaoMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterXiaoQiaoMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiXiaoQiaoPublicStates(global::CardGame.Core.CharacterState owner, string id) => this._targetFactory().MobileShiXiaoQiaoPublicStates(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterChenQunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterChenQunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenQunPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiChenQunPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime))]
internal sealed class CoreGameActivationProgressCharacterQuanCongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterQuanCongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryQuanCongPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().BoundaryQuanCongPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterHuangZuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterHuangZuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiHuangZuPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiHuangZuPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CoreGameActivationProgressCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get => this._targetFactory().ProgramSelections; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime))]
internal sealed class CoreGameActivationProgressCharacterYanYanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterYanYanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryYanYanPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().BoundaryYanYanPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterDengAiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterDengAiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiDengAiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiDengAiPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterZhangYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterZhangYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiZhangYanPublicStates(global::CardGame.Core.CharacterState owner, string id) => this._targetFactory().MobileShiZhangYanPublicStates(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreGameActivationProgressCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed class CoreGameActivationProgressCharacterLiuChengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterLiuChengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _targetFactory = targetFactory;
    public int MouLiuChengPhaseUses(int owner, string skill, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().MouLiuChengPhaseUses(owner, skill, phase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterWeiYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterWeiYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiWeiYanPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiWeiYanPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationAwakeningRuntime))]
internal sealed class CoreGameActivationProgressCoreGameActivationAwakeningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreGameActivationAwakeningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> _targetFactory = targetFactory;
    public int GameProgramActivationCount(int ownerSeat, string skillId, string activationId) => this._targetFactory().GameProgramActivationCount(ownerSeat, skillId, activationId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterChenZhiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterChenZhiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenZhiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiChenZhiPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterSunJunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterSunJunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiSunJunPublicStates(global::CardGame.Core.CharacterState owner, string id) => this._targetFactory().MobileShiSunJunPublicStates(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterTianFengMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterTianFengMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiTianFengPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiTianFengPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreGameActivationProgressCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot>? GetHandComparisonPublicRuleStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().GetHandComparisonPublicRuleStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterXinXianYingMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterXinXianYingMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> ShiXinXianYingPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().ShiXinXianYingPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongZhaoMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterDongZhaoMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterDongZhaoMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiDongZhaoPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiDongZhaoPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterSunChenMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterSunChenMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiSunChenPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().MobileShiSunChenPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaiShiCiMobileShiRuntime))]
internal sealed class CoreGameActivationProgressCharacterTaiShiCiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaiShiCiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterTaiShiCiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaiShiCiMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> ShiTaiShiCiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().ShiTaiShiCiPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime))]
internal sealed class CoreGameActivationProgressCharacterZhuZhiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCharacterZhuZhiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryZhuZhiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().BoundaryZhuZhiPublicStates(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreGameActivationProgressCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => this._targetFactory().CurrentActualDiscardRecoveryPhase();
    public bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().IsRecordedActualDiscardRecoveryPhase(phase);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGameActivationProgressRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChenDaoMobileShiEntries _characterChenDaoMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChenQunMobileShiEntries _characterChenQunMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChenZhiMobileShiEntries _characterChenZhiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDengAiMobileShiEntries _characterDengAiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongZhaoMobileShiEntries _characterDongZhaoMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoYuanMobileShiEntries _characterGuoYuanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeQiMobileShiEntries _characterHeQiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries _characterHuangZuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuYuShengMobileShiEntries _characterLuYuShengMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQuanCongBoundaryMobileEntries _characterQuanCongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunChenMobileShiEntries _characterSunChenMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunJunMobileShiEntries _characterSunJunMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTaiShiCiMobileShiEntries _characterTaiShiCiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTianFengMobileShiEntries _characterTianFengMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWangChangMobileShiEntries _characterWangChangMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries _characterWeiYanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMobileShiEntries _characterXiaoQiaoMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXinXianYingMobileShiEntries _characterXinXianYingMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanYanBoundaryMobileEntries _characterYanYanBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanMobileShiEntries _characterZhangYanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhuZhiBoundaryMobileEntries _characterZhuZhiBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries _coreActualDiscardRecoveryPhasesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGameActivationAwakeningEntries _coreGameActivationAwakeningOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries _coreHandComparisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessBasicEntries _coreRoundOnceCardlessBasicOperations;
    private readonly ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations _coreProgramPhaseUsedProjectionsOperations;
    private readonly ICoreGameActivationProgressCoreProgramSelectionModulesOperations _coreProgramSelectionModulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess;
    internal CoreGameEngineGameActivationProgressRuntime(global::CardGame.Core.RuntimeEntries.CharacterChenDaoMobileShiEntries characterChenDaoMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterChenQunMobileShiEntries characterChenQunMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterChenZhiMobileShiEntries characterChenZhiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterDengAiMobileShiEntries characterDengAiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterDongZhaoMobileShiEntries characterDongZhaoMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoYuanMobileShiEntries characterGuoYuanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterHeQiMobileShiEntries characterHeQiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries characterHuangZuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterLuYuShengMobileShiEntries characterLuYuShengMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterQuanCongBoundaryMobileEntries characterQuanCongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterSunChenMobileShiEntries characterSunChenMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterSunJunMobileShiEntries characterSunJunMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterTaiShiCiMobileShiEntries characterTaiShiCiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterTianFengMobileShiEntries characterTianFengMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterWangChangMobileShiEntries characterWangChangMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries characterWeiYanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMobileShiEntries characterXiaoQiaoMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterXinXianYingMobileShiEntries characterXinXianYingMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterYanYanBoundaryMobileEntries characterYanYanBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanMobileShiEntries characterZhangYanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterZhuZhiBoundaryMobileEntries characterZhuZhiBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries coreActualDiscardRecoveryPhasesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGameActivationAwakeningEntries coreGameActivationAwakeningOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries coreHandComparisonOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessBasicEntries coreRoundOnceCardlessBasicOperations, ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations coreProgramPhaseUsedProjectionsOperations, ICoreGameActivationProgressCoreProgramSelectionModulesOperations coreProgramSelectionModulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess)
    {
        this._characterChenDaoMobileShiOperations = characterChenDaoMobileShiOperations;
        this._characterChenQunMobileShiOperations = characterChenQunMobileShiOperations;
        this._characterChenZhiMobileShiOperations = characterChenZhiMobileShiOperations;
        this._characterDengAiMobileShiOperations = characterDengAiMobileShiOperations;
        this._characterDongZhaoMobileShiOperations = characterDongZhaoMobileShiOperations;
        this._characterGuoYuanMobileShiOperations = characterGuoYuanMobileShiOperations;
        this._characterHeQiMobileShiOperations = characterHeQiMobileShiOperations;
        this._characterHuangZuMobileShiOperations = characterHuangZuMobileShiOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterLuYuShengMobileShiOperations = characterLuYuShengMobileShiOperations;
        this._characterQuanCongBoundaryMobileOperations = characterQuanCongBoundaryMobileOperations;
        this._characterSunChenMobileShiOperations = characterSunChenMobileShiOperations;
        this._characterSunJunMobileShiOperations = characterSunJunMobileShiOperations;
        this._characterTaiShiCiMobileShiOperations = characterTaiShiCiMobileShiOperations;
        this._characterTianFengMobileShiOperations = characterTianFengMobileShiOperations;
        this._characterWangChangMobileShiOperations = characterWangChangMobileShiOperations;
        this._characterWeiYanMobileShiOperations = characterWeiYanMobileShiOperations;
        this._characterXiaoQiaoMobileShiOperations = characterXiaoQiaoMobileShiOperations;
        this._characterXinXianYingMobileShiOperations = characterXinXianYingMobileShiOperations;
        this._characterYanYanBoundaryMobileOperations = characterYanYanBoundaryMobileOperations;
        this._characterZhangYanMobileShiOperations = characterZhangYanMobileShiOperations;
        this._characterZhuZhiBoundaryMobileOperations = characterZhuZhiBoundaryMobileOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreGameActivationAwakeningOperations = coreGameActivationAwakeningOperations;
        this._coreHandComparisonOperations = coreHandComparisonOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreRoundOnceCardlessBasicOperations = coreRoundOnceCardlessBasicOperations;
        this._coreProgramPhaseUsedProjectionsOperations = coreProgramPhaseUsedProjectionsOperations;
        this._coreProgramSelectionModulesOperations = coreProgramSelectionModulesOperations;
        this._coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess = coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess;
    }
}

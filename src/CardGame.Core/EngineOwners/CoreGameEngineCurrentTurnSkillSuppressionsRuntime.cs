#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries _characterCaoPiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoPiMouEntries _characterCaoPiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoMobileGodEntries _characterMaChaoMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWangGuanOlEntries _characterWangGuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiahouJieAnniversaryEntries _characterXiahouJieAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries _characterZhangLiaoMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangRenMobileShiEntries _characterZhangRenMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries _characterZouShiLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries _coreAlcoholSlashSuppressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAnqiFeirenEntries _coreAnqiFeirenOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduActivitiesEntries _coreGuanduActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeFactionChangesEntries _coreNativeFactionChangesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuExpandedPoliciesEntries _coreShenwuExpandedPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangOriginalSkillRulesEntries _coreWushuangOriginalSkillRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess;
    internal CoreGameEngineCurrentTurnSkillSuppressionsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries characterCaoPiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoPiMouEntries characterCaoPiMouOperations, global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoMobileGodEntries characterMaChaoMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterWangGuanOlEntries characterWangGuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXiahouJieAnniversaryEntries characterXiahouJieAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries characterZhangLiaoMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangRenMobileShiEntries characterZhangRenMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries characterZouShiLeOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries coreAlcoholSlashSuppressionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAnqiFeirenEntries coreAnqiFeirenOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduActivitiesEntries coreGuanduActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeFactionChangesEntries coreNativeFactionChangesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuExpandedPoliciesEntries coreShenwuExpandedPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangOriginalSkillRulesEntries coreWushuangOriginalSkillRulesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess)
    {
        this._characterCaoPiAnniversaryGodOperations = characterCaoPiAnniversaryGodOperations;
        this._characterCaoPiMouOperations = characterCaoPiMouOperations;
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterMaChaoMobileGodOperations = characterMaChaoMobileGodOperations;
        this._characterWangGuanOlOperations = characterWangGuanOlOperations;
        this._characterXiahouJieAnniversaryOperations = characterXiahouJieAnniversaryOperations;
        this._characterZhangLiaoMobileGodOperations = characterZhangLiaoMobileGodOperations;
        this._characterZhangRenMobileShiOperations = characterZhangRenMobileShiOperations;
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._characterZouShiLeOperations = characterZouShiLeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlcoholSlashSuppressionsOperations = coreAlcoholSlashSuppressionsOperations;
        this._coreAnqiFeirenOperations = coreAnqiFeirenOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDyingWindowSkillPoliciesOperations = coreDyingWindowSkillPoliciesOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeFactionChangesOperations = coreNativeFactionChangesOperations;
        this._coreShenwuExpandedPoliciesOperations = coreShenwuExpandedPoliciesOperations;
        this._coreWushuangOriginalSkillRulesOperations = coreWushuangOriginalSkillRulesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess = coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess;
    }
}

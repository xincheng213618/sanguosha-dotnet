#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineFinalTargetSlashRuntime : global::CardGame.Core.IFinalTargetSlashProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries _characterHuangZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAssistedFactionSlashEntries _coreAssistedFactionSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries _coreProgramPersistentCombatPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries _coreTurnRedSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeQinglongEntries _coreNativeQinglongOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly ICoreFinalTargetSlashCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICoreFinalTargetSlashCoreProgramDistanceRulesOperations _coreProgramDistanceRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineFinalTargetSlashRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries characterHuangZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAssistedFactionSlashEntries coreAssistedFactionSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries coreProgramPersistentCombatPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries coreTurnRedSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeQinglongEntries coreNativeQinglongOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, ICoreFinalTargetSlashCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICoreFinalTargetSlashCoreProgramDistanceRulesOperations coreProgramDistanceRulesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterHuangZhongBoundaryMobileOperations = characterHuangZhongBoundaryMobileOperations;
        this._characterHuangfuSongWumiaoOperations = characterHuangfuSongWumiaoOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreAssistedFactionSlashOperations = coreAssistedFactionSlashOperations;
        this._coreJudgedRankSlashPoliciesOperations = coreJudgedRankSlashPoliciesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramPersistentCombatPoliciesOperations = coreProgramPersistentCombatPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeQinglongOperations = coreNativeQinglongOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreProgramDistanceRulesOperations = coreProgramDistanceRulesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNoDamageSkillDisableRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess _coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess;
    internal CoreGameEngineNoDamageSkillDisableRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess = coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess;
        this._coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess = coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterFanJiangZhangDaOlRuntime : global::CardGame.Core.IFanJiangZhangDaHost, global::CardGame.Core.IJueshengTargetQuery
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLifetimeSlashUsesEntries _coreLifetimeSlashUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries _coreSkillOwnershipWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery _resolutionStackOperations2;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterFanJiangZhangDaOlRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineLifetimeSlashUsesEntries coreLifetimeSlashUsesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries coreSkillOwnershipWindowsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations,global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery resolutionStackOperations2,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreLifetimeSlashUsesOperations = coreLifetimeSlashUsesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingProgramsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries _characterCuiYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries _characterHuangYueyingOlMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhuJunOlEntries _characterZhuJunOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries _coreActualTurnLegacyCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDyingAlcoholReturnsEntries _coreBoundDyingAlcoholReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardRecoveryModifiersEntries _coreCardRecoveryModifiersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries _coreNativeRecoveryFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDyingProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries characterCuiYanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries characterHuangYueyingOlMouOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhuJunOlEntries characterZhuJunOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries coreActualTurnLegacyCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDyingAlcoholReturnsEntries coreBoundDyingAlcoholReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardRecoveryModifiersEntries coreCardRecoveryModifiersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries coreNativeRecoveryFramesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCuiYanOlOperations = characterCuiYanOlOperations;
        this._characterHuangYueyingOlMouOperations = characterHuangYueyingOlMouOperations;
        this._characterLiuYanOlOperations = characterLiuYanOlOperations;
        this._characterZhuJunOlOperations = characterZhuJunOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActualTurnLegacyCompletionOperations = coreActualTurnLegacyCompletionOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreBoundDyingAlcoholReturnsOperations = coreBoundDyingAlcoholReturnsOperations;
        this._coreCardRecoveryModifiersOperations = coreCardRecoveryModifiersOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

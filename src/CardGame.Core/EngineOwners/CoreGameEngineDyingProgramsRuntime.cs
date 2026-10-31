#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingProgramsRuntime
{
    private readonly ICoreDyingProgramsCharacterCuiYanOlOperations _characterCuiYanOlOperations;
    private readonly ICoreDyingProgramsCharacterHuangYueyingOlMouOperations _characterHuangYueyingOlMouOperations;
    private readonly ICoreDyingProgramsCharacterLiuYanOlOperations _characterLiuYanOlOperations;
    private readonly ICoreDyingProgramsCharacterZhuJunOlOperations _characterZhuJunOlOperations;
    private readonly ICoreDyingProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreDyingProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreDyingProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDyingProgramsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreDyingProgramsCoreActualTurnLegacyCompletionOperations _coreActualTurnLegacyCompletionOperations;
    private readonly ICoreDyingProgramsCoreAlcoholIdentityPoliciesOperations _coreAlcoholIdentityPoliciesOperations;
    private readonly ICoreDyingProgramsCoreBoundDyingAlcoholReturnsOperations _coreBoundDyingAlcoholReturnsOperations;
    private readonly ICoreDyingProgramsCoreCardRecoveryModifiersOperations _coreCardRecoveryModifiersOperations;
    private readonly ICoreDyingProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreDyingProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreDyingProgramsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreDyingProgramsCoreNativeCardUseOperations _coreNativeCardUseOperations;
    private readonly ICoreDyingProgramsCoreNativeCardUseCompletionOperations _coreNativeCardUseCompletionOperations;
    private readonly ICoreDyingProgramsCoreNativeCardUseFramesOperations _coreNativeCardUseFramesOperations;
    private readonly ICoreDyingProgramsCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICoreDyingProgramsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreDyingProgramsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreDyingProgramsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreDyingProgramsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreDyingProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDyingProgramsRuntime(ICoreDyingProgramsCharacterCuiYanOlOperations characterCuiYanOlOperations, ICoreDyingProgramsCharacterHuangYueyingOlMouOperations characterHuangYueyingOlMouOperations, ICoreDyingProgramsCharacterLiuYanOlOperations characterLiuYanOlOperations, ICoreDyingProgramsCharacterZhuJunOlOperations characterZhuJunOlOperations, ICoreDyingProgramsCardZonesOperations cardZonesOperations, ICoreDyingProgramsContentRegistryOperations contentRegistryOperations, ICoreDyingProgramsResolutionStackOperations resolutionStackOperations, ICoreDyingProgramsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreDyingProgramsCoreActualTurnLegacyCompletionOperations coreActualTurnLegacyCompletionOperations, ICoreDyingProgramsCoreAlcoholIdentityPoliciesOperations coreAlcoholIdentityPoliciesOperations, ICoreDyingProgramsCoreBoundDyingAlcoholReturnsOperations coreBoundDyingAlcoholReturnsOperations, ICoreDyingProgramsCoreCardRecoveryModifiersOperations coreCardRecoveryModifiersOperations, ICoreDyingProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreDyingProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreDyingProgramsCoreMatchLogOperations coreMatchLogOperations, ICoreDyingProgramsCoreNativeCardUseOperations coreNativeCardUseOperations, ICoreDyingProgramsCoreNativeCardUseCompletionOperations coreNativeCardUseCompletionOperations, ICoreDyingProgramsCoreNativeCardUseFramesOperations coreNativeCardUseFramesOperations, ICoreDyingProgramsCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICoreDyingProgramsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreDyingProgramsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreDyingProgramsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreDyingProgramsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreDyingProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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

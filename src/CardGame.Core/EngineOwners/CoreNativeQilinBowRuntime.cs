#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeQilinBowRuntime
{
    private readonly ICoreNativeQilinBowCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreNativeQilinBowCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreNativeQilinBowCoreFieldEquipmentOperations _coreFieldEquipmentOperations;
    private readonly ICoreNativeQilinBowCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreNativeQilinBowCoreNativeCardUseFramesOperations _coreNativeCardUseFramesOperations;
    private readonly ICoreNativeQilinBowCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreNativeQilinBowCoreNativeEquipmentInvocationsOperations _coreNativeEquipmentInvocationsOperations;
    private readonly ICoreNativeQilinBowCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreNativeQilinBowCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreNativeQilinBowCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreNativeQilinBowKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeQilinBowRuntime(ICoreNativeQilinBowCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreNativeQilinBowCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreNativeQilinBowCoreFieldEquipmentOperations coreFieldEquipmentOperations, ICoreNativeQilinBowCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreNativeQilinBowCoreNativeCardUseFramesOperations coreNativeCardUseFramesOperations, ICoreNativeQilinBowCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreNativeQilinBowCoreNativeEquipmentInvocationsOperations coreNativeEquipmentInvocationsOperations, ICoreNativeQilinBowCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreNativeQilinBowCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreNativeQilinBowCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreNativeQilinBowKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeEquipmentInvocationsOperations = coreNativeEquipmentInvocationsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

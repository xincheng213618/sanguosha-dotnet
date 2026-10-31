#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAttackRangeAidRuntime
{
    private readonly ICoreProgramAttackRangeAidCharacterHanDangBoundaryMobileOperations _characterHanDangBoundaryMobileOperations;
    private readonly ICoreProgramAttackRangeAidCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramAttackRangeAidContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramAttackRangeAidResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramAttackRangeAidCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICoreProgramAttackRangeAidCoreProgramDodgeResponsesOperations _coreProgramDodgeResponsesOperations;
    private readonly ICoreProgramAttackRangeAidCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramAttackRangeAidCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramAttackRangeAidCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreProgramAttackRangeAidCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramAttackRangeAidCoreNativeAiDecisionsOperations _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramAttackRangeAidCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramAttackRangeAidCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramAttackRangeAidCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreProgramAttackRangeAidCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramAttackRangeAidCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramAttackRangeAidKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramAttackRangeAidRuntime(ICoreProgramAttackRangeAidCharacterHanDangBoundaryMobileOperations characterHanDangBoundaryMobileOperations, ICoreProgramAttackRangeAidCardZonesOperations cardZonesOperations, ICoreProgramAttackRangeAidContentRegistryOperations contentRegistryOperations, ICoreProgramAttackRangeAidResolutionStackOperations resolutionStackOperations, ICoreProgramAttackRangeAidCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICoreProgramAttackRangeAidCoreProgramDodgeResponsesOperations coreProgramDodgeResponsesOperations, ICoreProgramAttackRangeAidCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramAttackRangeAidCoreRuntimeOperations coreRuntimeOperations, ICoreProgramAttackRangeAidCoreTableOrderOperations coreTableOrderOperations, ICoreProgramAttackRangeAidCoreMatchLogOperations coreMatchLogOperations, ICoreProgramAttackRangeAidCoreNativeAiDecisionsOperations coreNativeAiDecisionsOperations, ICoreProgramAttackRangeAidCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramAttackRangeAidCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramAttackRangeAidCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreProgramAttackRangeAidCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramAttackRangeAidCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramAttackRangeAidKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHanDangBoundaryMobileOperations = characterHanDangBoundaryMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

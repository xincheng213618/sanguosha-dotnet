#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramPhaseHandDiscardsRuntime
{
    private readonly ICoreProgramPhaseHandDiscardsCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramPhaseHandDiscardsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramPhaseHandDiscardsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreProgramCardPaymentsOperations _coreProgramCardPaymentsOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreNativeAiDecisionsOperations _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramPhaseHandDiscardsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramPhaseHandDiscardsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramPhaseHandDiscardsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramPhaseHandDiscardsRuntime(ICoreProgramPhaseHandDiscardsCardZonesOperations cardZonesOperations, ICoreProgramPhaseHandDiscardsContentRegistryOperations contentRegistryOperations, ICoreProgramPhaseHandDiscardsResolutionStackOperations resolutionStackOperations, ICoreProgramPhaseHandDiscardsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramPhaseHandDiscardsCoreProgramCardPaymentsOperations coreProgramCardPaymentsOperations, ICoreProgramPhaseHandDiscardsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramPhaseHandDiscardsCoreRuntimeOperations coreRuntimeOperations, ICoreProgramPhaseHandDiscardsCoreMatchLogOperations coreMatchLogOperations, ICoreProgramPhaseHandDiscardsCoreNativeAiDecisionsOperations coreNativeAiDecisionsOperations, ICoreProgramPhaseHandDiscardsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramPhaseHandDiscardsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramPhaseHandDiscardsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

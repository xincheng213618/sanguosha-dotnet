#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramHoldCardsRuntime
{
    private readonly ICoreProgramHoldCardsCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramHoldCardsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramHoldCardsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramHoldCardsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramHoldCardsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramHoldCardsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramHoldCardsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramHoldCardsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramHoldCardsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramHoldCardsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreProgramHoldCardsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramHoldCardsRuntime(ICoreProgramHoldCardsCardZonesOperations cardZonesOperations, ICoreProgramHoldCardsContentRegistryOperations contentRegistryOperations, ICoreProgramHoldCardsResolutionStackOperations resolutionStackOperations, ICoreProgramHoldCardsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramHoldCardsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramHoldCardsCoreRuntimeOperations coreRuntimeOperations, ICoreProgramHoldCardsCoreMatchLogOperations coreMatchLogOperations, ICoreProgramHoldCardsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramHoldCardsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramHoldCardsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreProgramHoldCardsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

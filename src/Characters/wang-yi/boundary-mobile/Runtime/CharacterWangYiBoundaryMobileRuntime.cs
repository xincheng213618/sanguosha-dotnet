#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterWangYiBoundaryMobileRuntime : global::CardGame.Core.IBoundaryWangYiHost
{
    private readonly ICharacterWangYiBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterWangYiBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterWangYiBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterWangYiBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterWangYiBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterWangYiBoundaryMobileCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterWangYiBoundaryMobileCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterWangYiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterWangYiBoundaryMobileRuntime(ICharacterWangYiBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterWangYiBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterWangYiBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterWangYiBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterWangYiBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterWangYiBoundaryMobileCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterWangYiBoundaryMobileCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterWangYiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

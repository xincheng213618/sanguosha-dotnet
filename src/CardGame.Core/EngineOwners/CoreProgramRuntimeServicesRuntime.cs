#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramRuntimeServicesRuntime : global::CardGame.Core.IProgramRuntimeEvents, global::CardGame.Core.IProgramRuntimePrompts
{
    private readonly ICoreProgramRuntimeServicesCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramRuntimeServicesCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramRuntimeServicesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    internal CoreProgramRuntimeServicesRuntime(ICoreProgramRuntimeServicesCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramRuntimeServicesCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramRuntimeServicesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess)
    {
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
    }
}

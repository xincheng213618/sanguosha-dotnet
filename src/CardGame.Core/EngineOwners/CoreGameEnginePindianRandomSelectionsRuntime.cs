#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianRandomSelectionsRuntime
{
    private readonly ICorePindianRandomSelectionsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePindianRandomSelectionsRandomOperations _randomOperations;
    private readonly ICorePindianRandomSelectionsCorePindianOperations _corePindianOperations;
    private readonly ICorePindianRandomSelectionsCorePindianPoliciesOperations _corePindianPoliciesOperations;
    private readonly ICorePindianRandomSelectionsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePindianRandomSelectionsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePindianRandomSelectionsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICorePindianRandomSelectionsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICorePindianRandomSelectionsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICorePindianRandomSelectionsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianRandomSelectionsRuntime(ICorePindianRandomSelectionsContentRegistryOperations contentRegistryOperations, ICorePindianRandomSelectionsRandomOperations randomOperations, ICorePindianRandomSelectionsCorePindianOperations corePindianOperations, ICorePindianRandomSelectionsCorePindianPoliciesOperations corePindianPoliciesOperations, ICorePindianRandomSelectionsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePindianRandomSelectionsCoreRuntimeOperations coreRuntimeOperations, ICorePindianRandomSelectionsCoreMatchLogOperations coreMatchLogOperations, ICorePindianRandomSelectionsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICorePindianRandomSelectionsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICorePindianRandomSelectionsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._corePindianOperations = corePindianOperations;
        this._corePindianPoliciesOperations = corePindianPoliciesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSoleTargetReplacementCandidatesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries _coreSoleTargetQuarantineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries _coreSoleTargetRedirectOperations;
    internal CoreGameEngineSoleTargetReplacementCandidatesRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries coreSoleTargetQuarantineOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries coreSoleTargetRedirectOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreSoleTargetProtectionOperations = coreSoleTargetProtectionOperations;
        this._coreSoleTargetQuarantineOperations = coreSoleTargetQuarantineOperations;
        this._coreSoleTargetRedirectOperations = coreSoleTargetRedirectOperations;
    }
}

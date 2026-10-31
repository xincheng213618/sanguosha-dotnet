#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSoleTargetReplacementCandidatesRuntime
{
    private readonly ICoreSoleTargetReplacementCandidatesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSoleTargetReplacementCandidatesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreSoleTargetReplacementCandidatesCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreSoleTargetReplacementCandidatesCoreSoleTargetProtectionOperations _coreSoleTargetProtectionOperations;
    private readonly ICoreSoleTargetReplacementCandidatesCoreSoleTargetQuarantineOperations _coreSoleTargetQuarantineOperations;
    private readonly ICoreSoleTargetReplacementCandidatesCoreSoleTargetRedirectOperations _coreSoleTargetRedirectOperations;
    internal CoreGameEngineSoleTargetReplacementCandidatesRuntime(ICoreSoleTargetReplacementCandidatesResolutionStackOperations resolutionStackOperations, ICoreSoleTargetReplacementCandidatesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreSoleTargetReplacementCandidatesCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreSoleTargetReplacementCandidatesCoreSoleTargetProtectionOperations coreSoleTargetProtectionOperations, ICoreSoleTargetReplacementCandidatesCoreSoleTargetQuarantineOperations coreSoleTargetQuarantineOperations, ICoreSoleTargetReplacementCandidatesCoreSoleTargetRedirectOperations coreSoleTargetRedirectOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreSoleTargetProtectionOperations = coreSoleTargetProtectionOperations;
        this._coreSoleTargetQuarantineOperations = coreSoleTargetQuarantineOperations;
        this._coreSoleTargetRedirectOperations = coreSoleTargetRedirectOperations;
    }
}

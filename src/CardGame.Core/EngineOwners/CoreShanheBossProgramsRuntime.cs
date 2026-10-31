#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreShanheBossProgramsRuntime
{
    private readonly ICoreShanheBossProgramsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreShanheBossProgramsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    internal CoreShanheBossProgramsRuntime(ICoreShanheBossProgramsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreShanheBossProgramsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
    }
}

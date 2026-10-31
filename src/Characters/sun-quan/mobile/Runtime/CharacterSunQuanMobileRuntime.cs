#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterSunQuanMobileRuntime
{
    private readonly ICharacterSunQuanMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterSunQuanMobileCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICharacterSunQuanMobileCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    internal CharacterSunQuanMobileRuntime(ICharacterSunQuanMobileResolutionStackOperations resolutionStackOperations, ICharacterSunQuanMobileCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICharacterSunQuanMobileCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
    }
}

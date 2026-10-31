#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterLuSuMobileRuntime
{
    private readonly ICharacterLuSuMobileCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterLuSuMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLuSuMobileCoreRuntimeOperations _coreRuntimeOperations;
    internal CharacterLuSuMobileRuntime(ICharacterLuSuMobileCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterLuSuMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLuSuMobileCoreRuntimeOperations coreRuntimeOperations)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
    }
}

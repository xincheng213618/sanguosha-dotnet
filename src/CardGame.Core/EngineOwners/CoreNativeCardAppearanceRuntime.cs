#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardAppearanceRuntime
{
    private readonly ICoreNativeCardAppearanceCharacterZhangFeiAnniversaryGodOperations _characterZhangFeiAnniversaryGodOperations;
    private readonly ICoreNativeCardAppearanceCoreCardDeclarationsOperations _coreCardDeclarationsOperations;
    private readonly ICoreNativeCardAppearanceCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    internal CoreNativeCardAppearanceRuntime(ICoreNativeCardAppearanceCharacterZhangFeiAnniversaryGodOperations characterZhangFeiAnniversaryGodOperations, ICoreNativeCardAppearanceCoreCardDeclarationsOperations coreCardDeclarationsOperations, ICoreNativeCardAppearanceCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations)
    {
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardAppearanceRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    internal CoreNativeCardAppearanceRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations)
    {
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
    }
}

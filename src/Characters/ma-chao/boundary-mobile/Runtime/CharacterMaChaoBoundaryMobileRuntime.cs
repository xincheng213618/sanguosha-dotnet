#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterMaChaoBoundaryMobileRuntime
{
    private readonly ICharacterMaChaoBoundaryMobileCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICharacterMaChaoBoundaryMobileCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICharacterMaChaoBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    internal CharacterMaChaoBoundaryMobileRuntime(ICharacterMaChaoBoundaryMobileCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICharacterMaChaoBoundaryMobileCoreCardAttackStateOperations coreCardAttackStateOperations, ICharacterMaChaoBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations)
    {
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
    }
}

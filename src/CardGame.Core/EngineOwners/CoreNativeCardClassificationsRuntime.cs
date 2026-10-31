#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardClassificationsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoMobileGodEntries _characterMaChaoMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    internal CoreNativeCardClassificationsRuntime(global::CardGame.Core.RuntimeEntries.CharacterMaChaoMobileGodEntries characterMaChaoMobileGodOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations)
    {
        this._characterMaChaoMobileGodOperations = characterMaChaoMobileGodOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
    }
}

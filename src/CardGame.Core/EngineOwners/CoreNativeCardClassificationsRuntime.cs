#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardClassificationsRuntime
{
    private readonly ICoreNativeCardClassificationsCharacterMaChaoMobileGodOperations _characterMaChaoMobileGodOperations;
    private readonly ICoreNativeCardClassificationsCoreNativeDamagePoliciesOperations _coreNativeDamagePoliciesOperations;
    internal CoreNativeCardClassificationsRuntime(ICoreNativeCardClassificationsCharacterMaChaoMobileGodOperations characterMaChaoMobileGodOperations, ICoreNativeCardClassificationsCoreNativeDamagePoliciesOperations coreNativeDamagePoliciesOperations)
    {
        this._characterMaChaoMobileGodOperations = characterMaChaoMobileGodOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
    }
}

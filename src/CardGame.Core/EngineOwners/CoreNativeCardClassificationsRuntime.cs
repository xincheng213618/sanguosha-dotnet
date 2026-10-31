#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardClassificationsRuntime
{
    private readonly RuntimeEntries.CharacterMaChaoMobileGodEntries _characterMaChaoMobileGodOperations;
    private readonly RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    internal CoreNativeCardClassificationsRuntime(RuntimeEntries.CharacterMaChaoMobileGodEntries characterMaChaoMobileGodOperations, RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations)
    {
        this._characterMaChaoMobileGodOperations = characterMaChaoMobileGodOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
    }
}

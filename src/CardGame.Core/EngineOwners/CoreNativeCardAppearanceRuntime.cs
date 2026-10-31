#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardAppearanceRuntime
{
    private readonly RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    internal CoreNativeCardAppearanceRuntime(RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations)
    {
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
    }
}

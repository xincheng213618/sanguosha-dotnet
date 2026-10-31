#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseCompletionZhangbaRuntime
{
    private readonly RuntimeEntries.CharacterZhaoYunMobileShiEntries _characterZhaoYunMobileShiOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    internal CoreGameEngineResponseCompletionZhangbaRuntime(RuntimeEntries.CharacterZhaoYunMobileShiEntries characterZhaoYunMobileShiOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations)
    {
        this._characterZhaoYunMobileShiOperations = characterZhaoYunMobileShiOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
    }
}

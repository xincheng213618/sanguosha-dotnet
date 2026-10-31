#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseCompletionZhangbaRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoYunMobileShiEntries _characterZhaoYunMobileShiOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    internal CoreGameEngineResponseCompletionZhangbaRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhaoYunMobileShiEntries characterZhaoYunMobileShiOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations)
    {
        this._characterZhaoYunMobileShiOperations = characterZhaoYunMobileShiOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
    }
}

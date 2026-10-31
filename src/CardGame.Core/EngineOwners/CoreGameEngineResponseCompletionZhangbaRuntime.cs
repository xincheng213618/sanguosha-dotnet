#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseCompletionZhangbaRuntime
{
    private readonly ICoreResponseCompletionZhangbaCharacterZhaoYunMobileShiOperations _characterZhaoYunMobileShiOperations;
    private readonly ICoreResponseCompletionZhangbaContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreResponseCompletionZhangbaCoreActionDiscardColorsOperations _coreActionDiscardColorsOperations;
    private readonly ICoreResponseCompletionZhangbaCoreNativeOwnedCardQueriesOperations _coreNativeOwnedCardQueriesOperations;
    internal CoreGameEngineResponseCompletionZhangbaRuntime(ICoreResponseCompletionZhangbaCharacterZhaoYunMobileShiOperations characterZhaoYunMobileShiOperations, ICoreResponseCompletionZhangbaContentRegistryOperations contentRegistryOperations, ICoreResponseCompletionZhangbaCoreActionDiscardColorsOperations coreActionDiscardColorsOperations, ICoreResponseCompletionZhangbaCoreNativeOwnedCardQueriesOperations coreNativeOwnedCardQueriesOperations)
    {
        this._characterZhaoYunMobileShiOperations = characterZhaoYunMobileShiOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
    }
}

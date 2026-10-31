#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeFactionSlashCompletionRuntime
{
    private readonly ICoreNativeFactionSlashCompletionResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeFactionSlashCompletionCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    internal CoreNativeFactionSlashCompletionRuntime(ICoreNativeFactionSlashCompletionResolutionStackOperations resolutionStackOperations, ICoreNativeFactionSlashCompletionCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
    }
}

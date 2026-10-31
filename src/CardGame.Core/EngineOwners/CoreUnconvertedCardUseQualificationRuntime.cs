#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreUnconvertedCardUseQualificationRuntime
{
    private readonly ICoreUnconvertedCardUseQualificationResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreUnconvertedCardUseQualificationCoreResponseUseCompletionOperations _coreResponseUseCompletionOperations;
    private readonly ICoreUnconvertedCardUseQualificationCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    internal CoreUnconvertedCardUseQualificationRuntime(ICoreUnconvertedCardUseQualificationResolutionStackOperations resolutionStackOperations, ICoreUnconvertedCardUseQualificationCoreResponseUseCompletionOperations coreResponseUseCompletionOperations, ICoreUnconvertedCardUseQualificationCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
    }
}

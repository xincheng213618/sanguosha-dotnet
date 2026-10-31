#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentObserversEntries _coreDamageJudgmentSuitPaymentObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries _coreProgramJudgmentOutcomesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineZhenyiJudgmentWindowsEntries _coreZhenyiJudgmentWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    internal CoreGameEngineJudgmentOutcomeDamageEdgesRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentObserversEntries coreDamageJudgmentSuitPaymentObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries coreProgramJudgmentOutcomesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineZhenyiJudgmentWindowsEntries coreZhenyiJudgmentWindowsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageJudgmentSuitPaymentObserversOperations = coreDamageJudgmentSuitPaymentObserversOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramJudgmentOutcomesOperations = coreProgramJudgmentOutcomesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreZhenyiJudgmentWindowsOperations = coreZhenyiJudgmentWindowsOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
    }
}

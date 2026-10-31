#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentObserversEntries _coreDamageJudgmentSuitPaymentObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries _coreProgramJudgmentOutcomesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineZhenyiJudgmentWindowsEntries _coreZhenyiJudgmentWindowsOperations;
    private readonly RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    internal CoreGameEngineJudgmentOutcomeDamageEdgesRuntime(IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentObserversEntries coreDamageJudgmentSuitPaymentObserversOperations,RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries coreProgramJudgmentOutcomesOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineZhenyiJudgmentWindowsEntries coreZhenyiJudgmentWindowsOperations,RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations)
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

#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramNativeDrawRuntime
{
    private readonly ICoreProgramNativeDrawNativeDrawProducerOperations _nativeDrawProducerOperations;
    internal CoreGameEngineProgramNativeDrawRuntime(ICoreProgramNativeDrawNativeDrawProducerOperations nativeDrawProducerOperations, RuntimeAccess.IClosedProgramOperationDrawsSource _closedProgramOperationDrawsSource)
    {
        this._nativeDrawProducerOperations = nativeDrawProducerOperations;
        this._closedProgramOperationDrawsSource = _closedProgramOperationDrawsSource;
    }

    private readonly RuntimeAccess.IClosedProgramOperationDrawsSource _closedProgramOperationDrawsSource;
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramNativeDrawRuntime
{
    private readonly ICoreProgramNativeDrawNativeDrawProducerOperations _nativeDrawProducerOperations;
    internal CoreGameEngineProgramNativeDrawRuntime(ICoreProgramNativeDrawNativeDrawProducerOperations nativeDrawProducerOperations, global::CardGame.Core.RuntimeAccess.IClosedProgramOperationDrawsSource _closedProgramOperationDrawsSource)
    {
        this._nativeDrawProducerOperations = nativeDrawProducerOperations;
        this._closedProgramOperationDrawsSource = _closedProgramOperationDrawsSource;
    }

    private readonly global::CardGame.Core.RuntimeAccess.IClosedProgramOperationDrawsSource _closedProgramOperationDrawsSource;
}

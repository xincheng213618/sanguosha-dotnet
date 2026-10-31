#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawOrdinalsRuntime
{
    private readonly ICoreNativeDrawOrdinalsNativeDrawOrdinalTriggerOperations _nativeDrawOrdinalTriggerOperations;
    internal CoreGameEngineNativeDrawOrdinalsRuntime(ICoreNativeDrawOrdinalsNativeDrawOrdinalTriggerOperations nativeDrawOrdinalTriggerOperations)
    {
        this._nativeDrawOrdinalTriggerOperations = nativeDrawOrdinalTriggerOperations;
    }
}

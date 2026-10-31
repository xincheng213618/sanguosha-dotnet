#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePhysicalDrawFacadesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhysicalCardDrawEntries _corePhysicalCardDrawOperations;
    internal CorePhysicalDrawFacadesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEnginePhysicalCardDrawEntries corePhysicalCardDrawOperations)
    {
        this._corePhysicalCardDrawOperations = corePhysicalCardDrawOperations;
    }
}

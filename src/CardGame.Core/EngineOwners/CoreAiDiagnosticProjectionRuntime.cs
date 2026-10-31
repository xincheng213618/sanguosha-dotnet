#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreAiDiagnosticProjectionRuntime
{
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess;
    internal CoreAiDiagnosticProjectionRuntime(global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess)
    {
        this._coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess = coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess;
    }
}

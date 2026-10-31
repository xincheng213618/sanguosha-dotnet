#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreAiDiagnosticProjectionRuntime
{
    private readonly EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess;
    internal CoreAiDiagnosticProjectionRuntime(EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess)
    {
        this._coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess = coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess;
    }
}

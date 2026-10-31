#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreAiDiagnosticProjectionRuntime
{
    internal IReadOnlyList<AiThoughtRecord> AiThoughts => _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiThoughts.AsReadOnly();
    internal IReadOnlyList<AiGeneralThought> AiGeneralThoughts => _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiGeneralThoughts.AsReadOnly();
}

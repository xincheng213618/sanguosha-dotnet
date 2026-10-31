#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreAiDiagnosticProjectionRuntime
{
    internal IReadOnlyList<AiThoughtRecord> AiThoughts => _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiThoughts.AsReadOnly();
    internal IReadOnlyList<AiGeneralThought> AiGeneralThoughts => _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiGeneralThoughts.AsReadOnly();
}

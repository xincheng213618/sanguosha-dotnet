#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess : IProgramRuntimeOperations
{
    ref readonly List<AiThoughtRecord> _aiThoughts { get; }

    ref readonly List<AiGeneralThought> _aiGeneralThoughts { get; }
}

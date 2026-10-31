#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiThoughtRecord> _aiThoughts { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiGeneralThought> _aiGeneralThoughts { get; }
}

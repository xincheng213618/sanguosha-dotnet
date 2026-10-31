#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _thoughtSequence { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiThoughtRecord> _aiThoughts { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiGeneralThought> _aiGeneralThoughts { get; }
}

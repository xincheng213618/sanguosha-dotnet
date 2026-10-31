#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess : IProgramRuntimeOperations
{
    ref int _thoughtSequence { get; }

    ref readonly List<AiThoughtRecord> _aiThoughts { get; }

    ref readonly List<AiGeneralThought> _aiGeneralThoughts { get; }
}

#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreAiDiagnosticsState : global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.AiGeneralThought> _aiGeneralThoughts = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiGeneralThought> ReferenceToAiGeneralThoughts => ref _aiGeneralThoughts;

    private global::System.Collections.Generic.List<global::CardGame.Core.AiThoughtRecord> _aiThoughts = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiThoughtRecord> ReferenceToAiThoughts => ref _aiThoughts;

    private int _thoughtSequence = default !;
    internal ref int ReferenceToThoughtSequence => ref _thoughtSequence;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess._thoughtSequence => ref _thoughtSequence;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence => ref _thoughtSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiThoughtRecord> global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._aiThoughts => ref _aiThoughts;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiGeneralThought> global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._aiGeneralThoughts => ref _aiGeneralThoughts;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiThoughtRecord> global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiThoughts => ref _aiThoughts;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.AiGeneralThought> global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiGeneralThoughts => ref _aiGeneralThoughts;
}

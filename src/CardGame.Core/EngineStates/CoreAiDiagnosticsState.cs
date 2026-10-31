#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreAiDiagnosticsState : Contracts.ICoreAiDiagnosticsThoughtSequenceAccess, Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess, Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess
{
    private List<AiGeneralThought> _aiGeneralThoughts = [];
    internal ref readonly List<AiGeneralThought> ReferenceToAiGeneralThoughts => ref _aiGeneralThoughts;

    private List<AiThoughtRecord> _aiThoughts = [];
    internal ref readonly List<AiThoughtRecord> ReferenceToAiThoughts => ref _aiThoughts;

    private int _thoughtSequence = default !;
    internal ref int ReferenceToThoughtSequence => ref _thoughtSequence;

    ref int Contracts.ICoreAiDiagnosticsThoughtSequenceAccess._thoughtSequence => ref _thoughtSequence;

    ref int Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence => ref _thoughtSequence;

    ref readonly List<AiThoughtRecord> Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._aiThoughts => ref _aiThoughts;

    ref readonly List<AiGeneralThought> Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._aiGeneralThoughts => ref _aiGeneralThoughts;

    ref readonly List<AiThoughtRecord> Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiThoughts => ref _aiThoughts;

    ref readonly List<AiGeneralThought> Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess._aiGeneralThoughts => ref _aiGeneralThoughts;
}

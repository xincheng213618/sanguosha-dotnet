namespace CardGame.Core;

public sealed record ProgramLiejieCostColorFrozenEvent(long FrameId, string SourceBind,
    string SkillInstanceId, string GameplayHash, int Count, int RedCount, long BeforeMoveSequence) : IGameEvent;
public sealed record ProgramLiejieSelectionStartedEvent(long FrameId, int InstructionIndex,
    long ParentFrameId, int SourceSeat, int RedCount, int MaximumCount) : IGameEvent;
public sealed record ProgramLiejieSourceDiscardResolvedEvent(long FrameId, int Count) : IGameEvent;

public sealed record LiejieDiscardPendingState(int SourceSeat, int MaximumCount)
{
    public int InstructionIndex { get; init; }
    public long ParentFrameId { get; init; }
    public int FrozenRedCount { get; init; }
    public bool Paid { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public long? BatchId { get; init; }
    private IReadOnlyList<int> _candidateIds = Array.AsReadOnly(Array.Empty<int>());
    private IReadOnlyList<CardLocation> _candidateLocations = Array.AsReadOnly(Array.Empty<CardLocation>());
    private IReadOnlyList<int> _selectedIds = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> CandidateIds { get => _candidateIds; init => _candidateIds = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardLocation> CandidateLocations { get => _candidateLocations; init => _candidateLocations = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> SelectedIds { get => _selectedIds; init => _selectedIds = Array.AsReadOnly(value.ToArray()); }
}

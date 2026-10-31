#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ObserverSource(Func<Action<GameSnapshot>?> getAction, Func<Action<GameLogEntry>?> getAction2, Func<Action<AiThoughtRecord>?> getAction3, Func<Action<AiGeneralThought>?> getAction4, Func<Action<CardMovementRecord>?> getAction5, Func<Action<EventEnvelope>?> getAction6, Func<RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, EngineStates.CoreCommandJournalStorageState commandJournalStorageState) : IObserverSource
{
    public Action<GameSnapshot>? StateChanged => getAction();
    public Action<GameLogEntry>? LogAdded => getAction2();
    public Action<AiThoughtRecord>? AiThoughtAdded => getAction3();
    public Action<AiGeneralThought>? AiGeneralThoughtAdded => getAction4();
    public Action<CardMovementRecord>? CardMoved => getAction5();
    public Action<EventEnvelope>? EventCommitted => getAction6();

    public IReadOnlySet<long>? CaptureHiddenLogSequences() => getCharacterSimaYiMouRuntime().PrepareMouSmPublicProjection()?.Logs;
    public bool StatePublicationPending { get => commandJournalStorageState.ReferenceToPendingStatePublication; set => commandJournalStorageState.ReferenceToPendingStatePublication = value; }
}

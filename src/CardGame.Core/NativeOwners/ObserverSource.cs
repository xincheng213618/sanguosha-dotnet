#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ObserverSource(global::System.Func<global::System.Action<global::CardGame.Core.GameSnapshot>?> getAction, global::System.Func<global::System.Action<global::CardGame.Core.GameLogEntry>?> getAction2, global::System.Func<global::System.Action<global::CardGame.Core.AiThoughtRecord>?> getAction3, global::System.Func<global::System.Action<global::CardGame.Core.AiGeneralThought>?> getAction4, global::System.Func<global::System.Action<global::CardGame.Core.CardMovementRecord>?> getAction5, global::System.Func<global::System.Action<global::CardGame.Core.EventEnvelope>?> getAction6, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, global::CardGame.Core.EngineStates.CoreCommandJournalStorageState commandJournalStorageState) : IObserverSource
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

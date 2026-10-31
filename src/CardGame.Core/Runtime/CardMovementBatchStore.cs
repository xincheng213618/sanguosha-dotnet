namespace CardGame.Core;

internal interface ICardMovementBatchNativeDrawInvoices : IProgramRuntimeOperations
{
    void AssignNativeDrawInvoice(int pendingIndex, NativeDrawInvocationProof proof);
}

internal interface ICardMovementBatchQueue : IProgramRuntimeOperations
{
    int PendingBatchCount { get; }
    CardMovementBatchContext PendingBatchAt(int index);
    void RemovePendingBatch(CardMovementBatchContext batch);
}

internal interface ICardMovementEventSuppression : IProgramRuntimeOperations
{
    bool HasSuppressedCardMovedEvents { get; }
    void Suppress(long batchId);
    bool TryPopSuppression(out long batchId);
}

internal interface ICardMovementBatchLifecycle : IProgramRuntimeOperations
{
    long? ActiveBatchId { get; }
    void PushActiveBatch(long batchId);
    bool TryPopActiveBatch(out long batchId);
    void AppendCompletedBatch(CardMovementBatchContext batch);
}

// The one movement-batch owner. Legacy native producers temporarily retain
// aliases to these exact collections while their metadata algorithms migrate.
internal sealed class CardMovementBatchStore : ICardMovementBatchQueue, ICardMovementBatchNativeDrawInvoices,
    IGameStartingMovementBatchOrigins, ICardMovementEventSuppression,
    ICardMovementBatchLifecycle
{
    internal List<CardMovementBatchContext> PendingBatches { get; } = [];
    internal Stack<long> ActiveBatchIds { get; } = new();
    internal Stack<long> SuppressedEventBatchIds { get; } = new();

    public int PendingBatchCount => PendingBatches.Count;
    public CardMovementBatchContext PendingBatchAt(int index) => PendingBatches[index];
    public void AssignNativeDrawInvoice(int pendingIndex, NativeDrawInvocationProof proof) =>
        PendingBatches[pendingIndex] = PendingBatches[pendingIndex] with { NativeDrawInvocation = proof };
    public void AssignGameStartingMovementOrigin(int pendingIndex, GameStartingNativeMovementOrigin origin) =>
        PendingBatches[pendingIndex] = PendingBatches[pendingIndex] with { GameStartingMovementOrigin = origin };
    public void RemovePendingBatch(CardMovementBatchContext batch) => PendingBatches.Remove(batch);
    public bool HasSuppressedCardMovedEvents => SuppressedEventBatchIds.Count != 0;
    public void Suppress(long batchId) => SuppressedEventBatchIds.Push(batchId);
    public bool TryPopSuppression(out long batchId) => SuppressedEventBatchIds.TryPop(out batchId);
    public long? ActiveBatchId => ActiveBatchIds.TryPeek(out var batchId) ? batchId : null;
    public void PushActiveBatch(long batchId) => ActiveBatchIds.Push(batchId);
    public bool TryPopActiveBatch(out long batchId) => ActiveBatchIds.TryPop(out batchId);
    public void AppendCompletedBatch(CardMovementBatchContext batch) => PendingBatches.Add(batch);
}

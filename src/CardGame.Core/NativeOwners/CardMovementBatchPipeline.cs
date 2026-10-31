#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class CardMovementBatchPipeline(Func<ProgramPipelineHooks> getProgramPipelineHooks) : ICardMovementBatchPipeline
    {
        public void Begin(CardMovementBatchBuilder batch) => getProgramPipelineHooks().BeginMovementBatch(batch);
        public void ObserveCommitted(CardMovementBatchBuilder batch, CardMovementRecord movement) =>
            getProgramPipelineHooks().ObserveCommittedMovement(batch, movement);
        public void CaptureFacts(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) =>
            getProgramPipelineHooks().CaptureMovementBatchFacts(batch, movements);
        public CardMovementBatchContext Complete(CardMovementBatchBuilder batch, CardMovementBatchContext completed) =>
            getProgramPipelineHooks().CompleteMovementBatch(batch, completed);
    }

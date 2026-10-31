#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAllHandTrickPaymentsEntries
{
    internal void CaptureProgramAllHandTrickPayment(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureProgramAllHandTrickPayment(batch);
    internal bool HasProgramAllHandTrickPaymentProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().HasProgramAllHandTrickPaymentProcessing(processing);
    internal bool HasProgramAllHandTrickPaymentTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().HasProgramAllHandTrickPaymentTail(use, continuation);
    internal bool IsProgramAllHandTrickPaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsProgramAllHandTrickPaymentBatch(use, batch);
    internal bool ProgramAllHandTrickMaterialsMatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ProgramAllHandTrickMaterialsMatch(use, batch);
    internal global::CardGame.Core.CardConversionSource? ProgramAllHandTrickSource(global::CardGame.Core.CardUseFrame use) => _resolve().ProgramAllHandTrickSource(use);
}

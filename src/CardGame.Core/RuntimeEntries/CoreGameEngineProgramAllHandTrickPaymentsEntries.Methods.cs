#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAllHandTrickPaymentsEntries
{
    internal void CaptureProgramAllHandTrickPayment(CardMovementBatchContext batch) => _resolve().CaptureProgramAllHandTrickPayment(batch);
    internal bool HasProgramAllHandTrickPaymentProcessing(IReadOnlyList<Card> processing) => _resolve().HasProgramAllHandTrickPaymentProcessing(processing);
    internal bool HasProgramAllHandTrickPaymentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().HasProgramAllHandTrickPaymentTail(use, continuation);
    internal bool IsProgramAllHandTrickPaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsProgramAllHandTrickPaymentBatch(use, batch);
    internal bool ProgramAllHandTrickMaterialsMatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().ProgramAllHandTrickMaterialsMatch(use, batch);
    internal CardConversionSource? ProgramAllHandTrickSource(CardUseFrame use) => _resolve().ProgramAllHandTrickSource(use);
}

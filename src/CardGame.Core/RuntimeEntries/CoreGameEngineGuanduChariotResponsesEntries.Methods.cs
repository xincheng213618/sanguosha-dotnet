#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduChariotResponsesEntries
{
    internal void CaptureGuanduChariotResponsePayments(CardActionContext action, long parentFrameId, ProgramCardContinuation continuation) => _resolve().CaptureGuanduChariotResponsePayments(action, parentFrameId, continuation);
    internal CardMovementRecord? GuanduChariotResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation? continuation = default) => _resolve().GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
    internal bool HasGuanduChariotResponsePayment(CardActionContext action, ProgramCardContinuation continuation) => _resolve().HasGuanduChariotResponsePayment(action, continuation);
    internal bool IsExactGuanduChariotDestruction(CardActionCost cost, CardMovementRecord move) => _resolve().IsExactGuanduChariotDestruction(cost, move);
}

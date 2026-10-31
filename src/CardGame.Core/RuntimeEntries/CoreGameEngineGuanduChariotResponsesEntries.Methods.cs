#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduChariotResponsesEntries
{
    internal void CaptureGuanduChariotResponsePayments(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().CaptureGuanduChariotResponsePayments(action, parentFrameId, continuation);
    internal global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default) => _resolve().GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
    internal bool HasGuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasGuanduChariotResponsePayment(action, continuation);
    internal bool IsExactGuanduChariotDestruction(global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord move) => _resolve().IsExactGuanduChariotDestruction(cost, move);
}

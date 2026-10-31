#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeUsedDiscardOriginsEntries
{
    internal void CaptureClaimedDeclarationTrueUse(global::CardGame.Core.DeclaredCardPayment paid) => _resolve().CaptureClaimedDeclarationTrueUse(paid);
    internal void CaptureNativeTrueUseProcessingEntries(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureNativeTrueUseProcessingEntries(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureNativeTrueUseProcessingEntries(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNativeTrueUseProcessingEntries(batch, completed);
    internal bool IsNativeTrueUseDiscardMovement(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardMovementRecord move) => _resolve().IsNativeTrueUseDiscardMovement(batch, move);
}

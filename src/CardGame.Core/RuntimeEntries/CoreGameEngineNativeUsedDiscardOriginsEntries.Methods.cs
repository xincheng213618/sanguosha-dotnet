#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeUsedDiscardOriginsEntries
{
    internal void CaptureClaimedDeclarationTrueUse(DeclaredCardPayment paid) => _resolve().CaptureClaimedDeclarationTrueUse(paid);
    internal void CaptureNativeTrueUseProcessingEntries(CardMovementBatchContext batch) => _resolve().CaptureNativeTrueUseProcessingEntries(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureNativeTrueUseProcessingEntries(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNativeTrueUseProcessingEntries(batch, completed);
    internal bool IsNativeTrueUseDiscardMovement(CardMovementBatchContext batch, CardMovementRecord move) => _resolve().IsNativeTrueUseDiscardMovement(batch, move);
}

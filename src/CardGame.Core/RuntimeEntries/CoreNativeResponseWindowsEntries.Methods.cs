#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeResponseWindowsEntries
{
    internal void PopResponseWindow(long parentFrameId) => _resolve().PopResponseWindow(parentFrameId);
    internal void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind) => _resolve().PushResponseWindow(parentFrameId, sourceSeat, responderSeat, incomingCard, requiredCardKind);
    internal void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => _resolve().SetResponseParentStep(frameId, step);
}

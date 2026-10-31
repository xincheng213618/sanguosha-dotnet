#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeResponseWindowsEntries
{
    internal void PopResponseWindow(long parentFrameId) => _resolve().PopResponseWindow(parentFrameId);
    internal void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, CardKind incomingCard, CardKind requiredCardKind) => _resolve().PushResponseWindow(parentFrameId, sourceSeat, responderSeat, incomingCard, requiredCardKind);
    internal void SetResponseParentStep(long frameId, ResolutionFrameStep step) => _resolve().SetResponseParentStep(frameId, step);
}

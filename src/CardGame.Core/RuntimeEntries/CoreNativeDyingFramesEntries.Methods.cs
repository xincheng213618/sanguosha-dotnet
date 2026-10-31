#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDyingFramesEntries
{
    internal void SetDyingFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => _resolve().SetDyingFrameStep(frameId, step);
    internal global::CardGame.Core.DyingFrame UpdateDyingFrame(long frameId, global::System.Func<global::CardGame.Core.DyingFrame, global::CardGame.Core.DyingFrame> update) => _resolve().UpdateDyingFrame(frameId, update);
}

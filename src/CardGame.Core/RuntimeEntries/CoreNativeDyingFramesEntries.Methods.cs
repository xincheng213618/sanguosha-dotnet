#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDyingFramesEntries
{
    internal void SetDyingFrameStep(long frameId, ResolutionFrameStep step) => _resolve().SetDyingFrameStep(frameId, step);
    internal DyingFrame UpdateDyingFrame(long frameId, Func<DyingFrame, DyingFrame> update) => _resolve().UpdateDyingFrame(frameId, update);
}

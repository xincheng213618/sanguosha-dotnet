#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardUseFramesEntries
{
    internal void RedirectCardUseTarget(long frameId, int originalTargetSeat, int redirectedTargetSeat) => _resolve().RedirectCardUseTarget(frameId, originalTargetSeat, redirectedTargetSeat);
    internal void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => _resolve().SetCardUseStep(frameId, step);
}

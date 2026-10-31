#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseGiftBasicEntries
{
    internal void AccumulatePaidPhaseGift(ProgramSkillFrame frame, string resultBind, int threshold) => _resolve().AccumulatePaidPhaseGift(frame, resultBind, threshold);
    internal void CapturePaidPhaseGift(long frameId, SkillProgramActivation activation, bool toDiscard) => _resolve().CapturePaidPhaseGift(frameId, activation, toDiscard);
}

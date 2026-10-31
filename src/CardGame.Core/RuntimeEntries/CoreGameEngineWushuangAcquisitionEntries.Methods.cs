#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAcquisitionEntries
{
    internal bool AllowsWushuangAcquiredBinding(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangAcquiredBinding(gate);
    internal void AssertWushuangAcquisitionOwner(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertWushuangAcquisitionOwner(frame, plan);
    internal bool ReturnWushuangAcquisitionBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().ReturnWushuangAcquisitionBinding(frame, completed);
    internal bool TryBeginWushuangAcquisition(long windowId, global::CardGame.Core.WushuangShopOffer offer) => _resolve().TryBeginWushuangAcquisition(windowId, offer);
}

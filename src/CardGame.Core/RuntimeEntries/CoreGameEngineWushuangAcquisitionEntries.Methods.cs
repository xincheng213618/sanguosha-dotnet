#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAcquisitionEntries
{
    internal bool AllowsWushuangAcquiredBinding(ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangAcquiredBinding(gate);
    internal bool ReturnWushuangAcquisitionBinding(ProgramSkillFrame frame, bool completed) => _resolve().ReturnWushuangAcquisitionBinding(frame, completed);
    internal bool TryBeginWushuangAcquisition(long windowId, WushuangShopOffer offer) => _resolve().TryBeginWushuangAcquisition(windowId, offer);
}

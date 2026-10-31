#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMobileEntries
{
    internal void AssertNativeDiscardEntry(NativeDiscardEntryFrame frame) => _resolve().AssertNativeDiscardEntry(frame);
    internal bool AssertPreEntryDiscardPolicyChoice(PendingDecision pending) => _resolve().AssertPreEntryDiscardPolicyChoice(pending);
    internal void ContinueNativeDiscardEntry(long frameId) => _resolve().ContinueNativeDiscardEntry(frameId);
    internal PromptChoice DiscardEntryPolicyChoice(NativeDiscardEntryFrame frame, bool apply) => _resolve().DiscardEntryPolicyChoice(frame, apply);
    internal bool TryBeginPreEntryDiscardPolicy(CharacterState current) => _resolve().TryBeginPreEntryDiscardPolicy(current);
    internal bool TryResolvePreEntryDiscardPolicy(bool useSkill) => _resolve().TryResolvePreEntryDiscardPolicy(useSkill);
}

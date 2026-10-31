#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMobileEntries
{
    internal void AssertNativeDiscardEntry(global::CardGame.Core.NativeDiscardEntryFrame frame) => _resolve().AssertNativeDiscardEntry(frame);
    internal bool AssertPreEntryDiscardPolicyChoice(global::CardGame.Core.PendingDecision pending) => _resolve().AssertPreEntryDiscardPolicyChoice(pending);
    internal void ContinueNativeDiscardEntry(long frameId) => _resolve().ContinueNativeDiscardEntry(frameId);
    internal global::CardGame.Core.PromptChoice DiscardEntryPolicyChoice(global::CardGame.Core.NativeDiscardEntryFrame frame, bool apply) => _resolve().DiscardEntryPolicyChoice(frame, apply);
    internal bool TryBeginPreEntryDiscardPolicy(global::CardGame.Core.CharacterState current) => _resolve().TryBeginPreEntryDiscardPolicy(current);
    internal bool TryResolvePreEntryDiscardPolicy(bool useSkill) => _resolve().TryResolvePreEntryDiscardPolicy(useSkill);
}

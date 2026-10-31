#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeIceSwordEntries
{
    internal void AppendIceSwordLogicalDiscard(NativeOwners.IceSwordHandle pending, Card material) => _resolve().AppendIceSwordLogicalDiscard(pending, material);
    internal void AssertIceSwordLogicalDiscard(NativeOwners.IceSwordHandle pending) => _resolve().AssertIceSwordLogicalDiscard(pending);
    internal void FinishIceSwordPrevention(NativeOwners.IceSwordHandle pending) => _resolve().FinishIceSwordPrevention(pending);
    internal int GetIceSwordTargetCardCount(CharacterState target, int actorSeat) => _resolve().GetIceSwordTargetCardCount(target, actorSeat);
    internal void PublishIceSwordChoice(NativeOwners.IceSwordHandle pending) => _resolve().PublishIceSwordChoice(pending);
    internal void ResolveIceSwordChoice(PromptChoice selected) => _resolve().ResolveIceSwordChoice(selected);
    internal bool TryBeginIceSwordChoice(NativeOwners.CardAttackHandle attack, int preventedDamageAmount) => _resolve().TryBeginIceSwordChoice(attack, preventedDamageAmount);
}

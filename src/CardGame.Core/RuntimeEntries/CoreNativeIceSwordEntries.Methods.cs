#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeIceSwordEntries
{
    internal void AppendIceSwordLogicalDiscard(global::CardGame.Core.NativeOwners.IceSwordHandle pending, global::CardGame.Core.Card material) => _resolve().AppendIceSwordLogicalDiscard(pending, material);
    internal void AssertIceSwordLogicalDiscard(global::CardGame.Core.NativeOwners.IceSwordHandle pending) => _resolve().AssertIceSwordLogicalDiscard(pending);
    internal void FinishIceSwordPrevention(global::CardGame.Core.NativeOwners.IceSwordHandle pending) => _resolve().FinishIceSwordPrevention(pending);
    internal int GetIceSwordTargetCardCount(global::CardGame.Core.CharacterState target, int actorSeat) => _resolve().GetIceSwordTargetCardCount(target, actorSeat);
    internal void PublishIceSwordChoice(global::CardGame.Core.NativeOwners.IceSwordHandle pending) => _resolve().PublishIceSwordChoice(pending);
    internal void ResolveIceSwordChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveIceSwordChoice(selected);
    internal bool TryBeginIceSwordChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount) => _resolve().TryBeginIceSwordChoice(attack, preventedDamageAmount);
}

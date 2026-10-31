#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeQilinBowEntries
{
    internal IReadOnlyList<Card> GetQilinBowTargetMounts(CharacterState target) => _resolve().GetQilinBowTargetMounts(target);
    internal void PublishQilinBowChoice(NativeOwners.QilinBowHandle pending) => _resolve().PublishQilinBowChoice(pending);
    internal void ResolveQilinBowChoice(PromptChoice selected) => _resolve().ResolveQilinBowChoice(selected);
    internal bool TryBeginQilinBowChoice(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginQilinBowChoice(attack);
}

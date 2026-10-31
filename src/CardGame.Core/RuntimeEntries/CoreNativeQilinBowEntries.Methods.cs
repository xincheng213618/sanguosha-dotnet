#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeQilinBowEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetQilinBowTargetMounts(global::CardGame.Core.CharacterState target) => _resolve().GetQilinBowTargetMounts(target);
    internal void PublishQilinBowChoice(global::CardGame.Core.NativeOwners.QilinBowHandle pending) => _resolve().PublishQilinBowChoice(pending);
    internal void ResolveQilinBowChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveQilinBowChoice(selected);
    internal bool TryBeginQilinBowChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginQilinBowChoice(attack);
}

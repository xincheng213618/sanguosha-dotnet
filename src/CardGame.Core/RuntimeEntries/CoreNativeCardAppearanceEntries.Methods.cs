#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardAppearanceEntries
{
    internal Suit EffectiveSuit(CharacterState owner, Card card) => _resolve().EffectiveSuit(owner, card);
}

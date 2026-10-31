#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQuYiClassicEntries
{
    internal int ClassicJiaoziPublicDamageBonus(GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards) => _resolve().ClassicJiaoziPublicDamageBonus(view, sourceSeat, targetSeat, sourceLess, paidHandCards);
}

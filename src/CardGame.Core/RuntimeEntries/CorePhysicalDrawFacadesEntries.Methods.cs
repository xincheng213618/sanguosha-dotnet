#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalDrawFacadesEntries
{
    internal IReadOnlyList<int> DrawCards(CharacterState player, int count, bool log, CardMoveReason? reason = default, CardConversionSource? directProducer = default) => _resolve().DrawCards(player, count, log, reason, directProducer);
    internal Card? DrawOne(CharacterState player, CardMoveReason reason) => _resolve().DrawOne(player, reason);
    internal Card? DrawOneToProcessing(CardMoveReason reason) => _resolve().DrawOneToProcessing(reason);
    internal bool EnsureDrawPile() => _resolve().EnsureDrawPile();
}

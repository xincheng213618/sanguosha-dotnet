#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalDrawFacadesEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => _resolve().DrawCards(player, count, log, reason, directProducer);
    internal global::CardGame.Core.Card? DrawOne(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardMoveReason reason) => _resolve().DrawOne(player, reason);
    internal global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason) => _resolve().DrawOneToProcessing(reason);
    internal bool EnsureDrawPile() => _resolve().EnsureDrawPile();
}

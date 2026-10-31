#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePhysicalDrawFacadesRuntime
{
    internal IReadOnlyList<int> DrawCards(CharacterState player, int count, bool log, CardMoveReason? reason = null, CardConversionSource? directProducer = null) => _corePhysicalCardDrawOperations.PhysicalCardDraws.DrawCards(player, count, log, reason, directProducer);
    internal Card? DrawOne(CharacterState player, CardMoveReason reason) => _corePhysicalCardDrawOperations.PhysicalCardDraws.DrawOne(player, reason);
    internal Card? DrawOneToProcessing(CardMoveReason reason) => _corePhysicalCardDrawOperations.PhysicalCardDraws.DrawOneToProcessing(reason);
    internal bool EnsureDrawPile() => _corePhysicalCardDrawOperations.PhysicalCardDraws.EnsureDrawPile();
}

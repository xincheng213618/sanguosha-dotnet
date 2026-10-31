#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePhysicalDrawFacadesRuntime
{
    internal IReadOnlyList<int> DrawCards(CharacterState player, int count, bool log, CardMoveReason? reason = null, CardConversionSource? directProducer = null) => _corePhysicalCardDrawOperations.Value.PhysicalCardDraws.DrawCards(player, count, log, reason, directProducer);
    internal Card? DrawOne(CharacterState player, CardMoveReason reason) => _corePhysicalCardDrawOperations.Value.PhysicalCardDraws.DrawOne(player, reason);
    internal Card? DrawOneToProcessing(CardMoveReason reason) => _corePhysicalCardDrawOperations.Value.PhysicalCardDraws.DrawOneToProcessing(reason);
    internal bool EnsureDrawPile() => _corePhysicalCardDrawOperations.Value.PhysicalCardDraws.EnsureDrawPile();
}

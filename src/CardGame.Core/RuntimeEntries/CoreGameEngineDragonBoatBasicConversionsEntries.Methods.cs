#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatBasicConversionsEntries
{
    internal void AddDragonBoatBasicConversionActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddDragonBoatBasicConversionActions(actions, actor, playable, selectedPhysicalCardId);
    internal void AddDragonBoatZeroBasicActions(List<LegalAction> actions, CharacterState actor, CardConversionSource source, CardKind kind) => _resolve().AddDragonBoatZeroBasicActions(actions, actor, source, kind);
}

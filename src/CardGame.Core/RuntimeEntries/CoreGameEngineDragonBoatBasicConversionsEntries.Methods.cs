#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatBasicConversionsEntries
{
    internal void AddDragonBoatBasicConversionActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddDragonBoatBasicConversionActions(actions, actor, playable, selectedPhysicalCardId);
    internal void AddDragonBoatZeroBasicActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => _resolve().AddDragonBoatZeroBasicActions(actions, actor, source, kind);
}

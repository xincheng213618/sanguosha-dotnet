#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduBasicConversionsEntries
{
    internal void AddGuanduBasicConversionActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddGuanduBasicConversionActions(actions, actor, playable, selectedPhysicalCardId);
    internal void AddGuanduZeroBasicActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source) => _resolve().AddGuanduZeroBasicActions(actions, actor, source);
}

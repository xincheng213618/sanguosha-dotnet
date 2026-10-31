#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduBasicConversionsEntries
{
    internal void AddGuanduBasicConversionActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddGuanduBasicConversionActions(actions, actor, playable, selectedPhysicalCardId);
    internal void AddGuanduZeroBasicActions(List<LegalAction> actions, CharacterState actor, CardConversionSource source) => _resolve().AddGuanduZeroBasicActions(actions, actor, source);
}

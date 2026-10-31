#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnexpectedAssaultActionsEntries
{
    internal void AddUnexpectedAssaultActions(CharacterState actor, IReadOnlyList<Card> playableCards, List<LegalAction> actions) => _resolve().AddUnexpectedAssaultActions(actor, playableCards, actions);
}

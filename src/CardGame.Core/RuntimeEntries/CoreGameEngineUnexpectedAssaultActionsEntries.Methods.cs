#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnexpectedAssaultActionsEntries
{
    internal void AddUnexpectedAssaultActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playableCards, global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions) => _resolve().AddUnexpectedAssaultActions(actor, playableCards, actions);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeActionDispatchEntries
{
    internal void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default) => _resolve().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

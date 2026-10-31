#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeActionDispatchEntries
{
    internal void ExecuteAction(CharacterState actor, LegalAction action, IReadOnlyList<int>? activeSkillCardIds = default, IReadOnlyList<int>? activeSkillTargetSeats = default) => _resolve().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFriendDuelResponsesEntries
{
    internal void ContinueFriendDuelResponse(long id) => _resolve().ContinueFriendDuelResponse(id);
    internal IReadOnlyList<PromptChoice> FriendDuelResponseChoices(NativeOwners.DuelHandle duel, CharacterState responder) => _resolve().FriendDuelResponseChoices(duel, responder);
    internal bool IsFriendDuelResponseMovementReturn(FriendDuelResponseFrame parent, CardsMovedTriggerWindowFrame movement) => _resolve().IsFriendDuelResponseMovementReturn(parent, movement);
    internal bool IsFriendResponseParent(FriendDuelResponseFrame f) => _resolve().IsFriendResponseParent(f);
    internal void ResolveFriendDuelDiscard(PromptChoice supplied) => _resolve().ResolveFriendDuelDiscard(supplied);
}

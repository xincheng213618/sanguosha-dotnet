#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFriendDuelResponsesEntries
{
    internal void AssertFriendEventFrames() => _resolve().AssertFriendEventFrames();
    internal void ContinueFriendDuelResponse(long id) => _resolve().ContinueFriendDuelResponse(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendDuelResponseChoices(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder) => _resolve().FriendDuelResponseChoices(duel, responder);
    internal bool HasFriendDuelResponseObserver(long useId) => _resolve().HasFriendDuelResponseObserver(useId);
    internal bool IsFriendDuelResponseMovementReturn(global::CardGame.Core.FriendDuelResponseFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => _resolve().IsFriendDuelResponseMovementReturn(parent, movement);
    internal bool IsFriendResponseParent(global::CardGame.Core.FriendDuelResponseFrame f) => _resolve().IsFriendResponseParent(f);
    internal void ResolveFriendDuelDiscard(global::CardGame.Core.PromptChoice supplied) => _resolve().ResolveFriendDuelDiscard(supplied);
}

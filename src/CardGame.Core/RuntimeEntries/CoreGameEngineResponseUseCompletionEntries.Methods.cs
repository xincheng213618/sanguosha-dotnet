#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseUseCompletionEntries
{
    internal void ContinueCompletedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().ContinueCompletedResponseUse(attack, frame);
    internal void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action) => _resolve().ContinueNullificationAfterResponseUse(action);
    internal global::CardGame.Core.Suit? FreezeCompletedResponseUseSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => _resolve().FreezeCompletedResponseUseSuit(owner, cards, kind);
    internal bool HasResponseUseCompletionObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasResponseUseCompletionObserver(action, continuation);
    internal bool IsCompletedResponseParentConsistent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().IsCompletedResponseParentConsistent(window);
    internal bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().IsCompletedResponseUse(action, continuation);
    internal bool IsCompletedResponseUse2(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind) => _resolve().IsCompletedResponseUse(action, kind);
    internal bool TryBeginCompletedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().TryBeginCompletedResponseUsePrograms(attack, action, continuation);
}

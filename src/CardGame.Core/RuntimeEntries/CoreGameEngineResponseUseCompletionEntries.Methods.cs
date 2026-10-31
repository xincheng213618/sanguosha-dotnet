#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseUseCompletionEntries
{
    internal void ContinueCompletedResponseUse(NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame frame) => _resolve().ContinueCompletedResponseUse(attack, frame);
    internal void ContinueNullificationAfterResponseUse(CardActionContext action) => _resolve().ContinueNullificationAfterResponseUse(action);
    internal Suit? FreezeCompletedResponseUseSuit(CharacterState owner, IReadOnlyList<Card> cards, CardKind kind) => _resolve().FreezeCompletedResponseUseSuit(owner, cards, kind);
    internal bool HasResponseUseCompletionObserver(CardActionContext action, ProgramCardContinuation continuation) => _resolve().HasResponseUseCompletionObserver(action, continuation);
    internal bool IsCompletedResponseParentConsistent(ProgramCardTriggerWindowFrame window) => _resolve().IsCompletedResponseParentConsistent(window);
    internal bool IsCompletedResponseUse(CardActionContext action, ProgramCardContinuation continuation) => _resolve().IsCompletedResponseUse(action, continuation);
    internal bool IsCompletedResponseUse2(CardActionContext action, ProgramCompletedResponseKind kind) => _resolve().IsCompletedResponseUse(action, kind);
    internal bool TryBeginCompletedResponseUsePrograms(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().TryBeginCompletedResponseUsePrograms(attack, action, continuation);
}

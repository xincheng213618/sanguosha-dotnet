#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCommittedResponseUsesEntries
{
    internal void ContinueCommittedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().ContinueCommittedResponseUse(attack, frame);
    internal bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().TryBeginCommittedResponseUsePrograms(attack, action, continuation);
}

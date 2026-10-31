#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCommittedResponseUsesEntries
{
    internal void ContinueCommittedResponseUse(NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame frame) => _resolve().ContinueCommittedResponseUse(attack, frame);
    internal bool TryBeginCommittedResponseUsePrograms(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().TryBeginCommittedResponseUsePrograms(attack, action, continuation);
}

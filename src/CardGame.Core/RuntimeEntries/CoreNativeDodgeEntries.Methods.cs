#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDodgeEntries
{
    internal void CompleteFactionDefenseBaguaResponse(NativeOwners.CardAttackHandle attack, bool succeeded) => _resolve().CompleteFactionDefenseBaguaResponse(attack, succeeded);
    internal void CompleteSuccessfulDodgeResponse(NativeOwners.CardAttackHandle attack) => _resolve().CompleteSuccessfulDodgeResponse(attack);
    internal void ContinueAcceptedCardResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().ContinueAcceptedCardResponse(attack, action, continuation);
    internal void ContinueFinishedCardResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().ContinueFinishedCardResponse(attack, action, continuation);
    internal void ContinueSlashAfterDodgeCancellation(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterDodgeCancellation(attack);
    internal void ContinueSlashAfterDodgePrograms(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterDodgePrograms(attack);
    internal void FinishSuccessfulDodgeResponse(NativeOwners.CardAttackHandle attack) => _resolve().FinishSuccessfulDodgeResponse(attack);
    internal void PublishOwnerDodgeDecision(NativeOwners.CardAttackHandle attack) => _resolve().PublishOwnerDodgeDecision(attack);
    internal void ResolveDodgeResponse(NativeOwners.CardAttackHandle attack, CharacterState defender, Card responseCard) => _resolve().ResolveDodgeResponse(attack, defender, responseCard);
}

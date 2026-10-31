#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDodgeEntries
{
    internal void CompleteFactionDefenseBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded) => _resolve().CompleteFactionDefenseBaguaResponse(attack, succeeded);
    internal void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CompleteSuccessfulDodgeResponse(attack);
    internal void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().ContinueAcceptedCardResponse(attack, action, continuation);
    internal void ContinueFinishedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().ContinueFinishedCardResponse(attack, action, continuation);
    internal void ContinueSlashAfterDodgeCancellation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterDodgeCancellation(attack);
    internal void ContinueSlashAfterDodgePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueSlashAfterDodgePrograms(attack);
    internal void FinishSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().FinishSuccessfulDodgeResponse(attack);
    internal void PublishOwnerDodgeDecision(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().PublishOwnerDodgeDecision(attack);
    internal void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard) => _resolve().ResolveDodgeResponse(attack, defender, responseCard);
}

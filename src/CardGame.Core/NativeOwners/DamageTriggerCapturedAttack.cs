#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageTriggerCapturedAttack(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.NativeOwners.DamageTriggerCompletionFactory createDamageTriggerCompletion, global::CardGame.Core.GameEngine.IDamageAttempt attack) : IDamageTriggerCapturedAttack
{
    public IDamageTriggerCompletion BindTarget() => createDamageTriggerCompletion(attack, matchParticipantsState.ReferenceToPlayers[attack.TargetSeat]);
}

#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageTriggerCapturedAttack(EngineStates.CoreMatchParticipantsState matchParticipantsState, DamageTriggerCompletionFactory createDamageTriggerCompletion, GameEngine.IDamageAttempt attack) : IDamageTriggerCapturedAttack
{
    public IDamageTriggerCompletion BindTarget() => createDamageTriggerCompletion(attack, matchParticipantsState.ReferenceToPlayers[attack.TargetSeat]);
}

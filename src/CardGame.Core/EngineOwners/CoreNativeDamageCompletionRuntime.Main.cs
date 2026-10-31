#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamageCompletionRuntime
{
    internal void CompleteDamageAttack(GameEngine.IDamageAttempt attack)
    {
        if (attack is NativeOwners.CardAttackHandle card)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(card);
            return;
        }

        _coreProgramAttackAttemptOperations.CompleteProgramAttack(attack);
    }
}

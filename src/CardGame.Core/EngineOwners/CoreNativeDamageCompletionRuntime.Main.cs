#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamageCompletionRuntime
{
    internal void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack is global::CardGame.Core.NativeOwners.CardAttackHandle card)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(card);
            return;
        }

        _coreProgramAttackAttemptOperations.CompleteProgramAttack(attack);
    }
}

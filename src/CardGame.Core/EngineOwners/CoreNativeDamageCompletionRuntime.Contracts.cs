#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDamageCompletionCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeDamageCompletionCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteProgramAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

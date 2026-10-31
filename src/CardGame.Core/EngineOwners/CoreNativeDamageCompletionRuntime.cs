#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageCompletionRuntime
{
    private readonly ICoreNativeDamageCompletionCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreNativeDamageCompletionCoreNativeAttackCompletionOperations _coreNativeAttackCompletionOperations;
    internal CoreNativeDamageCompletionRuntime(ICoreNativeDamageCompletionCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreNativeDamageCompletionCoreNativeAttackCompletionOperations coreNativeAttackCompletionOperations)
    {
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
    }
}

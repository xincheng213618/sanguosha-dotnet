#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageCompletionRuntime
{
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    internal CoreNativeDamageCompletionRuntime(RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations)
    {
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
    }
}

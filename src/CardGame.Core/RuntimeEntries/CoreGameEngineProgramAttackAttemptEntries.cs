#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed partial class CoreGameEngineProgramAttackAttemptEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _resolve;
    private CoreGameEngineProgramAttackAttemptEntries(Func<EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAttackAttemptEntries Create(Func<EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> resolve) => new(resolve);
}

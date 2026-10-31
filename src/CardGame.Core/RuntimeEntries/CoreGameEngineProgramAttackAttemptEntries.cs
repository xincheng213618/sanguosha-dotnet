#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed partial class CoreGameEngineProgramAttackAttemptEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _resolve;
    private CoreGameEngineProgramAttackAttemptEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAttackAttemptEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime))]
internal sealed partial class CoreGameEngineProgramPersistentCombatPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> _resolve;
    private CoreGameEngineProgramPersistentCombatPoliciesEntries(Func<EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPersistentCombatPoliciesEntries Create(Func<EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> resolve) => new(resolve);
}

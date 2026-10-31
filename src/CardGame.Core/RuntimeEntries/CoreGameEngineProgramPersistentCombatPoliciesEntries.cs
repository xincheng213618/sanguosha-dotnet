#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime))]
internal sealed partial class CoreGameEngineProgramPersistentCombatPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> _resolve;
    private CoreGameEngineProgramPersistentCombatPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPersistentCombatPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> resolve) => new(resolve);
}

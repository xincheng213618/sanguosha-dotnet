#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime))]
internal sealed partial class CoreGameEngineProgramFactionPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> _resolve;
    private CoreGameEngineProgramFactionPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramFactionPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> resolve) => new(resolve);
}

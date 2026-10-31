#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime))]
internal sealed partial class CoreGameEngineProgramFactionPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> _resolve;
    private CoreGameEngineProgramFactionPoliciesEntries(Func<EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramFactionPoliciesEntries Create(Func<EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> resolve) => new(resolve);
}

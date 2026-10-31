#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianPoliciesRuntime))]
internal sealed partial class CoreGameEnginePindianPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianPoliciesRuntime> _resolve;
    private CoreGameEnginePindianPoliciesEntries(Func<EngineOwners.CoreGameEnginePindianPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianPoliciesEntries Create(Func<EngineOwners.CoreGameEnginePindianPoliciesRuntime> resolve) => new(resolve);
}

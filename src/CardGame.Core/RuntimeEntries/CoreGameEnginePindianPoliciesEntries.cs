#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime))]
internal sealed partial class CoreGameEnginePindianPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> _resolve;
    private CoreGameEnginePindianPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> resolve) => new(resolve);
}

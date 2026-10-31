#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> _resolve;
    private CoreGameEngineAdvancedPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> resolve) => new(resolve);
}

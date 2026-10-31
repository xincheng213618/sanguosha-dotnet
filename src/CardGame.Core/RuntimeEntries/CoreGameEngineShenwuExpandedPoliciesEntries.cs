#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuExpandedPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> _resolve;
    private CoreGameEngineShenwuExpandedPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuExpandedPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> resolve) => new(resolve);
}

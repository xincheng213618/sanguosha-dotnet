#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShownEntityTurnPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _resolve;
    private CoreGameEngineShownEntityTurnPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShownEntityTurnPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> resolve) => new(resolve);
}

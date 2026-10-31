#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpOrderedDistancePoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpOrderedDistancePoliciesRuntime> _resolve;
    private CoreGameEngineHpOrderedDistancePoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpOrderedDistancePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpOrderedDistancePoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpOrderedDistancePoliciesRuntime> resolve) => new(resolve);
}

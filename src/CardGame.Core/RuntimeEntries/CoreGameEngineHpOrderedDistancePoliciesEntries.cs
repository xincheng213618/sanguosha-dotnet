#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpOrderedDistancePoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHpOrderedDistancePoliciesRuntime> _resolve;
    private CoreGameEngineHpOrderedDistancePoliciesEntries(Func<EngineOwners.CoreGameEngineHpOrderedDistancePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpOrderedDistancePoliciesEntries Create(Func<EngineOwners.CoreGameEngineHpOrderedDistancePoliciesRuntime> resolve) => new(resolve);
}

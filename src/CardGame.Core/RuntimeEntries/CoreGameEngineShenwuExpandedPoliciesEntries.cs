#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime))]
internal sealed partial class CoreGameEngineShenwuExpandedPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> _resolve;
    private CoreGameEngineShenwuExpandedPoliciesEntries(Func<EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuExpandedPoliciesEntries Create(Func<EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> resolve) => new(resolve);
}

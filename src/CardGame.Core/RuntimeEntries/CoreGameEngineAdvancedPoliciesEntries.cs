#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAdvancedPoliciesRuntime))]
internal sealed partial class CoreGameEngineAdvancedPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> _resolve;
    private CoreGameEngineAdvancedPoliciesEntries(Func<EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedPoliciesEntries Create(Func<EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> resolve) => new(resolve);
}

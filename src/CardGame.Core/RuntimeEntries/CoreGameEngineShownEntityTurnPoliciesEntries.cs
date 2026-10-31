#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime))]
internal sealed partial class CoreGameEngineShownEntityTurnPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _resolve;
    private CoreGameEngineShownEntityTurnPoliciesEntries(Func<EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShownEntityTurnPoliciesEntries Create(Func<EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> resolve) => new(resolve);
}

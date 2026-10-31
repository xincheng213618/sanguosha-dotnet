#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime))]
internal sealed partial class CoreGameEnginePassiveRulePoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> _resolve;
    private CoreGameEnginePassiveRulePoliciesEntries(Func<EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePassiveRulePoliciesEntries Create(Func<EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> resolve) => new(resolve);
}

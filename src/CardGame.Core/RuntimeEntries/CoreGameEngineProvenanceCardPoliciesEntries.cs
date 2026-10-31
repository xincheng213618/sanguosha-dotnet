#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed partial class CoreGameEngineProvenanceCardPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _resolve;
    private CoreGameEngineProvenanceCardPoliciesEntries(Func<EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceCardPoliciesEntries Create(Func<EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> resolve) => new(resolve);
}

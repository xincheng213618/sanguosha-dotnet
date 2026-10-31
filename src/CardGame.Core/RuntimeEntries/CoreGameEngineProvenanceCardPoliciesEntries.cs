#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed partial class CoreGameEngineProvenanceCardPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _resolve;
    private CoreGameEngineProvenanceCardPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceCardPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> resolve) => new(resolve);
}

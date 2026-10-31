#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed partial class CoreGameEngineIssuedPlayUsePoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _resolve;
    private CoreGameEngineIssuedPlayUsePoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIssuedPlayUsePoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> resolve) => new(resolve);
}

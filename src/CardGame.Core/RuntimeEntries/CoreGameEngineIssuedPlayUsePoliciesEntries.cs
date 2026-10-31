#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed partial class CoreGameEngineIssuedPlayUsePoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _resolve;
    private CoreGameEngineIssuedPlayUsePoliciesEntries(Func<EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIssuedPlayUsePoliciesEntries Create(Func<EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> resolve) => new(resolve);
}

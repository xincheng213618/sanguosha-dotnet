#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed partial class CoreGameEngineAlcoholIdentityPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _resolve;
    private CoreGameEngineAlcoholIdentityPoliciesEntries(Func<EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlcoholIdentityPoliciesEntries Create(Func<EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> resolve) => new(resolve);
}

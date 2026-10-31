#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed partial class CoreGameEngineAlcoholIdentityPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _resolve;
    private CoreGameEngineAlcoholIdentityPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlcoholIdentityPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> resolve) => new(resolve);
}

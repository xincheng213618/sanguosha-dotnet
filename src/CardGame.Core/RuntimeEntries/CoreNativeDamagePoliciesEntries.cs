#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed partial class CoreNativeDamagePoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _resolve;
    private CoreNativeDamagePoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamagePoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> resolve) => new(resolve);
}

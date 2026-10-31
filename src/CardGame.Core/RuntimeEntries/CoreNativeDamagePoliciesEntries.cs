#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed partial class CoreNativeDamagePoliciesEntries
{
    private readonly Func<EngineOwners.CoreNativeDamagePoliciesRuntime> _resolve;
    private CoreNativeDamagePoliciesEntries(Func<EngineOwners.CoreNativeDamagePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamagePoliciesEntries Create(Func<EngineOwners.CoreNativeDamagePoliciesRuntime> resolve) => new(resolve);
}

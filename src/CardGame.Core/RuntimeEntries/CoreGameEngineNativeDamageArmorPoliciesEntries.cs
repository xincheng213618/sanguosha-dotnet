#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime))]
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> _resolve;
    private CoreGameEngineNativeDamageArmorPoliciesEntries(Func<EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDamageArmorPoliciesEntries Create(Func<EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> resolve) => new(resolve);
}

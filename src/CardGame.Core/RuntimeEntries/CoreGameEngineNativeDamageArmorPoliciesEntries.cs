#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime))]
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> _resolve;
    private CoreGameEngineNativeDamageArmorPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDamageArmorPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> resolve) => new(resolve);
}

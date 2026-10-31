#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> _resolve;
    private CoreGameEngineNativeDamageArmorPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDamageArmorPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDamageArmorPoliciesRuntime> resolve) => new(resolve);
}

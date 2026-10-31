#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed partial class CoreForeignEquipmentDiscardPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _resolve;
    private CoreForeignEquipmentDiscardPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreForeignEquipmentDiscardPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> resolve) => new(resolve);
}

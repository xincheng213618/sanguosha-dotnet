#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed partial class CoreForeignEquipmentDiscardPoliciesEntries
{
    private readonly Func<EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _resolve;
    private CoreForeignEquipmentDiscardPoliciesEntries(Func<EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreForeignEquipmentDiscardPoliciesEntries Create(Func<EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> resolve) => new(resolve);
}

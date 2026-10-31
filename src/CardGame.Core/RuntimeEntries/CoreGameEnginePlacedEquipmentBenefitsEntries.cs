#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime))]
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime> _resolve;
    private CoreGameEnginePlacedEquipmentBenefitsEntries(Func<EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlacedEquipmentBenefitsEntries Create(Func<EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime))]
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> _resolve;
    private CoreGameEnginePlacedEquipmentBenefitObserversEntries(Func<EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlacedEquipmentBenefitObserversEntries Create(Func<EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> resolve) => new(resolve);
}

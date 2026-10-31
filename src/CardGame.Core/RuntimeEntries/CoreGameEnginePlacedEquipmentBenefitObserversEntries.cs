#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime))]
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> _resolve;
    private CoreGameEnginePlacedEquipmentBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlacedEquipmentBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> resolve) => new(resolve);
}

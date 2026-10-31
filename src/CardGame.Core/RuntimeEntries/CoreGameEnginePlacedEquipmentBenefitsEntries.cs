#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime> _resolve;
    private CoreGameEnginePlacedEquipmentBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlacedEquipmentBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime> resolve) => new(resolve);
}

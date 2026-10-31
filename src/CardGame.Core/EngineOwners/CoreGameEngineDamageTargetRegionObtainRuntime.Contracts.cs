#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDamageTargetRegionObtainCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

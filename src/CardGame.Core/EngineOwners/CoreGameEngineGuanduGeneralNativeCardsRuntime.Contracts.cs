#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreGuanduGeneralNativeCardsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

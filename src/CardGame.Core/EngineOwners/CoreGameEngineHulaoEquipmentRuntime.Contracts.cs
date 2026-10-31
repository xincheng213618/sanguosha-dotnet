#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHulaoEquipmentCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

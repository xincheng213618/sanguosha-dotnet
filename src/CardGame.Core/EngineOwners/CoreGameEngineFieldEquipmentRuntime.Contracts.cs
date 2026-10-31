#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreFieldEquipmentCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
